// -----------------------------------------------------------------------
// <copyright file="CayleyHamiltonInverter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

/// <summary>
/// The <see href="https://en.wikipedia.org/wiki/Cayley%E2%80%93Hamilton_theorem"/> <see cref="IMatrixInverter"/>.
/// </summary>
public class CayleyHamiltonInverter : IMatrixInverter
{
    /// <inheritdoc />
    public static IMatrixInverter Instance { get; } = new CayleyHamiltonInverter();

    /// <inheritdoc />
    public ReadOnlySpan2D<T> Invert<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T> => InvertCayleyHamilton(matrix, out _);

    private static ReadOnlySpan2D<T> InvertCayleyHamilton<T>(ReadOnlySpan2D<T> matrix, out T determinant)
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
    {
        var coefficients = FaddeevLeVerrierCoefficients(matrix);

        var diagonal = MatrixDiagonal(matrix.Height, coefficients[matrix.Height - 1]);
        var k = matrix.Height - 2;
        var valuePower = new Span2D<T>(matrix.ToArray());
        while (k >= 0)
        {
            diagonal += (ReadOnlySpan2D<T>)valuePower * coefficients[k];
            valuePower *= matrix;
            k--;
        }

        determinant = coefficients[matrix.Height];
        diagonal *= -T.One / determinant;
        if (matrix.Height % 2 is not 0)
        {
            determinant *= -T.One;
        }

        return diagonal;

        static ReadOnlySpan2D<T> MatrixDiagonal(int n, T value)
        {
            var result = new T[n * n];
            var i = 0;
            while (i < result.Length)
            {
                result[i] = value;
                i += n + 1;
            }

            return new(result, n, n);
        }
    }

    private static T[] FaddeevLeVerrierCoefficients<T>(ReadOnlySpan2D<T> a)
        where T : System.Numerics.INumberBase<T>
    {
        var rows = a.Height;
        var coefficients = new T[rows + 1];
        coefficients[0] = T.One;

        var ak = new Span2D<T>(a.ToArray());
        _ = ak.TryGetSpan(out var source);
        Span<T> destination = new T[ak.Length];
        for (var k = 1; k <= rows; ++k)
        {
            var coefficient = -T.One * DiagonalSum(ak) / T.CreateChecked(k);
            coefficients[k] = coefficient;
            for (var i = 0; i < rows; ++i)
            {
                ak[i, i] += coefficient;
            }

            ReadOnlySpan2D<T>.Multiply(a, ak, destination);
            destination.CopyTo(source);
        }

        return coefficients;

        static T DiagonalSum(ReadOnlySpan2D<T> a)
        {
            var result = T.Zero;
            for (var i = 0; i < a.Height; ++i)
            {
                result += a[i, i];
            }

            return result;
        }
    }
}