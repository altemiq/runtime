// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// <see cref="string"/> extensions.
/// </summary>
public static class StringExtensions
{
    private const char QuoteChar = '\"';

    private const string QuoteString = "\"";

    private const string EscapedQuoteString = QuoteString + QuoteString;

    /// <summary>
    /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="separator">A character that delimits the substrings in <paramref name="s"/>.</param>
    /// <param name="options"><see cref="StringSplitOptions"/>.<see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions"/>.<see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
    /// <returns>An array whose elements contain the substrings from <paramref name="s"/> that are delimited by <paramref name="separator"/>.</returns>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(s))]
    public static string[]? SplitQuoted(this string s, char separator, StringSplitOptions options = StringSplitOptions.None) => SplitQuoted(s, new[] { separator }, options);

    /// <summary>
    /// Splits a string into substrings that are based on the specified separator, ignoring separators in quoted areas.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="separator">A character that delimits the substrings in <paramref name="s"/>.</param>
    /// <param name="options"><see cref="StringSplitOptions"/>.<see cref="StringSplitOptions.RemoveEmptyEntries"/> to omit empty array elements from the array returned; or <see cref="StringSplitOptions"/>.<see cref="StringSplitOptions.None"/> to include empty array elements in the array returned.</param>
    /// <returns>An array whose elements contain the substrings from <paramref name="s"/> that are delimited by <paramref name="separator"/>.</returns>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(s))]
    public static string[] SplitQuoted(this string s, char[] separator, StringSplitOptions options = StringSplitOptions.None)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(s);
        if (s.Length == 0)
        {
#if NETSTANDARD1_3_OR_GREATER || NET46_OR_GREATER || NETCOREAPP1_0_OR_GREATER
            return Array.Empty<string>();
#else
            return new string[0];
#endif
        }

        // work through each one
        var splits = new List<int>();
        var combining = false;
        for (var i = 0; i < s.Length; i++)
        {
            if ((separator is null && char.IsWhiteSpace(s[i])) || Contains(separator!, s[i]))
            {
                if (!combining)
                {
                    splits.Add(i);
                }
            }
            else if (s[i] == QuoteChar)
            {
                combining = !combining;
            }
        }

        var values = new string[splits.Count + 1];
        var returnIndex = 0;
        var lastIndex = 0;

        foreach (var index in splits)
        {
            if (index == lastIndex)
            {
                if (options == StringSplitOptions.RemoveEmptyEntries)
                {
                    lastIndex++;
                }
                else
                {
                    returnIndex++;
                }

                continue;
            }

            var startIndex = lastIndex;
            if (s[startIndex] == QuoteChar)
            {
                startIndex++;
            }

            var endIndex = index;
            if (s[endIndex - 1] == QuoteChar)
            {
                endIndex--;
            }

            values[returnIndex] = s[startIndex..endIndex];
            lastIndex = index + 1;
            returnIndex++;
        }

        if (options != StringSplitOptions.RemoveEmptyEntries || s.Length - lastIndex != 0)
        {
            values[returnIndex] = s[lastIndex..];
            returnIndex++;
        }

        // resize the array
        if (values.Length != returnIndex)
        {
            Array.Resize(ref values, returnIndex);
        }

        return values;

        static bool Contains(IList<char> characters, char character)
        {
            for (var i = 0; i < characters.Count; i++)
            {
                if (characters[i] == character)
                {
                    return true;
                }
            }

            return false;
        }
    }

    /// <summary>
    /// Quotes the specified string.
    /// </summary>
    /// <param name="s">The string to quote.</param>
    /// <param name="options">The options to use to quote the string.</param>
    /// <returns>The quoted string.</returns>
    public static string Quote(this string? s, StringQuoteOptions options = StringQuoteOptions.None) => Quote(
        s,
#if NETSTANDARD1_3_OR_GREATER || NETCOREAPP || NET46_OR_GREATER
        Array.Empty<char>(),
#else
        new char[0],
#endif
        options);

    /// <summary>
    /// Quotes the specified string.
    /// </summary>
    /// <param name="s">The string to quote.</param>
    /// <param name="delimeter">The delimeter to quote.</param>
    /// <param name="options">The options to use to quote the string.</param>
    /// <returns>The quoted string.</returns>
    public static string Quote(this string? s, char delimeter, StringQuoteOptions options = StringQuoteOptions.QuoteAll) => Quote(s, new[] { delimeter }, options);

    /// <summary>
    /// Quotes the specified string.
    /// </summary>
    /// <param name="s">The string to quote.</param>
    /// <param name="delimeter">The delimeter to quote.</param>
    /// <param name="options">The options to use to quote the string.</param>
    /// <returns>The quoted string.</returns>
    public static string Quote(this string? s, char[] delimeter, StringQuoteOptions options = StringQuoteOptions.QuoteAll) => s switch
    {
        // quote an empty string to differentiate it from a null string
        { Length: 0 } => string.Concat(QuoteString, QuoteString),
        string value => SmartQuote(value, delimeter, options),
        _ => string.Empty,
    };

    /// <summary>
    /// Formats the specified <see cref="IFormattable"/> instance using the specified <see cref="IFormatProvider"/>.
    /// </summary>
    /// <param name="formattable">The object that can be formatted.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>The value of <paramref name="formattable"/> using <paramref name="provider"/>.</returns>
    public static string? ToString(this IFormattable formattable, IFormatProvider? provider) => formattable.ToString(format: default, provider);

    private static string SmartQuote(string value, IList<char> delimeter, StringQuoteOptions options)
    {
        var quoteDelimeter = ShouldQuoteDelimeter(delimeter);
        var quoteNewLine = options.HasFlag(StringQuoteOptions.QuoteNewLine);
        var quoteQuotes = options.HasFlag(StringQuoteOptions.QuoteQuotes);
        var quoteNonAscii = options.HasFlag(StringQuoteOptions.QuoteNonAscii);

        if (!quoteDelimeter && !quoteNewLine && !quoteQuotes && !quoteNonAscii)
        {
            // we have no delimeter, and are not checking anything else
            return QuoteReturnString(value);
        }

        // see if we should quote or not
        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];
            if ((quoteDelimeter && delimeter[0] == character && CheckDelimeter(value, i, delimeter))
                || (quoteNewLine && character is '\r' or '\n')
                || (quoteQuotes && character is QuoteChar)
                || (quoteNonAscii && character >= 128))
            {
                return QuoteReturnString(value);
            }
        }

        return value;

        static bool ShouldQuoteDelimeter([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] IList<char>? delimeter)
        {
            return delimeter?.Count > 0;
        }

        static bool CheckDelimeter(string s, int index, IList<char> delimeter)
        {
            if (delimeter.Count > s.Length - index)
            {
                return false;
            }

            for (var i = 0; i < delimeter.Count; i++)
            {
                if (s[index + i] != delimeter[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    private static string QuoteReturnString(string value) =>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
         QuoteString + value.Replace(QuoteString, EscapedQuoteString, StringComparison.Ordinal) + QuoteString;
#else
         QuoteString + value.Replace(QuoteString, EscapedQuoteString) + QuoteString;
#endif
}