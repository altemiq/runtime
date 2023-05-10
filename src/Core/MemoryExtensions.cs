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
public delegate TOutput Parse<TInput, TOutput>(ReadOnlySpan<TInput> input, IFormatProvider? provider);

/// <summary>
/// Parses out the value of <typeparamref name="T"/> from the specified value.
/// </summary>
/// <typeparam name="T">The type to parse.</typeparam>
/// <param name="input">The input value.</param>
/// <returns>The parsed value.</returns>
public delegate T Parse<T>(ReadOnlySpan<char> input);

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

/// <summary>
/// Memory extensions.
/// </summary>
public static class MemoryExtensions
{
    /// <summary>
    /// Gets the next value from <paramref name="enumerator"/> or throw.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The enumerator.</param>
    /// <returns>The next value.</returns>
    /// <exception cref="InvalidOperationException">No more values.</exception>
    public static ReadOnlySpan<char> GetNextOrThrow(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator) => enumerator.MoveNext()
        ? span[enumerator.Current]
        : throw new InvalidOperationException();

    /// <summary>
    /// Moves to the next value from <paramref name="enumerator"/> or throws.
    /// </summary>
    /// <param name="enumerator">The enumerator.</param>
    /// <exception cref="InvalidOperationException">No more values.</exception>
    public static void MoveNextOrThrow(this ref SpanSplitEnumerator<char> enumerator)
    {
        if (!enumerator.MoveNext())
        {
            throw new InvalidOperationException();
        }
    }

    /// <summary>
    /// Gets the next value from the span.
    /// </summary>
    /// <typeparam name="T">The type of value to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="parser">The parser for <typeparamref name="T"/>.</param>
    /// <returns>The result from <paramref name="parser"/> for the next value from <paramref name="span"/>.</returns>
    public static T GetNextValue<T>(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, Parse<T> parser) => parser(span.GetNextOrThrow(ref enumerator));

    /// <summary>
    /// Gets the next value from the span as the specified enum.
    /// </summary>
    /// <typeparam name="T">The type of enum to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <returns>The result from <see cref="Enum.Parse{T}(string)"/> for the next value from <paramref name="span"/>.</returns>
    public static T GetNextEnum<T>(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator)
        where T : struct, Enum =>
#if NET6_0_OR_GREATER
         Enum.Parse<T>(span.GetNextOrThrow(ref enumerator));
#else
         Enum.Parse<T>(span.GetNextOrThrow(ref enumerator).ToString());
#endif

    /// <summary>
    /// Gets the next value from the span as the specified enum.
    /// </summary>
    /// <typeparam name="T">The type of enum to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
    /// <returns>The result from <see cref="Enum.Parse{T}(string, bool)"/> for the next value from <paramref name="span"/>.</returns>
    public static T GetNextEnum<T>(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, bool ignoreCase)
        where T : struct, Enum =>
#if NET6_0_OR_GREATER
        Enum.Parse<T>(span.GetNextOrThrow(ref enumerator), ignoreCase);
#else
        Enum.Parse<T>(span.GetNextOrThrow(ref enumerator).ToString(), ignoreCase);
#endif

    /// <summary>
    /// Gets the next value from the span as a <see cref="double"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="double"/> value.</returns>
    public static double GetNextDouble(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => double.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="float"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="float"/> value.</returns>
    public static float GetNextSingle(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => float.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="short"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="short"/> value.</returns>
    public static short GetNextInt16(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => short.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="ushort"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="ushort"/> value.</returns>
    public static ushort GetNextUInt16(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => ushort.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="int"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="int"/> value.</returns>
    public static int GetNextInt32(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => int.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="uint"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="uint"/> value.</returns>
    public static uint GetNextUInt32(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => uint.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="long"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="long"/> value.</returns>
    public static long GetNextInt64(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => long.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="ulong"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="ulong"/> value.</returns>
    public static ulong GetNextUInt64(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => ulong.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="byte"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="byte"/> value.</returns>
    public static byte GetNextByte(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => byte.Parse(v, style: style, provider: provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="string"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <returns>The next value from the span as a <see cref="string"/> value.</returns>
    public static string GetNextString(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator) => span.GetNextValue(ref enumerator, v => v.ToString());

    /// <summary>
    /// Gets the value array from the <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of span.</typeparam>
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="parser">The parser.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>The value array.</returns>
    public static T[] GetValues<T>(this ReadOnlySpan<char> input, string separator, int count, Parse<char, T> parser, IFormatProvider? provider) => GetValues(input, input.Split(separator), count, parser, provider);

    /// <summary>
    /// Gets the value array from the <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of span.</typeparam>
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="parser">The parser.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>The value array.</returns>
    public static T[] GetValues<T>(this ReadOnlySpan<char> input, char separator, int count, Parse<char, T> parser, IFormatProvider? provider) => GetValues(input, input.Split(separator), count, parser, provider);

    /// <summary>
    /// Tries to get the next value from the span, with the result indicating success.
    /// </summary>
    /// <typeparam name="T">The type of value to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="parser">The parser for <typeparamref name="T"/>.</param>
    /// <param name="value">The result from <paramref name="parser"/> for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextValue<T>(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, TryParse<T> parser, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="value">The enum result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextEnum<T>(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, out T value)
        where T : struct, Enum
    {
        return span.TryGetNextValue(ref enumerator, TryParse, out value);

        static bool TryParse(ReadOnlySpan<char> span, out T value)
        {
#if NET6_0_OR_GREATER
            return Enum.TryParse(span, out value);
#else
            return Enum.TryParse(span.ToString(), out value);
#endif
        }
    }

    /// <summary>
    /// Tries to get the next value from the span as the specified enum, with the result indicating success.
    /// </summary>
    /// <typeparam name="T">The type of enum to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
    /// <param name="value">The enum result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextEnum<T>(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, bool ignoreCase, out T value)
        where T : struct, Enum
    {
        return span.TryGetNextValue(ref enumerator, TryParse, out value);

        bool TryParse(ReadOnlySpan<char> span, out T value)
        {
#if NET6_0_OR_GREATER
            return Enum.TryParse(span, ignoreCase, out value);
#else
            return Enum.TryParse(span.ToString(), ignoreCase, out value);
#endif
        }
    }

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="double"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextDouble(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out double value) => span.TryGetNextDouble(ref enumerator, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="double"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextDouble(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out double value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="float"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextSingle(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out float value) => span.TryGetNextSingle(ref enumerator, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="double"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextSingle(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out float value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="short"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt16(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out short value) => span.TryGetNextInt16(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="short"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="short"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt16(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out short value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="ushort"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextUInt16(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out ushort value) => span.TryGetNextUInt16(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="ushort"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="ushort"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextUInt16(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out ushort value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="int"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt32(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out int value) => span.TryGetNextInt32(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="int"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="int"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt32(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out int value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="uint"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextUInt32(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out uint value) => span.TryGetNextUInt32(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="uint"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="uint"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextUInt32(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out uint value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="long"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt64(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out long value) => span.TryGetNextInt64(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="long"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="long"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt64(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out long value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="ulong"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextUInt64(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out ulong value) => span.TryGetNextUInt64(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="ulong"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="ulong"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextUInt64(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out ulong value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="byte"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextByte(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out byte value) => TryGetNextByte(span, ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="byte"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="byte"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextByte(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out byte value)
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
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="value">The <see cref="string"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextString(this ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string? value)
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
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="parser">The parser.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <param name="output">The output values.</param>
    /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
    public static bool TryGetValues<T>(
        this ReadOnlySpan<char> input,
        string separator,
        int count,
        TryParse<char, T> parser,
        IFormatProvider? provider,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T[]? output) => TryGetValues(input, input.Split(separator), count, parser, provider, out output);

    /// <summary>
    /// Tries to get the value array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
    /// </summary>
    /// <typeparam name="T">The type of span.</typeparam>
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="parser">The parser.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <param name="output">The output values.</param>
    /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
    public static bool TryGetValues<T>(
        this ReadOnlySpan<char> input,
        char separator,
        int count,
        TryParse<char, T> parser,
        IFormatProvider? provider,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T[]? output) => TryGetValues(input, input.Split(separator), count, parser, provider, out output);

    /// <summary>
    /// Gets the value double array from the <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>The value array.</returns>
    public static double[] GetDoubleValues(this ReadOnlySpan<char> input, string separator, int count, IFormatProvider? provider)
    {
        return GetValues(input, separator, count, Parse, provider);

        static double Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
        {
            return double.Parse(input, provider: provider);
        }
    }

    /// <summary>
    /// Gets the value double array from the <see cref="ReadOnlySpan{T}"/>.
    /// </summary>
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <returns>The value array.</returns>
    public static double[] GetDoubleValues(this ReadOnlySpan<char> input, char separator, int count, IFormatProvider? provider)
    {
        return GetValues(input, separator, count, Parse, provider);

        static double Parse(ReadOnlySpan<char> input, IFormatProvider? provider)
        {
            return double.Parse(input, provider: provider);
        }
    }

    /// <summary>
    /// Tries to get the value double array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
    /// </summary>
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <param name="output">The output values.</param>
    /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
    public static bool TryGetDoubleValues(
        this ReadOnlySpan<char> input,
        string separator,
        int count,
        IFormatProvider? provider,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out double[]? output)
    {
        return TryGetValues(input, separator, count, TryParse, provider, out output);

        static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? provider, out double output)
        {
            return double.TryParse(input, System.Globalization.NumberStyles.Float, provider, out output);
        }
    }

    /// <summary>
    /// Tries to get the value double array from the <see cref="ReadOnlySpan{T}"/> and returns whether it was successful.
    /// </summary>
    /// <param name="input">The input span.</param>
    /// <param name="separator">The separator.</param>
    /// <param name="count">The number of elements to parse.</param>
    /// <param name="provider">An <see cref="IFormatProvider"/> that supplies culture-specific formatting information.</param>
    /// <param name="output">The output values.</param>
    /// <returns><see langword="true"/> if the values were obtained; otherwise <see langword="false"/>.</returns>
    public static bool TryGetDoubleValues(
        this ReadOnlySpan<char> input,
        char separator,
        int count,
        IFormatProvider? provider,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out double[]? output)
    {
        return TryGetValues(input, separator, count, TryParse, provider, out output);

        static bool TryParse(ReadOnlySpan<char> input, IFormatProvider? provider, out double output)
        {
            return double.TryParse(input, System.Globalization.NumberStyles.Float, provider, out output);
        }
    }

    private static TOutput[] GetValues<TInput, TOutput>(
        ReadOnlySpan<TInput> input,
        SpanSplitEnumerator<TInput> enumerator,
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

        if (index != count)
        {
            // not enough items
            throw new FormatException(Properties.Resources.IncorrectInputString);
        }

        return list;
    }

    private static bool TryGetValues<TInput, TOutput>(
        ReadOnlySpan<TInput> input,
        SpanSplitEnumerator<TInput> enumerator,
        int count,
        TryParse<TInput, TOutput> parser,
        IFormatProvider? provider,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out TOutput[]? output)
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