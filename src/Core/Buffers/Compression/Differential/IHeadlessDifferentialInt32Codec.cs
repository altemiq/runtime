// -----------------------------------------------------------------------
// <copyright file="IHeadlessDifferentialInt32Codec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// <para>Interface describing a standard codec to compress integers.</para>
/// <para>This is a variation on the <see cref="IInt32Codec"/> interface meant to be used for random access and with integrated differential coding (i.e., given a large array, you can segment it and decode just the subarray you need).</para>
/// </summary>
/// <remarks>
/// <para>The main differences are that we must specify the number of integers we wish to decode as well as the initial value (for differential coding).</para>
/// <para>This information might be stored elsewhere.</para>
/// </remarks>
internal interface IHeadlessDifferentialInt32Codec
{
    /// <summary>
    /// Compress data from an array to another array.
    /// </summary>
    /// <remarks>
    /// Both <paramref name="sourceIndex"/> and <paramref name="destinationIndex"/> are modified to represent how much data was read and written to if 12 ints (length = 12) are compressed to 3 ints,
    /// then <paramref name="sourceIndex"/> will be incremented by 12 while <paramref name="destinationIndex"/> will be incremented by 3.
    /// </remarks>
    /// <param name="source">The input array.</param>
    /// <param name="sourceIndex">The location in the input array.</param>
    /// <param name="destination">The output array.</param>
    /// <param name="destinationIndex">Where to write in the output array.</param>
    /// <param name="length">How many integers to compress.</param>
    /// <param name="initialValue">The initial value for the purpose of differential coding, the value is automatically updated.</param>
    void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, ref int initialValue);

    /// <summary>
    /// Decompress data from an array to another array.
    /// </summary>
    /// <remarks>
    /// Both <paramref name="sourceIndex"/> and <paramref name="destinationIndex"/> parameters are modified to indicate new positions after read/write.
    /// </remarks>
    /// <param name="source">The array containing data in compressed form.</param>
    /// <param name="sourceIndex">Where to start reading in the array.</param>
    /// <param name="destination">The array where to write the compressed output.</param>
    /// <param name="destinationIndex">Where to write the compressed output in out.</param>
    /// <param name="length">The length of the compressed data (ignored by some schemes).</param>
    /// <param name="number">The number of integers we want to decode, the actual number of integers decoded can be less.</param>
    /// <param name="initialValue">The initial value for the purpose of differential coding, the value is automatically updated.</param>
    void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number, ref int initialValue);
}