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

        var outlength = source[sourceIndex];
        sourceIndex++;
        HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, outlength);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(Simple9);

    private static void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        var temporaryDestinationIndex = destinationIndex;
        var currentPos = sourceIndex;
        var finalin = currentPos + length;

        while (currentPos < finalin - 28)
        {
            if (!TryGetSelector(source, destination, ref temporaryDestinationIndex, ref currentPos))
            {
                continue;
            }

            if (source[currentPos] >= 1 << BitLength[8])
            {
                throw new InvalidDataException("Too big a number");
            }

            destination[temporaryDestinationIndex++] = source[currentPos++] | (8 << 28);

            static bool TryGetSelector(int[] source, int[] destination, ref int temporaryDestinationIndex, ref int currentPos)
            {
                var selector = 0;
                while (selector < 8)
                {
                    var compressedNum = CodeNum[selector];
                    if (!Check(source, currentPos, compressedNum, ref selector, out var res))
                    {
                        continue;
                    }

                    destination[temporaryDestinationIndex++] = res;
                    currentPos += compressedNum;
                    return false;

                    static bool Check(int[] source, int currentPos, int compressedNum, ref int selector, out int res)
                    {
                        var b = BitLength[selector];
                        var max = 1 << b;
                        res = 0;
                        for (var i = 0; i < compressedNum; i++)
                        {
                            if (max <= source[currentPos + i])
                            {
                                selector++;
                                return false;
                            }

                            res = (res << b) + source[currentPos + i];
                        }

                        res |= selector << 28;
                        return true;
                    }
                }

                return true;
            }
        }

        while (currentPos < finalin)
        {
            if (!TryGetSelector(source, destination, ref temporaryDestinationIndex, ref currentPos, finalin))
            {
                continue;
            }

            if (source[currentPos] >= 1 << BitLength[8])
            {
                throw new InvalidDataException("Too big a number");
            }

            destination[temporaryDestinationIndex++] = source[currentPos++] | (8 << 28);

            static bool TryGetSelector(int[] source, int[] destination, ref int temporaryDestinationIndex, ref int currentPos, int finalin)
            {
                var selector = 0;
                while (selector < 8)
                {
                    var compressedNum = CodeNum[selector];
                    if (finalin <= currentPos + compressedNum - 1)
                    {
                        compressedNum = finalin - currentPos;
                    }

                    if (!Check(source, currentPos, ref selector, out var res, compressedNum))
                    {
                        continue;
                    }

                    destination[temporaryDestinationIndex++] = res;
                    currentPos += compressedNum;
                    return false;

                    static bool Check(int[] source, int currentPos, ref int selector, out int res, int compressedNum)
                    {
                        res = default;
                        var b = BitLength[selector];
                        var max = 1 << b;
                        for (var i = 0; i < compressedNum; i++)
                        {
                            if (max <= source[currentPos + i])
                            {
                                selector++;
                                return false;
                            }

                            res = (res << b) + source[currentPos + i];
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

        sourceIndex = currentPos;
        destinationIndex = temporaryDestinationIndex;
    }

    private static void HeadlessUncompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int number)
    {
        var currentIndex = destinationIndex;
        var temporarySourceIndex = sourceIndex;
        var endDestinationIndex = currentIndex + number;
        while (currentIndex < endDestinationIndex - 28)
        {
            var value = source[temporarySourceIndex++];
            switch ((int)((uint)value >> 28))
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

        while (currentIndex < endDestinationIndex)
        {
            var val = source[temporarySourceIndex++];
            switch ((int)((uint)val >> 28))
            {
                case 0:
                    {
                        // number : 28, bitwidth : 1
                        var count = endDestinationIndex - currentIndex;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << (k + 4)) >> 31);
                        }

                        break;
                    }

                case 1:
                    {
                        // number : 14, bitwidth : 2
                        var count = endDestinationIndex - currentIndex < 14 ? endDestinationIndex - currentIndex : 14;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((2 * k) + 4)) >> 30);
                        }

                        break;
                    }

                case 2:
                    {
                        // number : 9, bitwidth : 3
                        var count = endDestinationIndex - currentIndex < 9 ? endDestinationIndex - currentIndex : 9;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((3 * k) + 5)) >> 29);
                        }

                        break;
                    }

                case 3:
                    {
                        // number : 7, bitwidth : 4
                        var count = endDestinationIndex - currentIndex < 7 ? endDestinationIndex - currentIndex : 7;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((4 * k) + 4)) >> 28);
                        }

                        break;
                    }

                case 4:
                    {
                        // number : 5, bitwidth : 5
                        var count = endDestinationIndex - currentIndex < 5 ? endDestinationIndex - currentIndex : 5;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((5 * k) + 7)) >> 27);
                        }

                        break;
                    }

                case 5:
                    {
                        // number : 4, bitwidth : 7
                        var count = endDestinationIndex - currentIndex < 4 ? endDestinationIndex - currentIndex : 4;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((7 * k) + 4)) >> 25);
                        }

                        break;
                    }

                case 6:
                    {
                        // number : 3, bitwidth : 9
                        var count = endDestinationIndex - currentIndex < 3 ? endDestinationIndex - currentIndex : 3;
                        for (var k = 0; k < count; k++)
                        {
                            destination[currentIndex++] = (int)((uint)(val << ((9 * k) + 5)) >> 23);
                        }

                        break;
                    }

                case 7:
                    {
                        // number : 2, bitwidth : 14
                        var count = endDestinationIndex - currentIndex < 2 ? endDestinationIndex - currentIndex : 2;
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

        destinationIndex = currentIndex;
        sourceIndex = temporarySourceIndex;
    }
}