// -----------------------------------------------------------------------
// <copyright file="YamlStreamConfigurationSource.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Configuration.Yaml;

/// <summary>
/// Represents a YAML file as an <see cref="IConfigurationSource"/>.
/// </summary>
public class YamlStreamConfigurationSource : StreamConfigurationSource
{
    /// <inheritdoc />
    public override IConfigurationProvider Build(IConfigurationBuilder builder) => new YamlStreamConfigurationProvider(this);
}