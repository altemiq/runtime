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
public static class StringExtensions
{
    /// <summary>
    /// Converts the specified string to camel case.
    /// </summary>
    /// <param name="input">The string to convert to camel case.</param>
    /// <returns>The specified string converted to camel case.</returns>
    public static string ToCamelCase(this string input) => ToCamelCase(input, default);

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
    public static int ToCamelCase(this ReadOnlySpan<char> source, Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToCamelCase(source, destination);

    /// <summary>
    /// Converts the specified string to kebab case.
    /// </summary>
    /// <param name="input">The string to convert to kebab case.</param>
    /// <returns>The specified string converted to kebab case.</returns>
    public static string ToKebabCase(this string input) => ToKebabCase(input, default);

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
    public static string ToSnakeCase(this string input) => ToSnakeCase(input, default);

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

    private static Globalization.TextInfo GetTextInfo(Globalization.CultureInfo? culture)
    {
        return culture switch
        {
            { TextInfo: var textInfo } => textInfo,
            _ => Globalization.CultureInfo.CurrentCulture.TextInfo,
        };
    }
}