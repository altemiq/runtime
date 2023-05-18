// -----------------------------------------------------------------------
// <copyright file="ArgumentExceptionThrower.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CS8777, SA1402, SA1403, SA1638, SA1649, S3236
namespace System;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="ArgumentNullException"/> helper.
/// </summary>
public class ArgumentExceptionEx
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ArgumentExceptionEx"/> class.
    /// </summary>
    protected ArgumentExceptionEx()
    {
    }

    /// <summary>
    /// Throws an exception if <paramref name="argument"/> is null or empty.
    /// </summary>
    /// <param name="argument">The string argument to validate as non-null and non-empty.</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
    /// <exception cref="ArgumentNullException"><paramref name="argument"/> is null.</exception>
    /// <exception cref="ArgumentException"><paramref name="argument"/> is empty.</exception>
    public static void ThrowIfNullOrEmpty([Diagnostics.CodeAnalysis.NotNull] string? argument, [Runtime.CompilerServices.CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            ThrowNullOrEmptyException(argument, paramName);
        }
    }

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowNullOrEmptyException(string? argument, string? paramName)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(argument, paramName);
        throw new ArgumentException("The value cannot be an empty string", paramName);
    }
}
#pragma warning restore CS8777, SA1402, SA1403, SA1638, SA1649, S3236