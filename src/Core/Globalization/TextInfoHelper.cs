// -----------------------------------------------------------------------
// <copyright file="TextInfoHelper.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace System.Globalization;
#pragma warning restore IDE0130

/// <summary>
/// The <see cref="TextInfo"/> helper.
/// </summary>
public static class TextInfoHelper
{
    /// <summary>
    /// Gets the <see cref="TextInfo"/> associated with the specified <see cref="IFormatProvider"/>.
    /// </summary>
    /// <param name="formatProvider">The <see cref="IFormatProvider"/> used to get the <see cref="TextInfo"/>.</param>
    /// <returns>The <see cref="TextInfo"/> associated with the specified <see cref="IFormatProvider"/>.</returns>
    public static TextInfo GetInstance(IFormatProvider? formatProvider) => formatProvider switch
    {
        CultureInfo cultureProvider => cultureProvider.TextInfo,
        _ => CultureInfo.CurrentCulture.TextInfo,
    };
}