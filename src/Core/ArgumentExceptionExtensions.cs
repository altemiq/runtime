// -----------------------------------------------------------------------
// <copyright file="ArgumentExceptionExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System;
#pragma warning restore IDE0130, CheckNamespace

#pragma warning disable CA1708
/// <summary>
/// Extensions for <see cref="ArgumentException"/> and derived types.
/// </summary>
public static class ArgumentExceptionExtensions
{
#if !NET8_0_OR_GREATER
    /// <summary>
    /// The <see cref="ArgumentOutOfRangeException"/> extensions.
    /// </summary>
    extension(ArgumentOutOfRangeException)
    {
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
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
                ArgumentOutOfRangeException.ThrowNegative(value, paramName);
            }
        }
#endif

        [Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ThrowNegative<T>(T value, string? paramName) => throw new ArgumentOutOfRangeException(paramName, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeNonNegative, paramName, value));
    }
#endif
}