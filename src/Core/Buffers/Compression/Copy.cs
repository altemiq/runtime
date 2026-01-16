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
    (int Read, int Written) ICompressHeadlessCodec<int, int>.Compress(ReadOnlySpan<int> source, Span<int> destination) => CopyData(source, destination, source.Length);

    /// <inheritdoc/>
    (int Read, int Written) IDecompressHeadlessCodec<int, int>.Decompress(ReadOnlySpan<int> source, Span<int> destination) => CopyData(source, destination, source.Length);

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination) => CopyData(source, destination, source.Length);

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination) => CopyData(source, destination, source.Length);

    /// <inheritdoc/>
    public override string ToString() => nameof(Copy);

    private static (int Read, int Written) CopyData(ReadOnlySpan<int> source, Span<int> destination, int length)
    {
        source[..length].CopyTo(destination);
        return (length, length);
    }
}