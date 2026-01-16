// -----------------------------------------------------------------------
// <copyright file="IDecompressCodec{TSource,TDestination}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The compress codec.
/// </summary>
/// <typeparam name="TSource">The type of value to decode.</typeparam>
/// <typeparam name="TDestination">The type of value to store.</typeparam>
internal interface IDecompressCodec<TSource, TDestination>
#if NET7_0_OR_GREATER
    where TSource : System.Numerics.IBinaryInteger<TSource>, System.Numerics.ISignedNumber<TSource>
    where TDestination : System.Numerics.IBinaryInteger<TDestination>, System.Numerics.ISignedNumber<TDestination>
#endif
{
    /// <summary>
    /// Decompress data from an array to another array.
    /// </summary>
    /// <param name="source">array containing data in compressed form.</param>
    /// <param name="destination">array where to write the compressed output.</param>
    /// <returns>How much data was read and written to.</returns>
    (int Read, int Written) Decompress(ReadOnlySpan<TSource> source, Span<TDestination> destination);
}