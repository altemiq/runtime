// -----------------------------------------------------------------------
// <copyright file="XorBinaryPacking.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// <see cref="BinaryPacking"/> over XOR differential.
/// </summary>
internal sealed class XorBinaryPacking : IDifferentialInt32Codec
{
    private const int BlockLength = 128;

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = source.Length;
        length -= length % BlockLength;
        if (length is 0)
        {
            return default;
        }

        destination[0] = length;
        var written = 1;

        var context = 0;
        Span<int> work = stackalloc int[32];

        var read = 0;
        var sourceIndexLast = length;
        for (; read < sourceIndexLast; read += BlockLength)
        {
            var span1 = source.Slice(read + 0, 32);
            var span2 = source.Slice(read + 32, 32);
            var span3 = source.Slice(read + 64, 32);
            var span4 = source.Slice(read + 96, 32);

            var context2 = source[read + 31];
            var context3 = source[read + 63];
            var context4 = source[read + 95];

            var bits1 = XorMaxBits(span1, context);
            var bits2 = XorMaxBits(span2, context2);
            var bits3 = XorMaxBits(span3, context3);
            var bits4 = XorMaxBits(span4, context4);

            destination[written++] = (bits1 << 24) | (bits2 << 16) | (bits3 << 8) | (bits4 << 0);
            written += XorPack(span1, destination[written..], bits1, context, work);
            written += XorPack(span2, destination[written..], bits2, context2, work);
            written += XorPack(span3, destination[written..], bits3, context3, work);
            written += XorPack(span4, destination[written..], bits4, context4, work);

            context = source[read + 127];
        }

        return (read, written);

        static int XorMaxBits(ReadOnlySpan<int> buffer, int context)
        {
            var mask = 0;
            foreach (var item in buffer)
            {
                mask |= item ^ context;
                context = item;
            }

            return Util.Bits(mask);
        }

        static int XorPack(ReadOnlySpan<int> input, Span<int> output, int validBits, int context, Span<int> work)
        {
            var i = 0;
            foreach (var item in input)
            {
                work[i] = item ^ context;
                context = item;
                i++;
            }

            BitPacking.PackWithoutMask(work, output, validBits);

            return validBits;
        }
    }

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var read = 0;
        var destinationLength = source[read];
        read++;

        var context = 0;
        Span<int> work = stackalloc int[32];

        var written = 0;
        for (; written < destinationLength; written += BlockLength)
        {
            var bits1 = source[read] >>> 24;
            var bits2 = source[read] >>> 16 & 0xFF;
            var bits3 = source[read] >>> 8 & 0xFF;
            var bits4 = source[read] >>> 0 & 0xFF;
            read++;
            read += XorUnpack(source[read..], destination[(written + 0)..], bits1, context, work);
            read += XorUnpack(source[read..], destination[(written + 32)..], bits2, destination[written + 31], work);
            read += XorUnpack(source[read..], destination[(written + 64)..], bits3, destination[written + 63], work);
            read += XorUnpack(source[read..], destination[(written + 96)..], bits4, destination[written + 95], work);
            context = destination[written + 127];
        }

        return (read, written);

        static int XorUnpack(ReadOnlySpan<int> input, Span<int> output, int validBits, int context, Span<int> work)
        {
            BitPacking.Unpack(input, work, validBits);
            output[0] = context = work[0] ^ context;
            for (int i = 1, p = 1; i < 32; i++, p++)
            {
                output[p] = context = work[i] ^ context;
            }

            return validBits;
        }
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(XorBinaryPacking);
}