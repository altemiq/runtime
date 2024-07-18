// -----------------------------------------------------------------------
// <copyright file="ArgumentOutOfRangeExceptionThrower.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System;

/// <summary>
/// <see cref="ArgumentOutOfRangeException"/> helper.
/// </summary>
[Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1863:Use 'CompositeFormat'", Justification = "This is for exceptions, taken from resources")]
#pragma warning disable IDE0079
[Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Checked")]
#pragma warning restore IDE0079
public partial class ArgumentOutOfRangeExceptionEx
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
#pragma warning disable S1133
    [Obsolete($"Use {nameof(ThrowIfNegative)} instead")]
#pragma warning restore S1133
    public static void ThrowIfLessThanZero(int argument, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null) => ThrowIfNegative(argument, paramName);

    /// <summary>
    /// Throws an exception if <paramref name="argument"/> is less not between the specified minimum and maximum, inclusive.
    /// </summary>
    /// <param name="argument">The value to validate as between <paramref name="minimum"/> and <paramref name="maximum"/>.</param>
    /// <param name="minimum">The value that <paramref name="argument"/> cannot be less than.</param>
    /// <param name="maximum">The value that <paramref name="argument"/> cannot be more than.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="argument"/> is less than zero.</exception>
#pragma warning disable S1133
    [Obsolete($"Use {nameof(ThrowIfGreaterThan)} and {nameof(ThrowIfLessThan)} instead")]
#pragma warning restore S1133
    public static void ThrowIfNotBetween(int argument, int minimum = int.MinValue, int maximum = int.MaxValue, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument < minimum || argument > maximum)
        {
            ThrowOutOfRange(paramName, argument, minimum, maximum);
        }
    }

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowOutOfRange(string? paramName, int value, int minimum, int maximum) => throw new ArgumentOutOfRangeException(paramName, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeBetween, paramName, value, minimum, maximum));
}