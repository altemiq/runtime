// -----------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

#pragma warning disable RCS1263, SA1101, S2325

/// <summary>
/// <see cref="Stream"/> extensions.
/// </summary>
internal static class StreamExtensions
{
    /// <content>
    /// <see cref="MemoryStream"/> extensions.
    /// </content>
    /// <param name="stream">The stream.</param>
    extension(MemoryStream stream)
    {
        /// <summary>
        /// Clears the stream.
        /// </summary>
        public void Clear()
        {
#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER
            var buffer = stream.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            stream.Position = 0;
#else
            var length = stream.Length;
            stream.SetLength(0);
            stream.SetLength(length);
            stream.Position = 0;
#endif
        }
    }

    /// <content>
    /// <see cref="Stream"/> extensions.
    /// </content>
    /// <param name="stream">The stream.</param>
    extension(Stream stream)
    {
        /// <summary>
        /// Writes a signed byte to the current stream at the current position.
        /// </summary>
        /// <param name="value">The signed byte to write.</param>
        public void WriteSByte(sbyte value) => stream.WriteByte((byte)value);

        /// <summary>
        /// Reads a signed byte from the current stream at the current position.
        /// </summary>
        /// <returns>The signed byte.</returns>
        public sbyte ReadSByte() => (sbyte)stream.ReadByte();

        /// <summary>
        /// Writes the values to the stream.
        /// </summary>
        /// <param name="buffer">The source buffer.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The number of values to write.</param>
        public void Write(int[] buffer, int offset, int count)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            var bytes = System.Runtime.InteropServices.MemoryMarshal.AsBytes(buffer.AsSpan(offset, count));
            stream.Write(bytes);
#else
            var bytes = new byte[4];
            var end = offset + count;
            for (var i = offset; i < end; i++)
            {
                if (BitConverter.IsLittleEndian)
                {
                    System.Buffers.Binary.BinaryPrimitives.WriteInt32LittleEndian(bytes, buffer[i]);
                }
                else
                {
                    System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(bytes, buffer[i]);
                }

                stream.Write(bytes, 0, 4);
            }
#endif
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <param name="buffer">The destination.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The length.</param>
        /// <param name="byteOrder">The byte ordering.</param>
        /// <returns>The number of values read.</returns>
        public int Read(int[] buffer, int offset, int count, ByteOrder byteOrder)
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            var bytes = System.Runtime.InteropServices.MemoryMarshal.AsBytes(buffer.AsSpan(offset, count));
            var read = stream.Read(bytes);
            count = read / 4;

            if (!BitConverter.IsLittleEndian || byteOrder is not ByteOrder.BigEndian)
            {
                return count;
            }

            var end = offset + count;
            for (var i = offset; i < end; i++)
            {
                buffer[i] = System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(buffer[i]);
            }

            return count;

#else
            var bytes = new byte[count * 4];

            var read = stream.Read(bytes, 0, bytes.Length);
            count = read / 4;

            for (var i = 0; i < count; i++)
            {
                buffer[offset + i] = byteOrder switch
                {
                    ByteOrder.BigEndian => System.Buffers.Binary.BinaryPrimitives.ReadInt32BigEndian(bytes.AsSpan(i * 4, 4)),
                    _ => System.Buffers.Binary.BinaryPrimitives.ReadInt32LittleEndian(bytes.AsSpan(i * 4, 4)),
                };
            }

            return count;
#endif
        }
    }
}