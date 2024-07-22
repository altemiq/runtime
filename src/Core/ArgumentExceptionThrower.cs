// -----------------------------------------------------------------------
// <copyright file="ArgumentExceptionThrower.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CS8777, SA1402, SA1403, SA1638, SA1649, S3236
namespace System;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="ArgumentException"/> helper.
/// </summary>
[Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This is part of the API now")]
#pragma warning disable IDE0079
[Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "Checked")]
#pragma warning restore IDE0079
#if NET7_0
[Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1194:Implement exception constructors", Justification = "Checked")]
[Diagnostics.CodeAnalysis.SuppressMessage("Minor Code Smell", "S3376:Attribute, EventArgs, and Exception type names should end with the type being extended", Justification = "Checked")]
[Diagnostics.CodeAnalysis.SuppressMessage("Naming", "MA0058:Class name should end with 'Exception'", Justification = "Checked")]
#endif
public class ArgumentExceptionEx
#if NET7_0
    : ArgumentException
#endif
{
    /// <summary>
    /// Initialises a new instance of the <see cref="ArgumentExceptionEx"/> class.
    /// </summary>
    protected ArgumentExceptionEx()
    {
    }

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
        if (string.IsNullOrEmpty(argument))
        {
            ThrowNullOrEmptyException(argument, paramName);
        }
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
        if (string.IsNullOrWhiteSpace(argument))
        {
            ThrowNullOrWhiteSpaceException(argument, paramName);
        }
    }

#if !NET7_0_OR_GREATER
    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowNullOrEmptyException(string? argument, string? paramName)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(argument, paramName);
        throw new ArgumentException(Altemiq.Properties.Resources.EmptyString, paramName);
    }
#endif

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowNullOrWhiteSpaceException(string? argument, string? paramName)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(argument, paramName);
        throw new ArgumentException(Altemiq.Properties.Resources.EmptyOrWhiteSpaceString, paramName);
    }
}
#pragma warning restore CS8777, SA1402, SA1403, SA1638, SA1649, S3236