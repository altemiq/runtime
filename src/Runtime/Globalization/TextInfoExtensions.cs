// -----------------------------------------------------------------------
// <copyright file="TextInfoExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Globalization;

/// <summary>
/// <see cref="System.Globalization.TextInfo"/> extensions.
/// </summary>
public static class TextInfoExtensions
{
    /// <summary>
    /// Converts the specified string to camel case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="str">The string to convert to camel case.</param>
    /// <returns>The specified string converted to camel case.</returns>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(str))]
    public static string? ToCamelCase(this System.Globalization.TextInfo textInfo, string? str)
    {
        return str switch
        {
            null => default,
            { Length: 0 } => string.Empty,
            _ => ToCamelCaseCore(textInfo, str),
        };

        static string ToCamelCaseCore(System.Globalization.TextInfo textInfo, ReadOnlySpan<char> source)
        {
            var destination = System.Buffers.ArrayPool<char>.Shared.Rent(source.Length);
            var used = textInfo.ToCamelCase(source, destination);
            var returnString = destination.AsSpan(0, used).AsString();
            System.Buffers.ArrayPool<char>.Shared.Return(destination);
            return returnString;
        }
    }

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to camel case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    public static int ToCamelCase(this System.Globalization.TextInfo textInfo, ReadOnlySpan<char> source, Span<char> destination) => ToCamelPascalCase(source, destination, textInfo.ToLower, textInfo.ToUpper);

    /// <summary>
    /// Converts the specified string to pascal case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="str">The string to convert to pascal case.</param>
    /// <returns>The specified string converted to pascal case.</returns>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(str))]
    public static string? ToPascalCase(this System.Globalization.TextInfo textInfo, string? str)
    {
        return str switch
        {
            null => default,
            { Length: 0 } => string.Empty,
            _ => ToPascalCaseCore(textInfo, str),
        };

        static string ToPascalCaseCore(System.Globalization.TextInfo textInfo, ReadOnlySpan<char> source)
        {
            var destination = System.Buffers.ArrayPool<char>.Shared.Rent(source.Length);
            var used = textInfo.ToPascalCase(source, destination);
            var returnString = destination.AsSpan(0, used).AsString();
            System.Buffers.ArrayPool<char>.Shared.Return(destination);
            return returnString;
        }
    }

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to pascal case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    public static int ToPascalCase(this System.Globalization.TextInfo textInfo, ReadOnlySpan<char> source, Span<char> destination) => ToCamelPascalCase(source, destination, textInfo.ToUpper, textInfo.ToUpper);

    /// <summary>
    /// Converts the specified string to kebab case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="str">The string to convert to kebab case.</param>
    /// <returns>The specified string converted to kebab case.</returns>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(str))]
    public static string? ToKebabCase(this System.Globalization.TextInfo textInfo, string? str) => InsertAndConvert(str, '-', textInfo.ToLower);

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to kebab case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    public static int ToKebabCase(this System.Globalization.TextInfo textInfo, ReadOnlySpan<char> source, Span<char> destination) => InsertAndConvert(source, destination, '-', textInfo.ToLower);

    /// <summary>
    /// Converts the specified string to snake case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="str">The string to convert to snake case.</param>
    /// <returns>The specified string converted to snake case.</returns>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(str))]
    public static string? ToSnakeCase(this System.Globalization.TextInfo textInfo, string? str) => InsertAndConvert(str, '_', textInfo.ToLower);

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to snake case.
    /// </summary>
    /// <param name="textInfo">The text info.</param>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    public static int ToSnakeCase(this System.Globalization.TextInfo textInfo, ReadOnlySpan<char> source, Span<char> destination) => InsertAndConvert(source, destination, '_', textInfo.ToLower);

    private static string? InsertAndConvert(string? str, char separator, Func<char, char> convert)
    {
        if (str is null)
        {
            return default;
        }

        ReadOnlySpan<char> source = str.AsSpan();
        var destination = System.Buffers.ArrayPool<char>.Shared.Rent(source.Length * 2);
        var used = InsertAndConvert(source, destination, separator, convert);
        var returnString = destination.AsSpan(0, used).AsString();
        System.Buffers.ArrayPool<char>.Shared.Return(destination);
        return returnString;
    }

    private static int InsertAndConvert(ReadOnlySpan<char> source, Span<char> destination, char separator, Func<char, char> convert)
    {
        var current = 0;
        var previousWhiteSpaceOrPunctuation = false;
        var previousUpper = false;
        foreach (var chr in source)
        {
            var whiteSpaceOrPunctuation = char.IsWhiteSpace(chr) || char.IsPunctuation(chr);
            if (!whiteSpaceOrPunctuation)
            {
                var isUpper = char.IsUpper(chr);
                if (isUpper && !previousUpper && !previousWhiteSpaceOrPunctuation && current is not 0)
                {
                    // new word, insert separator
                    destination[current] = separator;
                    current++;
                }

                previousUpper = isUpper;

                destination[current] = convert(chr);
                current++;
            }
            else if (!previousWhiteSpaceOrPunctuation && current is not 0)
            {
                destination[current] = separator;
                current++;
                previousUpper = false;
            }

            previousWhiteSpaceOrPunctuation = whiteSpaceOrPunctuation;
        }

        return current;
    }

    private static int ToCamelPascalCase(ReadOnlySpan<char> source, Span<char> destination, Func<char, char> firstCharConvert, Func<char, char> toUpper)
    {
        if (destination.Length < source.Length)
        {
            return -1;
        }

        if (source.Length is 0)
        {
            return default;
        }

        // find the first non-whitespace/punctation
        var start = -1;
        for (var i = 0; i < source.Length; i++)
        {
            var chr = source[i];
            if (char.IsWhiteSpace(chr) || char.IsPunctuation(chr))
            {
                continue;
            }

            start = i;
            break;
        }

        if (start is -1)
        {
            return 0;
        }

        destination[0] = firstCharConvert(source[start]);

        // go through the rest of the characters, making sure we title case
        var current = 1;
        var previousWhiteSpaceOrPunctuation = false;
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        source = source[(start + 1)..];
#else
        source = source.Slice(start + 1);
#endif
        foreach (var chr in source)
        {
            var whiteSpaceOrPunctuation = char.IsWhiteSpace(chr) || char.IsPunctuation(chr);
            if (!whiteSpaceOrPunctuation)
            {
                if (previousWhiteSpaceOrPunctuation)
                {
                    destination[current] = toUpper(chr);
                }
                else
                {
                    destination[current] = chr;
                }

                current++;
            }

            previousWhiteSpaceOrPunctuation = whiteSpaceOrPunctuation;
        }

        return current;
    }

    private static
#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_1_OR_GREATER
        unsafe
#endif
        string AsString(this Span<char> source)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        => new(source);
#else
    {
        var ptr = (char*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref source.GetPinnableReference());
        return new(ptr, 0, source.Length);
    }
#endif
}