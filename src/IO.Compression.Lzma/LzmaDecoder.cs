// -----------------------------------------------------------------------
// <copyright file="LzmaDecoder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

using static System.IO.Compression.Constants;

/// <summary>
/// The LZMA decoder.
/// </summary>
public class LzmaDecoder
{
    private readonly LZ.OutWindow outWindow = new();
    private readonly RangeCoder.RangeDecoder rangeDecoder = new();

    private readonly RangeCoder.BitDecoder[] matchDecoders = new RangeCoder.BitDecoder[States << PositionStatesBitsMaximum];
    private readonly RangeCoder.BitDecoder[] repDecoders = new RangeCoder.BitDecoder[States];
    private readonly RangeCoder.BitDecoder[] repG0Decoders = new RangeCoder.BitDecoder[States];
    private readonly RangeCoder.BitDecoder[] repG1Decoders = new RangeCoder.BitDecoder[States];
    private readonly RangeCoder.BitDecoder[] repG2Decoders = new RangeCoder.BitDecoder[States];
    private readonly RangeCoder.BitDecoder[] rep0LongDecoders = new RangeCoder.BitDecoder[States << PositionStatesBitsMaximum];

    private readonly RangeCoder.BitTreeDecoder[] posSlotDecoder = new RangeCoder.BitTreeDecoder[LengthToPositionStates];
    private readonly RangeCoder.BitDecoder[] posDecoders = new RangeCoder.BitDecoder[FullDistances - EndPositionModelIndex];

    private readonly LenDecoder lenDecoder;
    private readonly LenDecoder repLenDecoder;

    private readonly LiteralDecoder literalDecoder;

    private readonly RangeCoder.BitTreeDecoder posAlignDecoder = new(AlignBits);

    private readonly uint positionStateMask;

    private State state;
    private ulong bytesRead;

    private uint dictionarySize;
    private uint dictionarySizeCheck;

    private bool firstRead;

    /// <summary>
    /// Initializes a new instance of the <see cref="LzmaDecoder"/> class.
    /// </summary>
    /// <param name="properties">The properties.</param>
    public LzmaDecoder(params IList<byte> properties)
    {
        if (properties.Count < 5)
        {
            throw new ArgumentOutOfRangeException(nameof(properties));
        }

        this.dictionarySize = uint.MaxValue;
        for (var i = 0; i < LengthToPositionStates; i++)
        {
            this.posSlotDecoder[i] = new(PositionSlotBits);
        }

        var lc = properties[0] % 9;
        var remainder = properties[0] / 9;
        var lp = remainder % 5;
        var positionBits = remainder / 5;
        if (positionBits > PositionStatesBitsMaximum)
        {
            throw new InvalidDataException();
        }

        var currentDictionarySize = 0U;
        for (var i = 0; i < 4; i++)
        {
            currentDictionarySize += ((uint)properties[1 + i]) << (i * 8);
        }

        SetDictionarySize(currentDictionarySize);
        this.literalDecoder = CreateLiteralDecoder(lp, lc);
        var positionStateCount = 1U << positionBits;
        this.lenDecoder = new(positionStateCount);
        this.repLenDecoder = new(positionStateCount);
        this.positionStateMask = positionStateCount - 1;

        void SetDictionarySize(uint desiredSize)
        {
            if (this.dictionarySize == desiredSize)
            {
                return;
            }

            this.dictionarySize = desiredSize;
            this.dictionarySizeCheck = Math.Max(this.dictionarySize, 1);
            var blockSize = Math.Max(this.dictionarySizeCheck, 1 << 12);
            this.outWindow.Create((int)blockSize);
        }

        static LiteralDecoder CreateLiteralDecoder(int positionBitCount, int previousBitCount)
        {
            return (numPosBits: positionBitCount, numPrevBits: previousBitCount) switch
            {
#pragma warning disable SA1008
                ( > 8, _) => throw new ArgumentOutOfRangeException(nameof(positionBitCount)),
#pragma warning restore SA1008
                (_, > 8) => throw new ArgumentOutOfRangeException(nameof(previousBitCount)),
                _ => new(positionBitCount, previousBitCount),
            };
        }
    }

    /// <summary>
    /// Decompresses the stream to the output.
    /// </summary>
    /// <param name="output">The output stream.</param>
    /// <param name="outputSize">The output size.</param>
    public void Decompress(Stream output, long outputSize = -1)
    {
        this.SetOutputStream(output);

        var size = outputSize < 0
            ? ulong.MaxValue
            : this.bytesRead + (ulong)outputSize - (ulong)this.outWindow.BytesToWrite;

        this.Decode(ref this.state, ref this.bytesRead, this.firstRead, size);
        this.firstRead = false;

        this.outWindow.ReleaseStream();
    }

    /// <summary>
    /// Decodes the input stream to the output.
    /// </summary>
    /// <param name="input">The input stream.</param>
    /// <param name="output">The output stream.</param>
    /// <param name="outputSize">The output size.</param>
    public void Decode(Stream input, Stream output, long outputSize = -1)
    {
        this.SetInputStream(input);
        this.SetOutputStream(output);

        State currentState = default;
        var nowPos64 = 0UL;
        this.Decode(ref currentState, ref nowPos64, this.firstRead, (ulong)outputSize);

        this.outWindow.ReleaseStream();
        this.rangeDecoder.ReleaseStream();
    }

    /// <summary>
    /// Sets the input stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public void SetInputStream(Stream? stream)
    {
        this.rangeDecoder.ReleaseStream();
        if (stream is null)
        {
            return;
        }

        this.rangeDecoder.Init(stream);
        this.firstRead = true;

        for (var i = 0U; i < States; i++)
        {
            for (var j = 0U; j <= this.positionStateMask; j++)
            {
                var index = (i << PositionStatesBitsMaximum) + j;
                this.matchDecoders[index].Init();
                this.rep0LongDecoders[index].Init();
            }

            this.repDecoders[i].Init();
            this.repG0Decoders[i].Init();
            this.repG1Decoders[i].Init();
            this.repG2Decoders[i].Init();
        }

        this.literalDecoder.Init();
        for (var i = 0U; i < LengthToPositionStates; i++)
        {
            this.posSlotDecoder[i].Init();
        }

        for (var i = 0U; i < FullDistances - EndPositionModelIndex; i++)
        {
            this.posDecoders[i].Init();
        }

        this.lenDecoder.Init();
        this.repLenDecoder.Init();
        this.posAlignDecoder.Init();
    }

    /// <summary>
    /// Sets the output stream.
    /// </summary>
    /// <param name="stream">The stream.</param>
    public void SetOutputStream(Stream stream) => this.outWindow.Init(stream);

    private void Decode(ref State decoderState, ref ulong position, bool first, ulong outputSize)
    {
        var rep0 = 0U;
        var rep1 = 0U;
        var rep2 = 0U;
        var rep3 = 0U;

        if (first)
        {
            if (this.matchDecoders[decoderState.Index << PositionStatesBitsMaximum].Decode(this.rangeDecoder) is not 0U)
            {
                throw new InvalidDataException();
            }

            decoderState.UpdateChar();
            var b = this.literalDecoder.DecodeNormal(this.rangeDecoder, 0, 0);
            this.outWindow.Write(b);
            position++;
        }

        while (position < outputSize)
        {
            var posState = (uint)position & this.positionStateMask;
            if (this.matchDecoders[(decoderState.Index << PositionStatesBitsMaximum) + posState].Decode(this.rangeDecoder) is 0)
            {
                var prevByte = this.outWindow.ReadByte(0);
                var b = decoderState.IsCharState()
                    ? this.literalDecoder.DecodeNormal(this.rangeDecoder, (uint)position, prevByte)
                    : this.literalDecoder.DecodeWithMatchByte(this.rangeDecoder, (uint)position, prevByte, this.outWindow.ReadByte(rep0));
                this.outWindow.Write(b);
                decoderState.UpdateChar();
                position++;
                continue;
            }

            uint length;
            if (this.repDecoders[decoderState.Index].Decode(this.rangeDecoder) is 1U)
            {
                if (this.repG0Decoders[decoderState.Index].Decode(this.rangeDecoder) is 0U)
                {
                    if (this.rep0LongDecoders[(decoderState.Index << PositionStatesBitsMaximum) + posState].Decode(this.rangeDecoder) is 0U)
                    {
                        decoderState.UpdateShortRep();
                        this.outWindow.Write(this.outWindow.ReadByte(rep0));
                        position++;
                        continue;
                    }
                }
                else
                {
                    uint distance;
                    if (this.repG1Decoders[decoderState.Index].Decode(this.rangeDecoder) is 0U)
                    {
                        distance = rep1;
                    }
                    else
                    {
                        if (this.repG2Decoders[decoderState.Index].Decode(this.rangeDecoder) is 0U)
                        {
                            distance = rep2;
                        }
                        else
                        {
                            distance = rep3;
                            rep3 = rep2;
                        }

                        rep2 = rep1;
                    }

                    rep1 = rep0;
                    rep0 = distance;
                }

                length = this.repLenDecoder.Decode(this.rangeDecoder, posState) + MatchMinimumLength;
                decoderState.UpdateRep();
            }
            else
            {
                rep3 = rep2;
                rep2 = rep1;
                rep1 = rep0;
                length = MatchMinimumLength + this.lenDecoder.Decode(this.rangeDecoder, posState);
                decoderState.UpdateMatch();
                var posSlot = this.posSlotDecoder[GetLenToPosState(length)].Decode(this.rangeDecoder);
                if (posSlot >= StartPositionModelIndex)
                {
                    var numDirectBits = (int)((posSlot >> 1) - 1);
                    rep0 = (2 | (posSlot & 1)) << numDirectBits;
                    if (posSlot < EndPositionModelIndex)
                    {
                        rep0 += RangeCoder.BitTreeDecoder.ReverseDecode(this.posDecoders, rep0 - posSlot - 1, this.rangeDecoder, numDirectBits);
                    }
                    else
                    {
                        rep0 += this.rangeDecoder.DecodeDirectBits(numDirectBits - AlignBits) << AlignBits;
                        rep0 += this.posAlignDecoder.ReverseDecode(this.rangeDecoder);
                    }
                }
                else
                {
                    rep0 = posSlot;
                }
            }

            if (rep0 >= position || rep0 >= this.dictionarySizeCheck)
            {
                if (rep0 is uint.MaxValue)
                {
                    break;
                }

                throw new InvalidDataException();
            }

            _ = this.outWindow.CopyBlock(rep0, length);
            position += length;
        }
    }

    private sealed class LenDecoder
    {
        private readonly RangeCoder.BitTreeDecoder[] lowCoder = new RangeCoder.BitTreeDecoder[PositionStatesMaximum];
        private readonly RangeCoder.BitTreeDecoder[] midCoder = new RangeCoder.BitTreeDecoder[PositionStatesMaximum];
        private readonly RangeCoder.BitTreeDecoder highCoder = new(HighLengthBits);
        private readonly uint count;
#pragma warning disable S3459
        private RangeCoder.BitDecoder firstChoice;
        private RangeCoder.BitDecoder secondChoice;
#pragma warning restore S3459

        public LenDecoder(uint count)
        {
            for (var i = this.count; i < count; i++)
            {
                this.lowCoder[i] = new(LowLengthBits);
                this.midCoder[i] = new(MidLengthBits);
            }

            this.count = count;
        }

        public void Init()
        {
            this.firstChoice.Init();
            for (var i = 0U; i < this.count; i++)
            {
                this.lowCoder[i].Init();
                this.midCoder[i].Init();
            }

            this.secondChoice.Init();
            this.highCoder.Init();
        }

        public uint Decode(RangeCoder.RangeDecoder rangeDecoder, uint positionState)
        {
            if (this.firstChoice.Decode(rangeDecoder) is 0U)
            {
                return this.lowCoder[positionState].Decode(rangeDecoder);
            }

            var symbol = LowLengthSymbols;
            if (this.secondChoice.Decode(rangeDecoder) is 0U)
            {
                symbol += this.midCoder[positionState].Decode(rangeDecoder);
            }
            else
            {
                symbol += MidLengthSymbols;
                symbol += this.highCoder.Decode(rangeDecoder);
            }

            return symbol;
        }
    }

    private sealed class LiteralDecoder
    {
        private readonly Decoder2[]? coders;
        private readonly int previousBitCount;
        private readonly int positionBitCount;
        private readonly uint positionMask;

        public LiteralDecoder(int positionBitCount, int previousBitCount)
        {
            if (this.coders is not null
                && this.previousBitCount == previousBitCount
                && this.positionBitCount == positionBitCount)
            {
                return;
            }

            this.positionBitCount = positionBitCount;
            this.positionMask = (1U << positionBitCount) - 1;
            this.previousBitCount = previousBitCount;
            var numStates = 1U << (this.previousBitCount + this.positionBitCount);
            this.coders = new Decoder2[numStates];
            for (var i = 0; i < numStates; i++)
            {
                this.coders[i] = new();
            }
        }

        public void Init()
        {
            if (this.coders is null)
            {
                return;
            }

            var stateCount = 1U << (this.previousBitCount + this.positionBitCount);
            for (var i = 0U; i < stateCount; i++)
            {
                this.coders[i].Init();
            }
        }

        public byte DecodeNormal(RangeCoder.RangeDecoder rangeDecoder, uint position, byte previousByte) => this.coders is not null
            ? this.coders[this.GetState(position, previousByte)].DecodeNormal(rangeDecoder)
            : throw new InvalidOperationException();

        public byte DecodeWithMatchByte(RangeCoder.RangeDecoder rangeDecoder, uint position, byte previousByte, byte matchByte) => this.coders is not null
            ? this.coders[this.GetState(position, previousByte)].DecodeWithMatchByte(rangeDecoder, matchByte)
            : throw new InvalidOperationException();

        private uint GetState(uint position, byte previousByte) => ((position & this.positionMask) << this.previousBitCount) + (uint)(previousByte >> (8 - this.previousBitCount));

        private readonly struct Decoder2()
        {
            private readonly RangeCoder.BitDecoder[] decoders = new RangeCoder.BitDecoder[0x300];

            public void Init()
            {
                for (var i = 0; i < 0x300; i++)
                {
                    this.decoders[i].Init();
                }
            }

            public byte DecodeNormal(RangeCoder.RangeDecoder rangeDecoder)
            {
                var symbol = 1U;
                do
                {
                    symbol = (symbol << 1) | this.decoders[symbol].Decode(rangeDecoder);
                }
                while (symbol < 0x100);

                return unchecked((byte)symbol);
            }

            public byte DecodeWithMatchByte(RangeCoder.RangeDecoder rangeDecoder, byte matchByte)
            {
                var symbol = 1U;
                do
                {
                    var matchBit = (uint)(matchByte >> 7) & 1U;
                    matchByte <<= 1;
                    var bit = this.decoders[((1 + matchBit) << 8) + symbol].Decode(rangeDecoder);
                    symbol = (symbol << 1) | bit;
                    if (matchBit != bit)
                    {
                        while (symbol < 0x100)
                        {
                            symbol = (symbol << 1) | this.decoders[symbol].Decode(rangeDecoder);
                        }

                        break;
                    }
                }
                while (symbol < 0x100);

                return unchecked((byte)symbol);
            }
        }
    }
}