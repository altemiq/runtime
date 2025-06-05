// -----------------------------------------------------------------------
// <copyright file="TextInfoHelperTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Globalization;

using System.Globalization;

public class TextInfoHelperTests
{
    [Test]
    public async Task GetInfoFromCultureInfo()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        await Test(TextInfo.GetInstance, cultureInfo, cultureInfo.TextInfo);
    }

    [Test]
    public Task GetInfoFromFormatProvider()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        return Test(TextInfo.GetInstance, NumberFormatInfo.GetInstance(cultureInfo), cultureInfo.TextInfo);
    }

    [Test]
    public Task GetInfoFromNull()
    {
        return Test(TextInfo.GetInstance, default(IFormatProvider), CultureInfo.CurrentCulture.TextInfo);
    }

    private static async Task Test<T>(Func<T?, TextInfo> func, T? value, TextInfo textInfo)
        where T : IFormatProvider
    {
        await Assert.That(func(value)).IsEqualTo(textInfo);
    }
}