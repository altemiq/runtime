// -----------------------------------------------------------------------
// <copyright file="StructConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.Converters;

/// <summary>
/// The <see cref="Struct"/> converter.
/// </summary>
public class StructConverter : System.Text.Json.Serialization.JsonConverter<Struct>
{
    /// <summary>
    /// Gets the instance.
    /// </summary>
    public static readonly StructConverter Instance = new();

    /// <summary>
    /// Creates a <see cref="Struct"/> from a <see cref="JsonDocument"/> instance.
    /// </summary>
    /// <param name="document">The <see cref="JsonDocument"/> instance.</param>
    /// <returns>The <see cref="Struct"/> instance.</returns>
    public static Struct? ToStruct(JsonDocument? document)
    {
        if (document is null
            || document.RootElement is { ValueKind: JsonValueKind.Null })
        {
            return default;
        }

        // convert this to a `google.protobuf.Struct`
        return document.RootElement.ToValue() switch
        {
            { StructValue: { } structValue } => structValue,
            var v => new Struct { Fields = { { "root", v } } },
        };
    }

    /// <summary>
    /// Creates a <see cref="JsonDocument"/> from a <see cref="Struct"/> instance.
    /// </summary>
    /// <param name="struct">The <see cref="Struct"/> instance.</param>
    /// <returns>The <see cref="JsonDocument"/> instance.</returns>
    public static JsonDocument? ToJsonDocument(Struct? @struct) => @struct is null ? default : JsonDocument.Parse(@struct.ToString());

    /// <inheritdoc/>
    public override Struct? Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType is JsonTokenType.StartObject)
        {
            var @struct = new Struct();

            while (reader.Read() && reader.TokenType is not JsonTokenType.EndObject)
            {
                if (reader.TokenType is JsonTokenType.PropertyName)
                {
                    var propertyName = reader.GetString();

                    if (reader.Read())
                    {
                        @struct.Fields.Add(propertyName, ValueConverter.Instance.Read(ref reader, typeToConvert, options));
                    }
                }
            }

            return @struct;
        }

        return default;
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Struct value, JsonSerializerOptions options)
    {
        if (value is null)
        {
            writer.WriteNullValue();
            return;
        }

        writer.WriteStartObject();

        foreach (var field in value.Fields)
        {
            writer.WritePropertyName(field.Key);
            ValueConverter.Instance.Write(writer, field.Value, options);
        }

        writer.WriteEndObject();
    }
}