// -----------------------------------------------------------------------
// <copyright file="ProtobufSerializer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Caching.Hybrid.Internal;

/// <summary>
/// The protobuf serializer.
/// </summary>
internal static class ProtobufSerializer
{
    /// <summary>
    /// Creates a hybrid cache serializer.
    /// </summary>
    /// <typeparam name="T">The type of serializer.</typeparam>
    /// <returns>The serializer.</returns>
    /// <exception cref="InvalidOperationException">Invalid type.</exception>
    [System.Diagnostics.CodeAnalysis.RequiresDynamicCode("The native code for this instantiation might not be available at runtime.")]
    public static Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>? Create<[System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] T>()
    {
        return CreateInternal() as Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>;

        static object? CreateInternal()
        {
            // assume anything that is an IMessage is also an IMessage<T>
            return Activator.CreateInstance<T>() is Google.Protobuf.IMessage message
                ? Activator.CreateInstance(typeof(ProtobufSerializer<>).MakeGenericType(typeof(T)), [message.Descriptor.Parser])
                : default;
        }
    }

    /// <summary>
    /// Gets the message parser.
    /// </summary>
    /// <typeparam name="T">The type to get the message parser for.</typeparam>
    /// <returns>The message parser.</returns>
    public static Google.Protobuf.MessageParser<T> GetMessageParser<T>()
        where T : Google.Protobuf.IMessage<T>, new() => GetMessageParser<T>(new T());

    /// <summary>
    /// Gets the message parser.
    /// </summary>
    /// <typeparam name="T">The type to get the message parser for.</typeparam>
    /// <param name="message">The message.</param>
    /// <returns>The message parser.</returns>
    /// <exception cref="InvalidOperationException">The message parser is not the correct type.</exception>
    public static Google.Protobuf.MessageParser<T> GetMessageParser<T>(T message)
        where T : Google.Protobuf.IMessage<T> => message.Descriptor.Parser as Google.Protobuf.MessageParser<T> ?? throw new InvalidOperationException();
}