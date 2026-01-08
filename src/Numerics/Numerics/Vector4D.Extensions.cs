// -----------------------------------------------------------------------
// <copyright file="Vector4D.Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Altemiq.Runtime.Intrinsics;

#pragma warning disable CA1708, RCS1263, SA1101, S2325

/// <content>
/// <see cref="Vector4D"/> extensions.
/// </content>
public static unsafe partial class VectorD
{
    /// <summary>
    /// <see cref="Vector4D"/> extensions.
    /// </summary>
    /// <param name="value">The vector to reinterpret.</param>
    extension(Vector4D value)
    {
        /// <summary>Reinterprets a <see cref="Vector4D"/> as a new <see cref="PlaneD"/>.</summary>
        /// <returns>The input reinterpreted as a new <see cref="PlaneD"/>.</returns>
        public PlaneD AsPlaneD() => Unsafe.BitCast<Vector4D, PlaneD>(value);

        /// <summary>Reinterprets a <see cref="Vector4D"/> as a new <see cref="QuaternionD"/>.</summary>
        /// <returns>The input reinterpreted as a new <see cref="QuaternionD"/>.</returns>
        public QuaternionD AsQuaternionD() => Unsafe.BitCast<Vector4D, QuaternionD>(value);

        /// <summary>Reinterprets a <see cref="Vector4D"/> as a new <see cref="Vector2D"/>.</summary>
        /// <returns>The input reinterpreted as a new <see cref="Vector2D"/>.</returns>
        public Vector2D AsVector2D() => value.AsVector256().AsVector2D();

        /// <summary>Reinterprets a <see cref="Vector4D"/> as a new <see cref="Vector3D"/>.</summary>
        /// <returns>The input reinterpreted as a new <see cref="Vector3D" />.</returns>
        public Vector3D AsVector3D() => value.AsVector256().AsVector3D();

        /// <inheritdoc cref="Vector256.ExtractMostSignificantBits{T}(Vector256{T})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ExtractMostSignificantBits() => value.AsVector256().ExtractMostSignificantBits();

        /// <inheritdoc cref="Vector256.GetElement{T}(Vector256{T}, int)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetElement(int index) => value.AsVector256().GetElement(index);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="destination">The destination at which The input will be stored.</param>
        public void Store(double* destination) => value.AsVector256().Store(destination);

        /// <summary>Stores a vector at the given 16-byte aligned destination.</summary>
        /// <param name="destination">The aligned destination at which The input will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 16-byte aligned.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreAligned(double* destination) => value.AsVector256().StoreAligned(destination);

        /// <summary>Stores a vector at the given 16-byte aligned destination.</summary>
        /// <param name="destination">The aligned destination at which The input will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 16-byte aligned.</exception>
        /// <remarks>This method may bypass the cache on certain platforms.</remarks>
        public void StoreAlignedNonTemporal(double* destination) => value.AsVector256().StoreAlignedNonTemporal(destination);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="destination">The destination at which The input will be stored.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreUnsafe(ref double destination) => value.AsVector256().StoreUnsafe(ref destination);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="destination">The destination to which <paramref name="elementOffset" /> will be added before the vector will be stored.</param>
        /// <param name="elementOffset">The element offset from <paramref name="destination" /> from which the vector will be stored.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreUnsafe(ref double destination, nuint elementOffset) => value.AsVector256().StoreUnsafe(ref destination, elementOffset);

        /// <inheritdoc cref="Vector256.ToScalar{T}(Vector256{T})" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ToScalar() => value.AsVector256().ToScalar();

        /// <inheritdoc cref="Vector256.WithElement{T}(Vector256{T}, int, T)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector4D WithElement(int index, double value1) => value.AsVector256().WithElement(index, value1).AsVector4D();
    }
}