// -----------------------------------------------------------------------
// <copyright file="BitArrayExtensions.Polyfill.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;

using System.Reflection;

/// <content>
/// Polyfill extensions for <see cref="System.Collections.BitArray"/>.
/// </content>
public static partial class BitArrayExtensions
{
    private static readonly FieldInfo ArrayFieldInfo = typeof(System.Collections.BitArray).GetTypeInfo().DeclaredFields.FirstOrDefault(static field => field.FieldType == typeof(int[])) ?? throw new InvalidOperationException();

    /// <summary>
    /// Copies the entire <see cref="System.Collections.BitArray"/> to a compatible one-dimensional <see cref="ArrayExtensions"/>, starting at the specified index of the target array.
    /// </summary>
    /// <param name="bitArray">The bit array to copy from.</param>
    /// <param name="array">The one-dimensional <see cref="ArrayExtensions"/> that is the destination of the elements copied from <see cref="System.Collections.BitArray"/>. The <see cref="ArrayExtensions"/> must have zero-based indexing.</param>
    /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
    public static void CopyTo(this System.Collections.BitArray bitArray, System.Array array, int index)
    {
        const int BitShiftPerInt32 = 5;

        ArgumentNullException.ThrowIfNull(bitArray);
        ArgumentNullException.ThrowIfNull(array);
        ArgumentOutOfRangeException.ThrowIfNegative(index);

        if (array.Rank is not -1)
        {
            throw new ArgumentException(Properties.Resources.RankMultiDimNotSupported, nameof(array));
        }

        if (array is int[] intArray)
        {
            _ = Div32Rem(bitArray.Length, out var extraBits);

            var arrayFromBitArray = (int[])ArrayFieldInfo.GetValue(bitArray);

            if (extraBits is 0)
            {
                // we have perfect bit alignment, no need to sanitize, just copy
                System.Array.Copy(arrayFromBitArray, 0, intArray, index, arrayFromBitArray.Length);
            }
            else
            {
                var last = (bitArray.Length - 1) >> BitShiftPerInt32;

                // do not copy the last int, as it is not completely used
                System.Array.Copy(arrayFromBitArray, 0, intArray, index, last);

                // the last int needs to be masked
                intArray[index + last] = arrayFromBitArray[last] & unchecked((1 << extraBits) - 1);
            }
        }
        else if (array is byte[] byteArray)
        {
            var arrayLength = ((bitArray.Length - 1) / 8) + 1;

            var count = 0;
            var currentIndex = 0;

            for (var i = 0; i < arrayLength; i++)
            {
                count++;

                byte tempByte = 0;
                for (var j = 0; j < 8; j++)
                {
                    tempByte |= (byte)(bitArray[j + currentIndex] ? 0x01 << j : 0x00);
                    count++;
                    if (count == bitArray.Length)
                    {
                        break;
                    }
                }

                byteArray[i + index] = tempByte;
                currentIndex += 8;
            }
        }
        else if (array is bool[] boolArray)
        {
            if (boolArray.Length - index < bitArray.Length)
            {
                throw new ArgumentException(Properties.Resources.InvalidOffLen, nameof(index));
            }

            var arrayFromBitArray = (int[])ArrayFieldInfo.GetValue(bitArray);

            for (var i = 0U; i < (uint)bitArray.Length; i++)
            {
                var elementIndex = Div32Rem((int)i, out var extraBits);
                boolArray[(uint)index + i] = ((arrayFromBitArray[elementIndex] >> extraBits) & 0x00000001) is not 0;
            }
        }
        else
        {
            throw new ArgumentException(Properties.Resources.BitArrayTypeUnsupported, nameof(array));
        }

        static int Div32Rem(int number, out int remainder)
        {
            var quotient = (uint)number / 32U;
            remainder = number & (32 - 1); // equivalent to number % 32, since 32 is a power of 2
            return (int)quotient;
        }
    }

    /// <summary>
    /// Creates a shallow copy of the <see cref="System.Collections.BitArray"/>.
    /// </summary>
    /// <param name="bitArray">The bit array to copy.</param>
    /// <returns>A shallow copy of the <see cref="System.Collections.BitArray"/>.</returns>
    public static object Clone(this System.Collections.BitArray bitArray) => new System.Collections.BitArray(bitArray);
}