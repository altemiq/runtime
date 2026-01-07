// -----------------------------------------------------------------------
// <copyright file="State.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

/// <summary>
/// The state structure.
/// </summary>
internal struct State
{
    /// <summary>
    /// The index.
    /// </summary>
    public uint Index;

    /// <summary>
    /// Updates the character.
    /// </summary>
    public void UpdateChar() =>
        this.Index = this.Index switch
        {
            < 4U => 0U,
            < 10U and var i => i - 3U,
            var i => i - 6U,
        };

    /// <summary>
    /// Updates the match.
    /// </summary>
    public void UpdateMatch() =>
        this.Index = this.Index switch
        {
            < 7U => 7U,
            _ => 10U,
        };

    /// <summary>
    /// Updates the rep.
    /// </summary>
    public void UpdateRep() =>
        this.Index = this.Index switch
        {
            < 7U => 8U,
            _ => 11U,
        };

    /// <summary>
    /// Updates the short rep.
    /// </summary>
    public void UpdateShortRep() =>
        this.Index = this.Index switch
        {
            < 7U => 9U,
            _ => 11U,
        };

    /// <summary>
    /// Gets a value indicating whether this instance is a char.
    /// </summary>
    /// <returns><see langword="true"/> if this is a char; otherwise <see langword="false"/>.</returns>
    public readonly bool IsCharState() => this.Index < 7U;
}