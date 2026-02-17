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
#if NET8_0_OR_GREATER
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
#else
        where T : struct, System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
#endif
        => InvertCayleyHamilton(matrix, out _);

    private static ReadOnlySpan2D<T> InvertCayleyHamilton<T>(ReadOnlySpan2D<T> matrix, out T determinant)
#if NET8_0_OR_GREATER
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
#else
        where T : struct, System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
#endif
    {
        using var coefficients = CommunityToolkit.HighPerformance.Buffers.SpanOwner<T>.Allocate(matrix.Height + 1);
        FaddeevLeVerrierCoefficients(matrix, coefficients.Span);

        var diagonal = MatrixDiagonal(matrix.Height, coefficients.Span[matrix.Height - 1]);
        var k = matrix.Height - 2;
        var valuePowerArray = System.Buffers.ArrayPool<T>.Shared.Rent(matrix.Height * matrix.Width);
        matrix.CopyTo(valuePowerArray);
        var valuePower = new Span2D<T>(valuePowerArray, matrix.Height, matrix.Width);
        while (k >= 0)
        {
            diagonal += (ReadOnlySpan2D<T>)valuePower * coefficients.Span[k];
            valuePower *= matrix;
            k--;
        }

        System.Buffers.ArrayPool<T>.Shared.Return(valuePowerArray);

        determinant = coefficients.Span[matrix.Height];

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

    private static void FaddeevLeVerrierCoefficients<T>(ReadOnlySpan2D<T> a, Span<T> coefficients)
        where T : System.Numerics.INumberBase<T>
    {
        var rows = a.Height;
        coefficients[0] = T.One;

        var length = a.Height * a.Width;
        using var owner = CommunityToolkit.HighPerformance.Buffers.SpanOwner<T>.Allocate(a.Height * a.Width);
        var source = owner.Span;
        a.CopyTo(source);
        var ak = Span2D<T>.DangerousCreate(ref source[0], a.Height, a.Width, 0);
        var destination = CommunityToolkit.HighPerformance.Buffers.SpanOwner<T>.Allocate(length);
        for (var k = 1; k <= rows; k++)
        {
            var coefficient = -T.One * DiagonalSum(ak) / T.CreateChecked(k);
            coefficients[k] = coefficient;
            for (var i = 0; i < rows; i++)
            {
                ak[i, i] += coefficient;
            }

            ReadOnlySpan2D<T>.Multiply(a, ak, destination.Span);
            destination.Span.CopyTo(source);
        }

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