// -----------------------------------------------------------------------
// <copyright file="BitArrayExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;

/// <summary>
/// Extensions for <see cref="System.Collections.BitArray"/>.
/// </summary>
public static partial class BitArrayExtensions
{
    /// <summary>
    /// Gets the byte from the array of bits.
    /// </summary>
    /// <param name="bits">The bit array.</param>
    /// <returns>A byte value.</returns>
    public static byte GetByte(this System.Collections.BitArray bits)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(bits);
        return GetByte(bits, 0, bits.Length);
    }

    /// <summary>
    /// Gets the byte from the array of bits.
    /// </summary>
    /// <param name="bits">The bit array.</param>
    /// <param name="startIndex">The start index in the array.</param>
    /// <param name="length">The number of bits to use.</param>
    /// <returns>A byte value.</returns>
    public static byte GetByte(this System.Collections.BitArray bits, int startIndex, int length)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(bits);
        ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan(length, 8);
        ArgumentOutOfRangeExceptionThrower.ThrowIfNegative(startIndex);
        ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan(startIndex + length, bits.Length);

        byte @byte = 0;

        for (var j = 0; j < length; j++)
        {
            @byte |= (byte)(bits[j + startIndex] ? 0x01 << j : 0x00);
        }

        return @byte;
    }

    /// <summary>
    /// Gets the bytes from the array of bits.
    /// </summary>
    /// <param name="bits">The bit array.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.</returns>
    public static IEnumerable<byte> GetBytes(this System.Collections.BitArray bits)
    {
        const int BitsPerByte = 8;
        var totalLength = bits.Length;
        for (var current = 0; totalLength > 0; totalLength -= BitsPerByte, current++)
        {
            yield return bits.GetByte(current * BitsPerByte, totalLength > BitsPerByte ? BitsPerByte : totalLength);
        }
    }

    /// <summary>
    /// Gets the hexadecimal string for the bit array.
    /// </summary>
    /// <param name="bits">The bit array.</param>
    /// <param name="provider">The format provider.</param>
    /// <returns>The hexadecimal string for <paramref name="bits"/>.</returns>
    public static string ToHexString(this System.Collections.BitArray bits, IFormatProvider? provider)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        const char Space = ' ';
#else
        const string Space = " ";
#endif
        return string.Join(Space, GetHexStringParts(GetBytes(bits).Reverse(), provider));

        static IEnumerable<string> GetHexStringParts(IEnumerable<byte> bytes, IFormatProvider? provider = default)
        {
            foreach (var byteValue in bytes)
            {
                yield return byteValue is 0 ? byteValue.ToString(provider) : byteValue.ToString("x2", provider);
            }
        }
    }

    /// <summary>
    /// Determines whether the 2 instances of <see cref="System.Collections.BitArray"/> instances are considered equal.
    /// </summary>
    /// <param name="bits">The bits.</param>
    /// <param name="other">The other.</param>
    /// <returns><see langword="true"/> if <paramref name="bits"/> and <paramref name="other"/> are considered equal; otherwise <see langword="false"/>.</returns>
    public static bool Equals(this System.Collections.BitArray bits, System.Collections.BitArray other) => BitArrayEqualityComparer.Instance.Equals(bits, other);
}