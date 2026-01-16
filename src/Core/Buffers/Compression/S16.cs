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

    private static readonly int[][] ShiftedS16Bits = [.. S16Bits.Select(static x => x.Select(x => 1 << x).ToArray())];

    /// <inheritdoc cref="Compression.Compress" />
    public static int Compress(ReadOnlySpan<int> source, Span<int> destination, int length)
    {
        var sourceIndex = 0;
        var destinationIndex = 0;
        var endSourceIndex = length;
        while (sourceIndex < endSourceIndex)
        {
            var offset = CompressBlock(source.Slice(sourceIndex, length), destination[destinationIndex..]);
            if (offset is -1)
            {
                throw new InvalidDataException("Too big a number");
            }

            destinationIndex++;
            sourceIndex += offset;
            length -= offset;
        }

        return destinationIndex;

        static int CompressBlock(ReadOnlySpan<int> source, Span<int> destination)
        {
            for (var i = 0; i < S16NumSize; i++)
            {
                destination[0] = i << S16BitsSize;
                var offset = Math.Min(S16Num[i], source.Length);

                var bits = 0;
                int j;
                for (j = 0; (j < offset) && (source[j] < ShiftedS16Bits[i][j]); j++)
                {
                    destination[0] |= source[j] << bits;
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
    public static void Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var sourceIndex = 0;
        var length = source.Length;
        var destinationIndex = 0;
        var destinationLength = destination.Length;
        while (sourceIndex < length)
        {
            var count = DecompressBlock(source[sourceIndex..], destination.Slice(destinationIndex, destinationLength));
            destinationLength -= count;
            destinationIndex += count;
            sourceIndex++;
        }

        static int DecompressBlock(ReadOnlySpan<int> source, Span<int> destination)
        {
            var length = destination.Length;
            var index = source[0] >>> S16BitsSize;
            var count = Math.Min(S16Num[index], length);
            var bits = 0;
            for (var j = 0; j < count; j++)
            {
                destination[j] = source[0] >>> bits & (int)(0xffffffff >> (32 - S16Bits[index][j]));
                bits += S16Bits[index][j];
            }

            return count;
        }
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(S16);
}