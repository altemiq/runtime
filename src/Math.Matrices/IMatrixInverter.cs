// -----------------------------------------------------------------------
// <copyright file="IMatrixInverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

/// <summary>
/// The <see cref="ReadOnlySpan2D{T}"/> inverter.
/// </summary>
public interface IMatrixInverter
{
    /// <summary>
    /// Gets the <see cref="IMatrixInverter"/> instance.
    /// </summary>
    static abstract IMatrixInverter Instance { get; }

    /// <summary>
    /// Inverts this instance.
    /// </summary>
    /// <typeparam name="T">The type of element in the matrix.</typeparam>
    /// <param name="matrix">The matrix.</param>
    /// <returns>The inverted matrix.</returns>
    ReadOnlySpan2D<T> Invert<T>(ReadOnlySpan2D<T> matrix)
#if NET8_0_OR_GREATER
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>;
#else
        where T : struct, System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>;
#endif
}