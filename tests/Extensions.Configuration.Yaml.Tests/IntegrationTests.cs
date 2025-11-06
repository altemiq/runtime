using Microsoft.Extensions.Configuration;

namespace Altemiq.Extensions.Configuration.Yaml;

public class IntegrationTest
{
    [Test]
    public async Task MinimalYaml_GetChildrenFromConfiguration_NoConfigurationSection()
    {
        var yaml = "{}";

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddYamlStream(TestStreamHelpers.StringToStream(yaml));
        var configuration = configurationBuilder.Build();

        await Assert.That(configuration.GetChildren()).IsEmpty();
    }

    [Test]
    public async Task LoadYamlConfiguration()
    {
        var yaml =
            """
            a: b
            c:
              d: e
            f: ''
            g: null
            h: {}
            i:
              k: {}
            """;

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddYamlStream(TestStreamHelpers.StringToStream(yaml));
        var configuration = configurationBuilder.Build();

        var children = configuration.GetChildren();

        foreach (var (x, i) in children.Select((section, i) => (section, i)))
        {
            Task task = i switch
            {
                0 => AssertSection(x, "a", "b"),
                1 => AssertSection(x, "c", null, [c => AssertSection(c, "d", "e")]),
                2 => AssertSection(x, "f", ""),
                3 => AssertSection(x, "g", null),
                4 => AssertSection(x, "h", null),
                5 => AssertSection(x, "i", null, [c => AssertSection(c, "k", null)]),
                _ => throw new KeyNotFoundException(),
            };

            await task;
        }
    }

    private static Task AssertSection(IConfigurationSection configurationSection, string key, string? value)
        => AssertSection(configurationSection, key, value, []);

    private static async Task AssertSection(IConfigurationSection configurationSection, string key, string? value, Func<IConfigurationSection, Task>[] childrenInspectors)
    {
        if (key != configurationSection.Key || value != configurationSection.Value)
        {
            Assert.Fail($"The key/value pairs are not equal, expected: {GetString(key, value)}, actual: {GetString(configurationSection)}");
        }

        foreach (var (child, inspector) in configurationSection.GetChildren().Zip(childrenInspectors))
        {
            await inspector(child);
        }
    }

    private static string GetString(IConfigurationSection configurationSection) => GetString(configurationSection.Key, configurationSection.Value);
    private static string GetString(string key, string? value) => $"\"{key}\":" + (value is null ? "null" : $"\"{value}\"");
}