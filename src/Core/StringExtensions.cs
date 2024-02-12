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
    /// Splits a string into substrings based on specified delimiting characters, ignoring separators in quoted areas.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="separator">An array of delimiting characters, an empty array that contains no delimiters, or <see langword="null"/>.</param>
    /// <returns>An array whose elements contain the substrings from <paramref name="s"/> that are delimited by <paramref name="separator"/>.</returns>
    public static string[] SplitQuoted(this string s, params char[]? separator) => SplitQuotedInternal(s, separator, StringSplitOptions.None);

    /// <summary>
    /// Splits a string into substrings based on a specified delimiting character, ignoring separators in quoted areas.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="separator">A character that delimits the substrings in <paramref name="s"/>.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    /// <returns>An array whose elements contain the substrings from <paramref name="s"/> that are delimited by <paramref name="separator"/>.</returns>
    public static string[]? SplitQuoted(this string s, char separator, StringSplitOptions options = StringSplitOptions.None) => SplitQuotedInternal(s, new[] { separator }, options);

    /// <summary>
    /// Splits a string into substrings based on specified delimiting characters, ignoring separators in quoted areas.
    /// </summary>
    /// <param name="s">The string to split.</param>
    /// <param name="separator">An array of delimiting characters, an empty array that contains no delimiters, or <see langword="null"/>.</param>
    /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
    /// <returns>An array whose elements contain the substrings from <paramref name="s"/> that are delimited by <paramref name="separator"/>.</returns>
    public static string[] SplitQuoted(this string s, char[]? separator, StringSplitOptions options = StringSplitOptions.None) => SplitQuotedInternal(s, separator, options);

    /// <summary>
    /// Quotes the specified string.
    /// </summary>
    /// <param name="s">The string to quote.</param>
    /// <param name="options">The options to use to quote the string.</param>
    /// <returns>The quoted string.</returns>
    public static string Quote(this string? s, StringQuoteOptions options = StringQuoteOptions.None) => Quote(s, [], options);

    /// <summary>
    /// Quotes the specified string.
    /// </summary>
    /// <param name="s">The string to quote.</param>
    /// <param name="delimiter">The delimiter to quote.</param>
    /// <param name="options">The options to use to quote the string.</param>
    /// <returns>The quoted string.</returns>
    public static string Quote(this string? s, char delimiter, StringQuoteOptions options = StringQuoteOptions.QuoteAll) => Quote(s, [delimiter], options);

    /// <summary>
    /// Quotes the specified string.
    /// </summary>
    /// <param name="s">The string to quote.</param>
    /// <param name="delimiter">The delimiter to quote.</param>
    /// <param name="options">The options to use to quote the string.</param>
    /// <returns>The quoted string.</returns>
    public static string Quote(this string? s, char[] delimiter, StringQuoteOptions options = StringQuoteOptions.QuoteAll) => s switch
    {
        // quote an empty string to differentiate it from a null string
        { Length: 0 } => string.Concat(QuoteString, QuoteString),
        string value => SmartQuote(value, delimiter, options),
        _ => string.Empty,
    };

    /// <summary>
    /// Formats the specified <see cref="IFormattable"/> instance using the specified <see cref="IFormatProvider"/>.
    /// </summary>
    /// <param name="formattable">The object that can be formatted.</param>
    /// <param name="provider">The provider to use to format the value.</param>
    /// <returns>The value of <paramref name="formattable"/> using <paramref name="provider"/>.</returns>
    public static string? ToString(this IFormattable formattable, IFormatProvider? provider) => formattable.ToString(format: default, provider);

    private static string[] SplitQuotedInternal(string s, IList<char>? separator, StringSplitOptions options)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(s);
        if (s.Length == 0)
        {
            return options.HasFlag(StringSplitOptions.RemoveEmptyEntries) ? [] : [s];
        }

        var splitOnWhiteSpace = separator is null || separator.Count == 0;
#if NET5_0_OR_GREATER
        if (splitOnWhiteSpace)
        {
            options &= ~StringSplitOptions.TrimEntries;
        }
#endif

        // work through each one
        var splits = new List<int>();
        var combining = false;
        for (var i = 0; i < s.Length; i++)
        {
            var isSplit = splitOnWhiteSpace ? char.IsWhiteSpace(s[i]) : Contains(separator!, s[i]);
            if (isSplit)
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
                if (options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
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

#if NET5_0_OR_GREATER
            if (options.HasFlag(StringSplitOptions.TrimEntries))
            {
                TrimStartEnd(s, ref startIndex, ref endIndex);
            }
#endif

            values[returnIndex] = s[startIndex..endIndex];
            lastIndex = index + 1;
            returnIndex++;
        }

        if (!options.HasFlag(StringSplitOptions.RemoveEmptyEntries) || s.Length - lastIndex != 0)
        {
            var startIndex = lastIndex;
            var endIndex = s.Length;
#if NET5_0_OR_GREATER
            if (options.HasFlag(StringSplitOptions.TrimEntries))
            {
                TrimStartEnd(s, ref startIndex, ref endIndex);
            }
#endif

            values[returnIndex] = s[startIndex..endIndex];
            returnIndex++;
        }

        // resize the array
        if (values.Length != returnIndex)
        {
            System.Array.Resize(ref values, returnIndex);
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

#if NET5_0_OR_GREATER
        static void TrimStartEnd(string s, ref int startIndex, ref int endIndex)
        {
            while (char.IsWhiteSpace(s[startIndex]) && startIndex < endIndex)
            {
                startIndex++;
            }

            while (char.IsWhiteSpace(s[endIndex - 1]) && startIndex < endIndex)
            {
                endIndex--;
            }
        }
#endif
    }

    private static string SmartQuote(string value, IList<char> delimiter, StringQuoteOptions options)
    {
        var quoteDelimiter = ShouldQuoteDelimiter(delimiter);
        var quoteNewLine = options.HasFlag(StringQuoteOptions.QuoteNewLine);
        var quoteQuotes = options.HasFlag(StringQuoteOptions.QuoteQuotes);
        var quoteNonAscii = options.HasFlag(StringQuoteOptions.QuoteNonAscii);

        if (!quoteDelimiter && !quoteNewLine && !quoteQuotes && !quoteNonAscii)
        {
            // we have no delimiter, and are not checking anything else
            return QuoteReturnString(value);
        }

        // see if we should quote or not
        for (var i = 0; i < value.Length; i++)
        {
            var character = value[i];
            if ((quoteDelimiter && delimiter[0] == character && CheckDelimiter(value, i, delimiter))
                || (quoteNewLine && character is '\r' or '\n')
                || (quoteQuotes && character is QuoteChar)
                || (quoteNonAscii && character >= 128))
            {
                return QuoteReturnString(value);
            }
        }

        return value;

        static bool ShouldQuoteDelimiter([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] IList<char>? delimiter)
        {
            return delimiter?.Count > 0;
        }

        static bool CheckDelimiter(string s, int index, IList<char> delimiter)
        {
            if (delimiter.Count > s.Length - index)
            {
                return false;
            }

            for (var i = 0; i < delimiter.Count; i++)
            {
                if (s[index + i] != delimiter[i])
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