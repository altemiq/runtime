// -----------------------------------------------------------------------
// <copyright file="ValueConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.Converters;

/// <summary>
/// The <see cref="Google.Protobuf.WellKnownTypes.Value"/> converter.
/// </summary>
public static class ValueConverter
{
    /// <summary>
    /// Creates a <see cref="Google.Protobuf.WellKnownTypes.Value"/> from a <see cref="System.Text.Json.JsonElement"/> instance.
    /// </summary>
    /// <param name="element">The <see cref="System.Text.Json.JsonElement"/> instance.</param>
    /// <returns>The <see cref="Google.Protobuf.WellKnownTypes.Value"/> instance.</returns>
    public static Google.Protobuf.WellKnownTypes.Value ToValue(this System.Text.Json.JsonElement element)
    {
        return element switch
        {
            { ValueKind: System.Text.Json.JsonValueKind.Array } @array => ToList(@array),
            { ValueKind: System.Text.Json.JsonValueKind.Object } @object => ToStruct(@object),
            { ValueKind: System.Text.Json.JsonValueKind.String } @string => Google.Protobuf.WellKnownTypes.Value.ForString(@string.GetString()),
            { ValueKind: System.Text.Json.JsonValueKind.Null } => Google.Protobuf.WellKnownTypes.Value.ForNull(),
            { ValueKind: System.Text.Json.JsonValueKind.True } => Google.Protobuf.WellKnownTypes.Value.ForBool(value: true),
            { ValueKind: System.Text.Json.JsonValueKind.False } => Google.Protobuf.WellKnownTypes.Value.ForBool(value: false),
            { ValueKind: System.Text.Json.JsonValueKind.Number } @number => Google.Protobuf.WellKnownTypes.Value.ForNumber(@number.GetDouble()),
            _ => throw new InvalidCastException(),
        };

        static Google.Protobuf.WellKnownTypes.Value ToList(System.Text.Json.JsonElement element)
        {
            var values = new List<Google.Protobuf.WellKnownTypes.Value>();
            foreach (var item in element.EnumerateArray())
            {
                values.Add(item.ToValue());
            }

            return Google.Protobuf.WellKnownTypes.Value.ForList([.. values]);
        }

        static Google.Protobuf.WellKnownTypes.Value ToStruct(System.Text.Json.JsonElement element)
        {
            var @struct = new Google.Protobuf.WellKnownTypes.Struct();
            foreach (var item in element.EnumerateObject())
            {
                @struct.Fields[item.Name] = item.Value.ToValue();
            }

            return Google.Protobuf.WellKnownTypes.Value.ForStruct(@struct);
        }
    }

    /// <summary>
    /// Creates a <see cref="Google.Protobuf.WellKnownTypes.Value"/> from a <see cref="System.Text.Json.Nodes.JsonNode"/> instance.
    /// </summary>
    /// <param name="node">The <see cref="System.Text.Json.Nodes.JsonNode"/> instance.</param>
    /// <returns>The <see cref="Google.Protobuf.WellKnownTypes.Value"/> instance.</returns>
    public static Google.Protobuf.WellKnownTypes.Value ToValue(this System.Text.Json.Nodes.JsonNode? node)
    {
        return (node, node?.GetValueKind() ?? System.Text.Json.JsonValueKind.Undefined) switch
        {
            (System.Text.Json.Nodes.JsonArray array, _) => ToList(array),
            (System.Text.Json.Nodes.JsonObject @object, _) => ToStruct(@object),
            (null, _) or (_, System.Text.Json.JsonValueKind.Null) => Google.Protobuf.WellKnownTypes.Value.ForNull(),
            (System.Text.Json.Nodes.JsonValue, System.Text.Json.JsonValueKind.False) => Google.Protobuf.WellKnownTypes.Value.ForBool(value: false),
            (System.Text.Json.Nodes.JsonValue, System.Text.Json.JsonValueKind.True) => Google.Protobuf.WellKnownTypes.Value.ForBool(value: true),
            (System.Text.Json.Nodes.JsonValue value, System.Text.Json.JsonValueKind.String) => value.GetValue<string?>() is string stringValue ? Google.Protobuf.WellKnownTypes.Value.ForString(stringValue) : Google.Protobuf.WellKnownTypes.Value.ForNull(),
            (System.Text.Json.Nodes.JsonValue value, System.Text.Json.JsonValueKind.Number) => Google.Protobuf.WellKnownTypes.Value.ForNumber(value.GetValue<double>()),
            _ => throw new InvalidCastException(),
        };

        static Google.Protobuf.WellKnownTypes.Value ToList(System.Text.Json.Nodes.JsonArray array)
        {
            var values = new List<Google.Protobuf.WellKnownTypes.Value?>();
            foreach (var item in array)
            {
                values.Add(item.ToValue());
            }

            return Google.Protobuf.WellKnownTypes.Value.ForList([.. values]);
        }

        static Google.Protobuf.WellKnownTypes.Value ToStruct(System.Text.Json.Nodes.JsonObject @object)
        {
            var @struct = new Google.Protobuf.WellKnownTypes.Struct();
            foreach (var item in @object)
            {
                @struct.Fields[item.Key] = item.Value.ToValue();
            }

            return Google.Protobuf.WellKnownTypes.Value.ForStruct(@struct);
        }
    }

    /// <summary>
    /// Creates a <see cref="System.Text.Json.JsonElement"/> from a <see cref="Google.Protobuf.WellKnownTypes.Value"/> instance.
    /// </summary>
    /// <param name="value">The <see cref="Google.Protobuf.WellKnownTypes.Value"/> instance.</param>
    /// <returns>The <see cref="System.Text.Json.JsonDocument"/> instance.</returns>
    public static System.Text.Json.JsonElement ToJsonElement(Google.Protobuf.WellKnownTypes.Value value) => System.Text.Json.JsonDocument.Parse(value.ToString()).RootElement;

    /// <summary>
    /// Creates a <see cref="System.Text.Json.Nodes.JsonNode"/> from a <see cref="Google.Protobuf.WellKnownTypes.Value"/> instance.
    /// </summary>
    /// <param name="value">The <see cref="Google.Protobuf.WellKnownTypes.Value"/> instance.</param>
    /// <returns>The <see cref="System.Text.Json.JsonDocument"/> instance.</returns>
    public static System.Text.Json.Nodes.JsonNode? ToJsonNode(this Google.Protobuf.WellKnownTypes.Value value)
    {
        return value.KindCase switch
        {
            Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NullValue => default,
            Google.Protobuf.WellKnownTypes.Value.KindOneofCase.BoolValue => value.BoolValue,
            Google.Protobuf.WellKnownTypes.Value.KindOneofCase.NumberValue => value.NumberValue,
            Google.Protobuf.WellKnownTypes.Value.KindOneofCase.StringValue => value.StringValue,
            Google.Protobuf.WellKnownTypes.Value.KindOneofCase.StructValue => FromStruct(value.StructValue),
            Google.Protobuf.WellKnownTypes.Value.KindOneofCase.ListValue => FromList(value.ListValue),
            Google.Protobuf.WellKnownTypes.Value.KindOneofCase.None => throw new InvalidOperationException(),
            _ => throw new InvalidCastException(),
        };

        static System.Text.Json.Nodes.JsonObject FromStruct(Google.Protobuf.WellKnownTypes.Struct @struct)
        {
            var node = new System.Text.Json.Nodes.JsonObject();

            foreach (var field in @struct.Fields)
            {
                node.Add(field.Key, field.Value.ToJsonNode());
            }

            return node;
        }

        static System.Text.Json.Nodes.JsonArray FromList(Google.Protobuf.WellKnownTypes.ListValue list)
        {
            var node = new System.Text.Json.Nodes.JsonArray();

            foreach (var item in list.Values)
            {
                node.Add(item.ToJsonNode());
            }

            return node;
        }
    }
}