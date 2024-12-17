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

    // Working area for compress and uncompress.
    private readonly int[] dataPointers = new int[33];
    private readonly int[] freqs = new int[33];

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
    /// <param name="pagesize">The page size.</param>
    public FastPatchingFrameOfReference128(int pagesize)
    {
        this.pageSize = pagesize;

        // Initiate arrrays.
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
    public override string ToString() => nameof(FastPatchingFrameOfReference128);

    private void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length = Util.GreatestMultiple(length, BlockSize);

        // Allocate memory for working area.
        var finalSourceIndex = sourceIndex + length;
        while (sourceIndex != finalSourceIndex)
        {
            var thissize = Math.Min(this.pageSize, finalSourceIndex - sourceIndex);
            EncodePage(source, ref sourceIndex, thissize, destination, ref destinationIndex);
        }

        void EncodePage(int[] source, ref int sourceIndex, int thissize, int[] destination, ref int destinationIndex)
        {
            var headerpos = destinationIndex;
            destinationIndex++;
            var temporaryDestinationIndex = destinationIndex;

            // Clear working area.
            System.Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            this.byteContainer.Clear();

            var temporarySourceIndex = sourceIndex;
            for (var finalSourceIndex = temporarySourceIndex + thissize - BlockSize; temporarySourceIndex <= finalSourceIndex; temporarySourceIndex += BlockSize)
            {
                GetBestBFromData(source, temporarySourceIndex);

                var tmpbestb = this.bestB;
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
                            this.dataTobePacked[index][this.dataPointers[index]++] = (int)((uint)source[k + temporarySourceIndex] >> tmpbestb);
                        }
                    }
                }

                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(source.AsSpan(temporarySourceIndex + k), destination.AsSpan(temporaryDestinationIndex), tmpbestb);
                    temporaryDestinationIndex += tmpbestb;
                }
            }

            sourceIndex = temporarySourceIndex;
            destination[headerpos] = temporaryDestinationIndex - headerpos;

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
                    var thiscost = (cexcept * OverheadOfEachExcept)
                                   + (cexcept * (this.maxB - b)) + (b
                                   * BlockSize) + 8;
                    if (this.maxB - b == 1)
                    {
                        thiscost -= cexcept;
                    }

                    if (thiscost < bestcost)
                    {
                        bestcost = thiscost;
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
        var finalDestinationIndex = destinationIndex + number;
        while (destinationIndex != finalDestinationIndex)
        {
            DecodePage(source, ref sourceIndex, destination, ref destinationIndex, Math.Min(this.pageSize, finalDestinationIndex - destinationIndex));
        }

        void DecodePage(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int size)
        {
            var initialPosition = sourceIndex;

            var metaIndex = source[sourceIndex];
            sourceIndex++;
            var exceptIndex = initialPosition + metaIndex;

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
                        var buffer = new int[roundedup / 32 * k];
                        var initialExceptIndex = exceptIndex;
                        System.Array.Copy(source, exceptIndex, buffer, 0, source.Length - exceptIndex);

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

            System.Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            var temporaryDestinationIndex = destinationIndex;
            var temporarySourceIndex = sourceIndex;

            var runEnd = size / BlockSize;
            for (var run = 0; run < runEnd; run++, temporaryDestinationIndex += BlockSize)
            {
                int b = this.byteContainer.ReadSByte();

                var currentExceptions = this.byteContainer.ReadSByte() & 0xFF;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(temporaryDestinationIndex + k), b);
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
                            destination[pos + temporaryDestinationIndex] |= 1 << b;
                        }
                    }
                    else
                    {
                        for (var k = 0; k < currentExceptions; k++)
                        {
                            var pos = this.byteContainer.ReadSByte() & 0xFF;

                            destination[pos + temporaryDestinationIndex] |= this.dataTobePacked[index][this.dataPointers[index]++] << b;
                        }
                    }
                }
            }

            destinationIndex = temporaryDestinationIndex;
            sourceIndex = exceptIndex;
        }
    }
}