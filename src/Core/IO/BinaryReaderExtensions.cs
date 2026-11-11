// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable CA1050, MA0047, RCS1110

#if NETSTANDARD1_0
using BinaryPrimitives = Altemiq.Buffers.Binary.BinaryPrimitives;
#else
using BinaryPrimitives = System.Buffers.Binary.BinaryPrimitives;
#endif

/// <summary>
/// <see cref="BinaryReader"/> extensions.
/// </summary>
public static class BinaryReaderExtensions
{
    /// <summary><see cref="BinaryReader"/> extensions.</summary>
    /// <param name="reader">The reader.</param>
    extension(BinaryReader reader)
    {
        /// <summary>
        /// Reads the specified characters from the string.
        /// </summary>
        /// <param name="count">The number of characters.</param>
        /// <returns>The string from the reader.</returns>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public string ReadString(int count) => new(reader.ReadChars(count));

        /// <inheritdoc cref="BinaryReader.ReadByte" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public short ReadByte(Altemiq.ByteOrder byteOrder) => reader.ReadByte();

        /// <inheritdoc cref="BinaryReader.ReadBytes" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public byte[] ReadBytes(int count, Altemiq.ByteOrder byteOrder) => reader.ReadBytes(count);

        /// <inheritdoc cref="BinaryReader.ReadChar" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public char ReadChar(Altemiq.ByteOrder byteOrder)
        {
            // check whether this should be reversed
            var c = reader.ReadChar();
            return (uint)c <= '\x007f' ? c : ReverseEndiannessIfRequired(c, byteOrder, ReverseEndianness);
        }

        /// <inheritdoc cref="BinaryReader.ReadChars" />
        public char[] ReadChars(int count, Altemiq.ByteOrder byteOrder)
        {
            var chars = reader.ReadChars(count);
            if (byteOrder is Altemiq.ByteOrder.LittleEndian)
            {
                return chars;
            }

            for (var i = 0; i < count; i++)
            {
                chars[i] = ReverseEndianness(chars[i]);
            }

            return chars;
        }

        /// <inheritdoc cref="BinaryReader.ReadInt16" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public short ReadInt16(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadInt16(), byteOrder, BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadInt32" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int ReadInt32(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadInt32(), byteOrder, BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadInt64" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public long ReadInt64(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadInt64(), byteOrder, BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadSByte" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public sbyte ReadSByte(Altemiq.ByteOrder byteOrder) => reader.ReadSByte();

        /// <inheritdoc cref="BinaryReader.ReadUInt16" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadUInt16(), byteOrder, BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadUInt32" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadUInt32(), byteOrder, BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadUInt64" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadUInt64(), byteOrder, BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadSingle" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public float ReadSingle(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadSingle(), byteOrder, ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadDouble" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadDouble(), byteOrder, ReverseEndianness);

#if NET5_0
        /// <summary>
        /// Reads a 2-byte floating point value from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <returns>A 2-byte floating point value read from the current stream.</returns>
        public Half ReadHalf() => System.BitConverter.Int16BitsToHalf(reader.ReadInt16());
#endif

#if NET5_0_OR_GREATER
        /// <summary>
        /// Reads an 2-byte floating point value from the current stream and advances the current position of the stream by two bytes.
        /// </summary>
        /// <param name="byteOrder">The byte order.</param>
        /// <returns>An 2-byte floating point value read.</returns>
        /// <remarks>This instance reads this data type in the specified <paramref name="byteOrder"/>.</remarks>
        /// <exception cref="EndOfStreamException">The end of the stream is reached.</exception>
        /// <exception cref="IOException">An I/O error occurs.</exception>
        /// <exception cref="ObjectDisposedException">The stream is closed.</exception>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public Half ReadHalf(Altemiq.ByteOrder byteOrder) => ReverseEndiannessIfRequired(reader.ReadHalf(), byteOrder, ReverseEndianness);
#endif
    }

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static T ReverseEndiannessIfRequired<T>(T value, Altemiq.ByteOrder byteOrder, Func<T, T> reverseEndiannessFunction) => byteOrder is not Altemiq.ByteOrder.LittleEndian ? reverseEndiannessFunction(value) : value;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static char ReverseEndianness(char value) => (char)BinaryPrimitives.ReverseEndianness(unchecked((short)value));

#if NET5_0_OR_GREATER
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static Half ReverseEndianness(Half value) => System.BitConverter.Int16BitsToHalf(BinaryPrimitives.ReverseEndianness(System.BitConverter.HalfToInt16Bits(value)));
#endif

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static float ReverseEndianness(float value) => System.BitConverter.Int32BitsToSingle(BinaryPrimitives.ReverseEndianness(System.BitConverter.SingleToInt32Bits(value)));

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double ReverseEndianness(double value) => System.BitConverter.Int64BitsToDouble(BinaryPrimitives.ReverseEndianness(System.BitConverter.DoubleToInt64Bits(value)));
}