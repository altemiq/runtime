namespace Altemiq.Extensions.Configuration.Yaml;

using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

public class YamlConfigurationFileParserTests
{
    private static YamlConfigurationProvider LoadProvider(string yaml)
    {
        var p = new YamlConfigurationProvider(new YamlConfigurationSource { Optional = true });
        p.Load(TestStreamHelpers.StringToStream(yaml));
        return p;
    }

    [Test]
    public async Task CanLoadValidYamlFromStreamProvider()
    {
        var yaml =
            """
            firstname: test
            test.last.name: last.name
            residential.address:
              street.name: Something street
              zipcode: '12345'
            """;
        var config = new ConfigurationBuilder().AddYamlStream(TestStreamHelpers.StringToStream(yaml)).Build();
        _ = await Assert.That(config["firstname"]).IsEqualTo("test");
        _ = await Assert.That(config["test.last.name"]).IsEqualTo("last.name");
        _ = await Assert.That(config["residential.address:STREET.name"]).IsEqualTo("Something street");
        _ = await Assert.That(config["residential.address:zipcode"]).IsEqualTo("12345");
    }

    [Test]
    public async Task ReloadThrowsFromStreamProvider()
    {
        var yaml = "firstname: test";
        var config = new ConfigurationBuilder().AddYamlStream(TestStreamHelpers.StringToStream(yaml)).Build();
        _ = await Assert.That(config.Reload).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task LoadKeyValuePairsFromValidYaml()
    {
        var yaml =
            """
        firstname: test
        test.last.name: last.name
        residential.address:
          street.name: Something street
          zipcode: '12345'
        """;
        var yamlConfigSrc = LoadProvider(yaml);

        _ = await Assert.That(yamlConfigSrc.Get("firstname")).IsEqualTo("test");
        _ = await Assert.That(yamlConfigSrc.Get("test.last.name")).IsEqualTo("last.name");
        _ = await Assert.That(yamlConfigSrc.Get("residential.address:STREET.name")).IsEqualTo("Something street");
        _ = await Assert.That(yamlConfigSrc.Get("residential.address:zipcode")).IsEqualTo("12345");
    }

    [Test]
    public async Task LoadMethodCanHandleEmptyValue()
    {
        var yaml = "name: ''";
        var yamlConfigSrc = LoadProvider(yaml);
        _ = await Assert.That(yamlConfigSrc.Get("name")).IsEmpty();
    }

    [Test]
    public async Task LoadWithCulture()
    {
        var previousCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = new("fr-FR");

            var yaml = "number: 3.14";
            var yamlConfigSrc = LoadProvider(yaml);
            _ = await Assert.That(yamlConfigSrc.Get("number")).IsEqualTo("3.14");
        }
        finally
        {
            CultureInfo.CurrentCulture = previousCulture;
        }
    }

    [Test]
    public async Task NonObjectRootIsInvalid()
    {
        var yaml =
            """
        - first
        - second
        """;

        _ = await Assert.That(() => LoadProvider(yaml)).Throws<FormatException>().And.Member(m => m.Message, message => message.IsNotNull());
    }

    [Test]
    public async Task SupportAndIgnoreComments()
    {
        var yaml =
            """
        # Comments
        name: test
        # Comments
        address:
          street: Something street # Comments
          zipcode: '12345'
        """;
        var yamlConfigSrc = LoadProvider(yaml);
        _ = await Assert.That(yamlConfigSrc.Get("name")).IsEqualTo("test");
        _ = await Assert.That(yamlConfigSrc.Get("address:street")).IsEqualTo("Something street");
        _ = await Assert.That(yamlConfigSrc.Get("address:zipcode")).IsEqualTo("12345");
    }

    [Test]
    public async Task ThrowExceptionWhenPassingNullAsFilePath()
    {
        _ = await Assert.That(static () => new ConfigurationBuilder().AddYamlFile(path: null!)).Throws<ArgumentException>().WithParameterName("path");
    }

    [Test]
    public async Task ThrowExceptionWhenPassingEmptyStringAsFilePath()
    {
        _ = await Assert.That(static () => new ConfigurationBuilder().AddYamlFile(path: string.Empty)).Throws<ArgumentException>().WithParameterName("path");
    }

    [Test]
    public async Task YamlConfigurationThrowsOnMissingConfigurationFile()
    {
        var config = new ConfigurationBuilder().AddYamlFile("NotExistingConfig.yaml", optional: false);
        _ = await Assert.That(() => config.Build())
            .Throws<FileNotFoundException>()
            .WithMessageContaining("The configuration file 'NotExistingConfig.yaml' was not found and is not optional.");
    }

    [Test]
    public async Task YamlConfigurationDoesNotThrowOnOptionalConfiguration()
    {
        _ = await Assert.That(static () => new ConfigurationBuilder().AddYamlFile("NotExistingConfig.yaml", optional: true).Build()).ThrowsNothing();
    }

    [Test]
    public async Task ThrowFormatExceptionWhenFileIsEmpty()
    {
        _ = await Assert.That(static () => LoadProvider(@"")).Throws<FormatException>().WithMessage("Could not parse the YAML file.");
    }

    [Test]
    public async Task AddYamlFileFileProviderIsNotDisposedWhenSourcesGetReloaded()
    {
        var filePath = Path.GetTempFileName();
        File.WriteAllText(filePath, "some: value");

        IConfigurationBuilder builder = new ConfigurationManager();

        _ = builder.AddYamlFile(filePath, optional: false);

        var fileConfigurationSource = (FileConfigurationSource)builder.Sources.Last();
        var fileProvider = fileConfigurationSource.FileProvider as PhysicalFileProvider;

        _ = await Assert.That(fileConfigurationSource.FileProvider)
            .IsTypeOf<PhysicalFileProvider>().And
            .Satisfies(GetIsDisposed, static disposed => disposed.IsFalse());

        builder.Properties.Add("simplest", "repro");

        _ = await Assert.That(fileConfigurationSource.FileProvider)
            .IsTypeOf<PhysicalFileProvider>().And
            .Satisfies(GetIsDisposed, static disposed => disposed.IsFalse());

        fileProvider?.Dispose();
        _ = await Assert.That(fileConfigurationSource.FileProvider)
            .IsTypeOf<PhysicalFileProvider>().And
            .Satisfies(GetIsDisposed, static disposed => disposed.IsTrue());

        File.Delete(filePath);
    }

    private static bool GetIsDisposed(PhysicalFileProvider? fileProvider)
    {
        var isDisposedField = typeof(PhysicalFileProvider).GetField("_disposed", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return isDisposedField?.GetValue(fileProvider) is bool b && b;
    }
}