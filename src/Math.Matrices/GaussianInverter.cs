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
#if NET8_0_OR_GREATER
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
#else
        where T : struct, System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
#endif
    {
        var degree = matrix.Width * 2;

        // Get the matrix
        using var workingOwner = CommunityToolkit.HighPerformance.Buffers.SpanOwner<T>.Allocate(matrix.Height * degree);
        var working = Span2D<T>.DangerousCreate(ref workingOwner.Span[0], matrix.Height, degree, 0);

        for (var i = 0; i < matrix.Height; i++)
        {
            matrix.GetRowSpan(i).CopyTo(working.GetRowSpan(i));
        }

        // Augment Identity matrix onto matrix M to get [M|I]
        for (var i = matrix.Width; i < degree; i++)
        {
            working[i - matrix.Width, i] = T.One;
        }

        // Reduce so we get the leading diagonal
        for (var pivotRow = 0; pivotRow < working.Height; pivotRow++)
        {
            var pivot = working[pivotRow, pivotRow];

            for (var row = 0; row < working.Height; row++)
            {
                if (pivotRow == row)
                {
                    continue;
                }

                var temp = working[row, pivotRow] / pivot;

                if (T.IsNaN(temp))
                {
                    throw new InvalidOperationException();
                }

                // Invert the value
                temp = -temp;

                for (var column = 0; column < working.Width; column++)
                {
                    working[row, column] += working[pivotRow, column] * temp;
                }
            }
        }

        // Divide through to get the identity element
        // Note: the identity element may have very small values (close to zero) because of the way floating points are stored.
        for (var row = 0; row < matrix.Height; row++)
        {
            var divisor = working[row, row];

            for (var column = 0; column < degree; column++)
            {
                working[row, column] /= divisor;
            }
        }

        var returnArray = new T[matrix.Width * matrix.Height];
        for (var row = 0; row < matrix.Height; row++)
        {
            working.GetRowSpan(row)[matrix.Width..].CopyTo(returnArray.AsSpan(matrix.Width * row, matrix.Width));
        }

        return new(returnArray, matrix.Height, matrix.Width);
    }
}