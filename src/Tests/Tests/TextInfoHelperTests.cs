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
        Assert.Equal(cultureInfo.TextInfo, TextInfoHelper.GetInstance(cultureInfo));
    }

    [Fact]
    public void GetInfoFromFormatProvider()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        var numberFormatProvider = NumberFormatInfo.GetInstance(cultureInfo);
        Assert.Equal(cultureInfo.TextInfo, TextInfoHelper.GetInstance(numberFormatProvider));
    }

    [Fact]
    public void GetInfoFromNull()
    {
        var cultureInfo = CultureInfo.CurrentCulture;
        Assert.Equal(cultureInfo.TextInfo, TextInfoHelper.GetInstance(default));
    }
}