// -----------------------------------------------------------------------
// <copyright file="YamlConfigurationExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace Microsoft.Extensions.Configuration;
#pragma warning restore IDE0130, CheckNamespace

#pragma warning disable RCS1263, SA1101, S2325

/// <summary>
/// Extension methods for adding <see cref="Altemiq.Extensions.Configuration.Yaml.YamlConfigurationProvider"/>.
/// </summary>
public static class YamlConfigurationExtensions
{
    /// <content>
    /// Extension methods for adding <see cref="Altemiq.Extensions.Configuration.Yaml.YamlConfigurationProvider"/>.
    /// </content>
    /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
    extension(IConfigurationBuilder builder)
    {
        /// <summary>
        /// Adds the YAML configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public IConfigurationBuilder AddYamlFile(string path) => builder.AddYamlFile(provider: null, path: path, optional: false, reloadOnChange: false);

        /// <summary>
        /// Adds the YAML configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public IConfigurationBuilder AddYamlFile(string path, bool optional) => builder.AddYamlFile(provider: null, path: path, optional: optional, reloadOnChange: false);

        /// <summary>
        /// Adds the YAML configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public IConfigurationBuilder AddYamlFile(string path, bool optional, bool reloadOnChange) => builder.AddYamlFile(provider: null, path: path, optional: optional, reloadOnChange: reloadOnChange);

        /// <summary>
        /// Adds a YAML configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="provider">The <see cref="FileProviders.IFileProvider"/> to use to access the file.</param>
        /// <param name="path">Path relative to the base path stored in
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="optional">Whether the file is optional.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public IConfigurationBuilder AddYamlFile(FileProviders.IFileProvider? provider, string path, bool optional, bool reloadOnChange)
        {
            ArgumentNullException.ThrowIfNull(builder);

            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(Altemiq.Extensions.Configuration.Yaml.Properties.Resources.Error_InvalidFilePath, nameof(path));
            }

            return builder.AddYamlFile(s =>
            {
                s.FileProvider = provider;
                s.Path = path;
                s.Optional = optional;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        /// <summary>
        /// Adds a YAML configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public IConfigurationBuilder AddYamlFile(Action<Altemiq.Extensions.Configuration.Yaml.YamlConfigurationSource>? configureSource)
            => builder.Add(configureSource);

        /// <summary>
        /// Adds a YAML configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to read the yaml configuration data from.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public IConfigurationBuilder AddYamlStream(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(builder);
            return builder.Add<Altemiq.Extensions.Configuration.Yaml.YamlStreamConfigurationSource>(s => s.Stream = stream);
        }
    }
}