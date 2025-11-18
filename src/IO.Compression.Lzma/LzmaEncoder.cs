// -----------------------------------------------------------------------
// <copyright file="LzmaEncoder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

using static System.IO.Compression.Constants;

/// <summary>
/// The LZMA encoder.
/// </summary>
public class LzmaEncoder
{
    private const uint InfinityPrice = uint.MaxValue;
    private const int DefaultDictionarySize = 22;
    private const uint FastBytesDefault = 0x20U;
    private const uint OptimalCount = 1U << 12;

    private static readonly byte[] FastPos = CreatePosSlots();

    private static readonly string[] MatchFinderIDs =
    [
        "BT2",
        "BT4",
    ];

    private readonly uint[] repDistances = new uint[RegisteredDistances];

    private readonly Optimal[] optimum = new Optimal[OptimalCount];
    private readonly RangeCoder.RangeEncoder rangeEncoder = new();

    private readonly RangeCoder.BitEncoder[] matchEncoders = new RangeCoder.BitEncoder[States << PositionStatesBitsMaximum];
    private readonly RangeCoder.BitEncoder[] repEncoders = new RangeCoder.BitEncoder[States];
    private readonly RangeCoder.BitEncoder[] repG0Encoders = new RangeCoder.BitEncoder[States];
    private readonly RangeCoder.BitEncoder[] repG1Encoders = new RangeCoder.BitEncoder[States];
    private readonly RangeCoder.BitEncoder[] repG2Encoders = new RangeCoder.BitEncoder[States];
    private readonly RangeCoder.BitEncoder[] rep0LongEncoders = new RangeCoder.BitEncoder[States << PositionStatesBitsMaximum];

    private readonly RangeCoder.BitTreeEncoder[] posSlotEncoder = new RangeCoder.BitTreeEncoder[LengthToPositionStates];

    private readonly RangeCoder.BitEncoder[] posEncoders = new RangeCoder.BitEncoder[FullDistances - EndPositionModelIndex];
    private readonly LengthPriceTableEncoder lengthEncoder = new();
    private readonly LengthPriceTableEncoder repMatchLengthEncoder = new();

    private readonly uint[] matchDistances = new uint[(MatchMaximumLength * 2) + 2];

    private readonly RangeCoder.BitTreeEncoder posAlignEncoder = new(AlignBits);

    private readonly uint[] posSlotPrices = new uint[1 << (PositionSlotBits + LengthToPositionStatesBits)];
    private readonly uint[] distancesPrices = new uint[FullDistances << LengthToPositionStatesBits];
    private readonly uint[] alignPrices = new uint[AlignTableSize];

    private readonly uint[] reps = new uint[RegisteredDistances];
    private readonly uint[] repLens = new uint[RegisteredDistances];

    private readonly uint[] tempPrices = new uint[FullDistances];

    private readonly uint fastBytes = FastBytesDefault;

    private readonly uint distributionTableSize = DefaultDictionarySize * 2;

    private readonly int positionStateBits = 2;
    private readonly uint positionStateMask = 4U - 1U;
    private readonly int literalPositionStateBits;
    private readonly int literalContextBits = 3;

    private readonly EMatchFinderType matchFinderType = EMatchFinderType.Bt4;

    private readonly uint dictionarySize = 1U << DefaultDictionarySize;

    private LiteralEncoder literalEncoder = new(0, 0);

    private uint matchPriceCount;

#pragma warning disable S3459
    private State state;
#pragma warning restore S3459
    private byte previousByte;

    private LZ.BinaryTree? matchFinder;

    private uint previousDictionarySize = uint.MaxValue;
    private uint previousFastBytes = uint.MaxValue;

    private uint longestMatchLength;
    private uint distancePairs;

    private uint additionalOffset;

    private uint optimumEndIndex;
    private uint optimumCurrentIndex;

    private bool longestMatchWasFound;

    private uint alignPriceCount;

    private long currentPosition;
    private bool finished;
    private Stream? stream;

    private bool writeEndMark;

    private bool releaseMatchFinderStream;

    /// <summary>
    /// Initializes a new instance of the <see cref="LzmaEncoder"/> class.
    /// </summary>
    /// <param name="properties">The properties.</param>
    public LzmaEncoder(IDictionary<CoderPropId, object> properties)
    {
        for (var i = 0; i < OptimalCount; i++)
        {
            this.optimum[i] = new();
        }

        for (var i = 0; i < LengthToPositionStates; i++)
        {
            this.posSlotEncoder[i] = new(PositionSlotBits);
        }

        foreach (var kvp in properties)
        {
            const int DictionaryLogSizeMaximumCompress = 30;
            var prop = kvp.Value;
            switch (kvp.Key)
            {
                case CoderPropId.FastBytes when prop is int fastBytesProperty and >= 5 and <= (int)MatchMaximumLength:
                    this.fastBytes = (uint)fastBytesProperty;
                    break;
                case CoderPropId.Algorithm:
                    break;
                case CoderPropId.MatchFinder when prop is string stringProperty && FindMatchFinder(stringProperty) is >= 0 and var match:
                    var matchFinderIndexPrevious = this.matchFinderType;
                    this.matchFinderType = (EMatchFinderType)match;
                    if (this.matchFinder is not null && matchFinderIndexPrevious != this.matchFinderType)
                    {
                        this.previousDictionarySize = uint.MaxValue;
                        this.matchFinder = null;
                    }

                    break;
                case CoderPropId.DictionarySize when prop is int dictionarySizeProperty and >= (int)(1U << DictionaryMinimumSize) and <= (int)(1U << DictionaryLogSizeMaximumCompress):
                    this.dictionarySize = (uint)dictionarySizeProperty;
                    int size;
                    for (size = 0; size < DictionaryLogSizeMaximumCompress; size++)
                    {
                        if (dictionarySizeProperty <= (1U << size))
                        {
                            break;
                        }
                    }

                    this.distributionTableSize = (uint)size * 2;
                    break;
                case CoderPropId.PositionStateBits when prop is int positionStateBitsProperty and >= 0 and <= PositionStatesBitsEncodingMaximum:
                    this.positionStateBits = positionStateBitsProperty;
                    this.positionStateMask = (1U << this.positionStateBits) - 1;
                    break;
                case CoderPropId.LiteralPositionBits when prop is int literalPositionStateBitsProperty and >= 0 and <= (int)LiteralPositionStatesBitsEncodingMaximum:
                    this.literalPositionStateBits = literalPositionStateBitsProperty;
                    break;
                case CoderPropId.LiteralContextBits when prop is int literalContextBitsProperty and >= 0 and <= (int)LiteralContextBitsMaximum:
                    this.literalContextBits = literalContextBitsProperty;
                    break;
                case CoderPropId.EndMarker when prop is bool endMarkerProperty:
                    this.SetWriteEndMarkerMode(endMarkerProperty);
                    break;
                default:
                    throw new InvalidDataException();
            }

            static int FindMatchFinder(string s)
            {
                for (var m = 0; m < MatchFinderIDs.Length; m++)
                {
                    if (string.Equals(s, MatchFinderIDs[m], StringComparison.OrdinalIgnoreCase))
                    {
                        return m;
                    }
                }

                return -1;
            }
        }
    }

    private enum EMatchFinderType
    {
        Bt2,
        Bt4,
    }

    /// <summary>
    /// Compresses the input stream to the output.
    /// </summary>
    /// <param name="input">The input stream.</param>
    /// <param name="output">The output stream.</param>
    /// <param name="progress">The progress.</param>
    public void Compress(Stream input, Stream output, Action<long, long>? progress = null)
    {
        this.releaseMatchFinderStream = false;
        progress ??= static (_, _) => { };
        try
        {
            this.stream = input;
            this.finished = false;

            if (this.matchFinder is null)
            {
                var numHashBytes = this.matchFinderType switch
                {
                    EMatchFinderType.Bt2 => 2,
                    _ => 4,
                };

                this.matchFinder = new(numHashBytes);
            }

            this.literalEncoder = new(this.literalPositionStateBits, this.literalContextBits);

            if (this.dictionarySize == this.previousDictionarySize && this.previousFastBytes == this.fastBytes)
            {
                return;
            }

            this.matchFinder.Create(this.dictionarySize, OptimalCount, this.fastBytes, MatchMaximumLength + 1);
            this.previousDictionarySize = this.dictionarySize;
            this.previousFastBytes = this.fastBytes;

            this.previousByte = 0;
            for (var i = 0U; i < RegisteredDistances; i++)
            {
                this.repDistances[i] = 0;
            }

            this.rangeEncoder.Init(output);

            for (var i = 0U; i < States; i++)
            {
                for (var j = 0U; j <= this.positionStateMask; j++)
                {
                    var complexState = (i << PositionStatesBitsMaximum) + j;
                    this.matchEncoders[complexState].Init();
                    this.rep0LongEncoders[complexState].Init();
                }

                this.repEncoders[i].Init();
                this.repG0Encoders[i].Init();
                this.repG1Encoders[i].Init();
                this.repG2Encoders[i].Init();
            }

            this.literalEncoder.Init();
            for (var i = 0U; i < LengthToPositionStates; i++)
            {
                this.posSlotEncoder[i].Init();
            }

            for (var i = 0U; i < FullDistances - EndPositionModelIndex; i++)
            {
                this.posEncoders[i].Init();
            }

            this.lengthEncoder.Init(1U << this.positionStateBits);
            this.repMatchLengthEncoder.Init(1U << this.positionStateBits);

            this.posAlignEncoder.Init();

            this.longestMatchWasFound = false;
            this.optimumEndIndex = 0;
            this.optimumCurrentIndex = 0;
            this.additionalOffset = 0;

            this.FillDistancesPrices();
            this.FillAlignPrices();

            this.lengthEncoder.SetTableSize(this.fastBytes + 1 - MatchMinimumLength);
            this.lengthEncoder.UpdateTables(1U << this.positionStateBits);
            this.repMatchLengthEncoder.SetTableSize(this.fastBytes + 1 - MatchMinimumLength);
            this.repMatchLengthEncoder.UpdateTables(1U << this.positionStateBits);

            this.currentPosition = 0;
            while (true)
            {
                if (this.EncodeOneBlock(out var processedInputSize, out var processedOutputSize))
                {
                    return;
                }

                progress.Invoke(processedInputSize, processedOutputSize);
            }
        }
        finally
        {
            this.ReleaseMatchFinderStream();
            this.rangeEncoder.ReleaseStream();
        }
    }

    /// <summary>
    /// Encodes one block.
    /// </summary>
    /// <param name="inSize">The input size.</param>
    /// <param name="outSize">The output size.</param>
    /// <returns>Whether this is finished.</returns>
    public bool EncodeOneBlock(out long inSize, out long outSize)
    {
        if (this.matchFinder is null)
        {
            throw new InvalidOperationException();
        }

        inSize = default;
        outSize = default;

        if (this.stream is not null)
        {
            this.matchFinder.Init(this.stream);
            this.releaseMatchFinderStream = true;
            this.stream = null;
        }

        if (this.finished)
        {
            return true;
        }

        this.finished = true;

        var progressPosValuePrev = this.currentPosition;
        if (this.currentPosition is 0L)
        {
            if (this.matchFinder.AvailableBytes is 0U)
            {
                this.Flush((uint)this.currentPosition);
                return true;
            }

            this.ReadMatchDistances(out _, out _);
            var posState = (uint)this.currentPosition & this.positionStateMask;
            this.matchEncoders[(this.state.Index << PositionStatesBitsMaximum) + posState].Encode(this.rangeEncoder, 0);
            this.state.UpdateChar();
            var curByte = this.matchFinder.GetIndexByte((int)(0 - this.additionalOffset));
            this.literalEncoder.GetSubCoder((uint)this.currentPosition, this.previousByte).Encode(this.rangeEncoder, curByte);
            this.previousByte = curByte;
            this.additionalOffset--;
            this.currentPosition++;
        }

        if (this.matchFinder.AvailableBytes is 0U)
        {
            this.Flush((uint)this.currentPosition);
            return true;
        }

        while (true)
        {
            var length = this.GetOptimum((uint)this.currentPosition, out var position);

            var posState = ((uint)this.currentPosition) & this.positionStateMask;
            var complexState = (this.state.Index << PositionStatesBitsMaximum) + posState;
            if (length is 1 && position is uint.MaxValue)
            {
                this.matchEncoders[complexState].Encode(this.rangeEncoder, 0);
                var currentByte = this.matchFinder.GetIndexByte((int)(0 - this.additionalOffset));
                if (this.literalEncoder?.GetSubCoder((uint)this.currentPosition, this.previousByte) is { } subCoder)
                {
                    if (this.state.IsCharState())
                    {
                        subCoder.Encode(this.rangeEncoder, currentByte);
                    }
                    else
                    {
                        subCoder.EncodeMatched(this.rangeEncoder, this.matchFinder.GetIndexByte((int)(0 - this.repDistances[0] - 1 - this.additionalOffset)), currentByte);
                    }
                }

                this.previousByte = currentByte;
                this.state.UpdateChar();
            }
            else
            {
                this.matchEncoders[complexState].Encode(this.rangeEncoder, 1);
                if (position < RegisteredDistances)
                {
                    this.repEncoders[this.state.Index].Encode(this.rangeEncoder, 1);
                    if (position is 0U)
                    {
                        this.repG0Encoders[this.state.Index].Encode(this.rangeEncoder, 0);
                        this.rep0LongEncoders[complexState].Encode(this.rangeEncoder, length is 1U ? 0U : 1U);
                    }
                    else
                    {
                        this.repG0Encoders[this.state.Index].Encode(this.rangeEncoder, 1);
                        if (position is 1U)
                        {
                            this.repG1Encoders[this.state.Index].Encode(this.rangeEncoder, 0);
                        }
                        else
                        {
                            this.repG1Encoders[this.state.Index].Encode(this.rangeEncoder, 1);
                            this.repG2Encoders[this.state.Index].Encode(this.rangeEncoder, position - 2);
                        }
                    }

                    if (length is 1U)
                    {
                        this.state.UpdateShortRep();
                    }
                    else
                    {
                        this.repMatchLengthEncoder.Encode(this.rangeEncoder, length - MatchMinimumLength, posState);
                        this.state.UpdateRep();
                    }

                    var distance = this.repDistances[position];
                    if (position is not 0U)
                    {
                        for (var i = position; i >= 1; i--)
                        {
                            this.repDistances[i] = this.repDistances[i - 1];
                        }

                        this.repDistances[0] = distance;
                    }
                }
                else
                {
                    this.repEncoders[this.state.Index].Encode(this.rangeEncoder, 0);
                    this.state.UpdateMatch();
                    this.lengthEncoder.Encode(this.rangeEncoder, length - MatchMinimumLength, posState);
                    position -= RegisteredDistances;
                    var posSlot = GetPosSlot(position);
                    var lenToPosState = GetLenToPosState(length);
                    this.posSlotEncoder[lenToPosState].Encode(this.rangeEncoder, posSlot);

                    if (posSlot >= StartPositionModelIndex)
                    {
                        var footerBits = (int)((posSlot >> 1) - 1);
                        var baseVal = (2 | (posSlot & 1)) << footerBits;
                        var posReduced = position - baseVal;

                        if (posSlot < EndPositionModelIndex)
                        {
                            RangeCoder.BitTreeEncoder.ReverseEncode(
                                this.posEncoders,
                                baseVal - posSlot - 1,
                                this.rangeEncoder,
                                footerBits,
                                posReduced);
                        }
                        else
                        {
                            this.rangeEncoder.EncodeDirectBits(posReduced >> AlignBits, footerBits - AlignBits);
                            this.posAlignEncoder.ReverseEncode(this.rangeEncoder, posReduced & AlignMask);
                            this.alignPriceCount++;
                        }
                    }

                    var distance = position;
                    for (var i = RegisteredDistances - 1; i >= 1; i--)
                    {
                        this.repDistances[i] = this.repDistances[i - 1];
                    }

                    this.repDistances[0] = distance;
                    this.matchPriceCount++;
                }

                this.previousByte = this.matchFinder.GetIndexByte((int)(length - 1 - this.additionalOffset));
            }

            this.additionalOffset -= length;
            this.currentPosition += length;
            if (this.additionalOffset is not 0U)
            {
                continue;
            }

            if (this.matchPriceCount >= (1U << 7))
            {
                this.FillDistancesPrices();
            }

            if (this.alignPriceCount >= AlignTableSize)
            {
                this.FillAlignPrices();
            }

            inSize = this.currentPosition;
            outSize = this.rangeEncoder.GetProcessedSizeAdd();
            if (this.matchFinder.AvailableBytes is 0U)
            {
                this.Flush((uint)this.currentPosition);
                return true;
            }

            if (this.currentPosition - progressPosValuePrev < (1 << 12))
            {
                continue;
            }

            this.finished = false;
            return false;
        }
    }

    /// <summary>
    /// Writes the coder properties.
    /// </summary>
    /// <param name="output">The stream to write to.</param>
    public void WriteCoderProperties(Stream output)
    {
        byte[] properties =
        [
            (byte)((((this.positionStateBits * 5) + this.literalPositionStateBits) * 9) + this.literalContextBits),
            (byte)((this.dictionarySize >> 0) & byte.MaxValue),
            (byte)((this.dictionarySize >> 8) & byte.MaxValue),
            (byte)((this.dictionarySize >> 16) & byte.MaxValue),
            (byte)((this.dictionarySize >> 24) & byte.MaxValue),
        ];

        output.Write(properties, 0, properties.Length);
    }

    private static byte[] CreatePosSlots()
    {
        const byte Start = 2;
        const byte FastSlots = 22;
        var c = 2;
        var fastPos = new byte[1 << 11];
        fastPos[0] = 0;
        fastPos[1] = 1;
        for (var slotFast = Start; slotFast < FastSlots; slotFast++)
        {
            var k = 1U << ((slotFast >> 1) - 1);
            for (var j = 0U; j < k; j++, c++)
            {
                fastPos[c] = slotFast;
            }
        }

        return fastPos;
    }

    private static uint GetPosSlot(uint pos) => pos switch
    {
        < 1U << 11 => FastPos[pos],
        < 1U << 21 => FastPos[pos >> 10] + 20U,
        _ => FastPos[pos >> 20] + 40U,
    };

    private void SetWriteEndMarkerMode(bool writeEndMarker) => this.writeEndMark = writeEndMarker;

    private void ReadMatchDistances(out uint lenRes, out uint numDistancePairs)
    {
        if (this.matchFinder is null)
        {
            throw new InvalidOperationException();
        }

        lenRes = 0;
        numDistancePairs = this.matchFinder.GetMatches(this.matchDistances);
        if (numDistancePairs > 1)
        {
            lenRes = this.matchDistances[numDistancePairs - 2];
            if (lenRes == this.fastBytes)
            {
                lenRes += this.matchFinder.GetMatchLength(
                    (int)lenRes - 1,
                    this.matchDistances[numDistancePairs - 1],
                    MatchMaximumLength - lenRes);
            }
        }

        this.additionalOffset++;
    }

    private void MovePos(uint num)
    {
        if (num > 0 && this.matchFinder is not null)
        {
            this.matchFinder.Skip(num);
            this.additionalOffset += num;
        }
    }

    private uint GetRepLen1Price(State currentState, uint positionState) => this.repG0Encoders[currentState.Index].GetPrice0() + this.rep0LongEncoders[(currentState.Index << PositionStatesBitsMaximum) + positionState].GetPrice0();

    private uint GetPureRepPrice(uint index, State currentState, uint positionState)
    {
        uint price;
        if (index is 0U)
        {
            price = this.repG0Encoders[currentState.Index].GetPrice0();
            price += this.rep0LongEncoders[(currentState.Index << PositionStatesBitsMaximum) + positionState].GetPrice1();
        }
        else
        {
            price = this.repG0Encoders[currentState.Index].GetPrice1();
            if (index is 1U)
            {
                price += this.repG1Encoders[currentState.Index].GetPrice0();
            }
            else
            {
                price += this.repG1Encoders[currentState.Index].GetPrice1();
                price += this.repG2Encoders[currentState.Index].GetPrice(index - 2);
            }
        }

        return price;
    }

    private uint GetRepPrice(uint index, uint length, State currentState, uint positionState)
    {
        var price = this.repMatchLengthEncoder.GetPrice(length - MatchMinimumLength, positionState);
        return price + this.GetPureRepPrice(index, currentState, positionState);
    }

    private uint GetPosLenPrice(uint position, uint length, uint positionState)
    {
        var lenToPosState = GetLenToPosState(length);
        var price = position < FullDistances
            ? this.distancesPrices[(lenToPosState * FullDistances) + position]
            : this.posSlotPrices[(lenToPosState << PositionSlotBits) + GetPosSlot2(position)] + this.alignPrices[position & AlignMask];

        return price + this.lengthEncoder.GetPrice(length - MatchMinimumLength, positionState);

        static uint GetPosSlot2(uint pos)
        {
            return pos switch
            {
                < 1 << 17 => FastPos[pos >> 6] + 12U,
                < 1 << 27 => FastPos[pos >> 16] + 32U,
                _ => FastPos[pos >> 26] + 52U,
            };
        }
    }

    private uint Backward(out uint backRes, uint cur)
    {
        this.optimumEndIndex = cur;
        var posMem = this.optimum[cur].PosPrev;
        var backMem = this.optimum[cur].BackPrev;
        do
        {
            if (this.optimum[cur].Prev1IsChar)
            {
                this.optimum[posMem].MakeAsChar();
                this.optimum[posMem].PosPrev = posMem - 1;
                if (this.optimum[cur].Prev2)
                {
                    this.optimum[posMem - 1].Prev1IsChar = false;
                    this.optimum[posMem - 1].PosPrev = this.optimum[cur].PosPrev2;
                    this.optimum[posMem - 1].BackPrev = this.optimum[cur].BackPrev2;
                }
            }

            var posPrev = posMem;
            var backCur = backMem;

            backMem = this.optimum[posPrev].BackPrev;
            posMem = this.optimum[posPrev].PosPrev;

            this.optimum[posPrev].BackPrev = backCur;
            this.optimum[posPrev].PosPrev = cur;
            cur = posPrev;
        }
        while (cur > 0);
        backRes = this.optimum[0].BackPrev;
        this.optimumCurrentIndex = this.optimum[0].PosPrev;
        return this.optimumCurrentIndex;
    }

    private uint GetOptimum(uint position, out uint backRes)
    {
        if (this.optimumEndIndex != this.optimumCurrentIndex)
        {
            var lenRes = this.optimum[this.optimumCurrentIndex].PosPrev - this.optimumCurrentIndex;
            backRes = this.optimum[this.optimumCurrentIndex].BackPrev;
            this.optimumCurrentIndex = this.optimum[this.optimumCurrentIndex].PosPrev;
            return lenRes;
        }

        this.optimumCurrentIndex = this.optimumEndIndex = 0;

        uint lenMain;
        uint currentNumDistancePairs;
        if (!this.longestMatchWasFound)
        {
            this.ReadMatchDistances(out lenMain, out currentNumDistancePairs);
        }
        else
        {
            lenMain = this.longestMatchLength;
            currentNumDistancePairs = this.distancePairs;
            this.longestMatchWasFound = false;
        }

        if (this.matchFinder is null)
        {
            throw new InvalidOperationException();
        }

        var numAvailableBytes = this.matchFinder.AvailableBytes + 1;
        if (numAvailableBytes < 2)
        {
            backRes = uint.MaxValue;
            return 1;
        }

        var repMaxIndex = 0U;
        for (var i = 0U; i < RegisteredDistances; i++)
        {
            this.reps[i] = this.repDistances[i];
            this.repLens[i] = this.matchFinder.GetMatchLength(0 - 1, this.reps[i], MatchMaximumLength);
            if (this.repLens[i] > this.repLens[repMaxIndex])
            {
                repMaxIndex = i;
            }
        }

        if (this.repLens[repMaxIndex] >= this.fastBytes)
        {
            backRes = repMaxIndex;
            var lenRes = this.repLens[repMaxIndex];
            this.MovePos(lenRes - 1);
            return lenRes;
        }

        if (lenMain >= this.fastBytes)
        {
            backRes = this.matchDistances[currentNumDistancePairs - 1] + RegisteredDistances;
            this.MovePos(lenMain - 1);
            return lenMain;
        }

        var currentByte = this.matchFinder.GetIndexByte(0 - 1);
        var matchByte = this.matchFinder.GetIndexByte((int)(0 - this.repDistances[0] - 1 - 1));

        if (lenMain < 2U && currentByte != matchByte && this.repLens[repMaxIndex] < 2U)
        {
            backRes = uint.MaxValue;
            return 1U;
        }

        this.optimum[0].State = this.state;

        var posState = position & this.positionStateMask;

        this.optimum[1].Price = this.matchEncoders[(this.state.Index << PositionStatesBitsMaximum)
            + posState].GetPrice0()
            + this.literalEncoder.GetSubCoder(position, this.previousByte).GetPrice(!this.state.IsCharState(), matchByte, currentByte);
        this.optimum[1].MakeAsChar();

        var matchPrice = this.matchEncoders[(this.state.Index << PositionStatesBitsMaximum) + posState].GetPrice1();
        var repMatchPrice = matchPrice + this.repEncoders[this.state.Index].GetPrice1();

        if (matchByte == currentByte)
        {
            var shortRepPrice = repMatchPrice + this.GetRepLen1Price(this.state, posState);
            if (shortRepPrice < this.optimum[1].Price)
            {
                this.optimum[1].Price = shortRepPrice;
                this.optimum[1].MakeAsShortRep();
            }
        }

        var lenEnd = (lenMain >= this.repLens[repMaxIndex]) ? lenMain : this.repLens[repMaxIndex];

        if (lenEnd < 2)
        {
            backRes = this.optimum[1].BackPrev;
            return 1;
        }

        this.optimum[1].PosPrev = 0;

        this.optimum[0].Backs0 = this.reps[0];
        this.optimum[0].Backs1 = this.reps[1];
        this.optimum[0].Backs2 = this.reps[2];
        this.optimum[0].Backs3 = this.reps[3];

        var len = lenEnd;
        do
        {
            this.optimum[len--].Price = InfinityPrice;
        }
        while (len >= 2);

        for (var i = 0U; i < RegisteredDistances; i++)
        {
            var repLen = this.repLens[i];
            if (repLen < 2)
            {
                continue;
            }

            var price = repMatchPrice + this.GetPureRepPrice(i, this.state, posState);
            do
            {
                var curAndLenPrice = price + this.repMatchLengthEncoder.GetPrice(repLen - 2, posState);
                var currentOptimum = this.optimum[repLen];
                if (curAndLenPrice < currentOptimum.Price)
                {
                    currentOptimum.Price = curAndLenPrice;
                    currentOptimum.PosPrev = 0;
                    currentOptimum.BackPrev = i;
                    currentOptimum.Prev1IsChar = false;
                }
            }
            while (--repLen >= 2);
        }

        var normalMatchPrice = matchPrice + this.repEncoders[this.state.Index].GetPrice0();

        len = (this.repLens[0] >= 2) ? this.repLens[0] + 1 : 2;
        if (len <= lenMain)
        {
            var offs = 0U;
            while (len > this.matchDistances[offs])
            {
                offs += 2;
            }

#pragma warning disable S1994 // "for" loop increment clauses should modify the loops' counters
            for (; ; len++)
            {
                var distance = this.matchDistances[offs + 1];
                var curAndLenPrice = normalMatchPrice + this.GetPosLenPrice(distance, len, posState);
                var currentOptimum = this.optimum[len];
                if (curAndLenPrice < currentOptimum.Price)
                {
                    currentOptimum.Price = curAndLenPrice;
                    currentOptimum.PosPrev = 0;
                    currentOptimum.BackPrev = distance + RegisteredDistances;
                    currentOptimum.Prev1IsChar = false;
                }

                if (len == this.matchDistances[offs])
                {
                    offs += 2;
                    if (offs == currentNumDistancePairs)
                    {
                        break;
                    }
                }
            }
#pragma warning restore S1994 // "for" loop increment clauses should modify the loops' counters
        }

        var cur = 0U;

        while (true)
        {
            cur++;
            if (cur == lenEnd)
            {
                return this.Backward(out backRes, cur);
            }

            this.ReadMatchDistances(out var newLen, out currentNumDistancePairs);
            if (newLen >= this.fastBytes)
            {
                this.distancePairs = currentNumDistancePairs;
                this.longestMatchLength = newLen;
                this.longestMatchWasFound = true;
                return this.Backward(out backRes, cur);
            }

            position++;
            var posPrev = this.optimum[cur].PosPrev;
            State optimumState;
            if (this.optimum[cur].Prev1IsChar)
            {
                posPrev--;
                if (this.optimum[cur].Prev2)
                {
                    optimumState = this.optimum[this.optimum[cur].PosPrev2].State;
                    if (this.optimum[cur].BackPrev2 < RegisteredDistances)
                    {
                        optimumState.UpdateRep();
                    }
                    else
                    {
                        optimumState.UpdateMatch();
                    }
                }
                else
                {
                    optimumState = this.optimum[posPrev].State;
                }

                optimumState.UpdateChar();
            }
            else
            {
                optimumState = this.optimum[posPrev].State;
            }

            if (posPrev == cur - 1)
            {
                if (this.optimum[cur].IsShortRep())
                {
                    optimumState.UpdateShortRep();
                }
                else
                {
                    optimumState.UpdateChar();
                }
            }
            else
            {
                uint pos;
                if (this.optimum[cur].Prev1IsChar && this.optimum[cur].Prev2)
                {
                    posPrev = this.optimum[cur].PosPrev2;
                    pos = this.optimum[cur].BackPrev2;
                    optimumState.UpdateRep();
                }
                else
                {
                    pos = this.optimum[cur].BackPrev;
                    if (pos < RegisteredDistances)
                    {
                        optimumState.UpdateRep();
                    }
                    else
                    {
                        optimumState.UpdateMatch();
                    }
                }

                var opt = this.optimum[posPrev];
                if (pos < RegisteredDistances)
                {
                    if (pos is 0U)
                    {
                        this.reps[0] = opt.Backs0;
                        this.reps[1] = opt.Backs1;
                        this.reps[2] = opt.Backs2;
                        this.reps[3] = opt.Backs3;
                    }
                    else if (pos is 1U)
                    {
                        this.reps[0] = opt.Backs1;
                        this.reps[1] = opt.Backs0;
                        this.reps[2] = opt.Backs2;
                        this.reps[3] = opt.Backs3;
                    }
                    else if (pos is 2U)
                    {
                        this.reps[0] = opt.Backs2;
                        this.reps[1] = opt.Backs0;
                        this.reps[2] = opt.Backs1;
                        this.reps[3] = opt.Backs3;
                    }
                    else
                    {
                        this.reps[0] = opt.Backs3;
                        this.reps[1] = opt.Backs0;
                        this.reps[2] = opt.Backs1;
                        this.reps[3] = opt.Backs2;
                    }
                }
                else
                {
                    this.reps[0] = pos - RegisteredDistances;
                    this.reps[1] = opt.Backs0;
                    this.reps[2] = opt.Backs1;
                    this.reps[3] = opt.Backs2;
                }
            }

            this.optimum[cur].State = optimumState;
            this.optimum[cur].Backs0 = this.reps[0];
            this.optimum[cur].Backs1 = this.reps[1];
            this.optimum[cur].Backs2 = this.reps[2];
            this.optimum[cur].Backs3 = this.reps[3];
            var curPrice = this.optimum[cur].Price;

            currentByte = this.matchFinder.GetIndexByte(0 - 1);
            matchByte = this.matchFinder.GetIndexByte((int)(0 - this.reps[0] - 1 - 1));

            posState = position & this.positionStateMask;

            var curAnd1Price = curPrice +
                this.matchEncoders[(optimumState.Index << PositionStatesBitsMaximum) + posState].GetPrice0() +
                this.literalEncoder.GetSubCoder(position, this.matchFinder.GetIndexByte(0 - 2)).
                GetPrice(!optimumState.IsCharState(), matchByte, currentByte);

            var nextOptimum = this.optimum[cur + 1];

            var nextIsChar = false;
            if (curAnd1Price < nextOptimum.Price)
            {
                nextOptimum.Price = curAnd1Price;
                nextOptimum.PosPrev = cur;
                nextOptimum.MakeAsChar();
                nextIsChar = true;
            }

            matchPrice = curPrice + this.matchEncoders[(optimumState.Index << PositionStatesBitsMaximum) + posState].GetPrice1();
            repMatchPrice = matchPrice + this.repEncoders[optimumState.Index].GetPrice1();

            if (matchByte == currentByte &&
                !(nextOptimum.PosPrev < cur && nextOptimum.BackPrev is 0U))
            {
                var shortRepPrice = repMatchPrice + this.GetRepLen1Price(optimumState, posState);
                if (shortRepPrice <= nextOptimum.Price)
                {
                    nextOptimum.Price = shortRepPrice;
                    nextOptimum.PosPrev = cur;
                    nextOptimum.MakeAsShortRep();
                    nextIsChar = true;
                }
            }

            var numAvailableBytesFull = this.matchFinder.AvailableBytes + 1;
            numAvailableBytesFull = Math.Min(OptimalCount - 1 - cur, numAvailableBytesFull);
            numAvailableBytes = numAvailableBytesFull;

            if (numAvailableBytes < 2)
            {
                continue;
            }

            if (numAvailableBytes > this.fastBytes)
            {
                numAvailableBytes = this.fastBytes;
            }

            if (!nextIsChar && matchByte != currentByte)
            {
                // try Literal + rep0
                var t = Math.Min(numAvailableBytesFull - 1, this.fastBytes);
                var lenTest2 = this.matchFinder.GetMatchLength(0, this.reps[0], t);
                if (lenTest2 >= 2)
                {
                    var state2 = optimumState;
                    state2.UpdateChar();
                    var posStateNext = (position + 1) & this.positionStateMask;
                    var nextRepMatchPrice = curAnd1Price +
                        this.matchEncoders[(state2.Index << PositionStatesBitsMaximum) + posStateNext].GetPrice1() +
                        this.repEncoders[state2.Index].GetPrice1();

                    var offset = cur + 1 + lenTest2;
                    while (lenEnd < offset)
                    {
                        this.optimum[++lenEnd].Price = InfinityPrice;
                    }

                    var curAndLenPrice = nextRepMatchPrice + this.GetRepPrice(0, lenTest2, state2, posStateNext);
                    var currentOptimum = this.optimum[offset];
                    if (curAndLenPrice < currentOptimum.Price)
                    {
                        currentOptimum.Price = curAndLenPrice;
                        currentOptimum.PosPrev = cur + 1;
                        currentOptimum.BackPrev = 0;
                        currentOptimum.Prev1IsChar = true;
                        currentOptimum.Prev2 = false;
                    }
                }
            }

            // speed optimization
            var startLen = 2U;

            for (var repIndex = 0U; repIndex < RegisteredDistances; repIndex++)
            {
                var lenTest = this.matchFinder.GetMatchLength(0 - 1, this.reps[repIndex], numAvailableBytes);
                if (lenTest < 2U)
                {
                    continue;
                }

                var lenTestTemp = lenTest;
                do
                {
                    while (lenEnd < cur + lenTest)
                    {
                        this.optimum[++lenEnd].Price = InfinityPrice;
                    }

                    var curAndLenPrice = repMatchPrice + this.GetRepPrice(repIndex, lenTest, optimumState, posState);
                    var currentOptimum = this.optimum[cur + lenTest];
                    if (curAndLenPrice < currentOptimum.Price)
                    {
                        currentOptimum.Price = curAndLenPrice;
                        currentOptimum.PosPrev = cur;
                        currentOptimum.BackPrev = repIndex;
                        currentOptimum.Prev1IsChar = false;
                    }
                }
                while (--lenTest >= 2);
                lenTest = lenTestTemp;

                if (repIndex is 0U)
                {
                    startLen = lenTest + 1;
                }

                // if (_maxMode)
                if (lenTest < numAvailableBytesFull)
                {
                    var t = Math.Min(numAvailableBytesFull - 1 - lenTest, this.fastBytes);
                    var lenTest2 = this.matchFinder.GetMatchLength((int)lenTest, this.reps[repIndex], t);
                    if (lenTest2 >= 2)
                    {
                        var state2 = optimumState;
                        state2.UpdateRep();
                        var posStateNext = (position + lenTest) & this.positionStateMask;
                        var curAndLenCharPrice = repMatchPrice
                            + this.GetRepPrice(repIndex, lenTest, optimumState, posState)
                            + this.matchEncoders[(state2.Index << PositionStatesBitsMaximum) + posStateNext].GetPrice0()
                            + this.literalEncoder.GetSubCoder(position + lenTest, this.matchFinder.GetIndexByte((int)lenTest - 1 - 1))
                                .GetPrice(
                                    matchMode: true,
                                    this.matchFinder.GetIndexByte((int)lenTest - 1 - (int)(this.reps[repIndex] + 1)),
                                    this.matchFinder.GetIndexByte((int)lenTest - 1));
                        state2.UpdateChar();
                        posStateNext = (position + lenTest + 1) & this.positionStateMask;
                        var nextMatchPrice = curAndLenCharPrice + this.matchEncoders[(state2.Index << PositionStatesBitsMaximum) + posStateNext].GetPrice1();
                        var nextRepMatchPrice = nextMatchPrice + this.repEncoders[state2.Index].GetPrice1();

                        var offset = lenTest + 1 + lenTest2;
                        while (lenEnd < cur + offset)
                        {
                            this.optimum[++lenEnd].Price = InfinityPrice;
                        }

                        var curAndLenPrice = nextRepMatchPrice + this.GetRepPrice(0, lenTest2, state2, posStateNext);
                        var currentOptimum = this.optimum[cur + offset];
                        if (curAndLenPrice < currentOptimum.Price)
                        {
                            currentOptimum.Price = curAndLenPrice;
                            currentOptimum.PosPrev = cur + lenTest + 1;
                            currentOptimum.BackPrev = 0;
                            currentOptimum.Prev1IsChar = true;
                            currentOptimum.Prev2 = true;
                            currentOptimum.PosPrev2 = cur;
                            currentOptimum.BackPrev2 = repIndex;
                        }
                    }
                }
            }

            if (newLen > numAvailableBytes)
            {
                newLen = numAvailableBytes;
                for (currentNumDistancePairs = 0; newLen > this.matchDistances[currentNumDistancePairs]; currentNumDistancePairs += 2)
                {
                    // this goes through the distances.
                }

                this.matchDistances[currentNumDistancePairs] = newLen;
                currentNumDistancePairs += 2;
            }

            if (newLen >= startLen)
            {
                normalMatchPrice = matchPrice + this.repEncoders[optimumState.Index].GetPrice0();
                while (lenEnd < cur + newLen)
                {
                    this.optimum[++lenEnd].Price = InfinityPrice;
                }

                var offs = 0U;
                while (startLen > this.matchDistances[offs])
                {
                    offs += 2;
                }

#pragma warning disable S1994 // "for" loop increment clauses should modify the loops' counters
                for (var lenTest = startLen; ; lenTest++)
                {
                    var curBack = this.matchDistances[offs + 1];
                    var curAndLenPrice = normalMatchPrice + this.GetPosLenPrice(curBack, lenTest, posState);
                    var currentOptimum = this.optimum[cur + lenTest];
                    if (curAndLenPrice < currentOptimum.Price)
                    {
                        currentOptimum.Price = curAndLenPrice;
                        currentOptimum.PosPrev = cur;
                        currentOptimum.BackPrev = curBack + RegisteredDistances;
                        currentOptimum.Prev1IsChar = false;
                    }

                    if (lenTest == this.matchDistances[offs])
                    {
                        if (lenTest < numAvailableBytesFull)
                        {
                            var t = Math.Min(numAvailableBytesFull - 1 - lenTest, this.fastBytes);
                            var lenTest2 = this.matchFinder.GetMatchLength((int)lenTest, curBack, t);
                            if (lenTest2 >= 2)
                            {
                                var state2 = optimumState;
                                state2.UpdateMatch();
                                var posStateNext = (position + lenTest) & this.positionStateMask;
                                var curAndLenCharPrice = curAndLenPrice
                                    + this.matchEncoders[(state2.Index << PositionStatesBitsMaximum)
                                    + posStateNext].GetPrice0()
                                    + this.literalEncoder.GetSubCoder(position + lenTest, this.matchFinder.GetIndexByte((int)lenTest - 1 - 1))
                                        .GetPrice(
                                            matchMode: true,
                                            this.matchFinder.GetIndexByte((int)lenTest - (int)(curBack + 1) - 1),
                                            this.matchFinder.GetIndexByte((int)lenTest - 1));
                                state2.UpdateChar();
                                posStateNext = (position + lenTest + 1) & this.positionStateMask;
                                var nextMatchPrice = curAndLenCharPrice + this.matchEncoders[(state2.Index << PositionStatesBitsMaximum) + posStateNext].GetPrice1();
                                var nextRepMatchPrice = nextMatchPrice + this.repEncoders[state2.Index].GetPrice1();

                                var offset = lenTest + 1 + lenTest2;
                                while (lenEnd < cur + offset)
                                {
                                    this.optimum[++lenEnd].Price = InfinityPrice;
                                }

                                curAndLenPrice = nextRepMatchPrice + this.GetRepPrice(0, lenTest2, state2, posStateNext);
                                currentOptimum = this.optimum[cur + offset];
                                if (curAndLenPrice < currentOptimum.Price)
                                {
                                    currentOptimum.Price = curAndLenPrice;
                                    currentOptimum.PosPrev = cur + lenTest + 1;
                                    currentOptimum.BackPrev = 0;
                                    currentOptimum.Prev1IsChar = true;
                                    currentOptimum.Prev2 = true;
                                    currentOptimum.PosPrev2 = cur;
                                    currentOptimum.BackPrev2 = curBack + RegisteredDistances;
                                }
                            }
                        }

                        offs += 2;
                        if (offs == currentNumDistancePairs)
                        {
                            break;
                        }
                    }
                }
#pragma warning restore S1994 // "for" loop increment clauses should modify the loops' counters
            }
        }
    }

    private void Flush(uint nowPos)
    {
        this.ReleaseMatchFinderStream();
        WriteEndMarker(nowPos & this.positionStateMask);
        this.rangeEncoder.FlushData();
        this.rangeEncoder.FlushStream();

        void WriteEndMarker(uint posState)
        {
            if (!this.writeEndMark)
            {
                return;
            }

            this.matchEncoders[(this.state.Index << PositionStatesBitsMaximum) + posState].Encode(this.rangeEncoder, 1);
            this.repEncoders[this.state.Index].Encode(this.rangeEncoder, 0);
            this.state.UpdateMatch();
            const uint Length = MatchMinimumLength;
            this.lengthEncoder.Encode(this.rangeEncoder, Length - MatchMinimumLength, posState);
            const uint PositionSlot = (1U << PositionSlotBits) - 1U;
            var lenToPosState = GetLenToPosState(Length);
            this.posSlotEncoder[lenToPosState].Encode(this.rangeEncoder, PositionSlot);
            const int FooterBits = 30;
            const uint PositionReduced = (1U << FooterBits) - 1U;
            this.rangeEncoder.EncodeDirectBits(PositionReduced >> AlignBits, FooterBits - AlignBits);
            this.posAlignEncoder.ReverseEncode(this.rangeEncoder, PositionReduced & AlignMask);
        }
    }

    private void ReleaseMatchFinderStream()
    {
        if (this.matchFinder is null || !this.releaseMatchFinderStream)
        {
            return;
        }

        this.matchFinder.ReleaseStream();
        this.releaseMatchFinderStream = false;
    }

    private void FillDistancesPrices()
    {
        for (var i = StartPositionModelIndex; i < FullDistances; i++)
        {
            var posSlot = GetPosSlot(i);
            var footerBits = (int)((posSlot >> 1) - 1);
            var baseVal = (2U | (posSlot & 1U)) << footerBits;
            this.tempPrices[i] = RangeCoder.BitTreeEncoder.ReverseGetPrice(this.posEncoders, baseVal - posSlot - 1, footerBits, i - baseVal);
        }

        for (var lenToPosState = 0U; lenToPosState < LengthToPositionStates; lenToPosState++)
        {
            var encoder = this.posSlotEncoder[lenToPosState];
            var st = lenToPosState << PositionSlotBits;

            uint posSlot;
            for (posSlot = 0; posSlot < this.distributionTableSize; posSlot++)
            {
                this.posSlotPrices[st + posSlot] = encoder.GetPrice(posSlot);
            }

            for (posSlot = EndPositionModelIndex; posSlot < this.distributionTableSize; posSlot++)
            {
                this.posSlotPrices[st + posSlot] += ((posSlot >> 1) - 1 - AlignBits) << RangeCoder.BitEncoder.BitPriceShiftBits;
            }

            var st2 = lenToPosState * FullDistances;
            uint i;
            for (i = 0U; i < StartPositionModelIndex; i++)
            {
                this.distancesPrices[st2 + i] = this.posSlotPrices[st + i];
            }

            for (; i < FullDistances; i++)
            {
                this.distancesPrices[st2 + i] = this.posSlotPrices[st + GetPosSlot(i)] + this.tempPrices[i];
            }
        }

        this.matchPriceCount = 0;
    }

    private void FillAlignPrices()
    {
        for (var i = 0U; i < AlignTableSize; i++)
        {
            this.alignPrices[i] = this.posAlignEncoder.ReverseGetPrice(i);
        }

        this.alignPriceCount = 0;
    }

    private sealed class LiteralEncoder
    {
        private readonly Encoder2[] coders;
        private readonly int previousBits;
        private readonly int positionBits;
        private readonly uint positionMask;

        public LiteralEncoder(int positionBits, int previousBits)
        {
            this.positionBits = positionBits;
            this.positionMask = (1U << positionBits) - 1;
            this.previousBits = previousBits;
            var numStates = 1U << (this.previousBits + this.positionBits);
            this.coders = new Encoder2[numStates];
            for (var i = 0U; i < numStates; i++)
            {
                this.coders[i] = new();
            }
        }

        public void Init()
        {
            var numStates = 1U << (this.previousBits + this.positionBits);
            for (var i = 0U; i < numStates; i++)
            {
                this.coders[i].Init();
            }
        }

        public Encoder2 GetSubCoder(uint pos, byte prevByte) => this.coders[((pos & this.positionMask) << this.previousBits) + (uint)(prevByte >> (8 - this.previousBits))];

        public readonly struct Encoder2
        {
            private readonly RangeCoder.BitEncoder[] encoders = new RangeCoder.BitEncoder[0x300];

            public Encoder2()
            {
            }

            public void Init()
            {
                for (var i = 0; i < 0x300; i++)
                {
                    this.encoders[i].Init();
                }
            }

            public void Encode(RangeCoder.RangeEncoder rangeEncoder, byte symbol)
            {
                var context = 1U;
                for (var i = 7; i >= 0; i--)
                {
                    var bit = (uint)(symbol >> i) & 1U;
                    this.encoders[context].Encode(rangeEncoder, bit);
                    context = (context << 1) | bit;
                }
            }

            public void EncodeMatched(RangeCoder.RangeEncoder rangeEncoder, byte matchByte, byte symbol)
            {
                var context = 1U;
                var same = true;
                for (var i = 7; i >= 0; i--)
                {
                    var bit = (uint)(symbol >> i) & 1U;
                    var state = context;
                    if (same)
                    {
                        var matchBit = (uint)(matchByte >> i) & 1U;
                        state += (1 + matchBit) << 8;
                        same = matchBit == bit;
                    }

                    this.encoders[state].Encode(rangeEncoder, bit);
                    context = (context << 1) | bit;
                }
            }

            public uint GetPrice(bool matchMode, byte matchByte, byte symbol)
            {
                var price = 0U;
                var context = 1U;
                var i = 7;
                if (matchMode)
                {
                    for (; i >= 0; i--)
                    {
                        var matchBit = (uint)(matchByte >> i) & 1U;
                        var bit = (uint)(symbol >> i) & 1U;
                        price += this.encoders[((1U + matchBit) << 8) + context].GetPrice(bit);
                        context = (context << 1) | bit;
                        if (matchBit != bit)
                        {
                            i--;
                            break;
                        }
                    }
                }

                for (; i >= 0; i--)
                {
                    var bit = (uint)(symbol >> i) & 1;
                    price += this.encoders[context].GetPrice(bit);
                    context = (context << 1) | bit;
                }

                return price;
            }
        }
    }

    private class LengthEncoder
    {
        private readonly RangeCoder.BitTreeEncoder[] lowCoder = new RangeCoder.BitTreeEncoder[PositionStatesEncodingMaximum];
        private readonly RangeCoder.BitTreeEncoder[] midCoder = new RangeCoder.BitTreeEncoder[PositionStatesEncodingMaximum];
        private readonly RangeCoder.BitTreeEncoder highCoder = new(HighLengthBits);
#pragma warning disable S3459
        private RangeCoder.BitEncoder firstChoice;
        private RangeCoder.BitEncoder secondChoice;
#pragma warning restore S3459

#pragma warning disable S1144
        protected LengthEncoder()
        {
            for (var i = 0U; i < PositionStatesEncodingMaximum; i++)
            {
                this.lowCoder[i] = new(LowLengthBits);
                this.midCoder[i] = new(MidLengthBits);
            }
        }
#pragma warning restore S1144

        public void Init(uint numPosStates)
        {
            this.firstChoice.Init();
            this.secondChoice.Init();
            for (var i = 0U; i < numPosStates; i++)
            {
                this.lowCoder[i].Init();
                this.midCoder[i].Init();
            }

            this.highCoder.Init();
        }

        protected void Encode(RangeCoder.RangeEncoder rangeEncoder, uint symbol, uint posState)
        {
            if (symbol < LowLengthSymbols)
            {
                this.firstChoice.Encode(rangeEncoder, 0);
                this.lowCoder[posState].Encode(rangeEncoder, symbol);
            }
            else
            {
                symbol -= LowLengthSymbols;
                this.firstChoice.Encode(rangeEncoder, 1);
                if (symbol < MidLengthSymbols)
                {
                    this.secondChoice.Encode(rangeEncoder, 0);
                    this.midCoder[posState].Encode(rangeEncoder, symbol);
                }
                else
                {
                    this.secondChoice.Encode(rangeEncoder, 1);
                    this.highCoder.Encode(rangeEncoder, symbol - MidLengthSymbols);
                }
            }
        }

        protected void SetPrices(uint posState, uint numSymbols, uint[] prices, uint st)
        {
            var a0 = this.firstChoice.GetPrice0();
            var a1 = this.firstChoice.GetPrice1();
            var b0 = a1 + this.secondChoice.GetPrice0();
            var b1 = a1 + this.secondChoice.GetPrice1();
            uint i;
            for (i = 0U; i < LowLengthSymbols; i++)
            {
                if (i >= numSymbols)
                {
                    return;
                }

                prices[st + i] = a0 + this.lowCoder[posState].GetPrice(i);
            }

            for (; i < LowLengthSymbols + MidLengthSymbols; i++)
            {
                if (i >= numSymbols)
                {
                    return;
                }

                prices[st + i] = b0 + this.midCoder[posState].GetPrice(i - LowLengthSymbols);
            }

            for (; i < numSymbols; i++)
            {
                prices[st + i] = b1 + this.highCoder.GetPrice(i - LowLengthSymbols - MidLengthSymbols);
            }
        }
    }

    private sealed class LengthPriceTableEncoder : LengthEncoder
    {
        private readonly uint[] prices = new uint[LengthSymbols << PositionStatesBitsEncodingMaximum];
        private readonly uint[] counters = new uint[PositionStatesEncodingMaximum];
        private uint tableSize;

        public void SetTableSize(uint desiredSize) => this.tableSize = desiredSize;

        public uint GetPrice(uint symbol, uint posState) => this.prices[(posState * LengthSymbols) + symbol];

        public void UpdateTables(uint numPosStates)
        {
            for (var posState = 0U; posState < numPosStates; posState++)
            {
                this.UpdateTable(posState);
            }
        }

        public new void Encode(RangeCoder.RangeEncoder rangeEncoder, uint symbol, uint posState)
        {
            base.Encode(rangeEncoder, symbol, posState);
            if (--this.counters[posState] is 0U)
            {
                this.UpdateTable(posState);
            }
        }

        private void UpdateTable(uint posState)
        {
            this.SetPrices(posState, this.tableSize, this.prices, posState * LengthSymbols);
            this.counters[posState] = this.tableSize;
        }
    }

    private sealed class Optimal
    {
        public State State { get; set; }

        public bool Prev1IsChar { get; set; }

        public bool Prev2 { get; set; }

        public uint PosPrev2 { get; set; }

        public uint BackPrev2 { get; set; }

        public uint Price { get; set; }

        public uint PosPrev { get; set; }

        public uint BackPrev { get; set; }

        public uint Backs0 { get; set; }

        public uint Backs1 { get; set; }

        public uint Backs2 { get; set; }

        public uint Backs3 { get; set; }

        public void MakeAsChar()
        {
            this.BackPrev = uint.MaxValue;
            this.Prev1IsChar = false;
        }

        public void MakeAsShortRep()
        {
            this.BackPrev = 0;
            this.Prev1IsChar = false;
        }

        public bool IsShortRep() => this.BackPrev is 0U;
    }
}