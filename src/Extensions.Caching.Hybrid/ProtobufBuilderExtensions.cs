// -----------------------------------------------------------------------
// <copyright file="ProtobufBuilderExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;
#pragma warning restore IDE0130, CheckNamespace

#pragma warning disable RCS1263, SA1101, S2325

/// <summary>
/// The <see cref="Google.Protobuf.IMessage{T}"/> serializer builder extensions.
/// </summary>
public static class ProtobufBuilderExtensions
{
    /// <content>
    /// The <see cref="Google.Protobuf.IMessage{T}"/> serializer builder extensions.
    /// </content>
    /// <param name="builder">The builder.</param>
    extension(Caching.Hybrid.IHybridCacheBuilder builder)
    {
        /// <summary>
        /// Add a <see cref="Google.Protobuf.IMessage{T}"/> serializer to the cache.
        /// </summary>
        /// <typeparam name="T">The type of message.</typeparam>
        /// <returns>The <see cref="Caching.Hybrid.IHybridCacheBuilder"/> instance.</returns>
        public Caching.Hybrid.IHybridCacheBuilder AddProtobufSerializer<T>()
            where T : Google.Protobuf.IMessage<T>, new() => builder.AddSerializer<T, Altemiq.Extensions.Caching.Hybrid.Internal.ProtobufSerializer<T>>();

        /// <summary>
        /// Add a <see cref="Google.Protobuf.IMessage{T}"/> serializer factory to the cache.
        /// </summary>
        /// <returns>The <see cref="Caching.Hybrid.IHybridCacheBuilder"/> instance.</returns>
        [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
        public Caching.Hybrid.IHybridCacheBuilder AddProtobufSerializerFactory() => builder.AddSerializerFactory<Altemiq.Extensions.Caching.Hybrid.Internal.ProtobufSerializerFactory>();
    }
}