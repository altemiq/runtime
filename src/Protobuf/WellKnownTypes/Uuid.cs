// -----------------------------------------------------------------------
// <copyright file="Uuid.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.WellKnownTypes;

using System.Diagnostics.CodeAnalysis;

/// <content>
/// The conversions to/from <see cref="Uuid"/>.
/// </content>
public sealed partial class Uuid :
    IComparable,
    IComparable<Uuid>,
    IComparable<Guid>,
#if NET7_0_OR_GREATER
    ISpanParsable<Uuid>,
    ISpanFormattable,
#else
    IFormattable,
#endif
#if NET8_0_OR_GREATER
    IUtf8SpanFormattable,
#endif
    IEquatable<Guid>
{
    /// <summary>
    /// A read-only instance of the <see cref="Uuid"/> structure whose value is all zeros.
    /// </summary>
    public static readonly Uuid Empty = ForGuid(Guid.Empty);

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(Uuid? left, Uuid? right) => left?.Equals(right) ?? right is null;

    /// <summary>
    /// Implements the not-equals operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(Uuid? left, Uuid? right) => !left?.Equals(right) ?? right is not null;

    /// <summary>
    /// Implements the less than operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <(Uuid? left, Uuid? right) => left is null ? right is not null : left.CompareTo(right) < 0;

    /// <summary>
    /// Implements the less than or equals operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <=(Uuid? left, Uuid? right) => left is null || left.CompareTo(right) <= 0;

    /// <summary>
    /// Implements the greater than operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >(Uuid? left, Uuid? right) => left?.CompareTo(right) > 0;

    /// <summary>
    /// Implements the greater than or equals operator.
    /// </summary>
    /// <param name="left">The left hand side.</param>
    /// <param name="right">The right hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(Uuid? left, Uuid? right) => left is null ? right is null : left.CompareTo(right) >= 0;

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
            TimeLow = System.Buffers.Binary.BinaryPrimitives.ReadUInt32LittleEndian(bytes[..4]),
            TimeMid = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes[4..6]),
            TimeHiAndVersion = System.Buffers.Binary.BinaryPrimitives.ReadUInt16LittleEndian(bytes[6..8]),
            ClockSeqHiAndReserved = bytes[8],
            ClockSeqLow = bytes[9],
            Node = System.Buffers.Binary.BinaryPrimitives.ReadUInt64BigEndian(bytes[8..16]) & 0x0000FFFFFFFFFFFF,
        };
    }

    /// <inheritdoc cref="Guid.Parse(string)"/>
    public static Uuid Parse(string s) => ForGuid(Guid.Parse(s));

#if NET7_0_OR_GREATER
    /// <inheritdoc/>
    public static Uuid Parse(string s, IFormatProvider? provider) => ForGuid(Guid.Parse(s, provider));

    /// <inheritdoc/>
    public static Uuid Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => ForGuid(Guid.Parse(s, provider));
#endif

    /// <inheritdoc cref="Guid.TryParse(string?, out Guid)"/>
    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Uuid result)
    {
        if (Guid.TryParse(s, out var guid))
        {
            result = ForGuid(guid);
            return true;
        }

        result = default;
        return false;
    }

#if NET7_0_OR_GREATER
    /// <inheritdoc/>
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Uuid result)
    {
        if (Guid.TryParse(s, provider, out var guid))
        {
            result = ForGuid(guid);
            return true;
        }

        result = default;
        return false;
    }

    /// <inheritdoc/>
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Uuid result)
    {
        if (Guid.TryParse(s, provider, out var guid))
        {
            result = ForGuid(guid);
            return true;
        }

        result = default;
        return false;
    }
#endif

    /// <summary>
    /// Converts this instance to <see cref="Guid"/>.
    /// </summary>
    /// <returns>The created <see cref="Guid"/>.</returns>
    public Guid ToGuid()
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> bytes = stackalloc byte[16];
#else
        var bytes = new byte[16];
#endif
        _ = this.TryWriteBytesCore(bytes);
        return new Guid(bytes);
    }

    /// <inheritdoc/>
    public bool Equals(Guid other) => this.Equals(ForGuid(other));

    /// <inheritdoc cref="Guid.ToString(string?)"/>
    public string ToString([StringSyntax(StringSyntaxAttribute.GuidFormat)] string? format) => this.ToGuid().ToString(format);

    /// <inheritdoc/>
    public string ToString([StringSyntax(StringSyntaxAttribute.GuidFormat)] string? format, IFormatProvider? formatProvider)
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETFRAMEWORK
        => this.ToGuid().ToString(format, formatProvider);
#else
        => this.ToGuid().ToString(format);
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="Guid.TryFormat(Span{char}, out int, ReadOnlySpan{char})"/>
    public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format) => this.ToGuid().TryFormat(destination, out charsWritten, format);
#endif

#if NET8_0_OR_GREATER
    /// <inheritdoc cref="Guid.TryFormat(Span{byte}, out int, ReadOnlySpan{char})" />
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format) => this.ToGuid().TryFormat(utf8Destination, out bytesWritten, format);
#endif

#if NET7_0_OR_GREATER
    /// <inheritdoc />
    bool ISpanFormattable.TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format, IFormatProvider? provider) => ((ISpanFormattable)this.ToGuid()).TryFormat(destination, out charsWritten, format, provider);
#endif

#if NET8_0_OR_GREATER
    /// <inheritdoc />
    bool IUtf8SpanFormattable.TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format, IFormatProvider? provider) => ((IUtf8SpanFormattable)this.ToGuid()).TryFormat(utf8Destination, out bytesWritten, format, provider);
#endif

    /// <inheritdoc/>
    public int CompareTo(object? obj) => obj switch
    {
        Uuid uuid => this.CompareTo(uuid),
        Guid guid => this.CompareTo(guid),
        _ => -1,
    };

    /// <inheritdoc/>
    [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "This makes it hard to read.")]
    public int CompareTo(Uuid? other)
    {
        if (other is null)
        {
            return -1;
        }

        if (this.timeLow_ != other.timeLow_)
        {
            return this.timeLow_.CompareTo(other.timeLow_);
        }

        if (this.timeMid_ != other.timeMid_)
        {
            return this.timeMid_.CompareTo(other.timeMid_);
        }

        if (this.timeHiAndVersion_ != other.timeHiAndVersion_)
        {
            return this.timeHiAndVersion_.CompareTo(other.timeHiAndVersion_);
        }

        if (this.clockSeqHiAndReserved_ != other.clockSeqHiAndReserved_)
        {
            return this.clockSeqHiAndReserved_.CompareTo(other.clockSeqHiAndReserved_);
        }

        if (this.clockSeqLow_ != other.clockSeqLow_)
        {
            return this.clockSeqLow_.CompareTo(other.clockSeqLow_);
        }

        if (this.node_ != other.node_)
        {
            return this.node_.CompareTo(other.node_);
        }

        return 0;
    }

    /// <inheritdoc/>
    public int CompareTo(Guid other) => this.ToGuid().CompareTo(other);

    /// <summary>
    /// Returns a 16-element byte array that contains the value of this instance.
    /// </summary>
    /// <returns>A 16-element byte array.</returns>
    public byte[] ToByteArray()
    {
        var byteArray = new byte[16];
        _ = this.TryWriteBytesCore(byteArray);
        return byteArray;
    }

#if NET8_OR_GREATER
    /// <summary>
    /// Returns a 16-element byte array that contains the value of this instance.
    /// </summary>
    /// <param name="bigEndian">Set to <see langword="true"/> to return a big endian byte array.</param>
    /// <returns>A 16-element byte array.</returns>
    public byte[] ToByteArray(bool bigEndian) => this.ToGuid().ToByteArray(bigEndian);
#endif

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Tries to write the current UUID instance into a span of bytes.
    /// </summary>
    /// <param name="destination">When this method returns, the UUID as a span of bytes.</param>
    /// <returns><see langword="true"/> if the UUID is successfully written to the specified span; <see langword="false"/> otherwise.</returns>
    public bool TryWriteBytes(Span<byte> destination) => this.TryWriteBytesCore(destination);
#endif

#if NET8_OR_GREATER
    /// <inheritdoc cref="Guid.TryWriteBytes(Span<byte>,bool,int)"/>
    public bool TryWriteBytes(Span<byte> destination, bool bigEndian, out int bytesWritten) => this.ToGuid().TryWriteBytes(destination, bigEndian, out bytesWritten);
#endif

    private bool TryWriteBytesCore(Span<byte> destination)
    {
        if (destination.Length < 16
            || !System.Buffers.Binary.BinaryPrimitives.TryWriteUInt32LittleEndian(destination[..4], this.timeLow_)
            || !System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16LittleEndian(destination[4..6], (ushort)this.timeMid_)
            || !System.Buffers.Binary.BinaryPrimitives.TryWriteUInt16LittleEndian(destination[6..8], (ushort)this.timeHiAndVersion_)
            || !System.Buffers.Binary.BinaryPrimitives.TryWriteUInt64BigEndian(destination[8..], this.node_))
        {
            return false;
        }

        destination[8] = (byte)this.ClockSeqHiAndReserved;
        destination[9] = (byte)this.ClockSeqLow;
        return true;
    }
}