namespace Altemiq.Extensions.Configuration.Yaml;

public class EmptyObjectTests
{
    [Test]
    public async Task EmptyObjectAddsAsNull()
    {
        var yaml = "key: {}";
        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        _ = await Assert.That(yamlConfigSource.Get("key")).IsNull();
    }

    [Test]
    [Arguments("key: ~")]
    [Arguments("key: null")]
    [Arguments("key:")]
    public async Task NullObjectAddsEmptyString(string yaml)
    {
        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        _ = await Assert.That(yamlConfigSource.Get("key")).IsNull();
    }

    [Test]
    public async Task NestedObjectDoesNotAddParent()
    {
        var yaml =
            """
            key:
              nested: value
            """;

        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        _ = await Assert.That(yamlConfigSource.TryGet("key", out _)).IsFalse();
        _ = await Assert.That(yamlConfigSource.Get("key:nested")).IsEqualTo("value");
    }
}