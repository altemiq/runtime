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
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length -= length % BlockLength;
        if (length is 0)
        {
            return;
        }

        destination[destinationIndex] = length;
        destinationIndex++;

        var context = 0;
        Span<int> work = stackalloc int[32];

        var sourceIndexLast = sourceIndex + length;
        for (; sourceIndex < sourceIndexLast; sourceIndex += BlockLength)
        {
            var span1 = source.AsSpan(sourceIndex + 0, 32);
            var span2 = source.AsSpan(sourceIndex + 32, 32);
            var span3 = source.AsSpan(sourceIndex + 64, 32);
            var span4 = source.AsSpan(sourceIndex + 96, 32);

            var context2 = source[sourceIndex + 31];
            var context3 = source[sourceIndex + 63];
            var context4 = source[sourceIndex + 95];

            var bits1 = XorMaxBits(span1, context);
            var bits2 = XorMaxBits(span2, context2);
            var bits3 = XorMaxBits(span3, context3);
            var bits4 = XorMaxBits(span4, context4);

            destination[destinationIndex++] = (bits1 << 24) | (bits2 << 16) | (bits3 << 8) | (bits4 << 0);
            destinationIndex += XorPack(span1, destination.AsSpan(destinationIndex), bits1, context, work);
            destinationIndex += XorPack(span2, destination.AsSpan(destinationIndex), bits2, context2, work);
            destinationIndex += XorPack(span3, destination.AsSpan(destinationIndex), bits3, context3, work);
            destinationIndex += XorPack(span4, destination.AsSpan(destinationIndex), bits4, context4, work);

            context = source[sourceIndex + 127];
        }

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
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var outLen = source[sourceIndex];
        sourceIndex++;

        var context = 0;
        Span<int> work = stackalloc int[32];

        var destinationIndexLast = destinationIndex + outLen;
        for (; destinationIndex < destinationIndexLast; destinationIndex += BlockLength)
        {
            var bits1 = source[sourceIndex] >>> 24;
            var bits2 = source[sourceIndex] >>> 16 & 0xFF;
            var bits3 = source[sourceIndex] >>> 8 & 0xFF;
            var bits4 = source[sourceIndex] >>> 0 & 0xFF;
            sourceIndex++;
            sourceIndex += XorUnpack(source.AsSpan(sourceIndex), destination.AsSpan(destinationIndex + 0), bits1, context, work);
            sourceIndex += XorUnpack(source.AsSpan(sourceIndex), destination.AsSpan(destinationIndex + 32), bits2, destination[destinationIndex + 31], work);
            sourceIndex += XorUnpack(source.AsSpan(sourceIndex), destination.AsSpan(destinationIndex + 64), bits3, destination[destinationIndex + 63], work);
            sourceIndex += XorUnpack(source.AsSpan(sourceIndex), destination.AsSpan(destinationIndex + 96), bits4, destination[destinationIndex + 95], work);
            context = destination[destinationIndex + 127];
        }

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