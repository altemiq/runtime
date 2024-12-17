// -----------------------------------------------------------------------
// <copyright file="Copy.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Codec that just copies the values.
/// </summary>
internal sealed class Copy : IInt32Codec, IHeadlessInt32Codec
{
    /// <inheritdoc/>
    void IHeadlessInt32Codec.Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    void IHeadlessInt32Codec.Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number) => HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, number);

    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => HeadlessCompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length) => HeadlessUncompress(source, ref sourceIndex, destination, ref destinationIndex, length);

    /// <inheritdoc/>
    public override string ToString() => nameof(Copy);

    private static void HeadlessCompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        System.Array.Copy(source, sourceIndex, destination, destinationIndex, length);
        sourceIndex += length;
        destinationIndex += length;
    }

    private static void HeadlessUncompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int number)
    {
        System.Array.Copy(source, sourceIndex, destination, destinationIndex, number);
        sourceIndex += number;
        destinationIndex += number;
    }
}