namespace Altemiq.Extensions.Configuration.Yaml;

using Microsoft.Extensions.Configuration;

public class YamlConfigurationExtensionsTest
{
    [Test]
    [Arguments(null)]
    [Arguments("")]
    public async Task AddYamlFileThrowsIfFilePathIsNullOrEmpty(string? path)
    {
        // Arrange
        var configurationBuilder = new ConfigurationBuilder();

        // Act and Assert
        _ = await Assert.That(() => configurationBuilder.AddYamlFile(path!))
            .Throws<ArgumentException>()
            .And.WithParameterName("path");
    }

    [Test]
    public async Task AddYamlFileThrowsIfFileDoesNotExistAtPath()
    {
        // Arrange
        var path = "file-does-not-exist.yaml";

        // Act and Assert
        _ = await Assert.That(() => new ConfigurationBuilder().AddYamlFile(path).Build()).Throws<FileNotFoundException>()
            .WithMessageContaining($"The configuration file '{path}' was not found and is not optional.");
    }
}