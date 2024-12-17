// -----------------------------------------------------------------------
// <copyright file="Util.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Utilities.
/// </summary>
internal static class Util
{
    /// <summary>
    /// Gets the greatest multiple.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="factor">The factor.</param>
    /// <returns>The greatest multiple.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int GreatestMultiple(int value, int factor) => value - (value % factor);

    /// <summary>
    /// Gets the maximum number of bits.
    /// </summary>
    /// <param name="input">The values.</param>
    /// <param name="start">The position.</param>
    /// <param name="length">The length.</param>
    /// <returns>The maximum number of bits.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int MaxBits(int[] input, int start, int length) => MaxBits(input.AsSpan(start, length));

    /// <summary>
    /// Gets the maximum number of bits.
    /// </summary>
    /// <param name="buffer">The buffer.</param>
    /// <returns>The maximum number of bits.</returns>
    public static int MaxBits(ReadOnlySpan<int> buffer)
    {
        var mask = 0;
        foreach (var value in buffer)
        {
            mask |= value;
        }

        return Bits(mask);
    }

    /// <summary>
    /// Gets the number of bits.
    /// </summary>
    /// <param name="value">The integer value.</param>
    /// <returns>The number of bits.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int Bits(int value) => 32 - LeadingZeroCount(value);

    /// <summary>
    /// Computes the number of leading zeros in a value.
    /// </summary>
    /// <param name="value">The value whose leading zeroes are to be counted.</param>
    /// <returns>The number of leading zeros in <paramref name="value"/>.</returns>
#if USE_GENERIC_MATH
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
#endif
    public static int LeadingZeroCount(int value)

#if USE_GENERIC_MATH
        => int.LeadingZeroCount(value);
#else
    {
        const int NumberOfIntegerBits = sizeof(int) * 8;

        // do the smearing
        value |= value >> 01;
        value |= value >> 02;
        value |= value >> 04;
        value |= value >> 08;
        value |= value >> 16;

        // count the ones
        value -= (value >> 01) & 0x55555555;
        value = ((value >> 02) & 0x33333333) + (value & 0x33333333);
        value = ((value >> 04) + value) & 0x0F0F0F0F;
        value += value >> 08;
        value += value >> 16;

        // subtract # of 1s from 32
        return NumberOfIntegerBits - (value & 0x0000003F);
    }
#endif
}