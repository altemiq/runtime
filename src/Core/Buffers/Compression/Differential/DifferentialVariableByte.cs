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
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var initOffset = 0;

        var buf = new MemoryStream(length * 8);

        for (var k = sourceIndex; k < sourceIndex + length; k++)
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

        var bufferPosition = (int)buf.Position;
        buf.Position = 0;
        _ = buf.Read(destination, destinationIndex, bufferPosition / 4, ByteOrder.LittleEndian);

        destinationIndex += bufferPosition / 4;
        sourceIndex += length;
    }

    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, sbyte[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var initialOffset = 0;
        var temporaryDestinationIndex = destinationIndex;
        for (var k = sourceIndex; k < sourceIndex + length; k++)
        {
            // To be consistent with unsigned integers in C/C++
            var val = (source[k] - initialOffset) & 0xFFFFFFFFL;
            initialOffset = source[k];
            if (val < (1 << 7))
            {
                destination[temporaryDestinationIndex++] = (sbyte)(val | (1 << 7));
            }
            else if (val < (1 << 14))
            {
                destination[temporaryDestinationIndex++] = Extract7Bits(0, val);
                destination[temporaryDestinationIndex++] = (sbyte)(Extract7BitsWithoutMask(1, val) | (1 << 7));
            }
            else if (val < (1 << 21))
            {
                destination[temporaryDestinationIndex++] = Extract7Bits(0, val);
                destination[temporaryDestinationIndex++] = Extract7Bits(1, val);
                destination[temporaryDestinationIndex++] = (sbyte)(Extract7BitsWithoutMask(2, val) | (1 << 7));
            }
            else if (val < (1 << 28))
            {
                destination[temporaryDestinationIndex++] = Extract7Bits(0, val);
                destination[temporaryDestinationIndex++] = Extract7Bits(1, val);
                destination[temporaryDestinationIndex++] = Extract7Bits(2, val);
                destination[temporaryDestinationIndex++] = (sbyte)(Extract7BitsWithoutMask(3, val) | (1 << 7));
            }
            else
            {
                destination[temporaryDestinationIndex++] = Extract7Bits(0, val);
                destination[temporaryDestinationIndex++] = Extract7Bits(1, val);
                destination[temporaryDestinationIndex++] = Extract7Bits(2, val);
                destination[temporaryDestinationIndex++] = Extract7Bits(3, val);
                destination[temporaryDestinationIndex++] = (sbyte)(Extract7BitsWithoutMask(4, val) | (1 << 7));
            }
        }

        destinationIndex = temporaryDestinationIndex;
        sourceIndex += length;
    }

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        var index = sourceIndex;
        var sourceEnd = sourceIndex + length;
        var temporaryDestinationIndex = destinationIndex;
        var initialOffset = 0;

        var s = 0;
        var v = 0;
        var shift = 0;

        while (index < sourceEnd)
        {
            var val = source[index];
            int c = (sbyte)(val >>> s);
            s += 8;
            index += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[temporaryDestinationIndex] = v + initialOffset;
                initialOffset = destination[temporaryDestinationIndex];
                temporaryDestinationIndex++;
                v = 0;
                shift = 0;
            }
            else
            {
                shift += 7;
            }
        }

        destinationIndex = temporaryDestinationIndex;
        sourceIndex += length;
    }

    /// <inheritdoc/>
    public void Decompress(sbyte[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        var index = sourceIndex;
        var initialOffset = 0;
        var finalIndex = sourceIndex + length;
        var temporaryDestinationIndex = destinationIndex;
        int v;
        while (index < finalIndex)
        {
            v = source[index] & 0x7F;
            if (source[index] < 0)
            {
                index++;
                Update();
                continue;
            }

            v = ((source[index + 1] & 0x7F) << 7) | v;
            if (source[index + 1] < 0)
            {
                index += 2;
                Update();
                continue;
            }

            v = ((source[index + 2] & 0x7F) << 14) | v;
            if (source[index + 2] < 0)
            {
                index += 3;
                Update();
                continue;
            }

            v = ((source[index + 3] & 0x7F) << 21) | v;
            if (source[index + 3] < 0)
            {
                index += 4;
                Update();
                continue;
            }

            v = ((source[index + 4] & 0x7F) << 28) | v;
            index += 5;
            Update();

            void Update()
            {
                initialOffset += v;
                destination[temporaryDestinationIndex++] = initialOffset;
            }
        }

        destinationIndex = temporaryDestinationIndex;
        sourceIndex += index;
    }

    /// <inheritdoc/>
    void IHeadlessDifferentialInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, ref int initialValue) => HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length, ref initialValue);

    /// <inheritdoc/>
    void IHeadlessDifferentialInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number, ref int initialValue) => HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, number, ref initialValue);

    /// <inheritdoc/>
    public override string ToString() => nameof(DifferentialVariableByte);

    private static sbyte Extract7Bits(int i, long val) => (sbyte)((val >> (7 * i)) & ((1 << 7) - 1));

    private static sbyte Extract7BitsWithoutMask(int i, long val) => (sbyte)(val >> (7 * i));

    private static void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, ref int initialValue)
    {
        if (length is 0)
        {
            return;
        }

        var initialOffset = initialValue;
        initialValue = source[sourceIndex + length - 1];
        var byteBuffer = new MemoryStream(length * 8);

        for (var k = sourceIndex; k < sourceIndex + length; k++)
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

        var bufferPosition = (int)byteBuffer.Position;
        byteBuffer.Position = 0;
        _ = byteBuffer.Read(destination, destinationIndex, bufferPosition / 4, ByteOrder.LittleEndian);

        destinationIndex += bufferPosition / 4;
        sourceIndex += length;
    }

    private static void HeadlessDecompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int num, ref int initialValue)
    {
        var s = 0;
        var p = sourceIndex;
        var initialOffset = initialValue;
        var temporaryDestinationIndex = destinationIndex;
        var finalDestinationIndex = num + temporaryDestinationIndex;
        var v = 0;
        var shift = 0;

        while (temporaryDestinationIndex < finalDestinationIndex)
        {
            var val = source[p];
            var c = val >>> s;
            s += 8;
            p += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[temporaryDestinationIndex++] = initialOffset += v;
                v = 0;
                shift = 0;
            }
            else
            {
                shift += 7;
            }
        }

        initialValue = destination[temporaryDestinationIndex - 1];
        destinationIndex = temporaryDestinationIndex;

        sourceIndex = p + (s is not 0 ? 1 : 0);
    }
}