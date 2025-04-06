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
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length = Util.GreatestMultiple(length, BlockSize);
        if (length is 0)
        {
            return;
        }

        destination[destinationIndex] = length;
        destinationIndex++;
        var initialValue = 0;
        HeadlessCompress(source, ref sourceIndex, length, destination, ref destinationIndex, ref initialValue);
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
        var initValue = 0;
        HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, destinationLength, ref initValue);
    }

    /// <inheritdoc/>
    void IHeadlessDifferentialInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, ref int initialValue) => HeadlessCompress(source, ref sourceIndex, length, destination, ref destinationIndex, ref initialValue);

    /// <inheritdoc/>
    void IHeadlessDifferentialInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number, ref int initialValue) => HeadlessDecompress(source, ref sourceIndex, destination, ref destinationIndex, number, ref initialValue);

    /// <inheritdoc/>
    public override string ToString() => nameof(DifferentialBinaryPacking);

    private static int MaxDiffBits(int offset, int[] source, int index, int length)
    {
        var mask = 0;
        mask |= source[index] - offset;
        for (var k = index + 1; k < index + length; k++)
        {
            mask |= source[k] - source[k - 1];
        }

        return Util.Bits(mask);
    }

    private static void HeadlessCompress(int[] source, ref int sourceIndex, int length, int[] destination, ref int destinationIndex, ref int initialValue)
    {
        length = Util.GreatestMultiple(length, BlockSize);
        if (length is 0)
        {
            return;
        }

        var temporaryDestinationIndex = destinationIndex;
        var firstInitialOffset = initialValue;
        initialValue = source[sourceIndex + length - 1];
        var index = sourceIndex;
        for (; index + (BlockSize * 4) - 1 < sourceIndex + length; index += BlockSize * 4)
        {
            var firstMaxBits = MaxDiffBits(firstInitialOffset, source, index, BlockSize);
            var secondInitialOffset = source[index + 31];
            var secondMaxBits = MaxDiffBits(secondInitialOffset, source, index + BlockSize, BlockSize);
            var thirdInitialOffset = source[index + BlockSize + 31];
            var thirdMaxBits = MaxDiffBits(thirdInitialOffset, source, index + (2 * BlockSize), BlockSize);
            var forthInitialOffset = source[index + (2 * BlockSize) + 31];
            var forthMaxBits = MaxDiffBits(forthInitialOffset, source, index + (3 * BlockSize), BlockSize);
            destination[temporaryDestinationIndex++] = (firstMaxBits << 24) | (secondMaxBits << 16) | (thirdMaxBits << 8) | forthMaxBits;
            DifferentialBitPacking.Pack(firstInitialOffset, source.AsSpan(index), destination.AsSpan(temporaryDestinationIndex), firstMaxBits);
            temporaryDestinationIndex += firstMaxBits;
            DifferentialBitPacking.Pack(secondInitialOffset, source.AsSpan(index + BlockSize), destination.AsSpan(temporaryDestinationIndex), secondMaxBits);
            temporaryDestinationIndex += secondMaxBits;
            DifferentialBitPacking.Pack(thirdInitialOffset, source.AsSpan(index + (2 * BlockSize)), destination.AsSpan(temporaryDestinationIndex), thirdMaxBits);
            temporaryDestinationIndex += thirdMaxBits;
            DifferentialBitPacking.Pack(forthInitialOffset, source.AsSpan(index + (3 * BlockSize)), destination.AsSpan(temporaryDestinationIndex), forthMaxBits);
            temporaryDestinationIndex += forthMaxBits;
            firstInitialOffset = source[index + (3 * BlockSize) + 31];
        }

        for (; index < sourceIndex + length; index += BlockSize)
        {
            var maxDiffBits = MaxDiffBits(firstInitialOffset, source, index, BlockSize);
            destination[temporaryDestinationIndex++] = maxDiffBits;
            DifferentialBitPacking.Pack(firstInitialOffset, source.AsSpan(index), destination.AsSpan(temporaryDestinationIndex), maxDiffBits);
            temporaryDestinationIndex += maxDiffBits;
            firstInitialOffset = source[index + 31];
        }

        sourceIndex += length;
        destinationIndex = temporaryDestinationIndex;
    }

    private static void HeadlessDecompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int num, ref int initialValue)
    {
        var destinationLength = Util.GreatestMultiple(num, BlockSize);
        var temporarySourceIndex = sourceIndex;
        var initialOffset = initialValue;
        var s = destinationIndex;
        for (; s + (BlockSize * 4) - 1 < destinationIndex + destinationLength; s += BlockSize * 4)
        {
            var bits1 = source[temporarySourceIndex] >>> 24;
            var bits2 = source[temporarySourceIndex] >>> 16 & 0xFF;
            var bits3 = source[temporarySourceIndex] >>> 8 & 0xFF;
            var bits4 = source[temporarySourceIndex] & 0xFF;

            temporarySourceIndex++;
            DifferentialBitPacking.Unpack(initialOffset, source.AsSpan(temporarySourceIndex), destination.AsSpan(s), bits1);
            temporarySourceIndex += bits1;
            initialOffset = destination[s + 31];
            DifferentialBitPacking.Unpack(initialOffset, source.AsSpan(temporarySourceIndex), destination.AsSpan(s + BlockSize), bits2);
            temporarySourceIndex += bits2;
            initialOffset = destination[s + BlockSize + 31];
            DifferentialBitPacking.Unpack(initialOffset, source.AsSpan(temporarySourceIndex), destination.AsSpan(s + (2 * BlockSize)), bits3);
            temporarySourceIndex += bits3;
            initialOffset = destination[s + (2 * BlockSize) + 31];
            DifferentialBitPacking.Unpack(initialOffset, source.AsSpan(temporarySourceIndex), destination.AsSpan(s + (3 * BlockSize)), bits4);
            temporarySourceIndex += bits4;
            initialOffset = destination[s + (3 * BlockSize) + 31];
        }

        for (; s < destinationIndex + destinationLength; s += BlockSize)
        {
            var bits = source[temporarySourceIndex];
            temporarySourceIndex++;
            DifferentialBitPacking.Unpack(initialOffset, source.AsSpan(temporarySourceIndex), destination.AsSpan(s), bits);
            initialOffset = destination[s + 31];

            temporarySourceIndex += bits;
        }

        destinationIndex += destinationLength;
        initialValue = initialOffset;
        sourceIndex = temporarySourceIndex;
    }
}