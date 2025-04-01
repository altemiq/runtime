// -----------------------------------------------------------------------
// <copyright file="ISByteCodec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The <see cref="sbyte"/> to <see cref="int"/> codec.
/// </summary>
internal interface ISByteCodec
{
    /// <summary>
    /// Compress data from an array to another array.
    /// </summary>
    /// <remarks>
    /// Both <paramref name="sourceIndex"/> and <paramref name="destinationIndex"/> are modified to represent how much data was
    /// read and written to if 12 ints (length = 12) are compressed to 3
    /// bytes, then <paramref name="sourceIndex"/> will be incremented by 12 while <paramref name="destinationIndex"/> will be
    /// incremented by 3 we use ref int to pass the values by reference.
    /// </remarks>
    /// <param name="source">The input array.</param>
    /// <param name="sourceIndex">The location in the input array.</param>
    /// <param name="destination">The output array.</param>
    /// <param name="destinationIndex">Where to write in the output array.</param>
    /// <param name="length">How many integers to compress.</param>
    void Compress(int[] source, ref int sourceIndex, sbyte[] destination, ref int destinationIndex, int length);

    /// <summary>
    /// Decompress data from an array to another array.
    /// </summary>
    /// <param name="source">The array containing data in compressed form.</param>
    /// <param name="sourceIndex">Where to start reading in the array.</param>
    /// <param name="destination">The array where to write the compressed output.</param>
    /// <param name="destinationIndex">Where to write the compressed output in out.</param>
    /// <param name="length">The length of the compressed data (ignored by some schemes).</param>
    void Decompress(sbyte[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length);
}