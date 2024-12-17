// -----------------------------------------------------------------------
// <copyright file="NewPfd.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// NewPFD/NewPFOR: fast patching scheme by Yan et al.
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
        this.HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, length, destinationLength);
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => this.HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, length, number);

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

        void EncodePage(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
        {
            var temporaryDestinationIndex = destinationIndex;
            var temporarySourceIndex = sourceIndex;
            for (var finalSourceIndex = temporarySourceIndex + length; temporarySourceIndex + BlockSize <= finalSourceIndex; temporarySourceIndex += BlockSize)
            {
                var (bestBit, bestException) = GetBestBit(source, temporarySourceIndex);
                var exceptionSize = 0;
                var remember = temporaryDestinationIndex;
                temporaryDestinationIndex++;
                if (bestException > 0)
                {
                    var c = 0;
                    for (var i = 0; i < BlockSize; i++)
                    {
                        if ((int)((uint)source[temporarySourceIndex + i] >> Bits[bestBit]) is not 0)
                        {
                            this.exceptionBuffer[c + bestException] = i;
                            this.exceptionBuffer[c] = (int)((uint)source[temporarySourceIndex + i] >> Bits[bestBit]);
                            c++;
                        }
                    }

                    exceptionSize = this.compress(this.exceptionBuffer, 0, destination, temporaryDestinationIndex, 2 * bestException);
                    temporaryDestinationIndex += exceptionSize;
                }

                destination[remember] = bestBit | bestException << 8 | exceptionSize << 16;
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Pack(source.AsSpan(temporarySourceIndex + k), destination.AsSpan(temporaryDestinationIndex), Bits[bestBit]);
                    temporaryDestinationIndex += Bits[bestBit];
                }
            }

            sourceIndex = temporarySourceIndex;
            destinationIndex = temporaryDestinationIndex;

            static (int BestB, int BestExcept) GetBestBit(int[] source, int pos)
            {
                var maxBits = Util.MaxBits(source, pos, BlockSize);
                var minimum = 0;
                if (minimum + 28 < Bits[InverseBits[maxBits]])
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
                        if ((int)((uint)source[k] >> Bits[i]) is not 0)
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

    private void HeadlessUncompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number)
    {
        if (length is 0)
        {
            return;
        }

        DecodePage(source, ref sourceIndex, destination, ref destinationIndex, Util.GreatestMultiple(number, BlockSize));

        void DecodePage(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
        {
            var temporaryDestinationIndex = destinationIndex;
            var temporarySourceIndex = sourceIndex;

            var runEnd = length / BlockSize;
            for (var run = 0; run < runEnd; run++, temporaryDestinationIndex += BlockSize)
            {
                var b = source[temporarySourceIndex] & 0xFF;
                var exceptionCount = (int)((uint)source[temporarySourceIndex] >> 8) & 0xFF;
                var exceptionSize = (int)((uint)source[temporarySourceIndex] >> 16);
                temporarySourceIndex++;
                this.decompress(source, temporarySourceIndex, exceptionSize, this.exceptionBuffer, 0, 2 * exceptionCount);
                temporarySourceIndex += exceptionSize;
                var bit = Bits[b];
                for (var k = 0; k < BlockSize; k += 32)
                {
                    BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(temporaryDestinationIndex + k), bit);
                    temporarySourceIndex += bit;
                }

                for (var k = 0; k < exceptionCount; k++)
                {
                    destination[temporaryDestinationIndex + this.exceptionBuffer[k + exceptionCount]] |= this.exceptionBuffer[k] << bit;
                }
            }

            destinationIndex = temporaryDestinationIndex;
            sourceIndex = temporarySourceIndex;
        }
    }
}