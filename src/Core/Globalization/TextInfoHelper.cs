// -----------------------------------------------------------------------
// <copyright file="TextInfoHelper.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System.Globalization;
#pragma warning restore IDE0130, CheckNamespace

////#pragma warning disable SA1137, SA1400, S1144

/// <summary>
/// The <see cref="TextInfo"/> helper.
/// </summary>
public static class TextInfoHelper
{
    /// <summary>
    /// The extensions for <see cref="TextInfo"/>.
    /// </summary>
    extension(TextInfo)
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
}