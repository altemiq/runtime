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
    public static Uuid Parse(string s) => Uuid.ForGuid(Guid.Parse(s));

#if NET7_0_OR_GREATER
    /// <inheritdoc/>
    public static Uuid Parse(string s, IFormatProvider? provider) => Uuid.ForGuid(Guid.Parse(s, provider));

    /// <inheritdoc/>
    public static Uuid Parse(ReadOnlySpan<char> s, IFormatProvider? provider) => Uuid.ForGuid(Guid.Parse(s, provider));
#endif

    /// <inheritdoc cref="Guid.TryParse(string?, out Guid)"/>
    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Uuid result)
    {
        if (Guid.TryParse(s, out var guid))
        {
            result = Uuid.ForGuid(guid);
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
            result = Uuid.ForGuid(guid);
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
            result = Uuid.ForGuid(guid);
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
        checked
        {
            Span<byte> bytes = stackalloc byte[16];
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32LittleEndian(bytes[..4], this.TimeLow);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(bytes[4..6], (ushort)this.TimeMid);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16LittleEndian(bytes[6..8], (ushort)this.TimeHiAndVersion);
            System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(bytes[8..], this.Node);
            bytes[8] = (byte)this.ClockSeqHiAndReserved;
            bytes[9] = (byte)this.ClockSeqLow;

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
            return new Guid(bytes);
#else
            return new Guid(bytes.ToArray());
#endif
        }
    }

    /// <inheritdoc/>
    public bool Equals(Guid other) => this.Equals(Uuid.ForGuid(other));

    /// <inheritdoc cref="Guid.ToString(string?)"/>
    public string ToString([StringSyntax(StringSyntaxAttribute.GuidFormat)] string? format) => this.ToString(format, formatProvider: null);

    /// <inheritdoc/>
    public string ToString([StringSyntax(StringSyntaxAttribute.GuidFormat)] string? format, IFormatProvider? formatProvider) =>
        this.ToGuid() is IFormattable formattable
            ? formattable.ToString(format, formatProvider)
            : this.ToString();

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc cref="Guid.TryFormat(Span{char}, out int, ReadOnlySpan{char})"/>
    public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format)
    {
        var guid = this.ToGuid();
        return guid.TryFormat(destination, out charsWritten, format);
    }
#endif

#if NET7_0_OR_GREATER
    /// <inheritdoc />
    public bool TryFormat(Span<char> destination, out int charsWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var guid = this.ToGuid();
        return guid is ISpanFormattable formattable
            ? formattable.TryFormat(destination, out charsWritten, format, provider)
            : guid.TryFormat(destination, out charsWritten, format);
    }
#endif

#if NET8_0_OR_GREATER
    /// <inheritdoc cref="Guid.TryFormat(Span{byte}, out int, ReadOnlySpan{char})" />
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format)
    {
        var guid = this.ToGuid();
        return guid.TryFormat(utf8Destination, out bytesWritten, format);
    }

    /// <inheritdoc />
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, [StringSyntax(StringSyntaxAttribute.GuidFormat)] ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var guid = this.ToGuid();
        return guid is IUtf8SpanFormattable utf8SpanFormattable
            ? utf8SpanFormattable.TryFormat(utf8Destination, out bytesWritten, format, provider)
            : guid.TryFormat(utf8Destination, out bytesWritten, format);
    }
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
}