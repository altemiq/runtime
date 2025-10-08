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
    
    [Test]
    [Arguments("TestSuffix", "Suffix", "Test")]
    [Arguments("TestSuffixes", "Suffix", "TestSuffixes")]
    [Arguments("Test", "Test", "")]
    public async Task RemoveSuffix(string input, string suffix, string expected) => await Assert.That(input.RemoveSuffix(suffix)).IsEqualTo(expected);
}