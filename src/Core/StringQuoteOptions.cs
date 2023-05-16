// -----------------------------------------------------------------------
// <copyright file="StringQuoteOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// The string quoting options.
/// </summary>
[Flags]
public enum StringQuoteOptions
{
    /// <summary>
    /// Use the default options when quoting strings.
    /// </summary>
    None = 0,

    /// <summary>
    /// Quote newline characters.
    /// </summary>
    QuoteNewLine = 1 << 0,

    /// <summary>
    /// Quote quote characters.
    /// </summary>
    QuoteQuotes = 1 << 1,

    /// <summary>
    /// Quote non-ascii characters.
    /// </summary>
    QuoteNonAscii = 1 << 2,

    /// <summary>
    /// Default settings.
    /// </summary>
    QuoteAll = QuoteNewLine | QuoteQuotes | QuoteNonAscii,
}