// -----------------------------------------------------------------------
// <copyright file="YamlConfigurationProvider.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Configuration.Yaml;

/// <summary>
/// Provides configuration key-value pairs that are obtained from a YAML file.
/// </summary>
public class YamlConfigurationProvider(FileConfigurationSource source) : FileConfigurationProvider(source)
{
    /// <inheritdoc />
    public override void Load(Stream stream)
    {
        try
        {
            this.Data = YamlConfigurationFileParser.Parse(stream);
        }
        catch (YamlDotNet.Core.YamlException e)
        {
            throw new FormatException(Properties.Resources.Error_YAMLParseError, e);
        }
    }
}