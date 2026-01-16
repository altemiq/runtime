// -----------------------------------------------------------------------
// <copyright file="ICompressHeadlessCodec{TSource,TDestination}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The compress codec.
/// </summary>
/// <typeparam name="TSource">The type of value to encode.</typeparam>
/// <typeparam name="TDestination">The type of value to store.</typeparam>
internal interface ICompressHeadlessCodec<TSource, TDestination>
#if NET7_0_OR_GREATER
    where TSource : System.Numerics.IBinaryInteger<TSource>, System.Numerics.ISignedNumber<TSource>
    where TDestination : System.Numerics.IBinaryInteger<TDestination>, System.Numerics.ISignedNumber<TDestination>
#endif
{
    /// <summary>
    /// Compress data from an array to another array.
    /// </summary>
    /// <param name="source">input array.</param>
    /// <param name="destination">output array.</param>
    /// <returns>How much data was read and written to.</returns>
    (int Read, int Written) Compress(ReadOnlySpan<TSource> source, Span<TDestination> destination);
}