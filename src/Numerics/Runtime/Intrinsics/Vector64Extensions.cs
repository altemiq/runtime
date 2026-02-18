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

            for (var index = 0; index < Vector64<double>.Count; index++)
            {
                var value = (GetElementUnsafe(left, index) * GetElementUnsafe(right, index)) + GetElementUnsafe(addend, index);
                SetElementUnsafe(result, index, value);
            }

            return result;
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T GetElementUnsafe<T>(in Vector64<T> vector, int index)
#if !NET8_0_OR_GREATER
        where T : struct
#endif
    {
        ref T address = ref Unsafe.As<Vector64<T>, T>(ref Unsafe.AsRef(in vector));
        return Unsafe.Add(ref address, index);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void SetElementUnsafe<T>(in Vector64<T> vector, int index, T value)
#if !NET8_0_OR_GREATER
        where T : struct
#endif
    {
        ref T address = ref Unsafe.As<Vector64<T>, T>(ref Unsafe.AsRef(in vector));
        Unsafe.Add(ref address, index) = value;
    }
}
#endif