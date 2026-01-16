// -----------------------------------------------------------------------
// <copyright file="HeadlessDifferentialComposition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// Helper class to compose schemes.
/// </summary>
/// <remarks>
/// <para>
/// Compose a scheme from a first one (f1) and a second one (f2). The first
/// one is called first and then the second one tries to compress whatever
/// remains from the first run.
/// </para>
/// <para>
/// By convention, the first scheme should be such that if, during decoding,
/// a 32-bit zero is first encountered, then there is no output.
/// </para>
/// </remarks>
/// <param name="first">first codec.</param>
/// <param name="second">second codec.</param>
internal sealed class HeadlessDifferentialComposition(IHeadlessDifferentialInt32Codec first, IHeadlessDifferentialInt32Codec second) : IHeadlessDifferentialInt32Codec
{
    /// <summary>
    /// Creates a new instance using the specified codecs.
    /// </summary>
    /// <typeparam name="T1">The first type.</typeparam>
    /// <typeparam name="T2">The second type.</typeparam>
    /// <returns>The created composition.</returns>
    public static HeadlessDifferentialComposition Create<T1, T2>()
        where T1 : IHeadlessDifferentialInt32Codec, new()
        where T2 : IHeadlessDifferentialInt32Codec, new() => new(new T1(), new T2());

    /// <inheritdoc/>
    public (int Read, int Written) Compress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var (firstRead, firstWritten) = first.Compress(source, destination, ref initialValue);
        var (secondRead, secondWritten) = second.Compress(source[firstRead..], destination[firstWritten..], ref initialValue);
        return (firstRead + secondRead, firstWritten + secondWritten);
    }

    /// <inheritdoc/>
    public (int Read, int Written) Decompress(ReadOnlySpan<int> source, Span<int> destination, ref int initialValue)
    {
        if (source.Length is 0)
        {
            return default;
        }

        var (firstRead, firstWritten) = first.Decompress(source, destination, ref initialValue);
        var (secondRead, secondWritten) = second.Decompress(source[firstRead..], destination[firstWritten..], ref initialValue);
        return (firstRead + secondRead, firstWritten + secondWritten);
    }

    /// <inheritdoc/>
    public override string ToString() => $"{first} + {second}";
}