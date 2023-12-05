// -----------------------------------------------------------------------
// <copyright file="JoinedSpanSplitEnumerator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// <see cref="JoinedSpanSplitEnumerator{T}"/> allows for enumeration of each element within a <see cref="ReadOnlySpan{T}"/>
/// that has been split using a provided separator, and joined by the provided joiner.
/// </summary>
/// <typeparam name="T">The type of element in the span.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public ref struct JoinedSpanSplitEnumerator<T>
    where T : IEquatable<T>
{
    private readonly ReadOnlySpan<T> buffer;

    private readonly ReadOnlySpan<T> separators;
    private readonly T separator;

    private readonly ReadOnlySpan<T> joiners;
    private readonly T joiner;

    private readonly int separatorLength;
    private readonly bool splitOnSingleToken;

    private readonly int joinerLength;
    private readonly bool joinOnSingleToken;

    private readonly bool isInitialized;

    private readonly StringSplitOptions options;

    private int startCurrent;
    private int endCurrent;
    private int startNext;

    /// <summary>
    /// Initialises a new instance of the <see cref="JoinedSpanSplitEnumerator{T}"/> struct.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="separators">The separators.</param>
    /// <param name="joiners">The joiners.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    internal JoinedSpanSplitEnumerator(ReadOnlySpan<T> span, ReadOnlySpan<T> separators, ReadOnlySpan<T> joiners, StringSplitOptions options)
    {
        this.isInitialized = true;
        this.buffer = span;
        this.separators = separators;
        this.separator = default!;
        this.splitOnSingleToken = false;
        this.joiner = default!;
        this.joiners = joiners;
        this.joinOnSingleToken = false;
        this.separatorLength = this.separators.Length != 0 ? this.separators.Length : 1;
        this.joinerLength = this.joiners.Length != 0 ? this.joiners.Length : 1;
        this.options = options;
        this.startCurrent = 0;
        this.endCurrent = 0;
        this.startNext = 0;
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="JoinedSpanSplitEnumerator{T}"/> struct.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="joiner">The joiner.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    internal JoinedSpanSplitEnumerator(ReadOnlySpan<T> span, T separator, T joiner, StringSplitOptions options)
    {
        this.isInitialized = true;
        this.buffer = span;
        this.separator = separator;
        this.separators = default;
        this.splitOnSingleToken = true;
        this.joiner = joiner;
        this.joiners = default;
        this.joinOnSingleToken = true;
        this.separatorLength = 1;
        this.joinerLength = 1;
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
    public readonly JoinedSpanSplitEnumerator<T> GetEnumerator() => this;

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

        int elementLength;
        var currentValid = false;
        var removeEmpty = this.options.HasFlag(StringSplitOptions.RemoveEmptyEntries);
        do
        {
            if (this.startNext > this.buffer.Length)
            {
                break;
            }

            var slice = this.buffer[this.startNext..];

            // go through the slice to get the next value
            var separatorIndex = -1;
            var combining = false;
            var currentStart = 0;
            while (true)
            {
                var testSlice = slice[currentStart..];
                var tempSeparatorIndex = this.splitOnSingleToken ? testSlice.IndexOf(this.separator) : testSlice.IndexOf(this.separators);
                if (tempSeparatorIndex < 0)
                {
                    break;
                }

                var joinCount = this.joinOnSingleToken
                    ? GetJoinCountSingle(testSlice, this.joiner, this.joinerLength)
                    : GetJoinCountMultiple(testSlice, this.joiners, this.joinerLength);
                if (joinCount % 2 != 0)
                {
                    combining = !combining;
                }

                if (!combining)
                {
                    separatorIndex = currentStart + tempSeparatorIndex;
                }

                currentStart = tempSeparatorIndex + this.separatorLength;
            }

            this.startCurrent = this.startNext;

            elementLength = separatorIndex != -1 ? separatorIndex : slice.Length;

            this.endCurrent = this.startCurrent + elementLength;
            this.startNext = this.endCurrent + this.separatorLength;

            currentValid = !removeEmpty || elementLength != 0;
        }
        while (!currentValid);

        return currentValid;

        static int GetJoinCountSingle(ReadOnlySpan<T> span, T joiner, int joinerLength)
        {
            var count = 0;
            int index;
            while ((index = span.IndexOf(joiner)) >= 0)
            {
                count++;
                index += joinerLength;
                span = span[index..];
            }

            return count;
        }

        static int GetJoinCountMultiple(ReadOnlySpan<T> span, ReadOnlySpan<T> joiners, int joinerLength)
        {
            var count = 0;
            int index;
            while ((index = span.IndexOf(joiners)) >= 0)
            {
                count++;
                index += joinerLength;
                span = span[index..];
            }

            return count;
        }
    }
}