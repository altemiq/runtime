// -----------------------------------------------------------------------
// <copyright file="OptPfd.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// OptPFD: fast patching scheme by Yan et al.
/// </summary>
/// <param name="compress">The compression delegate.</param>
/// <param name="decompress">The decompression delegate.</param>
/// <param name="estimateCompress">The compression estimate delegate.</param>
internal abstract class OptPfd(Compress compress, Decompress decompress, EstimateCompress estimateCompress) : IInt32Codec, IHeadlessInt32Codec
{
    private const int BlockSize = 128;

    private static readonly int[] Bits = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16, 20, 32];

    private static readonly int[] InverseBits = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 14, 14, 15, 15, 15, 15, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16, 16];

    private readonly int[] exceptionBuffer = new int[2 * BlockSize];

    /// <inheritdoc/>
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => this.HeadlessCompress(source, destination);

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination) => this.HeadlessDecompress(source, destination);

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = source.Length / BlockSize * BlockSize;
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
    public override string ToString() => nameof(OptPfd);

    private (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = Util.GreatestMultiple(source.Length, BlockSize);
        return length is 0 ? default : EncodePage(source[..length], destination);

        (int Read, int Written) EncodePage(ReadOnlySpan<int> sourcePage, Span<int> destinationPage)
        {
            var size = sourcePage.Length;
            var written = 0;
            var read = 0;
            for (; read + BlockSize <= size; read += BlockSize)
            {
                var (bestBit, bestException) = GetBestBit(sourcePage[read..]);
                var exceptionSize = 0;
                var remember = written;
                written++;
                if (bestException > 0)
                {
                    var c = 0;
                    for (var i = 0; i < BlockSize; i++)
                    {
                        if (sourcePage[read + i] >>> Bits[bestBit] is not 0)
                        {
                            this.exceptionBuffer[c + bestException] = i;
                            this.exceptionBuffer[c] = sourcePage[read + i] >>> Bits[bestBit];
                            c++;
                        }
                    }

                    exceptionSize = compress(this.exceptionBuffer, destinationPage[written..], 2 * bestException);
                    written += exceptionSize;
                }

                destinationPage[remember] = bestBit | (bestException << 8) | (exceptionSize << 16);
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(sourcePage[(read + k)..], destinationPage[written..], Bits[bestBit]);
                    written += Bits[bestBit];
                }
            }

            return (read, written);

            (int BestB, int BestExcept) GetBestBit(ReadOnlySpan<int> input)
            {
                var maxBits = Util.MaxBits(input[..BlockSize]);
                var minimum = 0;
                if (Bits[InverseBits[maxBits]] >= 28)
                {
                    minimum = Bits[InverseBits[maxBits]] - 28; // 28 is the max for
                }

                // exceptions
                var best = Bits.Length - 1;
                var bestCost = Bits[best] * 4;
                var exceptionCounter = 0;
                for (var i = minimum; i < Bits.Length - 1; i++)
                {
                    var counter = 0;
                    for (var k = 0; k < BlockSize; k++)
                    {
                        if (input[k] >>> Bits[i] is not 0)
                        {
                            counter++;
                        }
                    }

                    if (counter == BlockSize)
                    {
                        continue; // no need
                    }

                    var c = 0;
                    for (var k = 0; k < BlockSize; k++)
                    {
                        if (input[k] >>> Bits[i] is not 0)
                        {
                            this.exceptionBuffer[counter + c] = k;
                            this.exceptionBuffer[c] = input[k] >>> Bits[i];
                            c++;
                        }
                    }

                    var cost = (Bits[i] * 4) + estimateCompress(this.exceptionBuffer, 0, 2 * counter);
                    if (cost <= bestCost)
                    {
                        bestCost = cost;
                        best = i;
                        exceptionCounter = counter;
                    }
                }

                return (best, exceptionCounter);
            }
        }
    }

    private (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        return source.Length is 0 ? default : DecodePage(source,  destination[..Util.GreatestMultiple(destination.Length, BlockSize)]);

        (int Read, int Written) DecodePage(ReadOnlySpan<int> sourcePage, Span<int> destinationPage)
        {
            var size = destinationPage.Length;
            var written = 0;
            var read = 0;

            var runEnd = size / BlockSize;
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