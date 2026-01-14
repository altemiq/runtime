// -----------------------------------------------------------------------
// <copyright file="BinaryPrimitives.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

using BitConverter = System.BitConverter;

/// <summary>
/// Extensions for the binary primitives class.
/// </summary>
public static class BinaryPrimitives
{
#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="ulong" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ReverseEndianness(ulong value)
        => (value >> 56)
            | ((value & 0xFF000000000000) >> 40)
            | ((value & 0xFF0000000000) >> 24)
            | ((value & 0xFF00000000) >> 8)
            | ((value & 0xFF000000) << 8)
            | ((value & 0xFF0000) << 24)
            | ((value & 0xFF00) << 40)
            | (value << 56);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(ulong)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ReverseEndianness(ulong value)
        => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="uint" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ReverseEndianness(uint value) => (value >> 24)
            | ((value & 0xFF0000) >> 8)
            | ((value & 0xFF00) << 8)
            | (value << 24);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(uint)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ReverseEndianness(uint value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="ushort" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ReverseEndianness(ushort value) => (ushort)((value >> 8) + (value << 8));
#else

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(ushort)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ReverseEndianness(ushort value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="sbyte"/> value, which effectively does nothing for a <see cref="sbyte"/>.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The passed-in value, unmodified.</returns>
    /// <remarks>This method effectively does nothing and was added only for consistency.</remarks>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static sbyte ReverseEndianness(sbyte value) => value;
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(sbyte)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static sbyte ReverseEndianness(sbyte value) => value;
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="short" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ReverseEndianness(short value) => (short)ReverseEndianness((ushort)value);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(short)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ReverseEndianness(short value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="int" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ReverseEndianness(int value) => (int)ReverseEndianness((uint)value);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ReverseEndianness(int value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="byte"/> value, which effectively does nothing for a <see cref="byte"/>.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The passed-in value, unmodified.</returns>
    /// <remarks>This method effectively does nothing and was added only for consistency.</remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte ReverseEndianness(byte value) => value;
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(byte)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte ReverseEndianness(byte value) => value;
#endif

#if NETSTANDARD1_0 || NETSTANDARD1_1
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="long" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ReverseEndianness(long value) => (long)ReverseEndianness((ulong)value);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(long)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ReverseEndianness(long value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian" />
    public static void WriteInt16BigEndian(Span<byte> destination, short value) => System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteInt16LittleEndian" />
    public static void WriteInt16LittleEndian(Span<byte> destination, short value) => System.Buffers.Binary.BinaryPrimitives.WriteInt16LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian" />
    public static void WriteInt32BigEndian(Span<byte> destination, int value) => System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian" />
    public static void WriteInt32LittleEndian(Span<byte> destination, int value) => System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian" />
    public static void WriteInt64BigEndian(Span<byte> destination, long value) => System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian" />
    public static void WriteInt64LittleEndian(Span<byte> destination, long value) => System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteUInt16BigEndian" />
    [CLSCompliant(false)]
    public static void WriteUInt16BigEndian(Span<byte> destination, ushort value) => System.Buffers.Binary.BinaryPrimitives.WriteUInt16BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian" />
    [CLSCompliant(false)]
    public static void WriteUInt16LittleEndian(Span<byte> destination, ushort value) => System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian" />
    [CLSCompliant(false)]
    public static void WriteUInt32BigEndian(Span<byte> destination, uint value) => System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian" />
    [CLSCompliant(false)]
    public static void WriteUInt32LittleEndian(Span<byte> destination, uint value) => System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian" />
    [CLSCompliant(false)]
    public static void WriteUInt64BigEndian(Span<byte> destination, ulong value) => System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian" />
    [CLSCompliant(false)]
    public static void WriteUInt64LittleEndian(Span<byte> destination, ulong value) => System.Buffers.Binary.BinaryPrimitives.WriteUInt64LittleEndian(destination, value);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteHalfBigEndian" />
    public static void WriteHalfBigEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.WriteHalfBigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteHalfLittleEndian" />
    public static void WriteHalfLittleEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.WriteHalfLittleEndian(destination, value);
#elif NET5_0_OR_GREATER
    /// <summary>
    /// Writes a <see cref="Half"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="Half"/>.</exception>
    public static void WriteHalfBigEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(destination, BitConverter.HalfToInt16Bits(value));

    /// <summary>
    /// Writes a <see cref="Half"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="Half"/>.</exception>
    public static void WriteHalfLittleEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.WriteInt16LittleEndian(destination, BitConverter.HalfToInt16Bits(value));
#endif

#if NET5_0_OR_GREATER
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteSingleBigEndian" />
    public static void WriteSingleBigEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.WriteSingleBigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteSingleLittleEndian" />
    public static void WriteSingleLittleEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.WriteSingleLittleEndian(destination, value);
#else
    /// <summary>
    /// Writes a <see cref="float"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="float"/>.</exception>
    public static void WriteSingleBigEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(destination, BitConverter.SingleToInt32Bits(value));

    /// <summary>
    /// Writes a <see cref="float"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="float"/>.</exception>
    public static void WriteSingleLittleEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(destination, BitConverter.SingleToInt32Bits(value));
#endif

#if NET5_0_OR_GREATER
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteDoubleBigEndian" />
    public static void WriteDoubleBigEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.WriteDoubleBigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.WriteDoubleLittleEndian" />
    public static void WriteDoubleLittleEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.WriteDoubleLittleEndian(destination, value);
#else
    /// <summary>
    /// Writes a <see cref="double"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="double"/>.</exception>
    public static void WriteDoubleBigEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(destination, BitConverter.DoubleToInt64Bits(value));

    /// <summary>
    /// Writes a <see cref="double"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="destination"/> is too small to contain a <see cref="double"/>.</exception>
    public static void WriteDoubleLittleEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.WriteInt64LittleEndian(destination, BitConverter.DoubleToInt64Bits(value));
#endif

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteInt16BigEndian" />
    public static bool TryWriteInt16BigEndian(Span<byte> destination, short value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt16BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteInt16LittleEndian" />
    public static bool TryWriteInt16LittleEndian(Span<byte> destination, short value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt16LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteInt32BigEndian" />
    public static bool TryWriteInt32BigEndian(Span<byte> destination, int value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt32BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteInt32LittleEndian" />
    public static bool TryWriteInt32LittleEndian(Span<byte> destination, int value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt32LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteInt64BigEndian" />
    public static bool TryWriteInt64BigEndian(Span<byte> destination, long value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt64BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteInt64LittleEndian" />
    public static bool TryWriteInt64LittleEndian(Span<byte> destination, long value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt64LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16BigEndian" />
    [CLSCompliant(false)]
    public static bool TryWriteUInt16BigEndian(Span<byte> destination, ushort value) => System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16LittleEndian" />
    [CLSCompliant(false)]
    public static bool TryWriteUInt16LittleEndian(Span<byte> destination, ushort value) => System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteUInt32BigEndian" />
    [CLSCompliant(false)]
    public static bool TryWriteUInt32BigEndian(Span<byte> destination, uint value) => System.Buffers.Binary.BinaryPrimitives.TryWriteUInt32BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteUInt32LittleEndian" />
    [CLSCompliant(false)]
    public static bool TryWriteUInt32LittleEndian(Span<byte> destination, uint value) => System.Buffers.Binary.BinaryPrimitives.TryWriteUInt32LittleEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteUInt64BigEndian" />
    [CLSCompliant(false)]
    public static bool TryWriteUInt64BigEndian(Span<byte> destination, ulong value) => System.Buffers.Binary.BinaryPrimitives.TryWriteUInt64BigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteUInt64LittleEndian" />
    [CLSCompliant(false)]
    public static bool TryWriteUInt64LittleEndian(Span<byte> destination, ulong value) => System.Buffers.Binary.BinaryPrimitives.TryWriteUInt64LittleEndian(destination, value);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteHalfBigEndian" />
    public static bool TryWriteHalfBigEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.TryWriteHalfBigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteHalfLittleEndian" />
    public static bool TryWriteHalfLittleEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.TryWriteHalfLittleEndian(destination, value);
#elif NET5_0_OR_GREATER
    /// <summary>
    /// Writes a <see cref="Half"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="Half"/>; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteHalfBigEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt16BigEndian(destination, BitConverter.HalfToInt16Bits(value));

    /// <summary>
    /// Writes a <see cref="Half"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as little endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="Half"/>; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteHalfLittleEndian(Span<byte> destination, Half value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt16LittleEndian(destination, BitConverter.HalfToInt16Bits(value));
#endif

#if NET5_0_OR_GREATER
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteSingleBigEndian" />
    public static bool TryWriteSingleBigEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.TryWriteSingleBigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteSingleLittleEndian" />
    public static bool TryWriteSingleLittleEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.TryWriteSingleLittleEndian(destination, value);
#else
    /// <summary>
    /// Writes a <see cref="float"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="float"/>; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteSingleBigEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt32BigEndian(destination, BitConverter.SingleToInt32Bits(value));

    /// <summary>
    /// Writes a <see cref="float"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="float"/>; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteSingleLittleEndian(Span<byte> destination, float value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt32LittleEndian(destination, BitConverter.SingleToInt32Bits(value));
#endif

#if NET5_0_OR_GREATER
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteDoubleBigEndian" />
    public static bool TryWriteDoubleBigEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.TryWriteDoubleBigEndian(destination, value);

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.TryWriteDoubleLittleEndian" />
    public static bool TryWriteDoubleLittleEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.TryWriteDoubleLittleEndian(destination, value);
#else
    /// <summary>
    /// Writes a <see cref="double"/> into a span of bytes, as big endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="double"/>; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteDoubleBigEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt64BigEndian(destination, BitConverter.DoubleToInt64Bits(value));

    /// <summary>
    /// Writes a <see cref="double"/> into a span of bytes, as little endian.
    /// </summary>
    /// <param name="destination">The span of bytes where the value is to be written, as big endian.</param>
    /// <param name="value">The value to write into the span of bytes.</param>
    /// <returns><see langword="true"/> if the span is large enough to contain a <see cref="double"/>; otherwise, <see langword="false"/>.</returns>
    public static bool TryWriteDoubleLittleEndian(Span<byte> destination, double value) => System.Buffers.Binary.BinaryPrimitives.TryWriteInt64LittleEndian(destination, BitConverter.DoubleToInt64Bits(value));
#endif
#endif
}