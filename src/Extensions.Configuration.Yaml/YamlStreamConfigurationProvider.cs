// -----------------------------------------------------------------------
// <copyright file="YamlStreamConfigurationProvider.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Configuration.Yaml;

/// <summary>
/// Provides configuration key-value pairs that are obtained from a YAML stream.
/// </summary>
public class YamlStreamConfigurationProvider(StreamConfigurationSource source) : StreamConfigurationProvider(source)
{
    /// <summary>
    /// Loads YAML configuration key-value pairs from a stream into a provider.
    /// </summary>
    /// <param name="stream">The <see cref="Stream"/> to load INI configuration data from.</param>
    public override void Load(Stream stream) => this.Data = YamlConfigurationFileParser.Parse(stream);
}