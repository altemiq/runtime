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
internal sealed class RuntimeConfig(string path)
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
            TryGetFromProperty(runtimeOptions, "tfm", tfm =>
            {
                if (tfm.GetString() is { } v)
                {
                    runtimeConfig.WithTfm(v);
                }
            });
            TryGetFromProperty(runtimeOptions, "framework", framework => runtimeConfig.WithFramework(Framework.FromJson(framework)));
            TryGetFromProperty(runtimeOptions, "frameworks", frameworks => ForEach(frameworks.EnumerateArray(), framework => runtimeConfig.WithFramework(Framework.FromJson(framework))));
            TryGetFromProperty(runtimeOptions, "includedFrameworks", includedFrameworks => ForEach(includedFrameworks.EnumerateArray(), includedFramework => runtimeConfig.WithIncludedFramework(Framework.FromJson(includedFramework))));
            TryGetFromProperty(runtimeOptions, "configProperties", configProperties => ForEach(configProperties.EnumerateObject(), property =>
            {
                var value = property.Value switch
                {
                    { ValueKind: System.Text.Json.JsonValueKind.True } => "true",
                    { ValueKind: System.Text.Json.JsonValueKind.False } => "false",
                    var v => v.ToString(),
                };

                _ = runtimeConfig.WithProperty(property.Name, value);
            }));
            TryGetFromProperty(runtimeOptions, Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward => runtimeConfig.rollForward = rollForward.GetString());
            TryGetFromProperty(runtimeOptions, Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx => runtimeConfig.rollForwardOnNoCandidateFx = rollForwardOnNoCandidateFx.GetInt32());
            TryGetFromProperty(runtimeOptions, Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches => runtimeConfig.applyPatches = applyPatches.GetBoolean());

            static void TryGetFromProperty(System.Text.Json.JsonElement element, string name, Action<System.Text.Json.JsonElement> action)
            {
                if (element.TryGetProperty(name, out var property))
                {
                    action(property);
                }
            }

            static void ForEach<T>(IEnumerable<T> source, Action<T> action)
            {
                foreach (var item in source)
                {
                    action(item);
                }
            }
#else
            using var textReader = File.OpenText(path);
            using var reader = new Newtonsoft.Json.JsonTextReader(textReader) { MaxDepth = null };
            var root = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.Linq.JToken.ReadFrom(reader);
            var runtimeOptions = (Newtonsoft.Json.Linq.JObject)root["runtimeOptions"];
            TryGetFromProperty<Newtonsoft.Json.Linq.JToken>(runtimeOptions, "tfm", tfm => runtimeConfig.WithTfm((string)tfm));
            TryGetFromProperty<Newtonsoft.Json.Linq.JObject>(runtimeOptions, "framework", singleFramework => runtimeConfig.WithFramework(Framework.FromJson(singleFramework)));
            TryGetFromProperty<Newtonsoft.Json.Linq.JToken>(runtimeOptions, "frameworks", frameworksToken => ForEach<Newtonsoft.Json.Linq.JObject>(frameworksToken, framework => runtimeConfig.WithFramework(Framework.FromJson(framework))));
            TryGetFromProperty<Newtonsoft.Json.Linq.JToken>(runtimeOptions, "includedFrameworks", includedFrameworksToken => ForEach<Newtonsoft.Json.Linq.JObject>(includedFrameworksToken, includedFramework => runtimeConfig.WithFramework(Framework.FromJson(includedFramework))));
            TryGetFromProperty<Newtonsoft.Json.Linq.JObject>(runtimeOptions, "configProperties", configProperties =>
            {
                foreach (var property in configProperties)
                {
                    runtimeConfig.WithProperty(property.Key, (string)property.Value);
                }
            });
            TryGetFromProperty<Newtonsoft.Json.Linq.JToken>(runtimeOptions, Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward => runtimeConfig.rollForward = (string)rollForward);
            TryGetFromProperty<Newtonsoft.Json.Linq.JToken>(runtimeOptions, Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx => runtimeConfig.rollForwardOnNoCandidateFx = (int?)rollForwardOnNoCandidateFx);
            TryGetFromProperty<Newtonsoft.Json.Linq.JToken>(runtimeOptions, Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches => runtimeConfig.applyPatches = (bool?)applyPatches);

            static void TryGetFromProperty<T>(Newtonsoft.Json.Linq.JObject @object, string name, Action<T> action)
                where T : Newtonsoft.Json.Linq.JToken
            {
                if (@object[name] is T value)
                {
                    action(value);
                }
            }

            static void ForEach<T>(IEnumerable<Newtonsoft.Json.Linq.JToken> source, Action<T> action)
                where T : Newtonsoft.Json.Linq.JToken
            {
                foreach (var item in source.OfType<T>())
                {
                    action(item);
                }
            }
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

    /// <inheritdoc/>
    public override string ToString()
    {
#if NET461_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        using var stream = new MemoryStream();
        using (var writer = new System.Text.Json.Utf8JsonWriter(stream, new System.Text.Json.JsonWriterOptions { Indented = true }))
        {
            writer.WriteStartObject();
            writer.WritePropertyName("runtimeOptions");
            writer.WriteStartObject();
            WriteNull(writer, this.Tfm, (writer, tfm) => writer.WriteString("tfm", tfm));
            WriteArray(writer, "framework", this.frameworks, (writer, framework) => framework.Save(writer));
            WriteArray(writer, "includedFramework", this.includedFrameworks, (writer, framework) => framework.Save(writer));
            WriteNull(writer, this.rollForward, (writer, rollForward) => writer.WriteString(Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward));
            WriteNullable(writer, this.rollForwardOnNoCandidateFx, (writer, rollForwardOnNoCandidateFx) => writer.WriteNumber(Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx));
            WriteNullable(writer, this.applyPatches, (writer, applyPatches) => writer.WriteBoolean(Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches));
            WriteArray(writer, Constants.AdditionalProbingPath.RuntimeConfigPropertyName, this.additionalProbingPaths, (writer, additionalProbingPath) => writer.WriteStringValue(additionalProbingPath));
            WriteObject(writer, "configProperties", this.properties, (writer, value) =>
            {
                if (bool.TryParse(value, out var result))
                {
                    writer.WriteBooleanValue(result);
                }
                else
                {
                    writer.WriteStringValue(value);
                }
            });
            writer.WriteEndObject();
            writer.WriteEndObject();

            static void WriteArray<T>(System.Text.Json.Utf8JsonWriter writer, string name, List<T> values, Action<System.Text.Json.Utf8JsonWriter, T> write)
            {
                if (values.Count is 0)
                {
                    return;
                }

                if (values.Count is 1)
                {
                    writer.WritePropertyName(name);
                    write(writer, values[0]);
                }
                else
                {
                    writer.WritePropertyName(name + "s");
                    writer.WriteStartArray();
                    foreach (var value in values)
                    {
                        write(writer, value);
                    }

                    writer.WriteEndArray();
                }
            }

            static void WriteObject(System.Text.Json.Utf8JsonWriter writer, string name, List<Tuple<string, string?>> values, Action<System.Text.Json.Utf8JsonWriter, string?> write)
            {
                if (values.Count is 0)
                {
                    return;
                }

                writer.WritePropertyName(name);
                writer.WriteStartObject();

                foreach (var value in values)
                {
                    writer.WritePropertyName(value.Item1);
                    write(writer, value.Item2);
                }

                writer.WriteEndObject();
            }

            static void WriteNull<T>(System.Text.Json.Utf8JsonWriter writer, T? value, Action<System.Text.Json.Utf8JsonWriter, T> write)
                where T : class
            {
                if (value is T v)
                {
                    write(writer, v);
                }
            }

            static void WriteNullable<T>(System.Text.Json.Utf8JsonWriter writer, T? value, Action<System.Text.Json.Utf8JsonWriter, T> write)
                where T : struct
            {
                if (value.HasValue)
                {
                    write(writer, value.Value);
                }
            }
        }

        stream.Position = 0;

        return System.Text.Encoding.UTF8.GetString(stream.ToArray());
#else
        Newtonsoft.Json.Linq.JObject runtimeOptions = [];
        WriteNull(this.Tfm, tfm => runtimeOptions.Add("tfm", tfm));
        WriteArray(runtimeOptions, "framework", this.frameworks, framework => framework.ToJson());
        WriteArray(runtimeOptions, "includedFramework", this.includedFrameworks, framework => framework.ToJson());
        WriteNull(this.rollForward, rollForward => runtimeOptions.Add(Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward));
        WriteNullable(this.rollForwardOnNoCandidateFx, rollForwardOnNoCandidateFx => runtimeOptions.Add(Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx));
        WriteNullable(this.applyPatches, applyPatches => runtimeOptions.Add(Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches));
        WriteArray(runtimeOptions, "additionalProbingPath", this.additionalProbingPaths, additionalProbingPath => new Newtonsoft.Json.Linq.JValue(additionalProbingPath));
        WriteObject(runtimeOptions, "configProperties", this.properties, property =>
        {
            Newtonsoft.Json.Linq.JToken tokenValue = bool.TryParse(property, out var result)
                ? result
                : property;
            return tokenValue;
        });

        var json = new Newtonsoft.Json.Linq.JObject
        {
            ["runtimeOptions"] = runtimeOptions,
        };

        return json.ToString();

        static void WriteArray<T>(Newtonsoft.Json.Linq.JObject runtimeOptions, string name, List<T> values, Func<T, Newtonsoft.Json.Linq.JToken> write)
        {
            if (values.Count is 0)
            {
                return;
            }

            if (values.Count is 1)
            {
                runtimeOptions.Add(name, write(values[0]));
            }
            else
            {
                runtimeOptions.Add(name + "s", new Newtonsoft.Json.Linq.JArray(values.Select(write).ToArray()));
            }
        }

        static void WriteObject(Newtonsoft.Json.Linq.JObject runtimeOptions, string name, List<Tuple<string, string?>> values, Func<string?, Newtonsoft.Json.Linq.JToken> write)
        {
            Newtonsoft.Json.Linq.JObject @object = [];
            foreach (var value in values)
            {
                @object.Add(value.Item1, write(value.Item2));
            }

            runtimeOptions.Add(name, @object);
        }

        static void WriteNull<T>(T? value, Action<T> write)
            where T : class
        {
            if (value is T t)
            {
                write(t);
            }
        }

        static void WriteNullable<T>(T? value, Action<T> write)
            where T : struct
        {
            if (value.HasValue)
            {
                write(value.Value);
            }
        }
#endif
    }

    /// <summary>
    /// Saves this instance.
    /// </summary>
    public void Save() => File.WriteAllText(path, this.ToString());

    /// <summary>
    /// The framework class.
    /// </summary>
    /// <remarks>
    /// Initialises a new instance of the <see cref="Framework"/> class.
    /// </remarks>
    /// <param name="name">The name.</param>
    /// <param name="version">The version.</param>
    public sealed class Framework(string name, string version)
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
            TryGetFromProperty(element, Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward => framework.RollForward = rollForward.GetString());
            TryGetFromProperty(element, Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx => framework.RollForwardOnNoCandidateFx = rollForwardOnNoCandidateFx.GetInt32());
            TryGetFromProperty(element, Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches => framework.ApplyPatches = applyPatches.GetBoolean());
            return framework;

            static void TryGetFromProperty(System.Text.Json.JsonElement element, string name, Action<System.Text.Json.JsonElement> action)
            {
                if (element.TryGetProperty(name, out var property))
                {
                    action(property);
                }
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <param name="writer">The writer to save to.</param>
        internal void Save(System.Text.Json.Utf8JsonWriter writer)
        {
            writer.WriteStartObject();
            WriteNull(this.Name, name => writer.WriteString("name", name));
            WriteNull(this.Version, version => writer.WriteString("version", version));
            WriteNull(this.RollForward, rollForward => writer.WriteString(Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward));
            WriteNullable(this.RollForwardOnNoCandidateFx, rollForwardOnNoCandidateFx => writer.WriteNumber(Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx));
            WriteNullable(this.ApplyPatches, applyPatches => writer.WriteBoolean(Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches));
            writer.WriteEndObject();

            static void WriteNull<T>(T? value, Action<T> write)
                where T : class
            {
                if (value is T v)
                {
                    write(v);
                }
            }

            static void WriteNullable<T>(T? value, Action<T> write)
                where T : struct
            {
                if (value.HasValue)
                {
                    write(value.Value);
                }
            }
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

            WriteNull(this.Name, name => frameworkReference.Add("name", name));
            WriteNull(this.Version, versionToWrite => frameworkReference.Add("version", versionToWrite));
            WriteNull(this.RollForward, rollForward => frameworkReference.Add(Constants.RollForwardSetting.RuntimeConfigPropertyName, rollForward));
            WriteNullable(this.RollForwardOnNoCandidateFx, rollForwardOnNoCandidateFx => frameworkReference.Add(Constants.RollForwardOnNoCandidateFxSetting.RuntimeConfigPropertyName, rollForwardOnNoCandidateFx));
            WriteNullable(this.ApplyPatches, applyPatches => frameworkReference.Add(Constants.ApplyPatchesSetting.RuntimeConfigPropertyName, applyPatches));

            return frameworkReference;

            static void WriteNull<T>(T? value, Action<T> write)
                where T : class
            {
                if (value is T t)
                {
                    write(t);
                }
            }

            static void WriteNullable<T>(T? value, Action<T> write)
                where T : struct
            {
                if (value.HasValue)
                {
                    write(value.Value);
                }
            }
        }
#endif
    }
}