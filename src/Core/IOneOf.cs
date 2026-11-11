// -----------------------------------------------------------------------
// <copyright file="IOneOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// Represents an option type.
/// </summary>
public interface IOneOf
#if NET11_0_OR_GREATER
    : System.Runtime.CompilerServices.IUnion
#endif
{
#if !NET11_0_OR_GREATER
    /// <summary>
    /// Gets the value.
    /// </summary>
    object? Value { get; }
#endif

    /// <summary>
    /// Gets the index.
    /// </summary>
    int Index { get; }
}