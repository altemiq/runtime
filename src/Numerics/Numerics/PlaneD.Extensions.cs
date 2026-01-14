// -----------------------------------------------------------------------
// <copyright file="PlaneD.Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

#pragma warning disable CA1708

/// <content>
/// <see cref="PlaneD"/> extensions.
/// </content>
public static partial class VectorD
{
    /// <summary>Reinterprets a <see cref="PlaneD"/> as a new <see cref="Vector4D"/>.</summary>
    /// <param name="value">The plane to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="Vector4D"/>.</returns>
    public static Vector4D AsVector4D(this PlaneD value) => Unsafe.BitCast<PlaneD, Vector4D>(value);
}