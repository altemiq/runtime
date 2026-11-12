namespace Altemiq.Globalization;

public class TextInfoExtensionTests
{
    [Test]
    [MethodDataSource(nameof(GetFormatProviders))]
    public async Task GetTextInfoFromNullOrInvalid(IFormatProvider? formatProvider)
    {
        _ = await Assert.That(System.Globalization.TextInfo.GetInstance(formatProvider))
            .IsEqualTo(System.Globalization.CultureInfo.CurrentCulture.TextInfo).And
            .IsEqualTo(System.Globalization.TextInfo.CurrentInfo);
    }

    [Test]
    public async Task GetTextInfoFromInstance()
    {
        _ = await Assert.That(System.Globalization.TextInfo.GetInstance(System.Globalization.CultureInfo.InvariantCulture))
            .IsEqualTo(System.Globalization.CultureInfo.InvariantCulture.TextInfo).And
            .IsEqualTo(System.Globalization.TextInfo.InvariantInfo);
    }

    [Test]
    public async Task GetInvariant()
    {
        _ = await Assert.That(System.Globalization.TextInfo.InvariantInfo).IsEqualTo(System.Globalization.CultureInfo.InvariantCulture.TextInfo);
    }

    [Test]
    [Data.CamelCaseDataGenerator]
    public async Task ToCamelCase(string input, string expected)
    {
        _ = await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToCamelCase(input)).IsEqualTo(expected);
    }

    [Test]
    [Data.PascalCaseDataGenerator]
    public async Task ToPascalCase(string input, string expected)
    {
        _ = await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToPascalCase(input)).IsEqualTo(expected);
    }

    [Test]
    [Data.SnakeCaseDataGenerator]
    public async Task ToSnakeCase(string input, string expected)
    {
        _ = await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToSnakeCase(input)).IsEqualTo(expected);
    }

    [Test]
    [Data.KebabCaseDataGenerator]
    public async Task ToKebabCase(string input, string expected)
    {
        _ = await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToKebabCase(input)).IsEqualTo(expected);
    }

    public IEnumerable<Func<IFormatProvider?>> GetFormatProviders()
    {
        yield return () => null;
        yield return () => System.Globalization.NumberFormatInfo.InvariantInfo;
    }
}