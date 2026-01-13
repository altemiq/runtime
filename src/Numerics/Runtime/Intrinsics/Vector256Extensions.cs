// -----------------------------------------------------------------------
// <copyright file="Vector256Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.Intrinsics;

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

#pragma warning disable CA1708, RCS1263, SA1101, S2325

/// <summary>
/// <see cref="Vector256"/> extensions.
/// </summary>
public static class Vector256Extensions
{
    extension(Vector256)
    {
#if NET10_0_OR_GREATER
        /// <inheritdoc cref="Numerics.Vector4D.All(Numerics.Vector4D, double)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool All(Numerics.Vector2D vector, double value) => vector.AsVector256() == Numerics.Vector2D.Create(value).AsVector256();

        /// <inheritdoc cref="Numerics.Vector4D.All(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool All(Numerics.Vector3D vector, double value) => vector.AsVector256() == Numerics.Vector3D.Create(value).AsVector256();

        /// <inheritdoc cref="Numerics.Vector4D.AllWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AllWhereAllBitsSet(Numerics.Vector2D vector) => vector.AsVector256().AsInt64() == Numerics.Vector2D.AllBitsSet.AsVector256().AsInt64();

        /// <inheritdoc cref="Numerics.Vector4D.AllWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AllWhereAllBitsSet(Numerics.Vector3D vector) => vector.AsVector256().AsInt64() == Numerics.Vector3D.AllBitsSet.AsVector256().AsInt64();

        /// <inheritdoc cref="Numerics.Vector4D.Any(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool Any(Numerics.Vector2D vector, double value) => Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, -1, -1));

        /// <inheritdoc cref="Numerics.Vector4D.Any(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool Any(Numerics.Vector3D vector, double value) => Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, value, -1));

        /// <inheritdoc cref="Numerics.Vector4D.AnyWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AnyWhereAllBitsSet(Numerics.Vector2D vector) => Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);

        /// <inheritdoc cref="Numerics.Vector4D.AnyWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AnyWhereAllBitsSet(Numerics.Vector3D vector) => Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);

        /// <inheritdoc cref="Numerics.Vector4D.Count(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Count(Numerics.Vector2D vector, double value) => System.Numerics.BitOperations.PopCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, -1, -1)).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.Count(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int Count(Numerics.Vector3D vector, double value) => System.Numerics.BitOperations.PopCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, value, -1)).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.CountWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CountWhereAllBitsSet(Numerics.Vector2D vector) => System.Numerics.BitOperations.PopCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.CountWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int CountWhereAllBitsSet(Numerics.Vector3D vector) => System.Numerics.BitOperations.PopCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.IndexOf(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int IndexOf(Numerics.Vector2D vector, double value)
        {
            var result = System.Numerics.BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, -1, -1)).ExtractMostSignificantBits());
            return (result != 32) ? result : -1;
        }

        /// <inheritdoc cref="Numerics.Vector4D.IndexOf(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int IndexOf(Numerics.Vector3D vector, double value)
        {
            var result = System.Numerics.BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, value, -1)).ExtractMostSignificantBits());
            return (result != 32) ? result : -1;
        }

        /// <inheritdoc cref="Numerics.Vector4D.IndexOfWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int IndexOfWhereAllBitsSet(Numerics.Vector2D vector)
        {
            var result = System.Numerics.BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());
            return (result != 32) ? result : -1;
        }

        /// <inheritdoc cref="Numerics.Vector4D.IndexOfWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int IndexOfWhereAllBitsSet(Numerics.Vector3D vector)
        {
            var result = System.Numerics.BitOperations.TrailingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());
            return (result != 32) ? result : -1;
        }

        /// <inheritdoc cref="Numerics.Vector4D.LastIndexOf(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int LastIndexOf(Numerics.Vector2D vector, double value) =>
            31 - System.Numerics.BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, -1, -1)).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.LastIndexOf(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int LastIndexOf(Numerics.Vector3D vector, double value) =>
            31 - System.Numerics.BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256(), Vector256.Create(value, value, value, -1)).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.LastIndexOfWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int LastIndexOfWhereAllBitsSet(Numerics.Vector2D vector) => 31 - System.Numerics.BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.LastIndexOfWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static int LastIndexOfWhereAllBitsSet(Numerics.Vector3D vector) => 31 - System.Numerics.BitOperations.LeadingZeroCount(Vector256.Equals(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet).ExtractMostSignificantBits());

        /// <inheritdoc cref="Numerics.Vector4D.None(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool None(Numerics.Vector2D vector, double value) => !Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, -1, -1));

        /// <inheritdoc cref="Numerics.Vector4D.None(Numerics.Vector4D, double)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool None(Numerics.Vector3D vector, double value) => !Vector256.EqualsAny(vector.AsVector256(), Vector256.Create(value, value, value, -1));

        /// <inheritdoc cref="Numerics.Vector4D.NoneWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool NoneWhereAllBitsSet(Numerics.Vector2D vector) => !Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);

        /// <inheritdoc cref="Numerics.Vector4D.NoneWhereAllBitsSet(Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool NoneWhereAllBitsSet(Numerics.Vector3D vector) => !Vector256.EqualsAny(vector.AsVector256().AsInt64(), Vector256<long>.AllBitsSet);
#endif

#if !NET9_0_OR_GREATER
        /// <inheritdoc cref="Numerics.Vector4D.MultiplyAddEstimate(Numerics.Vector4D,Numerics.Vector4D,Numerics.Vector4D)" />
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector256<double> MultiplyAddEstimate(Vector256<double> left, Vector256<double> right, Vector256<double> addend) =>
            Vector256.Create(
                Vector128.MultiplyAddEstimate(left.GetLower(), right.GetLower(), addend.GetLower()),
                Vector128.MultiplyAddEstimate(left.GetUpper(), right.GetUpper(), addend.GetUpper()));
#endif
    }

#if !NET8_0_OR_GREATER
    extension<T>(Vector256<T> value)
        where T : struct
    {
        /// <summary>
        /// Divides a vector by a scalar to compute the per-element quotient.
        /// </summary>
        /// <param name="left">The vector that will be divided by right.</param>
        /// <param name="right">The scalar that will divide left.</param>
        /// <returns>The quotient of left divided by right.</returns>
        public static Vector256<T> operator /(Vector256<T> left, T right) => left / Vector256.Create(right);
    }
    
    extension(Vector256<double>)
    {
        /// <summary>
        /// Shifts each element of a vector value by the specified amount.
        /// </summary>
        /// <param name="value">The vector whose elements are to be shifted.</param>
        /// <param name="shiftCount">The number of bits by which to shift each element.</param>
        /// <returns>A vector whose elements where shifted left by shiftCount.</returns>
        public static Vector256<double> operator <<(Vector256<double> value, int shiftCount) => Vector256.ShiftLeft(value.As<double, ulong>(), shiftCount).As<ulong, double>();

        /// <summary>
        /// Shifts (signed) each element of a vector right by the specified amount.
        /// </summary>
        /// <param name="value">The vector whose elements are to be shifted.</param>
        /// <param name="shiftCount">The number of bits by which to shift each element.</param>
        /// <returns>A vector whose elements where shifted right by shiftCount.</returns>
        public static Vector256<double> operator >>(Vector256<double> value, int shiftCount) => Vector256.ShiftRightArithmetic(value.As<double, long>(), shiftCount).As<long, double>();

        /// <summary>
        /// Shifts (unsigned) each element of a vector shiftCount by the specified amount.
        /// </summary>
        /// <param name="value">The vector whose elements are to be shifted.</param>
        /// <param name="shiftCount">The number of bits by which to shift each element.</param>
        /// <returns>A vector whose elements where shifted shiftCount by shiftCount.</returns>
        public static Vector256<double> operator >>>(Vector256<double> value, int shiftCount) => Vector256.ShiftRightLogical(value.As<double, ulong>(), shiftCount).As<ulong, double>();
    }
#endif

    /// <summary>The <see cref="Vector256{Double}"/> extensions.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    extension(Vector256<double> value)
    {
        /// <summary>Reinterprets a <see langword="Vector256{double}" /> as a new <see cref="Numerics.PlaneD" />.</summary>
        /// <returns>The input reinterpreted as a new <see cref="Numerics.PlaneD" />.</returns>
        public Numerics.PlaneD AsPlaneD() => Unsafe.BitCast<Vector256<double>, Numerics.PlaneD>(value);

        /// <summary>Reinterprets a <see langword="Vector256{Double}" /> as a new <see cref="Numerics.QuaternionD" />.</summary>
        /// <returns>The input reinterpreted as a new <see cref="Numerics.QuaternionD" />.</returns>
        public Numerics.QuaternionD AsQuaternionD() => Unsafe.BitCast<Vector256<double>, Numerics.QuaternionD>(value);

        /// <summary>Reinterprets a <see langword="Vector256{double}" /> as a new <see cref="Numerics.Vector2D" />.</summary>
        /// <returns>The input reinterpreted as a new <see cref="Numerics.Vector2D" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Numerics.Vector2D AsVector2D()
        {
            ref var address = ref Unsafe.As<Vector256<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Numerics.Vector2D>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector256{double}" /> as a new <see cref="Numerics.Vector3D" />.</summary>
        /// <returns>The input reinterpreted as a new <see cref="Numerics.Vector3D" />.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Numerics.Vector3D AsVector3D()
        {
            ref var address = ref Unsafe.As<Vector256<double>, byte>(ref value);
            return Unsafe.ReadUnaligned<Numerics.Vector3D>(ref address);
        }

        /// <summary>Reinterprets a <see langword="Vector256{double}" /> as a new <see cref="Numerics.Vector4D" />.</summary>
        /// <returns>The input reinterpreted as a new <see cref="Numerics.Vector4D" />.</returns>
        public Numerics.Vector4D AsVector4D() => Unsafe.BitCast<Vector256<double>, Numerics.Vector4D>(value);
    }

    /// <summary>The <see cref="Numerics.Vector2D"/> extensions.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    extension(Numerics.Vector2D value)
    {
        /// <summary>Reinterprets a <see langword="Numerics.Vector2" /> as a new <see cref="Vector256{Double}" /> with the new elements zeroed.</summary>
        /// <returns>The input reinterpreted as a new <see langword="Vector256{Double}" /> with the new elements zeroed.</returns>
        public Vector256<double> AsVector256() => Numerics.Vector4D.Create(value, 0, 0).AsVector256();

        /// <summary>Reinterprets a <see langword="Vector2" /> as a new <see cref="Vector256{Double}" />, leaving the new elements undefined.</summary>
        /// <returns>The input reinterpreted as a new <see langword="Vector256{Double}" />.</returns>
        public Vector256<double> AsVector256Unsafe()
        {
            // This relies on us stripping the "init" flag from the ".locals" declaration to let the upper bits be uninitialized.
            Unsafe.SkipInit(out Vector256<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<double>, byte>(ref result), value);
            return result;
        }
    }

    /// <summary>The <see cref="Numerics.Vector3D"/> extensions.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    extension(Numerics.Vector3D value)
    {
        /// <summary>Reinterprets a <see langword="Numerics.Vector3D" /> as a new <see cref="Vector256{Double}" /> with the new elements zeroed.</summary>
        /// <returns>The input reinterpreted as a new <see langword="Vector256{Double}" /> with the new elements zeroed.</returns>
        public Vector256<double> AsVector256() => Numerics.Vector4D.Create(value, 0).AsVector256();

        /// <summary>Reinterprets a <see langword="Numerics.Vector3D" /> as a new <see cref="Vector256{Double}" />, leaving the new elements undefined.</summary>
        /// <returns>The input reinterpreted as a new <see langword="Vector256{Double}" />.</returns>
        public Vector256<double> AsVector256Unsafe()
        {
            // This relies on us stripping the "init" flag from the ".locals" declaration to let the upper bits be uninitialized.
            Unsafe.SkipInit(out Vector256<double> result);
            Unsafe.WriteUnaligned(ref Unsafe.As<Vector256<double>, byte>(ref result), value);
            return result;
        }
    }

    /// <summary>Reinterprets a <see cref="Numerics.PlaneD" /> as a new <see langword="Vector256{double}" />.</summary>
    /// <param name="value">The plane to reinterpret.</param>
    /// <returns>The input reinterpreted as a new <see langword="Vector256{double}" />.</returns>
    public static Vector256<double> AsVector256(this Numerics.PlaneD value) => Unsafe.BitCast<Numerics.PlaneD, Vector256<double>>(value);

    /// <summary>Reinterprets a <see cref="Numerics.QuaternionD" /> as a new <see langword="Vector256{double}" />.</summary>
    /// <param name="value">The quaternion to reinterpret.</param>
    /// <returns>The input reinterpreted as a new <see langword="Vector256{double}" />.</returns>
    public static Vector256<double> AsVector256(this Numerics.QuaternionD value) => Unsafe.BitCast<Numerics.QuaternionD, Vector256<double>>(value);

    /// <summary>Reinterprets a <see langword="Numerics.Vector4D" /> as a new <see cref="Vector256{Double}" />.</summary>
    /// <param name="value">The vector to reinterpret.</param>
    /// <returns>The input reinterpreted as a new <see langword="Vector256{Double}" />.</returns>
    public static Vector256<double> AsVector256(this Numerics.Vector4D value) => Unsafe.BitCast<Numerics.Vector4D, Vector256<double>>(value);
}