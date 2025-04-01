// -----------------------------------------------------------------------
// <copyright file="ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThanOrEqual.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System;

/// <content>
/// Methods for when the value is greater than or equal.
/// </content>
public partial class ArgumentOutOfRangeExceptionEx
{
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

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowGreaterThanOrEqual<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeLess, paramName, (object?)value ?? "null", (object?)other ?? "null"));
}