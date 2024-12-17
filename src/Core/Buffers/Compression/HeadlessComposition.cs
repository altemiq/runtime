// -----------------------------------------------------------------------
// <copyright file="HeadlessComposition.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Helper class to compose headless schemes.
/// </summary>
/// <remarks>
/// <para>Compose a scheme from a first one (f1) and a second one (f2).</para>
/// <para>The first one is called first and then the second one tries to compress whatever remains from the first run.</para>
/// <para>By convention, the first scheme should be such that if, during decoding, a 32-bit zero is first encountered, then there is no output.</para>
/// </remarks>
/// <param name="first">The first codec.</param>
/// <param name="second">The second codec.</param>
internal class HeadlessComposition(IHeadlessInt32Codec first, IHeadlessInt32Codec second) : IHeadlessInt32Codec
{
    private readonly IHeadlessInt32Codec first = first;
    private readonly IHeadlessInt32Codec second = second;

    /// <inheritdoc/>
    public void Compress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length)
    {
        var init = sourceIndex;
        this.first.Compress(source, ref sourceIndex, destination, ref destinationIndex, length);
        length -= sourceIndex - init;
        this.second.Compress(source, ref sourceIndex, destination, ref destinationIndex, length);
    }

    /// <inheritdoc/>
    public void Decompress(int[] source, ref int sourceIndex, int[] destination, ref int destinationIndex, int length, int number)
    {
        var initialSourceIndex = sourceIndex;
        this.first.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length, number);
        length -= sourceIndex - initialSourceIndex;
        number -= destinationIndex;
        this.second.Decompress(source, ref sourceIndex, destination, ref destinationIndex, length, number);
    }

    /// <inheritdoc/>
    public override string ToString() => this.first + "+" + this.second;
}