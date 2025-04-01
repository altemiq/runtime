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
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, sbyte[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var temporaryDestinationIndex = destinationIndex;
        for (var k = sourceIndex; k < sourceIndex + length; k++)
        {
            var val = source[k] & 0xFFFFFFFFL; // To be consistent with

            // unsigned integers in C/C++
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
    void IHeadlessInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        var index = sourceIndex;
        var endIndex = sourceIndex + length;
        var temporaryDestinationIndex = destinationIndex;

        var s = 0;
        var v = 0;
        var shift = 0;

        while (index < endIndex)
        {
            var value = source[index];
            int c = (sbyte)(value >>> s);
            s += 8;
            index += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[temporaryDestinationIndex++] = v;
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
        var endIndex = sourceIndex + length;
        var temporaryDestinationIndex = destinationIndex;
        while (index < endIndex)
        {
            var value = source[index] & 0x7F;
            if (source[index] < 0)
            {
                index++;
                Update();
                continue;
            }

            value = ((source[index + 1] & 0x7F) << 7) | value;
            if (source[index + 1] < 0)
            {
                index += 2;
                Update();
                continue;
            }

            value = ((source[index + 2] & 0x7F) << 14) | value;
            if (source[index + 2] < 0)
            {
                index += 3;
                Update();
                continue;
            }

            value = ((source[index + 3] & 0x7F) << 21) | value;
            if (source[index + 3] < 0)
            {
                index += 4;
                Update();
                continue;
            }

            value = ((source[index + 4] & 0x7F) << 28) | value;
            index += 5;
            Update();

            [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
            void Update()
            {
                destination[temporaryDestinationIndex++] = value;
            }
        }

        destinationIndex = temporaryDestinationIndex;
        sourceIndex += index;
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, number);

    /// <inheritdoc/>
    public override string ToString() => nameof(VariableByte);

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static sbyte Extract7Bits(int i, long val) => (sbyte)((val >> (7 * i)) & ((1 << 7) - 1));

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static sbyte Extract7BitsWithoutMask(int i, long val) => (sbyte)(val >> (7 * i));

    private static void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var buffer = new MemoryStream(length * 8);
        for (var k = sourceIndex; k < sourceIndex + length; k++)
        {
            var val = source[k] & 0xFFFFFFFFL; // To be consistent with unsigned integers in C/C++
            if (val < (1 << 7))
            {
                buffer.WriteSByte((sbyte)(val | (1 << 7)));
            }
            else if (val < (1 << 14))
            {
                buffer.WriteSByte(Extract7Bits(0, val));
                buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(1, val) | (1 << 7)));
            }
            else if (val < (1 << 21))
            {
                buffer.WriteSByte(Extract7Bits(0, val));
                buffer.WriteSByte(Extract7Bits(1, val));
                buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(2, val) | (1 << 7)));
            }
            else if (val < (1 << 28))
            {
                buffer.WriteSByte(Extract7Bits(0, val));
                buffer.WriteSByte(Extract7Bits(1, val));
                buffer.WriteSByte(Extract7Bits(2, val));
                buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(3, val) | (1 << 7)));
            }
            else
            {
                buffer.WriteSByte(Extract7Bits(0, val));
                buffer.WriteSByte(Extract7Bits(1, val));
                buffer.WriteSByte(Extract7Bits(2, val));
                buffer.WriteSByte(Extract7Bits(3, val));
                buffer.WriteSByte((sbyte)(Extract7BitsWithoutMask(4, val) | (1 << 7)));
            }
        }

        while (buffer.Position % 4 is not 0)
        {
            buffer.WriteByte(default);
        }

        var bufferPosition = (int)buffer.Position;
        buffer.Position = 0;
        _ = buffer.Read(destination, destinationIndex, bufferPosition / 4, ByteOrder.LittleEndian);
        destinationIndex += bufferPosition / 4;
        sourceIndex += length;
    }

    private static void HeadlessDecompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int number)
    {
        var s = 0;
        var p = sourceIndex;
        var temporaryDestinationIndex = destinationIndex;
        var endDestinationIndex = number + temporaryDestinationIndex;
        var v = 0;
        var shift = 0;

        while (temporaryDestinationIndex < endDestinationIndex)
        {
            var value = source[p];
            var c = value >>> s;
            s += 8;
            p += s >> 5;
            s &= 31;
            v += (c & 127) << shift;
            if ((c & 128) is 128)
            {
                destination[temporaryDestinationIndex++] = v;
                v = 0;
                shift = 0;
            }
            else
            {
                shift += 7;
            }
        }

        destinationIndex = temporaryDestinationIndex;
        sourceIndex = p + (s is not 0 ? 1 : 0);
    }
}