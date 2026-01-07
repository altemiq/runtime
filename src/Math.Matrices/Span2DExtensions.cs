// -----------------------------------------------------------------------
// <copyright file="Span2DExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

#pragma warning disable CA1708, SA1101, RCS1263

/// <summary>
/// The <see cref="Span2D{T}"/> extensions.
/// </summary>
public static class Span2DExtensions
{
    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ref Span2D<T> matrix)
        where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IAdditionOperators<T, T, T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the multiplication of the two specified matrices.
        /// </summary>
        /// <param name="other">The second instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the multiplication of the two specified matrices.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Multiply{T}(ReadOnlySpan2D{T}, ReadOnlySpan2D{T})"/>
        public void operator *=(ReadOnlySpan2D<T> other)
        {
            if (matrix.Height != other.Width)
            {
                throw new InvalidOperationException();
            }

            // copy this to a temporary matrix
            var temp = new ReadOnlySpan2D<T>(matrix.ToArray());
            if (matrix.TryGetSpan(out var matrixSpan))
            {
                ReadOnlySpan2D<T>.Multiply(temp, other, matrixSpan);
                return;
            }

            for (var column = 0; column < other.Width; column++)
            {
                for (var row = 0; row < temp.Height; row++)
                {
                    var rowEnumerator = temp.GetRow(row).GetEnumerator();
                    var columnEnumerator = other.GetColumn(column).GetEnumerator();

                    if (!rowEnumerator.MoveNext() || !columnEnumerator.MoveNext())
                    {
                        continue;
                    }

                    var total = rowEnumerator.Current * columnEnumerator.Current;
                    while (rowEnumerator.MoveNext() && columnEnumerator.MoveNext())
                    {
                        total += rowEnumerator.Current * columnEnumerator.Current;
                    }

                    matrix[row, column] = total;
                }
            }
        }

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.
        /// </summary>
        /// <param name="scale">A value to scale the matrix by.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Scale{T}(ReadOnlySpan2D{T},T)"/>
        public void operator *=(T scale)
        {
            if (matrix.TryGetSpan(out var span))
            {
                for (int i = 0; i < span.Length; i++)
                {
                    span[i] *= scale;
                }
            }
            else
            {
                for (var row = 0; row < matrix.Height; row++)
                {
                    var matrixRowSpan = matrix.GetRowSpan(row);
                    for (var column = 0; column < matrix.Width; column++)
                    {
                        matrixRowSpan[column] *= scale;
                    }
                }
            }
        }
    }

    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ref Span2D<T> matrix)
        where T : System.Numerics.IAdditionOperators<T, T, T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the addition of the two specified matrices.
        /// </summary>
        /// <param name="other">The second instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the addition of the two specified matrices.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Add{T}(ReadOnlySpan2D{T}, ReadOnlySpan2D{T})"/>
        public void operator +=(ReadOnlySpan2D<T> other)
        {
            if (matrix.Height != other.Height || matrix.Width != other.Width)
            {
                throw new InvalidOperationException();
            }

            if (matrix.TryGetSpan(out var matrixSpan1))
            {
                if (other.TryGetSpan(out var matrixSpan2))
                {
                    for (var i = 0; i < matrixSpan1.Length; i++)
                    {
                        matrixSpan1[i] += matrixSpan2[i];
                    }
                }
                else
                {
                    for (var row = 0; row < matrix.Height; row++)
                    {
                        var matrixRowSpan2 = other.GetRowSpan(row);
                        var rowIndex = row * matrix.Width;
                        for (var column = 0; column < matrix.Width; column++)
                        {
                            matrixSpan1[rowIndex + column] += matrixRowSpan2[column];
                        }
                    }
                }
            }
            else
            {
                for (var row = 0; row < matrix.Height; row++)
                {
                    var matrixRowSpan1 = matrix.GetRowSpan(row);
                    var matrixRowSpan2 = other.GetRowSpan(row);
                    for (var column = 0; column < matrix.Width; column++)
                    {
                        matrixRowSpan1[column] += matrixRowSpan2[column];
                    }
                }
            }
        }
    }

    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ref Span2D<T> matrix)
        where T : System.Numerics.ISubtractionOperators<T, T, T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the subtraction of the two specified matrices.
        /// </summary>
        /// <param name="other">The second instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the subtraction of the two specified matrices.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Subtract{T}(ReadOnlySpan2D{T}, ReadOnlySpan2D{T})"/>
        public void operator -=(ReadOnlySpan2D<T> other)
        {
            if (matrix.Height != other.Height || matrix.Width != other.Width)
            {
                throw new InvalidOperationException();
            }

            if (matrix.TryGetSpan(out var matrixSpan1))
            {
                if (other.TryGetSpan(out var matrixSpan2))
                {
                    for (var i = 0; i < matrixSpan1.Length; i++)
                    {
                        matrixSpan1[i] -= matrixSpan2[i];
                    }
                }
                else
                {
                    for (var row = 0; row < matrix.Height; row++)
                    {
                        var matrixRowSpan2 = other.GetRowSpan(row);
                        var rowIndex = row * matrix.Width;
                        for (var column = 0; column < matrix.Width; column++)
                        {
                            matrixSpan1[rowIndex + column] -= matrixRowSpan2[column];
                        }
                    }
                }
            }
            else
            {
                for (var row = 0; row < matrix.Height; row++)
                {
                    var matrixRowSpan1 = matrix.GetRowSpan(row);
                    var matrixRowSpan2 = other.GetRowSpan(row);
                    for (var column = 0; column < matrix.Width; column++)
                    {
                        matrixRowSpan1[column] -= matrixRowSpan2[column];
                    }
                }
            }
        }
    }
}