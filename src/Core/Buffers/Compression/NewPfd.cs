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

    private readonly Compress compress = compress;

    private readonly Decompress decompress = decompress;

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
    void IHeadlessInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => this.HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var destinationLength = source[sourceIndex];
        sourceIndex++;
        this.HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, length, destinationLength);
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => this.HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, length, number);

    /// <inheritdoc/>
    public override string ToString() => nameof(NewPfd);

    private void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length = Util.GreatestMultiple(length, BlockSize);
        if (length is 0)
        {
            return;
        }

        EncodePage(source, ref sourceIndex, destination, ref destinationIndex, length);

        void EncodePage(int[] sourcePage, ref int sourcePageIndex, int[] destinationPage, ref int destinationPageIndex, int size)
        {
            var temporaryDestinationIndex = destinationPageIndex;
            var temporarySourceIndex = sourcePageIndex;
            for (var finalSourceIndex = temporarySourceIndex + size; temporarySourceIndex + BlockSize <= finalSourceIndex; temporarySourceIndex += BlockSize)
            {
                var (bestBit, bestException) = GetBestBit(sourcePage, temporarySourceIndex);
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

                    exceptionSize = this.compress(this.exceptionBuffer, 0, destinationPage, temporaryDestinationIndex, 2 * bestException);
                    temporaryDestinationIndex += exceptionSize;
                }

                destinationPage[remember] = bestBit | bestException << 8 | exceptionSize << 16;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(sourcePage.AsSpan(temporarySourceIndex + k), destinationPage.AsSpan(temporaryDestinationIndex), Bits[bestBit]);
                    temporaryDestinationIndex += Bits[bestBit];
                }
            }

            sourcePageIndex = temporarySourceIndex;
            destinationPageIndex = temporaryDestinationIndex;

            static (int BestB, int BestExcept) GetBestBit(int[] source, int pos)
            {
                var maxBits = Util.MaxBits(source, pos, BlockSize);
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
                    for (var k = pos; k < BlockSize + pos; k++)
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

    private void HeadlessDecompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number)
    {
        if (length is 0)
        {
            return;
        }

        DecodePage(source, ref sourceIndex, destination, ref destinationIndex, Util.GreatestMultiple(number, BlockSize));

        void DecodePage(int[] sourcePage, ref int sourcePageIndex, int[] destinationPage, ref int destinationPageIndex, int size)
        {
            var temporaryDestinationIndex = destinationPageIndex;
            var temporarySourceIndex = sourcePageIndex;

            var runEnd = size / BlockSize;
            for (var run = 0; run < runEnd; run++, temporaryDestinationIndex += BlockSize)
            {
                var b = sourcePage[temporarySourceIndex] & 0xFF;
                var exceptionCount = sourcePage[temporarySourceIndex] >>> 8 & 0xFF;
                var exceptionSize = sourcePage[temporarySourceIndex] >>> 16;
                temporarySourceIndex++;
                this.decompress(sourcePage, temporarySourceIndex, exceptionSize, this.exceptionBuffer, 0, 2 * exceptionCount);
                temporarySourceIndex += exceptionSize;
                var bit = Bits[b];
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(sourcePage.AsSpan(temporarySourceIndex), destinationPage.AsSpan(temporaryDestinationIndex + k), bit);
                    temporarySourceIndex += bit;
                }

                for (var k = 0; k < exceptionCount; k++)
                {
                    destinationPage[temporaryDestinationIndex + this.exceptionBuffer[k + exceptionCount]] |= this.exceptionBuffer[k] << bit;
                }
            }

            destinationPageIndex = temporaryDestinationIndex;
            sourcePageIndex = temporarySourceIndex;
        }
    }
}