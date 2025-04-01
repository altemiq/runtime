// -----------------------------------------------------------------------
// <copyright file="MemoryExtensions.NET9.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <content>
/// .NET 9.0 extensions.
/// </content>
public static partial class MemoryExtensions
{
    /// <summary>
    /// Gets the next value from <paramref name="enumerator"/> or throw.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The enumerator.</param>
    /// <returns>The next value.</returns>
    /// <exception cref="InvalidOperationException">No more values.</exception>
    public static ReadOnlySpan<char> GetNextOrThrow(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator) => enumerator.MoveNext()
        ? span[enumerator.Current]
        : throw new InvalidOperationException();

    /// <summary>
    /// Moves to the next value from <paramref name="enumerator"/> or throws.
    /// </summary>
    /// <param name="enumerator">The enumerator.</param>
    /// <exception cref="InvalidOperationException">No more values.</exception>
    public static void MoveNextOrThrow(this ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator)
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
    public static T GetNextValue<T>(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, Parse<T> parser) => parser(span.GetNextOrThrow(ref enumerator));

    /// <summary>
    /// Gets the next value from the span.
    /// </summary>
    /// <typeparam name="T">The type of value to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="parser">The parser for <typeparamref name="T"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The result from <paramref name="parser"/> for the next value from <paramref name="span"/>.</returns>
    public static T GetNextValue<T>(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, Parse<char, T> parser, IFormatProvider? provider) => parser(span.GetNextOrThrow(ref enumerator), provider);

    /// <summary>
    /// Gets the next value from the span as the specified enum.
    /// </summary>
    /// <typeparam name="T">The type of enum to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <returns>The result from <see cref="Enum.Parse{T}(string)"/> for the next value from <paramref name="span"/>.</returns>
    public static T GetNextEnum<T>(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator)
        where T : struct, Enum => Enum.Parse<T>(span.GetNextOrThrow(ref enumerator));

    /// <summary>
    /// Gets the next value from the span as the specified enum.
    /// </summary>
    /// <typeparam name="T">The type of enum to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="ignoreCase"><see langword="true"/> to ignore case; <see langword="false"/> to regard case.</param>
    /// <returns>The result from <see cref="Enum.Parse{T}(string, bool)"/> for the next value from <paramref name="span"/>.</returns>
    public static T GetNextEnum<T>(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, bool ignoreCase)
        where T : struct, Enum => Enum.Parse<T>(span.GetNextOrThrow(ref enumerator), ignoreCase);

    /// <summary>
    /// Gets the next value from the span as a <see cref="double"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="double"/> value.</returns>
    public static double GetNextDouble(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => double.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="float"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="float"/> value.</returns>
    public static float GetNextSingle(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => float.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="short"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="short"/> value.</returns>
    public static short GetNextInt16(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => short.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="ushort"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="ushort"/> value.</returns>
    [CLSCompliant(false)]
    public static ushort GetNextUInt16(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => ushort.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="int"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="int"/> value.</returns>
    public static int GetNextInt32(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => int.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="uint"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="uint"/> value.</returns>
    [CLSCompliant(false)]
    public static uint GetNextUInt32(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => uint.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="long"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="long"/> value.</returns>
    public static long GetNextInt64(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => long.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="ulong"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="ulong"/> value.</returns>
    [CLSCompliant(false)]
    public static ulong GetNextUInt64(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => ulong.Parse(v, style, provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="byte"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <returns>The next value from the span as a <see cref="byte"/> value.</returns>
    public static byte GetNextByte(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style = System.Globalization.NumberStyles.Integer, IFormatProvider? provider = null) => span.GetNextValue(ref enumerator, v => byte.Parse(v, style: style, provider: provider));

    /// <summary>
    /// Gets the next value from the span as a <see cref="string"/> value.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <returns>The next value from the span as a <see cref="string"/> value.</returns>
    public static string GetNextString(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator) => span.GetNextValue(ref enumerator, static v => v.ToString());

    /// <summary>
    /// Tries to get the next value from the span, with the result indicating success.
    /// </summary>
    /// <typeparam name="T">The type of value to get.</typeparam>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="parser">The parser for <typeparamref name="T"/>.</param>
    /// <param name="value">The result from <paramref name="parser"/> for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextValue<T>(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, TryParse<T> parser, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out T? value)
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
    public static bool TryGetNextEnum<T>(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, out T value)
        where T : struct, Enum
    {
        return span.TryGetNextValue(ref enumerator, TryParse, out value);

        static bool TryParse(ReadOnlySpan<char> span, out T value)
        {
            return Enum.TryParse(span, ignoreCase: false, out value);
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
    public static bool TryGetNextEnum<T>(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, bool ignoreCase, out T value)
        where T : struct, Enum
    {
        return span.TryGetNextValue(ref enumerator, TryParse, out value);

        bool TryParse(ReadOnlySpan<char> input, out T output)
        {
            return Enum.TryParse(input, ignoreCase, out output);
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
    public static bool TryGetNextDouble(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out double value) => span.TryGetNextDouble(ref enumerator, System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="double"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextDouble(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out double value)
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
    public static bool TryGetNextSingle(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out float value) => span.TryGetNextSingle(ref enumerator, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="double"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="double"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextSingle(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out float value)
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
    public static bool TryGetNextInt16(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out short value) => span.TryGetNextInt16(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="short"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="short"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt16(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out short value)
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
    [CLSCompliant(false)]
    public static bool TryGetNextUInt16(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out ushort value) => span.TryGetNextUInt16(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="ushort"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="ushort"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    [CLSCompliant(false)]
    public static bool TryGetNextUInt16(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out ushort value)
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
    public static bool TryGetNextInt32(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out int value) => span.TryGetNextInt32(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="int"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="int"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt32(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out int value)
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
    [CLSCompliant(false)]
    public static bool TryGetNextUInt32(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out uint value) => span.TryGetNextUInt32(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="uint"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="uint"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    [CLSCompliant(false)]
    public static bool TryGetNextUInt32(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out uint value)
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
    public static bool TryGetNextInt64(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out long value) => span.TryGetNextInt64(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="long"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="long"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextInt64(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out long value)
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
    [CLSCompliant(false)]
    public static bool TryGetNextUInt64(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out ulong value) => span.TryGetNextUInt64(ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="ulong"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="ulong"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    [CLSCompliant(false)]
    public static bool TryGetNextUInt64(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out ulong value)
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
    public static bool TryGetNextByte(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out byte value) => TryGetNextByte(span, ref enumerator, System.Globalization.NumberStyles.Integer, provider, out value);

    /// <summary>
    /// Tries to get the next value from the span as a <see cref="byte"/>, with the result indicating success.
    /// </summary>
    /// <param name="span">The span.</param>
    /// <param name="enumerator">The span enumerator.</param>
    /// <param name="style">A bitwise combination of enumeration values that indicates the style elements that can be present in <paramref name="span"/>.</param>
    /// <param name="provider">An object that supplies culture-specific information about the format of <paramref name="span"/>. If provider is <see langword="null"/>, the thread current culture is used.</param>
    /// <param name="value">The <see cref="byte"/> result for the next value from <paramref name="span"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="value"/> was successfully parsed from <paramref name="span"/>; otherwise <see langword="false"/>.</returns>
    public static bool TryGetNextByte(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider, out byte value)
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
    public static bool TryGetNextString(this ReadOnlySpan<char> span, ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out string? value)
    {
        return span.TryGetNextValue(ref enumerator, TryParse, out value);

        static bool TryParse(ReadOnlySpan<char> v, out string value)
        {
            value = v.ToString();
            return true;
        }
    }
}