// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System;
#pragma warning restore IDE0130, CheckNamespace

using Altemiq.Globalization;

/// <summary>
/// The <see cref="string"/> extensions.
/// </summary>
[Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "ConvertToExtensionBlock", Justification = "This is valid here")]
public static class StringExtensions
{
    /// <summary>
    /// Converts the specified string to camel case.
    /// </summary>
    /// <param name="input">The string to convert to camel case.</param>
    /// <returns>The specified string converted to camel case.</returns>
    public static string ToCamelCase(this string input) => input.ToCamelCase(default);

    /// <summary>
    /// Converts the specified string to camel case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="input">The string to convert to camel case.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The specified string converted to camel case.</returns>
    public static string ToCamelCase(this string input, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToCamelCase(input);

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to camel case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    // ReSharper disable once ConvertToExtensionBlock
    public static int ToCamelCase(this ReadOnlySpan<char> source, Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToCamelCase(source, destination);

    /// <summary>
    /// Converts the specified string to pascal case.
    /// </summary>
    /// <param name="input">The string to convert to pascal case.</param>
    /// <returns>The specified string converted to pascal case.</returns>
    public static string ToPascalCase(this string input) => input.ToPascalCase(default);

    /// <summary>
    /// Converts the specified string to pascal case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="input">The string to convert to pascal case.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The specified string converted to pascal case.</returns>
    public static string ToPascalCase(this string input, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToPascalCase(input);

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to pascal case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    // ReSharper disable once ConvertToExtensionBlock
    public static int ToPascalCase(this ReadOnlySpan<char> source, Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToPascalCase(source, destination);

    /// <summary>
    /// Converts the specified string to kebab case.
    /// </summary>
    /// <param name="input">The string to convert to kebab case.</param>
    /// <returns>The specified string converted to kebab case.</returns>
    public static string ToKebabCase(this string input) => input.ToKebabCase(default);

    /// <summary>
    /// Converts the specified string to kebab case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="input">The string to convert to kebab case.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The specified string converted to kebab case.</returns>
    public static string ToKebabCase(this string input, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToKebabCase(input);

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to kebab case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    public static int ToKebabCase(this ReadOnlySpan<char> source, Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToKebabCase(source, destination);

    /// <summary>
    /// Converts the specified string to snake case.
    /// </summary>
    /// <param name="input">The string to convert to snake case.</param>
    /// <returns>The specified string converted to snake case.</returns>
    public static string ToSnakeCase(this string input) => input.ToSnakeCase(default);

    /// <summary>
    /// Converts the specified string to snake case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="input">The string to convert to snake case.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The specified string converted to snake case.</returns>
    public static string ToSnakeCase(this string input, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToSnakeCase(input);

    /// <summary>
    /// Copies the characters from the source span into the destination, converting each character to snake case, using the casing rules of the specified culture.
    /// </summary>
    /// <param name="source">The source span.</param>
    /// <param name="destination">The destination span which contains the transformed characters.</param>
    /// <param name="culture">An object that supplies culture-specific casing rules.</param>
    /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
    public static int ToSnakeCase(this ReadOnlySpan<char> source, Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToSnakeCase(source, destination);

    /// <summary>
    /// Removes the specified suffix from the input if found.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="suffix">The suffix to remove.</param>
    /// <returns><paramref name="source"/> with <paramref name="suffix"/> removed.</returns>
    public static string RemoveSuffix(this string source, string suffix) =>
        source.AsSpan().RemoveSuffix(suffix.AsSpan()) switch
        {
            -1 => source,
            0 => string.Empty,
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            var length => source[..length],
#else
            var length => source.Substring(0, length),
#endif
        };

    /// <summary>
    /// Removes the specified suffix from the input if found.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="suffix">The suffix to remove.</param>
    /// <returns>The new length of <paramref name="source"/> if <paramref name="suffix"/> was found; otherwise -1.</returns>
    public static int RemoveSuffix(this ReadOnlySpan<char> source, ReadOnlySpan<char> suffix)
    {
        for (var i = 0; i < suffix.Length; i++)
        {
            if (suffix[suffix.Length - i - 1] != source[source.Length - i - 1])
            {
                return -1;
            }
        }

        return source.Length - suffix.Length;
    }

    private static Globalization.TextInfo GetTextInfo(Globalization.CultureInfo? culture) =>
        culture switch
        {
            { TextInfo: var textInfo } => textInfo,
            _ => Globalization.CultureInfo.CurrentCulture.TextInfo,
        };
}