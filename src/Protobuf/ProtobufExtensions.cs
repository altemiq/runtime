// -----------------------------------------------------------------------
// <copyright file="ProtobufExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf;

/// <summary>
/// <see cref="Protobuf"/> extensions.
/// </summary>
public static class ProtobufExtensions
{
#if NETSTANDARD2_0_OR_GREATER || NET461_OR_GREATER || NETCOREAPP3_0_OR_GREATER
    /// <inheritdoc cref="Converters.StructConverter.ToStruct(System.Text.Json.JsonDocument?)"/>
    public static Google.Protobuf.WellKnownTypes.Struct? ToStruct(this System.Text.Json.JsonDocument? document) => Converters.StructConverter.ToStruct(document);

    /// <inheritdoc cref="Converters.StructConverter.ToStruct(System.Text.Json.JsonElement)"/>
    public static Google.Protobuf.WellKnownTypes.Struct? ToStruct(this System.Text.Json.JsonElement element) => Converters.StructConverter.ToStruct(element);

    /// <inheritdoc cref="Converters.StructConverter.ToJsonDocument(Google.Protobuf.WellKnownTypes.Struct?)"/>
    public static System.Text.Json.JsonDocument? ToJsonDocument(this Google.Protobuf.WellKnownTypes.Struct? @struct) => Converters.StructConverter.ToJsonDocument(@struct);

    /// <inheritdoc cref="Converters.StructConverter.ToJsonElement(Google.Protobuf.WellKnownTypes.Struct?)"/>
    public static System.Text.Json.JsonElement ToJsonElement(this Google.Protobuf.WellKnownTypes.Struct? @struct) => System.Text.Json.JsonDocument.Parse(@struct?.ToString() ?? "null").RootElement;
#endif

    /// <inheritdoc cref="WellKnownTypes.Uuid.ForGuid(System.Guid)"/>
    public static Altemiq.Protobuf.WellKnownTypes.Uuid ToUuid(this System.Guid guid) => WellKnownTypes.Uuid.ForGuid(guid);
}