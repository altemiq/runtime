// -----------------------------------------------------------------------
// <copyright file="NewPfd.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// NewPFD: fast patching scheme by Yan et al.
/// </summary>
/// <param name="compress">The compression delegate.</param>
/// <param name="decompress">The decompression delegate.</param>
internal abstract class NewPfd(Compress compress, Decompress decompress) : IInt32Codec, IHeadlessInt32Codec
{
    private const int BlockSize = 128;

    private static readonly int[] Bits = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16, 20, 32];

    private static readonly int[] InverseBits = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16];

    private readonly int[] exceptionBuffer = new int[2 * BlockSize];

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
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => this.HeadlessCompress(source, destination);

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
    public override string ToString() => nameof(NewPfd);

    private (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = Util.GreatestMultiple(source.Length, BlockSize);
        return length is 0 ? default : EncodePage(source[..length], destination);

        (int Read, int Written) EncodePage(ReadOnlySpan<int> sourcePage, Span<int> destinationPage)
        {
            var size = sourcePage.Length;
            var temporaryDestinationIndex = 0;
            var temporarySourceIndex = 0;
            for (; temporarySourceIndex + BlockSize <= size; temporarySourceIndex += BlockSize)
            {
                var (bestBit, bestException) = GetBestBit(sourcePage[temporarySourceIndex..]);
                var exceptionSize = 0;
                var remember = temporaryDestinationIndex;
                temporaryDestinationIndex++;
                if (bestException > 0)
                {
                    var c = 0;
                    for (var i = 0; i < BlockSize; i++)
                    {
                        if (sourcePage[temporarySourceIndex + i] >>> Bits[bestBit] is not 0)
                        {
                            this.exceptionBuffer[c + bestException] = i;
                            this.exceptionBuffer[c] = sourcePage[temporarySourceIndex + i] >>> Bits[bestBit];
                            c++;
                        }
                    }

                    exceptionSize = compress(this.exceptionBuffer, destinationPage[temporaryDestinationIndex..], 2 * bestException);
                    temporaryDestinationIndex += exceptionSize;
                }

                destinationPage[remember] = bestBit | bestException << 8 | exceptionSize << 16;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(sourcePage[(temporarySourceIndex + k)..], destinationPage[temporaryDestinationIndex..], Bits[bestBit]);
                    temporaryDestinationIndex += Bits[bestBit];
                }
            }

            return (temporarySourceIndex, temporaryDestinationIndex);

            static (int BestB, int BestExcept) GetBestBit(ReadOnlySpan<int> source)
            {
                var maxBits = Util.MaxBits(source[..BlockSize]);
                var minimum = 0;
                if (Bits[InverseBits[maxBits]] >= 28)
                {
                    minimum = Bits[InverseBits[maxBits]] - 28; // 28 is the max for
                }

                // exceptions
                var best = Bits.Length - 1;
                var exceptionCounter = 0;
                for (var i = minimum; i < Bits.Length - 1; i++)
                {
                    var counter = 0;
                    for (var k = 0; k < BlockSize; k++)
                    {
                        if (source[k] >>> Bits[i] is not 0)
                        {
                            counter++;
                        }
                    }

                    if (counter * 10 <= BlockSize)
                    {
                        best = i;
                        exceptionCounter = counter;
                        break;
                    }
                }

                return (best, exceptionCounter);
            }
        }
    }

    private (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        return source.Length is 0 ? default : DecodePage(source, destination[..Util.GreatestMultiple(destination.Length, BlockSize)]);

        (int Read, int Written) DecodePage(ReadOnlySpan<int> sourcePage, Span<int> destinationPage)
        {
            var written = 0;
            var read = 0;

            var runEnd = destinationPage.Length / BlockSize;
            for (var run = 0; run < runEnd; run++, written += BlockSize)
            {
                var b = sourcePage[read] & 0xFF;
                var exceptionCount = sourcePage[read] >>> 8 & 0xFF;
                var exceptionSize = sourcePage[read] >>> 16;
                read++;
                decompress(sourcePage.Slice(read, exceptionSize), this.exceptionBuffer.AsSpan(0, 2 * exceptionCount));
                read += exceptionSize;
                var bit = Bits[b];
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(sourcePage[read..], destinationPage[(written + k)..], bit);
                    read += bit;
                }

                for (var k = 0; k < exceptionCount; k++)
                {
                    destinationPage[written + this.exceptionBuffer[k + exceptionCount]] |= this.exceptionBuffer[k] << bit;
                }
            }

            return (read, written);
        }
    }
}