// -----------------------------------------------------------------------
// <copyright file="PlaneD.Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

#pragma warning disable CA1708, RCS1263, SA1101

/// <content>
/// <see cref="PlaneD"/> extensions.
/// </content>
public static partial class VectorD
{
    /// <content>
    /// <see cref="PlaneD"/> extensions.
    /// </content>
    /// <param name="value">The plane to reinterpret.</param>
    extension(PlaneD value)
    {
        /// <summary>Reinterprets a <see cref="PlaneD"/> as a new <see cref="Vector4D"/>.</summary>
        /// <returns>This instance reinterpreted as a new <see cref="Vector4D"/>.</returns>
        public Vector4D AsVector4D() => Unsafe.BitCast<PlaneD, Vector4D>(value);
    }
}