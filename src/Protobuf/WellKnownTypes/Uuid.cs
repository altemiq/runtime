// -----------------------------------------------------------------------
// <copyright file="Uuid.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.WellKnownTypes;

/// <content>
/// The conversions to/from <see cref="Uuid"/>.
/// </content>
public sealed partial class Uuid
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Uuid"/> message.
    /// </summary>
    /// <returns>A new UUID object.</returns>
    public static Uuid NewUuid() => ForGuid(Guid.NewGuid());

#if NET9_0_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="Uuid"/> according to RFC 9562, following the Version 7 format.
    /// </summary>
    /// <returns>A new <see cref="Uuid"/> according to RFC 9562, following the Version 7 format.</returns>
    public static Uuid CreateVersion7() => ForGuid(Guid.CreateVersion7());

    /// <summary>
    /// Creates a new <see cref="Uuid"/> according to RFC 9562, following the Version 7 format.
    /// </summary>
    /// <param name="timestamp">The date-time offset used to determine the Unix Epoch timestamp.</param>
    /// <returns>A new <see cref="Uuid"/> according to RFC 9562, following the Version 7 format.</returns>
    public static Uuid CreateVersion7(DateTimeOffset timestamp) => ForGuid(Guid.CreateVersion7(timestamp));
#endif

    /// <summary>
    /// Convenience method to create a <see cref="Uuid"/> message with a <see cref="Guid"/>.
    /// </summary>
    /// <param name="guid">The guid.</param>
    /// <returns>A newly-created <see cref="Uuid"/> message with the given value.</returns>
    public static Uuid ForGuid(Guid guid)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> bytes = stackalloc byte[16];
        guid.TryWriteBytes(bytes);
#else
        Span<byte> bytes = guid.ToByteArray();
#endif
        return new Uuid
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
            TimeLow = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(bytes[..4]),
            TimeMid = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes[4..6]),
            TimeHiAndVersion = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes[6..8]),
            ClockSeqHiAndReserved = bytes[8],
            ClockSeqLow = bytes[9],
            Node = System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(bytes[8..16]) & 0x0000FFFFFFFFFFFF,
#else
            TimeLow = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(bytes.Slice(0, 4)),
            TimeMid = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(4, 2)),
            TimeHiAndVersion = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes.Slice(6, 2)),
            ClockSeqHiAndReserved = bytes[8],
            ClockSeqLow = bytes[9],
            Node = System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(bytes.Slice(8, 8)) & 0x0000FFFFFFFFFFFF,
#endif
        };
    }

    /// <summary>
    /// Converts this instance to <see cref="Guid"/>.
    /// </summary>
    /// <returns>The created <see cref="Guid"/>.</returns>
    public Guid ToGuid()
    {
        checked
        {
            Span<byte> bytes = stackalloc byte[16];
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(bytes[..4], this.TimeLow);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(bytes[4..6], (ushort)this.TimeMid);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(bytes[6..8], (ushort)this.TimeHiAndVersion);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(bytes[8..], this.Node);
#else
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(bytes.Slice(0, 4), this.TimeLow);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(bytes.Slice(4, 2), (ushort)this.TimeMid);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(bytes.Slice(6, 2), (ushort)this.TimeHiAndVersion);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(bytes.Slice(8), this.Node);
#endif
            bytes[8] = (byte)this.ClockSeqHiAndReserved;
            bytes[9] = (byte)this.ClockSeqLow;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            return new Guid(bytes);
#else
            return new Guid(bytes.ToArray());
#endif
        }
    }
}