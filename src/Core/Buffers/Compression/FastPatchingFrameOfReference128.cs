// -----------------------------------------------------------------------
// <copyright file="FastPatchingFrameOfReference128.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// This class is similar to <see cref="FastPatchingFrameOfReference256"/> but uses a small block size.
/// </summary>
internal sealed class FastPatchingFrameOfReference128 : IInt32Codec, IHeadlessInt32Codec
{
    /// <summary>
    /// The block size.
    /// </summary>
    public const int BlockSize = 128;

    private const int OverheadOfEachExcept = 8;
    private const int DefaultPageSize = 65536;

    private readonly int pageSize;
    private readonly int[][] dataTobePacked = new int[33][];
    private readonly MemoryStream byteContainer;

    // Working area for compress and decompress.
    private readonly int[] dataPointers = new int[33];
    private readonly int[] frequencies = new int[33];

    private int bestB;
    private int bestExcept;
    private int maxB;

    /// <summary>
    /// Initializes a new instance of the <see cref="FastPatchingFrameOfReference128"/> class.
    /// </summary>
    public FastPatchingFrameOfReference128()
        : this(DefaultPageSize)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FastPatchingFrameOfReference128"/> class.
    /// </summary>
    /// <param name="pageSize">The page size.</param>
    public FastPatchingFrameOfReference128(int pageSize)
    {
        this.pageSize = pageSize;

        // Initiate arrays.
        this.byteContainer = new((3 * this.pageSize / BlockSize) + this.pageSize);
        for (var k = 1; k < this.dataTobePacked.Length; k++)
        {
            this.dataTobePacked[k] = new int[this.pageSize / 32 * 4]; // heuristic
        }
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => this.HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length = Util.GreatestMultiple(length, BlockSize);
        if (length is 0)
        {
            return;
        }

        destination[destinationIndex] = length;
        destinationIndex++;
        this.HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var outputLength = source[sourceIndex];
        sourceIndex++;
        this.HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, length, outputLength);
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => this.HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, length, number);

    /// <inheritdoc/>
    public override string ToString() => nameof(FastPatchingFrameOfReference128);

    private void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length = Util.GreatestMultiple(length, BlockSize);

        // Allocate memory for working area.
        var finalSourceIndex = sourceIndex + length;
        while (sourceIndex != finalSourceIndex)
        {
            var thisSize = Math.Min(this.pageSize, finalSourceIndex - sourceIndex);
            EncodePage(source, ref sourceIndex, destination, ref destinationIndex, thisSize);
        }

        void EncodePage(int[] sourcePage, ref int sourcePageIndex, int[] destinationPage, ref int destinationPageIndex, int size)
        {
            var headerPosition = destinationPageIndex;
            destinationPageIndex++;
            var temporaryDestinationIndex = destinationPageIndex;

            // Clear working area.
            Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            this.byteContainer.Clear();

            var temporarySourceIndex = sourcePageIndex;
            for (var index = temporarySourceIndex + size - BlockSize; temporarySourceIndex <= index; temporarySourceIndex += BlockSize)
            {
                GetBestBFromData(sourcePage, temporarySourceIndex);

                var temporaryBestB = this.bestB;
                this.byteContainer.WriteSByte((sbyte)this.bestB);
                this.byteContainer.WriteSByte((sbyte)this.bestExcept);
                if (this.bestExcept > 0)
                {
                    this.byteContainer.WriteSByte((sbyte)this.maxB);

                    var dataIndex = this.maxB - this.bestB;
                    if (this.dataPointers[dataIndex] + this.bestExcept >= this.dataTobePacked[dataIndex].Length)
                    {
                        var newSize = 2 * (this.dataPointers[dataIndex] + this.bestExcept);

                        // make sure it is a multiple of 32
                        newSize = Util.GreatestMultiple(newSize + 31, 32);
                        var temp = this.dataTobePacked[dataIndex];
                        Array.Resize(ref temp, newSize);
                        this.dataTobePacked[dataIndex] = temp;
                    }

                    for (var k = 0; k < BlockSize; k++)
                    {
                        if (sourcePage[k + temporarySourceIndex] >>> this.bestB is not 0)
                        {
                            // we have an exception
                            this.byteContainer.WriteSByte((sbyte)k);
                            this.dataTobePacked[dataIndex][this.dataPointers[dataIndex]++] = sourcePage[k + temporarySourceIndex] >>> temporaryBestB;
                        }
                    }
                }

                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(sourcePage.AsSpan(temporarySourceIndex + k), destinationPage.AsSpan(temporaryDestinationIndex), temporaryBestB);
                    temporaryDestinationIndex += temporaryBestB;
                }
            }

            sourcePageIndex = temporarySourceIndex;
            destinationPage[headerPosition] = temporaryDestinationIndex - headerPosition;

            var byteSize = (int)this.byteContainer.Position;
            while ((this.byteContainer.Position & 3) is not 0)
            {
                this.byteContainer.WriteByte(0);
            }

            destinationPage[temporaryDestinationIndex++] = byteSize;

            var count = (int)this.byteContainer.Position / 4;
            this.byteContainer.Position = 0;
            _ = this.byteContainer.Read(destinationPage, temporaryDestinationIndex, count, ByteOrder.LittleEndian);
            temporaryDestinationIndex += count;
            var bitmap = 0;
            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is not 0)
                {
                    bitmap |= 1 << (k - 1);
                }
            }

            destinationPage[temporaryDestinationIndex++] = bitmap;

            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is not 0)
                {
                    destinationPage[temporaryDestinationIndex++] = this.dataPointers[k]; // size
                    var j = 0;
                    for (; j < this.dataPointers[k]; j += 32)
                    {
                        BitPacking.Pack(this.dataTobePacked[k].AsSpan(j), destinationPage.AsSpan(temporaryDestinationIndex), k);
                        temporaryDestinationIndex += k;
                    }

                    var overflow = j - this.dataPointers[k];
                    temporaryDestinationIndex -= overflow * k / 32;
                }
            }

            destinationPageIndex = temporaryDestinationIndex;

            void GetBestBFromData(int[] input, int position)
            {
                Array.Clear(this.frequencies, 0, this.frequencies.Length);
                var end = position + BlockSize;
                for (var k = position; k < end; k++)
                {
                    this.frequencies[Util.Bits(input[k])]++;
                }

                this.bestB = 32;
                while (this.frequencies[this.bestB] is 0)
                {
                    this.bestB--;
                }

                this.maxB = this.bestB;
                var bestCost = this.bestB * BlockSize;
                var currentExcept = 0;
                this.bestExcept = currentExcept;
                for (var b = this.bestB - 1; b >= 0; --b)
                {
                    currentExcept += this.frequencies[b + 1];
                    if (currentExcept == BlockSize)
                    {
                        break;
                    }

                    // the extra 8 is the cost of storing max-bits
                    var currentCost = (currentExcept * OverheadOfEachExcept)
                                      + (currentExcept * (this.maxB - b))
                                      + (b * BlockSize) + 8;
                    if (this.maxB - b is 1)
                    {
                        currentCost -= currentExcept;
                    }

                    if (currentCost < bestCost)
                    {
                        bestCost = currentCost;
                        this.bestB = b;
                        this.bestExcept = currentExcept;
                    }
                }
            }
        }
    }

    private void HeadlessDecompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number)
    {
        if (length is 0)
        {
            return;
        }

        number = Util.GreatestMultiple(number, BlockSize);
        var finalDestinationIndex = destinationIndex + number;
        while (destinationIndex != finalDestinationIndex)
        {
            DecodePage(source, ref sourceIndex, destination, ref destinationIndex, Math.Min(this.pageSize, finalDestinationIndex - destinationIndex));
        }

        void DecodePage(int[] sourcePage, ref int sourcePageIndex, int[] destinationPage, ref int destinationPageIndex, int size)
        {
            var initialPosition = sourcePageIndex;

            var metaIndex = sourcePage[sourcePageIndex];
            sourcePageIndex++;
            var exceptIndex = initialPosition + metaIndex;

            var byteSize = sourcePage[exceptIndex++];
            this.byteContainer.Clear();
            this.byteContainer.Write(sourcePage, exceptIndex, (byteSize + 3) / 4);
            this.byteContainer.Position = 0;
            exceptIndex += (byteSize + 3) / 4;

            var bitmap = sourcePage[exceptIndex++];
            for (var k = 2; k <= 32; k++)
            {
                if ((bitmap & (1 << (k - 1))) is not 0)
                {
                    var currentSize = sourcePage[exceptIndex++];
                    var roundedUp = Util.GreatestMultiple(currentSize + 31, 32);
                    if (this.dataTobePacked[k].Length < roundedUp)
                    {
                        this.dataTobePacked[k] = new int[roundedUp];
                    }

                    if (exceptIndex + (roundedUp / 32 * k) <= sourcePage.Length)
                    {
                        var j = 0;
                        for (; j < currentSize; j += 32)
                        {
                            BitPacking.Unpack(sourcePage.AsSpan(exceptIndex), this.dataTobePacked[k].AsSpan(j), k);
                            exceptIndex += k;
                        }

                        var overflow = j - currentSize;
                        exceptIndex -= overflow * k / 32;
                    }
                    else
                    {
                        var buffer = new int[roundedUp / 32 * k];
                        var initialExceptIndex = exceptIndex;
                        Array.Copy(sourcePage, exceptIndex, buffer, 0, sourcePage.Length - exceptIndex);

                        var j = 0;
                        for (; j < currentSize; j += 32)
                        {
                            BitPacking.Unpack(buffer.AsSpan(exceptIndex - initialExceptIndex), this.dataTobePacked[k].AsSpan(j), k);
                            exceptIndex += k;
                        }

                        var overflow = j - currentSize;
                        exceptIndex -= overflow * k / 32;
                    }
                }
            }

            Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            var temporaryDestinationIndex = destinationPageIndex;
            var temporarySourceIndex = sourcePageIndex;

            var runEnd = size / BlockSize;
            for (var run = 0; run < runEnd; run++, temporaryDestinationIndex += BlockSize)
            {
                int b = this.byteContainer.ReadSByte();

                var currentExceptions = this.byteContainer.ReadSByte() & 0xFF;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(sourcePage.AsSpan(temporarySourceIndex), destinationPage.AsSpan(temporaryDestinationIndex + k), b);
                    temporarySourceIndex += b;
                }

                if (currentExceptions > 0)
                {
                    int maxBits = this.byteContainer.ReadSByte();

                    var index = maxBits - b;
                    if (index is 1)
                    {
                        for (var k = 0; k < currentExceptions; k++)
                        {
                            var pos = this.byteContainer.ReadSByte() & 0xFF;
                            destinationPage[pos + temporaryDestinationIndex] |= 1 << b;
                        }
                    }
                    else
                    {
                        for (var k = 0; k < currentExceptions; k++)
                        {
                            var pos = this.byteContainer.ReadSByte() & 0xFF;

                            destinationPage[pos + temporaryDestinationIndex] |= this.dataTobePacked[index][this.dataPointers[index]++] << b;
                        }
                    }
                }
            }

            destinationPageIndex = temporaryDestinationIndex;
            sourcePageIndex = exceptIndex;
        }
    }
}