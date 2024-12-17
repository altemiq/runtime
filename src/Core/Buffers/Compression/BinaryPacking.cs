// -----------------------------------------------------------------------
// <copyright file="BinaryPacking.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Encodes integers in blocks of 32 integers.
/// </summary>
/// <remarks>For arrays containing an arbitrary number of integers, you should use it in conjunction with another codec.
/// </remarks>
internal sealed class BinaryPacking : IInt32Codec, IHeadlessInt32Codec
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
        HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var destinationLength = source[sourceIndex];
        sourceIndex++;
        HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, destinationLength);
    }

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, number);

    /// <inheritdoc/>
    public override string ToString() => nameof(BinaryPacking);

    private static void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        length = Util.GreatestMultiple(length, BlockSize);
        var temporaryDestinationIndex = destinationIndex;
        var s = sourceIndex;
        for (; s + (BlockSize * 4) - 1 < sourceIndex + length; s += BlockSize * 4)
        {
            var firstMaxBits = Util.MaxBits(source, s, BlockSize);
            var secondMaxBits = Util.MaxBits(source, s + BlockSize, BlockSize);
            var thirdMaxBits = Util.MaxBits(source, s + (2 * BlockSize), BlockSize);
            var forthMaxBits = Util.MaxBits(source, s + (3 * BlockSize), BlockSize);
            destination[temporaryDestinationIndex++] = (firstMaxBits << 24) | (secondMaxBits << 16) | (thirdMaxBits << 8) | forthMaxBits;
            BitPacking.PackWithoutMask(source.AsSpan(s), destination.AsSpan(temporaryDestinationIndex), firstMaxBits);
            temporaryDestinationIndex += firstMaxBits;
            BitPacking.PackWithoutMask(source.AsSpan(s + BlockSize), destination.AsSpan(temporaryDestinationIndex), secondMaxBits);
            temporaryDestinationIndex += secondMaxBits;
            BitPacking.PackWithoutMask(source.AsSpan(s + (2 * BlockSize)), destination.AsSpan(temporaryDestinationIndex), thirdMaxBits);
            temporaryDestinationIndex += thirdMaxBits;
            BitPacking.PackWithoutMask(source.AsSpan(s + (3 * BlockSize)), destination.AsSpan(temporaryDestinationIndex), forthMaxBits);
            temporaryDestinationIndex += forthMaxBits;
        }

        for (; s < sourceIndex + length; s += BlockSize)
        {
            var maxBits = Util.MaxBits(source, s, BlockSize);
            destination[temporaryDestinationIndex++] = maxBits;
            BitPacking.PackWithoutMask(source.AsSpan(s), destination.AsSpan(temporaryDestinationIndex), maxBits);
            temporaryDestinationIndex += maxBits;
        }

        sourceIndex += length;
        destinationIndex = temporaryDestinationIndex;
    }

    private static void HeadlessUncompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int number)
    {
        var destinationLength = Util.GreatestMultiple(number, BlockSize);
        var temporarySourceIndex = sourceIndex;
        int s;
        for (s = destinationIndex; s + (BlockSize * 4) - 1 < destinationIndex + destinationLength; s += BlockSize * 4)
        {
            var firstMaxBits = (int)((uint)source[temporarySourceIndex] >> 24);
            var secondMaxBits = (int)((uint)source[temporarySourceIndex] >> 16) & 0xFF;
            var thirdMaxBits = (int)((uint)source[temporarySourceIndex] >> 8) & 0xFF;
            var forthMaxBits = (int)(uint)source[temporarySourceIndex] & 0xFF;
            temporarySourceIndex++;
            BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(s), firstMaxBits);
            temporarySourceIndex += firstMaxBits;
            BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(s + BlockSize), secondMaxBits);
            temporarySourceIndex += secondMaxBits;
            BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(s + (2 * BlockSize)), thirdMaxBits);
            temporarySourceIndex += thirdMaxBits;
            BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(s + (3 * BlockSize)), forthMaxBits);
            temporarySourceIndex += forthMaxBits;
        }

        for (; s < destinationIndex + destinationLength; s += BlockSize)
        {
            var maxBits = source[temporarySourceIndex];
            temporarySourceIndex++;
            BitPacking.Unpack(source.AsSpan(temporarySourceIndex), destination.AsSpan(s), maxBits);
            temporarySourceIndex += maxBits;
        }

        destinationIndex += destinationLength;
        sourceIndex = temporarySourceIndex;
    }
}