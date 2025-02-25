// -----------------------------------------------------------------------
// <copyright file="ProtobufSerializerFactory.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Caching.Hybrid.Internal;

/// <summary>
/// Hybrid cache serializer factory for <see cref="Google.Protobuf"/> types.
/// </summary>
[System.Diagnostics.CodeAnalysis.RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
[System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("If some of the generic arguments are annotated (either with DynamicallyAccessedMembersAttribute, or generic constraints), trimming can't validate that the requirements of those annotations are met.")]
public sealed class ProtobufSerializerFactory : Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializerFactory
{
    /// <inheritdoc/>
    public bool TryCreateSerializer<T>(
#if NETSTANDARD2_1_OR_GREATER || NET5_0_OR_GREATER
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>? serializer)
#else
        out Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T> serializer)
#endif
    {
        try
        {
            if (ProtobufSerializer.Create<T>() is { } createdSerializer)
            {
                serializer = createdSerializer;
                return true;
            }
        }
        catch (Exception ex)
        {
            // Unexpected; maybe manually implemented and missing .Parser property?
            // Log it and ignore the type.
            System.Diagnostics.Debug.WriteLine(ex.Message);
        }

        // This does not appear to be a Google.Protobuf type.
        serializer = null!;
        return false;
    }
}