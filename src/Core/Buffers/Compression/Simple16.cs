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
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessCompress(source, destination);

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessDecompress(source, destination);

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        destination[0] = source.Length;
        var (read, written) = HeadlessCompress(source, destination[1..]);
        return (read, written + 1);
    }

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var (read, written) = HeadlessDecompress(source[1..], destination[..source[0]]);
        return (read + 1, written);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(Simple16);

    private static (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = source.Length;
        var read = 0;
        var written = 0;
        while (read < length)
        {
            var offset = CompressBlock(source[read..], destination[written..]);
            if (offset is -1)
            {
                throw new InvalidDataException("Too big a number");
            }

            written++;
            read += offset;
        }

        return (read, written);

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

    private static (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var temporarySourceIndex = 0;
        var temporaryDestinationIndex = 0;
        var number = destination.Length;
        while (number > 0)
        {
            var count = DecompressBlock(source[temporarySourceIndex..], destination[temporaryDestinationIndex..]);
            number -= count;
            temporaryDestinationIndex += count;
            temporarySourceIndex++;
        }

        return (temporarySourceIndex, temporaryDestinationIndex);

        static int DecompressBlock(ReadOnlySpan<int> source, Span<int> destination)
        {
            var index = source[0] >>> S16BitsSize;
            var count = Math.Min(S16Num[index], destination.Length);
            var bits = 0;
            for (var j = 0; j < count; j++)
            {
                destination[j] = source[0] >>> bits & (int)(0xffffffff >> (32 - S16Bits[index][j]));
                bits += S16Bits[index][j];
            }

            return count;
        }
    }
}