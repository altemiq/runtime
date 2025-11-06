// -----------------------------------------------------------------------
// <copyright file="YamlConfigurationFileParser.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Configuration.Yaml;

using YamlDotNet.Core.Events;
using YamlDotNet.RepresentationModel;

/// <summary>
/// The YAML file parser.
/// </summary>
internal sealed class YamlConfigurationFileParser
{
    private readonly Dictionary<string, string?> data = new(StringComparer.OrdinalIgnoreCase);
    private readonly Stack<string> paths = new();
    private readonly CustomEmitter customEmitter = new();

    private YamlConfigurationFileParser()
    {
    }

    /// <summary>
    /// Parses the stream.
    /// </summary>
    /// <param name="input">The input.</param>
    /// <returns>The parsed stream.</returns>
    public static IDictionary<string, string?> Parse(Stream input)
        => new YamlConfigurationFileParser().ParseStream(input);

    private Dictionary<string, string?> ParseStream(Stream input)
    {
        using (var reader = new StreamReader(input))
        {
            var scanner = new YamlDotNet.Core.Scanner(reader);
            var parser = new YamlDotNet.Core.Parser(scanner);
            var yaml = new YamlStream();
            yaml.Load(parser);

            if (yaml.Documents.Count is 0)
            {
                throw new YamlDotNet.Core.YamlException("Failed to load documents");
            }

            foreach (var document in yaml.Documents)
            {
                if (document.RootNode is YamlMappingNode mappingNode)
                {
                    this.VisitObjectElement(mappingNode);
                }
                else
                {
                    throw new YamlDotNet.Core.YamlException("Document root node must be a mapping");
                }
            }
        }

        return this.data;
    }

    private void VisitObjectElement(YamlMappingNode node)
    {
        var isEmpty = true;

        // get all the nodes
        foreach (var childNode in node.Children)
        {
            isEmpty = false;
            this.EnterContext(childNode.Key);
            this.VisitValue(childNode.Value);
            this.ExitContext();
        }

        this.SetNullIfElementIsEmpty(isEmpty);
    }

    private void VisitArrayElement(YamlSequenceNode element)
    {
        var index = 0;

        foreach (var child in element.Children)
        {
            this.EnterContext(index.ToString(System.Globalization.CultureInfo.InvariantCulture));
            this.VisitValue(child);
            this.ExitContext();
            index++;
        }

        this.SetEmptyIfElementIsEmpty(isEmpty: index is 0);
    }

    private void SetNullIfElementIsEmpty(bool isEmpty)
    {
        if (isEmpty && this.paths.Count > 0)
        {
            this.data[this.paths.Peek()] = null;
        }
    }

    private void SetEmptyIfElementIsEmpty(bool isEmpty)
    {
        if (isEmpty && this.paths.Count > 0)
        {
            this.data[this.paths.Peek()] = string.Empty;
        }
    }

    private void VisitValue(YamlNode value)
    {
        System.Diagnostics.Debug.Assert(this.paths.Count > 0, "_paths must not be empty");

        switch (value)
        {
            case YamlMappingNode mappingNode:
                this.VisitObjectElement(mappingNode);
                break;
            case YamlSequenceNode sequenceNode:
                this.VisitArrayElement(sequenceNode);
                break;
            case YamlScalarNode scalarNode:
                var key = this.paths.Peek();
                if (this.data.ContainsKey(key))
                {
                    throw new FormatException(Properties.Resources.Error_KeyIsDuplicated);
                }

                var scalarValue = scalarNode.Value;
                if (bool.TryParse(scalarValue, out var boolValue))
                {
                    scalarValue = boolValue ? bool.TrueString : bool.FalseString;
                }

                // see if we should set this to null
                if (scalarNode is { Style: YamlDotNet.Core.ScalarStyle.Plain } and YamlDotNet.Serialization.IYamlConvertible convertable)
                {
                    convertable.Write(this.customEmitter, default!);
                    if (this.customEmitter.Current is Scalar { IsPlainImplicit: true } scalar
                        && (scalar.Tag == YamlDotNet.Serialization.Schemas.JsonSchema.Tags.Null || scalar.Value is "~" or "null"))
                    {
                        scalarValue = null;
                    }
                }

                this.data[key] = scalarValue;
                break;
        }
    }

    private void EnterContext(YamlNode node)
    {
        if (node is YamlScalarNode scalarNode)
        {
            this.EnterContext(scalarNode);
        }
    }

    private void EnterContext(YamlScalarNode scalarNode)
    {
        if (scalarNode.Value is { } value)
        {
            this.EnterContext(value);
        }
    }

    private void EnterContext(string context) => this.paths.Push(this.paths.Count > 0 ? this.paths.Peek() + ConfigurationPath.KeyDelimiter + context : context);

    private void ExitContext() => this.paths.Pop();

    private sealed class CustomEmitter : YamlDotNet.Core.IEmitter
    {
        public ParsingEvent? Current { get; private set; }

        public void Emit(ParsingEvent @event) => this.Current = @event;
    }
}