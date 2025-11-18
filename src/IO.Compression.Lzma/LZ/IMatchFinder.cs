// -----------------------------------------------------------------------
// <copyright file="IMatchFinder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression.LZ;

/// <summary>
/// The match finder.
/// </summary>
internal interface IMatchFinder : IInWindowStream
{
    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <param name="historySize">The history size.</param>
    /// <param name="addBufferBefore">The keep add buffer before.</param>
    /// <param name="matchLength">The match max length.</param>
    /// <param name="addBufferAfter">The keep add buffer after.</param>
    void Create(uint historySize, uint addBufferBefore, uint matchLength, uint addBufferAfter);

    /// <summary>
    /// Gets the matches.
    /// </summary>
    /// <param name="distances">The distances.</param>
    /// <returns>The matches.</returns>
    uint GetMatches(uint[] distances);

    /// <summary>
    /// Skips the specified number.
    /// </summary>
    /// <param name="num">The number to skip.</param>
    void Skip(uint num);
}