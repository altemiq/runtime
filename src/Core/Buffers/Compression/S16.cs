// -----------------------------------------------------------------------
// <copyright file="S16.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Version of <see cref="Simple16"/> for <see cref="NewPfd"/> and <see cref="OptPfd"/>.
/// </summary>
internal sealed class S16
{
    private const int S16NumSize = 16;
    private const int S16BitsSize = 28;

    // the possible number of bits used to represent one integer
    private static readonly int[] S16Num = [28, 21, 21, 21, 14, 9, 8, 7, 6, 6, 5, 5, 4, 3, 2, 1];

    // the corresponding number of elements for each value of the number of
    // bits
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

    private static readonly int[][] ShiftedS16Bits = S16Bits.Select(x => x.Select(x => 1 << x).ToArray()).ToArray();

    /// <inheritdoc cref="Compression.Compress" />
    public static int Compress(int[] source, int sourceIndex, int[] destination, int destinationIndex, int length)
    {
        var temporaryDestinationIndex = destinationIndex;
        var endSourceIndex = sourceIndex + length;
        while (sourceIndex < endSourceIndex)
        {
            var offset = CompressBlock(destination, temporaryDestinationIndex++, source, sourceIndex, length);
            if (offset is -1)
            {
                throw new InvalidDataException("Too big a number");
            }

            sourceIndex += offset;
            length -= offset;
        }

        return temporaryDestinationIndex - destinationIndex;

        static int CompressBlock(int[] destination, int destinationIndex, int[] source, int sourceIndex, int length)
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

    /// <inheritdoc cref="Compression.EstimateCompress"/>
    public static int EstimateCompress(int[] source, int index, int length)
    {
        var endIndex = index + length;
        var counter = 0;
        while (index < endIndex)
        {
            var offset = FakeCompressBlock(source, index, length);
            if (offset is -1)
            {
                throw new InvalidDataException("Too big a number");
            }

            index += offset;
            length -= offset;
            counter++;
        }

        return counter;

        static int FakeCompressBlock(int[] source, int index, int length)
        {
            for (var i = 0; i < S16NumSize; i++)
            {
                var offset = Math.Min(S16Num[i], length);

                int j;
                for (j = 0; (j < offset) && (source[index + j] < ShiftedS16Bits[i][j]); j++)
                {
                    // this is to count the values
                }

                if (j == offset)
                {
                    return offset;
                }
            }

            return -1;
        }
    }

    /// <inheritdoc cref="Compression.Decompress" />
    public static void Uncompress(int[] source, int sourceIndex, int length, int[] destination, int destinationIndex, int destinationLength)
    {
        var endSourceIndex = sourceIndex + length;
        while (sourceIndex < endSourceIndex)
        {
            var count = DecompressBlock(source, sourceIndex, destination, destinationIndex, destinationLength);
            destinationLength -= count;
            destinationIndex += count;
            sourceIndex++;
        }

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

    /// <inheritdoc/>
    public override string ToString() => nameof(S16);
}