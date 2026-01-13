// -----------------------------------------------------------------------
// <copyright file="QuaternionD.Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

using System.Runtime.CompilerServices;

/// <content>
/// <see cref="QuaternionD"/> extensions.
/// </content>
public static partial class VectorD
{
    /// <summary>Reinterprets a <see cref="QuaternionD"/> as a new <see cref="Vector4D"/>.</summary>
    /// <param name="value">The quaternion to reinterpret.</param>
    /// <returns><paramref name="value"/> reinterpreted as a new <see cref="QuaternionD" />.</returns>
    public static Vector4D AsVector4D(this QuaternionD value) => Unsafe.BitCast<QuaternionD, Vector4D>(value);
}