// -----------------------------------------------------------------------
// <copyright file="BitConverter.VarInt.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <content>
/// The <see cref="BitConverter"/> for variable integers.
/// </content>
public static partial class BitConverter
{
    /// <summary>
    /// Extension methods for <see cref="System.BitConverter"/> for variable integers.
    /// </summary>
#pragma warning disable SA1137, SA1400, S1144
    extension(System.BitConverter)
    {
        /// <summary>
        /// Returns the specified signed byte value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">Signed byte value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        [CLSCompliant(false)]
        public static byte[] GetVarBytes(sbyte value) =>
#if NET7_0_OR_GREATER
            GetVarBytesCore(EncodeZigZag<sbyte, byte>(value, sizeof(sbyte) * 8));
#else
        GetVarBytesCore((ulong)EncodeZigZag(value, sizeof(sbyte) * 8));
#endif

        /// <summary>
        /// Returns the specified byte value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">Byte value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        [CLSCompliant(false)]
        public static byte[] GetVarBytes(byte value) => GetVarBytesCore(value);

        /// <summary>
        /// Returns the specified 16-bit signed value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">16-bit signed value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        public static byte[] GetVarBytes(short value) =>
#if NET7_0_OR_GREATER
            GetVarBytesCore(EncodeZigZag<short, ushort>(value, sizeof(short) * 8));
#else
            GetVarBytesCore((ulong)EncodeZigZag(value, sizeof(short) * 8));
#endif

        /// <summary>
        /// Returns the specified 16-bit unsigned value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">16-bit unsigned value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        [CLSCompliant(false)]
        public static byte[] GetVarBytes(ushort value) => GetVarBytesCore(value);

        /// <summary>
        /// Returns the specified 32-bit signed value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">32-bit signed value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        public static byte[] GetVarBytes(int value) =>
#if NET7_0_OR_GREATER
            GetVarBytesCore(EncodeZigZag<int, uint>(value, sizeof(int) * 8));
#else
            GetVarBytesCore((ulong)EncodeZigZag(value, sizeof(int) * 8));
#endif

        /// <summary>
        /// Returns the specified 32-bit unsigned value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">32-bit unsigned value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        [CLSCompliant(false)]
        public static byte[] GetVarBytes(uint value) => GetVarBytesCore(value);

        /// <summary>
        /// Returns the specified 64-bit signed value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">64-bit signed value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        public static byte[] GetVarBytes(long value) =>
#if NET7_0_OR_GREATER
            GetVarBytesCore(EncodeZigZag<long, ulong>(value, sizeof(long) * 8));
#else
            GetVarBytesCore((ulong)EncodeZigZag(value, sizeof(long) * 8));
#endif

        /// <summary>
        /// Returns the specified 64-bit unsigned value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">64-bit unsigned value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        [CLSCompliant(false)]
        public static byte[] GetVarBytes(ulong value) => GetVarBytesCore(value);

#if NET7_0_OR_GREATER
        /// <summary>
        /// Returns the specified 64-bit signed value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">64-bit signed value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        public static byte[] GetVarBytes(Int128 value) => GetVarBytesCore(EncodeZigZag<Int128, UInt128>(value, 128));

        /// <summary>
        /// Returns the specified 64-bit unsigned value as <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="value">64-bit unsigned value.</param>
        /// <returns><c>varint</c> array of bytes.</returns>
        [CLSCompliant(false)]
        public static byte[] GetVarBytes(UInt128 value) => GetVarBytesCore(value);
#endif

        /// <summary>
        /// Returns signed byte value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>Byte value.</returns>
        [CLSCompliant(false)]
        public static sbyte ToSByte(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<byte, sbyte>(ToTarget<byte>(bytes, startIndex, sizeof(sbyte) * 8, out bytesRead));
#else
            (sbyte)DecodeZigZag(ToTarget(bytes, startIndex, sizeof(sbyte) * 8, out bytesRead));
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns signed byte value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>Byte value.</returns>
        [CLSCompliant(false)]
        public static sbyte ToSByte(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<byte, sbyte>(ToTarget<byte>(bytes, sizeof(sbyte) * 8, out bytesRead));
#else
            (sbyte)DecodeZigZag(ToTarget(bytes, sizeof(sbyte) * 8, out bytesRead));
#endif
#endif

        /// <summary>
        /// Returns byte value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>Byte value.</returns>
        public static byte ToByte(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<byte>(bytes, startIndex, sizeof(byte) * 8, out bytesRead);
#else
            (byte)ToTarget(bytes, startIndex, sizeof(byte) * 8, out bytesRead);
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns byte value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>Byte value.</returns>
        public static byte ToByte(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<byte>(bytes, sizeof(byte) * 8, out bytesRead);
#else
            (byte)ToTarget(bytes, sizeof(byte) * 8, out bytesRead);
#endif
#endif

        /// <summary>
        /// Returns 16-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>16-bit signed value.</returns>
        public static short ToInt16(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<ushort, short>(ToTarget<ushort>(bytes, startIndex, sizeof(short) * 8, out bytesRead));
#else
            (short)DecodeZigZag(ToTarget(bytes, startIndex, sizeof(short) * 8, out bytesRead));
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns 16-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>16-bit signed value.</returns>
        public static short ToInt16(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<ushort, short>(ToTarget<ushort>(bytes, sizeof(short) * 8, out bytesRead));
#else
            (short)DecodeZigZag(ToTarget(bytes, sizeof(short) * 8, out bytesRead));
#endif
#endif

        /// <summary>
        /// Returns 16-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>16-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static ushort ToUInt16(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<ushort>(bytes, startIndex, sizeof(ushort) * 8, out bytesRead);
#else
            (ushort)ToTarget(bytes, startIndex, sizeof(ushort) * 8, out bytesRead);
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns 16-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>16-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static ushort ToUInt16(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<ushort>(bytes, sizeof(ushort) * 8, out bytesRead);
#else
            (ushort)ToTarget(bytes, sizeof(ushort) * 8, out bytesRead);
#endif
#endif

        /// <summary>
        /// Returns 32-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>32-bit signed value.</returns>
        public static int ToInt32(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<uint, int>(ToTarget<uint>(bytes, startIndex, sizeof(int) * 8, out bytesRead));
#else
            (int)DecodeZigZag(ToTarget(bytes, startIndex, sizeof(int) * 8, out bytesRead));
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns 32-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>32-bit signed value.</returns>
        public static int ToInt32(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<uint, int>(ToTarget<uint>(bytes, sizeof(int) * 8, out bytesRead));
#else
            (int)DecodeZigZag(ToTarget(bytes, sizeof(int) * 8, out bytesRead));
#endif
#endif

        /// <summary>
        /// Returns 32-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>32-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static uint ToUInt32(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<uint>(bytes, startIndex, sizeof(uint) * 8, out bytesRead);
#else
            (uint)ToTarget(bytes, startIndex, sizeof(uint) * 8, out bytesRead);
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns 32-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>32-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static uint ToUInt32(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<uint>(bytes, sizeof(uint) * 8, out bytesRead);
#else
            (uint)ToTarget(bytes, sizeof(uint) * 8, out bytesRead);
#endif
#endif

        /// <summary>
        /// Returns 64-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit signed value.</returns>
        public static long ToInt64(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<ulong, long>(ToTarget<ulong>(bytes, startIndex, sizeof(long) * 8, out bytesRead));
#else
            DecodeZigZag(ToTarget(bytes, startIndex, sizeof(long) * 8, out bytesRead));
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns 64-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit signed value.</returns>
        [CLSCompliant(false)]
        public static long ToInt64(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            DecodeZigZag<ulong, long>(ToTarget<ulong>(bytes, sizeof(long) * 8, out bytesRead));
#else
            DecodeZigZag(ToTarget(bytes, sizeof(long) * 8, out bytesRead));
#endif
#endif

        /// <summary>
        /// Returns 64-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static ulong ToUInt64(byte[] bytes, int startIndex, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<ulong>(bytes, startIndex, sizeof(ulong) * 8, out bytesRead);
#else
            ToTarget(bytes, startIndex, sizeof(ulong) * 8, out bytesRead);
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Returns 64-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static ulong ToUInt64(ReadOnlySpan<byte> bytes, out int bytesRead) =>
#if NET7_0_OR_GREATER
            ToTarget<ulong>(bytes, sizeof(ulong) * 8, out bytesRead);
#else
            ToTarget(bytes, sizeof(ulong) * 8, out bytesRead);
#endif
#endif

#if NET7_0_OR_GREATER
        /// <summary>
        /// Returns 64-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit signed value.</returns>
        public static Int128 ToInt128(byte[] bytes, int startIndex, out int bytesRead) => DecodeZigZag<UInt128, Int128>(ToTarget<UInt128>(bytes, startIndex, 128, out bytesRead));

        /// <summary>
        /// Returns 64-bit signed value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit signed value.</returns>
        [CLSCompliant(false)]
        public static Int128 ToInt128(ReadOnlySpan<byte> bytes, out int bytesRead) => DecodeZigZag<UInt128, Int128>(ToTarget<UInt128>(bytes, 128, out bytesRead));

        /// <summary>
        /// Returns 64-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="startIndex">The start index.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static UInt128 ToUInt128(byte[] bytes, int startIndex, out int bytesRead) => ToTarget<UInt128>(bytes, startIndex, 128, out bytesRead);

        /// <summary>
        /// Returns 64-bit unsigned value from <c>varint</c> encoded array of bytes.
        /// </summary>
        /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
        /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
        /// <returns>64-bit unsigned value.</returns>
        [CLSCompliant(false)]
        public static UInt128 ToUInt128(ReadOnlySpan<byte> bytes, out int bytesRead) => ToTarget<UInt128>(bytes, 128, out bytesRead);
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        /// <summary>
        /// Converts an 8-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 8-bit signed integer.</param>
        /// <param name="value">The 8-bit signed integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, sbyte value, out int bytesWritten) =>
#if NET7_0_OR_GREATER
            TryWriteBytesCore(destination, EncodeZigZag<sbyte, byte>(value, sizeof(sbyte) * 8), out bytesWritten);
#else
            TryWriteBytesCore(destination, (ulong)EncodeZigZag(value, sizeof(sbyte) * 8), out bytesWritten);
#endif

        /// <summary>
        /// Converts an 8-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 8-bit unsigned integer.</param>
        /// <param name="value">The 8-bit unsigned integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, byte value, out int bytesWritten) => TryWriteBytesCore(destination, value, out bytesWritten);

        /// <summary>
        /// Converts a 16-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 16-bit signed integer.</param>
        /// <param name="value">The 16-bit signed integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, short value, out int bytesWritten) =>
#if NET7_0_OR_GREATER
            TryWriteBytesCore(destination, EncodeZigZag<short, ushort>(value, sizeof(short) * 8), out bytesWritten);
#else
            TryWriteBytesCore(destination, (ulong)EncodeZigZag(value, sizeof(short) * 8), out bytesWritten);
#endif

        /// <summary>
        /// Converts a 16-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 16-bit unsigned integer.</param>
        /// <param name="value">The 16-bit unsigned integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ushort value, out int bytesWritten) => TryWriteBytesCore(destination, value, out bytesWritten);

        /// <summary>
        /// Converts a 32-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 32-bit signed integer.</param>
        /// <param name="value">The 32-bit signed integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, int value, out int bytesWritten) =>
#if NET7_0_OR_GREATER
            TryWriteBytesCore(destination, EncodeZigZag<int, uint>(value, sizeof(int) * 8), out bytesWritten);
#else
            TryWriteBytesCore(destination, (ulong)EncodeZigZag(value, sizeof(int) * 8), out bytesWritten);
#endif

        /// <summary>
        /// Converts a 32-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 32-bit unsigned integer.</param>
        /// <param name="value">The 32-bit unsigned integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, uint value, out int bytesWritten) => TryWriteBytesCore(destination, value, out bytesWritten);

        /// <summary>
        /// Converts a 64-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit signed integer.</param>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, long value, out int bytesWritten) =>
#if NET7_0_OR_GREATER
            TryWriteBytesCore(destination, EncodeZigZag<long, ulong>(value, sizeof(long) * 8), out bytesWritten);
#else
            TryWriteBytesCore(destination, (ulong)EncodeZigZag(value, sizeof(long) * 8), out bytesWritten);
#endif

        /// <summary>
        /// Converts a 64-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit unsigned integer.</param>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, ulong value, out int bytesWritten) => TryWriteBytesCore(destination, value, out bytesWritten);

#if NET7_0_OR_GREATER
        /// <summary>
        /// Converts a 64-bit signed integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit signed integer.</param>
        /// <param name="value">The 64-bit signed integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        public static bool TryWriteBytes(Span<byte> destination, Int128 value, out int bytesWritten) => TryWriteBytesCore(destination, EncodeZigZag<Int128, UInt128>(value, 128), out bytesWritten);

        /// <summary>
        /// Converts a 64-bit unsigned integer into a span of bytes.
        /// </summary>
        /// <param name="destination">When this method returns, the bytes representing the converted 64-bit unsigned integer.</param>
        /// <param name="value">The 64-bit unsigned integer to convert.</param>
        /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
        /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
        [CLSCompliant(false)]
        public static bool TryWriteBytes(Span<byte> destination, UInt128 value, out int bytesWritten) => TryWriteBytesCore(destination, value, out bytesWritten);
#endif
#endif

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable IDE0051, S1144
#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
#if NET7_0_OR_GREATER
        private static bool TryWriteBytesCore<T>(Span<byte> destination, T value, out int bytesWritten)
            where T : System.Numerics.IBinaryInteger<T>
        {
            var valueMask = T.CreateChecked(ValueMask);
            var pos = 0;
            do
            {
                if (pos > destination.Length)
                {
                    bytesWritten = pos;
                    return false;
                }

                var byteValue = byte.CreateChecked(value & valueMask);
                value >>= BitShift;

                if (!T.IsZero(value))
                {
                    byteValue |= MostSignificantBit;
                }

                destination[pos++] = byteValue;
            }
            while (!T.IsZero(value));

            bytesWritten = pos;
            return true;
        }
#else
        private static bool TryWriteBytesCore(Span<byte> destination, ulong value, out int bytesWritten)
        {
            var pos = 0;
            do
            {
                if (pos > destination.Length)
                {
                    bytesWritten = pos;
                    return false;
                }

                var byteValue = value & ValueMask;
                value >>= BitShift;

                if (value is not 0)
                {
                    byteValue |= MostSignificantBit;
                }

                destination[pos++] = (byte)byteValue;
            }
            while (value is not 0);

            bytesWritten = pos;
            return true;
        }
#endif
#endif

#if NET7_0_OR_GREATER
        private static byte[] GetVarBytesCore<T>(T value)
            where T : System.Numerics.IBinaryInteger<T>
        {
            var valueMask = T.CreateChecked(ValueMask);
            Span<byte> buffer = stackalloc byte[20];

            var pos = 0;
            do
            {
                var byteValue = byte.CreateChecked(value & valueMask);
                value >>= BitShift;

                if (!T.IsZero(value))
                {
                    byteValue |= MostSignificantBit;
                }

                buffer[pos++] = byteValue;
            }
            while (!T.IsZero(value));

            return buffer[..pos].ToArray();
        }

        private static TOutput EncodeZigZag<TInput, TOutput>(TInput value, int bitLength)
            where TInput : System.Numerics.IBinaryInteger<TInput>, System.Numerics.ISignedNumber<TInput>
            where TOutput : System.Numerics.IBinaryInteger<TOutput>, System.Numerics.IUnsignedNumber<TOutput> =>
            TOutput.CreateTruncating((value << 1) ^ (value >> (bitLength - 1)));

        private static TOutput DecodeZigZag<TInput, TOutput>(TInput value)
            where TInput : System.Numerics.IBinaryInteger<TInput>, System.Numerics.IUnsignedNumber<TInput>
            where TOutput : System.Numerics.ISignedNumber<TOutput>
        {
            return TOutput.CreateTruncating(CreateValue(value));

            static TInput CreateValue(TInput value)
            {
                return (value & TInput.One) == TInput.One
                    ? ((value >> 1) + TInput.One) * -TInput.One
                    : value >> 1;
            }
        }

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static T ToTarget<T>(byte[] bytes, int startIndex, int sizeBites, out int bytesRead)
            where T : struct, System.Numerics.IBinaryInteger<T>, System.Numerics.IUnsignedNumber<T> => ToTarget<T>(bytes.AsSpan(startIndex), sizeBites, out bytesRead);

        private static T ToTarget<T>(ReadOnlySpan<byte> bytes, int sizeBites, out int bytesRead)
            where T : struct, System.Numerics.IBinaryInteger<T>, System.Numerics.IUnsignedNumber<T>
        {
            int shift = default;
            T result = default;

            var length = bytes.Length;
            for (var i = 0; i < length; i++)
            {
                var byteValue = bytes[i];
                var tmp = T.CreateChecked(byteValue & ValueMask);
                result |= tmp << shift;

                if (shift > sizeBites)
                {
                    throw new ArgumentOutOfRangeException(nameof(bytes), Properties.Resources.ByteArrayTooLarge);
                }

                if ((byteValue & MostSignificantBit) is not MostSignificantBit)
                {
                    bytesRead = i + 1;
                    return result;
                }

                shift += BitShift;
            }

            throw new ArgumentException(Properties.Resources.CanNotDecodeVarInt, nameof(bytes));
        }
#else
        private static byte[] GetVarBytesCore(ulong value)
        {
#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
            Span<byte> buffer = stackalloc byte[10];
#else
            var buffer = new byte[10];
#endif

            var pos = 0;
            do
            {
                var byteValue = value & ValueMask;
                value >>= BitShift;

                if (value is not 0)
                {
                    byteValue |= MostSignificantBit;
                }

                buffer[pos++] = (byte)byteValue;
            }
            while (value is not 0);

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
            return buffer[..pos].ToArray();
#else
            var result = new byte[pos];
            Buffer.BlockCopy(buffer, 0, result, 0, pos);
            return result;
#endif
        }

        private static long EncodeZigZag(long value, int bitLength) => (value << 1) ^ (value >> (bitLength - 1));

        private static long DecodeZigZag(ulong value) => (value & 0x1) is 0x1
            ? -1 * ((long)(value >> 1) + 1)
            : (long)(value >> 1);

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static ulong ToTarget(byte[] bytes, int startIndex, int sizeBites, out int bytesRead) => ToTarget(bytes.AsSpan(startIndex), sizeBites, out bytesRead);

        private static ulong ToTarget(ReadOnlySpan<byte> bytes, int sizeBites, out int bytesRead)
        {
            const int startIndex = 0;
#else
        private static ulong ToTarget(byte[] bytes, int startIndex, int sizeBites, out int bytesRead)
        {
#endif
            int shift = default;
            ulong result = default;

            var length = bytes.Length;
            for (var i = startIndex; i < length; i++)
            {
                var byteValue = bytes[i];
                var tmp = byteValue & ValueMask;
                result |= tmp << shift;

                if (shift > sizeBites)
                {
                    throw new ArgumentOutOfRangeException(nameof(bytes), "Byte array is too large.");
                }

                if ((byteValue & MostSignificantBit) is not MostSignificantBit)
                {
                    bytesRead = i - startIndex + 1;
                    return result;
                }

                shift += BitShift;
            }

            throw new ArgumentException("Cannot decode variable integer from byte array.", nameof(bytes));
        }
#endif
#pragma warning restore IDE0051, S1144, RCS1222, IDE0079
    }

#pragma warning disable SA1201
    private const byte MostSignificantBit = 0x80;

    private const
#if NET7_0_OR_GREATER
        byte
#else
        ulong
#endif
        ValueMask = 0x7F;

    private const int BitShift = 7;
#pragma warning restore SA1137, SA1201, SA1400, S1144
}