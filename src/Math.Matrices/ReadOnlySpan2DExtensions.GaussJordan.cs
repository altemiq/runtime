// -----------------------------------------------------------------------
// <copyright file="ReadOnlySpan2DExtensions.GaussJordan.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

#pragma warning disable SA1101, RCS1263

/// <content>
/// The <see href="https://en.wikipedia.org/wiki/Invertible_matrix#Gaussian_elimination"/>.
/// </content>
public static partial class ReadOnlySpan2DExtensions
{
    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.INumberBase<T>
    {
        /// <summary>
        /// Creates a new <see cref="ReadOnlySpan2D{T}"/> from the inverse of the current instance of <see cref="ReadOnlySpan2D{T}"/> using Gaussian elimination.
        /// </summary>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/>.</returns>
        public ReadOnlySpan2D<T> InvertGaussian()
        {
            var degree = matrix.Width * 2;

            // Get the matrix
            var value = new Span2D<T>(new T[matrix.Height, degree]);

            for (var i = 0; i < matrix.Height; i++)
            {
                matrix.GetRowSpan(i).CopyTo(value.GetRowSpan(i));
            }

            // Augment Identity matrix onto matrix M to get [M|I]
            for (var i = matrix.Width; i < degree; i++)
            {
                value[i - matrix.Width, i] = T.One;
            }

            // Reduce so we get the leading diagonal
            for (var pivotRow = 0; pivotRow < value.Height; pivotRow++)
            {
                var pivot = value[pivotRow, pivotRow];

                for (var row = 0; row < value.Height; row++)
                {
                    if (pivotRow == row)
                    {
                        continue;
                    }

                    var temp = value[row, pivotRow] / pivot;

                    if (T.IsNaN(temp))
                    {
                        throw new InvalidOperationException();
                    }

                    // Invert the value
                    temp = -temp;

                    for (var column = 0; column < value.Width; column++)
                    {
                        value[row, column] += value[pivotRow, column] * temp;
                    }
                }
            }

            // Divide through to get the identity element
            // Note: the identity element may have very small values (close to zero) because of the way floating points are stored.
            for (var row = 0; row < matrix.Height; row++)
            {
                var divisor = value[row, row];

                for (var column = 0; column < degree; column++)
                {
                    value[row, column] /= divisor;
                }
            }

            var returnArray = new T[matrix.Width * matrix.Height];
            for (var row = 0; row < matrix.Height; row++)
            {
                value.GetRowSpan(row)[matrix.Width..].CopyTo(returnArray.AsSpan(matrix.Width * row, matrix.Width));
            }

            return new(returnArray, matrix.Height, matrix.Width);
        }
    }
}