// -----------------------------------------------------------------------
// <copyright file="StructConverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.Converters;

/// <summary>
/// The <see cref="Google.Protobuf.WellKnownTypes.Struct"/> converter.
/// </summary>
public static class StructConverter
{
    /// <summary>
    /// Creates a <see cref="Google.Protobuf.WellKnownTypes.Struct"/> from a <see cref="System.Text.Json.JsonDocument"/> instance.
    /// </summary>
    /// <param name="document">The <see cref="System.Text.Json.JsonDocument"/> instance.</param>
    /// <returns>The <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.</returns>
    public static Google.Protobuf.WellKnownTypes.Struct? ToStruct(System.Text.Json.JsonDocument? document) => document is null ? default : ToStruct(document.RootElement);

    /// <summary>
    /// Creates a <see cref="Google.Protobuf.WellKnownTypes.Struct"/> from a <see cref="System.Text.Json.JsonElement"/> instance.
    /// </summary>
    /// <param name="element">The <see cref="System.Text.Json.JsonElement"/> instance.</param>
    /// <returns>The <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.</returns>
    public static Google.Protobuf.WellKnownTypes.Struct? ToStruct(System.Text.Json.JsonElement element)
    {
        if (element.ValueKind is System.Text.Json.JsonValueKind.Null)
        {
            return null;
        }

        // convert this to a `google.protobuf.Struct`
        return GetValue(element) switch
        {
            { StructValue: { } structValue } => structValue,
            var v => new Google.Protobuf.WellKnownTypes.Struct { Fields = { { "root", v } } },
        };

        static Google.Protobuf.WellKnownTypes.Value GetValue(System.Text.Json.JsonElement element)
        {
            return element switch
            {
                { ValueKind: System.Text.Json.JsonValueKind.Array } @array => GetArray(@array),
                { ValueKind: System.Text.Json.JsonValueKind.Object } @object => GetStruct(@object),
                { ValueKind: System.Text.Json.JsonValueKind.String } @string => Google.Protobuf.WellKnownTypes.Value.ForString(@string.GetString()),
                { ValueKind: System.Text.Json.JsonValueKind.Null } => Google.Protobuf.WellKnownTypes.Value.ForNull(),
                { ValueKind: System.Text.Json.JsonValueKind.True } => Google.Protobuf.WellKnownTypes.Value.ForBool(value: true),
                { ValueKind: System.Text.Json.JsonValueKind.False } => Google.Protobuf.WellKnownTypes.Value.ForBool(value: false),
                { ValueKind: System.Text.Json.JsonValueKind.Number } @number => Google.Protobuf.WellKnownTypes.Value.ForNumber(@number.GetDouble()),
                _ => throw new InvalidCastException(),
            };

            static Google.Protobuf.WellKnownTypes.Value GetArray(System.Text.Json.JsonElement element)
            {
                var values = new List<Google.Protobuf.WellKnownTypes.Value>();
                foreach (var item in element.EnumerateArray())
                {
                    values.Add(GetValue(item));
                }

                return Google.Protobuf.WellKnownTypes.Value.ForList([.. values]);
            }

            static Google.Protobuf.WellKnownTypes.Value GetStruct(System.Text.Json.JsonElement element)
            {
                var @struct = new Google.Protobuf.WellKnownTypes.Struct();
                foreach (var item in element.EnumerateObject())
                {
                    @struct.Fields[item.Name] = GetValue(item.Value);
                }

                return Google.Protobuf.WellKnownTypes.Value.ForStruct(@struct);
            }
        }
    }

    /// <summary>
    /// Creates a <see cref="System.Text.Json.JsonDocument"/> from a <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.
    /// </summary>
    /// <param name="struct">The <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.</param>
    /// <returns>The <see cref="System.Text.Json.JsonDocument"/> instance.</returns>
    public static System.Text.Json.JsonDocument? ToJsonDocument(Google.Protobuf.WellKnownTypes.Struct? @struct) => @struct is null ? default : System.Text.Json.JsonDocument.Parse(@struct.ToString());

    /// <summary>
    /// Creates a <see cref="System.Text.Json.JsonDocument"/> from a <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.
    /// </summary>
    /// <param name="struct">The <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.</param>
    /// <returns>The <see cref="System.Text.Json.JsonDocument"/> instance.</returns>
    public static System.Text.Json.JsonElement ToJsonElement(Google.Protobuf.WellKnownTypes.Struct? @struct) => System.Text.Json.JsonDocument.Parse(@struct?.ToString() ?? "null").RootElement;
}