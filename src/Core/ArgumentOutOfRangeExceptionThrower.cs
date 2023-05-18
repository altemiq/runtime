// -----------------------------------------------------------------------
// <copyright file="ArgumentOutOfRangeExceptionThrower.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, SA1649
namespace System;

/// <summary>
/// <see cref="ArgumentOutOfRangeException"/> helper.
/// </summary>
public class ArgumentOutOfRangeExceptionEx
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ArgumentOutOfRangeExceptionEx"/> class.
    /// </summary>
    protected ArgumentOutOfRangeExceptionEx()
    {
    }

    /// <summary>
    /// Throws an exception if <paramref name="argument"/> is less than zero.
    /// </summary>
    /// <param name="argument">The value to validate as greater or equal to zero.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="argument"/> is less than zero.</exception>
    public static void ThrowIfLessThanZero(int argument, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument < 0)
        {
            ThrowLessThanZero(paramName);
        }
    }

    /// <summary>
    /// Throws an exception if <paramref name="argument"/> is less not between the specified minimum and maximum, inclusive.
    /// </summary>
    /// <param name="argument">The value to validate as greater or equal to zero.</param>
    /// <param name="minimum">The value that <paramref name="argument"/> cannot be less than.</param>
    /// <param name="maximum">The value that <paramref name="argument"/> cannot be more than.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="argument"/> is less than zero.</exception>
    public static void ThrowIfNotBetween(int argument, int minimum = int.MinValue, int maximum = int.MaxValue, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument < minimum || argument > maximum)
        {
            ThrowOutOfRange(paramName, minimum, maximum, argument);
        }
    }

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowLessThanZero(string? paramName) => throw new ArgumentOutOfRangeException(paramName, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeGreaterThanZero, paramName));

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowOutOfRange(string? paramName, int minimum, int maximum, int argument) => throw new ArgumentOutOfRangeException(paramName, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeBetween, paramName, minimum, maximum, argument));
}
#pragma warning restore IDE0130, SA1649