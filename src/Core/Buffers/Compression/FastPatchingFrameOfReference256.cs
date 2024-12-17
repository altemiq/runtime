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
/// <para>For sufficiently compressible and long arrays, it is faster and better than other PFOR schemes.</para>
/// <para>Note that this does not use differential coding: if you are working on sorted lists, use <see cref="Differential"/> instead.</para>
/// <para>For multi-threaded applications, each thread should use its own <see cref="FastPatchingFrameOfReference256"/> object.</para>
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

    // Working area for compress and uncompress.
    private readonly int[] dataPointers = new int[33];
    private readonly int[] freqs = new int[33];
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
    /// <param name="pagesize">The page size.</param>
    public FastPatchingFrameOfReference256(int pagesize)
    {
        this.pageSize = pagesize;

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

        var outlength = source[sourceIndex];
        sourceIndex++;
        this.HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, length, outlength);
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => this.HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, length, number);

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

        void EncodePage(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
        {
            var headerIndex = destinationIndex;
            destinationIndex++;
            var temporaryDestinationIndex = destinationIndex;

            // Clear working area.
            System.Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            this.byteContainer.Clear();

            var temporarySourceIndex = sourceIndex;
            for (var finalSourceIndex = temporarySourceIndex + length - BlockSize; temporarySourceIndex <= finalSourceIndex; temporarySourceIndex += BlockSize)
            {
                GetBestBFromData(source, temporarySourceIndex);

                var temporaryBestBit = this.bestB;
                this.byteContainer.WriteSByte((sbyte)this.bestB);
                this.byteContainer.WriteSByte((sbyte)this.bestExcept);
                if (this.bestExcept > 0)
                {
                    this.byteContainer.WriteSByte((sbyte)this.maxB);

                    var index = this.maxB - this.bestB;
                    if (this.dataPointers[index] + this.bestExcept >= this.dataTobePacked[index].Length)
                    {
                        var newsize = 2 * (this.dataPointers[index] + this.bestExcept);

                        // make sure it is a multiple of 32
                        newsize = Util.GreatestMultiple(newsize + 31, 32);
                        var temp = this.dataTobePacked[index];
                        System.Array.Resize(ref temp, newsize);
                        this.dataTobePacked[index] = temp;
                    }

                    for (var k = 0; k < BlockSize; k++)
                    {
                        if ((int)((uint)source[k + temporarySourceIndex] >> this.bestB) is not 0)
                        {
                            // we have an exception
                            this.byteContainer.WriteSByte((sbyte)k);
                            this.dataTobePacked[index][this.dataPointers[index]++] = (int)((uint)source[k + temporarySourceIndex] >> temporaryBestBit);
                        }
                    }
                }

                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(source.AsSpan(temporarySourceIndex + k, 32), destination.AsSpan(temporaryDestinationIndex, 32), temporaryBestBit);
                    temporaryDestinationIndex += temporaryBestBit;
                }
            }

            sourceIndex = temporarySourceIndex;
            destination[headerIndex] = temporaryDestinationIndex - headerIndex;

            var bytesize = (int)this.byteContainer.Position;
            while ((this.byteContainer.Position & 3) is not 0)
            {
                this.byteContainer.WriteByte(0);
            }

            destination[temporaryDestinationIndex++] = bytesize;

            var count = (int)this.byteContainer.Position / 4;
            this.byteContainer.Position = 0;
            _ = this.byteContainer.Read(destination, temporaryDestinationIndex, count, ByteOrder.LittleEndian);
            temporaryDestinationIndex += count;
            var bitmap = 0;
            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is not 0)
                {
                    bitmap |= 1 << (k - 1);
                }
            }

            destination[temporaryDestinationIndex++] = bitmap;

            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is not 0)
                {
                    destination[temporaryDestinationIndex++] = this.dataPointers[k]; // size
                    var j = 0;
                    for (; j < this.dataPointers[k]; j += 32)
                    {
                        BitPacking.Pack(this.dataTobePacked[k].AsSpan(j), destination.AsSpan(temporaryDestinationIndex), k);
                        temporaryDestinationIndex += k;
                    }

                    var overflow = j - this.dataPointers[k];
                    temporaryDestinationIndex -= overflow * k / 32;
                }
            }

            destinationIndex = temporaryDestinationIndex;

            void GetBestBFromData(int[] source, int pos)
            {
                System.Array.Clear(this.freqs, 0, this.freqs.Length);
                for (int k = pos, k_end = pos + BlockSize; k < k_end; k++)
                {
                    this.freqs[Util.Bits(source[k])]++;
                }

                this.bestB = 32;
                while (this.freqs[this.bestB] is 0)
                {
                    this.bestB--;
                }

                this.maxB = this.bestB;
                var bestcost = this.bestB * BlockSize;
                var cexcept = 0;
                this.bestExcept = cexcept;
                for (var b = this.bestB - 1; b >= 0; --b)
                {
                    cexcept += this.freqs[b + 1];
                    if (cexcept == BlockSize)
                    {
                        break;
                    }

                    // the extra 8 is the cost of storing maxbits
                    var cost = (cexcept * OverheadOfEachExcept) + (cexcept * (this.maxB - b)) + (b * BlockSize) + 8;
                    if (this.maxB - b == 1)
                    {
                        cost -= cexcept;
                    }

                    if (cost < bestcost)
                    {
                        bestcost = cost;
                        this.bestB = b;
                        this.bestExcept = cexcept;
                    }
                }
            }
        }
    }

    private void HeadlessUncompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number)
    {
        if (length is 0)
        {
            return;
        }

        number = Util.GreatestMultiple(number, BlockSize);
        var finalout = destinationIndex + number;
        while (destinationIndex != finalout)
        {
            var thissize = Math.Min(
                this.pageSize,
                finalout - destinationIndex);
            DecodePage(source, ref sourceIndex, destination, ref destinationIndex, thissize);
        }

        void DecodePage(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int size)
        {
            var initialSourceIndex = sourceIndex;

            var metaLocation = source[sourceIndex];
            sourceIndex++;
            var exceptIndex = initialSourceIndex + metaLocation;

            var byteSize = source[exceptIndex++];
            this.byteContainer.Clear();
            this.byteContainer.Write(source, exceptIndex, (byteSize + 3) / 4);
            this.byteContainer.Position = 0;
            exceptIndex += (byteSize + 3) / 4;

            var bitmap = source[exceptIndex++];
            for (var k = 2; k <= 32; k++)
            {
                if ((bitmap & (1 << (k - 1))) is not 0)
                {
                    var currentSize = source[exceptIndex++];
                    var roundedup = Util.GreatestMultiple(currentSize + 31, 32);
                    if (this.dataTobePacked[k].Length < roundedup)
                    {
                        this.dataTobePacked[k] = new int[roundedup];
                    }

                    if (exceptIndex + (roundedup / 32 * k) <= source.Length)
                    {
                        var j = 0;
                        for (; j < currentSize; j += 32)
                        {
                            BitPacking.Unpack(source.AsSpan(exceptIndex), this.dataTobePacked[k].AsSpan(j), k);
                            exceptIndex += k;
                        }

                        var overflow = j - currentSize;
                        exceptIndex -= overflow * k / 32;
                    }
                    else
                    {
                        var buf = new int[roundedup / 32 * k];
                        var initialInexcept = exceptIndex;
                        System.Array.Copy(source, exceptIndex, buf, 0, source.Length - exceptIndex);

                        var j = 0;
                        for (; j < currentSize; j += 32)
                        {
                            BitPacking.Unpack(buf.AsSpan(exceptIndex - initialInexcept), this.dataTobePacked[k].AsSpan(j), k);
                            exceptIndex += k;
                        }

                        var overflow = j - currentSize;
                        exceptIndex -= overflow * k / 32;
                    }
                }
            }

            System.Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            var temporaryDestinationIndex = destinationIndex;
            var temporarySourceIndex = sourceIndex;

            var runEnd = size / BlockSize;
            for (var run = 0; run < runEnd; run++, temporaryDestinationIndex += BlockSize)
            {
                int b = this.byteContainer.ReadSByte();

                var currentExcept = this.byteContainer.ReadSByte() & 0xFF;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(temporaryDestinationIndex + k), b);
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
                            destination[position + temporaryDestinationIndex] |= 1 << b;
                        }
                    }
                    else
                    {
                        for (var k = 0; k < currentExcept; k++)
                        {
                            var position = this.byteContainer.ReadSByte() & 0xFF;

                            var exceptValue = this.dataTobePacked[index][this.dataPointers[index]++];
                            destination[position + temporaryDestinationIndex] |= exceptValue << b;
                        }
                    }
                }
            }

            destinationIndex = temporaryDestinationIndex;
            sourceIndex = exceptIndex;
        }
    }
}