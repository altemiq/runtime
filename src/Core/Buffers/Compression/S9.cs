// -----------------------------------------------------------------------
// <copyright file="S9.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Version of <see cref="Simple9" /> for <see cref="NewPfd"/> and <see cref="OptPfd"/>.
/// </summary>
internal sealed class S9
{
    private static readonly int[] BitLength = [1, 2, 3, 4, 5, 7, 9, 14, 28];

    private static readonly int[] CodeNum = [28, 14, 9, 7, 5, 4, 3, 2, 1];

    /// <inheritdoc cref="Compression.Compress"/>
    public static int Compress(int[] source, int sourceIndex, int[] destination, int destinationIndex, int length)
    {
        var temporaryDestinationIndex = destinationIndex;
        var endSourceIndex = sourceIndex + length;

        while (sourceIndex < endSourceIndex)
        {
            if (!TryGetSelector(source, ref sourceIndex, destination, ref destinationIndex, endSourceIndex))
            {
                continue;
            }

            if (source[sourceIndex] >= 1 << BitLength[8])
            {
                throw new InvalidOperationException("Too big a number");
            }

            destination[destinationIndex++] = source[sourceIndex++] | (8 << 28);
        }

        return destinationIndex - temporaryDestinationIndex;

        static bool TryGetSelector(int[] source, ref int startIndex, int[] destination, ref int destinationIndex, int endSourceIndex)
        {
            var selector = 0;
            while (selector < 8)
            {
                var compressedNum = CodeNum[selector];
                if (endSourceIndex <= startIndex + compressedNum - 1)
                {
                    compressedNum = endSourceIndex - startIndex;
                }

                if (!Check(source, startIndex, ref selector, out var res, compressedNum))
                {
                    continue;
                }

                destination[destinationIndex++] = res;
                startIndex += compressedNum;
                return false;

                static bool Check(int[] source, int currentPos, ref int selector, out int res, int compressedNum)
                {
                    res = 0;
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

    /// <inheritdoc cref="Compression.EstimateCompress"/>
    public static int EstimateCompress(int[] source, int index, int length)
    {
        var temporaryDestinationIndex = 0;
        var endIndex = index + length;
        while (index < endIndex)
        {
            if (!TryGetSelector(source, ref index, ref temporaryDestinationIndex, endIndex))
            {
                continue;
            }

            if (source[index] >= 1 << BitLength[8])
            {
                throw new InvalidDataException("Too big a number");
            }

            temporaryDestinationIndex++;
            index++;
        }

        return temporaryDestinationIndex;

        static bool TryGetSelector(int[] source, ref int sourceIndex, ref int destinationIndex, int endSourceIndex)
        {
            var selector = 0;
            while (selector < 8)
            {
                var compressedNum = CodeNum[selector];
                if (endSourceIndex <= sourceIndex + compressedNum - 1)
                {
                    compressedNum = endSourceIndex - sourceIndex;
                }

                if (!Check(source, sourceIndex, ref selector, compressedNum))
                {
                    continue;
                }

                sourceIndex += compressedNum;
                destinationIndex++;
                return false;

                static bool Check(int[] source, int index, ref int selector, int compressedNum)
                {
                    var b = BitLength[selector];
                    var max = 1 << b;
                    for (var i = 0; i < compressedNum; i++)
                    {
                        if (max <= source[index + i])
                        {
                            selector++;
                            return false;
                        }
                    }

                    return true;
                }
            }

            return true;
        }
    }

    /// <inheritdoc cref="Compression.Decompress"/>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is required for the API.")]
    public static void Decompress(int[] source, int sourceIndex, int sourceLength, int[] destination, int destinationIndex, int destinationLength)
    {
        var endDestinationIndex = destinationIndex + destinationLength;

        while (destinationIndex < endDestinationIndex)
        {
            var value = source[sourceIndex++];
            switch (value >>> 28)
            {
                case 0:
                    {
                        // number : 28, bit-width : 1
                        var count = endDestinationIndex - destinationIndex < 28 ? endDestinationIndex - destinationIndex : 28;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << (k + 4)) >>> 31;
                        }

                        break;
                    }

                case 1:
                    {
                        // number : 14, bit-width : 2
                        var count = endDestinationIndex - destinationIndex < 14 ? endDestinationIndex - destinationIndex : 14;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << ((2 * k) + 4)) >>> 30;
                        }

                        break;
                    }

                case 2:
                    {
                        // number : 9, bit-width : 3
                        var count = endDestinationIndex - destinationIndex < 9 ? endDestinationIndex - destinationIndex : 9;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << ((3 * k) + 5)) >>> 29;
                        }

                        break;
                    }

                case 3:
                    {
                        // number : 7, bit-width : 4
                        var count = endDestinationIndex - destinationIndex < 7 ? endDestinationIndex - destinationIndex : 7;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << ((4 * k) + 4)) >>> 28;
                        }

                        break;
                    }

                case 4:
                    {
                        // number : 5, bit-width : 5
                        var count = endDestinationIndex - destinationIndex < 5 ? endDestinationIndex - destinationIndex : 5;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << ((5 * k) + 7)) >>> 27;
                        }

                        break;
                    }

                case 5:
                    {
                        // number : 4, bit-width : 7
                        var count = endDestinationIndex - destinationIndex < 4 ? endDestinationIndex - destinationIndex : 4;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << ((7 * k) + 4)) >>> 25;
                        }

                        break;
                    }

                case 6:
                    {
                        // number : 3, bit-width : 9
                        var count = endDestinationIndex - destinationIndex < 3 ? endDestinationIndex - destinationIndex : 3;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << ((9 * k) + 5)) >>> 23;
                        }

                        break;
                    }

                case 7:
                    {
                        // number : 2, bit-width : 14
                        var count = endDestinationIndex - destinationIndex < 2 ? endDestinationIndex - destinationIndex : 2;
                        for (var k = 0; k < count; k++)
                        {
                            destination[destinationIndex++] = (value << ((14 * k) + 4)) >>> 18;
                        }

                        break;
                    }

                case 8:
                    // number : 1, bit-width : 28
                    destination[destinationIndex++] = (value << 4) >>> 4;
                    break;

                default:
                    throw new InvalidOperationException("shouldn't happen");
            }
        }
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(S9);
}