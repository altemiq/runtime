// -----------------------------------------------------------------------
// <copyright file="CholeskyInverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

/// <summary>
/// The <see href="https://en.wikipedia.org/wiki/Cholesky_decomposition"/> <see cref="IMatrixInverter"/>.
/// </summary>
public class CholeskyInverter : IMatrixInverter
{
    /// <inheritdoc />
    public static IMatrixInverter Instance { get; } = new CholeskyInverter();

    /// <inheritdoc />
    public ReadOnlySpan2D<T> Invert<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
    {
        var value = new Span2D<T>(matrix.ToArray());

        // Make all lower triangle values 0
        RemoveLowerTriangle(value);

        // Invert the matrix
        var factors = new T[value.Height];

        // Check to make sure we can
        for (var row = 0; row < value.Height; row++)
        {
            if (T.IsNegative(value[row, row]))
            {
                // Cannot do an inverse when a diagonal is 0
                throw new InvalidOperationException();
            }

            factors[row] = T.Sqrt(value[row, row]);
        }

        for (var row = 0; row < value.Height; row++)
        {
            for (var column = 0; column < value.Width; column++)
            {
                ref T current = ref value[row, column];
                current /= factors[row] * factors[column];
            }
        }

        // Cholesky Decomposition
        try
        {
            CholeskyDecomposition(value);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            throw new InvalidOperationException(message: null, ex);
        }

        // Form the w matrix
        for (var row = value.Height - 1; row >= 0; row--)
        {
            for (var column = value.Width - 1; column >= 0; column--)
            {
                if (row == column)
                {
                    ref T current = ref value[row, column];
                    current = T.One / current;
                }
                else
                {
                    var temp = T.Zero;

                    for (var k = row + 1; k <= column; k++)
                    {
                        temp += value[row, k] * value[k, column];
                    }

                    value[row, column] = -temp / value[row, row];
                }
            }
        }

        // Form the wwt (inverse)
        for (var row = 0; row < value.Height; row++)
        {
            for (var j = row; j < value.Height; j++)
            {
                var temp = T.Zero;

                for (var k = row; k < value.Width; k++)
                {
                    temp += value[row, k] * value[j, k];
                }

                value[row, j] = temp / (factors[row] * factors[j]);
            }
        }

        // fill the lower triangle
        FillLowerTriangle(value);

        return value;

        static void FillLowerTriangle(Span2D<T> value)
        {
            for (var row = 0; row < value.Height; row++)
            {
                for (var j = row - 1; j >= 0; j--)
                {
                    value[row, j] = value[j, row];
                }
            }
        }

        static void RemoveLowerTriangle(Span2D<T> value)
        {
            for (var row = 0; row < value.Height; row++)
            {
                for (var column = 0; column < value.Width; column++)
                {
                    if (row > column)
                    {
                        value[row, column] = T.Zero;
                    }
                }
            }
        }

        static void CholeskyDecomposition(Span2D<T> value)
        {
            for (var row = 0; row < value.Height; row++)
            {
                var temp = T.Zero;
                for (var k = 0; k < row; k++)
                {
                    var current = value[k, row];
                    temp += current * current;
                }

                temp = value[row, row] - temp;
                if (T.IsNegative(temp))
                {
                    throw new ArgumentOutOfRangeException(paramName: nameof(value));
                }

                value[row, row] = T.Sqrt(temp);

                for (var j = row + 1; j < value.Width; j++)
                {
                    temp = T.Zero;

                    for (var k = 0; k < row; k++)
                    {
                        temp += value[k, row] * value[k, j];
                    }

                    value[row, j] = (value[row, j] - temp) / value[row, row];
                }
            }
        }
    }
}