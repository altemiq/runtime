// -----------------------------------------------------------------------
// <copyright file="SpanExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

#pragma warning disable CA1708, SA1101, RCS1263

/// <summary>
/// The <see cref="Span{T}"/> extensions.
/// </summary>
public static class SpanExtensions
{
    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(ref Span<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.IMultiplyOperators<T, T, T>
#else
        where T : struct, System.Numerics.IMultiplyOperators<T, T, T>
#endif
    {
        /// <summary>
        /// Creates a new instance of <see cref="Span{T}"/> from the scaling of the specified span by the specified scale.
        /// </summary>
        /// <param name="scale">A value to scale the span by.</param>
        /// <returns>A new instance of <see cref="Span{T}"/> from the scaling of the specified span by the specified scale.</returns>
        /// <seealso cref="SpanExtensions.Scale{T}(Span{T},T)"/>
        public void operator *=(T scale) => input.Scale(scale);
    }

    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(ref Span<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.IAdditionOperators<T, T, T>
#else
        where T : struct, System.Numerics.IAdditionOperators<T, T, T>
#endif
    {
        /// <summary>
        /// Creates a new instance of <see cref="Span{T}"/> from the addition of the two specified spans.
        /// </summary>
        /// <param name="other">The second instance of <see cref="Span{T}"/>.</param>
        /// <returns>A new instance of <see cref="Span{T}"/> from the addition of the two specified spans.</returns>
        /// <seealso cref="SpanExtensions.Add{T}(Span{T}, ReadOnlySpan{T})"/>
        public void operator +=(Span<T> other) => input.Add(other);
    }

    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(ref Span<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.ISubtractionOperators<T, T, T>
#else
        where T : struct, System.Numerics.ISubtractionOperators<T, T, T>
#endif
    {
        /// <summary>
        /// Creates a new instance of <see cref="Span{T}"/> from the subtraction of the two specified spans.
        /// </summary>
        /// <param name="other">The second instance of <see cref="ReadOnlySpan{T}"/>.</param>
        /// <returns>A new instance of <see cref="Span{T}"/> from the subtraction of the two specified spans.</returns>
        /// <seealso cref="SpanExtensions.Subtract{T}(Span{T}, ReadOnlySpan{T})"/>
        public void operator -=(Span<T> other) => input.Subtract(other);
    }

    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(Span<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.IMultiplyOperators<T, T, T>
#else
        where T : struct, System.Numerics.IMultiplyOperators<T, T, T>
#endif
    {
        /// <summary>
        /// Scales the specified span by the specified scale to the specified destination.
        /// </summary>
        /// <param name="multiplier">A double value to scale the span by.</param>
        /// <seealso cref="SpanExtensions.op_MultiplicationAssignment{T}"/>
        public void Scale(T multiplier)
        {
            var count = System.Numerics.Vector<T>.Count;
            var (quotient, remainder) = System.Math.DivRem(input.Length, count);
            int index;
            for (var i = 0; i < quotient; i++)
            {
                index = i * count;
                var vector = new System.Numerics.Vector<T>(input[index..]) * multiplier;
                vector.CopyTo(input[index..]);
            }

            index = quotient * count;
            for (var i = 0; i < remainder; i++)
            {
                input[index + i] *= multiplier;
            }
        }
    }

    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(Span<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.IAdditionOperators<T, T, T>
#else
        where T : struct, System.Numerics.IAdditionOperators<T, T, T>
#endif
    {
        internal void Add(ReadOnlySpan<T> other)
        {
            if (input.Length != other.Length)
            {
                throw new InvalidOperationException();
            }

            var count = System.Numerics.Vector<T>.Count;
            var (quotient, remainder) = System.Math.DivRem(input.Length, count);
            int index;
            for (var i = 0; i < quotient; i++)
            {
                index = i * count;
                var vector = new System.Numerics.Vector<T>(input[index..]) + new System.Numerics.Vector<T>(other[index..]);
                vector.CopyTo(input[index..]);
            }

            index = quotient * count;
            for (var i = 0; i < remainder; i++)
            {
                input[index + i] += other[index + i];
            }
        }
    }

    /// <summary>
    /// The <see cref="Span{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the span.</typeparam>
    /// <param name="input">The span to operate on.</param>
    extension<T>(Span<T> input)
#if NET8_0_OR_GREATER
        where T : System.Numerics.ISubtractionOperators<T, T, T>
#else
        where T : struct, System.Numerics.ISubtractionOperators<T, T, T>
#endif
    {
        internal void Subtract(ReadOnlySpan<T> other)
        {
            if (input.Length != other.Length)
            {
                throw new InvalidOperationException();
            }

            var count = System.Numerics.Vector<T>.Count;
            var (quotient, remainder) = System.Math.DivRem(input.Length, count);
            int index;
            for (var i = 0; i < quotient; i++)
            {
                index = i * count;
                var vector = new System.Numerics.Vector<T>(input[index..]) - new System.Numerics.Vector<T>(other[index..]);
                vector.CopyTo(input[index..]);
            }

            index = quotient * count;
            for (var i = 0; i < remainder; i++)
            {
                input[index + i] -= other[index + i];
            }
        }
    }
}