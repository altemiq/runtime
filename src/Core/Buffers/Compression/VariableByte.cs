// -----------------------------------------------------------------------
// <copyright file="VariableByte.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Implementation of variable-byte. For best performance, use it using the <see cref="ISByteCodec"/> interface.
/// </summary>
internal sealed class VariableByte : IInt32Codec, ISByteCodec, IHeadlessInt32Codec
{
    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessCompress(source, destination);

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<sbyte> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var written = 0;
        var length = source.Length;
        for (var k = 0; k < length; k++)
        {
            var val = source[k] & 0xFFFFFFFFL; // To be consistent with unsigned integers in C/C++

            if (val < (1 << 7))
            {
                destination[written++] = (sbyte)(val | (1 << 7));
            }
            else if (val < (1 << 14))
            {
                destination[written++] = Extract7Bits(0, val);
                destination[written++] = (sbyte)(Extract7BitsWithoutMask(1, val) | (1 << 7));
            }
            else if (val < (1 << 21))
            {
                destination[written++] = Extract7Bits(0, val);
                destination[written++] = Extract7Bits(1, val);
                destination[written++] = (sbyte)(Extract7BitsWithoutMask(2, val) | (1 << 7));
            }
            else if (val < (1 << 28))
            {
                destination[written++] = Extract7Bits(0, val);
                destination[written++] = Extract7Bits(1, val);
                destination[written++] = Extract7Bits(2, val);
                destination[written++] = (sbyte)(Extract7BitsWithoutMask(3, val) | (1 << 7));
            }
            else
            {
                destination[written++] = Extract7Bits(0, val);
                destination[written++] = Extract7Bits(1, val);
                destination[written++] = Extract7Bits(2, val);
                destination[written++] = Extract7Bits(3, val);
                destination[written++] = (sbyte)(Extract7BitsWithoutMask(4, val) | (1 << 7));
            }
        }

        return (length, written);
    }

    /// <inheritdoc/>
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessCompress(source, destination);

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = source.Length;
        var index = 0;
        var written = 0;

        var s = 0;
        var v = 0;
        var shift = 0;

        while (index < length)
        {
            var value = source[index];
            int c = (sbyte)(value >>> s);
            s += 8;
            index += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[written++] = v;
                v = 0;
                shift = 0;
            }
            else
            {
                shift += 7;
            }
        }

        return (length, written);
    }

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<sbyte> source, Span<int> destination)
    {
        var length = source.Length;
        var read = 0;
        var written = 0;
        while (read < length)
        {
            var value = source[read] & 0x7F;
            if (source[read] < 0)
            {
                read++;
                Update(destination);
                continue;
            }

            value = ((source[read + 1] & 0x7F) << 7) | value;
            if (source[read + 1] < 0)
            {
                read += 2;
                Update(destination);
                continue;
            }

            value = ((source[read + 2] & 0x7F) << 14) | value;
            if (source[read + 2] < 0)
            {
                read += 3;
                Update(destination);
                continue;
            }

            value = ((source[read + 3] & 0x7F) << 21) | value;
            if (source[read + 3] < 0)
            {
                read += 4;
                Update(destination);
                continue;
            }

            value = ((source[read + 4] & 0x7F) << 28) | value;
            read += 5;
            Update(destination);

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            void Update(Span<int> span)
            {
                span[written++] = value;
            }
        }

        return (read, written);
    }

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessDecompress(source, destination);

    /// <inheritdoc/>
    public override string ToString() => nameof(VariableByte);

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static sbyte Extract7Bits(int i, long val) => (sbyte)((val >> (7 * i)) & ((1 << 7) - 1));

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static sbyte Extract7BitsWithoutMask(int i, long val) => (sbyte)(val >> (7 * i));

    private static (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var length = source.Length;
        var buffer = new MemoryStream(length * 8);
        for (var k = 0; k < length; k++)
        {
            var val = source[k] & 0xFFFFFFFFL; // To be consistent with unsigned integers in C/C++
            switch (val)
            {
                case < 1 << 7:
                    buffer.WriteSByte((sbyte)(val | (1 << 7)));
                    break;
                case < 1 << 14:
                    buffer.WriteSByte(Extract7Bits(0, val));
                    buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(1, val) | (1 << 7)));
                    break;
                case < 1 << 21:
                    buffer.WriteSByte(Extract7Bits(0, val));
                    buffer.WriteSByte(Extract7Bits(1, val));
                    buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(2, val) | (1 << 7)));
                    break;
                case < 1 << 28:
                    buffer.WriteSByte(Extract7Bits(0, val));
                    buffer.WriteSByte(Extract7Bits(1, val));
                    buffer.WriteSByte(Extract7Bits(2, val));
                    buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(3, val) | (1 << 7)));
                    break;
                default:
                    buffer.WriteSByte(Extract7Bits(0, val));
                    buffer.WriteSByte(Extract7Bits(1, val));
                    buffer.WriteSByte(Extract7Bits(2, val));
                    buffer.WriteSByte(Extract7Bits(3, val));
                    buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(4, val) | (1 << 7)));
                    break;
            }
        }

        while (buffer.Position % 4 is not 0)
        {
            buffer.WriteByte(default);
        }

        var destinationLength = (int)(buffer.Position / sizeof(int));
        buffer.Position = 0;
        _ = buffer.Read(destination[..destinationLength], ByteOrder.LittleEndian);
        return (length,  destinationLength);
    }

    private static (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var number = destination.Length;
        var s = 0;
        var p = 0;
        var written = 0;
        var v = 0;
        var shift = 0;

        while (written < number)
        {
            var value = source[p];
            var c = value >>> s;
            s += 8;
            p += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[written++] = v;
                v = 0;
                shift = 0;
            }
            else
            {
                shift += 7;
            }
        }

        return (p + (s is not 0 ? 1 : 0), written);
    }
}