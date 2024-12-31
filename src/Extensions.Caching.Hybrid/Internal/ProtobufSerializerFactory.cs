// -----------------------------------------------------------------------
// <copyright file="ProtobufSerializerFactory.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Caching.Hybrid.Internal;

/// <summary>
/// Hybrid cache serializer factory for <see cref="Google.Protobuf"/> types.
/// </summary>
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
            // need to ensure that the type is an IMessage<T> to work with serializer.
            if (typeof(Google.Protobuf.IMessage).IsAssignableFrom(typeof(T))
                && typeof(Google.Protobuf.IMessage<>).MakeGenericType(typeof(T)).IsAssignableFrom(typeof(T))
                && Activator.CreateInstance(typeof(ProtobufSerializer<>).MakeGenericType(typeof(T))) is Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T> createdSerializer)
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