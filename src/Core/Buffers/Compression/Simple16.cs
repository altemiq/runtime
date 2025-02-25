// -----------------------------------------------------------------------
// <copyright file="Simple16.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// This is an implementation of the popular Simple16 scheme. It is limited to 28-bit integers (between 0 and 2^28-1).
/// </summary>
internal sealed class Simple16 : IInt32Codec, IHeadlessInt32Codec
{
    private const int S16NumSize = 16;
    private const int S16BitsSize = 28;

    // the possible number of bits used to represent one integer
    private static readonly int[] S16Num = [28, 21, 21, 21, 14, 9, 8, 7, 6, 6, 5, 5, 4, 3, 2, 1];

    // the corresponding number of elements for each value of the number of bits
    private static readonly int[][] S16Bits =
    [
        [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1],
        [2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1],
        [1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 2],
        [2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2],
        [4, 3, 3, 3, 3, 3, 3, 3, 3],
        [3, 4, 4, 4, 4, 3, 3, 3],
        [4, 4, 4, 4, 4, 4, 4],
        [5, 5, 5, 5, 4, 4],
        [4, 4, 5, 5, 5, 5],
        [6, 6, 6, 5, 5],
        [5, 5, 6, 6, 6],
        [7, 7, 7, 7],
        [10, 9, 9,],
        [14, 14],
        [28],
    ];

    private static readonly int[][] ShiftedS16Bits = [.. S16Bits.Select(static x => x.Select(x => 1 << x).ToArray())];

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, number);

    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        destination[destinationIndex] = length;
        destinationIndex++;
        HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var destinationLength = source[sourceIndex];
        sourceIndex++;
        HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, destinationLength);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(Simple16);

    private static void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        var temporarySourceIndex = sourceIndex;
        var temporaryDestinationIndex = destinationIndex;
        var endStartIndex = temporarySourceIndex + length;
        while (temporarySourceIndex < endStartIndex)
        {
            var offset = Compressblock(source, temporarySourceIndex, destination, temporaryDestinationIndex++, length);
            if (offset is -1)
            {
                throw new InvalidDataException("Too big a number");
            }

            temporarySourceIndex += offset;
            length -= offset;
        }

        sourceIndex = temporarySourceIndex;
        destinationIndex = temporaryDestinationIndex;

        static int Compressblock(int[] source, int sourceIndex, int[] destination, int destinationIndex, int length)
        {
            for (var i = 0; i < S16NumSize; i++)
            {
                destination[destinationIndex] = i << S16BitsSize;
                var offset = Math.Min(S16Num[i], length);

                var bits = 0;
                int j;
                for (j = 0; (j < offset) && (source[sourceIndex + j] < ShiftedS16Bits[i][j]); j++)
                {
                    destination[destinationIndex] |= source[sourceIndex + j] << bits;
                    bits += S16Bits[i][j];
                }

                if (j == offset)
                {
                    return offset;
                }
            }

            return -1;
        }
    }

    private static void HeadlessUncompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int number)
    {
        var temporarySourceIndex = sourceIndex;
        var temporaryDestinationIndex = destinationIndex;
        while (number > 0)
        {
            var count = DecompressBlock(source, temporarySourceIndex, destination, temporaryDestinationIndex, number);
            number -= count;
            temporaryDestinationIndex += count;
            temporarySourceIndex++;
        }

        sourceIndex = temporarySourceIndex;
        destinationIndex = temporaryDestinationIndex;

        static int DecompressBlock(int[] source, int sourceIndex, int[] destination, int destinationIndex, int length)
        {
            var index = (int)((uint)source[sourceIndex] >> S16BitsSize);
            var count = Math.Min(S16Num[index], length);
            var bits = 0;
            for (var j = 0; j < count; j++)
            {
                destination[destinationIndex + j] = (int)((uint)source[sourceIndex] >> bits) & (int)(0xffffffff >> (32 - S16Bits[index][j]));
                bits += S16Bits[index][j];
            }

            return count;
        }
    }
}