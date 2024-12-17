// -----------------------------------------------------------------------
// <copyright file="IHeadlessInt32Codec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The headless <see cref="int"/> codec.
/// </summary>
internal interface IHeadlessInt32Codec
{
    /// <summary>
    /// Compress data from an array to another array.
    /// </summary>
    /// <remarks>
    /// Both <paramref name="sourceIndex"/> and <paramref name="destinationIndex"/> are modified to represent how much data was read
    /// and written to if 12 ints(length = 12) are compressed to 3 ints, then
    /// <paramref name="sourceIndex"/> will be incremented by 12 while <paramref name="destinationIndex"/> will be incremented by 3.
    /// </remarks>
    /// <param name="source">The input array.</param>
    /// <param name="sourceIndex">The location in the input array.</param>
    /// <param name="destination">The output array.</param>
    /// <param name="destinationIndex">The where to write in the output array.</param>
    /// <param name="length">How many integers to compress.</param>
    void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length);

    /// <summary>
    /// Uncompress data from an array to another array.
    /// </summary>
    /// <param name="source">The array containing data in compressed form.</param>
    /// <param name="sourceIndex">The where to start reading in the array.</param>
    /// <param name="destination">The array where to write the compressed output.</param>
    /// <param name="destinationIndex">The where to write the compressed output in out.</param>
    /// <param name="length">The length of the compressed data (ignored by some schemes).</param>
    /// <param name="number">The number of integers we want to decode, the actual number of integers decoded can be less.</param>
    void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number);
}