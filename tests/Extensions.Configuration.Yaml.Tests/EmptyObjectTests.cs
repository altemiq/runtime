namespace Altemiq.Extensions.Configuration.Yaml;

public class EmptyObjectTests
{
    [Test]
    public async Task EmptyObject_AddsAsNull()
    {
        var yaml = "key: {}";
        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        await Assert.That(yamlConfigSource.Get("key")).IsNull();
    }

    [Test]
    [Arguments("key: ~")]
    [Arguments("key: null")]
    [Arguments("key:")]
    public async Task NullObject_AddsEmptyString(string yaml)
    {
        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        await Assert.That(yamlConfigSource.Get("key")).IsNull();
    }

    [Test]
    public async Task NestedObject_DoesNotAddParent()
    {
        var yaml =
            """
            key:
              nested: value
            """;

        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        await Assert.That(yamlConfigSource.TryGet("key", out _)).IsFalse();
        await Assert.That(yamlConfigSource.Get("key:nested")).IsEqualTo("value");
    }
}