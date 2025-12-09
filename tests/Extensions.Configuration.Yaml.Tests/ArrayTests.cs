// -----------------------------------------------------------------------
// <copyright file="ArrayTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.Extensions.Configuration;

namespace Altemiq.Extensions.Configuration.Yaml;

public class ArrayTests
{
    [Test]
    public async Task ArraysAreConvertedToKeyValuePairs()
    {
        var yaml = """
                   ip:
                     - 1.2.3.4
                     - 7.8.9.10
                     - 11.12.13.14
                   """;

        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));
        await Assert.That(yamlConfigSource.Get("ip:0")).IsEqualTo("1.2.3.4");
        await Assert.That(yamlConfigSource.Get("ip:1")).IsEqualTo("7.8.9.10");
        await Assert.That(yamlConfigSource.Get("ip:2")).IsEqualTo("11.12.13.14");
    }

    [Test]
    public async Task ArrayOfObjects()
    {
        var yaml =
            """
            ip:
              - address: 1.2.3.4
                hidden: false
              - address: 5.6.7.8
                hidden: true
            """;

        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        await Assert.That(yamlConfigSource.Get("ip:0:address")).IsEqualTo("1.2.3.4");
        await Assert.That(yamlConfigSource.Get("ip:0:hidden")).IsEqualTo("False");
        await Assert.That(yamlConfigSource.Get("ip:1:address")).IsEqualTo("5.6.7.8");
        await Assert.That(yamlConfigSource.Get("ip:1:hidden")).IsEqualTo("True");
    }

    [Test]
    public async Task NestedArrays()
    {
        var yaml =
            """
            ip:
              - - 1.2.3.4
                - 5.6.7.8
              - - 9.10.11.12
                - 13.14.15.16
            """;

        var yamlConfigSource = new YamlConfigurationProvider(new YamlConfigurationSource());
        yamlConfigSource.Load(TestStreamHelpers.StringToStream(yaml));

        await Assert.That(yamlConfigSource.Get("ip:0:0")).IsEqualTo("1.2.3.4");
        await Assert.That(yamlConfigSource.Get("ip:0:1")).IsEqualTo("5.6.7.8");
        await Assert.That(yamlConfigSource.Get("ip:1:0")).IsEqualTo("9.10.11.12");
        await Assert.That(yamlConfigSource.Get("ip:1:1")).IsEqualTo("13.14.15.16");
    }

    [Test]
    public async Task ImplicitArrayItemReplacement()
    {
        var yaml1 =
            """
            ip:
              - 1.2.3.4
              - 7.8.9.10
              - 11.12.13.14
            """;

        var yaml2 =
            """
            ip:
              - 15.16.17.18
            """;

        var yamlConfigSource1 = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml1) };
        var yamlConfigSource2 = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml2) };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.Add(yamlConfigSource1);
        configurationBuilder.Add(yamlConfigSource2);
        var config = configurationBuilder.Build();

        await Assert.That(config.GetSection("ip").GetChildren()).Count().IsEqualTo(3);
        await Assert.That(config["ip:0"]).IsEqualTo("15.16.17.18");
        await Assert.That(config["ip:1"]).IsEqualTo("7.8.9.10");
        await Assert.That(config["ip:2"]).IsEqualTo("11.12.13.14");
    }

    [Test]
    public async Task ExplicitArrayReplacement()
    {
        var yaml1 =
            """
                ip:
                  - 1.2.3.4
                  - 7.8.9.10
                  - 11.12.13.14
                """;

        var yaml2 =
            """
                ip:
                  '1': 15.16.17.18
                """;

        var yamlConfigSource1 = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml1) };
        var yamlConfigSource2 = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml2) };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.Add(yamlConfigSource1);
        configurationBuilder.Add(yamlConfigSource2);
        var config = configurationBuilder.Build();

        await Assert.That(config.GetSection("ip").GetChildren()).Count().IsEqualTo(3);
        await Assert.That(config["ip:0"]).IsEqualTo("1.2.3.4");
        await Assert.That(config["ip:1"]).IsEqualTo("15.16.17.18");
        await Assert.That(config["ip:2"]).IsEqualTo("11.12.13.14");
    }

    [Test]
    public async Task ArrayMerge()
    {
        var yaml1 =
            """
                ip:
                  - 1.2.3.4
                  - 7.8.9.10
                  - 11.12.13.14
                """;

        var yaml2 =
            """
                ip:
                  '3': 15.16.17.18
                """;

        var yamlConfigSource1 = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml1) };
        var yamlConfigSource2 = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml2) };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.Add(yamlConfigSource1);
        configurationBuilder.Add(yamlConfigSource2);
        var config = configurationBuilder.Build();


        await Assert.That(config.GetSection("ip").GetChildren()).Count().IsEqualTo(4);
        await Assert.That(config["ip:0"]).IsEqualTo("1.2.3.4");
        await Assert.That(config["ip:1"]).IsEqualTo("7.8.9.10");
        await Assert.That(config["ip:2"]).IsEqualTo("11.12.13.14");
        await Assert.That(config["ip:3"]).IsEqualTo("15.16.17.18");
    }

    [Test]
    public async Task ArraysAreKeptInFileOrder()
    {
        var yaml =
            """
            setting:
              - b
              - a
              - '2'
            """;

        var yamlConfigSource = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml) };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.Add(yamlConfigSource);
        var config = configurationBuilder.Build();

        var configurationSection = config.GetSection("setting");
        var indexConfigurationSections = configurationSection.GetChildren().ToArray();

        await Assert.That(indexConfigurationSections).Count().IsEqualTo(3);
        await Assert.That(indexConfigurationSections[0].Value).IsEqualTo("b");
        await Assert.That(indexConfigurationSections[1].Value).IsEqualTo("a");
        await Assert.That(indexConfigurationSections[2].Value).IsEqualTo("2");
    }

    [Test]
    public async Task PropertiesAreSortedByNumberOnlyFirst()
    {
        var yaml =
            """
            setting:
              '4': d
              '10': e
              '42': c
              hello: a
              bob: b
              1text: f
            """;

        var yamlConfigSource = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml) };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.Add(yamlConfigSource);
        var config = configurationBuilder.Build();

        var configurationSection = config.GetSection("setting");
        var indexConfigurationSections = configurationSection.GetChildren().ToArray();

        await Assert.That(indexConfigurationSections).Count().IsEqualTo(6);
        await Assert.That(indexConfigurationSections[0].Key).IsEqualTo("4");
        await Assert.That(indexConfigurationSections[1].Key).IsEqualTo("10");
        await Assert.That(indexConfigurationSections[2].Key).IsEqualTo("42");
        await Assert.That(indexConfigurationSections[3].Key).IsEqualTo("1text");
        await Assert.That(indexConfigurationSections[4].Key).IsEqualTo("bob");
        await Assert.That(indexConfigurationSections[5].Key).IsEqualTo("hello");
    }

    [Test]
    public async Task EmptyArrayNotIgnored()
    {
        var yaml =
            """
            ip:
              array: []
              object: {}
            """;

        var yamlConfigSource = new YamlConfigurationSource { FileProvider = TestStreamHelpers.StringToFileProvider(yaml) };

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.Add(yamlConfigSource);
        var config = configurationBuilder.Build();

        var configurationSection = config.GetSection("ip");

        var ipSectionChildren = configurationSection.GetChildren().ToArray();

        await Assert.That(config.GetChildren()).Count().IsEqualTo(1);
        await Assert.That(ipSectionChildren).Count().IsEqualTo(2);
        await Assert.That(ipSectionChildren[0].Key).IsEqualTo("array");
        await Assert.That(ipSectionChildren[0].Value).IsEqualTo(string.Empty);
        await Assert.That(ipSectionChildren[1].Key).IsEqualTo("object");
        await Assert.That(ipSectionChildren[1].Value).IsNull();
        await Assert.That(ipSectionChildren[0].GetChildren()).IsEmpty();
        await Assert.That(ipSectionChildren[1].GetChildren()).IsEmpty();
    }
}