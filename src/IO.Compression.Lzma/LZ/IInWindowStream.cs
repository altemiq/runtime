// -----------------------------------------------------------------------
// <copyright file="IInWindowStream.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression.LZ;

/// <summary>
/// The in windows stream.
/// </summary>
internal interface IInWindowStream
{
    /// <summary>
    /// Gets the number of available bytes.
    /// </summary>
    /// <returns>The number of available bytes.</returns>
    uint AvailableBytes { get; }

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    /// <param name="stream">The stream.</param>
    void Init(Stream stream);

    /// <summary>
    /// Releases the stream.
    /// </summary>
    void ReleaseStream();

    /// <summary>
    /// Gets the index bytes.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <returns>The index byte.</returns>
    byte GetIndexByte(int index);

    /// <summary>
    /// Gets the match length.
    /// </summary>
    /// <param name="index">The index.</param>
    /// <param name="distance">The distance.</param>
    /// <param name="limit">The limit.</param>
    /// <returns>The match length.</returns>
    uint GetMatchLength(int index, uint distance, uint limit);
}