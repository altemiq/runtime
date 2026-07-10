// -----------------------------------------------------------------------
// <copyright file="BinaryPrimitiveExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETSTANDARD1_0

#pragma warning disable CA1050, MA0047, RCS1110

/// <summary>
/// Extensions for <see cref="System.Buffers.Binary.BinaryPrimitives"/>.
/// </summary>
public static class BinaryPrimitiveExtensions
{
    extension(System.Buffers.Binary.BinaryPrimitives)
    {
#if NETSTANDARD1_1
        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="ulong" /> value.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The reversed value.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ulong ReverseEndianness(ulong value) => (value >> 56) | ((value & 0xFF000000000000) >> 40) | ((value & 0xFF0000000000) >> 24) | ((value & 0xFF00000000) >> 8) | ((value & 0xFF000000) << 8) | ((value & 0xFF0000) << 24) | ((value & 0xFF00) << 40) | (value << 56);

        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="uint" /> value.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The reversed value.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint ReverseEndianness(uint value) => (value >> 24) | ((value & 0xFF0000) >> 8) | ((value & 0xFF00) << 8) | (value << 24);

        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="ushort" /> value.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The reversed value.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ushort ReverseEndianness(ushort value) => (ushort)((value >> 8) + (value << 8));

        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="sbyte"/> value, which effectively does nothing for a <see cref="sbyte"/>.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The passed-in value, unmodified.</returns>
        /// <remarks>This method effectively does nothing and was added only for consistency.</remarks>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static sbyte ReverseEndianness(sbyte value) => value;

        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="short" /> value.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The reversed value.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static short ReverseEndianness(short value) => (short)ReverseEndianness((ushort)value);

        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="int" /> value.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The reversed value.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int ReverseEndianness(int value) => (int)ReverseEndianness((uint)value);

        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="byte"/> value, which effectively does nothing for a <see cref="byte"/>.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The passed-in value, unmodified.</returns>
        /// <remarks>This method effectively does nothing and was added only for consistency.</remarks>
        /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(byte)" />
        public static byte ReverseEndianness(byte value) => value;

        /// <summary>
        /// Reverses a primitive value by performing an endianness swap of the specified <see cref="long" /> value.
        /// </summary>
        /// <param name="value">The value to reverse.</param>
        /// <returns>The reversed value.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long ReverseEndianness(long value) => (long)ReverseEndianness((ulong)value);
#endif

#if NET5_0_OR_GREATER && !NET6_0_OR_GREATER
        /// <summary>
        /// Writes a <see cref="Half"/> into a span of bytes, as big endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="Half"/>.</exception>
        public static void WriteHalfBigEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(destination, System.BitConverter.HalfToInt16Bits(value));

        /// <summary>
        /// Writes a <see cref="Half"/> into a span of bytes, as little endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="Half"/>.</exception>
        public static void WriteHalfLittleEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.WriteInt16LittleEndian(destination, System.BitConverter.HalfToInt16Bits(value));

        /// <summary>
        /// Writes a <see cref="Half"/> into a span of bytes, as big endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="Half"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryWriteHalfBigEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt16BigEndian(destination, System.BitConverter.HalfToInt16Bits(value));

        /// <summary>
        /// Writes a <see cref="Half"/> into a span of bytes, as little endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="Half"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryWriteHalfLittleEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt16LittleEndian(destination, System.BitConverter.HalfToInt16Bits(value));
#endif

#if !NET5_0_OR_GREATER
        /// <summary>
        /// Writes a <see cref="float"/> into a span of bytes, as big endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="float"/>.</exception>
        public static void WriteSingleBigEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(destination, System.BitConverter.SingleToInt32Bits(value));

        /// <summary>
        /// Writes a <see cref="float"/> into a span of bytes, as little endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="float"/>.</exception>
        public static void WriteSingleLittleEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination, System.BitConverter.SingleToInt32Bits(value));

        /// <summary>
        /// Writes a <see cref="float"/> into a span of bytes, as big endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="float"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryWriteSingleBigEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt32BigEndian(destination, System.BitConverter.SingleToInt32Bits(value));

        /// <summary>
        /// Writes a <see cref="float"/> into a span of bytes, as little endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="float"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryWriteSingleLittleEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt32LittleEndian(destination, System.BitConverter.SingleToInt32Bits(value));

        /// <summary>
        /// Writes a <see cref="double"/> into a span of bytes, as big endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="double"/>.</exception>
        public static void WriteDoubleBigEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(destination, System.BitConverter.DoubleToInt64Bits(value));

        /// <summary>
        /// Writes a <see cref="double"/> into a span of bytes, as little endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="double"/>.</exception>
        public static void WriteDoubleLittleEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(destination, System.BitConverter.DoubleToInt64Bits(value));

        /// <summary>
        /// Writes a <see cref="double"/> into a span of bytes, as big endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="double"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryWriteDoubleBigEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt64BigEndian(destination, System.BitConverter.DoubleToInt64Bits(value));

        /// <summary>
        /// Writes a <see cref="double"/> into a span of bytes, as little endian.
        /// </summary>
        /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
        /// <param name="value">The value to write into the span of bytes.</param>
        /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="double"/>; otherwise, <see langword="false"/>.</returns>
        public static bool TryWriteDoubleLittleEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt64LittleEndian(destination, System.BitConverter.DoubleToInt64Bits(value));
#endif
    }
}
#endif