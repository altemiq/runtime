// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System;
#pragma warning restore IDE0130, CheckNamespace

using Altemiq.Globalization;

#pragma warning disable CA1708, RCS1263, SA1101

/// <summary>
/// The <see cref="string"/> extensions.
/// </summary>
[Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "ConvertToExtensionBlock", Justification = "This is valid here")]
public static class StringExtensions
{
    /// <content>
    /// The <see cref="string"/> extensions.
    /// </content>
    /// <param name="input">The string to convert to camel case.</param>
    extension(string input)
    {
        /// <summary>
        /// Converts the specified string to camel case.
        /// </summary>
        /// <returns>The specified string converted to camel case.</returns>
        public string ToCamelCase() => input.ToCamelCase(default);

        /// <summary>
        /// Converts the specified string to camel case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The specified string converted to camel case.</returns>
        public string ToCamelCase(Globalization.CultureInfo? culture) => GetTextInfo(culture).ToCamelCase(input);

        /// <summary>
        /// Converts the specified string to pascal case.
        /// </summary>
        /// <returns>The specified string converted to pascal case.</returns>
        public string ToPascalCase() => input.ToPascalCase(default);

        /// <summary>
        /// Converts the specified string to pascal case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The specified string converted to pascal case.</returns>
        public string ToPascalCase(Globalization.CultureInfo? culture) => GetTextInfo(culture).ToPascalCase(input);

        /// <summary>
        /// Converts the specified string to kebab case.
        /// </summary>
        /// <returns>The specified string converted to kebab case.</returns>
        public string ToKebabCase() => input.ToKebabCase(default);

        /// <summary>
        /// Converts the specified string to kebab case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The specified string converted to kebab case.</returns>
        public string ToKebabCase(Globalization.CultureInfo? culture) => GetTextInfo(culture).ToKebabCase(input);

        /// <summary>
        /// Converts the specified string to snake case.
        /// </summary>
        /// <returns>The specified string converted to snake case.</returns>
        public string ToSnakeCase() => input.ToSnakeCase(default);

        /// <summary>
        /// Converts the specified string to snake case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The specified string converted to snake case.</returns>
        public string ToSnakeCase(Globalization.CultureInfo? culture) => GetTextInfo(culture).ToSnakeCase(input);

        /// <summary>
        /// Removes the specified suffix from the input if found.
        /// </summary>
        /// <param name="suffix">The suffix to remove.</param>
        /// <returns>The specified string with <paramref name="suffix"/> removed.</returns>
        public string RemoveSuffix(string suffix) =>
            input.AsSpan().RemoveSuffix(suffix.AsSpan()) switch
            {
                -1 => input,
                0 => string.Empty,
#if NETCOREAPP3_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                var length => input[..length],
#else
                var length => input.Substring(0, length),
#endif
            };
    }

    /// <content>
    /// The <see cref="ReadOnlySpan{Char}"/> extensions.
    /// </content>
    /// <param name="source">The source span.</param>
    extension(ReadOnlySpan<char> source)
    {
        /// <summary>
        /// Copies the characters from the source span into the destination, converting each character to camel case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="destination">The destination span which contains the transformed characters.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
        // ReSharper disable once ConvertToExtensionBlock
        public int ToCamelCase(Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToCamelCase(source, destination);

        /// <summary>
        /// Copies the characters from the source span into the destination, converting each character to pascal case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="destination">The destination span which contains the transformed characters.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
        // ReSharper disable once ConvertToExtensionBlock
        public int ToPascalCase(Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToPascalCase(source, destination);

        /// <summary>
        /// Copies the characters from the source span into the destination, converting each character to kebab case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="destination">The destination span which contains the transformed characters.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
        public int ToKebabCase(Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToKebabCase(source, destination);

        /// <summary>
        /// Copies the characters from the source span into the destination, converting each character to snake case, using the casing rules of the specified culture.
        /// </summary>
        /// <param name="destination">The destination span which contains the transformed characters.</param>
        /// <param name="culture">An object that supplies culture-specific casing rules.</param>
        /// <returns>The number of characters written into the destination span. If the destination is too small, returns -1.</returns>
        public int ToSnakeCase(Span<char> destination, Globalization.CultureInfo? culture) => GetTextInfo(culture).ToSnakeCase(source, destination);

        /// <summary>
        /// Removes the specified suffix from the input if found.
        /// </summary>
        /// <param name="suffix">The suffix to remove.</param>
        /// <returns>The new length if <paramref name="suffix"/> was found; otherwise -1.</returns>
        public int RemoveSuffix(ReadOnlySpan<char> suffix)
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
    }

    private static Globalization.TextInfo GetTextInfo(Globalization.CultureInfo? culture) =>
        culture switch
        {
            { TextInfo: var textInfo } => textInfo,
            _ => Globalization.CultureInfo.CurrentCulture.TextInfo,
        };
}