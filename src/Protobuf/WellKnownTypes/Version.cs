// -----------------------------------------------------------------------
// <copyright file="Version.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.WellKnownTypes;

using System.Diagnostics.CodeAnalysis;

/// <content>
/// The implemented methods.
/// </content>
public partial class Version :
    IComparable,
#if NET7_0_OR_GREATER
    ISpanFormattable,
    ISpanParsable<Version>,
#else
    IFormattable,
#endif
#if NET8_0_OR_GREATER
    IUtf8SpanFormattable,
    IUtf8SpanParsable<Version>,
#endif
    IComparable<Version?>
{
    private Version(System.Version version)
    {
        this.major_ = version.Major;
        this.minor_ = version.Minor;
        if (version.Build is >= 0)
        {
            this.Build = version.Build;
        }

        if (version.Revision is >= 0)
        {
            this.Revision = version.Revision;
        }
    }

    /// <inheritdoc cref="System.Version.MajorRevision" />
    public short MajorRevision => (short)(this.revision_ >> 16);

    /// <inheritdoc cref="System.Version.MinorRevision" />
    public short MinorRevision => (short)(this.revision_ & 0xFFFF);

    private int DefaultFormatFieldCount => (this.HasBuild, this.HasRevision) switch
    {
        (false, _) => 2,
        (_, false) => 3,
        _ => 4,
    };

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Version? lhs, Version? rhs) => rhs is null ? lhs is null : ReferenceEquals(rhs, lhs) || rhs.Equals(lhs);

    /// <summary>
    /// Implements the not equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(Version? lhs, Version? rhs) => !(lhs == rhs);

    /// <summary>
    /// Implements the less than operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <(Version? lhs, Version? rhs) => lhs is null ? rhs is not null : lhs.CompareTo(rhs) < 0;

    /// <summary>
    /// Implements the less than or equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <=(Version? lhs, Version? rhs) => lhs is null || lhs.CompareTo(rhs) <= 0;

    /// <summary>
    /// Implements the greater than operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >(Version? lhs, Version? rhs) => rhs < lhs;

    /// <summary>
    /// Implements the greater than or equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(Version? lhs, Version? rhs) => rhs <= lhs;

    /// <summary>
    /// Parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="s"/> could not be parsed.</exception>
    public static Version Parse(string s) => Parse(s, default);

    /// <summary>
    /// Parses a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="s"/> could not be parsed.</exception>
    public static Version Parse(ReadOnlySpan<char> s) => Parse(s, default);

#if NET7_0_OR_GREATER
    /// <inheritdoc />
#else
    /// <summary>
    /// Parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s"/>.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="s"/> could not be parsed.</exception>
#endif
    [SuppressMessage("Microsoft.Style", "IDE0060:Remove unused parameter", Justification = "This is required for implementing the interface")]
    public static Version Parse(string s, IFormatProvider? provider) => new(System.Version.Parse(s));

#if NET7_0_OR_GREATER
    /// <inheritdoc />
#else
    /// <summary>
    /// Parses a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s"/>.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="s"/> could not be parsed.</exception>
#endif
    public static Version Parse(ReadOnlySpan<char> s, IFormatProvider? provider) =>
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        new(System.Version.Parse(s));
#else
        new(System.Version.Parse(s.ToString()));
#endif

#if NET8_0_OR_GREATER
    /// <inheritdoc />
    public static Version Parse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider) => Parse(System.Text.Encoding.UTF8.GetString(utf8Text), provider: default);
#endif

    /// <summary>
    /// Tries to parse a string into a value.
    /// </summary>
    /// <param name="s">The string parse.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out Version result) => TryParse(s, provider: null, out result);

    /// <summary>
    /// Tries to parse a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out Version result) => TryParse(s, provider: null, out result);

#if NET7_0_OR_GREATER
    /// <inheritdoc />
#else
    /// <summary>
    /// Tries to parse a string into a value.
    /// </summary>
    /// <param name="s">The string parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s"/>.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
#endif
    [SuppressMessage("Microsoft.Style", "IDE0060:Remove unused parameter", Justification = "This is required for implementing the interface")]
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Version result)
    {
        if (s is not null && System.Version.TryParse(s, out var version))
        {
            result = new(version);
            return true;
        }

        result = default;
        return false;
    }

#if NET7_0_OR_GREATER
    /// <inheritdoc />
#else
    /// <summary>
    /// Tries to parse a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="provider">An object that provides culture-specific formatting information about <paramref name="s"/>.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
#endif
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out Version result)
    {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        if (System.Version.TryParse(s, out var version))
#else
        if (System.Version.TryParse(s.ToString(), out var version))
#endif
        {
            result = new(version);
            return true;
        }

        result = default;
        return false;
    }

#if NET8_0_OR_GREATER
    /// <inheritdoc />
    public static bool TryParse(ReadOnlySpan<byte> utf8Text, IFormatProvider? provider, [MaybeNullWhen(false)] out Version result) => TryParse(System.Text.Encoding.UTF8.GetString(utf8Text), provider: provider, out result);
#endif

    /// <summary>
    /// Convenience method to create a <see cref="Version"/> message with a <see cref="System.Version"/>.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <returns>A newly-created <see cref="Version"/> message with the given value.</returns>
    public static Version ForVersion(System.Version version) => new(version);

    /// <inheritdoc />
    public int CompareTo(object? obj) => this.CompareTo(obj as Version);

    /// <inheritdoc />
    public int CompareTo(Version? other)
    {
        if (ReferenceEquals(other, this))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        if (this.major_ != other.major_)
        {
            return this.major_.CompareTo(other.major_);
        }

        if (this.minor_ != other.minor_)
        {
            return this.minor_.CompareTo(other.minor_);
        }

        if (this.HasBuild != other.HasBuild && this.build_ != other.build_)
        {
            return this.build_.CompareTo(other.build_);
        }

        if (this.HasRevision != other.HasRevision && this.revision_ != other.revision_)
        {
            return this.revision_.CompareTo(other.revision_);
        }

        return 0;
    }

    /// <summary>
    /// Converts this to a version.
    /// </summary>
    /// <returns>The created version.</returns>
    public System.Version ToVersion() => (this.HasBuild, this.HasRevision) switch
    {
        (_, true) => new(this.major_, this.minor_, this.build_, this.revision_),
        (true, _) => new(this.major_, this.minor_, this.build_),
        _ => new(this.major_, this.minor_),
    };

    /// <inheritdoc cref="System.Version.ToString(int)" />
    public string ToString(int fieldCount) => this.ToVersion().ToString(fieldCount);

    /// <inheritdoc />
    public string ToString(string? format, IFormatProvider? formatProvider) => this.ToVersion().ToString();

#if NET7_0_OR_GREATER
    /// <inheritdoc />
#else
    /// <summary>
    /// Tries to format the value of the current instance into the provided span of characters.
    /// </summary>
    /// <param name="destination">The span in which to write this instance's value formatted as a span of characters.</param>
    /// <param name="charsWritten">When this method returns, contains the number of characters that were written in <paramref name="destination"/>.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="destination"/>.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="destination"/>.</param>
    /// <returns><see langword="true"/> if the formatting was successful; otherwise <see langword="false"/>.</returns>
#endif
    public bool TryFormat(Span<char> destination, out int charsWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        => this.ToVersion().TryFormat(destination, this.DefaultFormatFieldCount, out charsWritten);
#else
    {
        var s = this.ToString(this.DefaultFormatFieldCount);
        if (s.Length <= destination.Length)
        {
            for (int i = 0; i < s.Length; i++)
            {
                destination[i] = s[i];
            }

            charsWritten = s.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }
#endif

#if NET8_0_OR_GREATER
    /// <inheritdoc />
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        Span<char> destination = stackalloc char[utf8Destination.Length * 2];
        if (this.TryFormat(destination, out var charsWritten, format, provider))
        {
            bytesWritten = System.Text.Encoding.UTF8.GetBytes(destination[..charsWritten], utf8Destination);
            return true;
        }

        bytesWritten = 0;
        return false;
    }
#endif
}