// -----------------------------------------------------------------------
// <copyright file="ValueConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.Converters;

/// <summary>
/// The <see cref="Value"/> converter.
/// </summary>
public class ValueConverter : System.Text.Json.Serialization.JsonConverter<Value>
{
    /// <summary>
    /// Gets the instance.
    /// </summary>
    public static readonly ValueConverter Instance = new();

    /// <summary>
    /// Creates a <see cref="Value"/> from a <see cref="JsonElement"/> instance.
    /// </summary>
    /// <param name="element">The <see cref="JsonElement"/> instance.</param>
    /// <returns>The <see cref="Value"/> instance.</returns>
    public static Value ToValue(JsonElement element)
    {
        return element switch
        {
            { ValueKind: JsonValueKind.Array } @array => ToList(@array),
            { ValueKind: JsonValueKind.Object } @object => ToStruct(@object),
            { ValueKind: JsonValueKind.String } @string => Value.ForString(@string.GetString()),
            { ValueKind: JsonValueKind.Null } => Value.ForNull(),
            { ValueKind: JsonValueKind.True } => Value.ForBool(value: true),
            { ValueKind: JsonValueKind.False } => Value.ForBool(value: false),
            { ValueKind: JsonValueKind.Number } @number => Value.ForNumber(@number.GetDouble()),
            _ => throw new InvalidCastException(),
        };

        static Value ToList(JsonElement element)
        {
            return Value.ForList([.. element.EnumerateArray().Select(item => item.ToValue())]);
        }

        static Value ToStruct(JsonElement element)
        {
            var @struct = new Struct();
            foreach (var item in element.EnumerateObject())
            {
                @struct.Fields[item.Name] = item.Value.ToValue();
            }

            return Value.ForStruct(@struct);
        }
    }

    /// <summary>
    /// Creates a <see cref="Value"/> from a <see cref="JsonNode"/> instance.
    /// </summary>
    /// <param name="node">The <see cref="JsonNode"/> instance.</param>
    /// <returns>The <see cref="Value"/> instance.</returns>
    public static Value ToValue(JsonNode? node)
    {
        return (node, node?.GetValueKind() ?? JsonValueKind.Undefined) switch
        {
            (JsonArray array, _) => ToList(array),
            (JsonObject @object, _) => ToStruct(@object),
            (null, _) or (_, JsonValueKind.Null) => Value.ForNull(),
            (JsonValue, JsonValueKind.False) => Value.ForBool(value: false),
            (JsonValue, JsonValueKind.True) => Value.ForBool(value: true),
            (JsonValue value, JsonValueKind.String) => value.GetValue<string?>() is { } stringValue ? Value.ForString(stringValue) : Value.ForNull(),
            (JsonValue value, JsonValueKind.Number) => Value.ForNumber(value.GetValue<double>()),
            _ => throw new InvalidCastException(),
        };

        static Value ToList(JsonArray array)
        {
            return Value.ForList([.. array.Select(ToValue)]);
        }

        static Value ToStruct(JsonObject @object)
        {
            var @struct = new Struct();
            foreach (var item in @object)
            {
                @struct.Fields[item.Key] = ToValue(item.Value);
            }

            return Value.ForStruct(@struct);
        }
    }

    /// <summary>
    /// Creates a <see cref="JsonElement"/> from a <see cref="Value"/> instance.
    /// </summary>
    /// <param name="value">The <see cref="Value"/> instance.</param>
    /// <returns>The <see cref="JsonDocument"/> instance.</returns>
    public static JsonElement ToJsonElement(Value value) => JsonDocument.Parse(value.ToString()).RootElement;

    /// <summary>
    /// Creates a <see cref="JsonNode"/> from a <see cref="Value"/> instance.
    /// </summary>
    /// <param name="value">The <see cref="Value"/> instance.</param>
    /// <returns>The <see cref="JsonDocument"/> instance.</returns>
    public static JsonNode? ToJsonNode(Value value)
    {
        return value.KindCase switch
        {
            Value.KindOneofCase.NullValue => default,
            Value.KindOneofCase.BoolValue => value.BoolValue,
            Value.KindOneofCase.NumberValue => value.NumberValue,
            Value.KindOneofCase.StringValue => value.StringValue,
            Value.KindOneofCase.StructValue => FromStruct(value.StructValue),
            Value.KindOneofCase.ListValue => FromList(value.ListValue),
            Value.KindOneofCase.None => throw new InvalidOperationException(),
            _ => throw new InvalidCastException(),
        };

        static JsonObject FromStruct(Struct @struct)
        {
            var node = new JsonObject();

            foreach (var field in @struct.Fields)
            {
                node.Add(field.Key, ToJsonNode(field.Value));
            }

            return node;
        }

        static JsonArray FromList(ListValue list)
        {
            var node = new JsonArray();

            foreach (var item in list.Values)
            {
                node.Add(ToJsonNode(item));
            }

            return node;
        }
    }

    /// <inheritdoc/>
    public override Value? Read(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
    {
        // read the value
        return reader.TokenType switch
        {
            JsonTokenType.Null => Value.ForNull(),
            JsonTokenType.True => Value.ForBool(value: true),
            JsonTokenType.False => Value.ForBool(value: false),
            JsonTokenType.String => Value.ForString(reader.GetString()),
            JsonTokenType.Number => Value.ForNumber(reader.GetDouble()),
            JsonTokenType.StartObject => Value.ForStruct(StructConverter.Instance.Read(ref reader, typeToConvert, options)),
            JsonTokenType.StartArray => ReadArray(ref reader, typeToConvert, options),
            _ => throw new InvalidOperationException(),
        };

        static Value ReadArray(ref Utf8JsonReader reader, System.Type typeToConvert, JsonSerializerOptions options)
        {
            var values = new List<Value?>();
            while (reader.Read() && reader.TokenType is not JsonTokenType.EndArray)
            {
                values.Add(Instance.Read(ref reader, typeToConvert, options));
            }

            return Value.ForList([.. values]);
        }
    }

    /// <inheritdoc/>
    public override void Write(Utf8JsonWriter writer, Value value, JsonSerializerOptions options)
    {
        switch (value.KindCase)
        {
            case Value.KindOneofCase.NullValue:
                writer.WriteNullValue();
                break;

            case Value.KindOneofCase.BoolValue:
                writer.WriteBooleanValue(value.BoolValue);
                break;

            case Value.KindOneofCase.NumberValue:
                writer.WriteNumberValue(value.NumberValue);
                break;

            case Value.KindOneofCase.StringValue:
                writer.WriteStringValue(value.StringValue);
                break;

            case Value.KindOneofCase.StructValue:
                StructConverter.Instance.Write(writer, value.StructValue, options);
                break;

            case Value.KindOneofCase.ListValue:
                writer.WriteStartArray();

                foreach (var listItem in value.ListValue.Values)
                {
                    this.Write(writer, listItem, options);
                }

                writer.WriteEndArray();
                break;
        }
    }
}