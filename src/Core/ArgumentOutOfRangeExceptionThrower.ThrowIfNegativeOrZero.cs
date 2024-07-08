// -----------------------------------------------------------------------
// <copyright file="ArgumentOutOfRangeExceptionThrower.ThrowIfNegativeOrZero.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System;

/// <content>
/// Methods for when the value is negative.
/// </content>
public partial class ArgumentOutOfRangeExceptionEx
{
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
        const short Zero = (sbyte)0;
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
        const short Zero = (short)0;
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

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowNegativeOrZero<T>(T value, string? paramName) => throw new ArgumentOutOfRangeException(paramName, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeNonNegativeNonZero, paramName, value));
}