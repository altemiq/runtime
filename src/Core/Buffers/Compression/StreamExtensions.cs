// -----------------------------------------------------------------------
// <copyright file="StreamExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// <see cref="Stream"/> extensions.
/// </summary>
internal static class StreamExtensions
{
    /// <summary>
    /// Clears the stream.
    /// </summary>
    /// <param name="stream">The stream to clear.</param>
    public static void Clear(this MemoryStream stream)
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

    /// <summary>
    /// Writes a signed byte to the current stream at the current position.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="value">The signed byte to write.</param>
    public static void WriteSByte(this Stream stream, sbyte value) => stream.WriteByte((byte)value);

    /// <summary>
    /// Reads a signed byte from the current stream at the current position.
    /// </summary>
    /// <param name="stream">The stream to read from.</param>
    /// <returns>The signed byte.</returns>
    public static sbyte ReadSByte(this Stream stream) => (sbyte)stream.ReadByte();

    /// <summary>
    /// Writes the values to the stream.
    /// </summary>
    /// <param name="stream">The stream to write to.</param>
    /// <param name="buffer">The source buffer.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="count">The number of values to write.</param>
    public static void Write(this Stream stream, int[] buffer, int offset, int count)
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
    /// <param name="stream">The input stream.</param>
    /// <param name="buffer">The destination.</param>
    /// <param name="offset">The offset.</param>
    /// <param name="count">The length.</param>
    /// <param name="byteOrder">The byte ordering.</param>
    /// <returns>The number of values read.</returns>
    public static int Read(this Stream stream, int[] buffer, int offset, int count, ByteOrder byteOrder)
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