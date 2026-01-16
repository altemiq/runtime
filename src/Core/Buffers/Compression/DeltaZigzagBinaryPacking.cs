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
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = source.Length;
        length -= length % BlockLength;
        if (length is 0)
        {
            return default;
        }

        var written = 0;
        destination[written] = length;
        written++;

        var encoder = new DeltaZigzagEncoding.Encoder(0);
        Span<int> work = stackalloc int[BlockLength];

        for (var i = 0; i < length; i += BlockLength)
        {
            encoder.Encode(source.Slice(i, BlockLength), work);
            var bits1 = MaxBits32(work[..]);
            var bits2 = MaxBits32(work[32..]);
            var bits3 = MaxBits32(work[64..]);
            var bits4 = MaxBits32(work[96..]);
            destination[written++] = (bits1 << 24) | (bits2 << 16) | (bits3 << 8) | (bits4 << 0);
            written += Pack(work[..], destination[written..], bits1);
            written += Pack(work[32..], destination[written..], bits2);
            written += Pack(work[64..], destination[written..], bits3);
            written += Pack(work[96..], destination[written..], bits4);
        }

        return (length, written);

        static int MaxBits32(ReadOnlySpan<int> i)
        {
            var mask = i[0];
            mask |= i[1];
            mask |= i[2];
            mask |= i[3];
            mask |= i[4];
            mask |= i[5];
            mask |= i[6];
            mask |= i[7];
            mask |= i[8];
            mask |= i[9];
            mask |= i[10];
            mask |= i[11];
            mask |= i[12];
            mask |= i[13];
            mask |= i[14];
            mask |= i[15];
            mask |= i[16];
            mask |= i[17];
            mask |= i[18];
            mask |= i[19];
            mask |= i[20];
            mask |= i[21];
            mask |= i[22];
            mask |= i[23];
            mask |= i[24];
            mask |= i[25];
            mask |= i[26];
            mask |= i[27];
            mask |= i[28];
            mask |= i[29];
            mask |= i[30];
            mask |= i[31];
            return Util.Bits(mask);
        }

        static int Pack(ReadOnlySpan<int> inBuf, Span<int> outBuf, int validBits)
        {
            BitPacking.PackWithoutMask(inBuf, outBuf, validBits);
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

        var destinationLength = source[0];

        var context = new DeltaZigzagEncoding.Decoder(0);

        var sourcePosition = 1;
        var destinationPosition = 0;
        Span<int> working = stackalloc int[BlockLength];
        for (; destinationPosition < destinationLength; destinationPosition += BlockLength)
        {
            var n = source[sourcePosition++];
            sourcePosition += Unpack(source[sourcePosition..], working[0..], (n >> 24) & 0x3F);
            sourcePosition += Unpack(source[sourcePosition..], working[32..], (n >> 16) & 0x3F);
            sourcePosition += Unpack(source[sourcePosition..], working[64..], (n >> 8) & 0x3F);
            sourcePosition += Unpack(source[sourcePosition..], working[96..], (n >> 0) & 0x3F);
            context.Decode(working, destination[destinationPosition..]);
        }

        return (sourcePosition, destinationLength);

        static int Unpack(ReadOnlySpan<int> source, Span<int> destination, int validBits)
        {
            BitPacking.Unpack(source, destination, validBits);
            return validBits;
        }
    }

    /// <inheritdoc/>
    public override string ToString() => nameof(DeltaZigzagBinaryPacking);
}