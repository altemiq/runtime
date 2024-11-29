// -----------------------------------------------------------------------
// <copyright file="BitConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// Extended <see cref="System.BitConverter"/>.
/// </summary>
public static partial class BitConverter
{
    /// <inheritdoc cref="System.BitConverter.IsLittleEndian" />
    public static readonly bool IsLittleEndian = System.BitConverter.IsLittleEndian;

    /// <inheritdoc cref="System.BitConverter.GetBytes(bool)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(bool value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(bool)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static byte[] GetBytes(bool value, ByteOrder byteOrder) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(char)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(char value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(char)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(char value, ByteOrder byteOrder) => GetBytes(ReverseEndiannessIfRequired(unchecked((short)value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.GetBytes(double)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(double value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(double)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(double value, ByteOrder byteOrder) => GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.GetBytes(Half)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(Half value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(Half)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(Half value, ByteOrder byteOrder) => GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NET5_0_OR_GREATER
    /// <summary>
    /// Returns the specified half-precision floating-point value as an array of bytes.
    /// </summary>
    /// <param name="value">The number to convert.</param>
    /// <returns>An array of bytes with length 2.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(Half value) => System.BitConverter.GetBytes(HalfToUInt16Bits(value));

    /// <inheritdoc cref="GetBytes(Half)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(Half value, ByteOrder byteOrder) => GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

    /// <inheritdoc cref="System.BitConverter.GetBytes(short)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(short value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(short)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(short value, ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.GetBytes(int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(int value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(int value, ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.GetBytes(long)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(long value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(long)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(long value, ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.GetBytes(float)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(float value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(float)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(float value, ByteOrder byteOrder) => GetBytes(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.GetBytes(ushort)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(ushort value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(ushort)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(ushort value, ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.GetBytes(uint)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [CLSCompliant(false)]
    public static byte[] GetBytes(uint value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(uint)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [CLSCompliant(false)]
    public static byte[] GetBytes(uint value, ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.GetBytes(ulong)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(ulong value) => System.BitConverter.GetBytes(value);

    /// <inheritdoc cref="System.BitConverter.GetBytes(ulong)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte[] GetBytes(ulong value, ByteOrder byteOrder) => System.BitConverter.GetBytes(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));

    /// <inheritdoc cref="System.BitConverter.ToBoolean(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool ToBoolean(byte[] value, int startIndex) => System.BitConverter.ToBoolean(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToBoolean(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static bool ToBoolean(byte[] value, int startIndex, ByteOrder byteOrder) => ToBoolean(value, startIndex);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToBoolean(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool ToBoolean(ReadOnlySpan<byte> value) => System.BitConverter.ToBoolean(value);

    /// <inheritdoc cref="System.BitConverter.ToBoolean(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static bool ToBoolean(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ToBoolean(value);
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static bool ToBoolean(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ToBoolean(value);
#endif

    /// <inheritdoc cref="System.BitConverter.ToChar(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static char ToChar(byte[] value, int startIndex) => System.BitConverter.ToChar(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToChar(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static char ToChar(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToChar(value, startIndex), byteOrder, ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToChar(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static char ToChar(ReadOnlySpan<byte> value) => System.BitConverter.ToChar(value);

    /// <inheritdoc cref="System.BitConverter.ToChar(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static char ToChar(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToChar(value), byteOrder, ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte span into a character.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A character representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static char ToChar(ReadOnlySpan<byte> value) => unchecked((char)ToInt16(value));

    /// <summary>
    /// Converts a read-only byte span into a character.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A character representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static char ToChar(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToChar(value), byteOrder, ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToDouble(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(byte[] value, int startIndex) => System.BitConverter.ToDouble(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToDouble(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToDouble(value, startIndex), byteOrder, ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToDouble(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(ReadOnlySpan<byte> value) => System.BitConverter.ToDouble(value);

    /// <inheritdoc cref="System.BitConverter.ToDouble(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToDouble(value), byteOrder, ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte span into a double-precision floating-point value.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A double-precision floating-point value that represents the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(ReadOnlySpan<byte> value) => System.BitConverter.Int64BitsToDouble(IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadInt64BigEndian(value));

    /// <summary>
    /// Converts a read-only byte span into a double-precision floating-point value.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A double-precision floating-point value that represents the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static double ToDouble(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToDouble(value), byteOrder, ReverseEndianness);
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToHalf(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(byte[] value, int startIndex) => System.BitConverter.ToHalf(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToHalf(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToHalf(value, startIndex), byteOrder, ReverseEndianness);

    /// <inheritdoc cref="System.BitConverter.ToHalf(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(ReadOnlySpan<byte> value) => System.BitConverter.ToHalf(value);

    /// <inheritdoc cref="System.BitConverter.ToHalf(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToHalf(value), byteOrder, ReverseEndianness);
#elif NET5_0_OR_GREATER
    /// <summary>
    /// Returns a half-precision floating point number converted from two bytes at a specified position in a byte array.
    /// </summary>
    /// <param name="value">An array of bytes that includes the two bytes to convert.</param>
    /// <param name="startIndex">The starting position within <paramref name="value"/>.</param>
    /// <returns>A half-precision floating point number formed by two bytes beginning at <paramref name="startIndex"/>.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(byte[] value, int startIndex) => UInt16BitsToHalf(System.BitConverter.ToUInt16(value, startIndex));

    /// <inheritdoc cref="ToHalf(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToHalf(value, startIndex), byteOrder, ReverseEndianness);

    /// <summary>
    /// Converts a read-only byte span into a half-precision floating-point value.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A half-precision floating-point value that represents the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(ReadOnlySpan<byte> value) => UInt16BitsToHalf(System.BitConverter.ToUInt16(value));

    /// <inheritdoc cref="ToHalf(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ToHalf(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToHalf(value), byteOrder, ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToInt16(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ToInt16(byte[] value, int startIndex) => System.BitConverter.ToInt16(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToInt16(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ToInt16(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt16(value, startIndex), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToInt16(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ToInt16(ReadOnlySpan<byte> value) => System.BitConverter.ToInt16(value);

    /// <inheritdoc cref="System.BitConverter.ToInt16(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ToInt16(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt16(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte span into a 16-bit signed integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 16-bit signed integer representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ToInt16(ReadOnlySpan<byte> value) => IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt16LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadInt16BigEndian(value);

    /// <summary>
    /// Converts a read-only byte span into a 16-bit signed integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A 16-bit signed integer representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ToInt16(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt16(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToInt32(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(byte[] value, int startIndex) => System.BitConverter.ToInt32(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToInt32(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt32(value, startIndex), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToInt32(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(ReadOnlySpan<byte> value) => System.BitConverter.ToInt32(value);

    /// <inheritdoc cref="System.BitConverter.ToInt32(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt32(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte span into a 32-bit signed integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 32-bit signed integer representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(ReadOnlySpan<byte> value) => IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(value);

    /// <summary>
    /// Converts a read-only byte span into a 32-bit signed integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A 32-bit signed integer representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ToInt32(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt32(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToInt64(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ToInt64(byte[] value, int startIndex) => System.BitConverter.ToInt64(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToInt64(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ToInt64(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt64(value, startIndex), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToInt64(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ToInt64(ReadOnlySpan<byte> value) => System.BitConverter.ToInt64(value);

    /// <inheritdoc cref="System.BitConverter.ToInt64(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ToInt64(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt64(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte span into a 64-bit signed integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 64-bit signed integer representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ToInt64(ReadOnlySpan<byte> value) => IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt64LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadInt64BigEndian(value);

    /// <summary>
    /// Converts a read-only byte span into a 64-bit signed integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A 64-bit signed integer representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ToInt64(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToInt64(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToSingle(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static float ToSingle(byte[] value, int startIndex) => System.BitConverter.ToSingle(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToSingle(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static float ToSingle(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToSingle(value, startIndex), byteOrder, ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToSingle(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static float ToSingle(ReadOnlySpan<byte> value) => System.BitConverter.ToSingle(value);

    /// <inheritdoc cref="System.BitConverter.ToSingle(ReadOnlySpan{byte})" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static float ToSingle(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToSingle(value), byteOrder, ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte span into a single-precision floating-point value.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A single-precision floating-point representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static float ToSingle(ReadOnlySpan<byte> value) => Int32BitsToSingle(IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(value));

    /// <summary>
    /// Converts a read-only byte span into a single-precision floating-point value.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A single-precision floating-point representing the converted bytes.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static float ToSingle(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToSingle(value), byteOrder, ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToString(byte[])" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static string ToString(byte[] value) => System.BitConverter.ToString(value);

    /// <inheritdoc cref="System.BitConverter.ToString(byte[], int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static string ToString(byte[] value, int startIndex) => System.BitConverter.ToString(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToString(byte[], int, int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static string ToString(byte[] value, int startIndex, int length) => System.BitConverter.ToString(value, startIndex, length);

    /// <inheritdoc cref="System.BitConverter.ToUInt16(byte[], int)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ToUInt16(byte[] value, int startIndex) => System.BitConverter.ToUInt16(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToUInt16(byte[], int)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ToUInt16(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt16(value, startIndex), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToUInt16(ReadOnlySpan{byte})" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ToUInt16(ReadOnlySpan<byte> value) => System.BitConverter.ToUInt16(value);

    /// <inheritdoc cref="System.BitConverter.ToUInt16(ReadOnlySpan{byte})" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ToUInt16(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt16(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte-span into a 16-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 16-bit unsigned integer representing the converted bytes.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ToUInt16(ReadOnlySpan<byte> value) => IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadUInt16BigEndian(value);

    /// <summary>
    /// Converts a read-only byte-span into a 16-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A 16-bit unsigned integer representing the converted bytes.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ToUInt16(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt16(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToUInt32(byte[], int)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32(byte[] value, int startIndex) => System.BitConverter.ToUInt32(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToUInt32(byte[], int)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt32(value, startIndex), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToUInt32(ReadOnlySpan{byte})" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32(ReadOnlySpan<byte> value) => System.BitConverter.ToUInt32(value);

    /// <inheritdoc cref="System.BitConverter.ToUInt32(ReadOnlySpan{byte})" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt32(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte-span into a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 32-bit unsigned integer representing the converted bytes.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32(ReadOnlySpan<byte> value) => IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadUInt32BigEndian(value);

    /// <summary>
    /// Converts a read-only byte-span into a 32-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A 32-bit unsigned integer representing the converted bytes.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ToUInt32(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt32(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#endif

    /// <inheritdoc cref="System.BitConverter.ToUInt64(byte[], int)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64(byte[] value, int startIndex) => System.BitConverter.ToUInt64(value, startIndex);

    /// <inheritdoc cref="System.BitConverter.ToUInt64(byte[], int)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64(byte[] value, int startIndex, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt64(value, startIndex), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.ToUInt64(ReadOnlySpan{byte})" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64(ReadOnlySpan<byte> value) => System.BitConverter.ToUInt64(value);

    /// <inheritdoc cref="System.BitConverter.ToUInt64(ReadOnlySpan{byte})" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt64(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a read-only byte-span into a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <returns>A 64-bit unsigned integer representing the converted bytes.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64(ReadOnlySpan<byte> value) => IsLittleEndian ? System.Buffers.Binary.BinaryPrimitives.ReadUInt64LittleEndian(value) : System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(value);

    /// <summary>
    /// Converts a read-only byte-span into a 64-bit unsigned integer.
    /// </summary>
    /// <param name="value">A read-only span containing the bytes to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns>A 64-bit unsigned integer representing the converted bytes.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ToUInt64(ReadOnlySpan<byte> value, ByteOrder byteOrder) => ReverseEndiannessIfRequired(ToUInt64(value), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, bool)" />
    public static bool TryWriteBytes(Span<byte> destination, bool value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, bool)" />
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static bool TryWriteBytes(Span<byte> destination, bool value, ByteOrder byteOrder) => TryWriteBytes(destination, value);
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static bool TryWriteBytes(Span<byte> destination, bool value, ByteOrder byteOrder) => TryWriteBytes(destination, value);
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, char)" />
    public static bool TryWriteBytes(Span<byte> destination, char value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, char)" />
    public static bool TryWriteBytes(Span<byte> destination, char value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a character into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted character.</param>
    /// <param name="value">The character to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, char value) => TryWriteBytes(destination, (short)value);

    /// <summary>
    /// Converts a character into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted character.</param>
    /// <param name="value">The character to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, char value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, double)" />
    public static bool TryWriteBytes(Span<byte> destination, double value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, double)" />
    public static bool TryWriteBytes(Span<byte> destination, double value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a double-precision floating-point into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted double-precision floating-point.</param>
    /// <param name="value">The double-precision floating-point to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, double value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteInt64LittleEndian(destination, System.BitConverter.DoubleToInt64Bits(value))
        : System.Buffers.Binary.BinaryPrimitives.TryWriteInt64BigEndian(destination, System.BitConverter.DoubleToInt64Bits(value));

    /// <summary>
    /// Converts a double-precision floating-point into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted double-precision floating-point.</param>
    /// <param name="value">The double-precision floating-point to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, double value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, Half)" />
    public static bool TryWriteBytes(Span<byte> destination, Half value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, Half)" />
    public static bool TryWriteBytes(Span<byte> destination, Half value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NET5_0_OR_GREATER
    /// <summary>
    /// Converts a half-precision floating-point value into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted half-precision floating-point value.</param>
    /// <param name="value">The half-precision floating-point value to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, Half value) => System.BitConverter.TryWriteBytes(destination, HalfToUInt16Bits(value));

    /// <inheritdoc cref="TryWriteBytes(Span{byte}, Half)" />
    public static bool TryWriteBytes(Span<byte> destination, Half value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, short)" />
    public static bool TryWriteBytes(Span<byte> destination, short value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, short)" />
    public static bool TryWriteBytes(Span<byte> destination, short value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a 16-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 16-bit signed integer.</param>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, short value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteInt16LittleEndian(destination, value)
        : System.Buffers.Binary.BinaryPrimitives.TryWriteInt16BigEndian(destination, value);

    /// <summary>
    /// Converts a 16-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 16-bit signed integer.</param>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, short value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, int)" />
    public static bool TryWriteBytes(Span<byte> destination, int value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, int)" />
    public static bool TryWriteBytes(Span<byte> destination, int value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a 32-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 32-bit signed integer.</param>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, int value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteInt32LittleEndian(destination, value)
        : System.Buffers.Binary.BinaryPrimitives.TryWriteInt32BigEndian(destination, value);

    /// <summary>
    /// Converts a 32-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 32-bit signed integer.</param>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, int value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, long)" />
    public static bool TryWriteBytes(Span<byte> destination, long value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, long)" />
    public static bool TryWriteBytes(Span<byte> destination, long value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a 64-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 64-bit signed integer.</param>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, long value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteInt64LittleEndian(destination, value)
        : System.Buffers.Binary.BinaryPrimitives.TryWriteInt64BigEndian(destination, value);

    /// <summary>
    /// Converts a 64-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 64-bit signed integer.</param>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, long value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, float)" />
    public static bool TryWriteBytes(Span<byte> destination, float value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, float)" />
    public static bool TryWriteBytes(Span<byte> destination, float value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a single-precision floating-point into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted single-precision floating-point.</param>
    /// <param name="value">The single-precision floating-point to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, float value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteInt32LittleEndian(destination, SingleToInt32Bits(value))
        : System.Buffers.Binary.BinaryPrimitives.TryWriteInt32BigEndian(destination, SingleToInt32Bits(value));

    /// <summary>
    /// Converts a single-precision floating-point into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted single-precision floating-point.</param>
    /// <param name="value">The single-precision floating-point to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, float value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, ushort)" />
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ushort value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, ushort)" />
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ushort value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a 16-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 16-bit unsigned integer.</param>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ushort value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16LittleEndian(destination, value)
        : System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16BigEndian(destination, value);

    /// <summary>
    /// Converts a 16-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 16-bit unsigned integer.</param>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ushort value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, uint)" />
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, uint value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, uint)" />
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, uint value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a 32-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 32-bit unsigned integer.</param>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, uint value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteUInt32LittleEndian(destination, value)
        : System.Buffers.Binary.BinaryPrimitives.TryWriteUInt32BigEndian(destination, value);

    /// <summary>
    /// Converts a 32-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 32-bit unsigned integer.</param>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, uint value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, ulong)" />
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ulong value) => System.BitConverter.TryWriteBytes(destination, value);

    /// <inheritdoc cref="System.BitConverter.TryWriteBytes(Span{byte}, ulong)" />
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ulong value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#elif NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP
    /// <summary>
    /// Converts a 64-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 64-bit unsigned integer.</param>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ulong value) => IsLittleEndian
        ? System.Buffers.Binary.BinaryPrimitives.TryWriteUInt64LittleEndian(destination, value)
        : System.Buffers.Binary.BinaryPrimitives.TryWriteUInt64BigEndian(destination, value);

    /// <summary>
    /// Converts a 64-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 64-bit unsigned integer.</param>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <param name="byteOrder">The required byte order.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ulong value, ByteOrder byteOrder) => TryWriteBytes(destination, ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.HalfToInt16Bits(Half)" />
    public static short HalfToInt16Bits(Half value) => System.BitConverter.HalfToInt16Bits(value);
#elif NET5_0_OR_GREATER
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
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.HalfToUInt16Bits(Half)" />
    [CLSCompliant(false)]
    public static ushort HalfToUInt16Bits(Half value) => System.BitConverter.HalfToUInt16Bits(value);
#elif NET5_0_OR_GREATER
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
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.SingleToInt32Bits" />
    public static int SingleToInt32Bits(float value) => System.BitConverter.SingleToInt32Bits(value);
#else
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
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.SingleToUInt32Bits" />
    [CLSCompliant(false)]
    public static uint SingleToUInt32Bits(float value) => System.BitConverter.SingleToUInt32Bits(value);
#else
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
#endif

    /// <inheritdoc cref="System.BitConverter.DoubleToInt64Bits" />
    public static long DoubleToInt64Bits(double value) => System.BitConverter.DoubleToInt64Bits(value);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.DoubleToUInt64Bits(double)" />
    [CLSCompliant(false)]
    public static ulong DoubleToUInt64Bits(double value) => System.BitConverter.DoubleToUInt64Bits(value);
#else
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
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.Int16BitsToHalf" />
    public static Half Int16BitsToHalf(short value) => System.BitConverter.Int16BitsToHalf(value);
#elif NET5_0_OR_GREATER
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
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.UInt16BitsToHalf" />
    [CLSCompliant(false)]
    public static Half UInt16BitsToHalf(ushort value) => System.BitConverter.UInt16BitsToHalf(value);
#elif NET5_0_OR_GREATER
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

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.Int32BitsToSingle" />
    public static float Int32BitsToSingle(int value) => System.BitConverter.Int32BitsToSingle(value);
#else
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

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.UInt32BitsToSingle" />
    [CLSCompliant(false)]
    public static float UInt32BitsToSingle(uint value) => System.BitConverter.UInt32BitsToSingle(value);
#else
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
#endif

    /// <inheritdoc cref="System.BitConverter.Int64BitsToDouble" />
    public static double Int64BitsToDouble(long value) => System.BitConverter.Int64BitsToDouble(value);

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="System.BitConverter.UInt64BitsToDouble" />
    [CLSCompliant(false)]
    public static double UInt64BitsToDouble(ulong value) => System.BitConverter.UInt64BitsToDouble(value);
#else
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
#endif

    private static T ReverseEndiannessIfRequired<T>(T value, ByteOrder byteOrder, Func<T, T> reverseEndianessFunction)
    {
        return ShouldReverseEndiannesss(byteOrder)
            ? reverseEndianessFunction(value)
            : value;

        static bool ShouldReverseEndiannesss(ByteOrder byteOrder)
        {
            return IsLittleEndian
            ? byteOrder is not ByteOrder.LittleEndian
            : byteOrder is not ByteOrder.BigEndian;
        }
    }

    private static char ReverseEndianness(char value) => (char)Buffers.Binary.BinaryPrimitives.ReverseEndianness(unchecked((short)value));

#if NET5_0_OR_GREATER
    private static Half ReverseEndianness(Half value) => Int16BitsToHalf(Buffers.Binary.BinaryPrimitives.ReverseEndianness(HalfToInt16Bits(value)));
#endif

    private static float ReverseEndianness(float value) => Int32BitsToSingle(Buffers.Binary.BinaryPrimitives.ReverseEndianness(SingleToInt32Bits(value)));

    private static double ReverseEndianness(double value) => Int64BitsToDouble(Buffers.Binary.BinaryPrimitives.ReverseEndianness(DoubleToInt64Bits(value)));
}