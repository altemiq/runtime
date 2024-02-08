// -----------------------------------------------------------------------
// <copyright file="ArgumentOutOfRangeExceptionThrower.ThrowIfNotEqual.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System;

/// <content>
/// Methods for when the value does not equal.
/// </content>
public partial class ArgumentOutOfRangeExceptionEx
{
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

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowNotEqual<T>(T value, T other, string? paramName) => throw new ArgumentOutOfRangeException(paramName, value, string.Format(Altemiq.Properties.Resources.Culture, Altemiq.Properties.Resources.MustBeEqual, paramName, (object?)value ?? "null", (object?)other ?? "null"));
}