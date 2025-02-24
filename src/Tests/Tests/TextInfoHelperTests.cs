// -----------------------------------------------------------------------
// <copyright file="TextInfoHelperTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using System.Globalization;

public class TextInfoHelperTests
{
    [Test]
    public async Task GetInfoFromCultureInfo()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        await Test(TextInfoHelper.GetInstance, cultureInfo, cultureInfo);
    }

    [Test]
    public Task GetInfoFromFormatProvider()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        return Test(TextInfoHelper.GetInstance, NumberFormatInfo.GetInstance(cultureInfo), cultureInfo);
    }

    [Test]
    public Task GetInfoFromNull()
    {
        return Test(TextInfoHelper.GetInstance, default(IFormatProvider), CultureInfo.CurrentCulture);
    }

    private static async Task Test<T>(Func<T?, TextInfo> func, T? value, CultureInfo cultureInfo)
        where T : IFormatProvider
    {
        await Assert.That(func(value)).IsEqualTo(cultureInfo.TextInfo);
    }
}