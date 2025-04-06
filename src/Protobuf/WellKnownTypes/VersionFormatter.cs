// -----------------------------------------------------------------------
// <copyright file="VersionFormatter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Protobuf.WellKnownTypes;

using System.Reflection;
using System.Text;

/// <summary>
/// Custom formatter for versions.
/// </summary>
internal sealed class VersionFormatter : IFormatProvider, ICustomFormatter
{
    /// <summary>
    /// A static instance of the <see cref="VersionFormatter"/> class.
    /// </summary>
    public static readonly VersionFormatter Instance = new();

    /// <inheritdoc/>
    public string Format(string? format, object? arg, IFormatProvider? formatProvider)
    {
        if (arg is string stringValue)
        {
            return stringValue;
        }

        if (arg is not SemanticVersion version)
        {
            throw new NotSupportedException();
        }

        if (format is null or { Length: 0 })
        {
            format = "N";
        }

        var builder = new StringBuilder(256);

        foreach (var c in format)
        {
            FormatValue(builder, c, version);
        }

        return builder.ToString();

        static void FormatValue(StringBuilder builder, char c, SemanticVersion version)
        {
            switch (c)
            {
                case 'N':
                    AppendNormalized(builder, version);
                    return;
                case 'V':
                    AppendVersion(builder, version);
                    return;
                case 'F':
                    AppendNormalized(builder, version);

                    if (version.HasMetadata)
                    {
                        builder.Append('+');
                        builder.Append(version.Metadata);
                    }

                    return;
                case 'R':
                    builder.Append(version.Release);
                    return;
                case 'M':
                    builder.Append(version.Metadata);
                    return;
                case 'x':
                    AppendInt(builder, version.Major);
                    return;
                case 'y':
                    AppendInt(builder, version.Minor);
                    return;
                case 'z':
                    AppendInt(builder, version.Patch);
                    return;
                default:
                    builder.Append(c);
                    return;
            }

            static void AppendInt(StringBuilder sb, int value)
            {
                switch (value)
                {
                    case 0:
                        sb.Append('0');
                        return;

                    // special case min value since it'll overflow if we negate it
                    case int.MinValue:
                        sb.Append("-2147483648");
                        return;

                    // do all math with positive integers
                    case < 0:
                        sb.Append('-');
                        value = -value;
                        break;
                }

                // upper range of int is 1 billion, so we start dividing by that to get the digit at that position
                var divisor = 1_000_000_000;

                // remember when we found our first digit so we can keep adding intermediate zeroes
                var digitFound = false;
                while (divisor > 0)
                {
                    if (digitFound || value >= divisor)
                    {
                        digitFound = true;
                        int digit = value / divisor;
                        value -= digit * divisor;

                        // convert the digit to char by adding the value to '0'.
                        // '0' + 0 = 48 + 0 = 48 = '0'
                        // '0' + 1 = 48 + 1 = 49 = '1'
                        // '0' + 2 = 48 + 2 = 50 = '2'
                        // etc...
                        sb.Append((char)('0' + digit));
                    }

                    divisor /= 10;
                }
            }

            static void AppendVersion(StringBuilder builder, SemanticVersion version)
            {
                AppendInt(builder, version.Major);
                builder.Append('.');
                AppendInt(builder, version.Minor);
                builder.Append('.');
                AppendInt(builder, version.Patch);
            }

            static void AppendNormalized(StringBuilder builder, SemanticVersion version)
            {
                AppendVersion(builder, version);

                if (!version.IsPrerelease)
                {
                    return;
                }

                builder.Append('-');
                builder.Append(version.Release);
            }
        }
    }

    /// <inheritdoc/>
    public object? GetFormat(System.Type? formatType) => formatType == typeof(ICustomFormatter) || typeof(SemanticVersion).GetTypeInfo().IsAssignableFrom(formatType?.GetTypeInfo())
        ? this
        : default;
}