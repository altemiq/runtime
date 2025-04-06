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
    /// <inheritdoc cref="Converters.StructConverter.ToStruct"/>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(document))]
    public static Struct? ToStruct(this JsonDocument? document) => Converters.StructConverter.ToStruct(document);

    /// <inheritdoc cref="Converters.ValueConverter.ToValue(JsonElement)"/>
    public static Value ToValue(this JsonElement element) => Converters.ValueConverter.ToValue(element);

    /// <inheritdoc cref="Converters.ValueConverter.ToValue(JsonNode)"/>
    public static Value ToValue(this JsonNode? node) => Converters.ValueConverter.ToValue(node);

    /// <inheritdoc cref="Converters.StructConverter.ToJsonDocument"/>
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(@struct))]
    public static JsonDocument? ToJsonDocument(this Struct? @struct) => Converters.StructConverter.ToJsonDocument(@struct);

    /// <inheritdoc cref="Converters.ValueConverter.ToJsonElement"/>
    public static JsonElement ToJsonElement(this Value value) => Converters.ValueConverter.ToJsonElement(value);

    /// <inheritdoc cref="Converters.ValueConverter.ToJsonNode"/>
    public static JsonNode? ToJsonNode(this Value value) => Converters.ValueConverter.ToJsonNode(value);
#endif

    /// <inheritdoc cref="WellKnownTypes.Uuid.ForGuid"/>
    public static WellKnownTypes.Uuid ToUuid(this Guid guid) => WellKnownTypes.Uuid.ForGuid(guid);

    /// <inheritdoc cref="WellKnownTypes.Version.ForVersion" />
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(version))]
    public static WellKnownTypes.Version? ToVersion(this Version? version) => version is null ? default : WellKnownTypes.Version.ForVersion(version);

    /// <inheritdoc cref="WellKnownTypes.Version.ToVersion" />
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(version))]
    public static Version? ToVersion(this WellKnownTypes.Version? version) => version?.ToVersion();

    /// <inheritdoc cref="WellKnownTypes.SemanticVersion.ForVersion" />
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(version))]
    public static WellKnownTypes.SemanticVersion? ToSemanticVersion(this Version? version, IEnumerable<string>? releaseLabels = default, string? metadata = default) => version is null ? default : WellKnownTypes.SemanticVersion.ForVersion(version, releaseLabels, metadata);
}