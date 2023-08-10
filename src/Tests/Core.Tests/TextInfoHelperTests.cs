// -----------------------------------------------------------------------
// <copyright file="TextInfoHelperTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using System.Globalization;

public class TextInfoHelperTests
{
    [Fact]
    public void GetInfoFromCultureInfo()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        TextInfoHelper.GetInstance(cultureInfo).Should().Be(cultureInfo.TextInfo);
    }

    [Fact]
    public void GetInfoFromFormatProvider()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        var numberFormatProvider = NumberFormatInfo.GetInstance(cultureInfo);
        TextInfoHelper.GetInstance(numberFormatProvider).Should().Be(cultureInfo.TextInfo);
    }

    [Fact]
    public void GetInfoFromNull()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        TextInfoHelper.GetInstance(default).Should().Be(cultureInfo.TextInfo);
    }
}