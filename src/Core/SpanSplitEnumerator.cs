// -----------------------------------------------------------------------
// <copyright file="SpanSplitEnumerator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// <see cref="SpanSplitEnumerator{T}"/> allows for enumeration of each element within a <see cref="ReadOnlySpan{T}"/> that has been split using a provided separator.
/// </summary>
/// <typeparam name="T">The type of element in the span.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public ref struct SpanSplitEnumerator<T>
    where T : IEquatable<T>
{
    private readonly ReadOnlySpan<T> buffer;

    private readonly ReadOnlySpan<T> firstSpanSeparator;
    private readonly ReadOnlySpan<T> secondSpanSeparator;
    private readonly T separator;

    private readonly int separatorLength;
    private readonly bool splitOnSingleToken;
    private readonly bool multipleTokens;

    private readonly bool isInitialized;

    private readonly StringSplitOptions options;

    private int startCurrent;
    private int endCurrent;
    private int startNext;

    /// <summary>
    /// Initialises a new instance of the <see cref="SpanSplitEnumerator{T}"/> struct.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="separator">The separators.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    internal SpanSplitEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> separator, StringSplitOptions options)
    {
        this.isInitialized = true;
        this.buffer = span;
        this.firstSpanSeparator = separator;
        this.secondSpanSeparator = default;
        this.separator = default!;
        this.splitOnSingleToken = false;
        this.multipleTokens = false;
        this.separatorLength = this.firstSpanSeparator.Length is not 0 ? this.firstSpanSeparator.Length : 1;
        this.options = options;
        this.startCurrent = 0;
        this.endCurrent = 0;
        this.startNext = 0;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SpanSplitEnumerator{T}"/> struct.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="firstSeparator">The first separator.</param>
    /// <param name="secondSeparator">The second separator.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    internal SpanSplitEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> firstSeparator, ReadOnlySpan<T> secondSeparator, StringSplitOptions options)
    {
        this.isInitialized = true;
        this.buffer = span;
        this.firstSpanSeparator = firstSeparator;
        this.secondSpanSeparator = secondSeparator;
        this.separator = default!;
        this.splitOnSingleToken = false;
        this.multipleTokens = true;
        this.separatorLength = -1;
        this.options = options;
        this.startCurrent = 0;
        this.endCurrent = 0;
        this.startNext = 0;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SpanSplitEnumerator{T}"/> struct.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    internal SpanSplitEnumerator(ReadOnlySpan<T> span, T separator, StringSplitOptions options)
    {
        this.isInitialized = true;
        this.buffer = span;
        this.separator = separator;
        this.firstSpanSeparator = this.secondSpanSeparator = default;
        this.splitOnSingleToken = true;
        this.multipleTokens = false;
        this.separatorLength = 1;
        this.options = options;
        this.startCurrent = 0;
        this.endCurrent = 0;
        this.startNext = 0;
    }

    /// <summary>
    /// Gets the current element of the enumeration.
    /// </summary>
    /// <returns>Returns a <see cref="Range"/> instance that indicates the bounds of the current element withing the source span.</returns>
    public readonly Range Current => new(this.startCurrent, this.endCurrent);

    /// <summary>
    /// Returns an enumerator that allows for iteration over the split span.
    /// </summary>
    /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/> that can be used to iterate over the split span.</returns>
    public readonly SpanSplitEnumerator<T> GetEnumerator() => this;

    /// <summary>
    /// Advances the enumerator to the next element of the enumeration.
    /// </summary>
    /// <returns><see langword="true"/> if the enumerator was successfully advanced to the next element; <see langword="false"/> if the enumerator has passed the end of the enumeration.</returns>
    public bool MoveNext()
    {
        if (!this.isInitialized || this.startNext > this.buffer.Length)
        {
            return false;
        }

        var currentValid = false;
        var removeEmpty = this.options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
        do
        {
            if (this.startNext > this.buffer.Length)
            {
                break;
            }

            var slice = this.buffer[this.startNext..];
            this.startCurrent = this.startNext;

            var elementLength = this.multipleTokens
                ? this.MoveNextMultipleTokens(slice)
                : this.MoveNextSingleToken(slice);

            currentValid = !removeEmpty || elementLength is not 0;
        }
        while (!currentValid);

        return currentValid;
    }

    private int MoveNextMultipleTokens(ReadOnlySpan<T> slice)
    {
        var firstSeparatorIndex = slice.IndexOf(this.firstSpanSeparator);
        var secondSeparatorIndex = slice.IndexOf(this.secondSpanSeparator);

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable SA1008
        var (elementLength, length) = (firstSeparatorIndex, secondSeparatorIndex) switch
        {
            ( < 0, >= 0) => (secondSeparatorIndex, this.secondSpanSeparator.Length),
            ( >= 0, >= 0) when secondSeparatorIndex < firstSeparatorIndex => (secondSeparatorIndex, this.secondSpanSeparator.Length),
            ( >= 0, < 0) => (firstSeparatorIndex, this.firstSpanSeparator.Length),
            ( >= 0, >= 0) when firstSeparatorIndex < secondSeparatorIndex => (firstSeparatorIndex, this.firstSpanSeparator.Length),
            _ => (slice.Length, 1),
        };
#pragma warning restore SA1008, RCS1222, IDE0079

        this.endCurrent = this.startCurrent + elementLength;
        this.startNext = this.endCurrent + length;
        return elementLength;
    }

    private int MoveNextSingleToken(ReadOnlySpan<T> slice)
    {
        var separatorIndex = this.splitOnSingleToken ? slice.IndexOf(this.separator) : slice.IndexOf(this.firstSpanSeparator);
        var elementLength = separatorIndex is not -1 ? separatorIndex : slice.Length;

        this.endCurrent = this.startCurrent + elementLength;
        this.startNext = this.endCurrent + this.separatorLength;
        return elementLength;
    }
}