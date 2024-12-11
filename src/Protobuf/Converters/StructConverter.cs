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
    public static Google.Protobuf.WellKnownTypes.Struct? ToStruct(System.Text.Json.JsonDocument? document)
    {
        if (document is null
            || document.RootElement is { ValueKind: System.Text.Json.JsonValueKind.Null })
        {
            return default;
        }

        // convert this to a `google.protobuf.Struct`
        return document.RootElement.ToValue() switch
        {
            { StructValue: { } structValue } => structValue,
            var v => new Google.Protobuf.WellKnownTypes.Struct { Fields = { { "root", v } } },
        };
    }

    /// <summary>
    /// Creates a <see cref="System.Text.Json.JsonDocument"/> from a <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.
    /// </summary>
    /// <param name="struct">The <see cref="Google.Protobuf.WellKnownTypes.Struct"/> instance.</param>
    /// <returns>The <see cref="System.Text.Json.JsonDocument"/> instance.</returns>
    public static System.Text.Json.JsonDocument? ToJsonDocument(Google.Protobuf.WellKnownTypes.Struct? @struct) => @struct is null ? default : System.Text.Json.JsonDocument.Parse(@struct.ToString());
}