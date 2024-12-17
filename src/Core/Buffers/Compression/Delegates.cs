// -----------------------------------------------------------------------
// <copyright file="Delegates.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Compressed data from an input array into an output array.
/// </summary>
/// <param name="source">The array to compress.</param>
/// <param name="sourceIndex">Where to start reading.</param>
/// <param name="destination">The output array.</param>
/// <param name="destinationIndex">The location in the output array.</param>
/// <param name="length">How many integers to read.</param>
/// <returns>The number of 32-bit words written (in compressed form).</returns>
internal delegate int Compress(int[] source, int sourceIndex, int[] destination, int destinationIndex, int length);

/// <summary>
/// Estimate size of the compressed output.
/// </summary>
/// <param name="source">The array to compress.</param>
/// <param name="index">Where to start reading.</param>
/// <param name="length">How many integers to read.</param>
/// <returns>The estimated size of the output (in 32-bit integers).</returns>
internal delegate int EstimateCompress(int[] source, int index, int length);

/// <summary>
/// Uncompressed data from an input array into an output array.
/// </summary>
/// <param name="source">The input array (in compressed form).</param>
/// <param name="sourceIndex">The starting location in the compressed input array.</param>
/// <param name="sourceLength">How much data we wish the read (in 32-bit words).</param>
/// <param name="destination">The output array (in decompressed form).</param>
/// <param name="destinationIndex">The current position in the output array.</param>
/// <param name="destinationLength">The available data in the output array.</param>
internal delegate void Decompress(int[] source, int sourceIndex, int sourceLength, int[] destination, int destinationIndex, int destinationLength);