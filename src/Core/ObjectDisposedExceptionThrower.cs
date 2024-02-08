// -----------------------------------------------------------------------
// <copyright file="ObjectDisposedExceptionThrower.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, SA1649
namespace System;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="ArgumentOutOfRangeException"/> helper.
/// </summary>
[Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1711:Identifiers should not have incorrect suffix", Justification = "This is part of the API now")]
public sealed class ObjectDisposedExceptionEx
{
    private ObjectDisposedExceptionEx()
    {
    }

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

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowObjectDisposedException(object? instance) => ThrowObjectDisposedException(instance?.GetType());

    [Diagnostics.CodeAnalysis.DoesNotReturn]
    private static void ThrowObjectDisposedException(Type? type) => throw new ObjectDisposedException(type?.FullName);
}
#pragma warning restore SA1649