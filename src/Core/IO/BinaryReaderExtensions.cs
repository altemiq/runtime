// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

/// <summary>
/// <see cref="BinaryReader"/> extensions.
/// </summary>
public static class BinaryReaderExtensions
{
    /// <summary>
    /// Reads the specified characters from the string.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="count">The number of characters.</param>
    /// <returns>The string from the reader.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static string ReadString(this BinaryReader reader, int count) => new(reader.ReadChars(count));

    /// <inheritdoc cref="BinaryReader.ReadByte" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static short ReadByte(this BinaryReader reader, ByteOrder byteOrder) => reader.ReadByte();

    /// <inheritdoc cref="BinaryReader.ReadBytes" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static byte[] ReadBytes(this BinaryReader reader, int count, ByteOrder byteOrder) => reader.ReadBytes(count);

    /// <inheritdoc cref="BinaryReader.ReadChar" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static char ReadChar(this BinaryReader reader, ByteOrder byteOrder)
    {
        // check whether this should be reversed
        var c = reader.ReadChar();
        return (uint)c <= '\x007f' ? c : ReverseEndiannessIfRequired(c, byteOrder, ReverseEndianness);
    }

    /// <inheritdoc cref="BinaryReader.ReadChars" />
    public static char[] ReadChars(this BinaryReader reader, int count, ByteOrder byteOrder)
    {
        var chars = reader.ReadChars(count);
        if (byteOrder is not ByteOrder.LittleEndian)
        {
            for (var i = 0; i < count; i++)
            {
                chars[i] = ReverseEndianness(chars[i]);
            }
        }

        return chars;
    }

    /// <inheritdoc cref="BinaryReader.ReadInt16" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ReadInt16(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadInt16(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

    /// <inheritdoc cref="BinaryReader.ReadInt32" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ReadInt32(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadInt32(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

    /// <inheritdoc cref="BinaryReader.ReadInt64" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ReadInt64(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadInt64(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

    /// <inheritdoc cref="BinaryReader.ReadSByte" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static sbyte ReadSByte(this BinaryReader reader, ByteOrder byteOrder) => reader.ReadSByte();

    /// <inheritdoc cref="BinaryReader.ReadUInt16" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ReadUInt16(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadUInt16(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

    /// <inheritdoc cref="BinaryReader.ReadUInt32" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ReadUInt32(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadUInt32(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

    /// <inheritdoc cref="BinaryReader.ReadUInt64" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ReadUInt64(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadUInt64(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

#if NET5_0
    /// <summary>
    /// Reads a 2-byte floating point value from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <returns>A 2-byte floating point value read from the current stream.</returns>
    public static Half ReadHalf(this BinaryReader reader) => BitConverter.Int16BitsToHalf(reader.ReadInt16());
#endif

#if NET5_0_OR_GREATER
    /// <summary>
    /// Reads an 2-byte floating point value from the current stream and advances the current position of the stream by two bytes.
    /// </summary>
    /// <param name="reader">The binary reader.</param>
    /// <param name="byteOrder">The byte order.</param>
    /// <returns>An 2-byte floating point value read from <paramref name="reader"/>.</returns>
    /// <remarks><paramref name="reader"/> reads this data type in the specified <paramref name="byteOrder"/>.</remarks>
    /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
    /// <exception cref="IOException">An I/O error occurs.</exception>
    /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static Half ReadHalf(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadHalf(), byteOrder, ReverseEndianness);
#endif

    /// <inheritdoc cref="BinaryReader.ReadSingle" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static float ReadSingle(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadSingle(), byteOrder, ReverseEndianness);

    /// <inheritdoc cref="BinaryReader.ReadDouble" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static double ReadDouble(this BinaryReader reader, ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadDouble(), byteOrder, ReverseEndianness);

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static T ReverseEndiannessIfRequired<T>(T value, ByteOrder byteOrder, Func<T, T> reverseEndianessFunction) => byteOrder is not ByteOrder.LittleEndian ? reverseEndianessFunction(value) : value;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static char ReverseEndianness(char value) => (char)Buffers.Binary.BinaryPrimitives.ReverseEndianness(unchecked((short)value));

#if NET5_0_OR_GREATER
    private static Half ReverseEndianness(Half value) => BitConverter.Int16BitsToHalf(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.HalfToInt16Bits(value)));
#endif

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static float ReverseEndianness(float value) => BitConverter.Int32BitsToSingle(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value)));

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double ReverseEndianness(double value) => BitConverter.Int64BitsToDouble(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));
}