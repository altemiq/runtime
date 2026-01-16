// -----------------------------------------------------------------------
// <copyright file="Simple9.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// This is an implementation of the popular Simple9 scheme. It is limited to 28-bit integers (between 0 and 2^28-1).
/// </summary>
internal sealed class Simple9 : IInt32Codec, IHeadlessInt32Codec
{
    private static readonly int[] BitLength = [1, 2, 3, 4, 5, 7, 9, 14, 28];

    private static readonly int[] CodeNum = [28, 14, 9, 7, 5, 4, 3, 2, 1];

    /// <inheritdoc/>
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessCompress(source, destination);

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessUncompress(source, destination);

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

        var (read, written) = HeadlessUncompress(source[1..], destination[..source[0]]);
        return (read + 1, written);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(Simple9);

    private static (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var written = 0;
        var read = 0;
        var length = source.Length;

        while (read < length - 28)
        {
            if (!TryGetSelector(source, destination, ref read, ref written))
            {
                continue;
            }

            if (source[read] >= 1 << BitLength[8])
            {
                throw new InvalidDataException("Too big a number");
            }

            destination[written++] = source[read++] | (8 << 28);

            static bool TryGetSelector(ReadOnlySpan<int> source, Span<int> destination, ref int read, ref int written)
            {
                var selector = 0;
                while (selector < 8)
                {
                    var compressedNum = CodeNum[selector];
                    if (!Check(source.Slice(read, compressedNum), ref selector, out var res))
                    {
                        continue;
                    }

                    destination[written++] = res;
                    read += compressedNum;
                    return false;

                    static bool Check(ReadOnlySpan<int> source, ref int selector, out int res)
                    {
                        var b = BitLength[selector];
                        var max = 1 << b;
                        res = 0;
                        var compressedNum = source.Length;
                        for (var i = 0; i < compressedNum; i++)
                        {
                            if (max <= source[i])
                            {
                                selector++;
                                return false;
                            }

                            res = (res << b) + source[i];
                        }

                        res |= selector << 28;
                        return true;
                    }
                }

                return true;
            }
        }

        while (read < length)
        {
            if (!TryGetSelector(source, destination, ref written, ref read, length))
            {
                continue;
            }

            if (source[read] >= 1 << BitLength[8])
            {
                throw new InvalidDataException("Too big a number");
            }

            destination[written++] = source[read++] | (8 << 28);

            static bool TryGetSelector(ReadOnlySpan<int> source, Span<int> destination, ref int destinationIndex, ref int currentPos, int length)
            {
                var selector = 0;
                while (selector < 8)
                {
                    var compressedNum = CodeNum[selector];
                    if (length <= currentPos + compressedNum - 1)
                    {
                        compressedNum = length - currentPos;
                    }

                    if (!Check(source.Slice(currentPos, compressedNum), ref selector, out var res))
                    {
                        continue;
                    }

                    destination[destinationIndex++] = res;
                    currentPos += compressedNum;
                    return false;

                    static bool Check(ReadOnlySpan<int> source, ref int selector, out int res)
                    {
                        res = default;
                        var b = BitLength[selector];
                        var max = 1 << b;
                        var compressedNum = source.Length;
                        for (var i = 0; i < compressedNum; i++)
                        {
                            if (max <= source[i])
                            {
                                selector++;
                                return false;
                            }

                            res = (res << b) + source[i];
                        }

                        if (compressedNum != CodeNum[selector])
                        {
                            res <<= (CodeNum[selector] - compressedNum) * b;
                        }

                        res |= selector << 28;
                        return true;
                    }
                }

                return true;
            }
        }

        return (read, written);
    }

    private static (int Read, int Written) HeadlessUncompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var currentIndex = 0;
        var temporarySourceIndex = 0;
        var number = destination.Length;
        while (currentIndex < number - 28)
        {
            var value = source[temporarySourceIndex++];
            switch (value >>> 28)
            {
                case 0:
                    // number : 28, bitwidth : 1
                    destination[currentIndex++] = (int)((uint)(value << 4) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 5) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 6) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 7) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 8) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 9) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 10) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 11) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 12) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 13) >> 31); // 10
                    destination[currentIndex++] = (int)((uint)(value << 14) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 15) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 16) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 17) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 18) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 19) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 20) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 21) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 22) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 23) >> 31); // 20
                    destination[currentIndex++] = (int)((uint)(value << 24) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 25) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 26) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 27) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 28) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 29) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 30) >> 31);
                    destination[currentIndex++] = (int)((uint)(value << 31) >> 31);
                    break;
                case 1:
                    // number : 14, bitwidth : 2
                    destination[currentIndex++] = (int)((uint)(value << 4) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 6) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 8) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 10) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 12) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 14) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 16) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 18) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 20) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 22) >> 30); // 10
                    destination[currentIndex++] = (int)((uint)(value << 24) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 26) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 28) >> 30);
                    destination[currentIndex++] = (int)((uint)(value << 30) >> 30);
                    break;
                case 2:
                    // number : 9, bitwidth : 3
                    destination[currentIndex++] = (int)((uint)(value << 5) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 8) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 11) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 14) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 17) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 20) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 23) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 26) >> 29);
                    destination[currentIndex++] = (int)((uint)(value << 29) >> 29);
                    break;
                case 3:
                    // number : 7, bitwidth : 4
                    destination[currentIndex++] = (int)((uint)(value << 4) >> 28);
                    destination[currentIndex++] = (int)((uint)(value << 8) >> 28);
                    destination[currentIndex++] = (int)((uint)(value << 12) >> 28);
                    destination[currentIndex++] = (int)((uint)(value << 16) >> 28);
                    destination[currentIndex++] = (int)((uint)(value << 20) >> 28);
                    destination[currentIndex++] = (int)((uint)(value << 24) >> 28);
                    destination[currentIndex++] = (int)((uint)(value << 28) >> 28);
                    break;
                case 4:
                    // number : 5, bitwidth : 5
                    destination[currentIndex++] = (int)((uint)(value << 7) >> 27);
                    destination[currentIndex++] = (int)((uint)(value << 12) >> 27);
                    destination[currentIndex++] = (int)((uint)(value << 17) >> 27);
                    destination[currentIndex++] = (int)((uint)(value << 22) >> 27);
                    destination[currentIndex++] = (int)((uint)(value << 27) >> 27);
                    break;
                case 5:
                    // number : 4, bitwidth : 7
                    destination[currentIndex++] = (int)((uint)(value << 4) >> 25);
                    destination[currentIndex++] = (int)((uint)(value << 11) >> 25);
                    destination[currentIndex++] = (int)((uint)(value << 18) >> 25);
                    destination[currentIndex++] = (int)((uint)(value << 25) >> 25);
                    break;
                case 6:
                    // number : 3, bitwidth : 9
                    destination[currentIndex++] = (int)((uint)(value << 5) >> 23);
                    destination[currentIndex++] = (int)((uint)(value << 14) >> 23);
                    destination[currentIndex++] = (int)((uint)(value << 23) >> 23);
                    break;
                case 7:
                    // number : 2, bitwidth : 14
                    destination[currentIndex++] = (int)((uint)(value << 4) >> 18);
                    destination[currentIndex++] = (int)((uint)(value << 18) >> 18);
                    break;
                case 8:
                    // number : 1, bitwidth : 28
                    destination[currentIndex++] = (int)((uint)(value << 4) >> 4);
                    break;
                default:
                    throw new InvalidOperationException("shouldn't happen: limited to 28-bit integers");
            }
        }

        while (currentIndex < number)
        {
            var val = source[temporarySourceIndex++];
            switch ((int)((uint)val >> 28))
            {
                case 0:
                    {
                        // number : 28, bitwidth : 1
                        var count = number - currentIndex;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << (k + 4)) >> 31);
                        }

                        break;
                    }

                case 1:
                    {
                        // number : 14, bitwidth : 2
                        var count = number - currentIndex < 14 ? number - currentIndex : 14;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((2 * k) + 4)) >> 30);
                        }

                        break;
                    }

                case 2:
                    {
                        // number : 9, bitwidth : 3
                        var count = number - currentIndex < 9 ? number - currentIndex : 9;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((3 * k) + 5)) >> 29);
                        }

                        break;
                    }

                case 3:
                    {
                        // number : 7, bitwidth : 4
                        var count = number - currentIndex < 7 ? number - currentIndex : 7;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((4 * k) + 4)) >> 28);
                        }

                        break;
                    }

                case 4:
                    {
                        // number : 5, bitwidth : 5
                        var count = number - currentIndex < 5 ? number - currentIndex : 5;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((5 * k) + 7)) >> 27);
                        }

                        break;
                    }

                case 5:
                    {
                        // number : 4, bitwidth : 7
                        var count = number - currentIndex < 4 ? number - currentIndex : 4;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((7 * k) + 4)) >> 25);
                        }

                        break;
                    }

                case 6:
                    {
                        // number : 3, bitwidth : 9
                        var count = number - currentIndex < 3 ? number - currentIndex : 3;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((9 * k) + 5)) >> 23);
                        }

                        break;
                    }

                case 7:
                    {
                        // number : 2, bitwidth : 14
                        var count = number - currentIndex < 2 ? number - currentIndex : 2;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((14 * k) + 4)) >> 18);
                        }

                        break;
                    }

                case 8:
                    // number : 1, bitwidth : 28
                    destination[currentIndex++] = (int)((uint)(val << 4) >> 4);
                    break;
                default:
                    throw new InvalidOperationException("shouldn't happen");
            }
        }

        return (temporarySourceIndex, currentIndex);
    }
}