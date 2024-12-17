// -----------------------------------------------------------------------
// <copyright file="DeltaZigzagBinaryPacking.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// <see cref="BinaryPacking"/> with Delta+Zigzag Encoding.
/// </summary>
/// <remarks>
/// It encodes integers in blocks of 128 integers. For arrays containing
/// an arbitrary number of integers, you should use it in conjunction
/// with another <see cref="IInt32Codec"/>.
/// </remarks>
internal sealed class DeltaZigzagBinaryPacking : IInt32Codec
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

        var ctx = new DeltaZigzagEncoding.Encoder(0);
        var work = new int[BlockLength];

        var op = destinationIndex;
        var ip = sourceIndex;
        var sourceIndexLast = ip + length;
        for (; ip < sourceIndexLast; ip += BlockLength)
        {
            ctx.Encode(source.AsSpan(ip, BlockLength), work);
            var bits1 = MaxBits32(work, 0);
            var bits2 = MaxBits32(work, 32);
            var bits3 = MaxBits32(work, 64);
            var bits4 = MaxBits32(work, 96);
            destination[op++] = (bits1 << 24) | (bits2 << 16) | (bits3 << 8) | (bits4 << 0);
            op += Pack(work, 0, destination, op, bits1);
            op += Pack(work, 32, destination, op, bits2);
            op += Pack(work, 64, destination, op, bits3);
            op += Pack(work, 96, destination, op, bits4);
        }

        sourceIndex += length;
        destinationIndex = op;

        static int MaxBits32(int[] i, int pos)
        {
            var mask = i[pos];
            mask |= i[pos + 1];
            mask |= i[pos + 2];
            mask |= i[pos + 3];
            mask |= i[pos + 4];
            mask |= i[pos + 5];
            mask |= i[pos + 6];
            mask |= i[pos + 7];
            mask |= i[pos + 8];
            mask |= i[pos + 9];
            mask |= i[pos + 10];
            mask |= i[pos + 11];
            mask |= i[pos + 12];
            mask |= i[pos + 13];
            mask |= i[pos + 14];
            mask |= i[pos + 15];
            mask |= i[pos + 16];
            mask |= i[pos + 17];
            mask |= i[pos + 18];
            mask |= i[pos + 19];
            mask |= i[pos + 20];
            mask |= i[pos + 21];
            mask |= i[pos + 22];
            mask |= i[pos + 23];
            mask |= i[pos + 24];
            mask |= i[pos + 25];
            mask |= i[pos + 26];
            mask |= i[pos + 27];
            mask |= i[pos + 28];
            mask |= i[pos + 29];
            mask |= i[pos + 30];
            mask |= i[pos + 31];
            return Util.Bits(mask);
        }

        static int Pack(int[] inBuf, int inOff, int[] outBuf, int outOff, int validBits)
        {
            BitPacking.PackWithoutMask(inBuf.AsSpan(inOff), outBuf.AsSpan(outOff), validBits);
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

        var destinationLength = source[sourceIndex];
        sourceIndex++;

        var context = new DeltaZigzagEncoding.Decoder(0);

        var sourcePosition = sourceIndex;
        var destinationPosition = destinationIndex;
        var destinationIndexLast = destinationPosition + destinationLength;
        Span<int> working = stackalloc int[BlockLength];
        for (; destinationPosition < destinationIndexLast; destinationPosition += BlockLength)
        {
            var n = source[sourcePosition++];
            sourcePosition += Unpack(source.AsSpan(sourcePosition), working[0..], (n >> 24) & 0x3F);
            sourcePosition += Unpack(source.AsSpan(sourcePosition), working[32..], (n >> 16) & 0x3F);
            sourcePosition += Unpack(source.AsSpan(sourcePosition), working[64..], (n >> 8) & 0x3F);
            sourcePosition += Unpack(source.AsSpan(sourcePosition), working[96..], (n >> 0) & 0x3F);
            context.Decode(working, destination.AsSpan(destinationPosition));
        }

        destinationIndex += destinationLength;
        sourceIndex = sourcePosition;

        static int Unpack(ReadOnlySpan<int> source, Span<int> destination, int validBits)
        {
            BitPacking.Unpack(source, destination, validBits);
            return validBits;
        }
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(DeltaZigzagBinaryPacking);
}