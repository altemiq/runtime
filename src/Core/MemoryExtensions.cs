// -----------------------------------------------------------------------
// <copyright file="MemoryExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// Parses the <see cref="ReadOnlySpan{T}"/> to an instance of <typeparamref name="TOutput"/>.
/// </summary>
/// <typeparam name="TInput">The type of span.</typeparam>
/// <typeparam name="TOutput">The output type.</typeparam>
/// <param name="input">The input value.</param>
/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
/// <returns>The output value.</returns>
public delegate TOutput Parse<TInput, out TOutput>(ReadOnlySpan<TInput> input, IFormatProvider? provider);

/// <summary>
/// Parses out the value of <typeparamref name="T"/> from the specified value.
/// </summary>
/// <typeparam name="T">The type to parse.</typeparam>
/// <param name="input">The input value.</param>
/// <returns>The parsed value.</returns>
public delegate T Parse<out T>(ReadOnlySpan<char> input);

/// <summary>
/// Tries to parse the <see cref="ReadOnlySpan{T}"/> to an instance of <typeparamref name="TOutput"/> and returns whether it was successful.
/// </summary>
/// <typeparam name="TInput">The type of span.</typeparam>
/// <typeparam name="TOutput">The output type.</typeparam>
/// <param name="input">The input value.</param>
/// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
/// <param name="output">The output value.</param>
/// <returns><see langword="true"/> if <paramref name="output" /> was obtained; otherwise <see langword="false"/>.</returns>
public delegate bool TryParse<TInput, TOutput>(ReadOnlySpan<TInput> input, IFormatProvider? provider, out TOutput output);

/// <summary>
/// Tries to parse out the value of <typeparamref name="T"/> from the specified value with the return indicating success.
/// </summary>
/// <typeparam name="T">The type to parse.</typeparam>
/// <param name="input">The input value.</param>
/// <param name="value">The parsed value.</param>
/// <returns><see langword="true"/> if successful; otherwise <see langword="false"/>.</returns>
public delegate bool TryParse<T>(ReadOnlySpan<char> input, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T value);

#pragma warning disable CA1708, RCS1263, SA1101, S2325

/// <summary>
/// Memory extensions.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "S1133:Deprecated code should be removed", Justification = "These will be removed later")]
public static partial class MemoryExtensions
{
    /// <content>Memory extensions.</content>
    /// <param name="span">The span.</param>
    extension(ReadOnlySpan<char> span)
    {
        /// <summary>
        /// Returns a new span in which all occurrences of a specified span in the current instance are replaced with another specified span.
        /// </summary>
        /// <param name="oldValue">The span to be replaced.</param>
        /// <param name="newValue">The string to replace all occurrences of <paramref name="oldValue"/>.</param>
        /// <returns>A <see cref="ReadOnlySpan{T}"/> that is equivalent to this instance except that all instances of <paramref name="oldValue"/> are replaced with <paramref name="newValue"/>. If <paramref name="oldValue"/> is not found in the current instance, the method returns this instance unchanged.</returns>
        /// <exception cref="InvalidOperationException">The length of <paramref name="newValue"/> must be less than <paramref name="oldValue"/>.</exception>
        public ReadOnlySpan<char> Replace(ReadOnlySpan<char> oldValue, ReadOnlySpan<char> newValue)
        {
#pragma warning disable S3236
            ArgumentOutOfRangeException.ThrowIfGreaterThan(newValue.Length, oldValue.Length, nameof(newValue));
#pragma warning restore S3236

            var oldLength = oldValue.Length;
            var newLength = newValue.Length;

            Span<char> working = default;

            var current = 0;
            var length = span.Length;
            int idx;
            while ((idx = span.IndexOf(oldValue, StringComparison.Ordinal)) is not -1)
            {
                Initialise(ref working, length);
                span[..idx].CopyTo(working[current..]);
                current += idx;
                newValue.CopyTo(working[current..]);
                current += newLength;

                idx += oldLength;
                span = span[idx..];
            }

            if (current is 0)
            {
                // no changes, just return the span.
                return span;
            }

            // copy the rest
            span.CopyTo(working[current..]);
            current += span.Length;

            return working[..current];

            void Initialise(ref Span<char> span, int initLength)
            {
                if (span is { Length: 0 })
                {
                    span = new(new char[initLength - (oldLength - newLength)]);
                }
            }
        }

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using a single space as a separator character.
        /// </summary>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
#if NET9_0_OR_GREATER
        [Obsolete($"Use {nameof(System)}.{nameof(System.MemoryExtensions)}.{nameof(System.MemoryExtensions.Split)} instead")]
#endif
        public SpanSplitEnumerator<char> Split() => span.Split(StringSplitOptions.None);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using a single space as a separator character.
        /// </summary>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
        public SpanSplitEnumerator<char> Split(StringSplitOptions options) => new(span, ' ', options);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator character.
        /// </summary>
        /// <param name="separator">The separator character to be used to split the provided span.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
#if NET9_0_OR_GREATER
        [Obsolete($"Use {nameof(System)}.{nameof(System.MemoryExtensions)}.{nameof(System.MemoryExtensions.Split)}({nameof(ReadOnlySpan<char>)}, {nameof(Char)}) instead")]
#endif
        public SpanSplitEnumerator<char> Split(char separator) => span.Split(separator, StringSplitOptions.None);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator character.
        /// </summary>
        /// <param name="separator">The separator character to be used to split the provided span.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
        public SpanSplitEnumerator<char> Split(char separator, StringSplitOptions options) => new(span, separator, options);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator string.
        /// </summary>
        /// <param name="separator">The separator string to be used to split the provided span.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
#if NET9_0_OR_GREATER
        [Obsolete($"Use {nameof(System)}.{nameof(System.MemoryExtensions)}.{nameof(System.MemoryExtensions.Split)}({nameof(ReadOnlySpan<char>)}, {nameof(ReadOnlySpan<char>)}) instead")]
#else
        [Obsolete($"Use {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(Split)}({nameof(ReadOnlySpan<char>)}, {nameof(ReadOnlySpan<char>)}) instead")]
#endif
        public SpanSplitEnumerator<char> Split(string separator) => span.Split(separator, StringSplitOptions.None);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator string.
        /// </summary>
        /// <param name="separator">The separator string to be used to split the provided span.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
#if NET9_0_OR_GREATER
        [Obsolete($"Use {nameof(System)}.{nameof(System.MemoryExtensions)}.{nameof(System.MemoryExtensions.Split)}({nameof(ReadOnlySpan<char>)}, {nameof(ReadOnlySpan<char>)}) instead")]
#endif
        public SpanSplitEnumerator<char> Split(ReadOnlySpan<char> separator) => new(span, separator, StringSplitOptions.None);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator string.
        /// </summary>
        /// <param name="separator">The separator string to be used to split the provided span.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
        [Obsolete($"Use span based {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(Split)} methods instead")]
        public SpanSplitEnumerator<char> Split(string separator, StringSplitOptions options) => span.Split(separator.AsSpan(), options);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator string.
        /// </summary>
        /// <param name="separator">The separator string to be used to split the provided span.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
        public SpanSplitEnumerator<char> Split(ReadOnlySpan<char> separator, StringSplitOptions options) => new(span, separator, options);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator string.
        /// </summary>
        /// <param name="first">The first separator string to be used to split the provided span.</param>
        /// <param name="second">The second separator string to be used to split the provided span.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
        public SpanSplitEnumerator<char> Split(string first, string second) => span.Split(first, second, StringSplitOptions.None);

        /// <summary>
        /// Returns a type that allows for enumeration of each element within a split span
        /// using the provided separator strings.
        /// </summary>
        /// <param name="first">The first separator string to be used to split the provided span.</param>
        /// <param name="second">The second separator string to be used to split the provided span.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="SpanSplitEnumerator{T}"/>.</returns>
        public SpanSplitEnumerator<char> Split(string first, string second, StringSplitOptions options) => new(
            span,
            first.AsSpan(),
            second.AsSpan(),
            options);

        /// <summary>
        /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">A character that delimits the substrings.</param>
        /// <returns>Returns a <see cref="JoinedSpanSplitEnumerator{T}"/>.</returns>
        public JoinedSpanSplitEnumerator<char> SplitQuoted(char separator) => span.SplitQuoted(separator, StringSplitOptions.None);

        /// <summary>
        /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">A character that delimits the substrings.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="JoinedSpanSplitEnumerator{T}"/>.</returns>
        public JoinedSpanSplitEnumerator<char> SplitQuoted(char separator, StringSplitOptions options) => new(span, separator, '\"', options);

        /// <summary>
        /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">A character that delimits the substrings.</param>
        /// <returns>Returns a <see cref="JoinedSpanSplitEnumerator{T}"/>.</returns>
        [Obsolete($"Use span based {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(Split)} methods instead")]
        public JoinedSpanSplitEnumerator<char> SplitQuoted(string separator) => span.SplitQuoted(separator, StringSplitOptions.None);

        /// <summary>
        /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">A character that delimits the substrings.</param>
        /// <returns>Returns a <see cref="JoinedSpanSplitEnumerator{T}"/>.</returns>
        public JoinedSpanSplitEnumerator<char> SplitQuoted(ReadOnlySpan<char> separator) => span.SplitQuoted(separator, StringSplitOptions.None);

        /// <summary>
        /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">A character that delimits the substrings.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="JoinedSpanSplitEnumerator{T}"/>.</returns>
        [Obsolete($"Use span based {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(Split)} methods instead")]
        public JoinedSpanSplitEnumerator<char> SplitQuoted(string separator, StringSplitOptions options) => span.SplitQuoted(separator.AsSpan(), options);

        /// <summary>
        /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">A character that delimits the substrings.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>Returns a <see cref="JoinedSpanSplitEnumerator{T}"/>.</returns>
        public JoinedSpanSplitEnumerator<char> SplitQuoted(ReadOnlySpan<char> separator, StringSplitOptions options) => new(
            span,
            separator,
            "\"".AsSpan(),
            options);

        /// <summary>
        /// Gets the next value from <paramref name="enumerator"/> or throw.
        /// </summary>
        /// <param name="enumerator">The enumerator.</param>
        /// <returns>The next value.</returns>
        /// <exception cref="InvalidOperationException">No more values.</exception>
        public ReadOnlySpan<char> GetNextOrThrow(ref SpanSplitEnumerator<char> enumerator) => enumerator.MoveNext()
            ? span[enumerator.Current]
            : throw new InvalidOperationException();

        /// <summary>
        /// Gets the next value from the span.
        /// </summary>
        /// <typeparam name="T">The type of value to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="parser">The parser for <typeparamref name="T"/>.</param>
        /// <returns>The result from <paramref name="parser"/> for the next value from this instance.</returns>
        public T GetNextValue<T>(ref SpanSplitEnumerator<char> enumerator, Parse<T> parser) => parser(span.GetNextOrThrow(ref enumerator));

        /// <summary>
        /// Gets the next value from the span.
        /// </summary>
        /// <typeparam name="T">The type of value to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="parser">The parser for <typeparamref name="T"/>.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The result from <paramref name="parser"/> for the next value from this instance.</returns>
        public T GetNextValue<T>(ref SpanSplitEnumerator<char> enumerator, Parse<char, T> parser, IFormatProvider? provider) => parser(span.GetNextOrThrow(ref enumerator), provider);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        /// <summary>
        /// Gets the next value from the span as the specified enum.
        /// </summary>
        /// <typeparam name="T">The type of enum to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <returns>The result from <see cref="Enum.Parse{T}(string)"/> for the next value from this instance.</returns>
#else
        /// <summary>
        /// Gets the next value from the span as the specified enum.
        /// </summary>
        /// <typeparam name="T">The type of enum to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <returns>The result from <see cref="Enum.Parse(Type, string)"/> for the next value from this instance.</returns>
#endif
        public T GetNextEnum<T>(ref SpanSplitEnumerator<char> enumerator)
            where T : struct, Enum =>
#if NET6_0_OR_GREATER
            Enum.Parse<T>(span.GetNextOrThrow(ref enumerator));
#elif NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            Enum.Parse<T>(span.GetNextOrThrow(ref enumerator).ToString());
#else
            (T)Enum.Parse(typeof(T), span.GetNextOrThrow(ref enumerator).ToString());
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
        /// <summary>
        /// Gets the next value from the span as the specified enum.
        /// </summary>
        /// <typeparam name="T">The type of enum to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
        /// <returns>The result from <see cref="Enum.Parse{T}(string, bool)"/> for the next value from this instance.</returns>
#else
        /// <summary>
        /// Gets the next value from the span as the specified enum.
        /// </summary>
        /// <typeparam name="T">The type of enum to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
        /// <returns>The result from <see cref="Enum.Parse(Type, string, bool)"/> for the next value from this instance.</returns>
#endif
        public T GetNextEnum<T>(ref SpanSplitEnumerator<char> enumerator, bool ignoreCase)
            where T : struct, Enum =>
#if NET6_0_OR_GREATER
            Enum.Parse<T>(span.GetNextOrThrow(ref enumerator), ignoreCase);
#elif NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            Enum.Parse<T>(span.GetNextOrThrow(ref enumerator).ToString(), ignoreCase);
#else
            (T)Enum.Parse(typeof(T), span.GetNextOrThrow(ref enumerator).ToString(), ignoreCase);
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <summary>
        /// Gets the next value from the span as a <see cref="double"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="double"/> value.</returns>
        public double GetNextDouble(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, IFormatProvider? provider = null) =>
            span.GetNextValue(ref enumerator, v => double.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="float"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="float"/> value.</returns>
        public float GetNextSingle(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, IFormatProvider? provider = null) =>
            span.GetNextValue(ref enumerator, v => float.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="short"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="short"/> value.</returns>
        public short GetNextInt16(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => short.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="ushort"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="ushort"/> value.</returns>
        [CLSCompliant(false)]
        public ushort GetNextUInt16(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => ushort.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="int"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="int"/> value.</returns>
        public int GetNextInt32(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => int.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="uint"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="uint"/> value.</returns>
        [CLSCompliant(false)]
        public uint GetNextUInt32(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => uint.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="long"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="long"/> value.</returns>
        public long GetNextInt64(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => long.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="ulong"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="ulong"/> value.</returns>
        [CLSCompliant(false)]
        public ulong GetNextUInt64(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) =>
            span.GetNextValue(ref enumerator, v => ulong.Parse(v, style, provider));

        /// <summary>
        /// Gets the next value from the span as a <see cref="byte"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <returns>The next value from the span as a <see cref="byte"/> value.</returns>
        public byte GetNextByte(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) =>
            span.GetNextValue(ref enumerator, v => byte.Parse(v, style: style, provider: provider));
#endif

        /// <summary>
        /// Gets the next value from the span as a <see cref="string"/> value.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <returns>The next value from the span as a <see cref="string"/> value.</returns>
        public string GetNextString(ref SpanSplitEnumerator<char> enumerator) => span.GetNextValue(ref enumerator, static v => v.ToString());

        /// <summary>
        /// Gets the value array from the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of span.</typeparam>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="parser">The parser.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>The value array.</returns>
        [Obsolete($"Use span based {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(GetValues)}<T> instead")]
        public T[] GetValues<T>(string separator, int count, Parse<char, T> parser, IFormatProvider? provider) => span.GetValues(separator.AsSpan(), count, parser, provider);

        /// <summary>
        /// Gets the value array from the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of span.</typeparam>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="parser">The parser.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>The value array.</returns>
        public T[] GetValues<T>(ReadOnlySpan<char> separator, int count, Parse<char, T> parser, IFormatProvider? provider) => GetValues(
            span,
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, separator),
#else
            span.Split(separator),
#endif
            count,
            parser,
            provider);

        /// <summary>
        /// Gets the value array from the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <typeparam name="T">The type of span.</typeparam>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="parser">The parser.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>The value array.</returns>
        public T[] GetValues<T>(char separator, int count, Parse<char, T> parser, IFormatProvider? provider) => GetValues(
            span,
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, separator),
#else
            span.Split(separator),
#endif
            count,
            parser,
            provider);

        /// <summary>
        /// Tries to get the next value from the span, with the result indicating success.
        /// </summary>
        /// <typeparam name="T">The type of value to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="parser">The parser for <typeparamref name="T"/>.</param>
        /// <param name="value">The result from <paramref name="parser"/> for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextValue<T>(ref SpanSplitEnumerator<char> enumerator, TryParse<T> parser, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? value)
        {
            if (enumerator.MoveNext())
            {
                return parser(span[enumerator.Current].Trim(), out value);
            }

            value = default;
            return false;
        }

        /// <summary>
        /// Tries to get the next value from the span as the specified enum, with the result indicating success.
        /// </summary>
        /// <typeparam name="T">The type of enum to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="value">The enum result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextEnum<T>(ref SpanSplitEnumerator<char> enumerator, out T value)
            where T : struct, Enum
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            static bool TryParse(ReadOnlySpan<char> span, out T value)
            {
#if NET6_0_OR_GREATER
                return Enum.TryParse(span, ignoreCase: false, out value);
#else
                return Enum.TryParse(span.ToString(), ignoreCase: false, out value);
#endif
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as the specified enum, with the result indicating success.
        /// </summary>
        /// <typeparam name="T">The type of enum to get.</typeparam>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
        /// <param name="value">The enum result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextEnum<T>(ref SpanSplitEnumerator<char> enumerator, bool ignoreCase, out T value)
            where T : struct, Enum
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> input, out T output)
            {
#if NET6_0_OR_GREATER
                return Enum.TryParse(input, ignoreCase, out output);
#else
                return Enum.TryParse(input.ToString(), ignoreCase, out output);
#endif
            }
        }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <summary>
        /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="double"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextDouble(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out double value) => span.TryGetNextDouble(ref enumerator, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="double"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextDouble(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out double value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out double value)
            {
                return double.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="float"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="float"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextSingle(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out float value) => span.TryGetNextSingle(ref enumerator, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="double"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextSingle(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out float value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out float value)
            {
                return float.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="short"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="short"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextInt16(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out short value) => span.TryGetNextInt16(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="short"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="short"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextInt16(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out short value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out short value)
            {
                return short.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="ushort"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="ushort"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public bool TryGetNextUInt16(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out ushort value) => span.TryGetNextUInt16(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="ushort"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="ushort"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public bool TryGetNextUInt16(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out ushort value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out ushort value)
            {
                return ushort.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="int"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="int"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextInt32(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out int value) => span.TryGetNextInt32(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="int"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="int"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextInt32(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out int value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out int value)
            {
                return int.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="uint"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="uint"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public bool TryGetNextUInt32(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out uint value) => span.TryGetNextUInt32(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="uint"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="uint"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public bool TryGetNextUInt32(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out uint value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out uint value)
            {
                return uint.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="long"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="long"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextInt64(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out long value) => span.TryGetNextInt64(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="long"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="long"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextInt64(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out long value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out long value)
            {
                return long.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="ulong"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="ulong"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public bool TryGetNextUInt64(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out ulong value) => span.TryGetNextUInt64(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="ulong"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="ulong"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        [CLSCompliant(false)]
        public bool TryGetNextUInt64(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out ulong value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out ulong value)
            {
                return ulong.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="byte"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="byte"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextByte(ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out byte value) => span.TryGetNextByte(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="byte"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in this instance.</param>
        /// <param name="provider">An object that supplies culture-specific information about the format of this instance. If provider is <see langword="null"/>, the thread current culture is used.</param>
        /// <param name="value">The <see cref="byte"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextByte(ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out byte value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            bool TryParse(ReadOnlySpan<char> v, out byte value)
            {
                return byte.TryParse(v, style, provider, out value);
            }
        }

        /// <summary>
        /// Tries to get the next value from the span as a <see cref="string"/>, with the result indicating success.
        /// </summary>
        /// <param name="enumerator">The span enumerator.</param>
        /// <param name="value">The <see cref="string"/> result for the next value from this instance.</param>
        /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from this instance; otherwise <see langword="false"/>.</returns>
        public bool TryGetNextString(ref SpanSplitEnumerator<char> enumerator, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string? value)
        {
            return span.TryGetNextValue(ref enumerator, TryParse, out value);

            static bool TryParse(ReadOnlySpan<char> v, out string value)
            {
                value = v.ToString();
                return true;
            }
        }

        /// <summary>
        /// Tries to get the value array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
        /// </summary>
        /// <typeparam name="T">The type of span.</typeparam>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="parser">The parser.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="output">The output values.</param>
        /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
        [Obsolete($"Use span based {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(TryGetValues)}<T> instead")]
        public bool TryGetValues<T>(
            string separator,
            int count,
            TryParse<char, T> parser,
            IFormatProvider? provider,
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
            out T[]? output) => span.TryGetValues(separator.AsSpan(), count, parser, provider, out output);
#endif

        /// <summary>
        /// Tries to get the value array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
        /// </summary>
        /// <typeparam name="T">The type of span.</typeparam>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="parser">The parser.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="output">The output values.</param>
        /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
        public bool TryGetValues<T>(
            ReadOnlySpan<char> separator,
            int count,
            TryParse<char, T> parser,
            IFormatProvider? provider,
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
            out T[]? output) => TryGetValues(
                span,
#if NET9_0_OR_GREATER
                System.MemoryExtensions.Split(span, separator),
#else
                span.Split(separator),
#endif
                count,
                parser,
                provider,
                out output);

        /// <summary>
        /// Tries to get the value array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
        /// </summary>
        /// <typeparam name="T">The type of span.</typeparam>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="parser">The parser.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="output">The output values.</param>
        /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
        public bool TryGetValues<T>(
            char separator,
            int count,
            TryParse<char, T> parser,
            IFormatProvider? provider,
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
            out T[]? output) => TryGetValues(
                span,
#if NET9_0_OR_GREATER
                System.MemoryExtensions.Split(span, separator),
#else
                span.Split(separator),
#endif
                count,
                parser,
                provider,
                out output);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        /// <summary>
        /// Gets the value double array from the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>The value array.</returns>
        [Obsolete($"Use span based {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(GetDoubleValues)} instead")]
        public double[] GetDoubleValues(string separator, int count, IFormatProvider? provider) => span.GetDoubleValues(separator.AsSpan(), count, provider);

        /// <summary>
        /// Gets the value double array from the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>The value array.</returns>
        public double[] GetDoubleValues(ReadOnlySpan<char> separator, int count, IFormatProvider? provider)
        {
            return span.GetValues(separator, count, Parse, provider);

            static double Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
            {
                return double.Parse(input, provider: provider);
            }
        }

        /// <summary>
        /// Gets the value double array from the <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <returns>The value array.</returns>
        public double[] GetDoubleValues(char separator, int count, IFormatProvider? provider)
        {
            return span.GetValues(separator, count, Parse, provider);

            static double Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
            {
                return double.Parse(input, provider: provider);
            }
        }

        /// <summary>
        /// Tries to get the value double array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="output">The output values.</param>
        /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
        [Obsolete($"Use span based {nameof(Altemiq)}.{nameof(MemoryExtensions)}.{nameof(TryGetDoubleValues)} instead")]
        public bool TryGetDoubleValues(
            string separator,
            int count,
            IFormatProvider? provider,
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
            out double[]? output) =>
            span.TryGetDoubleValues(separator.AsSpan(), count, provider, out output);

        /// <summary>
        /// Tries to get the value double array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="output">The output values.</param>
        /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
        public bool TryGetDoubleValues(
            ReadOnlySpan<char> separator,
            int count,
            IFormatProvider? provider,
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
            out double[]? output)
        {
            return span.TryGetValues(separator, count, TryParse, provider, out output);

            static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? provider, out double output)
            {
                return double.TryParse(input, System.Globalization.NumberStyles.Float, provider, out output);
            }
        }

        /// <summary>
        /// Tries to get the value double array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
        /// </summary>
        /// <param name="separator">The separator.</param>
        /// <param name="count">The number of elements to parse.</param>
        /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
        /// <param name="output">The output values.</param>
        /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
        public bool TryGetDoubleValues(
            char separator,
            int count,
            IFormatProvider? provider,
            [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
            out double[]? output)
        {
            return span.TryGetValues(separator, count, TryParse, provider, out output);

            static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? provider, out double output)
            {
                return double.TryParse(input, System.Globalization.NumberStyles.Float, provider, out output);
            }
        }
#endif
    }

    /// <content>
    /// The <see cref="SpanSplitEnumerator{Char}"/> extensions.
    /// </content>
    /// <param name="enumerator">The enumerator.</param>
    extension(ref SpanSplitEnumerator<char> enumerator)
    {
        /// <summary>
        /// Moves to the next value from this instance or throws.
        /// </summary>
        /// <exception cref="InvalidOperationException">No more values.</exception>
        public void MoveNextOrThrow()
        {
            if (!enumerator.MoveNext())
            {
                throw new InvalidOperationException();
            }
        }
    }

    private static TOutput[] GetValues<TInput, TOutput>(
        ReadOnlySpan<TInput> input,
#if NET9_0_OR_GREATER
        System.MemoryExtensions.SpanSplitEnumerator<TInput> enumerator,
#else
        SpanSplitEnumerator<TInput> enumerator,
#endif
        int count,
        Parse<TInput, TOutput> parser,
        IFormatProvider? provider)
        where TInput : IEquatable<TInput>
    {
        var list = new TOutput[count];
        var index = 0;
        foreach (var range in enumerator)
        {
            if (index == count)
            {
                // too many items
                throw new FormatException(Properties.Resources.IncorrectInputString);
            }

            list[index] = parser(input[range], provider);
            index++;
        }

        return index == count
            ? list

            // not enough items
            : throw new FormatException(Properties.Resources.IncorrectInputString);
    }

    private static bool TryGetValues<TInput, TOutput>(
        ReadOnlySpan<TInput> input,
#if NET9_0_OR_GREATER
        System.MemoryExtensions.SpanSplitEnumerator<TInput> enumerator,
#else
        SpanSplitEnumerator<TInput> enumerator,
#endif
        int count,
        TryParse<TInput, TOutput> parser,
        IFormatProvider? provider,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)]
        out TOutput[]? output)
        where TInput : IEquatable<TInput>
    {
        var list = new TOutput[count];
        var index = 0;
        foreach (var range in enumerator)
        {
            // too many items, or failed to parse
            if (index == count
                || !parser(input[range], provider, out var value))
            {
                output = default;
                return false;
            }

            list[index] = value;
            index++;
        }

        if (index != count)
        {
            // not enough items
            output = default;
            return false;
        }

        output = list;
        return true;
    }
}