// -----------------------------------------------------------------------
// <copyright file="Vector2D.Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Altemiq.Runtime.Intrinsics;

#pragma warning disable CA1708, RCS1263, SA1101, S2325

/// <content>
/// <see cref="Vector2D"/> extensions.
/// </content>
public static unsafe partial class VectorD
{
    /// <summary>
    /// <see cref="Vector2D"/> extensions.
    /// </summary>
    /// <param name="value">The vector to reinterpret.</param>
    extension(Vector2D value)
    {
        /// <summary>Reinterprets a <see cref="Vector2D"/> to a new <see cref="Vector3D"/> with the new elements zeroed.</summary>
        /// <returns>The input reinterpreted to a new <see cref="Vector3D"/> with the new elements zeroed.</returns>
        public Vector3D AsVector3D() => value.AsVector256().AsVector3D();

        /// <summary>Reinterprets a <see cref="Vector2D"/> to a new <see cref="Vector3D" /> with the new elements undefined.</summary>
        /// <returns>The input reinterpreted to a new <see cref="Vector3D" /> with the new elements undefined.</returns>
        public Vector3D AsVector3DUnsafe() => value.AsVector256Unsafe().AsVector3D();

        /// <summary>Reinterprets a <see cref="Vector2D" /> to a new <see cref="Vector4D" /> with the new elements zeroed.</summary>
        /// <returns>The input reinterpreted to a new <see cref="Vector4D" /> with the new elements zeroed.</returns>
        public Vector4D AsVector4D() => value.AsVector256().AsVector4D();

        /// <summary>Reinterprets a <see cref="Vector2D" /> to a new <see cref="Vector4D" /> with the new elements undefined.</summary>
        /// <returns>The input reinterpreted to a new <see cref="Vector4D" /> with the new elements undefined.</returns>
        public Vector4D AsVector4DUnsafe() => value.AsVector256Unsafe().AsVector4D();

        /// <inheritdoc cref="ExtractMostSignificantBits(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ExtractMostSignificantBits() => value.AsVector256().ExtractMostSignificantBits();

        /// <inheritdoc cref="GetElement(Vector4D, int)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double GetElement(int index)
        {
            if ((uint)index >= Vector2D.ElementCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return value.AsVector256Unsafe().GetElement(index);
        }

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="destination">The destination at which The input will be stored.</param>
        public void Store(double* destination) => value.StoreUnsafe(ref *destination);

        /// <summary>Stores a vector at the given 8-byte aligned destination.</summary>
        /// <param name="destination">The aligned destination at which The input will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 8-byte aligned.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0012:Do not raise reserved exception type", Justification = "This is valid")]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "This is valid")]
        public void StoreAligned(double* destination)
        {
            if (((nuint)destination % Vector2D.Alignment) != 0)
            {
                throw new AccessViolationException();
            }

            *(Vector2D*)destination = value;
        }

        /// <summary>Stores a vector at the given 8-byte aligned destination.</summary>
        /// <param name="destination">The aligned destination at which The input will be stored.</param>
        /// <exception cref="AccessViolationException"><paramref name="destination" /> is not 8-byte aligned.</exception>
        /// <remarks>This method may bypass the cache on certain platforms.</remarks>
        public void StoreAlignedNonTemporal(double* destination) => value.StoreAligned(destination);

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="destination">The destination at which The input will be stored.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreUnsafe(ref double destination)
        {
            ref var address = ref Unsafe.As<double, byte>(ref destination);
            Unsafe.WriteUnaligned(ref address, value);
        }

        /// <summary>Stores a vector at the given destination.</summary>
        /// <param name="destination">The destination to which <paramref name="elementOffset" /> will be added before the vector will be stored.</param>
        /// <param name="elementOffset">The element offset from <paramref name="destination" /> from which the vector will be stored.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void StoreUnsafe(ref double destination, nuint elementOffset)
        {
            destination = ref Unsafe.Add(ref destination, (nint)elementOffset);
            Unsafe.WriteUnaligned(ref Unsafe.As<double, byte>(ref destination), value);
        }

        /// <inheritdoc cref="ToScalar(Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double ToScalar() => value.AsVector256Unsafe().ToScalar();

        /// <inheritdoc cref="WithElement(Vector4D, int, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Vector2D WithElement(int index, double value1)
        {
            if ((uint)index >= Vector2D.ElementCount)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            return value.AsVector256Unsafe().WithElement(index, value1).AsVector2D();
        }
    }
}