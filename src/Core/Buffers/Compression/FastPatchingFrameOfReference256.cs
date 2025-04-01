// -----------------------------------------------------------------------
// <copyright file="FastPatchingFrameOfReference256.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// This is a patching scheme designed for speed.
/// </summary>
/// <remarks>
/// <para>It encodes integers in blocks of integers within pages of up to 65536 integers. Note that it is important, to get good compression and good performance, to use sizeable blocks (greater than 1024 integers).</para>
/// <para>For arrays containing a number of integers that is not divisible by <see cref="BlockSize"/>, you should use it in conjunction with another <see cref="IInt32Codec"/>.</para>
/// <para>For sufficiently compressible and long arrays, it is faster and better than other Patching Frame of Reference schemes.</para>
/// <para>Note that this does not use differential coding: if you are working on sorted lists, use <see cref="Differential"/> instead.</para>
/// <para>For multithreaded applications, each thread should use its own <see cref="FastPatchingFrameOfReference256"/> object.</para>
/// </remarks>
/// <see href="http://onlinelibrary.wiley.com/doi/10.1002/spe.2203/abstract"/>
/// <see href="http://arxiv.org/abs/1209.2137"/>
internal sealed class FastPatchingFrameOfReference256 : IInt32Codec, IHeadlessInt32Codec
{
    /// <summary>
    /// The block size.
    /// </summary>
    public const int BlockSize = 256;

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
    /// Initializes a new instance of the <see cref="FastPatchingFrameOfReference256"/> class.
    /// </summary>
    public FastPatchingFrameOfReference256()
        : this(DefaultPageSize)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FastPatchingFrameOfReference256"/> class.
    /// </summary>
    /// <param name="pageSize">The page size.</param>
    public FastPatchingFrameOfReference256(int pageSize)
    {
        this.pageSize = pageSize;

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
    public override string ToString() => nameof(FastPatchingFrameOfReference256);

    private void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length = Util.GreatestMultiple(length, BlockSize);

        // Allocate memory for working area.
        var finalSourceIndex = sourceIndex + length;
        while (sourceIndex != finalSourceIndex)
        {
            EncodePage(source, ref sourceIndex, destination, ref destinationIndex, Math.Min(this.pageSize, finalSourceIndex - sourceIndex));
        }

        void EncodePage(int[] sourcePage, ref int sourcePageIndex, int[] destinationPage, ref int destinationPageIndex, int size)
        {
            var headerIndex = destinationPageIndex;
            destinationPageIndex++;
            var temporaryDestinationIndex = destinationPageIndex;

            // Clear working area.
            System.Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            this.byteContainer.Clear();

            var temporarySourceIndex = sourcePageIndex;
            for (var index = temporarySourceIndex + size - BlockSize; temporarySourceIndex <= index; temporarySourceIndex += BlockSize)
            {
                GetBestBFromData(sourcePage, temporarySourceIndex);

                var temporaryBestBit = this.bestB;
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
                        System.Array.Resize(ref temp, newSize);
                        this.dataTobePacked[dataIndex] = temp;
                    }

                    for (var k = 0; k < BlockSize; k++)
                    {
                        if (sourcePage[k + temporarySourceIndex] >>> this.bestB is not 0)
                        {
                            // we have an exception
                            this.byteContainer.WriteSByte((sbyte)k);
                            this.dataTobePacked[dataIndex][this.dataPointers[dataIndex]++] = sourcePage[k + temporarySourceIndex] >>> temporaryBestBit;
                        }
                    }
                }

                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(sourcePage.AsSpan(temporarySourceIndex + k, 32), destinationPage.AsSpan(temporaryDestinationIndex, 32), temporaryBestBit);
                    temporaryDestinationIndex += temporaryBestBit;
                }
            }

            sourcePageIndex = temporarySourceIndex;
            destinationPage[headerIndex] = temporaryDestinationIndex - headerIndex;

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

            void GetBestBFromData(int[] input, int index)
            {
                System.Array.Clear(this.frequencies, 0, this.frequencies.Length);
                int end = index + BlockSize;
                for (var k = index; k < end; k++)
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
                    var cost = (currentExcept * OverheadOfEachExcept) + (currentExcept * (this.maxB - b)) + (b * BlockSize) + 8;
                    if (this.maxB - b == 1)
                    {
                        cost -= currentExcept;
                    }

                    if (cost < bestCost)
                    {
                        bestCost = cost;
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
        var finalOutput = destinationIndex + number;
        while (destinationIndex != finalOutput)
        {
            var thisSize = Math.Min(
                this.pageSize,
                finalOutput - destinationIndex);
            DecodePage(source, ref sourceIndex, destination, ref destinationIndex, thisSize);
        }

        void DecodePage(int[] sourcePage, ref int sourcePageIndex, int[] destinationPage, ref int destinationPageIndex, int size)
        {
            var initialSourceIndex = sourcePageIndex;

            var metaLocation = sourcePage[sourcePageIndex];
            sourcePageIndex++;
            var exceptIndex = initialSourceIndex + metaLocation;

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
                        var buf = new int[roundedUp / 32 * k];
                        var initialExcept = exceptIndex;
                        System.Array.Copy(sourcePage, exceptIndex, buf, 0, sourcePage.Length - exceptIndex);

                        var j = 0;
                        for (; j < currentSize; j += 32)
                        {
                            BitPacking.Unpack(buf.AsSpan(exceptIndex - initialExcept), this.dataTobePacked[k].AsSpan(j), k);
                            exceptIndex += k;
                        }

                        var overflow = j - currentSize;
                        exceptIndex -= overflow * k / 32;
                    }
                }
            }

            System.Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            var temporaryDestinationIndex = destinationPageIndex;
            var temporarySourceIndex = sourcePageIndex;

            var runEnd = size / BlockSize;
            for (var run = 0; run < runEnd; run++, temporaryDestinationIndex += BlockSize)
            {
                int b = this.byteContainer.ReadSByte();

                var currentExcept = this.byteContainer.ReadSByte() & 0xFF;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(sourcePage.AsSpan(temporarySourceIndex), destinationPage.AsSpan(temporaryDestinationIndex + k), b);
                    temporarySourceIndex += b;
                }

                if (currentExcept > 0)
                {
                    int maxBits = this.byteContainer.ReadSByte();

                    var index = maxBits - b;
                    if (index is 1)
                    {
                        for (var k = 0; k < currentExcept; k++)
                        {
                            var position = this.byteContainer.ReadSByte() & 0xFF;
                            destinationPage[position + temporaryDestinationIndex] |= 1 << b;
                        }
                    }
                    else
                    {
                        for (var k = 0; k < currentExcept; k++)
                        {
                            var position = this.byteContainer.ReadSByte() & 0xFF;

                            var exceptValue = this.dataTobePacked[index][this.dataPointers[index]++];
                            destinationPage[position + temporaryDestinationIndex] |= exceptValue << b;
                        }
                    }
                }
            }

            destinationPageIndex = temporaryDestinationIndex;
            sourcePageIndex = exceptIndex;
        }
    }
}