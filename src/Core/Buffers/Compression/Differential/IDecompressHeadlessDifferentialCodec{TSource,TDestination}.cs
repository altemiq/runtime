// -----------------------------------------------------------------------
// <copyright file="IDecompressHeadlessDifferentialCodec{TSource,TDestination}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// <para>Interface describing a standard codec to compress integers.</para>
/// <para>This is a variation on the <see cref="IInt32Codec"/> interface meant to be used for random access and with integrated differential coding (i.e., given a large array, you can segment it and decode just the subarray you need).</para>
/// </summary>
/// <typeparam name="TSource">The type of value to encode.</typeparam>
/// <typeparam name="TDestination">The type of value to store.</typeparam>
/// <remarks>
/// <para>The main differences are that we must specify the number of integers we wish to decode as well as the initial value (for differential coding).</para>
/// <para>This information might be stored elsewhere.</para>
/// </remarks>
internal interface IDecompressHeadlessDifferentialCodec<TSource, TDestination>
#if NET7_0_OR_GREATER
    where TSource : System.Numerics.IBinaryInteger<TSource>, System.Numerics.ISignedNumber<TSource>
    where TDestination : System.Numerics.IBinaryInteger<TDestination>, System.Numerics.ISignedNumber<TDestination>
#endif
{
    /// <summary>
    /// Decompress data from an array to another array.
    /// </summary>
    /// <param name="source">The array containing data in compressed form.</param>
    /// <param name="destination">The array where to write the compressed output.</param>
    /// <param name="initialValue">The initial value for the purpose of differential coding, the value is automatically updated.</param>
    /// <returns>How much data was read and written to.</returns>
    (int Read, int Written) Decompress(ReadOnlySpan<TSource> source, Span<TDestination> destination, ref int initialValue);
}