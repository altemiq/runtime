namespace Altemiq.Extensions.Configuration.Yaml;

using Microsoft.Extensions.Configuration;

public class YamlConfigurationExtensionsTest
{
    [Test]
    [Arguments(null)]
    [Arguments("")]
    public async Task AddYamlFile_ThrowsIfFilePathIsNullOrEmpty(string? path)
    {
        // Arrange
        var configurationBuilder = new ConfigurationBuilder();

        // Act and Assert
        await Assert.That(() => configurationBuilder.AddYamlFile(path!))
            .Throws<ArgumentException>()
            .WithMessageContaining("File path must be a non-empty string.")
            .And.WithParameterName("path");
    }

    [Test]
    public async Task AddYamlFile_ThrowsIfFileDoesNotExistAtPath()
    {
        // Arrange
        var path = "file-does-not-exist.yaml";

        // Act and Assert
        await Assert.That(() => new ConfigurationBuilder().AddYamlFile(path).Build()).Throws<FileNotFoundException>()
            .WithMessageContaining($"The configuration file '{path}' was not found and is not optional.");
    }
}