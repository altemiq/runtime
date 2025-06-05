// -----------------------------------------------------------------------
// <copyright file="Int32Compressor.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The <see cref="int"/> compressor.
/// </summary>
/// <param name="codec">The codec.</param>
internal class Int32Compressor(IHeadlessInt32Codec codec)
{
    private readonly IHeadlessInt32Codec codec = codec;

    /// <summary>
    /// Initializes a new instance of the <see cref="Int32Compressor"/> class.
    /// </summary>
    public Int32Compressor()
        : this(new HeadlessComposition(new BinaryPacking(), new VariableByte()))
    {
    }

    /// <summary>
    /// Creates a new instance of the <see cref="Int32Compressor"/> class.
    /// </summary>
    /// <typeparam name="T">The type of compressor.</typeparam>
    /// <returns>The new instance.</returns>
    public static Int32Compressor Create<T>()
        where T : IHeadlessInt32Codec, new() => new(new T());

    /// <summary>
    /// Creates a new instance of the <see cref="Int32Compressor"/> class.
    /// </summary>
    /// <typeparam name="T1">The first type of compressor.</typeparam>
    /// <typeparam name="T2">The second type of compressor.</typeparam>
    /// <returns>The new instance.</returns>
    public static Int32Compressor Create<T1, T2>()
        where T1 : IHeadlessInt32Codec, new()
        where T2 : IHeadlessInt32Codec, new() => new(HeadlessComposition.Create<T1, T2>());

    /// <summary>
    /// Compress an array and returns the compressed result as a new array.
    /// </summary>
    /// <param name="input">input array to be compressed.</param>
    /// <returns>The compressed array.</returns>
    public int[] Compress(int[] input)
    {
        var compressed = new int[input.Length + 1024];
        compressed[0] = input.Length;
        var sourceIndex = 0;
        var destinationIndex = 1;
        this.codec.Compress(input, ref sourceIndex, compressed, ref destinationIndex, input.Length);
        System.Array.Resize(ref compressed, destinationIndex);
        return compressed;
    }

    /// <inheritdoc cref="Decompress"/>
    [Obsolete($"Use {nameof(Int32Compressor)}.{nameof(Decompress)} instead.")]
    public int[] Uncompress(int[] compressed) => this.Decompress(compressed);

    /// <summary>
    /// Decompress an array and returns the uncompressed result as a new array.
    /// </summary>
    /// <param name="compressed">compressed array.</param>
    /// <returns>uncompressed array.</returns>
    public int[] Decompress(int[] compressed)
    {
        var decompressed = new int[compressed[0]];
        var sourceIndex = 1;
        var destinationIndex = 0;
        this.codec.Decompress(compressed, ref sourceIndex, decompressed, ref destinationIndex, compressed.Length - sourceIndex, decompressed.Length);
        return decompressed;
    }

    /// <inheritdoc/>
    public override string? ToString() => this.codec.ToString();
}