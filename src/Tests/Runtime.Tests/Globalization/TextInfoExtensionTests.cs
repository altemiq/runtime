namespace Altemiq.Globalization;

public class TextInfoExtensionTests
{
    [Test]
    [Data.CamelCaseDataGenerator]
    public async Task ToCamelCase(string input, string expected) => await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToCamelCase(input)).IsEqualTo(expected);
    
    [Test]
    [Data.PascalCaseDataGenerator]
    public async Task ToPascalCase(string input, string expected) => await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToPascalCase(input)).IsEqualTo(expected);
    
    [Test]
    [Data.SnakeCaseDataGenerator]
    public async Task ToSnakeCase(string input, string expected) => await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToSnakeCase(input)).IsEqualTo(expected);

    [Test]
    [Data.KebabCaseDataGenerator]
    public async Task ToKebabCase(string input, string expected) => await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToKebabCase(input)).IsEqualTo(expected);
}