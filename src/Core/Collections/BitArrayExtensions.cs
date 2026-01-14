// -----------------------------------------------------------------------
// <copyright file="BitArrayExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;

#pragma warning disable CA1708, RCS1263, SA1101, S2325

/// <summary>
/// Extensions for <see cref="System.Collections.BitArray"/>.
/// </summary>
public static
#if !NETSTANDARD2_0_OR_GREATER && !NETCOREAPP2_0_OR_GREATER && !NET20_OR_GREATER
    partial
#endif
    class BitArrayExtensions
{
    /// <summary>
    /// Extensions for <see cref="System.Collections.BitArray"/>.
    /// </summary>
    /// <param name="bits">The bit array.</param>
    extension(System.Collections.BitArray bits)
    {
        /// <summary>
        /// Gets the byte from the array of bits.
        /// </summary>
        /// <returns>A byte value.</returns>
        public byte GetByte()
        {
            ArgumentNullException.ThrowIfNull(bits);
            return bits.GetByte(0, bits.Length);
        }

        /// <summary>
        /// Gets the byte from the array of bits.
        /// </summary>
        /// <param name="startIndex">The start index in the array.</param>
        /// <param name="length">The number of bits to use.</param>
        /// <returns>A byte value.</returns>
        public byte GetByte(int startIndex, int length)
        {
            ArgumentNullException.ThrowIfNull(bits);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(length, 8);
            ArgumentOutOfRangeException.ThrowIfNegative(startIndex);
#pragma warning disable S3236
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex + length, bits.Length, nameof(startIndex));
#pragma warning restore S3236

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
        /// <returns>An <see cref="IEnumerable{T}"/> of <see cref="byte"/> values.</returns>
        public IEnumerable<byte> GetBytes()
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
        /// <param name="provider">The format provider.</param>
        /// <returns>The hexadecimal string.</returns>
        public string ToHexString(IFormatProvider? provider)
        {
            const char Space = ' ';
            return string.Join(Space, GetHexStringParts(bits.GetBytes().Reverse(), provider));

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
        /// <param name="other">The other.</param>
        /// <returns><see langword="true"/> if this instance and <paramref name="other"/> are considered equal; otherwise <see langword="false"/>.</returns>
        public bool Equals(System.Collections.BitArray other) => BitArrayEqualityComparer.Instance.Equals(bits, other);
    }
}