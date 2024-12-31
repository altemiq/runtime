// -----------------------------------------------------------------------
// <copyright file="ProtobufSerializer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Caching.Hybrid.Internal;

using Google.Protobuf;

/// <summary>
/// The <see cref="IMessage{T}"/> serializer.
/// </summary>
/// <typeparam name="T">The type of message to serialize.</typeparam>
internal sealed class ProtobufSerializer<T> : Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>
    where T : IMessage<T>
{
    private static readonly MessageParser<T> Parser = GetDescriptor().Parser as MessageParser<T> ?? throw new InvalidOperationException("Message parser not found; type may not be Google.Protobuf");

    /// <inheritdoc/>
    T Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>.Deserialize(System.Buffers.ReadOnlySequence<byte> source) => Parser.ParseFrom(source);

    /// <inheritdoc/>
    void Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>.Serialize(T value, System.Buffers.IBufferWriter<byte> target) => value.WriteTo(target);

    private static Google.Protobuf.Reflection.MessageDescriptor GetDescriptor() => typeof(T)
        .GetProperty(nameof(IMessage.Descriptor), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static)?.GetValue(null)
            as Google.Protobuf.Reflection.MessageDescriptor ?? throw new InvalidOperationException("Message descriptor not found; type may not be Google.Protobuf");
}