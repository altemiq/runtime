// -----------------------------------------------------------------------
// <copyright file="IInt32Codec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The <see cref="int"/> codec.
/// </summary>
internal interface IInt32Codec
{
    /// <summary>
    /// Compress data from an array to another array.
    /// </summary>
    /// <remarks>
    /// Both <paramref name="sourceIndex"/> and <paramref name="destinationIndex"/> are modified to represent how much data was
    /// read and written to if 12 ints (length = 12) are compressed to 3
    /// ints, then <paramref name="sourceIndex"/> will be incremented by 12 while destinationIndex will be
    /// incremented by 3 we use ref int to pass the values by reference.
    /// </remarks>
    /// <param name="source">input array.</param>
    /// <param name="sourceIndex">how many integers to compress.</param>
    /// <param name="destination">output array.</param>
    /// <param name="destinationIndex">where to write in the output array.</param>
    /// <param name="length">location in the input array.</param>
    void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length);

    /// <summary>
    /// Decompress data from an array to another array.
    /// </summary>
    /// <remarks>
    /// Both <paramref name="sourceIndex"/> and <paramref name="destinationIndex"/> parameters are modified to indicate new positions after read/write.
    /// </remarks>
    /// <param name="source">array containing data in compressed form.</param>
    /// <param name="sourceIndex">where to start reading in the array.</param>
    /// <param name="destination">array where to write the compressed output.</param>
    /// <param name="destinationIndex">where to write the compressed output in out.</param>
    /// <param name="length">length of the compressed data (ignored by some schemes).</param>
    void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length);
}