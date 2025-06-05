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
internal class HeadlessDifferentialComposition(IHeadlessDifferentialInt32Codec first, IHeadlessDifferentialInt32Codec second) : IHeadlessDifferentialInt32Codec
{
    private readonly IHeadlessDifferentialInt32Codec first = first;
    private readonly IHeadlessDifferentialInt32Codec second = second;

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
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, ref int initialValue)
    {
        if (length is 0)
        {
            return;
        }

        var init = sourceIndex;
        this.first.Compress(source, ref sourceIndex, destination, ref destinationIndex, length, ref initialValue);
        if (destinationIndex is 0)
        {
            destination[0] = 0;
            destinationIndex++;
        }

        length -= sourceIndex - init;
        this.second.Compress(source, ref sourceIndex, destination, ref destinationIndex, length, ref initialValue);
    }

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number, ref int initialValue)
    {
        if (length is 0)
        {
            return;
        }

        var init = sourceIndex;
        this.first.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length, number, ref initialValue);
        length -= sourceIndex - init;
        number -= destinationIndex;
        this.second.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length, number, ref initialValue);
    }

    /// <inheritdoc/>
    public override string ToString() => $"{this.first} + {this.second}";
}