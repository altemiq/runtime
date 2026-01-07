// -----------------------------------------------------------------------
// <copyright file="LzmaCompressionOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

#pragma warning disable S2325

/// <summary>
/// Provides compression options to be used with <see cref="LzmaStream"/>.
/// </summary>
public sealed class LzmaCompressionOptions
{
    private const int DefaultDictionary = 23;
    private const int DefaultFastBytes = 128;
    private const int DefaultLiteralContextBits = 3;
    private const int DefaultLiteralPosBits = 0;
    private const int DefaultPosBits = 2;
    private const LzmaMatchFinder DefaultMatchFinder = LzmaMatchFinder.BinaryTree4;

    private const int DictionaryLogSizeMaximumCompress = 30;

    /// <summary>
    /// Gets or sets the dictionary.
    /// </summary>
    public int Dictionary
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, Constants.DictionaryMinimumSize);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, DictionaryLogSizeMaximumCompress);
            field = value;
        }
    }

    = DefaultDictionary;

    /// <summary>
    /// Gets or sets the number of fast bytes.
    /// </summary>
    public int FastBytes
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 5);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, (int)Constants.MatchMaximumLength);
            field = value;
        }
    }

    = DefaultFastBytes;

    /// <summary>
    /// Gets or sets the number of literal context bits.
    /// </summary>
    public int LiteralContextBits
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, (int)Constants.LiteralContextBitsMaximum);
            field = value;
        }
    }

    = DefaultLiteralContextBits;

    /// <summary>
    /// Gets or sets the number of literal position bits.
    /// </summary>
    public int LiteralPositionBits
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, (int)Constants.LiteralPositionStatesBitsEncodingMaximum);
            field = value;
        }
    }

    = DefaultLiteralPosBits;

    /// <summary>
    /// Gets or sets the number of position state bits.
    /// </summary>
    public int PositionStateBits
    {
        get;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 0);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Constants.PositionStatesBitsEncodingMaximum);
            field = value;
        }
    }

    = DefaultPosBits;

    /// <summary>
    /// Gets or sets the match finder.
    /// </summary>
    public LzmaMatchFinder MatchFinder { get; set; } = DefaultMatchFinder;

    /// <summary>
    /// Gets or sets a value indicating whether to write the end of stream marker.
    /// </summary>
    public bool EndMarker { get; set; }

    /// <summary>
    /// Computes the distribution table size.
    /// </summary>
    /// <returns>The distribution table size.</returns>
    internal uint DistributionTableSize()
    {
        var optionsDictionarySize = 1 << this.Dictionary;
        int size;
        for (size = 0; size < DictionaryLogSizeMaximumCompress; size++)
        {
            if (optionsDictionarySize <= (1 << size))
            {
                break;
            }
        }

        return (uint)size * 2;
    }

    /// <summary>
    /// Creates the encoder.
    /// </summary>
    /// <returns>The created encoder.</returns>
    internal LzmaEncoder CreateEncoder() => new(this);
}