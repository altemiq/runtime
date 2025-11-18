// -----------------------------------------------------------------------
// <copyright file="CoderPropId.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

/// <summary>
/// Provides the fields that represent properties identifiers for compressing.
/// </summary>
public enum CoderPropId
{
    /// <summary>
    /// Specifies default property.
    /// </summary>
    DefaultProp = 0,

    /// <summary>
    /// Specifies size of dictionary.
    /// </summary>
    DictionarySize,

    /// <summary>
    /// Specifies size of memory for PPM*.
    /// </summary>
    UsedMemorySize,

    /// <summary>
    /// Specifies order for PPM methods.
    /// </summary>
    Order,

    /// <summary>
    /// Specifies Block Size.
    /// </summary>
    BlockSize,

    /// <summary>
    /// Specifies number of position state bits for LZMA (0 &lt;= x &lt;= 4).
    /// </summary>
    PositionStateBits,

    /// <summary>
    /// Specifies number of literal context bits for LZMA (0 &lt;= x &lt;= 8).
    /// </summary>
    LiteralContextBits,

    /// <summary>
    /// Specifies number of literal position bits for LZMA (0 &lt;= x &lt;= 4).
    /// </summary>
    LiteralPositionBits,

    /// <summary>
    /// Specifies number of fast bytes for LZ*.
    /// </summary>
    FastBytes,

    /// <summary>
    /// Specifies match finder. LZMA: "BT2", "BT4" or "BT4B".
    /// </summary>
    MatchFinder,

    /// <summary>
    /// Specifies the number of match finder cycles.
    /// </summary>
    MatchFinderCycles,

    /// <summary>
    /// Specifies number of passes.
    /// </summary>
    Passes,

    /// <summary>
    /// Specifies number of algorithm.
    /// </summary>
    Algorithm,

    /// <summary>
    /// Specifies the number of threads.
    /// </summary>
    Threads,

    /// <summary>
    /// Specifies mode with end marker.
    /// </summary>
    EndMarker,
}