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
{
    /// <summary>
    /// Gets the value.
    /// </summary>
    object? Value { get; }

    /// <summary>
    /// Gets the index.
    /// </summary>
    int Index { get; }
}