// -----------------------------------------------------------------------
// <copyright file="DeltaZigzagVariableByte.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// <see cref="VariableByte"/> with Delta+Zigzag Encoding.
/// </summary>
internal sealed class DeltaZigzagVariableByte : IInt32Codec
{
    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = source.Length;
        if (length is 0)
        {
            return default;
        }

        var byteBuffer = new MemoryStream((length * 5) + 3);
        var context = new DeltaZigzagEncoding.Encoder(0);

        // Delta+Zigzag+VariableByte encoding.
        var sourcePosition = 0;

        for (; sourcePosition < length; sourcePosition++)
        {
            // Filter with delta+zigzag encoding.
            var n = context.Encode(source[sourcePosition]);

            // Variable byte encoding.
            var zeros = Util.LeadingZeroCount(n);

            if (zeros < 4)
            {
                byteBuffer.WriteSByte((sbyte)((n >>> 28 & 0x7F) | 0x80));
            }

            if (zeros < 11)
            {
                byteBuffer.WriteSByte((sbyte)((n >>> 21 & 0x7F) | 0x80));
            }

            if (zeros < 18)
            {
                byteBuffer.WriteSByte((sbyte)((n >>> 14 & 0x7F) | 0x80));
            }

            if (zeros < 25)
            {
                byteBuffer.WriteSByte((sbyte)((n >>> 7 & 0x7F) | 0x80));
            }

            byteBuffer.WriteSByte((sbyte)((uint)n & 0x7F));
        }

        // Padding buffer to considerable as IntBuffer.
        const sbyte Padding = unchecked((sbyte)0x80);
        for (var i = (4 - (byteBuffer.Position % 4)) % 4; i > 0; --i)
        {
            byteBuffer.WriteSByte(Padding);
        }

        var destinationLength = (int)byteBuffer.Position / 4;
        byteBuffer.Position = 0;
        byteBuffer.Read(destination[..destinationLength], ByteOrder.BigEndian);
        return (length, destinationLength);
    }

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var context = new DeltaZigzagEncoding.Decoder(0);

        var sourcePosition = 0;
        var destinationPosition = 0;

        // Variable Byte Context.
        var variableByteContextNumber = 0;
        var variableByteContextShift = 24;

        var sourceIndexLast = source.Length;
        while (sourcePosition < sourceIndexLast)
        {
            // Fetch a byte value.
            var n = source[sourcePosition] >>> variableByteContextShift & 0xFF;
            if (variableByteContextShift > 0)
            {
                variableByteContextShift -= 8;
            }
            else
            {
                variableByteContextShift = 24;
                sourcePosition++;
            }

            // Decode variable byte and delta+zigzag.
            variableByteContextNumber = (variableByteContextNumber << 7) + (n & 0x7F);
            if ((n & 0x80) is 0)
            {
                destination[destinationPosition++] = context.Decode(variableByteContextNumber);
                variableByteContextNumber = 0;
            }
        }

        return (sourceIndexLast, destinationPosition);
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(DeltaZigzagVariableByte);
}