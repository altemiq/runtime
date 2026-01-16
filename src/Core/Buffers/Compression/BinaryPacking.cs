// -----------------------------------------------------------------------
// <copyright file="BinaryPacking.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Encodes integers in blocks of 32 integers.
/// </summary>
/// <remarks>For arrays containing an arbitrary number of integers, you should use it in conjunction with another codec.</remarks>
internal sealed class BinaryPacking : IInt32Codec, IHeadlessInt32Codec
{
    private const int BlockSize = 32;

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = Util.GreatestMultiple(source.Length, BlockSize);
        if (length is 0)
        {
            return default;
        }

        destination[0] = length;
        var (sourceIndex, destinationIndex) = HeadlessCompress(source[..length], destination[1..]);
        return (sourceIndex, destinationIndex + 1);
    }

    /// <inheritdoc/>
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessCompress(source, destination);

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var (read, written) = HeadlessDecompress(source[1..], destination[..source[0]]);
        return (read + 1, written);
    }

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination) => HeadlessDecompress(source, destination);

    /// <inheritdoc/>
    public override string ToString() => nameof(BinaryPacking);

    private static (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var length = Util.GreatestMultiple(source.Length, BlockSize);
        var destinationIndex = 0;
        var sourceIndex = 0;
        for (; sourceIndex + (BlockSize * 4) - 1 < length; sourceIndex += BlockSize * 4)
        {
            var firstMaxBits = Util.MaxBits(source.Slice(sourceIndex, BlockSize));
            var secondMaxBits = Util.MaxBits(source.Slice(sourceIndex + BlockSize, BlockSize));
            var thirdMaxBits = Util.MaxBits(source.Slice(sourceIndex + (2 * BlockSize), BlockSize));
            var forthMaxBits = Util.MaxBits(source.Slice(sourceIndex + (3 * BlockSize), BlockSize));
            destination[destinationIndex++] = (firstMaxBits << 24) | (secondMaxBits << 16) | (thirdMaxBits << 8) | forthMaxBits;
            BitPacking.PackWithoutMask(source[sourceIndex..], destination[destinationIndex..], firstMaxBits);
            destinationIndex += firstMaxBits;
            BitPacking.PackWithoutMask(source[(sourceIndex + BlockSize)..], destination[destinationIndex..], secondMaxBits);
            destinationIndex += secondMaxBits;
            BitPacking.PackWithoutMask(source[(sourceIndex + (2 * BlockSize))..], destination[destinationIndex..], thirdMaxBits);
            destinationIndex += thirdMaxBits;
            BitPacking.PackWithoutMask(source[(sourceIndex + (3 * BlockSize))..], destination[destinationIndex..], forthMaxBits);
            destinationIndex += forthMaxBits;
        }

        for (; sourceIndex < length; sourceIndex += BlockSize)
        {
            var maxBits = Util.MaxBits(source.Slice(sourceIndex, BlockSize));
            destination[destinationIndex++] = maxBits;
            BitPacking.PackWithoutMask(source[sourceIndex..], destination[destinationIndex..], maxBits);
            destinationIndex += maxBits;
        }

        return (length, destinationIndex);
    }

    private static (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        var written = Util.GreatestMultiple(destination.Length, BlockSize);
        var read = 0;
        int s;
        for (s = 0; s + (BlockSize * 4) - 1 < written; s += BlockSize * 4)
        {
            var firstMaxBits = source[read] >>> 24;
            var secondMaxBits = source[read] >>> 16 & 0xFF;
            var thirdMaxBits = source[read] >>> 8 & 0xFF;
            var forthMaxBits = (int)(uint)source[read] & 0xFF;
            read++;
            BitPacking.Unpack(source[read..], destination[s..], firstMaxBits);
            read += firstMaxBits;
            BitPacking.Unpack(source[read..], destination[(s + BlockSize)..], secondMaxBits);
            read += secondMaxBits;
            BitPacking.Unpack(source[read..], destination[(s + (2 * BlockSize))..], thirdMaxBits);
            read += thirdMaxBits;
            BitPacking.Unpack(source[read..], destination[(s + (3 * BlockSize))..], forthMaxBits);
            read += forthMaxBits;
        }

        for (; s < written; s += BlockSize)
        {
            var maxBits = source[read];
            read++;
            BitPacking.Unpack(source[read..], destination[s..], maxBits);
            read += maxBits;
        }

        return (read, written);
    }
}