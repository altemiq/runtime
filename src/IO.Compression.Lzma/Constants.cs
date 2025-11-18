// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

/// <summary>
/// The constants class.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The number of registered distances.
    /// </summary>
    public const uint RegisteredDistances = 4U;

    /// <summary>
    /// The number of states.
    /// </summary>
    public const uint States = 12U;

    /// <summary>
    /// The number of position slot bits.
    /// </summary>
    public const int PositionSlotBits = 6;

    /// <summary>
    /// The dictionary size minimum.
    /// </summary>
    public const int DictionaryMinimumSize = 0;

    /// <summary>
    /// The number length to position state bits.
    /// </summary>
    public const int LengthToPositionStatesBits = 2; // it's for speed optimization

    /// <summary>
    /// The number length to position states.
    /// </summary>
    public const uint LengthToPositionStates = 1U << LengthToPositionStatesBits;

    /// <summary>
    /// The match minimum length.
    /// </summary>
    public const uint MatchMinimumLength = 2U;

    /// <summary>
    /// The number of align bits.
    /// </summary>
    public const int AlignBits = 4;

    /// <summary>
    /// The align table size.
    /// </summary>
    public const uint AlignTableSize = 1U << AlignBits;

    /// <summary>
    /// The align mask.
    /// </summary>
    public const uint AlignMask = AlignTableSize - 1;

    /// <summary>
    /// The start position model index.
    /// </summary>
    public const uint StartPositionModelIndex = 4U;

    /// <summary>
    /// The end position model index.
    /// </summary>
    public const uint EndPositionModelIndex = 14U;

    /// <summary>
    /// The number of full distances.
    /// </summary>
    public const uint FullDistances = 1U << ((int)EndPositionModelIndex / 2);

    /// <summary>
    /// The maximum number of literal position states <c>bit</c> encoding.
    /// </summary>
    public const uint LiteralPositionStatesBitsEncodingMaximum = 4U;

    /// <summary>
    /// The maximum number lit context bits.
    /// </summary>
    public const uint LiteralContextBitsMaximum = 8U;

    /// <summary>
    /// The maximum number of position state bits.
    /// </summary>
    public const int PositionStatesBitsMaximum = 4;

    /// <summary>
    /// The maximum number of position states.
    /// </summary>
    public const uint PositionStatesMaximum = 1 << PositionStatesBitsMaximum;

    /// <summary>
    /// The maximum number of position states bits encoding.
    /// </summary>
    public const int PositionStatesBitsEncodingMaximum = 4;

    /// <summary>
    /// The number os position states encoding maximum.
    /// </summary>
    public const uint PositionStatesEncodingMaximum = 1 << PositionStatesBitsEncodingMaximum;

    /// <summary>
    /// The number of low-length bits.
    /// </summary>
    public const int LowLengthBits = 3;

    /// <summary>
    /// The number of mid-length bits.
    /// </summary>
    public const int MidLengthBits = 3;

    /// <summary>
    /// The number of high-length bits.
    /// </summary>
    public const int HighLengthBits = 8;

    /// <summary>
    /// The number of low-length symbols.
    /// </summary>
    public const uint LowLengthSymbols = 1 << LowLengthBits;

    /// <summary>
    /// The number of mid-length symbols.
    /// </summary>
    public const uint MidLengthSymbols = 1 << MidLengthBits;

    /// <summary>
    /// The number of length symbols.
    /// </summary>
    public const uint LengthSymbols = LowLengthSymbols + MidLengthSymbols + (1 << HighLengthBits);

    /// <summary>
    /// The match maximum length.
    /// </summary>
    public const uint MatchMaximumLength = MatchMinimumLength + LengthSymbols - 1;

    /// <summary>
    /// Gets the length to position state.
    /// </summary>
    /// <param name="len">The length.</param>
    /// <returns>The distance to the position state.</returns>
    public static uint GetLenToPosState(uint len)
    {
        len -= MatchMinimumLength;
        return len < LengthToPositionStates
            ? len
            : LengthToPositionStates - 1;
    }
}