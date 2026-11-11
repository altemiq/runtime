// -----------------------------------------------------------------------
// <copyright file="ReadOnlySpanExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

/// <summary>
/// The <see cref="ReadOnlySpan{T}"/> extensions.
/// </summary>
public static class ReadOnlySpanExtensions
{
    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(ReadOnlySpan<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.IMultiplyOperators<T, T, T>
#else
        where T : struct, System.Numerics.IMultiplyOperators<T, T, T>
#endif
    {
        /// <summary>
        /// Multiplies this instance by the specified <see cref="ReadOnlySpan{T}"/> to the destination.
        /// </summary>
        /// <param name="multiplier">The multiplier.</param>
        /// <param name="destination">The destination.</param>
        internal void ScaleTo(T multiplier, Span<T> destination)
        {
            var count = System.Numerics.Vector<T>.Count;
            var (quotient, remainder) = System.Math.DivRem(input.Length, count);
            var length = quotient * count;
            if (quotient > 0)
            {
                var multiplierVector = new System.Numerics.Vector<T>(multiplier);
                for (var i = 0; i < length; i += count)
                {
                    var vector = new System.Numerics.Vector<T>(input[i..]) * multiplierVector;
                    vector.CopyTo(destination[i..]);
                }
            }

            for (var i = 0; i < remainder; i++)
            {
                destination[length + i] = input[length + i] * multiplier;
            }
        }
    }

    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(ReadOnlySpan<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.IAdditionOperators<T, T, T>
#else
        where T : struct, System.Numerics.IAdditionOperators<T, T, T>
#endif
    {
        /// <summary>
        /// Adds this instance and the specified <see cref="ReadOnlySpan{T}"/> to the destination.
        /// </summary>
        /// <param name="other">The other span to add.</param>
        /// <param name="destination">The destination.</param>
        /// <exception cref="InvalidOperationException">The lengths of <paramref name="input"/> and <paramref name="other"/> do not match.</exception>
        internal void AddTo(ReadOnlySpan<T> other, Span<T> destination)
        {
            if (input.Length != other.Length || input.Length != destination.Length)
            {
                throw new InvalidOperationException();
            }

            var count = System.Numerics.Vector<T>.Count;
            var (quotient, remainder) = System.Math.DivRem(input.Length, count);
            var length = quotient * count;
            for (var i = 0; i < length; i += count)
            {
                var vector = new System.Numerics.Vector<T>(input[i..]) + new System.Numerics.Vector<T>(other[i..]);
                vector.CopyTo(destination[i..]);
            }

            for (var i = 0; i < remainder; i++)
            {
                destination[length + i] = input[length + i] + other[length + i];
            }
        }
    }

    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(ReadOnlySpan<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.ISubtractionOperators<T, T, T>
#else
        where T : struct, System.Numerics.ISubtractionOperators<T, T, T>
#endif
    {
        /// <summary>
        /// Subtracts the specified <see cref="ReadOnlySpan{T}"/> from this instance and to the destination.
        /// </summary>
        /// <param name="other">The other span to add.</param>
        /// <param name="destination">The destination.</param>
        /// <exception cref="InvalidOperationException">The lengths of <paramref name="input"/> and <paramref name="other"/> do not match.</exception>
        internal void SubtractTo(ReadOnlySpan<T> other, Span<T> destination)
        {
            if (input.Length != other.Length || input.Length != destination.Length)
            {
                throw new InvalidOperationException();
            }

            var count = System.Numerics.Vector<T>.Count;
            var (quotient, remainder) = System.Math.DivRem(input.Length, count);
            var length = quotient * count;
            for (var i = 0; i < length; i += count)
            {
                var vector = new System.Numerics.Vector<T>(input[i..]) - new System.Numerics.Vector<T>(other[i..]);
                vector.CopyTo(destination[i..]);
            }

            for (var i = 0; i < remainder; i++)
            {
                destination[length + i] = input[length + i] - other[length + i];
            }
        }
    }
}