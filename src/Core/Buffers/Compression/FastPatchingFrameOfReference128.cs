// -----------------------------------------------------------------------
// <copyright file="FastPatchingFrameOfReference128.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// This class is similar to <see cref="FastPatchingFrameOfReference256"/> but uses a small block size.
/// </summary>
internal sealed class FastPatchingFrameOfReference128 : IInt32Codec, IHeadlessInt32Codec, IDisposable
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
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => this.HeadlessCompress(source, destination);

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = Util.GreatestMultiple(source.Length, BlockSize);
        if (length is 0)
        {
            return default;
        }

        destination[0] = length;
        var (read, written) = this.HeadlessCompress(source[..length], destination[1..]);
        return (read, written + 1);
    }

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var (read, written) = this.HeadlessDecompress(source[1..], destination[..source[0]]);
        return (read + 1, written);
    }

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination) => this.HeadlessDecompress(source, destination);

    /// <inheritdoc/>
    public override string ToString() => nameof(FastPatchingFrameOfReference128);

    /// <inheritdoc/>
    public void Dispose() => this.byteContainer.Dispose();

    private (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = Util.GreatestMultiple(source.Length, BlockSize);

        // Allocate memory for working area.
        var totalRead = 0;
        var totalWritten = 0;
        while (totalRead != length)
        {
            var (read, written) = EncodePage(source[totalRead..], destination[totalWritten..], Math.Min(this.pageSize, length - totalRead));
            totalRead += read;
            totalWritten += written;
        }

        return (totalRead, totalWritten);

        (int Read, int Written) EncodePage(ReadOnlySpan<int> sourcePage, Span<int> destinationPage, int size)
        {
            var destinationPageIndex = 1;

            // Clear working area.
            Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            this.byteContainer.Clear();

            var sourcePageIndex = 0;
            for (var index = size - BlockSize; sourcePageIndex <= index; sourcePageIndex += BlockSize)
            {
                GetBestBFromData(sourcePage[sourcePageIndex..]);

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
                        if (sourcePage[k + sourcePageIndex] >>> this.bestB is 0)
                        {
                            continue;
                        }

                        // we have an exception
                        this.byteContainer.WriteSByte((sbyte)k);
                        this.dataTobePacked[dataIndex][this.dataPointers[dataIndex]++] = sourcePage[k + sourcePageIndex] >>> temporaryBestB;
                    }
                }

                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(sourcePage[(sourcePageIndex + k)..], destinationPage[destinationPageIndex..], temporaryBestB);
                    destinationPageIndex += temporaryBestB;
                }
            }

            destinationPage[0] = destinationPageIndex;

            var byteSize = (int)this.byteContainer.Position;
            while ((this.byteContainer.Position & 3) is not 0)
            {
                this.byteContainer.WriteByte(0);
            }

            destinationPage[destinationPageIndex++] = byteSize;

            var count = (int)this.byteContainer.Position / 4;
            this.byteContainer.Position = 0;
            _ = this.byteContainer.Read(destinationPage.Slice(destinationPageIndex, count), ByteOrder.LittleEndian);
            destinationPageIndex += count;
            var bitmap = 0;
            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is not 0)
                {
                    bitmap |= 1 << (k - 1);
                }
            }

            destinationPage[destinationPageIndex++] = bitmap;

            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is not 0)
                {
                    destinationPage[destinationPageIndex++] = this.dataPointers[k]; // size
                    var j = 0;
                    for (; j < this.dataPointers[k]; j += 32)
                    {
                        BitPacking.Pack(this.dataTobePacked[k].AsSpan(j), destinationPage[destinationPageIndex..], k);
                        destinationPageIndex += k;
                    }

                    var overflow = j - this.dataPointers[k];
                    destinationPageIndex -= overflow * k / 32;
                }
            }

            return (sourcePageIndex, destinationPageIndex);

            void GetBestBFromData(ReadOnlySpan<int> input)
            {
                Array.Clear(this.frequencies, 0, this.frequencies.Length);
                for (var k = 0; k < BlockSize; k++)
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

    private (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var totalWritten = 0;
        var totalRead = 0;
        var number = Util.GreatestMultiple(destination.Length, BlockSize);
        while (totalWritten != number)
        {
            var (read, written) = DecodePage(source[totalRead..], destination[totalWritten..], Math.Min(this.pageSize, number - totalWritten));
            totalRead += read;
            totalWritten += written;
        }

        return (totalRead, totalWritten);

        (int Read, int Written) DecodePage(ReadOnlySpan<int> sourcePage, Span<int> destinationPage, int size)
        {
            var sourcePageIndex = 0;

            var metaIndex = sourcePage[sourcePageIndex];
            sourcePageIndex++;
            var exceptIndex = metaIndex;

            var byteSize = sourcePage[exceptIndex++];
            this.byteContainer.Clear();
            this.byteContainer.Write(sourcePage.Slice(exceptIndex, (byteSize + 3) / 4));
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
                            BitPacking.Unpack(sourcePage[exceptIndex..], this.dataTobePacked[k].AsSpan(j), k);
                            exceptIndex += k;
                        }

                        var overflow = j - currentSize;
                        exceptIndex -= overflow * k / 32;
                    }
                    else
                    {
                        var buffer = new int[roundedUp / 32 * k];
                        var initialExceptIndex = exceptIndex;
                        sourcePage[exceptIndex..].CopyTo(buffer);

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
            var written = 0;
            var temporarySourceIndex = sourcePageIndex;

            var runEnd = size / BlockSize;
            for (var run = 0; run < runEnd; run++, written += BlockSize)
            {
                int b = this.byteContainer.ReadSByte();

                var currentExceptions = this.byteContainer.ReadSByte() & 0xFF;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(sourcePage[temporarySourceIndex..], destinationPage[(written + k)..], b);
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
                            destinationPage[pos + written] |= 1 << b;
                        }
                    }
                    else
                    {
                        for (var k = 0; k < currentExceptions; k++)
                        {
                            var pos = this.byteContainer.ReadSByte() & 0xFF;

                            destinationPage[pos + written] |= this.dataTobePacked[index][this.dataPointers[index]++] << b;
                        }
                    }
                }
            }

            return (exceptIndex, written);
        }
    }
}