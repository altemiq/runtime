using System.Globalization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Altemiq.Extensions.Configuration.Yaml;

public class YamlConfigurationFileParserTests
{
    private const string PathParameterName = "path";
    
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
        await Assert.That(config["firstname"]).IsEqualTo("test");
        await Assert.That(config["test.last.name"]).IsEqualTo("last.name");
        await Assert.That(config["residential.address:STREET.name"]).IsEqualTo("Something street");
        await Assert.That(config["residential.address:zipcode"]).IsEqualTo("12345");
    }

    [Test]
    public async Task ReloadThrowsFromStreamProvider()
    {
        var yaml = "firstname: test";
        var config = new ConfigurationBuilder().AddYamlStream(TestStreamHelpers.StringToStream(yaml)).Build();
        await Assert.That(() => config.Reload()).Throws<InvalidOperationException>();
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

        await Assert.That(yamlConfigSrc.Get("firstname")).IsEqualTo("test");
        await Assert.That(yamlConfigSrc.Get("test.last.name")).IsEqualTo("last.name");
        await Assert.That(yamlConfigSrc.Get("residential.address:STREET.name")).IsEqualTo("Something street");
        await Assert.That(yamlConfigSrc.Get("residential.address:zipcode")).IsEqualTo("12345");
    }

    [Test]
    public async Task LoadMethodCanHandleEmptyValue()
    {
        var yaml = "name: ''";
        var yamlConfigSrc = LoadProvider(yaml);
        await Assert.That(yamlConfigSrc.Get("name")).IsEmpty();
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
            await Assert.That(yamlConfigSrc.Get("number")).IsEqualTo("3.14");
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

        await Assert.That(() => LoadProvider(yaml)).Throws<FormatException>().And.Member(m => m.Message, message => message.IsNotNull());
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
        await Assert.That(yamlConfigSrc.Get("name")).IsEqualTo("test");
        await Assert.That(yamlConfigSrc.Get("address:street")).IsEqualTo("Something street");
        await Assert.That(yamlConfigSrc.Get("address:zipcode")).IsEqualTo("12345");
    }

    [Test]
    public async Task ThrowExceptionWhenPassingNullAsFilePath()
    {
        var expectedMsg = new ArgumentException(Properties.Resources.Error_InvalidFilePath, PathParameterName).Message;

        await Assert.That(() => new ConfigurationBuilder().AddYamlFile(path: null!)).Throws<ArgumentException>().WithMessage(expectedMsg);
    }

    [Test]
    public async Task ThrowExceptionWhenPassingEmptyStringAsFilePath()
    {
        var expectedMsg = new ArgumentException(Properties.Resources.Error_InvalidFilePath, PathParameterName).Message;

        await Assert.That(() => new ConfigurationBuilder().AddYamlFile(path: string.Empty)).Throws<ArgumentException>().WithMessage(expectedMsg);
    }

    [Test]
    public async Task YamlConfiguration_Throws_On_Missing_Configuration_File()
    {
        var config = new ConfigurationBuilder().AddYamlFile("NotExistingConfig.yaml", optional: false);
        await Assert.That(() => config.Build())
            .Throws<FileNotFoundException>()
            .WithMessageContaining("The configuration file 'NotExistingConfig.yaml' was not found and is not optional.");
    }

    [Test]
    public async Task YamlConfiguration_Does_Not_Throw_On_Optional_Configuration()
    {
        await Assert.That(() => new ConfigurationBuilder().AddYamlFile("NotExistingConfig.yaml", optional: true).Build()).ThrowsNothing();
    }

    [Test]
    public async Task ThrowFormatExceptionWhenFileIsEmpty()
    {
        await Assert.That(() => LoadProvider(@"")).Throws<FormatException>().WithMessage("Could not parse the YAML file.");
    }

    [Test]
    public async Task AddYamlFile_FileProvider_Is_Not_Disposed_When_SourcesGetReloaded()
    {
        string filePath = Path.GetTempFileName();
        File.WriteAllText(filePath, "some: value");

        IConfigurationBuilder builder = new ConfigurationManager();

        builder.AddYamlFile(filePath, optional: false);

        FileConfigurationSource fileConfigurationSource = (FileConfigurationSource)builder.Sources.Last();
        PhysicalFileProvider? fileProvider = fileConfigurationSource.FileProvider as PhysicalFileProvider;

        await Assert.That(fileConfigurationSource.FileProvider)
            .IsTypeOf<PhysicalFileProvider>()
            .And.Satisfies(GetIsDisposed, disposed => disposed.IsFalse());

        builder.Properties.Add("simplest", "repro");

        await Assert.That(fileConfigurationSource.FileProvider)
            .IsTypeOf<PhysicalFileProvider>()
            .And.Satisfies(GetIsDisposed, disposed => disposed.IsFalse());

        fileProvider?.Dispose();
        await Assert.That(fileConfigurationSource.FileProvider)
            .IsTypeOf<PhysicalFileProvider>()
            .And.Satisfies(GetIsDisposed, disposed => disposed.IsTrue());
        
        File.Delete(filePath);
    }

    private static bool GetIsDisposed(PhysicalFileProvider? fileProvider)
    {
        var isDisposedField = typeof(PhysicalFileProvider).GetField("_disposed", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
        return (bool)isDisposedField!.GetValue(fileProvider)!;
    }
}