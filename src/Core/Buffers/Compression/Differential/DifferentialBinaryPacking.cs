// -----------------------------------------------------------------------
// <copyright file="DifferentialBinaryPacking.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// Differential binary packing.
/// </summary>
/// <remarks>
/// <para>Scheme based on a commonly used idea: can be extremely fast.</para>
/// <para>You should only use this scheme on sorted arrays. Use <see cref="BinaryPacking"/> if you have unsorted arrays.</para>
/// <para>It encodes integers in blocks of 32 integers. For arrays containing an arbitrary number of integers, you should use it in conjunction with another codec.</para>
/// </remarks>
internal sealed class DifferentialBinaryPacking : IDifferentialInt32Codec, IHeadlessDifferentialInt32Codec
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
        var initialValue = 0;
        var (read, written) = HeadlessCompress(source[..length], destination[1..], ref initialValue);
        return (read, written + 1);
    }

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var destinationLength = source[0];
        var initValue = 0;
        var (read, written) = HeadlessDecompress(source[1..], destination[..destinationLength], ref initValue);
        return (read + 1, written);
    }

    /// <inheritdoc/>
    (int Read, int Written) ICompressHeadlessDifferentialCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue) => HeadlessCompress(source, destination, ref initialValue);

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessDifferentialCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue) => HeadlessDecompress(source, destination, ref initialValue);

    /// <inheritdoc/>
    public override string ToString() => nameof(DifferentialBinaryPacking);

    private static int MaxDiffBits(int offset, ReadOnlySpan<int> source)
    {
        var mask = 0;
        mask |= source[0] - offset;
        for (var k = 1; k < source.Length; k++)
        {
            mask |= source[k] - source[k - 1];
        }

        return Util.Bits(mask);
    }

    private static (int Read, int Written) HeadlessCompress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue)
    {
        var length = Util.GreatestMultiple(source.Length, BlockSize);
        if (length is 0)
        {
            return default;
        }

        var temporaryDestinationIndex = 0;
        var firstInitialOffset = initialValue;
        initialValue = source[length - 1];
        var index = 0;
        for (; index + (BlockSize * 4) - 1 < length; index += BlockSize * 4)
        {
            var firstMaxBits = MaxDiffBits(firstInitialOffset, source.Slice(index, BlockSize));
            var secondInitialOffset = source[index + 31];
            var secondMaxBits = MaxDiffBits(secondInitialOffset, source.Slice(index + BlockSize, BlockSize));
            var thirdInitialOffset = source[index + BlockSize + 31];
            var thirdMaxBits = MaxDiffBits(thirdInitialOffset, source.Slice(index + (2 * BlockSize), BlockSize));
            var forthInitialOffset = source[index + (2 * BlockSize) + 31];
            var forthMaxBits = MaxDiffBits(forthInitialOffset, source.Slice(index + (3 * BlockSize), BlockSize));
            destination[temporaryDestinationIndex++] = (firstMaxBits << 24) | (secondMaxBits << 16) | (thirdMaxBits << 8) | forthMaxBits;
            DifferentialBitPacking.Pack(firstInitialOffset, source[index..], destination[temporaryDestinationIndex..], firstMaxBits);
            temporaryDestinationIndex += firstMaxBits;
            DifferentialBitPacking.Pack(secondInitialOffset, source[(index + BlockSize)..], destination[temporaryDestinationIndex..], secondMaxBits);
            temporaryDestinationIndex += secondMaxBits;
            DifferentialBitPacking.Pack(thirdInitialOffset, source[(index + (2 * BlockSize))..], destination[temporaryDestinationIndex..], thirdMaxBits);
            temporaryDestinationIndex += thirdMaxBits;
            DifferentialBitPacking.Pack(forthInitialOffset, source[(index + (3 * BlockSize))..], destination[temporaryDestinationIndex..], forthMaxBits);
            temporaryDestinationIndex += forthMaxBits;
            firstInitialOffset = source[index + (3 * BlockSize) + 31];
        }

        for (; index < length; index += BlockSize)
        {
            var maxDiffBits = MaxDiffBits(firstInitialOffset, source.Slice(index, BlockSize));
            destination[temporaryDestinationIndex++] = maxDiffBits;
            DifferentialBitPacking.Pack(firstInitialOffset, source[index..], destination[temporaryDestinationIndex..], maxDiffBits);
            temporaryDestinationIndex += maxDiffBits;
            firstInitialOffset = source[index + 31];
        }

        return (length, temporaryDestinationIndex);
    }

    private static (int Read, int Written) HeadlessDecompress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue)
    {
        var destinationLength = Util.GreatestMultiple(destination.Length, BlockSize);
        var temporarySourceIndex = 0;
        var initialOffset = initialValue;
        var s = 0;
        for (; s + (BlockSize * 4) - 1 < destinationLength; s += BlockSize * 4)
        {
            var bits1 = source[temporarySourceIndex] >>> 24;
            var bits2 = source[temporarySourceIndex] >>> 16 & 0xFF;
            var bits3 = source[temporarySourceIndex] >>> 8 & 0xFF;
            var bits4 = source[temporarySourceIndex] & 0xFF;

            temporarySourceIndex++;
            DifferentialBitPacking.Unpack(initialOffset, source[temporarySourceIndex..], destination[s..], bits1);
            temporarySourceIndex += bits1;
            initialOffset = destination[s + 31];
            DifferentialBitPacking.Unpack(initialOffset, source[temporarySourceIndex..], destination[(s + BlockSize)..], bits2);
            temporarySourceIndex += bits2;
            initialOffset = destination[s + BlockSize + 31];
            DifferentialBitPacking.Unpack(initialOffset, source[temporarySourceIndex..], destination[(s + (2 * BlockSize))..], bits3);
            temporarySourceIndex += bits3;
            initialOffset = destination[s + (2 * BlockSize) + 31];
            DifferentialBitPacking.Unpack(initialOffset, source[temporarySourceIndex..], destination[(s + (3 * BlockSize))..], bits4);
            temporarySourceIndex += bits4;
            initialOffset = destination[s + (3 * BlockSize) + 31];
        }

        for (; s < destinationLength; s += BlockSize)
        {
            var bits = source[temporarySourceIndex];
            temporarySourceIndex++;
            DifferentialBitPacking.Unpack(initialOffset, source[temporarySourceIndex..], destination[s..], bits);
            initialOffset = destination[s + 31];

            temporarySourceIndex += bits;
        }

        initialValue = initialOffset;
        return (temporarySourceIndex, destinationLength);
    }
}