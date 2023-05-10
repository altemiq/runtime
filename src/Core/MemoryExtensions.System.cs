// -----------------------------------------------------------------------
// <copyright file="MemoryExtensions.System.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace System;
#pragma warning restore IDE0130

/// <summary>
/// Memory extensions.
/// </summary>
public static class MemoryExtensions
{
    /// <summary>
    /// Returns a type that allows for enumeration of each element within a split span
    /// using a single space as a separator character.
    /// </summary>
    /// <param name="span">The source span to be enumerated.</param>
    /// <returns>Returns a <see cref="Altemiq.SpanSplitEnumerator{T}"/>.</returns>
    public static Altemiq.SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span) => new(span, ' ');

    /// <summary>
    /// Returns a type that allows for enumeration of each element within a split span
    /// using the provided separator character.
    /// </summary>
    /// <param name="span">The source span to be enumerated.</param>
    /// <param name="separator">The separator character to be used to split the provided span.</param>
    /// <returns>Returns a <see cref="Altemiq.SpanSplitEnumerator{T}"/>.</returns>
    public static Altemiq.SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span, char separator) => new(span, separator);

    /// <summary>
    /// Returns a type that allows for enumeration of each element within a split span
    /// using the provided separator string.
    /// </summary>
    /// <param name="span">The source span to be enumerated.</param>
    /// <param name="separator">The separator string to be used to split the provided span.</param>
    /// <returns>Returns a <see cref="Altemiq.SpanSplitEnumerator{T}"/>.</returns>
    public static Altemiq.SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span, string separator) => new(span, separator ?? string.Empty);

    /// <summary>
    /// Returns a type that allows for enumeration of each element within a split span
    /// using the provided separator string.
    /// </summary>
    /// <param name="span">The source span to be enumerated.</param>
    /// <param name="first">The first separator string to be used to split the provided span.</param>
    /// <param name="second">The second separator string to be used to split the provided span.</param>
    /// <returns>Returns a <see cref="Altemiq.SpanSplitEnumerator{T}"/>.</returns>
    public static Altemiq.SpanSplitEnumerator<char> Split(this ReadOnlySpan<char> span, string first, string second) => new(span, first ?? string.Empty, second ?? string.Empty);

    /// <summary>
    /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="separator">A character that delimits the substrings in <paramref name="s"/>.</param>
    /// <returns>Returns a <see cref="Altemiq.JoinedSpanSplitEnumerator{T}"/>.</returns>
    public static Altemiq.JoinedSpanSplitEnumerator<char> SplitQuoted(this ReadOnlySpan<char> s, char separator) => new(s, separator, '\"');

    /// <summary>
    /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="separator">A character that delimits the substrings in <paramref name="s"/>.</param>
    /// <returns>Returns a <see cref="Altemiq.JoinedSpanSplitEnumerator{T}"/>.</returns>
    public static Altemiq.JoinedSpanSplitEnumerator<char> SplitQuoted(this ReadOnlySpan<char> s, string separator) => new(s, separator, "\"");
}