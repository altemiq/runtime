namespace Altemiq;

public class StringExtensionTests
{
    [Test]
    [Data.CamelCaseDataGenerator]
    public async Task ToCamelCase(string input, string expected) => await Assert.That(input.ToCamelCase()).IsEqualTo(expected);
    
    [Test]
    [Data.SnakeCaseDataGenerator]
    public async Task ToSnakeCase(string input, string expected) => await Assert.That(input.ToSnakeCase()).IsEqualTo(expected);

    [Test]
    [Data.KebabCaseDataGenerator]
    public async Task ToKebabCase(string input, string expected) => await Assert.That(input.ToKebabCase()).IsEqualTo(expected);
}