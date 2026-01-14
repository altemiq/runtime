// -----------------------------------------------------------------------
// <copyright file="QuaternionD.Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

#pragma warning disable RCS1263, SA1101

/// <content>
/// <see cref="QuaternionD"/> extensions.
/// </content>
public static partial class VectorD
{
    /// <content>
    /// <see cref="QuaternionD"/> extensions.
    /// </content>
    /// <param name="value">The quaternion to reinterpret.</param>
    extension(QuaternionD value)
    {
        /// <summary>Reinterprets a <see cref="QuaternionD"/> as a new <see cref="Vector4D"/>.</summary>
        /// <returns>This value reinterpreted as a new <see cref="Vector4D" />.</returns>
        public Vector4D AsVector4D() => Unsafe.BitCast<QuaternionD, Vector4D>(value);
    }
}