// -----------------------------------------------------------------------
// <copyright file="ICompressCodec{TSource,TDestination}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The compress codec.
/// </summary>
/// <typeparam name="TSource">The type of value to encode.</typeparam>
/// <typeparam name="TDestination">The type of value to store.</typeparam>
internal interface ICompressCodec<TSource, TDestination>
#if NET7_0_OR_GREATER
    where TSource : System.Numerics.IBinaryInteger<TSource>, System.Numerics.ISignedNumber<TSource>
    where TDestination : System.Numerics.IBinaryInteger<TDestination>, System.Numerics.ISignedNumber<TDestination>
#endif
{
    /// <summary>
    /// Compress data from a span to another span.
    /// </summary>
    /// <param name="source">The input span.</param>
    /// <param name="destination">The output span.</param>
    /// <returns>How much data was read and written to.</returns>
    (int Read, int Written) Compress(ReadOnlySpan<TSource> source, Span<TDestination> destination);
}