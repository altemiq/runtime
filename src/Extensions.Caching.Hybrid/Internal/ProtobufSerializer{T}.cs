// -----------------------------------------------------------------------
// <copyright file="ProtobufSerializer{T}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Extensions.Caching.Hybrid.Internal;

/// <summary>
/// The <see cref="Google.Protobuf.IMessage{T}"/> serializer.
/// </summary>
/// <typeparam name="T">The type of message to serialize.</typeparam>
internal sealed class ProtobufSerializer<T>(Google.Protobuf.MessageParser<T> parser) : Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>
    where T : Google.Protobuf.IMessage<T>, new()
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ProtobufSerializer{T}"/> class.
    /// </summary>
    public ProtobufSerializer()
        : this(ProtobufSerializer.GetMessageParser<T>())
    {
    }

    /// <inheritdoc/>
    T Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>.Deserialize(System.Buffers.ReadOnlySequence<byte> source) => parser.ParseFrom(source);

    /// <inheritdoc/>
    void Microsoft.Extensions.Caching.Hybrid.IHybridCacheSerializer<T>.Serialize(T value, System.Buffers.IBufferWriter<byte> target) => Google.Protobuf.MessageExtensions.WriteTo(value, target);
}