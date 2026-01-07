// -----------------------------------------------------------------------
// <copyright file="GaussianInverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

/// <summary>
/// The <see href="https://en.wikipedia.org/wiki/Gaussian_elimination"/> <see cref="IMatrixInverter"/>.
/// </summary>
public class GaussianInverter : IMatrixInverter
{
    /// <inheritdoc />
    public static IMatrixInverter Instance { get; } = new GaussianInverter();

    /// <inheritdoc />
    public ReadOnlySpan2D<T> Invert<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
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