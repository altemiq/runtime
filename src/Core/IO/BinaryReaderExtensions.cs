// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

#pragma warning disable RCS1263, SA1101, S2325

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
        public short ReadByte(ByteOrder byteOrder) => reader.ReadByte();

        /// <inheritdoc cref="BinaryReader.ReadBytes" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public byte[] ReadBytes(int count, ByteOrder byteOrder) => reader.ReadBytes(count);

        /// <inheritdoc cref="BinaryReader.ReadChar" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public char ReadChar(ByteOrder byteOrder)
        {
            // check whether this should be reversed
            var c = reader.ReadChar();
            return (uint)c <= '\x007f' ? c : BinaryReader.ReverseEndiannessIfRequired(c, byteOrder, ReverseEndianness);
        }

        /// <inheritdoc cref="BinaryReader.ReadChars" />
        public char[] ReadChars(int count, ByteOrder byteOrder)
        {
            var chars = reader.ReadChars(count);
            if (byteOrder is ByteOrder.LittleEndian)
            {
                return chars;
            }

            for (var i = 0; i < count; i++)
            {
                chars[i] = BinaryReader.ReverseEndianness(chars[i]);
            }

            return chars;
        }

        /// <inheritdoc cref="BinaryReader.ReadInt16" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public short ReadInt16(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadInt16(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadInt32" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public int ReadInt32(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadInt32(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadInt64" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public long ReadInt64(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadInt64(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadSByte" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public sbyte ReadSByte(ByteOrder byteOrder) => reader.ReadSByte();

        /// <inheritdoc cref="BinaryReader.ReadUInt16" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ushort ReadUInt16(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadUInt16(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadUInt32" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadUInt32(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadUInt64" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public ulong ReadUInt64(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadUInt64(), byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadSingle" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public float ReadSingle(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadSingle(), byteOrder, ReverseEndianness);

        /// <inheritdoc cref="BinaryReader.ReadDouble" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public double ReadDouble(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadDouble(), byteOrder, ReverseEndianness);

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
        public Half ReadHalf(ByteOrder byteOrder) => BinaryReader.ReverseEndiannessIfRequired(reader.ReadHalf(), byteOrder, BinaryReader.ReverseEndianness);
#endif

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static T ReverseEndiannessIfRequired<T>(T value, ByteOrder byteOrder, Func<T, T> reverseEndiannessFunction) => byteOrder is not ByteOrder.LittleEndian ? reverseEndiannessFunction(value) : value;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static char ReverseEndianness(char value) => (char)Buffers.Binary.BinaryPrimitives.ReverseEndianness(unchecked((short)value));

#if NET5_0_OR_GREATER
        private static Half ReverseEndianness(Half value) => System.BitConverter.Int16BitsToHalf(Buffers.Binary.BinaryPrimitives.ReverseEndianness(System.BitConverter.HalfToInt16Bits(value)));
#endif

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static float ReverseEndianness(float value) => System.BitConverter.Int32BitsToSingle(Buffers.Binary.BinaryPrimitives.ReverseEndianness(System.BitConverter.SingleToInt32Bits(value)));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static double ReverseEndianness(double value) => BitConverter.Int64BitsToDouble(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));
    }
}