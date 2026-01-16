// -----------------------------------------------------------------------
// <copyright file="DifferentialVariableByte.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// Implementation of variable-byte with differential coding. For best performance, use it using the <see cref="IDifferentialSByteCodec"/> interface.
/// </summary>
/// <remarks>
/// You should only use this scheme on sorted arrays. Use <see cref="VariableByte"/> if you have unsorted arrays.
/// </remarks>
internal sealed class DifferentialVariableByte : IDifferentialInt32Codec, IDifferentialSByteCodec, IHeadlessDifferentialInt32Codec
{
    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var initOffset = 0;
        var length = source.Length;

        var buf = new MemoryStream(length * 8);

        for (var k = 0; k < length; k++)
        {
            var val = (source[k] - initOffset) & 0xFFFFFFFFL; // To be consistent with unsigned integers in C/C++
            initOffset = source[k];
            if (val < (1 << 7))
            {
                buf.WriteSByte((sbyte)(val | (1 << 7)));
            }
            else if (val < (1 << 14))
            {
                buf.WriteSByte(Extract7Bits(0, val));
                buf.WriteSByte((sbyte)(Extract7BitsWithoutMask(1, val) | (1 << 7)));
            }
            else if (val < (1 << 21))
            {
                buf.WriteSByte(Extract7Bits(0, val));
                buf.WriteSByte(Extract7Bits(1, val));
                buf.WriteSByte((sbyte)(Extract7BitsWithoutMask(2, val) | (1 << 7)));
            }
            else if (val < (1 << 28))
            {
                buf.WriteSByte(Extract7Bits(0, val));
                buf.WriteSByte(Extract7Bits(1, val));
                buf.WriteSByte(Extract7Bits(2, val));
                buf.WriteSByte((sbyte)(Extract7BitsWithoutMask(3, val) | (1 << 7)));
            }
            else
            {
                buf.WriteSByte(Extract7Bits(0, val));
                buf.WriteSByte(Extract7Bits(1, val));
                buf.WriteSByte(Extract7Bits(2, val));
                buf.WriteSByte(Extract7Bits(3, val));
                buf.WriteSByte((sbyte)(Extract7BitsWithoutMask(4, val) | (1 << 7)));
            }
        }

        while (buf.Position % 4 is not 0)
        {
            buf.WriteByte(0);
        }

        var destinationLength = (int)(buf.Position / sizeof(int));
        buf.Position = 0;
        _ = buf.Read(destination[..destinationLength], ByteOrder.LittleEndian);

        return (length, destinationLength);
    }

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<sbyte> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var initialOffset = 0;
        var written = 0;
        var length = source.Length;
        for (var k = 0; k < length; k++)
        {
            // To be consistent with unsigned integers in C/C++
            var val = (source[k] - initialOffset) & 0xFFFFFFFFL;
            initialOffset = source[k];
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
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = source.Length;
        var index = 0;
        var written = 0;
        var initialOffset = 0;

        var s = 0;
        var v = 0;
        var shift = 0;

        while (index < length)
        {
            var val = source[index];
            int c = (sbyte)(val >>> s);
            s += 8;
            index += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[written] = v + initialOffset;
                initialOffset = destination[written];
                written++;
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
        var index = 0;
        var offset = 0;
        var written = 0;
        var length = source.Length;
        while (index < length)
        {
            var v = source[index] & 0x7F;
            if (source[index] < 0)
            {
                index++;
                Update(destination);
                continue;
            }

            v = ((source[index + 1] & 0x7F) << 7) | v;
            if (source[index + 1] < 0)
            {
                index += 2;
                Update(destination);
                continue;
            }

            v = ((source[index + 2] & 0x7F) << 14) | v;
            if (source[index + 2] < 0)
            {
                index += 3;
                Update(destination);
                continue;
            }

            v = ((source[index + 3] & 0x7F) << 21) | v;
            if (source[index + 3] < 0)
            {
                index += 4;
                Update(destination);
                continue;
            }

            v = ((source[index + 4] & 0x7F) << 28) | v;
            index += 5;
            Update(destination);

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            void Update(Span<int> span)
            {
                offset += v;
                span[written++] = offset;
            }
        }

        return (index, written);
    }

    /// <inheritdoc/>
    (int Read, int Written) ICompressHeadlessDifferentialCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var initialOffset = initialValue;
        initialValue = source[^1];
        var length = source.Length;
        var byteBuffer = new MemoryStream(length * 8);

        for (var k = 0; k < length; k++)
        {
            var value = (source[k] - initialOffset) & 0xFFFFFFFFL;  // To be consistent with unsigned integers in C/C++
            initialOffset = source[k];
            if (value < (1 << 7))
            {
                byteBuffer.WriteSByte((sbyte)(value | (1 << 7)));
            }
            else if (value < (1 << 14))
            {
                byteBuffer.WriteSByte(Extract7Bits(0, value));
                byteBuffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(1, value) | (1 << 7)));
            }
            else if (value < (1 << 21))
            {
                byteBuffer.WriteSByte(Extract7Bits(0, value));
                byteBuffer.WriteSByte(Extract7Bits(1, value));
                byteBuffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(2, value) | (1 << 7)));
            }
            else if (value < (1 << 28))
            {
                byteBuffer.WriteSByte(Extract7Bits(0, value));
                byteBuffer.WriteSByte(Extract7Bits(1, value));
                byteBuffer.WriteSByte(Extract7Bits(2, value));
                byteBuffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(3, value) | (1 << 7)));
            }
            else
            {
                byteBuffer.WriteSByte(Extract7Bits(0, value));
                byteBuffer.WriteSByte(Extract7Bits(1, value));
                byteBuffer.WriteSByte(Extract7Bits(2, value));
                byteBuffer.WriteSByte(Extract7Bits(3, value));
                byteBuffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(4, value) | (1 << 7)));
            }
        }

        while (byteBuffer.Position % 4 is not 0)
        {
            byteBuffer.WriteByte(0);
        }

        var written = (int)(byteBuffer.Position / sizeof(int));
        byteBuffer.Position = 0;
        _ = byteBuffer.Read(destination[..written], ByteOrder.LittleEndian);

        return (length, written);
    }

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessDifferentialCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue)
    {
        var number = destination.Length;
        if (number is 0)
        {
            return default;
        }

        var s = 0;
        var p = 0;
        var initialOffset = initialValue;
        var written = 0;
        var v = 0;
        var shift = 0;

        while (written < number)
        {
            var val = source[p];
            var c = val >>> s;
            s += 8;
            p += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[written++] = initialOffset += v;
                v = 0;
                shift = 0;
            }
            else
            {
                shift += 7;
            }
        }

        initialValue = destination[written - 1];
        return (p + (s is not 0 ? 1 : 0), written);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(DifferentialVariableByte);

    private static sbyte Extract7Bits(int i, long val) => (sbyte)((val >> (7 * i)) & ((1 << 7) - 1));

    private static sbyte Extract7BitsWithoutMask(int i, long val) => (sbyte)(val >> (7 * i));
}