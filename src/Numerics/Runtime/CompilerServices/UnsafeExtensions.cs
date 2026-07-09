// -----------------------------------------------------------------------
// <copyright file="UnsafeExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NET8_0_OR_GREATER

#pragma warning disable IDE0130, CheckNamespace
namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130, CheckNamespace

/// <summary>
/// The <see cref="Unsafe"/> extensions.
/// </summary>
public static class UnsafeExtensions
{
    extension(Unsafe)
    {
        /// <summary>
        /// Reinterprets the given value of type <typeparamref name="TFrom" /> as a value of type <typeparamref name="TTo" />.
        /// </summary>
        /// <typeparam name="TFrom">The type to cast from.</typeparam>
        /// <typeparam name="TTo">The type to cast to.</typeparam>
        /// <param name="source">The source.</param>
        /// <exception cref="NotSupportedException">The size of <typeparamref name="TFrom" /> and <typeparamref name="TTo" /> are not the same.</exception>
        [Intrinsic]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TTo BitCast<TFrom, TTo>(TFrom source)
            where TFrom : struct
            where TTo : struct
        {
#pragma warning disable S6640 // Unsafe code blocks should not be used
            unsafe
            {
#pragma warning disable CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
                if (sizeof(TFrom) != sizeof(TTo))
                {
                    throw new NotSupportedException();
                }
#pragma warning restore CS8500 // This takes the address of, gets the size of, or declares a pointer to a managed type
            }
#pragma warning restore S6640 // Unsafe code blocks should not be used

            return Unsafe.As<byte, TTo>(ref Unsafe.As<TFrom, byte>(ref source));
        }
    }
}
#endif