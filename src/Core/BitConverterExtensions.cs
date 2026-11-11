// -----------------------------------------------------------------------
// <copyright file="BitConverterExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if NETSTANDARD1_0
using BinaryPrimitives = Altemiq.Buffers.Binary.BinaryPrimitives;
#else
using BinaryPrimitives = System.Buffers.Binary.BinaryPrimitives;
#endif

#pragma warning disable CA1050, MA0047, RCS1110

#if !NET6_0_OR_GREATER
#pragma warning disable S6640
#endif

/// <summary>
/// Extended <see cref="System.BitConverter"/>.
/// </summary>
public static partial class BitConverterExtensions
{
    extension(System.BitConverter)
    {
        /// <inheritdoc cref="System.BitConverter.GetBytes(bool)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public static byte[] GetBytes(bool value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(value);

        /// <inheritdoc cref="System.BitConverter.GetBytes(char)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(char value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(unchecked((short)value), byteOrder, BinaryPrimitives.ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.GetBytes(double)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(double value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));

#if NET6_0_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.GetBytes(Half)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(Half value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NET5_0_OR_GREATER
        /// <summary>
        /// Returns the specified half-precision floating-point value as an array of bytes.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>An array of bytes with length 2.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(Half value) => System.BitConverter.GetBytes(System.BitConverter.HalfToUInt16Bits(value));

        /// <inheritdoc cref="GetBytes(Half)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(Half value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

        /// <inheritdoc cref="System.BitConverter.GetBytes(short)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(short value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.GetBytes(int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(int value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.GetBytes(long)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(long value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.GetBytes(float)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(float value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.GetBytes(ushort)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static byte[] GetBytes(ushort value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.GetBytes(uint)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [CLSCompliant(false)]
        public static byte[] GetBytes(uint value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.GetBytes(ulong)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [CLSCompliant(false)]
        public static byte[] GetBytes(ulong value, Altemiq.ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));

        /// <inheritdoc cref="System.BitConverter.ToBoolean(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public static bool ToBoolean(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => System.BitConverter.ToBoolean(value, startIndex);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToBoolean(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public static bool ToBoolean(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => System.BitConverter.ToBoolean(value);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte span into a Boolean value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A Boolean representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 1.</exception>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool ToBoolean(ReadOnlySpan<byte> value) => value.Length < sizeof(byte) ? throw new ArgumentOutOfRangeException(nameof(value)) : value[0] is not 0;

        /// <summary>
        /// Converts a read-only byte span into a Boolean value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A Boolean representing the converted bytes.</returns>
        /// <exception cref="ArgumentOutOfRangeException">The length of <paramref name="value"/> is less than 1.</exception>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public static bool ToBoolean(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => System.BitConverter.ToBoolean(value);
#endif

        /// <inheritdoc cref="System.BitConverter.ToChar(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static char ToChar(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToChar(value, startIndex), byteOrder, ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToChar(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static char ToChar(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToChar(value), byteOrder, ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte span into a character.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A character representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static char ToChar(ReadOnlySpan<byte> value) => unchecked((char)System.BitConverter.ToInt16(value));

        /// <summary>
        /// Converts a read-only byte span into a character.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A character representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static char ToChar(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToChar(value), byteOrder, ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToDouble(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToDouble(value, startIndex), byteOrder, ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToDouble(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToDouble(value), byteOrder, ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte span into a double-precision floating-point value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A double-precision floating-point value that represents the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(ReadOnlySpan<byte> value) => System.BitConverter.Int64BitsToDouble(System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadInt64LittleEndian(value) : BinaryPrimitives.ReadInt64BigEndian(value));

        /// <summary>
        /// Converts a read-only byte span into a double-precision floating-point value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A double-precision floating-point value that represents the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToDouble(value), byteOrder, ReverseEndianness);
#endif

#if NET6_0_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToHalf(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToHalf(value, startIndex), byteOrder, ReverseEndianness);

        /// <inheritdoc cref="System.BitConverter.ToHalf(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToHalf(value), byteOrder, ReverseEndianness);
#elif NET5_0_OR_GREATER
        /// <summary>
        /// Returns a half-precision floating point number converted from two bytes at a specified position in a byte array.
        /// </summary>
        /// <param name="value">An array of bytes that includes the two bytes to convert.</param>
        /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
        /// <returns>A half-precision floating point number formed by two bytes beginning at <paramref name="startIndex"/>.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(byte[] value, int startIndex) => System.BitConverter.UInt16BitsToHalf(System.BitConverter.ToUInt16(value, startIndex));

        /// <inheritdoc cref="ToHalf(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToHalf(value, startIndex), byteOrder, ReverseEndianness);

        /// <summary>
        /// Converts a read-only byte span into a half-precision floating-point value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A half-precision floating-point value that represents the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(ReadOnlySpan<byte> value) => System.BitConverter.UInt16BitsToHalf(System.BitConverter.ToUInt16(value));

        /// <inheritdoc cref="ToHalf(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static Half ToHalf(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToHalf(value), byteOrder, ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToInt16(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static short ToInt16(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt16(value, startIndex), byteOrder, BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToInt16(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static short ToInt16(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt16(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte span into a 16-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 16-bit signed integer representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static short ToInt16(ReadOnlySpan<byte> value) => System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadInt16LittleEndian(value) : BinaryPrimitives.ReadInt16BigEndian(value);

        /// <summary>
        /// Converts a read-only byte span into a 16-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A 16-bit signed integer representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static short ToInt16(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt16(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToInt32(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt32(value, startIndex), byteOrder, BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToInt32(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt32(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte span into a 32-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 32-bit signed integer representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(ReadOnlySpan<byte> value) => System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadInt32LittleEndian(value) : BinaryPrimitives.ReadInt32BigEndian(value);

        /// <summary>
        /// Converts a read-only byte span into a 32-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A 32-bit signed integer representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static int ToInt32(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt32(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToInt64(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt64(value, startIndex), byteOrder, BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToInt64(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt64(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte span into a 64-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 64-bit signed integer representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(ReadOnlySpan<byte> value) => System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadInt64LittleEndian(value) : BinaryPrimitives.ReadInt64BigEndian(value);

        /// <summary>
        /// Converts a read-only byte span into a 64-bit signed integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A 64-bit signed integer representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static long ToInt64(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToInt64(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToSingle(byte[], int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float ToSingle(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToSingle(value, startIndex), byteOrder, ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToSingle(ReadOnlySpan{byte})" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float ToSingle(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToSingle(value), byteOrder, ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte span into a single-precision floating-point value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A single-precision floating-point representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float ToSingle(ReadOnlySpan<byte> value) => System.BitConverter.Int32BitsToSingle(System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadInt32LittleEndian(value) : BinaryPrimitives.ReadInt32BigEndian(value));

        /// <summary>
        /// Converts a read-only byte span into a single-precision floating-point value.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A single-precision floating-point representing the converted bytes.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static float ToSingle(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToSingle(value), byteOrder, ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToUInt16(byte[], int)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt16(value, startIndex), byteOrder, BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToUInt16(ReadOnlySpan{byte})" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt16(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte-span into a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 16-bit unsigned integer representing the converted bytes.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(ReadOnlySpan<byte> value) => System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadUInt16LittleEndian(value) : BinaryPrimitives.ReadUInt16BigEndian(value);

        /// <summary>
        /// Converts a read-only byte-span into a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A 16-bit unsigned integer representing the converted bytes.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ushort ToUInt16(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt16(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToUInt32(byte[], int)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt32(value, startIndex), byteOrder, BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToUInt32(ReadOnlySpan{byte})" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt32(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte-span into a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 32-bit unsigned integer representing the converted bytes.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(ReadOnlySpan<byte> value) => System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadUInt32LittleEndian(value) : BinaryPrimitives.ReadUInt32BigEndian(value);

        /// <summary>
        /// Converts a read-only byte-span into a 32-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A 32-bit unsigned integer representing the converted bytes.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static uint ToUInt32(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt32(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#endif

        /// <inheritdoc cref="System.BitConverter.ToUInt64(byte[], int)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(byte[] value, int startIndex, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt64(value, startIndex), byteOrder, BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.ToUInt64(ReadOnlySpan{byte})" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt64(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a read-only byte-span into a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <returns>A 64-bit unsigned integer representing the converted bytes.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(ReadOnlySpan<byte> value) => System.BitConverter.IsLittleEndian ? BinaryPrimitives.ReadUInt64LittleEndian(value) : BinaryPrimitives.ReadUInt64BigEndian(value);

        /// <summary>
        /// Converts a read-only byte-span into a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">A read-only span containing the bytes to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns>A 64-bit unsigned integer representing the converted bytes.</returns>
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static ulong ToUInt64(ReadOnlySpan<byte> value, Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(System.BitConverter.ToUInt64(value), byteOrder, BinaryPrimitives.ReverseEndianness);
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, bool)" />
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public static bool TryWriteBytes(Span<byte> destination, bool value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, value);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a Boolean into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted Boolean.</param>
        /// <param name="value">The Boolean to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, bool value)
        {
            if (destination.Length < sizeof(byte))
            {
                return false;
            }

            destination[0] = value ? (byte)1 : (byte)0;
            return true;
        }

        /// <summary>
        /// Converts a Boolean into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted Boolean.</param>
        /// <param name="value">The Boolean to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public static bool TryWriteBytes(Span<byte> destination, bool value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, value);
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, char)" />
        public static bool TryWriteBytes(Span<byte> destination, char value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a character into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted character.</param>
        /// <param name="value">The character to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, char value) => System.BitConverter.TryWriteBytes(destination, (short)value);

        /// <summary>
        /// Converts a character into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted character.</param>
        /// <param name="value">The character to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, char value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, double)" />
        public static bool TryWriteBytes(Span<byte> destination, double value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a double-precision floating-point into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted double-precision floating-point.</param>
        /// <param name="value">The double-precision floating-point to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, double value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteInt64LittleEndian(destination, System.BitConverter.DoubleToInt64Bits(value))
            : BinaryPrimitives.TryWriteInt64BigEndian(destination, System.BitConverter.DoubleToInt64Bits(value));

        /// <summary>
        /// Converts a double-precision floating-point into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted double-precision floating-point.</param>
        /// <param name="value">The double-precision floating-point to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, double value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NET6_0_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, Half)" />
        public static bool TryWriteBytes(Span<byte> destination, Half value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NET5_0_OR_GREATER
        /// <summary>
        /// Converts a half-precision floating-point value into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted half-precision floating-point value.</param>
        /// <param name="value">The half-precision floating-point value to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, Half value) => System.BitConverter.TryWriteBytes(destination, System.BitConverter.HalfToUInt16Bits(value));

        /// <inheritdoc cref="TryWriteBytes(Span{byte}, Half)" />
        public static bool TryWriteBytes(Span<byte> destination, Half value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, short)" />
        public static bool TryWriteBytes(Span<byte> destination, short value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a 16-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 16-bit signed integer.</param>
        /// <param name="value">The 16-bit signed integer to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, short value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteInt16LittleEndian(destination, value)
            : BinaryPrimitives.TryWriteInt16BigEndian(destination, value);

        /// <summary>
        /// Converts a 16-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 16-bit signed integer.</param>
        /// <param name="value">The 16-bit signed integer to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, short value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, int)" />
        public static bool TryWriteBytes(Span<byte> destination, int value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a 32-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 32-bit signed integer.</param>
        /// <param name="value">The 32-bit signed integer to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, int value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteInt32LittleEndian(destination, value)
            : BinaryPrimitives.TryWriteInt32BigEndian(destination, value);

        /// <summary>
        /// Converts a 32-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 32-bit signed integer.</param>
        /// <param name="value">The 32-bit signed integer to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, int value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, long)" />
        public static bool TryWriteBytes(Span<byte> destination, long value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a 64-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit signed integer.</param>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, long value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteInt64LittleEndian(destination, value)
            : BinaryPrimitives.TryWriteInt64BigEndian(destination, value);

        /// <summary>
        /// Converts a 64-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit signed integer.</param>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, long value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, float)" />
        public static bool TryWriteBytes(Span<byte> destination, float value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a single-precision floating-point into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted single-precision floating-point.</param>
        /// <param name="value">The single-precision floating-point to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, float value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteInt32LittleEndian(destination, System.BitConverter.SingleToInt32Bits(value))
            : BinaryPrimitives.TryWriteInt32BigEndian(destination, System.BitConverter.SingleToInt32Bits(value));

        /// <summary>
        /// Converts a single-precision floating-point into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted single-precision floating-point.</param>
        /// <param name="value">The single-precision floating-point to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, float value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, ushort)" />
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ushort value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a 16-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 16-bit unsigned integer.</param>
        /// <param name="value">The 16-bit unsigned integer to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ushort value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteUInt16LittleEndian(destination, value)
            : BinaryPrimitives.TryWriteUInt16BigEndian(destination, value);

        /// <summary>
        /// Converts a 16-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 16-bit unsigned integer.</param>
        /// <param name="value">The 16-bit unsigned integer to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ushort value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, uint)" />
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, uint value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a 32-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 32-bit unsigned integer.</param>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, uint value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteUInt32LittleEndian(destination, value)
            : BinaryPrimitives.TryWriteUInt32BigEndian(destination, value);

        /// <summary>
        /// Converts a 32-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 32-bit unsigned integer.</param>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, uint value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, ulong)" />
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ulong value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
        /// <summary>
        /// Converts a 64-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit unsigned integer.</param>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ulong value) => System.BitConverter.IsLittleEndian
            ? BinaryPrimitives.TryWriteUInt64LittleEndian(destination, value)
            : BinaryPrimitives.TryWriteUInt64BigEndian(destination, value);

        /// <summary>
        /// Converts a 64-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit unsigned integer.</param>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <param name="byteOrder">The required byte order.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ulong value, Altemiq.ByteOrder byteOrder) => System.BitConverter.TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, BinaryPrimitives.ReverseEndianness));
#endif

#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_1_OR_GREATER
        /// <summary>
        /// Converts a single-precision floating-point value into an integer.
        /// </summary>
        /// <param name="value">The single-precision floating-point value to convert.</param>
        /// <returns>An integer representing the converted single-precision floating-point value.</returns>
        public static int SingleToInt32Bits(float value)
        {
            unsafe
            {
                return *(int*)&value;
            }
        }

        /// <summary>
        /// Reinterprets the specified 32-bit integer as a single-precision floating-point value.
        /// </summary>
        /// <param name="value">The integer to convert.</param>
        /// <returns>A single-precision floating-point value that represents the converted integer.</returns>
        public static float Int32BitsToSingle(int value)
        {
            unsafe
            {
                return *(float*)&value;
            }
        }
#endif

#if !NET6_0_OR_GREATER
        /// <summary>
        /// Converts a single-precision floating-point value into an unsigned integer.
        /// </summary>
        /// <param name="value">The single-precision floating-point value to convert.</param>
        /// <returns>An unsigned integer representing the converted single-precision floating-point value.</returns>
        [CLSCompliant(false)]
        public static uint SingleToUInt32Bits(float value)
        {
            unsafe
            {
                return *(uint*)&value;
            }
        }

        /// <summary>
        /// Converts a double-precision floating-point value into a 64-bit unsigned integer.
        /// </summary>
        /// <param name="value">The double-precision floating-point value to convert.</param>
        /// <returns>An unsigned integer representing the converted double-precision floating-point value.</returns>
        [CLSCompliant(false)]
        public static ulong DoubleToUInt64Bits(double value)
        {
            unsafe
            {
                return *(ulong*)&value;
            }
        }

        /// <summary>
        /// Converts the specified 32-bit unsigned integer to a single-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A single-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [CLSCompliant(false)]
        public static float UInt32BitsToSingle(uint value)
        {
            unsafe
            {
                return *(float*)&value;
            }
        }

        /// <summary>
        /// Converts the specified 64-bit unsigned integer to a double-precision floating point number.
        /// </summary>
        /// <param name="value">The number to convert.</param>
        /// <returns>A double-precision floating point number whose bits are identical to <paramref name="value"/>.</returns>
        [CLSCompliant(false)]
        public static double UInt64BitsToDouble(ulong value)
        {
            unsafe
            {
                return *(double*)&value;
            }
        }

#if NET5_0_OR_GREATER
        /// <summary>
        /// Converts a half-precision floating-point value into a 16-bit integer.
        /// </summary>
        /// <param name="value">The half-precision floating-point value to convert.</param>
        /// <returns>A 16-bit integer representing the converted half-precision floating-point value.</returns>
        public static short HalfToInt16Bits(Half value)
        {
            unsafe
            {
                return *(short*)&value;
            }
        }

        /// <summary>
        /// Converts a half-precision floating-point value into a 16-bit unsigned integer.
        /// </summary>
        /// <param name="value">The half-precision floating-point value to convert.</param>
        /// <returns>A 16-bit unsigned integer representing the converted half-precision floating-point value.</returns>
        [CLSCompliant(false)]
        public static ushort HalfToUInt16Bits(Half value)
        {
            unsafe
            {
                return *(ushort*)&value;
            }
        }

        /// <summary>
        /// Reinterprets the specified 16-bit signed integer value as a half-precision floating-point value.
        /// </summary>
        /// <param name="value">The 16-bit signed integer value to convert.</param>
        /// <returns>A half-precision floating-point value that represents the converted integer.</returns>
        public static Half Int16BitsToHalf(short value)
        {
            unsafe
            {
                return *(Half*)&value;
            }
        }

        /// <summary>
        /// Reinterprets the specified 16-bit unsigned integer value as a half-precision floating-point value.
        /// </summary>
        /// <param name="value">The 16-bit unsigned integer value to convert.</param>
        /// <returns>A half-precision floating-point value that represents the converted integer.</returns>
        [CLSCompliant(false)]
        public static Half UInt16BitsToHalf(ushort value)
        {
            unsafe
            {
                return *(Half*)&value;
            }
        }
#endif
#endif
    }

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable IDE0051, S1144
    private static T ReverseEndiannessIfRequired<T>(T value, Altemiq.ByteOrder byteOrder, Func<T, T> reverseEndiannessFunction)
    {
        return ShouldReverseEndianness(byteOrder)
            ? reverseEndiannessFunction(value)
            : value;

        static bool ShouldReverseEndianness(Altemiq.ByteOrder byteOrder)
        {
            return System.BitConverter.IsLittleEndian
                ? byteOrder is not Altemiq.ByteOrder.LittleEndian
                : byteOrder is not Altemiq.ByteOrder.BigEndian;
        }
    }

    private static char ReverseEndianness(char value) => (char)BinaryPrimitives.ReverseEndianness(unchecked((short)value));

#if NET5_0_OR_GREATER
    private static Half ReverseEndianness(Half value) => System.BitConverter.Int16BitsToHalf(BinaryPrimitives.ReverseEndianness(System.BitConverter.HalfToInt16Bits(value)));
#endif

    private static float ReverseEndianness(float value) => System.BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(System.BitConverter.SingleToInt32Bits(value)));

    private static double ReverseEndianness(double value) => System.BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(System.BitConverter.DoubleToInt64Bits(value)));
#pragma warning restore IDE0051, IDE0079, RCS1222
}