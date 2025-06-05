// -----------------------------------------------------------------------
// <copyright file="Composition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Helper class to compose schemes.
/// </summary>
/// <remarks>
/// <para>Compose a scheme from a first one (f1) and a second one (f2). The
/// first one is called first and then the second one tries to compress
/// whatever remains from the first run.</para>
/// <para>By convention, the first scheme should be such that if, during
/// decoding, a 32-bit zero is first encountered, then there is no
/// output.</para>
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="Composition"/> class.
/// </remarks>
/// <param name="first">The first codec.</param>
/// <param name="second">The second codec.</param>
internal class Composition(IInt32Codec first, IInt32Codec second) : IInt32Codec
{
    private readonly IInt32Codec first = first;
    private readonly IInt32Codec second = second;

    /// <summary>
    /// Creates a new instance using the specified codecs.
    /// </summary>
    /// <typeparam name="T1">The first type.</typeparam>
    /// <typeparam name="T2">The second type.</typeparam>
    /// <returns>The created composision.</returns>
    public static Composition Create<T1, T2>()
        where T1 : IInt32Codec, new()
        where T2 : IInt32Codec, new() => new(new T1(), new T2());

    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var initialSourceIndex = sourceIndex;
        var initialDestinationIndex = destinationIndex;
        this.first.Compress(source, ref sourceIndex, destination, ref destinationIndex, length);
        if (destinationIndex == initialDestinationIndex)
        {
            destination[initialDestinationIndex] = 0;
            destinationIndex++;
        }

        length -= sourceIndex - initialSourceIndex;
        this.second.Compress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var init = sourceIndex;
        this.first.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length);
        length -= sourceIndex - init;
        this.second.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    public override string ToString() => this.first + " + " + this.second;
}