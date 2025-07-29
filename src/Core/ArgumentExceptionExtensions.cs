// -----------------------------------------------------------------------
// <copyright file="ArgumentExceptionExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System;
#pragma warning restore IDE0130, CheckNamespace

/// <summary>
/// Extensions for <see cref="ArgumentException"/> and derived types.
/// </summary>
public static class ArgumentExceptionExtensions
{
#if !NET8_0_OR_GREATER
    /// <summary>
    /// The <see cref="ArgumentException"/> extensions.
    /// </summary>
#pragma warning disable SA1137, SA1400, S1144
    extension(ArgumentException)
    {
#if !NET7_0_OR_GREATER
        /// <summary>
        /// Throws an exception if <paramref name="argument"/> is null or empty.
        /// </summary>
        /// <param name="argument">The string argument to validate as non-null and non-empty.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty.</exception>
        public static void ThrowIfNullOrEmpty([Diagnostics.CodeAnalysis.NotNull] string? argument, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            if (string.IsNullOrEmpty(argument))
            {
                ThrowNullOrEmptyException(argument, paramName);
            }
#else
            if (argument is null || string.IsNullOrEmpty(argument))
            {
                ThrowNullOrEmptyException(argument, paramName);
            }
#endif
        }
#endif

        /// <summary>
        /// Throws an exception if <paramref name="argument"/> is null, empty, or consists only of white-space characters.
        /// </summary>
        /// <param name="argument">The string argument to validate.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        /// <exception cref="ArgumentNullException"><paramref name="argument"/> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="argument"/> is empty or consists only of white-space characters.</exception>
        public static void ThrowIfNullOrWhiteSpace([Diagnostics.CodeAnalysis.NotNull] string? argument, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
#if NET5_0_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            if (string.IsNullOrWhiteSpace(argument))
            {
                ThrowNullOrWhiteSpaceException(argument, paramName);
            }
#else
            if (argument is null || string.IsNullOrWhiteSpace(argument))
            {
                ThrowNullOrWhiteSpaceException(argument, paramName);
            }
#endif
        }

#pragma warning disable IDE0051, S3236
#if !NET7_0_OR_GREATER
        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowNullOrEmptyException(string? argument, string? paramName)
        {
            ArgumentNullException.ThrowIfNull(argument, paramName);
            throw new ArgumentException(Altemiq.Properties.Resources.EmptyString, paramName);
        }
#endif

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowNullOrWhiteSpaceException(string? argument, string? paramName)
        {
            ArgumentNullException.ThrowIfNull(argument, paramName);
            throw new ArgumentException(Altemiq.Properties.Resources.EmptyOrWhiteSpaceString, paramName);
        }
#pragma warning restore IDE0051, S3236
    }
#endif

#if !NET6_0_OR_GREATER
    /// <summary>
    /// The <see cref="ArgumentNullException"/> extensions.
    /// </summary>
    extension(ArgumentNullException)
    {
        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.
        /// </summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        public static void ThrowIfNull([Diagnostics.CodeAnalysis.NotNull] object? argument, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

#pragma warning disable IDE0051
        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
#pragma warning restore IDE0051
    }
#endif

#if !NET8_0_OR_GREATER
    /// <summary>
    /// The <see cref="ArgumentOutOfRangeException"/> extensions.
    /// </summary>
    extension(ArgumentOutOfRangeException)
    {
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is equal to <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfEqual<T>(T value, T other, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : IEquatable<T>?
        {
            if (EqualityComparer<T>.Default.Equals(value, other))
            {
                ThrowEqual(value, other, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than to <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfGreaterThan<T>(T value, T other, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(other) > 0)
            {
                ThrowGreater(value, other, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is greater than or equal to <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfGreaterThanOrEqual<T>(T value, T other, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(other) >= 0)
            {
                ThrowGreaterThanOrEqual(value, other, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than to <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfLessThan<T>(T value, T other, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(other) < 0)
            {
                ThrowLess(value, other, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is less than or equal to <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfLessThanOrEqual<T>(T value, T other, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : IComparable<T>
        {
            if (value.CompareTo(other) <= 0)
            {
                ThrowLessThanOrEqual(value, other, paramName);
            }
        }

#if NET7_0_OR_GREATER
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative<T>(T value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : Numerics.INumberBase<T>
        {
            if (T.IsNegative(value))
            {
                ThrowNegative(value, paramName);
            }
        }
#else
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        [CLSCompliant(false)]
        public static void ThrowIfNegative(sbyte value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const sbyte Zero = 0;
            if (value < Zero)
            {
                ThrowNegative(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(short value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const short Zero = 0;
            if (value < Zero)
            {
                ThrowNegative(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(int value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0)
            {
                ThrowNegative(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(long value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0L)
            {
                ThrowNegative(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(float value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0F)
            {
                ThrowNegative(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(double value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0D)
            {
                ThrowNegative(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegative(decimal value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value < 0M)
            {
                ThrowNegative(value, paramName);
            }
        }
#endif

#if NET7_0_OR_GREATER
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegativeOrZero<T>(T value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : Numerics.INumberBase<T>
        {
            if (T.IsNegative(value) || T.IsZero(value))
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }
#else
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        [CLSCompliant(false)]
        public static void ThrowIfNegativeOrZero(sbyte value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const short Zero = 0;
            if (value <= Zero)
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegativeOrZero(short value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const short Zero = 0;
            if (value <= Zero)
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegativeOrZero(int value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0)
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegativeOrZero(long value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0L)
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegativeOrZero(float value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0F)
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegativeOrZero(double value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0D)
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is negative.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is negative.</exception>
        public static void ThrowIfNegativeOrZero(decimal value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value <= 0M)
            {
                ThrowNegativeOrZero(value, paramName);
            }
        }
#endif

#if NET7_0_OR_GREATER
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as non-zero.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfZero<T>(T value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : Numerics.INumberBase<T>
        {
            if (T.IsZero(value))
            {
                ThrowZero(value, paramName);
            }
        }
#else
        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is zero.</exception>
        [CLSCompliant(false)]
        public static void ThrowIfZero(sbyte value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const sbyte Zero = 0;
            if (value is Zero)
            {
                ThrowZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is zero.</exception>
        public static void ThrowIfZero(short value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            const short Zero = 0;
            if (value is Zero)
            {
                ThrowZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is zero.</exception>
        public static void ThrowIfZero(int value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value is 0)
            {
                ThrowZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is zero.</exception>
        public static void ThrowIfZero(long value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value is 0L)
            {
                ThrowZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is zero.</exception>
        public static void ThrowIfZero(float value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value is 0F)
            {
                ThrowZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is zero.</exception>
        public static void ThrowIfZero(double value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value is 0D)
            {
                ThrowZero(value, paramName);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is zero.
        /// </summary>
        /// <param name="value">The argument to validate as non-negative.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="value"/> is zero.</exception>
        public static void ThrowIfZero(decimal value, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
        {
            if (value is 0M)
            {
                ThrowZero(value, paramName);
            }
        }
#endif

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> if <paramref name="value"/> is not equal to <paramref name="other"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to validate.</typeparam>
        /// <param name="value">The argument to validate as not equal to <paramref name="other"/>.</param>
        /// <param name="other">The value to compare with <paramref name="value"/>.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="value"/> corresponds.</param>
        public static void ThrowIfNotEqual<T>(T value, T other, [Runtime.CompilerServices.CallerArgumentExpression(nameof(value))] string? paramName = null)
            where T : IEquatable<T>?
        {
            if (!EqualityComparer<T>.Default.Equals(value, other))
            {
                ThrowNotEqual(value, other, paramName);
            }
        }

#pragma warning disable IDE0051
        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowEqual<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeNotEqual, paramName, (object?)value ?? "null", (object?)other ?? "null"));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowGreater<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeLessOrEqual, paramName, (object?)value ?? "null", (object?)other ?? "null"));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowGreaterThanOrEqual<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeLess, paramName, (object?)value ?? "null", (object?)other ?? "null"));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowLess<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeGreaterOrEqual, paramName, (object?)value ?? "null", (object?)other ?? "null"));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowLessThanOrEqual<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeGreater, paramName, (object?)value ?? "null", (object?)other ?? "null"));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowNegative<T>(T value, string? paramName) => throw new ArgumentOutOfRangeException(paramName, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeNonNegative, paramName, value));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowNegativeOrZero<T>(T value, string? paramName) => throw new ArgumentOutOfRangeException(paramName, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeNonNegativeNonZero, paramName, value));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowZero<T>(T value, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeNonZero, paramName, value));

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowNotEqual<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeEqual, paramName, (object?)value ?? "null", (object?)other ?? "null"));
#pragma warning restore IDE0051
    }
#endif

#if !NET7_0_OR_GREATER
    /// <summary>
    /// The <see cref="ObjectDisposedException"/> extensions.
    /// </summary>
    extension(ObjectDisposedException)
    {
        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the specified <paramref name="condition"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="instance">The object whose type's full name should be included in any resulting <see cref="ObjectDisposedException"/>.</param>
        /// <exception cref="ObjectDisposedException">The <paramref name="condition"/> is <see langword="true"/>.</exception>
#if NET6_0_OR_GREATER
        [Diagnostics.StackTraceHidden]
#endif
        public static void ThrowIf([Diagnostics.CodeAnalysis.DoesNotReturnIf(true)] bool condition, object instance)
        {
            if (condition)
            {
                ThrowObjectDisposedException(instance);
            }
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> if the specified <paramref name="condition"/> is <see langword="true"/>.
        /// </summary>
        /// <param name="condition">The condition to evaluate.</param>
        /// <param name="type">The type whose full name should be included in any resulting <see cref="ObjectDisposedException"/>.</param>
        /// <exception cref="ObjectDisposedException">The <paramref name="condition"/> is <see langword="true"/>.</exception>
#if NET6_0_OR_GREATER
        [Diagnostics.StackTraceHidden]
#endif
        public static void ThrowIf([Diagnostics.CodeAnalysis.DoesNotReturnIf(true)] bool condition, Type type)
        {
            if (condition)
            {
                ThrowObjectDisposedException(type);
            }
        }

#pragma warning disable IDE0051
        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowObjectDisposedException(object? instance) => ThrowObjectDisposedException(instance?.GetType());

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowObjectDisposedException(Type? type) => throw new ObjectDisposedException(type?.FullName);
#pragma warning restore IDE0051
    }
#pragma warning restore SA1137, SA1400, S1144
#endif
}