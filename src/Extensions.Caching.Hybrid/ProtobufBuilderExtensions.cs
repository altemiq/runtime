// -----------------------------------------------------------------------
// <copyright file="ProtobufBuilderExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130 // Namespace does not match folder structure

/// <summary>
/// The <see cref="Google.Protobuf.IMessage{T}"/> serializer builder extensions.
/// </summary>
public static class ProtobufBuilderExtensions
{
    /// <summary>
    /// Add a <see cref="Google.Protobuf.IMessage{T}"/> serializer to the cache.
    /// </summary>
    /// <typeparam name="T">The type of message.</typeparam>
    /// <param name="builder">The builder.</param>
    /// <returns>The <see cref="Microsoft.Extensions.Caching.Hybrid.IHybridCacheBuilder"/> instance.</returns>
    public static Microsoft.Extensions.Caching.Hybrid.IHybridCacheBuilder AddProtobufSerializer<T>(this Microsoft.Extensions.Caching.Hybrid.IHybridCacheBuilder builder)
        where T : Google.Protobuf.IMessage<T>, new() => builder.AddSerializer<T, Altemiq.Extensions.Caching.Hybrid.Internal.ProtobufSerializer<T>>();

    /// <summary>
    /// Add a <see cref="Google.Protobuf.IMessage{T}"/> serializer factory to the cache.
    /// </summary>
    /// <param name="builder">The builder.</param>
    /// <returns>The <see cref="Microsoft.Extensions.Caching.Hybrid.IHybridCacheBuilder"/> instance.</returns>
    [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
    public static Microsoft.Extensions.Caching.Hybrid.IHybridCacheBuilder AddProtobufSerializerFactory(this Microsoft.Extensions.Caching.Hybrid.IHybridCacheBuilder builder) => builder.AddSerializerFactory<Altemiq.Extensions.Caching.Hybrid.Internal.ProtobufSerializerFactory>();
}