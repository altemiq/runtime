// -----------------------------------------------------------------------
// <copyright file="Vector64Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NET9_0_OR_GREATER
namespace Altemiq.Runtime.Intrinsics;

/// <summary>
/// <see cref="Vector64"/> extensions.
/// </summary>
public static class Vector64Extensions
{
    extension(Vector64)
    {
        /// <inheritdoc cref="Numerics.Vector4D.MultiplyAddEstimate(Numerics.Vector4D,Numerics.Vector4D,Numerics.Vector4D)" />
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector64<double> MultiplyAddEstimate(Vector64<double> left, Vector64<double> right, Vector64<double> addend)
        {
            Unsafe.SkipInit(out Vector64<double> result);

            for (int index = 0; index < Vector64<double>.Count; index++)
            {
                double value = (left.GetElementUnsafe(index) * right.GetElementUnsafe(index)) + addend.GetElementUnsafe(index);
                result.SetElementUnsafe(index, value);
            }

            return result;
        }
    }

#pragma warning disable SA1101
    extension<T>(in Vector64<T> vector)
#if !NET8_0_OR_GREATER
        where T : struct
#endif
    {
       [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private T GetElementUnsafe(int index)
        {
            ref T address = ref Unsafe.As<Vector64<T>, T>(ref Unsafe.AsRef(in vector));
            return Unsafe.Add(ref address, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void SetElementUnsafe(int index, T value)
        {
            ref T address = ref Unsafe.As<Vector64<T>, T>(ref Unsafe.AsRef(in vector));
            Unsafe.Add(ref address, index) = value;
        }
    }
#pragma warning restore SA1101
}
#endif