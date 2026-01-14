// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

#pragma warning disable CA1708, RCS1263, SA1101, S2325

/// <summary>
/// <see cref="string"/> extensions.
/// </summary>
public static class StringExtensions
{
    /// <content>
    /// <see cref="string"/> extensions.
    /// </content>
    /// <param name="s">The string to split.</param>
    extension(string s)
    {
        /// <summary>
        /// Splits a string into substrings based on specified delimiting characters, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">An array of delimiting characters, an empty array that contains no delimiters, or <see langword="null"/>.</param>
        /// <returns>An array whose elements contain the substrings from this instance that are delimited by <paramref name="separator"/>.</returns>
        public string[] SplitQuoted(params char[]? separator) => string.SplitQuotedInternal(s, separator, StringSplitOptions.None);

        /// <summary>
        /// Splits a string into substrings based on a specified delimiting character, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">A character that delimits the substrings.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>An array whose elements contain the substrings from this instance that are delimited by <paramref name="separator"/>.</returns>
        public string[] SplitQuoted(char separator, StringSplitOptions options = StringSplitOptions.None) => string.SplitQuotedInternal(s, [separator], options);

        /// <summary>
        /// Splits a string into substrings based on specified delimiting characters, ignoring separators in quoted areas.
        /// </summary>
        /// <param name="separator">An array of delimiting characters, an empty array that contains no delimiters, or <see langword="null"/>.</param>
        /// <param name="options">A bitwise combination of the enumeration values that specifies whether to trim substrings and include empty substrings.</param>
        /// <returns>An array whose elements contain the substrings from this instance that are delimited by <paramref name="separator"/>.</returns>
        public string[] SplitQuoted(char[]? separator, StringSplitOptions options = StringSplitOptions.None) => string.SplitQuotedInternal(s, separator, options);

        /// <summary>
        /// Quotes the specified string.
        /// </summary>
        /// <param name="options">The options to use to quote the string.</param>
        /// <returns>The quoted string.</returns>
        public string Quote(StringQuoteOptions options = StringQuoteOptions.None) => s.Quote([], options);

        /// <summary>
        /// Quotes the specified string.
        /// </summary>
        /// <param name="delimiter">The delimiter to quote.</param>
        /// <param name="options">The options to use to quote the string.</param>
        /// <returns>The quoted string.</returns>
        public string Quote(char delimiter, StringQuoteOptions options = StringQuoteOptions.QuoteAll) => s.Quote([delimiter], options);

        /// <summary>
        /// Quotes the specified string.
        /// </summary>
        /// <param name="delimiter">The delimiter to quote.</param>
        /// <param name="options">The options to use to quote the string.</param>
        /// <returns>The quoted string.</returns>
        public string Quote(char[] delimiter, StringQuoteOptions options = StringQuoteOptions.QuoteAll) => s switch
        {
            // quote an empty string to differentiate it from a null string
            { Length: 0 } => string.Concat(QuoteString, QuoteString),
            { } value => string.SmartQuote(value, delimiter, options),
            _ => string.Empty,
        };

        private static string[] SplitQuotedInternal(string str, IList<char>? separator, StringSplitOptions options)
        {
            ArgumentNullException.ThrowIfNull(str);
            if (str.Length is 0)
            {
                return options.HasFlag(StringSplitOptions.RemoveEmptyEntries) ? [] : [str];
            }

            var splitOnWhiteSpace = separator is not { Count: not 0 };
#if NET5_0_OR_GREATER
            if (splitOnWhiteSpace)
            {
                options &= ~StringSplitOptions.TrimEntries;
            }
#endif

            // work through each one
            var splits = Split(str, separator, splitOnWhiteSpace);

            return ProcessSplits(str, options, splits);

            static List<int> Split(string s, IList<char>? separator, bool splitOnWhiteSpace)
            {
                var splits = new List<int>();
                var combining = false;
                for (var i = 0; i < s.Length; i++)
                {
                    if (splitOnWhiteSpace ? char.IsWhiteSpace(s[i]) : Contains(separator!, s[i]))
                    {
                        if (!combining)
                        {
                            splits.Add(i);
                        }
                    }
                    else if (s[i] is QuoteChar)
                    {
                        combining = !combining;
                    }
                }

                return splits;

                static bool Contains(IList<char> characters, char character)
                {
                    return characters.Any(@char => @char == character);
                }
            }

            static string[] ProcessSplits(string s, StringSplitOptions options, List<int> splits)
            {
                var values = new string[splits.Count + 1];
                var returnIndex = 0;
                var lastIndex = 0;

                foreach (var index in splits)
                {
                    ProcessSplit(values, s, index, options, ref lastIndex, ref returnIndex);
                }

                if (!options.HasFlag(StringSplitOptions.RemoveEmptyEntries) || (s.Length - lastIndex) is not 0)
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
                    Array.Resize(ref values, returnIndex);
                }

                return values;

                static void ProcessSplit(string[] values, string s, int index, StringSplitOptions options, ref int lastIndex, ref int returnIndex)
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

                        return;
                    }

                    var startIndex = lastIndex;
                    if (s[startIndex] is QuoteChar)
                    {
                        startIndex++;
                    }

                    var endIndex = index;
                    if (s[endIndex - 1] is QuoteChar)
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

#if NET5_0_OR_GREATER
                static void TrimStartEnd(string str, ref int startIndex, ref int endIndex)
                {
                    while (char.IsWhiteSpace(str[startIndex]) && startIndex < endIndex)
                    {
                        startIndex++;
                    }

                    while (char.IsWhiteSpace(str[endIndex - 1]) && startIndex < endIndex)
                    {
                        endIndex--;
                    }
                }
#endif
            }
        }

        private static string SmartQuote(string value, IList<char> delimiter, StringQuoteOptions options)
        {
            var quoteDelimiter = ShouldQuoteDelimiter(delimiter);
            var quoteNewLine = options.HasFlag(StringQuoteOptions.QuoteNewLine);
            var quoteQuotes = options.HasFlag(StringQuoteOptions.QuoteQuotes);
            var quoteNonAscii = options.HasFlag(StringQuoteOptions.QuoteNonAscii);

            // see if we should quote or not
            return !HaveOptions() || Enumerable.Range(0, value.Length).Any(IsQuotableAt)
                ? string.QuoteReturnString(value)
                : value;

            bool HaveOptions()
            {
                return quoteDelimiter || quoteNewLine || quoteQuotes || quoteNonAscii;
            }

            bool IsQuotableAt(int i)
            {
                var character = value[i];
                return (quoteDelimiter && IsDelimiter(character, value, i, delimiter))
                       || (quoteNewLine && IsNewLine(character))
                       || (quoteQuotes && IsQuote(character))
                       || (quoteNonAscii && IsNonAscii(character));

                static bool IsDelimiter(char character, string s, int index, IList<char> delimiter)
                {
                    return delimiter[0] == character && CheckDelimiter(s, index, delimiter);

                    static bool CheckDelimiter(string s, int index, IList<char> delimiter)
                    {
                        if (delimiter.Count > s.Length - index)
                        {
                            return false;
                        }

                        return !delimiter.Where((t, i) => s[index + i] != t).Any();
                    }
                }

                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                static bool IsNewLine(char character)
                {
                    return character is '\r' or '\n';
                }

                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                static bool IsQuote(char character)
                {
                    return character is QuoteChar;
                }

                [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                static bool IsNonAscii(char character)
                {
                    return character >= 128;
                }
            }

            static bool ShouldQuoteDelimiter([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] IList<char>? delimiter)
            {
                return delimiter?.Count > 0;
            }
        }

        private static string QuoteReturnString(string value) => QuoteString + value.Replace(QuoteString, EscapedQuoteString, StringComparison.Ordinal) + QuoteString;
    }

    /// <content>
    /// The <see cref="IFormattable"/> extensions.
    /// </content>
    /// <param name="formattable">The object that can be formatted.</param>
    extension(IFormattable formattable)
    {
        /// <summary>
        /// Formats the specified <see cref="IFormattable"/> instance using the specified <see cref="IFormatProvider"/>.
        /// </summary>
        /// <param name="provider">The provider to use to format the value.</param>
        /// <returns>The string value of this instance using <paramref name="provider"/>.</returns>
        public string ToString(IFormatProvider? provider) => formattable.ToString(format: default, provider);
    }

    private const char QuoteChar = '\"';

    private const string QuoteString = "\"";

    private const string EscapedQuoteString = QuoteString + QuoteString;
}