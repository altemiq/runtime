// -----------------------------------------------------------------------
// <copyright file="BitConverter.VarInt.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altavec;

/// <content>
/// The <see cref="BitConverter"/> for variable integers.
/// </content>
public static partial class BitConverter
{
    private const byte MostSignificantBit = 0x80;
    private const ulong ByteValueShift = 0x7F;
    private const int BitShift = 7;

    /// <summary>
    /// Returns the specified signed byte value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">Signed byte value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    [CLSCompliant(false)]
    public static byte[] GetVarBytes(sbyte value) => GetVarBytes((ulong)EncodeZigZag(value, sizeof(sbyte) * 8));

    /// <summary>
    /// Returns the specified byte value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">Byte value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    [CLSCompliant(false)]
    public static byte[] GetVarBytes(byte value) => GetVarBytes((ulong)value);

    /// <summary>
    /// Returns the specified 16-bit signed value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">16-bit signed value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    public static byte[] GetVarBytes(short value) => GetVarBytes((ulong)EncodeZigZag(value, sizeof(short) * 8));

    /// <summary>
    /// Returns the specified 16-bit unsigned value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">16-bit unsigned value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    [CLSCompliant(false)]
    public static byte[] GetVarBytes(ushort value) => GetVarBytes((ulong)value);

    /// <summary>
    /// Returns the specified 32-bit signed value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">32-bit signed value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    public static byte[] GetVarBytes(int value) => GetVarBytes((ulong)EncodeZigZag(value, sizeof(int) * 8));

    /// <summary>
    /// Returns the specified 32-bit unsigned value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">32-bit unsigned value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    [CLSCompliant(false)]
    public static byte[] GetVarBytes(uint value) => GetVarBytes((ulong)value);

    /// <summary>
    /// Returns the specified 64-bit signed value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">64-bit signed value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    public static byte[] GetVarBytes(long value) => GetVarBytes((ulong)EncodeZigZag(value, sizeof(long) * 8));

    /// <summary>
    /// Returns the specified 64-bit unsigned value as <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="value">64-bit unsigned value.</param>
    /// <returns><c>varint</c> array of bytes.</returns>
    [CLSCompliant(false)]
    public static byte[] GetVarBytes(ulong value)
    {
#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
        Span<byte> buffer = stackalloc byte[10];
#else
        var buffer = new byte[10];
#endif

        var pos = 0;
        do
        {
            var byteVal = value & ByteValueShift;
            value >>= BitShift;

            if (value is not 0)
            {
                byteVal |= MostSignificantBit;
            }

            buffer[pos++] = (byte)byteVal;
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

    /// <summary>
    /// Returns signed byte value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>Byte value.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(byte[] bytes, int startIndex, out int bytesRead) => (sbyte)DecodeZigZag(ToTarget(bytes, startIndex, sizeof(sbyte) * 8, out bytesRead));

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns signed byte value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>Byte value.</returns>
    [CLSCompliant(false)]
    public static sbyte ToSByte(ReadOnlySpan<byte> bytes, out int bytesRead) => (sbyte)DecodeZigZag(ToTarget(bytes, sizeof(sbyte) * 8, out bytesRead));
#endif

    /// <summary>
    /// Returns byte value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>Byte value.</returns>
    public static byte ToByte(byte[] bytes, int startIndex, out int bytesRead) => (byte)ToTarget(bytes, startIndex, sizeof(byte) * 8, out bytesRead);

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns byte value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>Byte value.</returns>
    public static byte ToByte(ReadOnlySpan<byte> bytes, out int bytesRead) => (byte)ToTarget(bytes, sizeof(byte) * 8, out bytesRead);
#endif

    /// <summary>
    /// Returns 16-bit signed value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>16-bit signed value.</returns>
    public static short ToInt16(byte[] bytes, int startIndex, out int bytesRead) => (short)DecodeZigZag(ToTarget(bytes, startIndex, sizeof(short) * 8, out bytesRead));

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns 16-bit signed value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>16-bit signed value.</returns>
    public static short ToInt16(ReadOnlySpan<byte> bytes, out int bytesRead) => (short)DecodeZigZag(ToTarget(bytes, sizeof(short) * 8, out bytesRead));
#endif

    /// <summary>
    /// Returns 16-bit usigned value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>16-bit usigned value.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(byte[] bytes, int startIndex, out int bytesRead) => (ushort)ToTarget(bytes, startIndex, sizeof(ushort) * 8, out bytesRead);

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns 16-bit usigned value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>16-bit usigned value.</returns>
    [CLSCompliant(false)]
    public static ushort ToUInt16(ReadOnlySpan<byte> bytes, out int bytesRead) => (ushort)ToTarget(bytes, sizeof(ushort) * 8, out bytesRead);
#endif

    /// <summary>
    /// Returns 32-bit signed value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>32-bit signed value.</returns>
    public static int ToInt32(byte[] bytes, int startIndex, out int bytesRead) => (int)DecodeZigZag(ToTarget(bytes, startIndex, sizeof(int) * 8, out bytesRead));

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns 32-bit signed value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>32-bit signed value.</returns>
    public static int ToInt32(ReadOnlySpan<byte> bytes, out int bytesRead) => (int)DecodeZigZag(ToTarget(bytes, sizeof(int) * 8, out bytesRead));
#endif

    /// <summary>
    /// Returns 32-bit unsigned value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>32-bit unsigned value.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(byte[] bytes, int startIndex, out int bytesRead) => (uint)ToTarget(bytes, startIndex, sizeof(uint) * 8, out bytesRead);

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns 32-bit unsigned value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>32-bit unsigned value.</returns>
    [CLSCompliant(false)]
    public static uint ToUInt32(ReadOnlySpan<byte> bytes, out int bytesRead) => (uint)ToTarget(bytes, sizeof(uint) * 8, out bytesRead);
#endif

    /// <summary>
    /// Returns 64-bit signed value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>64-bit signed value.</returns>
    public static long ToInt64(byte[] bytes, int startIndex, out int bytesRead) => DecodeZigZag(ToTarget(bytes, startIndex, sizeof(long) * 8, out bytesRead));

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns 64-bit signed value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>64-bit signed value.</returns>
    [CLSCompliant(false)]
    public static long ToInt64(ReadOnlySpan<byte> bytes, out int bytesRead) => DecodeZigZag(ToTarget(bytes, sizeof(long) * 8, out bytesRead));
#endif

    /// <summary>
    /// Returns 64-bit unsigned value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="startIndex">The start index.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>64-bit unsigned value.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(byte[] bytes, int startIndex, out int bytesRead) => ToTarget(bytes, startIndex, sizeof(ulong) * 8, out bytesRead);

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Returns 64-bit unsigned value from <c>varint</c> encoded array of bytes.
    /// </summary>
    /// <param name="bytes"><c>varint</c> encoded array of bytes.</param>
    /// <param name="bytesRead">The number of bytes from <paramref name="bytes"/>.</param>
    /// <returns>64-bit unsigned value.</returns>
    [CLSCompliant(false)]
    public static ulong ToUInt64(ReadOnlySpan<byte> bytes, out int bytesRead) => ToTarget(bytes, sizeof(ulong) * 8, out bytesRead);
#endif

#if NETSTANDARD1_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER || NET45_OR_GREATER
    /// <summary>
    /// Converts a 8-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 8-bit signed integer.</param>
    /// <param name="value">The 8-bit signed integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, sbyte value, out int bytesWritten) => TryWriteBytes(destination, (ulong)EncodeZigZag(value, sizeof(sbyte) * 8), out bytesWritten);

    /// <summary>
    /// Converts a 8-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 8-bit unsigned integer.</param>
    /// <param name="value">The 8-bit unsigned integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, byte value, out int bytesWritten) => TryWriteBytes(destination, (ulong)value, out bytesWritten);

    /// <summary>
    /// Converts a 16-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 16-bit signed integer.</param>
    /// <param name="value">The 16-bit signed integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, short value, out int bytesWritten) => TryWriteBytes(destination, (ulong)EncodeZigZag(value, sizeof(short) * 8), out bytesWritten);

    /// <summary>
    /// Converts a 16-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 16-bit unsigned integer.</param>
    /// <param name="value">The 16-bit unsigned integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ushort value, out int bytesWritten) => TryWriteBytes(destination, (ulong)value, out bytesWritten);

    /// <summary>
    /// Converts a 32-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 32-bit signed integer.</param>
    /// <param name="value">The 32-bit signed integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, int value, out int bytesWritten) => TryWriteBytes(destination, (ulong)EncodeZigZag(value, sizeof(int) * 8), out bytesWritten);

    /// <summary>
    /// Converts a 32-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 32-bit unsigned integer.</param>
    /// <param name="value">The 32-bit unsigned integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, uint value, out int bytesWritten) => TryWriteBytes(destination, (ulong)value, out bytesWritten);

    /// <summary>
    /// Converts a 64-bit signed integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 64-bit signed integer.</param>
    /// <param name="value">The 64-bit signed integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    public static bool TryWriteBytes(Span<byte> destination, long value, out int bytesWritten) => TryWriteBytes(destination, (ulong)EncodeZigZag(value, sizeof(long) * 8), out bytesWritten);

    /// <summary>
    /// Converts a 64-bit unsigned integer into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the bytes representing the converted 64-bit unsigned integer.</param>
    /// <param name="value">The 64-bit unsigned integer to convert.</param>
    /// <param name="bytesWritten">The number of bytes written to <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the conversion was successful; <see langword="false"/> otherwise.</returns>
    [CLSCompliant(false)]
    public static bool TryWriteBytes(Span<byte> destination, ulong value, out int bytesWritten)
    {
        var pos = 0;
        do
        {
            if (pos > destination.Length)
            {
                bytesWritten = pos;
                return false;
            }

            var byteVal = value & ByteValueShift;
            value >>= BitShift;

            if (value is not 0)
            {
                byteVal |= MostSignificantBit;
            }

            destination[pos++] = (byte)byteVal;
        }
        while (value is not 0);

        bytesWritten = pos;
        return true;
    }
#endif

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
            var tmp = byteValue & ByteValueShift;
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

        throw new ArgumentException("Cannot decode varint from byte array.", nameof(bytes));
    }
}