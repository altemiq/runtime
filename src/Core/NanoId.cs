// -----------------------------------------------------------------------
// <copyright file="NanoId.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

#if NETSTANDARD1_3_OR_GREATER || NET || NETFRAMEWORK
using Random = Altemiq.Security.Cryptography.Random;
#endif

/// <summary>
/// Static class containing all functions and constants related to <see cref="NanoId"/>.
/// </summary>
public static class NanoId
{
    /// <summary>
    /// The default size of a <see cref="NanoId"/>.
    /// </summary>
    public const int DefaultIdSize = 21;

    /// <summary>
    /// Gets the global <see cref="Random"/> instance used to conveniently generate <see cref="NanoId"/>.
    /// </summary>
    /// <remarks>Lazily initialized in order to account for the <see cref="ThreadStaticAttribute"/> (learn more here: https://stackoverflow.com/a/18086509).</remarks>
    [field: ThreadStatic]
    [field: System.Diagnostics.CodeAnalysis.MaybeNull]
    public static Random GlobalRandom
        => field ??=
#if NETSTANDARD1_3_OR_GREATER || NET || NETFRAMEWORK
            new Random();
#else
            new Random((int)DateTime.UtcNow.Ticks);
#endif

    /// <summary>
    /// Generate a <see cref="NanoId"/> using a global instance of <see cref="Random"/>.
    /// </summary>
    /// <param name="alphabet">The set of characters used in generating the id. Defaults to <see cref="Alphabets.Default"/>.</param>
    /// <param name="size">The length of the id. Defaults to <see cref="DefaultIdSize"/>.</param>
    /// <returns>
    /// <para>A task that represents the asynchronous operation.</para>
    /// <para>The task result contains a new string representing a random <see cref="NanoId"/> with the specified <paramref name="alphabet"/> and <paramref name="size"/>.</para>
    /// </returns>
    /// <exception cref="ArgumentNullException">If any of the provided arguments are null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="alphabet"/>'s length is outside the range [0, 256] or if <paramref name="size"/> is less than or equal to 0.</exception>
    public static Task<string> GenerateAsync(string alphabet = Alphabets.Default, int size = DefaultIdSize)
    {
        Validate(alphabet, size);

        return Task.Run(() => GenerateImpl(GlobalRandom, alphabet, size));
    }

    /// <summary>
    /// Generate a Nanoid.
    /// </summary>
    /// <param name="random">A random number generator.</param>
    /// <param name="alphabet">The set of characters used in generating the id. Defaults to <see cref="Alphabets.Default"/>.</param>
    /// <param name="size">The length of the id. Defaults to <see cref="DefaultIdSize"/>.</param>
    /// <returns>
    /// A task that represents the asynchronous operation.
    /// The task result contains a new string representing a random nanoid with the specified <paramref name="alphabet"/> and <paramref name="size"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">If any of the provided arguments are null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="alphabet"/>'s length is outside the range [0, 256] or if <paramref name="size"/> is less than or equal to 0.</exception>
    public static Task<string> GenerateAsync(System.Random random, string alphabet = Alphabets.Default, int size = DefaultIdSize)
    {
        Validate(alphabet, size);

        return Task.Run(() => GenerateImpl(random, alphabet, size));
    }

    /// <summary>
    /// Generate a Nanoid using a global instance of <see cref="Random"/>.
    /// </summary>
    /// <param name="alphabet">The set of characters used in generating the id. Defaults to <see cref="Alphabets.Default"/>.</param>
    /// <param name="size">The length of the id. Defaults to <see cref="DefaultIdSize"/>.</param>
    /// <returns>A new string representing a random nanoid with the specified <paramref name="alphabet"/> and <paramref name="size"/>.</returns>
    /// <exception cref="ArgumentNullException">If any of the provided arguments are null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="alphabet"/>'s length is outside the range [0, 256] or if <paramref name="size"/> is less than or equal to 0.</exception>
    public static string Generate(string alphabet = Alphabets.Default, int size = 21)
    {
        Validate(alphabet, size);

        return GenerateImpl(GlobalRandom, alphabet, size);
    }

    /// <summary>
    /// Generate a Nanoid.
    /// </summary>
    /// <param name="random">A random number generator.</param>
    /// <param name="alphabet">The set of characters used in generating the id. Defaults to <see cref="Alphabets.Default"/>.</param>
    /// <param name="size">The length of the id. Defaults to <see cref="DefaultIdSize"/>.</param>
    /// <returns>A new string representing a random nanoid with the specified <paramref name="alphabet"/> and <paramref name="size"/>.</returns>
    /// <exception cref="ArgumentNullException">If any of the provided arguments are null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">If <paramref name="alphabet"/>'s length is outside the range [0, 256] or if <paramref name="size"/> is less than or equal to 0.</exception>
    public static string Generate(System.Random random, string alphabet = Alphabets.Default, int size = 21)
    {
        Validate(alphabet, size);

        return GenerateImpl(random, alphabet, size);
    }

#if !USE_GENERIC_MATH
    /// <summary>
    /// Counts the leading zeros of <paramref name="x"/>.
    /// </summary>
    /// <param name="x">The number to get the leading zeros count for.</param>
    /// <returns>The number of leading zeros.</returns>
    /// <remarks>Courtesy of spender/Sunsetquest see https://stackoverflow.com/a/10439333/623392.</remarks>
    internal static int LeadingZeroCount(int x)
    {
        const int NumberOfIntegerBits = sizeof(int) * 8;

        // do the smearing
        x |= x >> 01;
        x |= x >> 02;
        x |= x >> 04;
        x |= x >> 08;
        x |= x >> 16;

        // count the ones
        x -= (x >> 01) & 0x55555555;
        x = ((x >> 02) & 0x33333333) + (x & 0x33333333);
        x = ((x >> 04) + x) & 0x0F0F0F0F;
        x += x >> 08;
        x += x >> 16;

        // subtract # of 1s from 32
        return NumberOfIntegerBits - (x & 0x0000003F);
    }
#endif

    private static void Validate(string alphabet, int size)
    {
        ArgumentNullException.ThrowIfNull(alphabet);
        ArgumentOutOfRangeException.ThrowIfLessThan(alphabet.Length, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(alphabet.Length, byte.MaxValue);
        ArgumentOutOfRangeException.ThrowIfLessThanOrEqual(size, 0);
    }

    private static string GenerateImpl(System.Random random, string alphabet = Alphabets.Default, int size = 21)
    {
        // See https://github.com/ai/nanoid/blob/master/format.js for an explanation as to why masking with `random % alphabet` is a common mistake security-wise.
#if USE_GENERIC_MATH
        var mask = (2 << (31 - int.LeadingZeroCount((alphabet.Length - 1) | 1))) - 1;
#else
        var mask = (2 << (31 - LeadingZeroCount((alphabet.Length - 1) | 1))) - 1;
#endif

        // Original dev notes regarding this algorithm.
        // Source: https://github.com/ai/nanoid/blob/0454333dee4612d2c2e163d271af6cc3ce1e5aa4/index.js#L45
        //
        // "Next, a step determines how many random bytes to generate.
        // The number of random bytes gets decided upon the ID length, mask, alphabet length, and magic number 1.6 (using 1.6 peaks at performance according to benchmarks)."
        var step = (int)Math.Ceiling(1.6 * mask * size / alphabet.Length);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<char> builder = stackalloc char[size];
        Span<byte> bytes = stackalloc byte[step];
#else
        var builder = new char[size];
        var bytes = new byte[step];
#endif

        var cnt = 0;

        while (true)
        {
            random.NextBytes(bytes);

            for (var i = 0; i < step; i++)
            {
                var alphabetIndex = bytes[i] & mask;

                if (alphabetIndex >= alphabet.Length)
                {
                    continue;
                }

                builder[cnt] = alphabet[alphabetIndex];
                if (++cnt == size)
                {
                    return new string(builder);
                }
            }
        }
    }

    /// <summary>
    /// Useful alphabets for <see cref="NanoId"/> generation.
    /// </summary>
    /// <remarks>Taken from <see href="https://github.com/CyberAP/nanoid-dictionary"/> and <see href="https://github.com/SasLuca/zig-nanoid/blob/91e0a9a8890984f3dcdd98c99002a05a83d0ee89/src/nanoid.zig#L4"/>.</remarks>
    public static class Alphabets
    {
        /// <summary>
        /// All digits [0, 9].
        /// </summary>
        public const string Digits = "0123456789";

        /// <summary>
        /// English hexadecimal with lowercase characters.
        /// </summary>
        public const string HexadecimalLowercase = Digits + "abcdef";

        /// <summary>
        /// English hexadecimal with uppercase characters.
        /// </summary>
        public const string HexadecimalUppercase = Digits + "ABCDEF";

        /// <summary>
        /// Lowercase English alphabet letters.
        /// </summary>
        public const string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Uppercase English alphabet letters.
        /// </summary>
        public const string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Lowercase and uppercase English alphabet letters.
        /// </summary>
        public const string Letters = LowercaseLetters + UppercaseLetters;

        /// <summary>
        /// Lowercase English alphabet letters and digits.
        /// </summary>
        public const string LowercaseLettersAndDigits = Digits + LowercaseLetters;

        /// <summary>
        /// Uppercase English alphabet letters and digits.
        /// </summary>
        public const string UppercaseLettersAndDigits = Digits + UppercaseLetters;

        /// <summary>
        /// English alphabet letters and digits.
        /// </summary>
        public const string LettersAndDigits = Digits + LowercaseLetters + UppercaseLetters;

        /// <summary>
        /// English alphabet letters and digits without lookalikes: 1, l, I, 0, O, o, u, v, 5, S, s, 2, Z.
        /// </summary>
        public const string NoLookAlike = SubAlphabets.NoLookAlikeDigits + SubAlphabets.NoLookAlikeLetters;

        /// <summary>
        /// English alphabet letters and digits without lookalikes (1, l, I, 0, O, o, u, v, 5, S, s, 2, Z) and with removed vowels and the following letters: 3, 4, x, X, V.
        /// </summary>
        /// <remarks>This list should protect you from accidentally getting obscene words in generated strings.</remarks>
        public const string NoLookAlikeSafe = SubAlphabets.NoLookAlikeSafeDigits + SubAlphabets.NoLookAlikeSafeLetters;

        /// <summary>
        /// The default alphabet used by Nanoid. Includes ascii digits, letters and the symbols "_" and "-".
        /// </summary>
        public const string Default = SubAlphabets.Symbols + LettersAndDigits;

        /// <summary>
        /// Used for composition and documentation in building proper <see cref="Alphabets"/>.
        /// </summary>
        /// <remarks>Not recommended to be used on their own as alphabets for <see cref="NanoId"/>.</remarks>
        public static class SubAlphabets
        {
            /// <summary>
            /// All digits that don't look similar to other digits or letters.
            /// </summary>
            public const string NoLookAlikeDigits = "346789";

            /// <summary>
            /// All lowercase letters that don't look similar to other letters.
            /// </summary>
            public const string NoLookAlikeLettersLowercase = "abcdefghijkmnpqrtwxyz";

            /// <summary>
            /// All uppercase letters that don't look similar to other letters.
            /// </summary>
            public const string NoLookAlikeLettersUppercase = "ABCDEFGHJKLMNPQRTUVWXY";

            /// <summary>
            /// All letters that don't look similar to other letters.
            /// </summary>
            public const string NoLookAlikeLetters = NoLookAlikeLettersUppercase + NoLookAlikeLettersLowercase;

            /// <summary>
            /// All digits that don't look similar to other digits or letters and prevent potential obscene words from appearing in generated ids.
            /// </summary>
            public const string NoLookAlikeSafeDigits = "6789";

            /// <summary>
            /// All lowercase letters that don't look similar to other digits or letters and prevent potential obscene words from appearing in generated ids.
            /// </summary>
            public const string NoLookAlikeSafeLettersLowercase = "bcdfghjkmnpqrtwz";

            /// <summary>
            /// All uppercase letters that don't look similar to other digits or letters and prevent potential obscene words from appearing in generated ids.
            /// </summary>
            public const string NoLookAlikeSafeLettersUppercase = "BCDFGHJKLMNPQRTW";

            /// <summary>
            /// All letters that don't look similar to other digits or letters and prevent potential obscene words from appearing in generated ids.
            /// </summary>
            public const string NoLookAlikeSafeLetters = NoLookAlikeLettersUppercase + NoLookAlikeLettersLowercase;

            /// <summary>
            /// <see cref="Uri"/> safe symbols that can be used in a <see cref="NanoId"/>. Part of the default alphabet.
            /// </summary>
            public const string Symbols = "_-";
        }
    }
}