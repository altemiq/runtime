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
    /// <inheritdoc cref="Converters.StructConverter.ToStruct(JsonDocument?)"/>
    public static Struct? ToStruct(this JsonDocument? document) => Converters.StructConverter.ToStruct(document);

    /// <inheritdoc cref="Converters.ValueConverter.ToValue(JsonElement)"/>
    public static Value ToValue(this JsonElement element) => Converters.ValueConverter.ToValue(element);

    /// <inheritdoc cref="Converters.StructConverter.ToJsonDocument(Struct?)"/>
    public static JsonDocument? ToJsonDocument(this Struct? @struct) => Converters.StructConverter.ToJsonDocument(@struct);

    /// <inheritdoc cref="Converters.ValueConverter.ToJsonElement(Value)"/>
    public static JsonElement ToJsonElement(this Value value) => Converters.ValueConverter.ToJsonElement(value);

    /// <inheritdoc cref="Converters.ValueConverter.ToJsonElement(Value)"/>
    public static JsonNode? ToJsonNode(this Value value) => Converters.ValueConverter.ToJsonNode(value);
#endif

    /// <inheritdoc cref="WellKnownTypes.Uuid.ForGuid(System.Guid)"/>
    public static Altemiq.Protobuf.WellKnownTypes.Uuid ToUuid(this System.Guid guid) => WellKnownTypes.Uuid.ForGuid(guid);
}