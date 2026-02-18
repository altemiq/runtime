// -----------------------------------------------------------------------
// <copyright file="Span2DExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

#pragma warning disable CA1708, RCS1263, S2325, SA1101

/// <summary>
/// The <see cref="Span2D{T}"/> extensions.
/// </summary>
public static class Span2DExtensions
{
    extension(ref Span2D<float> input)
    {
#pragma warning disable InconsistentNaming
        /// <summary>
        /// Creates a new <see cref="System.Numerics.Matrix4x4" /> from this <see cref="Span2D{Double}"/>.
        /// </summary>
        /// <returns>The new <see cref="System.Numerics.Matrix4x4"/>.</returns>
        public System.Numerics.Matrix4x4 ToMatrix4x4()
        {
            if (input is not { Height: 4, Width: 4 })
            {
                throw new NotSupportedException();
            }

            if (input.TryGetSpan(out var span))
            {
                return ToMatrix4x4Core(span);
            }

            Span<float> temp = stackalloc float[16];
            input.CopyTo(temp);
            return ToMatrix4x4Core(temp);

            static System.Numerics.Matrix4x4 ToMatrix4x4Core(ReadOnlySpan<float> span)
            {
                return new(
                    span[0],
                    span[1],
                    span[2],
                    span[3],
                    span[4],
                    span[5],
                    span[6],
                    span[7],
                    span[8],
                    span[9],
                    span[10],
                    span[11],
                    span[12],
                    span[13],
                    span[14],
                    span[15]);
            }
        }

        /// <summary>
        /// Creates a new <see cref="System.Numerics.Matrix3x2" /> from this <see cref="Span2D{Double}"/>.
        /// </summary>
        /// <returns>The new <see cref="System.Numerics.Matrix3x2"/>.</returns>
        public System.Numerics.Matrix3x2 ToMatrix3x2()
        {
            if (input is not { Height: 3, Width: 2 })
            {
                throw new NotSupportedException();
            }

            if (input.TryGetSpan(out var span))
            {
                return ToMatrix3x2Core(span);
            }

            Span<float> temp = stackalloc float[6];
            input.CopyTo(temp);
            return ToMatrix3x2Core(temp);

            static System.Numerics.Matrix3x2 ToMatrix3x2Core(ReadOnlySpan<float> span)
            {
                return new(
                    span[0],
                    span[1],
                    span[2],
                    span[3],
                    span[4],
                    span[5]);
            }
        }

#pragma warning restore InconsistentNaming
        /// <summary>
        /// Creates a new <see cref="Span2D{Double}" /> from the <see cref="System.Numerics.Matrix4x4"/>.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The new <see cref="Span2D{Double}"/>.</returns>
        public static Span2D<float> FromMatrix(System.Numerics.Matrix4x4 matrix)
        {
            var items = new float[16];

            items[0] = matrix.M11;
            items[1] = matrix.M12;
            items[2] = matrix.M13;
            items[3] = matrix.M14;

            items[4] = matrix.M21;
            items[5] = matrix.M22;
            items[6] = matrix.M23;
            items[7] = matrix.M24;

            items[8] = matrix.M31;
            items[9] = matrix.M32;
            items[10] = matrix.M33;
            items[11] = matrix.M34;

            items[12] = matrix.M41;
            items[13] = matrix.M42;
            items[14] = matrix.M43;
            items[15] = matrix.M44;

            return new(items, 4, 4);
        }

        /// <summary>
        /// Creates a new <see cref="Span2D{Double}" /> from the <see cref="System.Numerics.Matrix3x2"/>.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The new <see cref="Span2D{Double}"/>.</returns>
        public static Span2D<float> FromMatrix(System.Numerics.Matrix3x2 matrix)
        {
            var items = new float[6];

            items[0] = matrix.M11;
            items[1] = matrix.M12;

            items[3] = matrix.M21;
            items[4] = matrix.M22;

            items[5] = matrix.M31;
            items[6] = matrix.M32;

            return new(items, 3, 2);
        }
    }

    extension(ref Span2D<double> input)
    {
#pragma warning disable InconsistentNaming
        /// <summary>
        /// Creates a new <see cref="Numerics.Matrix4x4D" /> from this <see cref="Span2D{Double}"/>.
        /// </summary>
        /// <returns>The new <see cref="Numerics.Matrix4x4D"/>.</returns>
        public Numerics.Matrix4x4D ToMatrix4x4()
        {
            if (input is not { Height: 4, Width: 4 })
            {
                throw new NotSupportedException();
            }

            if (input.TryGetSpan(out var span))
            {
                return ToMatrix4x4Core(span);
            }

            Span<double> temp = stackalloc double[16];
            input.CopyTo(temp);
            return ToMatrix4x4Core(temp);

            static Numerics.Matrix4x4D ToMatrix4x4Core(ReadOnlySpan<double> span)
            {
                return new(
                    span[0],
                    span[1],
                    span[2],
                    span[3],
                    span[4],
                    span[5],
                    span[6],
                    span[7],
                    span[8],
                    span[9],
                    span[10],
                    span[11],
                    span[12],
                    span[13],
                    span[14],
                    span[15]);
            }
        }

        /// <summary>
        /// Creates a new <see cref="Numerics.Matrix3x2D" /> from this <see cref="Span2D{Double}"/>.
        /// </summary>
        /// <returns>The new <see cref="Numerics.Matrix3x2D"/>.</returns>
        public Numerics.Matrix3x2D ToMatrix3x2()
        {
            if (input is not { Height: 3, Width: 2 })
            {
                throw new NotSupportedException();
            }

            if (input.TryGetSpan(out var span))
            {
                return ToMatrix3x2Core(span);
            }

            Span<double> temp = stackalloc double[6];
            input.CopyTo(temp);
            return ToMatrix3x2Core(temp);

            static Numerics.Matrix3x2D ToMatrix3x2Core(ReadOnlySpan<double> span)
            {
                return new(
                    span[0],
                    span[1],
                    span[2],
                    span[3],
                    span[4],
                    span[5]);
            }
        }
#pragma warning restore InconsistentNaming

        /// <summary>
        /// Creates a new <see cref="Span2D{Double}" /> from the <see cref="Numerics.Matrix4x4D"/>.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The new <see cref="Span2D{Double}"/>.</returns>
        public static Span2D<double> FromMatrix(Numerics.Matrix4x4D matrix)
        {
            var items = new double[16];

            matrix.X.CopyTo(items.AsSpan(0, 4));
            matrix.Y.CopyTo(items.AsSpan(4, 4));
            matrix.Z.CopyTo(items.AsSpan(8, 4));
            matrix.W.CopyTo(items.AsSpan(12, 4));

            return new(items, 4, 4);
        }

        /// <summary>
        /// Creates a new <see cref="Span2D{Double}" /> from the <see cref="Numerics.Matrix3x2D"/>.
        /// </summary>
        /// <param name="matrix">The input matrix.</param>
        /// <returns>The new <see cref="Span2D{Double}"/>.</returns>
        public static Span2D<double> FromMatrix(Numerics.Matrix3x2D matrix)
        {
            var items = new double[6];

            matrix.X.CopyTo(items.AsSpan(0, 2));
            matrix.Y.CopyTo(items.AsSpan(2, 2));
            matrix.Z.CopyTo(items.AsSpan(4, 2));

            return new(items, 3, 2);
        }
    }

    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ref Span2D<T> matrix)
#if NET8_0_OR_GREATER
        where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IAdditionOperators<T, T, T>
#else
        where T : struct, System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IAdditionOperators<T, T, T>
#endif
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
            using var workingOwner = CommunityToolkit.HighPerformance.Buffers.SpanOwner<T>.Allocate(matrix.Height * matrix.Width);
            matrix.CopyTo(workingOwner.Span);
            var working = ReadOnlySpan2D<T>.DangerousCreate(workingOwner.Span[0], matrix.Height, matrix.Width, 0);

            // if this is not square then we can't do this in place
            if (matrix.Height != matrix.Width)
            {
                matrix = new(new T[matrix.Height, other.Width]);
            }

            if (matrix.TryGetSpan(out var matrixSpan))
            {
                ReadOnlySpan2D<T>.Multiply(working, other, matrixSpan);
                return;
            }

            for (var column = 0; column < other.Width; column++)
            {
                for (var row = 0; row < working.Height; row++)
                {
                    var rowEnumerator = working.GetRow(row).GetEnumerator();
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
                span.Scale(scale);
            }
            else
            {
                for (var row = 0; row < matrix.Height; row++)
                {
                    matrix.GetRowSpan(row).Scale(scale);
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
#if NET8_0_OR_GREATER
        where T : System.Numerics.IAdditionOperators<T, T, T>
#else
        where T : struct, System.Numerics.IAdditionOperators<T, T, T>
#endif
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
                    matrixSpan1.Add(matrixSpan2);
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
                    matrix.GetRowSpan(row).Add(other.GetRowSpan(row));
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
#if NET8_0_OR_GREATER
        where T : System.Numerics.ISubtractionOperators<T, T, T>
#else
        where T : struct, System.Numerics.ISubtractionOperators<T, T, T>
#endif
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
                    matrixSpan1.Subtract(matrixSpan2);
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
                    matrix.GetRowSpan(row).Subtract(other.GetRowSpan(row));
                }
            }
        }
    }
}