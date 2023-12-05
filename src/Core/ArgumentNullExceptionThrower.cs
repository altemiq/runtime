// -----------------------------------------------------------------------
// <copyright file="ArgumentNullExceptionThrower.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, SA1402, SA1403, SA1638, SA1649
namespace System;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="ArgumentNullException"/> helper.
/// </summary>
[Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This is part of the API now")]
public sealed class ArgumentNullExceptionEx : ArgumentExceptionEx
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

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
}
#pragma warning restore SA1402, SA1403, SA1638, SA1649