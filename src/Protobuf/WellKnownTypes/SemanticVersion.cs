// -----------------------------------------------------------------------
// <copyright file="SemanticVersion.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.WellKnownTypes;

using System.Diagnostics.CodeAnalysis;

/// <summary>
/// The conversions to/from version.
/// </summary>
public partial class SemanticVersion :
    IComparable,
#if NET7_0_OR_GREATER
    ISpanParsable<SemanticVersion>,
    ISpanFormattable,
#else
    IFormattable,
#endif
#if NET8_0_OR_GREATER
    IUtf8SpanFormattable,
#endif
    IComparable<SemanticVersion>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SemanticVersion"/> class.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="releaseLabels">The release labels.</param>
    /// <param name="metadata">The metadata.</param>
    /// <exception cref="ArgumentNullException">version is null.</exception>
    public SemanticVersion(System.Version version, IEnumerable<string>? releaseLabels, string? metadata)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(version);
#else
        if (version is null)
        {
            throw new ArgumentNullException(nameof(version));
        }
#endif

        var normalizedVersion = NormalizeVersionValue(version);
        this.major_ = normalizedVersion.Major;
        this.minor_ = normalizedVersion.Minor;
        this.patch_ = normalizedVersion.Build;
        this.metadata_ = metadata;

        if (releaseLabels != null)
        {
            this.releaseLabels_.AddRange(releaseLabels);
        }
    }

    /// <summary>
    /// Gets the full pre-release label for the version.
    /// </summary>
    public string Release => this.releaseLabels_ switch
    {
        { Count: 1 } => this.releaseLabels_[0],
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        not null => string.Join('.', this.releaseLabels_),
#else
        not null => string.Join(".", this.releaseLabels_),
#endif
        _ => string.Empty,
    };

    /// <summary>
    /// Gets a value indicating whether pre-release labels exist for the version.
    /// </summary>
    public bool IsPrerelease => this.releaseLabels_?.Any(t => !string.IsNullOrEmpty(t)) is true;

    /// <summary>
    /// Implements the equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(SemanticVersion? lhs, SemanticVersion? rhs) => ReferenceEquals(lhs, rhs) || lhs?.Equals(rhs) is true;

    /// <summary>
    /// Implements the not equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(SemanticVersion? lhs, SemanticVersion? rhs) => !ReferenceEquals(lhs, rhs) && lhs?.Equals(rhs) is not true;

    /// <summary>
    /// Implements the less than operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <(SemanticVersion? lhs, SemanticVersion? rhs) => !ReferenceEquals(lhs, rhs) && lhs?.CompareTo(rhs) < 0;

    /// <summary>
    /// Implements the less than or equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator <=(SemanticVersion? lhs, SemanticVersion? rhs) => ReferenceEquals(lhs, rhs) || lhs?.CompareTo(rhs) <= 0;

    /// <summary>
    /// Implements the greater than operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >(SemanticVersion? lhs, SemanticVersion? rhs) => !ReferenceEquals(lhs, rhs) && (lhs is null || lhs.CompareTo(rhs) > 0);

    /// <summary>
    /// Implements the greater than or equals operator.
    /// </summary>
    /// <param name="lhs">The left-hand side.</param>
    /// <param name="rhs">The right-hand side.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator >=(SemanticVersion? lhs, SemanticVersion? rhs) => ReferenceEquals(lhs, rhs) || lhs?.CompareTo(rhs) >= 0;

    /// <summary>
    /// Convenience method to create a <see cref="SemanticVersion"/> message with a <see cref="System.Version"/>.
    /// </summary>
    /// <param name="version">The version.</param>
    /// <param name="releaseLabels">The release labels.</param>
    /// <param name="metadata">The metadata.</param>
    /// <returns>A newly-created <see cref="SemanticVersion"/> message with the given value.</returns>
    public static SemanticVersion ForVersion(System.Version version, IEnumerable<string>? releaseLabels = default, string? metadata = default) => new(version, releaseLabels, metadata);

    /// <summary>
    /// Parses a string into a value.
    /// </summary>
    /// <param name="s">The string to parse.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="s"/> could not be parsed.</exception>
    public static SemanticVersion Parse(string s) => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

    /// <summary>
    /// Parses a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <returns>The result of parsing <paramref name="s"/>.</returns>
    /// <exception cref="ArgumentException"><paramref name="s"/> could not be parsed.</exception>
    public static SemanticVersion Parse(ReadOnlySpan<char> s) => Parse(s, System.Globalization.CultureInfo.InvariantCulture);

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
    public static SemanticVersion Parse(string s, IFormatProvider? provider)
    {
        if (!TryParse(s, provider, out var ver))
        {
            throw new ArgumentException("Invalid format", nameof(s));
        }

        return ver;
    }

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
    [SuppressMessage("Microsoft.Style", "IDE0060:Remove unused parameter", Justification = "This is required for implementing the interface")]
    public static SemanticVersion Parse(ReadOnlySpan<char> s, IFormatProvider? provider)
    {
        if (!TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out var ver))
        {
            throw new ArgumentException("Invalid format", nameof(s));
        }

        return ver;
    }

    /// <summary>
    /// Tries to parse a string into a value.
    /// </summary>
    /// <param name="s">The string parse.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse([NotNullWhen(true)] string? s, [MaybeNullWhen(false)] out SemanticVersion result) => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

    /// <summary>
    /// Tries to parse a span of characters into a value.
    /// </summary>
    /// <param name="s">The span of characters to parse.</param>
    /// <param name="result">When this method returns, contains the result of successfully parsing <paramref name="s"/>, or an undefined value on failure.</param>
    /// <returns><see langword="true"/> if <paramref name="s"/> was successfully parsed; otherwise, <see langword="false"/>.</returns>
    public static bool TryParse(ReadOnlySpan<char> s, [MaybeNullWhen(false)] out SemanticVersion result) => TryParse(s, System.Globalization.CultureInfo.InvariantCulture, out result);

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
    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out SemanticVersion result)
    {
        result = null;
        return s is not null && TryParse(s.AsSpan(), provider, out result);
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
    public static bool TryParse(ReadOnlySpan<char> s, IFormatProvider? provider, [MaybeNullWhen(false)] out SemanticVersion result)
    {
        result = null;

        ParseSections(s, out var versionString, out var releaseLabels, out var buildMetadata);

        // null indicates the string did not meet the rules
        if (versionString is null || !System.Version.TryParse(versionString, out var systemVersion))
        {
            return false;
        }

        // validate the version string
        var parts = versionString.Split('.');

        if (parts.Length != 3)
        {
            // versions must be 3 parts
            return false;
        }

        if (parts.Any(part => !IsValidPart(part, allowLeadingZeros: false)))
        {
            return false;
        }

        // labels
        if (releaseLabels?.Any(t => !IsValidPart(t, allowLeadingZeros: false)) == true)
        {
            return false;
        }

        // build metadata
        if (buildMetadata is not null
            && !IsValid(buildMetadata, allowLeadingZeros: true))
        {
            return false;
        }

        var version = NormalizeVersionValue(systemVersion);

        result = new SemanticVersion(
            version: version,
            releaseLabels: releaseLabels,
            metadata: buildMetadata);

        return true;

        static void ParseSections(ReadOnlySpan<char> value, out string? versionString, out string[]? releaseLabels, out string? buildMetadata)
        {
            versionString = null;
            releaseLabels = null;
            buildMetadata = null;

            var dashPosition = -1;
            var plusPosition = -1;

            for (var i = 0; i < value.Length; i++)
            {
                var end = i == value.Length - 1;

                if (dashPosition < 0)
                {
                    if (!end && value[i] is not '-' && value[i] is not '+')
                    {
                        continue;
                    }

                    var endPos = i + (end ? 1 : 0);
                    versionString = value[..endPos].ToString();

                    dashPosition = i;

                    if (value[i] == '+')
                    {
                        plusPosition = i;
                    }
                }
                else if (plusPosition < 0)
                {
                    if (!end && value[i] is not '+')
                    {
                        continue;
                    }

                    var start = dashPosition + 1;
                    var endPos = i + (end ? 1 : 0);
                    var releaseLabel = value[start..endPos].ToString();

                    releaseLabels = releaseLabel.Split('.');

                    plusPosition = i;
                }
                else if (end)
                {
                    var start = plusPosition + 1;
                    var endPosition = i + (end ? 1 : 0);
                    buildMetadata = value[start..endPosition].ToString();
                }
            }
        }

        static bool IsValidPart(string s, bool allowLeadingZeros)
        {
            if (s.Length is 0)
            {
                // empty labels are not allowed
                return false;
            }

            // 0 is fine, but 00 is not.
            // 0A counts as an alphanumeric string where zeros are not counted
            if (allowLeadingZeros || s is { Length: <= 1 } or [not '0', ..])
            {
                return s
#if NETSTANDARD1_1
                    .ToCharArray()
#endif
                    .All(IsLetterOrDigitOrDash);
            }

            var allDigits = true;

            // Check if all characters are digits.
            // The first is already checked above
            for (var i = 1; i < s.Length; i++)
            {
                if (s[i] is >= '0' and <= '9')
                {
                    continue;
                }

                allDigits = false;
                break;
            }

            if (allDigits)
            {
                // leading zeros are not allowed in numeric labels
                return false;
            }

            return s
#if NETSTANDARD1_1
                .ToCharArray()
#endif
                .All(IsLetterOrDigitOrDash);

            static bool IsLetterOrDigitOrDash(char c)
            {
                return c is >= '0' and <= '9' or >= 'A' and <= 'Z' or >= 'a' and <= 'z' or '-';
            }
        }

        static bool IsValid(string s, bool allowLeadingZeros)
        {
            return s.Split('.').All(t => IsValidPart(t, allowLeadingZeros));
        }
    }

    /// <summary>
    /// Converts this instance to <see cref="System.Version"/>.
    /// </summary>
    /// <returns>The created <see cref="System.Version"/>.</returns>
    public System.Version ToVersion() => new(this.major_, this.minor_, this.patch_);

    /// <summary>
    /// Gives a normalized representation of the version.
    /// This string is unique to the identity of the version and does not contain metadata.
    /// </summary>
    /// <returns>>The normalized representation of the version.</returns>
    public string ToNormalizedString() => this.ToString("N", VersionFormatter.Instance);

    /// <summary>
    /// Gives a full representation of the version include metadata.
    /// This string is not unique to the identity of the version.
    /// Other versions that differ on metadata will have a different full string representation.
    /// </summary>
    /// <returns>The full representation of the version include metadata.</returns>
    public string ToFullString() => this.ToString("F", VersionFormatter.Instance);

    /// <inheritdoc/>
    public string ToString(string? format, IFormatProvider? formatProvider)
    {
        return TryFormatter(out var formattedString)
            ? formattedString!
            : this.ToString();

        bool TryFormatter(out string? output)
        {
            if (formatProvider?.GetFormat(this.GetType()) is ICustomFormatter formatter)
            {
                output = formatter.Format(format, this, formatProvider);
                return true;
            }

            output = null;
            return false;
        }
    }

#if NET8_0_OR_GREATER
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
    {
        var s = this.ToString(format.ToString(), provider ?? VersionFormatter.Instance);
        if (s.Length <= destination.Length)
        {
#if NET6_0_OR_GREATER
            s.CopyTo(destination);
#else
            for (int i = 0; i < s.Length; i++)
            {
                destination[i] = s[i];
            }
#endif
            charsWritten = s.Length;
            return true;
        }

        charsWritten = 0;
        return false;
    }

#if NET8_0_OR_GREATER
    /// <inheritdoc />
#else
    /// <summary>
    /// Tries to format the value of the current instance as UTF-8 into the provided span of bytes.
    /// </summary>
    /// <param name="utf8Destination">The span in which to write this instance's value formatted as a span of bytes.</param>
    /// <param name="bytesWritten">When this method returns, contains the number of bytes that were written in <paramref name="utf8Destination"/>.</param>
    /// <param name="format">A span containing the characters that represent a standard or custom format string that defines the acceptable format for <paramref name="utf8Destination"/>.</param>
    /// <param name="provider">An optional object that supplies culture-specific formatting information for <paramref name="utf8Destination"/>.</param>
    /// <returns><see langword="true"/> if the formatting was successful; otherwise <see langword="false"/>.</returns>
#endif
    public bool TryFormat(Span<byte> utf8Destination, out int bytesWritten, ReadOnlySpan<char> format, IFormatProvider? provider)
    {
        var b = System.Text.Encoding.UTF8.GetBytes(this.ToString(format.ToString(), provider ?? VersionFormatter.Instance));
        if (b.Length <= utf8Destination.Length)
        {
#if NET6_0_OR_GREATER
            b.CopyTo(utf8Destination);
#else
            for (int i = 0; i < b.Length; i++)
            {
                utf8Destination[i] = b[i];
            }
#endif
            bytesWritten = b.Length;
            return true;
        }

        bytesWritten = 0;
        return false;
    }

    /// <inheritdoc/>
    public int CompareTo(object? obj) => this.CompareTo(obj as SemanticVersion);

    /// <inheritdoc/>
    public int CompareTo(SemanticVersion? other)
    {
        if (ReferenceEquals(this, other))
        {
            return 0;
        }

        if (other is null)
        {
            return 1;
        }

        // compare version
        var result = this.Major.CompareTo(other.Major);
        if (result != 0)
        {
            return result;
        }

        result = this.Minor.CompareTo(other.Minor);
        if (result != 0)
        {
            return result;
        }

        result = this.Patch.CompareTo(other.Patch);
        if (result != 0)
        {
            return result;
        }

        // compare release labels
        var thisLabels = GetReleaseLabelsOrNull(this);
        var otherLabels = GetReleaseLabelsOrNull(other);

        if (thisLabels is not null
            && otherLabels is null)
        {
            return -1;
        }

        if (thisLabels is null
            && otherLabels is not null)
        {
            return 1;
        }

        if (thisLabels is not null
            && otherLabels is not null)
        {
            result = CompareReleaseLabels(thisLabels, otherLabels);
            if (result is not 0)
            {
                return result;
            }
        }

        // compare the metadata
        return StringComparer.OrdinalIgnoreCase.Compare(this.Metadata ?? string.Empty, other.Metadata ?? string.Empty);

        static IList<string>? GetReleaseLabelsOrNull(SemanticVersion version)
        {
            return version.IsPrerelease ? version.ReleaseLabels : null;
        }

        static int CompareReleaseLabels(IList<string> version1, IList<string> version2)
        {
            var count = Math.Max(version1.Count, version2.Count);

            for (var i = 0; i < count; i++)
            {
                switch (i < version1.Count)
                {
                    case false when i < version2.Count:
                        return -1;
                    case true when i >= version2.Count:
                        return 1;
                }

                // compare the labels
                var result = CompareRelease(version1[i], version2[i]);

                if (result != 0)
                {
                    return result;
                }
            }

            return 0;

            static int CompareRelease(string version1, string version2)
            {
                // check if the identifiers are numeric
                return (TryInt32Parse(version1, out var version1Num), TryInt32Parse(version2, out var version2Num)) switch
                {
                    // if both are numeric compare them as numbers
                    (true, true) => version1Num.CompareTo(version2Num),

                    // numeric labels come before alpha labels
                    (true, false) => -1,
                    (false, true) => 1,

                    // string compare
                    _ => StringComparer.OrdinalIgnoreCase.Compare(version1, version2),
                };

                static bool TryInt32Parse(string input, out int value)
                {
                    return int.TryParse(
                        input,
                        System.Globalization.NumberStyles.Integer,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out value);
                }
            }
        }
    }

    private static System.Version NormalizeVersionValue(System.Version version)
    {
        if (version.Build < 0
            || version.Revision < 0)
        {
            return new(
                version.Major,
                version.Minor,
                Math.Max(version.Build, 0),
                Math.Max(version.Revision, 0));
        }

        return version;
    }
}