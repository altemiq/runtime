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
internal sealed class FastPatchingFrameOfReference256 : IInt32Codec, IHeadlessInt32Codec, IDisposable
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
    public override string ToString() => nameof(FastPatchingFrameOfReference256);

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
            var written = 1;

            // Clear working area.
            Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            this.byteContainer.Clear();

            var read = 0;
            for (var index = size - BlockSize; read <= index; read += BlockSize)
            {
                GetBestBFromData(sourcePage[read..]);

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
                        Array.Resize(ref temp, newSize);
                        this.dataTobePacked[dataIndex] = temp;
                    }

                    for (var k = 0; k < BlockSize; k++)
                    {
                        if (sourcePage[k + read] >>> this.bestB is not 0)
                        {
                            // we have an exception
                            this.byteContainer.WriteSByte((sbyte)k);
                            this.dataTobePacked[dataIndex][this.dataPointers[dataIndex]++] = sourcePage[k + read] >>> temporaryBestBit;
                        }
                    }
                }

                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(sourcePage.Slice(read + k, 32), destinationPage.Slice(written, 32), temporaryBestBit);
                    written += temporaryBestBit;
                }
            }

            destinationPage[0] = written;

            var byteSize = (int)this.byteContainer.Position;
            while ((this.byteContainer.Position & 3) is not 0)
            {
                this.byteContainer.WriteByte(0);
            }

            destinationPage[written++] = byteSize;

            var count = (int)this.byteContainer.Position / 4;
            this.byteContainer.Position = 0;
            _ = this.byteContainer.Read(destinationPage.Slice(written, count), ByteOrder.LittleEndian);
            written += count;
            var bitmap = 0;
            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is not 0)
                {
                    bitmap |= 1 << (k - 1);
                }
            }

            destinationPage[written++] = bitmap;

            for (var k = 2; k <= 32; k++)
            {
                if (this.dataPointers[k] is 0)
                {
                    continue;
                }

                destinationPage[written++] = this.dataPointers[k]; // size
                var j = 0;
                for (; j < this.dataPointers[k]; j += 32)
                {
                    BitPacking.Pack(this.dataTobePacked[k].AsSpan(j), destinationPage[written..], k);
                    written += k;
                }

                var overflow = j - this.dataPointers[k];
                written -= overflow * k / 32;
            }

            return (read, written);

            void GetBestBFromData(ReadOnlySpan<int> input)
            {
                Array.Clear(this.frequencies, 0, this.frequencies.Length);
                int end = BlockSize;
                for (var k = 0; k < end; k++)
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

    private (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var number = Util.GreatestMultiple(destination.Length, BlockSize);
        var finalOutput = number;
        var totalRead = 0;
        var totalWritten = 0;
        while (totalWritten != finalOutput)
        {
            var thisSize = Math.Min(
                this.pageSize,
                finalOutput - totalWritten);
            var (read, written) = DecodePage(source[totalRead..], destination[totalWritten..], thisSize);
            totalRead += read;
            totalWritten += written;
        }

        return (totalRead, totalWritten);

        (int Read, int Written) DecodePage(ReadOnlySpan<int> sourcePage, Span<int> destinationPage, int size)
        {
            var sourcePageIndex = 0;

            var metaLocation = sourcePage[sourcePageIndex];
            sourcePageIndex++;
            var exceptIndex = metaLocation;

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
                        var buf = new int[roundedUp / 32 * k];
                        var initialExcept = exceptIndex;
                        sourcePage[exceptIndex..].CopyTo(buf);

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

            Array.Clear(this.dataPointers, 0, this.dataPointers.Length);
            var destinationIndex = 0;
            var temporarySourceIndex = sourcePageIndex;

            var runEnd = size / BlockSize;
            for (var run = 0; run < runEnd; run++, destinationIndex += BlockSize)
            {
                int b = this.byteContainer.ReadSByte();

                var currentExcept = this.byteContainer.ReadSByte() & 0xFF;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(sourcePage[temporarySourceIndex..], destinationPage[(destinationIndex + k)..], b);
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
                            destinationPage[position + destinationIndex] |= 1 << b;
                        }
                    }
                    else
                    {
                        for (var k = 0; k < currentExcept; k++)
                        {
                            var position = this.byteContainer.ReadSByte() & 0xFF;

                            var exceptValue = this.dataTobePacked[index][this.dataPointers[index]++];
                            destinationPage[position + destinationIndex] |= exceptValue << b;
                        }
                    }
                }
            }

            return (exceptIndex, destinationIndex);
        }
    }
}