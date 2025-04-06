// -----------------------------------------------------------------------
// <copyright file="DifferentialInt32Compressor.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// The differential integer compressor.
/// </summary>
/// <param name="c">The codec.</param>
internal sealed class DifferentialInt32Compressor(IHeadlessDifferentialInt32Codec c)
{
    private readonly IHeadlessDifferentialInt32Codec codec = c;

    /// <summary>
    /// Initializes a new instance of the <see cref="DifferentialInt32Compressor"/> class.
    /// </summary>
    public DifferentialInt32Compressor()
        : this(new HeadlessDifferentialComposition(new DifferentialBinaryPacking(), new DifferentialVariableByte()))
    {
    }

    /// <summary>
    /// Compress an array and returns the compressed result as a new array.
    /// </summary>
    /// <param name="input">The array to be compressed.</param>
    /// <returns>The compressed array.</returns>
    public int[] Compress(int[] input)
    {
        var compressed = new int[input.Length + 1024];
        compressed[0] = input.Length;
        var sourceIndex = 0;
        var destinationIndex = 1;
        var initValue = 0;
        this.codec.Compress(input, ref sourceIndex, compressed, ref destinationIndex, input.Length, ref initValue);
        System.Array.Resize(ref compressed, destinationIndex);
        return compressed;
    }

    /// <inheritdoc cref="Decompress"/>
    [Obsolete($"Use {nameof(Int32Compressor)}.{nameof(Decompress)} instead.")]
    public int[] Uncompress(int[] compressed) => this.Decompress(compressed);

    /// <summary>
    /// Decompress an array and returns the uncompressed result as a new array.
    /// </summary>
    /// <param name="compressed">The compressed array.</param>
    /// <returns>The uncompressed array.</returns>
    public int[] Decompress(int[] compressed)
    {
        var decompressed = new int[compressed[0]];
        var sourceIndex = 1;
        var destinationIndex = 0;
        var initValue = 0;
        this.codec.Decompress(
            compressed,
            ref sourceIndex,
            decompressed,
            ref destinationIndex,
            compressed.Length - sourceIndex,
            decompressed.Length,
            ref initValue);
        return decompressed;
    }
}