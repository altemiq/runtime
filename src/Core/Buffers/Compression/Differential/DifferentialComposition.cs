// -----------------------------------------------------------------------
// <copyright file="DifferentialComposition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// Helper class to compose differential schemes.
/// </summary>
/// <remarks>
/// <para>Compose a scheme from a first one (f1) and a second one (f2). The first one is called first and then the second one tries to compress whatever remains from the first run.</para>
/// <para>By convention, the first scheme should be such that if, during decoding, a 32-bit zero is first encountered, then there is no output.</para>
/// </remarks>
/// <param name="first">The first codec.</param>
/// <param name="second">The second codec.</param>
internal class DifferentialComposition(IDifferentialInt32Codec first, IDifferentialInt32Codec second) : IDifferentialInt32Codec
{
    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var sourceIndexInit = sourceIndex;
        var destinationIndexInit = destinationIndex;
        first.Compress(source, ref sourceIndex, destination, ref destinationIndex, length);
        if (destinationIndex == destinationIndexInit)
        {
            destination[destinationIndexInit] = 0;
            destinationIndex++;
        }

        length -= sourceIndex - sourceIndexInit;
        second.Compress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        if (length is 0)
        {
            return;
        }

        var init = sourceIndex;
        first.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length);
        length -= sourceIndex - init;
        second.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    public override string ToString() => first + " + " + second;
}