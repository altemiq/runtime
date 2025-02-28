// -----------------------------------------------------------------------
// <copyright file="ScreamCipher.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Security.Cryptography;

/// <summary>
/// The <c>Scream</c> cipher.
/// </summary>
public class ScreamCipher
{
    private static readonly Dictionary<char, string> PlainTextToScreamCipherMap = new()
    {
        { 'A', "A" },
        { 'B', "Ȧ" },
        { 'C', "A̧" },
        { 'D', "A̲" },
        { 'E', "Á" },
        { 'F', "A̮" },
        { 'G', "A̋" },
        { 'H', "A̰" },
        { 'I', "Ả" },
        { 'J', "A̓" },
        { 'K', "Ạ" },
        { 'L', "Ă" },
        { 'M', "Ǎ" },
        { 'N', "Â" },
        { 'O', "Å" },
        { 'P', "A̯" },
        { 'Q', "A̤" },
        { 'R', "Ȃ" },
        { 'S', "Ã" },
        { 'T', "Ā" },
        { 'U', "Ä" },
        { 'V', "À" },
        { 'W', "Ȁ" },
        { 'X', "A̽" },
        { 'Y', "A̦" },
        { 'Z', "Ⱥ" },
        { 'a', "a" },
        { 'b', "ȧ" },
        { 'c', "a̧" },
        { 'd', "a̲" },
        { 'e', "á" },
        { 'f', "a̮" },
        { 'g', "a̋" },
        { 'h', "a̰" },
        { 'i', "ả" },
        { 'j', "a̓" },
        { 'k', "ạ" },
        { 'l', "ă" },
        { 'm', "ǎ" },
        { 'n', "â" },
        { 'o', "å" },
        { 'p', "a̯" },
        { 'q', "a̤" },
        { 'r', "ȃ" },
        { 's', "ã" },
        { 't', "ā" },
        { 'u', "ä" },
        { 'v', "à" },
        { 'w', "ȁ" },
        { 'x', "a̽" },
        { 'y', "a̦" },
        { 'z', "ⱥ" },
    };

#if NET9_0_OR_GREATER
    private static readonly Dictionary<string, char>.AlternateLookup<ReadOnlySpan<char>> ScreamCipherToPlainTextMap = GetScreamCipherToPlainTextMap().GetAlternateLookup<ReadOnlySpan<char>>();
#else
    private static readonly Dictionary<string, char> ScreamCipherToPlainTextMap = GetScreamCipherToPlainTextMap();
#endif

    /// <summary>
    /// Initializes a new instance of the <see cref="ScreamCipher"/> class.
    /// </summary>
    protected ScreamCipher()
    {
    }

    /// <summary>
    /// Encodes the string.
    /// </summary>
    /// <param name="input">The string to encode.</param>
    /// <returns>The encoded string.</returns>
    public static string Encode(string input)
    {
#if NETSTANDARD1_0
        var stringBuider = new System.Text.StringBuilder();
        foreach (var c in input)
        {
            if (char.IsLetter(c))
            {
                stringBuider.Append(PlainTextToScreamCipherMap[c]);
            }
            else
            {
                stringBuider.Append(c);
            }
        }

        return stringBuider.ToString();
#else
        var source = input.AsSpan();
        Span<char> destination = stackalloc char[source.Length * 2];
        Encode(source, destination, out var charsWritten);
        return destination[..charsWritten].ToString();
#endif
    }

#if !NETSTANDARD1_0
    /// <summary>
    /// Encodes the character values.
    /// </summary>
    /// <param name="source">The characters to encode.</param>
    /// <param name="destination">The destination to write the encoded characters.</param>
    /// <param name="charsWritten">The number of characters written to <paramref name="destination"/>.</param>
    public static void Encode(ReadOnlySpan<char> source, Span<char> destination, out int charsWritten)
    {
        charsWritten = 0;
        foreach (var c in source)
        {
            if (char.IsLetter(c))
            {
                var v = PlainTextToScreamCipherMap[c];
                for (var i = 0; i < v.Length; i++)
                {
                    WriteCharacter(v[i], ref destination, ref charsWritten);
                }
            }
            else
            {
                WriteCharacter(c, ref destination, ref charsWritten);
            }
        }
    }
#endif

    /// <summary>
    /// Decodes the string.
    /// </summary>
    /// <param name="input">The string to decode.</param>
    /// <returns>The decoded string.</returns>
    public static string Decode(string input)
    {
#if !NETSTANDARD1_0
        var source = input.AsSpan();
        Span<char> destination = stackalloc char[source.Length];
        Decode(source, destination, out var charsWritten);
        return destination[..charsWritten].ToString();
#else
        var stringBuilder = new System.Text.StringBuilder();
        for (var i = 0; i < input.Length; i++)
        {
            var current = i + 1 > input.Length
                ? input.Substring(i, 1)
                : input.Substring(i, 2);

            var flag = false;
            while (current.Length > 0)
            {
                if (ScreamCipherToPlainTextMap.TryGetValue(current, out var plainText))
                {
                    stringBuilder.Append(plainText);
#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable S127
                    i += current.Length - 1;
#pragma warning restore S127, RCS1222, IDE0079
                    flag = true;
                    break;
                }

                current = current[..^1];
            }

            if (flag)
            {
                continue;
            }

            stringBuilder.Append(input[i]);
        }

        return stringBuilder.ToString();
#endif
    }

#if !NETSTANDARD1_0
    /// <summary>
    /// Decodes the character values.
    /// </summary>
    /// <param name="source">The characters to decode.</param>
    /// <param name="destination">The destination to write the decoded characters.</param>
    /// <param name="charsWritten">The number of characters written to <paramref name="destination"/>.</param>
    public static void Decode(ReadOnlySpan<char> source, Span<char> destination, out int charsWritten)
    {
        charsWritten = 0;

        while (source.Length > 0)
        {
            var current = source.Length < 2
                ? source[..1]
                : source[..2];

            var flag = false;
            while (current.Length > 0)
            {
#if NET9_0_OR_GREATER
                if (ScreamCipherToPlainTextMap.TryGetValue(current, out var plainText))
#else
                if (ScreamCipherToPlainTextMap.TryGetValue(current.ToString(), out var plainText))
#endif
                {
                    WriteCharacter(plainText, ref destination, ref charsWritten);
                    source = source[current.Length..];
                    flag = true;
                    break;
                }

                current = current[..^1];
            }

            if (flag)
            {
                continue;
            }

            WriteCharacter(source[0], ref destination, ref charsWritten);
            source = source[1..];
        }
    }
#endif

#if !NETSTANDARD1_0
    private static void WriteCharacter(char c, ref Span<char> destination, ref int charsWritten)
    {
        destination[charsWritten] = c;
        charsWritten++;
    }
#endif

    private static Dictionary<string, char> GetScreamCipherToPlainTextMap() => new(StringComparer.Ordinal)
    {
        { "A", 'A' },
        { "A̋", 'G' },
        { "A̓", 'J' },
        { "A̤", 'Q' },
        { "A̦", 'Y' },
        { "A̧", 'C' },
        { "A̮", 'F' },
        { "A̯", 'P' },
        { "A̰", 'H' },
        { "A̲", 'D' },
        { "A̽", 'X' },
        { "a", 'a' },
        { "a̋", 'g' },
        { "a̓", 'j' },
        { "a̤", 'q' },
        { "a̦", 'y' },
        { "a̧", 'c' },
        { "a̮", 'f' },
        { "a̯", 'p' },
        { "a̰", 'h' },
        { "a̲", 'd' },
        { "a̽", 'x' },
        { "À", 'V' },
        { "Á", 'E' },
        { "Â", 'N' },
        { "Ã", 'S' },
        { "Ä", 'U' },
        { "Å", 'O' },
        { "à", 'v' },
        { "á", 'e' },
        { "â", 'n' },
        { "ã", 's' },
        { "ä", 'u' },
        { "å", 'o' },
        { "Ā", 'T' },
        { "ā", 't' },
        { "Ă", 'L' },
        { "ă", 'l' },
        { "Ǎ", 'M' },
        { "ǎ", 'm' },
        { "Ȁ", 'W' },
        { "ȁ", 'w' },
        { "Ȃ", 'R' },
        { "ȃ", 'r' },
        { "Ȧ", 'B' },
        { "ȧ", 'b' },
        { "Ⱥ", 'Z' },
        { "Ạ", 'K' },
        { "ạ", 'k' },
        { "Ả", 'I' },
        { "ả", 'i' },
        { "ⱥ", 'z' },
    };
}