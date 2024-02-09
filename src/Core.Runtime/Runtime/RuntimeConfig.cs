// -----------------------------------------------------------------------
// <copyright file="RuntimeConfig.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

/// <summary>
/// Initialises a new instance of the <see cref="RuntimeConfig"/> class.
/// </summary>
/// <param name="path">The path.</param>
/// <remarks>
/// Creates new runtime config - overwrites existing file on Save if any.
/// </remarks>
internal class RuntimeConfig(string path)
{
    private readonly List<Framework> frameworks = [];
    private readonly List<Framework> includedFrameworks = [];
    private readonly List<Tuple<string, string?>> properties = [];
    private readonly List<string> additionalProbingPaths = [];

    private string? rollForward;
    private int? rollForwardOnNoCandidateFx;
    private bool? applyPatches;

    /// <summary>
    /// Gets the target framework moniker.
    /// </summary>
    public string? Tfm { get; private set; }

    /// <summary>
    /// Creates the object over existing file - reading its content.
    /// Save should recreate the file assuming we can store all the values in it in this class.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The runtime configuration.</returns>
    public static RuntimeConfig FromFile(string path)
    {
        RuntimeConfig runtimeConfig = new(path);
        if (File.Exists(path))
        {
#if NET461_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            var document = System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
            var root = document.RootElement;
            var runtimeOptions = root.GetProperty("runtimeOptions");
            if (runtimeOptions.TryGetProperty("framework", out var singleFramework))
            {
                _ = runtimeConfig.WithFramework(Framework.FromJson(singleFramework));
            }

            if (runtimeOptions.TryGetProperty("frameworks", out var frameworksElement))
            {
                foreach (var framework in frameworksElement.EnumerateArray())
                {
                    _ = runtimeConfig.WithFramework(Framework.FromJson(framework));
                }
            }

            if (runtimeOptions.TryGetProperty("includedFrameworks", out var includedFrameworksElement))
            {
                foreach (var includedFramework in includedFrameworksElement.EnumerateArray())
                {
                    _ = runtimeConfig.WithIncludedFramework(Framework.FromJson(includedFramework));
                }
            }

            if (runtimeOptions.TryGetProperty("configProperties", out var configProperties))
            {
                foreach (var property in configProperties.EnumerateObject())
                {
                    var value = property.Value switch
                    {
                        { ValueKind: System.Text.Json.JsonValueKind.True } => "true",
                        { ValueKind: System.Text.Json.JsonValueKind.False } => "false",
                        var v => v.ToString(),
                    };

                    _ = runtimeConfig.WithProperty(property.Name, value);
                }
            }

            if (runtimeOptions.TryGetProperty(Constants.RollForwardSetting.RuntimeConfigPropertyName, out var rollForwardElement))
            {
                runtimeConfig.rollForward = rollForwardElement.GetString();
            }

            if (runtimeOptions.TryGetProperty(Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, out var rollForwardOnNoCandidateFxElement))
            {
                runtimeConfig.rollForwardOnNoCandidateFx = rollForwardOnNoCandidateFxElement.GetInt32();
            }

            if (runtimeOptions.TryGetProperty(Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, out var applyPatchesElement))
            {
                runtimeConfig.applyPatches = applyPatchesElement.GetBoolean();
            }
#else
            using var textReader = File.OpenText(path);
            using var reader = new Newtonsoft.Json.JsonTextReader(textReader) { MaxDepth = null };
            var root = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
            var runtimeOptions = (Newtonsoft.Json.Linq.JObject)root["runtimeOptions"];
            if (runtimeOptions["framework"] is Newtonsoft.Json.Linq.JObject singleFramework)
            {
                runtimeConfig.WithFramework(Framework.FromJson(singleFramework));
            }

            if (runtimeOptions["frameworks"] is { } frameworksToken)
            {
                foreach (var framework in frameworksToken.OfType<Newtonsoft.Json.Linq.JObject>())
                {
                    runtimeConfig.WithFramework(Framework.FromJson(framework));
                }
            }

            if (runtimeOptions["includedFrameworks"] is { } includedFrameworksToken)
            {
                foreach (var includedFramework in includedFrameworksToken.OfType<Newtonsoft.Json.Linq.JObject>())
                {
                    runtimeConfig.WithFramework(Framework.FromJson(includedFramework));
                }
            }

            if (runtimeOptions["configProperties"] is Newtonsoft.Json.Linq.JObject configProperties)
            {
                foreach (var property in configProperties)
                {
                    runtimeConfig.WithProperty(property.Key, (string)property.Value);
                }
            }

            runtimeConfig.rollForward = (string)runtimeOptions[Constants.RollForwardSetting.RuntimeConfigPropertyName];
            runtimeConfig.rollForwardOnNoCandidateFx = (int?)runtimeOptions[Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName];
            runtimeConfig.applyPatches = (bool?)runtimeOptions[Constants.ApplyPatchesSetting.RuntimeConfigPropertyName];
#endif
        }

        return runtimeConfig;
    }

    /// <summary>
    /// Creates a <see cref="RuntimeConfig"/> from the specified path.
    /// </summary>
    /// <param name="path">The path.</param>
    /// <returns>The runtime config.</returns>
    public static RuntimeConfig Path(string path) => new(path);

    /// <summary>
    /// Gets the framework.
    /// </summary>
    /// <param name="name">The framework name.</param>
    /// <returns>The framework.</returns>
    public Framework? GetFramework(string name) => this.frameworks.Find(f => string.Equals(f.Name, name, StringComparison.Ordinal));

    /// <summary>
    /// Gets the property value.
    /// </summary>
    /// <param name="name">The name of the property.</param>
    /// <returns>The property value.</returns>
    public string? GetPropertyValue(string name) => this.properties.Find(p => string.Equals(p.Item1, name, StringComparison.Ordinal)) is { } tuple ? tuple.Item2 : default;

    /// <summary>
    /// Sets the framework.
    /// </summary>
    /// <param name="framework">The framework to set.</param>
    /// <returns>This instance with the framework set.</returns>
    public RuntimeConfig WithFramework(Framework framework)
    {
        this.frameworks.Add(framework);
        return this;
    }

    /// <summary>
    /// Sets the framework.
    /// </summary>
    /// <param name="name">The name to set.</param>
    /// <param name="version">The version to set.</param>
    /// <returns>This instance with the framework set.</returns>
    public RuntimeConfig WithFramework(string name, string version) => this.WithFramework(new Framework(name, version));

    /// <summary>
    /// Removes the framework.
    /// </summary>
    /// <param name="name">The framework to remove.</param>
    /// <returns>This instance with the framework removed.</returns>
    public RuntimeConfig RemoveFramework(string name)
    {
        if (this.GetFramework(name) is { } framework)
        {
            _ = this.frameworks.Remove(framework);
        }

        return this;
    }

    /// <summary>
    /// Gets the included framework.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The framework, if included.</returns>
    public Framework? GetIncludedFramework(string name) => this.includedFrameworks.Find(f => string.Equals(f.Name, name, StringComparison.Ordinal));

    /// <summary>
    /// Sets the included framework.
    /// </summary>
    /// <param name="framework">The framework to set.</param>
    /// <returns>This instance with the included framework set.</returns>
    public RuntimeConfig WithIncludedFramework(Framework framework)
    {
        this.includedFrameworks.Add(framework);
        return this;
    }

    /// <summary>
    /// Sets the included framework.
    /// </summary>
    /// <param name="name">The name to set.</param>
    /// <param name="version">The version to set.</param>
    /// <returns>This instance with the included framework set.</returns>
    public RuntimeConfig WithIncludedFramework(string name, string version) => this.WithIncludedFramework(new Framework(name, version));

    /// <summary>
    /// Sets the roll forward.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>This instance with roll forward set.</returns>
    public RuntimeConfig WithRollForward(string value)
    {
        this.rollForward = value;
        return this;
    }

    /// <summary>
    /// Sets the roll forward with no candidate framework.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>This instance with roll forward set.</returns>
    public RuntimeConfig WithRollForwardOnNoCandidateFx(int? value)
    {
        this.rollForwardOnNoCandidateFx = value;
        return this;
    }

    /// <summary>
    /// Sets the apply patches.
    /// </summary>
    /// <param name="value">The value to set.</param>
    /// <returns>This instance with apply patches set.</returns>
    public RuntimeConfig WithApplyPatches(bool? value)
    {
        this.applyPatches = value;
        return this;
    }

    /// <summary>
    /// Sets the TFM.
    /// </summary>
    /// <param name="tfm">The value to set.</param>
    /// <returns>This instance with TFM set.</returns>
    public RuntimeConfig WithTfm(string tfm)
    {
        this.Tfm = tfm;
        return this;
    }

    /// <summary>
    /// Sets the additional probing path.
    /// </summary>
    /// <param name="path">The additional probing path to set.</param>
    /// <returns>This instance with the additional probing path set.</returns>
    public RuntimeConfig WithAdditionalProbingPath(string path)
    {
        this.additionalProbingPaths.Add(path);
        return this;
    }

    /// <summary>
    /// Sets the property.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns>This instance with the property set.</returns>
    public RuntimeConfig WithProperty(string name, string? value)
    {
        this.properties.Add(new Tuple<string, string?>(name, value));
        return this;
    }

    /// <summary>
    /// Saves this instance.
    /// </summary>
    public void Save()
    {
#if NET461_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        using var stream = new MemoryStream();
        using (var writer = new System.Text.Json.Utf8JsonWriter(stream))
        {
            writer.WriteStartObject();

            writer.WritePropertyName("runtimeOptions");

            writer.WriteStartObject();

            if (this.frameworks.Count != 0)
            {
                writer.WritePropertyName("frameworks");
                writer.WriteStartArray();
                foreach (var framework in this.frameworks)
                {
                    framework.Save(writer);
                }

                writer.WriteEndArray();
            }

            if (this.includedFrameworks.Count != 0)
            {
                writer.WritePropertyName("includedFrameworks");
                writer.WriteStartArray();
                foreach (var framework in this.includedFrameworks)
                {
                    framework.Save(writer);
                }

                writer.WriteEndArray();
            }

            if (this.rollForward != null)
            {
                writer.WriteString(
                    Constants.RollForwardSetting.RuntimeConfigPropertyName,
                    this.rollForward);
            }

            if (this.rollForwardOnNoCandidateFx.HasValue)
            {
                writer.WriteNumber(
                    Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName,
                    this.rollForwardOnNoCandidateFx.Value);
            }

            if (this.applyPatches.HasValue)
            {
                writer.WriteBoolean(
                    Constants.ApplyPatchesSetting.RuntimeConfigPropertyName,
                    this.applyPatches.Value);
            }

            if (this.Tfm is not null)
            {
                writer.WriteString("tfm", this.Tfm);
            }

            if (this.additionalProbingPaths.Count > 0)
            {
                writer.WritePropertyName(Constants.AdditionalProbingPath.RuntimeConfigPropertyName);
                writer.WriteStartArray();
                foreach (var additionalProbingPath in this.additionalProbingPaths)
                {
                    writer.WriteStringValue(additionalProbingPath);
                }

                writer.WriteEndArray();
            }

            if (this.properties.Count > 0)
            {
                writer.WritePropertyName("configProperties");
                writer.WriteStartObject();

                foreach (var property in this.properties)
                {
                    writer.WritePropertyName(property.Item1);
                    if (bool.TryParse(property.Item2, out var result))
                    {
                        writer.WriteBooleanValue(result);
                    }
                    else
                    {
                        writer.WriteStringValue(property.Item2);
                    }
                }

                writer.WriteEndObject();
            }

            writer.WriteEndObject();
            writer.WriteEndObject();
        }

        stream.Position = 0;

        File.WriteAllBytes(path, stream.ToArray());
#else
        Newtonsoft.Json.Linq.JObject runtimeOptions = [];
        if (this.frameworks.Count != 0)
        {
            runtimeOptions.Add(
                "frameworks",
                new Newtonsoft.Json.Linq.JArray(this.frameworks.Select(f => f.ToJson()).ToArray()));
        }

        if (this.includedFrameworks.Count != 0)
        {
            runtimeOptions.Add(
                "includedFrameworks",
                new Newtonsoft.Json.Linq.JArray(this.includedFrameworks.Select(f => f.ToJson()).ToArray()));
        }

        if (this.rollForward != null)
        {
            runtimeOptions.Add(
                Constants.RollForwardSetting.RuntimeConfigPropertyName,
                this.rollForward);
        }

        if (this.rollForwardOnNoCandidateFx.HasValue)
        {
            runtimeOptions.Add(
                Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName,
                this.rollForwardOnNoCandidateFx.Value);
        }

        if (this.applyPatches.HasValue)
        {
            runtimeOptions.Add(
                Constants.ApplyPatchesSetting.RuntimeConfigPropertyName,
                this.applyPatches.Value);
        }

        if (this.Tfm is not null)
        {
            runtimeOptions.Add("tfm", this.Tfm);
        }

        if (this.additionalProbingPaths.Count > 0)
        {
            runtimeOptions.Add(
                Constants.AdditionalProbingPath.RuntimeConfigPropertyName,
                new Newtonsoft.Json.Linq.JArray(this.additionalProbingPaths.Select(p => new Newtonsoft.Json.Linq.JValue(p)).ToArray()));
        }

        if (this.properties.Count > 0)
        {
            Newtonsoft.Json.Linq.JObject configProperties = [];
            foreach (var property in this.properties)
            {
                Newtonsoft.Json.Linq.JToken tokenValue = bool.TryParse(property.Item2, out var result)
                    ? result
                    : property.Item2;
                configProperties.Add(property.Item1, tokenValue);
            }

            runtimeOptions.Add("configProperties", configProperties);
        }

        var json = new Newtonsoft.Json.Linq.JObject
        {
            ["runtimeOptions"] = runtimeOptions,
        };

        File.WriteAllText(path, json.ToString());
#endif
    }

    /// <summary>
    /// The framework class.
    /// </summary>
    /// <remarks>
    /// Initialises a new instance of the <see cref="Framework"/> class.
    /// </remarks>
    /// <param name="name">The name.</param>
    /// <param name="version">The version.</param>
    public class Framework(string name, string version)
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; } = name;

        /// <summary>
        /// Gets the version.
        /// </summary>
        public string Version { get; } = version;

        /// <summary>
        /// Gets the roll forward option.
        /// </summary>
        public string? RollForward { get; private set; }

        /// <summary>
        /// Gets the framework for when there is not candidate.
        /// </summary>
        public int? RollForwardOnNoCandidateFx { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether to apply patches.
        /// </summary>
        public bool? ApplyPatches { get; set; }

        /// <summary>
        /// Sets the roll forward.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>This instance with roll forward set.</returns>
        public Framework WithRollForward(string value)
        {
            this.RollForward = value;
            return this;
        }

        /// <summary>
        /// Sets the roll forward with no candidate framework.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>This instance with roll forward set.</returns>
        public Framework WithRollForwardOnNoCandidateFx(int? value)
        {
            this.RollForwardOnNoCandidateFx = value;
            return this;
        }

        /// <summary>
        /// Sets the apply patches.
        /// </summary>
        /// <param name="value">The value to set.</param>
        /// <returns>This instance with apply patches set.</returns>
        public Framework WithApplyPatches(bool? value)
        {
            this.ApplyPatches = value;
            return this;
        }

#if NET461_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        /// <summary>
        /// Reads the framework from the element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns>The framework.</returns>
        internal static Framework FromJson(System.Text.Json.JsonElement element)
        {
            var framework = new Framework(element.GetProperty("name").GetString()!, element.GetProperty("version").GetString()!);
            if (element.TryGetProperty(Constants.RollForwardSetting.RuntimeConfigPropertyName, out var rollForwardElement))
            {
                framework.RollForward = rollForwardElement.GetString();
            }

            if (element.TryGetProperty(Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, out var rollForwardOnNoCandidateFxElement))
            {
                framework.RollForwardOnNoCandidateFx = rollForwardOnNoCandidateFxElement.GetInt32();
            }

            if (element.TryGetProperty(Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, out var applyPatchesElement))
            {
                framework.ApplyPatches = applyPatchesElement.GetBoolean();
            }

            return framework;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="writer">The writer to save to.</param>
        internal void Save(System.Text.Json.Utf8JsonWriter writer)
        {
            writer.WriteStartObject();

            if (this.Name is { } nameToWrite)
            {
                writer.WriteString("name", nameToWrite);
            }

            if (this.Version is { } versionToWrite)
            {
                writer.WriteString("version", versionToWrite);
            }

            if (this.RollForward is { } rollForward)
            {
                writer.WriteString(Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward);
            }

            if (this.RollForwardOnNoCandidateFx is int rollForwardOnNoCandidateFx)
            {
                writer.WriteNumber(Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx);
            }

            if (this.ApplyPatches is bool applyPatches)
            {
                writer.WriteBoolean(Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches);
            }

            writer.WriteEndObject();
        }
#else
        /// <summary>
        /// Reads the framework from the object.
        /// </summary>
        /// <param name="json">The object.</param>
        /// <returns>The framework.</returns>
        internal static Framework FromJson(Newtonsoft.Json.Linq.JObject json) => new((string)json["name"], (string)json["version"])
        {
            RollForward = (string)json[Constants.RollForwardSetting.RuntimeConfigPropertyName],
            RollForwardOnNoCandidateFx = (int?)json[Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName],
            ApplyPatches = (bool?)json[Constants.ApplyPatchesSetting.RuntimeConfigPropertyName],
        };

        /// <summary>
        /// Writes this instance as an object.
        /// </summary>
        /// <returns>The JDON object.</returns>
        internal Newtonsoft.Json.Linq.JObject ToJson()
        {
            Newtonsoft.Json.Linq.JObject frameworkReference = [];

            if (this.Name is { } nameToWrite)
            {
                frameworkReference.Add("name", nameToWrite);
            }

            if (this.Version is { } versionToWrite)
            {
                frameworkReference.Add("version", versionToWrite);
            }

            if (this.RollForward is { } rollForward)
            {
                frameworkReference.Add(
                    Constants.RollForwardSetting.RuntimeConfigPropertyName,
                    rollForward);
            }

            if (this.RollForwardOnNoCandidateFx is { } rollForwardOnNoCandidateFx)
            {
                frameworkReference.Add(
                    Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName,
                    rollForwardOnNoCandidateFx);
            }

            if (this.ApplyPatches is { } applyPatches)
            {
                frameworkReference.Add(
                    Constants.ApplyPatchesSetting.RuntimeConfigPropertyName,
                    applyPatches);
            }

            return frameworkReference;
        }
#endif
    }
}