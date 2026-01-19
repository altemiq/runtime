// -----------------------------------------------------------------------
// <copyright file="ReadOnlySpan2DExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Math;

#pragma warning disable CA1708, SA1101, RCS1263

/// <summary>
/// The <see cref="ReadOnlySpan2D{T}"/> extensions.
/// </summary>
public static class ReadOnlySpan2DExtensions
{
    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ReadOnlySpan2D<T> matrix)
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ReadOnlySpan2D{T}"/> is square.
        /// </summary>
        public bool IsSquare => matrix.Length is not 0 && matrix.Height == matrix.Width;

        /// <summary>
        /// Create a new instance of <see cref="ReadOnlySpan2D{T}"/> from the transpose of the current instance.
        /// </summary>
        /// <returns>This transposes the matrix, creating a new matrix with the dimensions (column, row), with each element(i, j) = element(j, i).</returns>
        public ReadOnlySpan2D<T> Transpose()
        {
            var returnValue = new T[matrix.Height * matrix.Width];

            if (matrix.TryGetSpan(out var span))
            {
                for (var row = 0; row < matrix.Height; row++)
                {
                    for (var column = 0; column < matrix.Width; column++)
                    {
                        returnValue[(column * matrix.Height) + row] = span[(row * matrix.Width) + column];
                    }
                }
            }
            else
            {
                for (var row = 0; row < matrix.Height; row++)
                {
                    for (var column = 0; column < matrix.Width; column++)
                    {
                        returnValue[(column * matrix.Height) + row] = matrix[row, column];
                    }
                }
            }

            return new(returnValue, matrix.Width, matrix.Height);
        }
    }

    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.IAdditionOperators<T, T, T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the addition of the two specified matrices.
        /// </summary>
        /// <param name="other">The instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the addition of the two specified matrices.</returns>
        /// <seealso cref="op_Addition"/>
        public ReadOnlySpan2D<T> Add(ReadOnlySpan2D<T> other) => matrix + other;

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the addition of the two specified matrices.
        /// </summary>
        /// <param name="matrix1">The first instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <param name="matrix2">The second instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the addition of the two specified matrices.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Add{T}(ReadOnlySpan2D{T}, ReadOnlySpan2D{T})"/>
        public static ReadOnlySpan2D<T> operator +(ReadOnlySpan2D<T> matrix1, ReadOnlySpan2D<T> matrix2)
        {
            if (matrix1.Height != matrix2.Height || matrix1.Width != matrix2.Width)
            {
                throw new InvalidOperationException();
            }

            var returnValue = new T[matrix1.Height * matrix1.Width];
            if (matrix1.TryGetSpan(out var matrixSpan1))
            {
                if (matrix2.TryGetSpan(out var matrixSpan2))
                {
                    for (var i = 0; i < returnValue.Length; i++)
                    {
                        returnValue[i] = matrixSpan1[i] + matrixSpan2[i];
                    }
                }
                else
                {
                    matrixSpan1.CopyTo(returnValue);
                    for (var row = 0; row < matrix1.Height; row++)
                    {
                        var matrixRowSpan2 = matrix2.GetRowSpan(row);
                        var rowIndex = row * matrix1.Width;
                        for (var column = 0; column < matrix1.Width; column++)
                        {
                            returnValue[rowIndex + column] += matrixRowSpan2[column];
                        }
                    }
                }
            }
            else
            {
                for (var row = 0; row < matrix1.Height; row++)
                {
                    var matrixRowSpan1 = matrix1.GetRowSpan(row);
                    var matrixRowSpan2 = matrix2.GetRowSpan(row);
                    var rowIndex = row * matrix1.Width;
                    for (var column = 0; column < matrix1.Width; column++)
                    {
                        returnValue[rowIndex + column] = matrixRowSpan1[column] + matrixRowSpan2[column];
                    }
                }
            }

            return new(returnValue, matrix1.Height, matrix1.Width);
        }
    }

    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.ISubtractionOperators<T, T, T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the subtraction of the two specified matrices.
        /// </summary>
        /// <param name="other">The instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the subtraction of the two specified matrices.</returns>
        /// <seealso cref="op_Subtraction"/>
        public ReadOnlySpan2D<T> Subtract(ReadOnlySpan2D<T> other) => matrix - other;

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the subtraction of the two specified matrices.
        /// </summary>
        /// <param name="matrix1">The first instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <param name="matrix2">The second instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the subtraction of the two specified matrices.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Subtract{T}(ReadOnlySpan2D{T}, ReadOnlySpan2D{T})"/>
        public static ReadOnlySpan2D<T> operator -(ReadOnlySpan2D<T> matrix1, ReadOnlySpan2D<T> matrix2)
        {
            if (matrix1.Height != matrix2.Height || matrix1.Width != matrix2.Width)
            {
                throw new InvalidOperationException();
            }

            var returnValue = new T[matrix1.Height * matrix1.Width];
            if (matrix1.TryGetSpan(out var matrixSpan1))
            {
                if (matrix2.TryGetSpan(out var matrixSpan2))
                {
                    for (var i = 0; i < returnValue.Length; i++)
                    {
                        returnValue[i] = matrixSpan1[i] - matrixSpan2[i];
                    }
                }
                else
                {
                    matrixSpan1.CopyTo(returnValue);
                    for (var row = 0; row < matrix1.Height; row++)
                    {
                        var matrixRowSpan2 = matrix2.GetRowSpan(row);
                        var rowIndex = row * matrix1.Width;
                        for (var column = 0; column < matrix1.Width; column++)
                        {
                            returnValue[rowIndex + column] -= matrixRowSpan2[column];
                        }
                    }
                }
            }
            else
            {
                for (var row = 0; row < matrix1.Height; row++)
                {
                    var matrixRowSpan1 = matrix1.GetRowSpan(row);
                    var matrixRowSpan2 = matrix2.GetRowSpan(row);
                    var rowIndex = row * matrix1.Width;
                    for (var column = 0; column < matrix1.Width; column++)
                    {
                        returnValue[rowIndex + column] = matrixRowSpan1[column] - matrixRowSpan2[column];
                    }
                }
            }

            return new(returnValue, matrix1.Height, matrix1.Width);
        }
    }

    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.IMultiplyOperators<T, T, T>, System.Numerics.IAdditionOperators<T, T, T>
    {
        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the multiplication of the two specified matrices.
        /// </summary>
        /// <param name="other">The instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the multiplication of the two specified matrices.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.op_Multiply{T}(ReadOnlySpan2D{T},ReadOnlySpan2D{T})"/>
        public ReadOnlySpan2D<T> Multiply(ReadOnlySpan2D<T> other) => matrix * other;

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.
        /// </summary>
        /// <param name="multiplier">A double value to scale the matrix by.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.op_Multiply{T}(ReadOnlySpan2D{T},T)"/>
        public ReadOnlySpan2D<T> Scale(T multiplier) => matrix * multiplier;

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the multiplication of the two specified matrices.
        /// </summary>
        /// <param name="matrix1">The first instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <param name="matrix2">The second instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the multiplication of the two specified matrices.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Multiply{T}(ReadOnlySpan2D{T}, ReadOnlySpan2D{T})"/>
        public static ReadOnlySpan2D<T> operator *(ReadOnlySpan2D<T> matrix1, ReadOnlySpan2D<T> matrix2)
        {
            if (matrix1.Height != matrix2.Width)
            {
                throw new InvalidOperationException();
            }

            var returnValue = new T[matrix1.Height * matrix2.Width];
            ReadOnlySpan2D<T>.Multiply(matrix1, matrix2, returnValue);
            return new(returnValue, matrix1.Height, matrix2.Width);
        }

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.
        /// </summary>
        /// <param name="scale">A value to scale the matrix by.</param>
        /// <param name="value">An instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Scale{T}(ReadOnlySpan2D{T},T)"/>
        public static ReadOnlySpan2D<T> operator *(T scale, ReadOnlySpan2D<T> value) => value * scale;

        /// <summary>
        /// Creates a new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.
        /// </summary>
        /// <param name="value">An instance of <see cref="ReadOnlySpan2D{T}"/>.</param>
        /// <param name="scale">A value to scale the matrix by.</param>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/> from the scaling of the specified matrix by the specified scale.</returns>
        /// <seealso cref="ReadOnlySpan2DExtensions.Scale{T}(ReadOnlySpan2D{T},T)"/>
        public static ReadOnlySpan2D<T> operator *(ReadOnlySpan2D<T> value, T scale)
        {
            if (value.IsEmpty)
            {
                return ReadOnlySpan2D<T>.Empty;
            }

            var returnValue = new T[value.Height * value.Width];
            if (value.TryGetSpan(out var span))
            {
                for (var i = 0; i < span.Length; i++)
                {
                    returnValue[i] = span[i] * scale;
                }
            }
            else
            {
                for (var row = 0; row < value.Height; row++)
                {
                    var matrixRowSpan1 = value.GetRowSpan(row);
                    var rowIndex = row * value.Width;
                    for (var column = 0; column < value.Width; column++)
                    {
                        returnValue[rowIndex + column] = matrixRowSpan1[column] * scale;
                    }
                }
            }

            return new(returnValue, value.Height, value.Width);
        }

        internal static void Multiply(ReadOnlySpan2D<T> matrix1, ReadOnlySpan2D<T> matrix2, Span<T> destination)
        {
            for (var column = 0; column < matrix2.Width; column++)
            {
                for (var row = 0; row < matrix1.Height; row++)
                {
                    var rowEnumerator = matrix1.GetRow(row).GetEnumerator();
                    var columnEnumerator = matrix2.GetColumn(column).GetEnumerator();

                    if (!rowEnumerator.MoveNext() || !columnEnumerator.MoveNext())
                    {
                        continue;
                    }

                    var total = rowEnumerator.Current * columnEnumerator.Current;
                    while (rowEnumerator.MoveNext() && columnEnumerator.MoveNext())
                    {
                        total += rowEnumerator.Current * columnEnumerator.Current;
                    }

                    destination[(row * matrix1.Height) + column] = total;
                }
            }
        }
    }

    /// <summary>
    /// The <see cref="ReadOnlySpan2D{T}"/> extension block.
    /// </summary>
    /// <typeparam name="T">The type of number in the matrix.</typeparam>
    /// <param name="matrix">The matrix to operate on.</param>
    extension<T>(ReadOnlySpan2D<T> matrix)
        where T : System.Numerics.INumberBase<T>, System.Numerics.IRootFunctions<T>
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ReadOnlySpan2D{T}"/> is the identity matrix.
        /// </summary>
        public bool IsIdentity
        {
            get
            {
                for (var row = 0; row < matrix.Height; row++)
                {
                    var rowSpan = matrix.GetRowSpan(row);
                    for (var column = 0; column < rowSpan.Length; column++)
                    {
                        var value = rowSpan[column];
                        if ((column == row && value != T.One) || (column != row && !T.IsZero(value)))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Creates a new <see cref="ReadOnlySpan2D{T}"/> from the inverse of the specified instance of <see cref="ReadOnlySpan2D{T}"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/>.</returns>
        public static ReadOnlySpan2D<T> operator ^(ReadOnlySpan2D<T> x, int y) => y is not -1 ? throw new NotSupportedException() : x.Invert();

        /// <summary>
        /// Creates a new <see cref="ReadOnlySpan2D{T}"/> from the inverse of the specified instance of <see cref="ReadOnlySpan2D{T}"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/>.</returns>
        public static ReadOnlySpan2D<T> operator ^(ReadOnlySpan2D<T> x, IMatrixInverter y) => y.Invert(x);

        /// <summary>
        /// Gets the determinant of the current <see cref="ReadOnlySpan2D{T}"/>.
        /// </summary>
        /// <returns>A double value representing the determinant of the matrix.</returns>
        public T GetDeterminant() =>
            matrix switch
            {
                { Height: var h, Width: var w } when h != w => throw new NotSupportedException(),
                { Height: 0 } => T.Zero,
                { Height: 1 } => matrix[0, 0],
                { Height: 2 } => (matrix[0, 0] * matrix[1, 1]) - (matrix[0, 1] * matrix[1, 0]),
                _ => ReadOnlySpan2D<T>.ComputeDeterminant(matrix),
            };

        /// <summary>
        /// Creates a new <see cref="ReadOnlySpan2D{T}"/> from the inverse of the current instance of <see cref="ReadOnlySpan2D{T}"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/>.</returns>
        public ReadOnlySpan2D<T> Pow(int y) => matrix ^ y;

        /// <summary>
        /// Creates a new <see cref="ReadOnlySpan2D{T}"/> from the inverse of the current instance of <see cref="ReadOnlySpan2D{T}"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/>.</returns>
        public ReadOnlySpan2D<T> Invert(IMatrixInverter inverter) => inverter.Invert(matrix);

        /// <summary>
        /// Creates a new <see cref="ReadOnlySpan2D{T}"/> from the inverse of the current instance of <see cref="ReadOnlySpan2D{T}"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/>.</returns>
        public ReadOnlySpan2D<T> Invert<TInverter>()
            where TInverter : IMatrixInverter => matrix.Invert(TInverter.Instance);

        /// <summary>
        /// Creates a new <see cref="ReadOnlySpan2D{T}"/> from the inverse of the current instance of <see cref="ReadOnlySpan2D{T}"/>.
        /// </summary>
        /// <returns>A new instance of <see cref="ReadOnlySpan2D{T}"/>.</returns>
        public ReadOnlySpan2D<T> Invert()
        {
            if (matrix.Height != matrix.Width)
            {
                throw new InvalidOperationException();
            }

            if (matrix.Height is 1)
            {
                return T.IsZero(matrix[0, 0])
                    ? throw new ArgumentOutOfRangeException(nameof(matrix))
                    : matrix.Slice(0, 0, 1, 1);
            }

            var determinant = ReadOnlySpan2D<T>.ComputeDeterminant(matrix);

            if (T.IsZero(determinant))
            {
                // Cannot invert matrix with Determinant = 0
                throw new InvalidOperationException();
            }

            if (matrix.Height is 2)
            {
                // Calculate the inverse of a 2x2 matrix directly
                var scaleValue = T.One / determinant;
                return new([matrix[1, 1] * scaleValue, matrix[0, 1] * scaleValue, matrix[1, 0] * scaleValue, matrix[0, 0] * scaleValue], 2, 2);
            }

            var value = new T[matrix.Width * matrix.Height];
            var count = matrix.Height;
            var subMatrixSize = count - 1;
            var subMatrix = new T[subMatrixSize, subMatrixSize];

            for (var i = 0; i < count; i++)
            {
                for (var j = 0; j < count; j++)
                {
                    FillSubMatrix(subMatrix, subMatrixSize, matrix, i, j);
                    determinant = ReadOnlySpan2D<T>.ComputeDeterminant(subMatrix, subMatrixSize);

                    // Temp is our C matrix
                    value[(i * count) + j] = determinant * T.CreateChecked(System.Math.Pow(-1, i + j));
                }
            }

            // Transpose C matrix
            var transpose = new ReadOnlySpan2D<T>(value, matrix.Height, matrix.Width).Transpose();

            // Return the matrix
            return transpose * (T.One / determinant);

            static void FillSubMatrix(T[,] subMatrix, int count, ReadOnlySpan2D<T> matrix, int row, int column)
            {
                // Copy out the sub-matrix from the original matrix
                var oldRow = -1;
                for (var i = 0; i < count; i++)
                {
                    if (i == row)
                    {
                        oldRow++;
                    }

                    oldRow++;
                    var oldColumn = -1;
                    for (var j = 0; j < count; j++)
                    {
                        if (j == column)
                        {
                            oldColumn += 2;
                        }
                        else
                        {
                            oldColumn++;
                        }

                        subMatrix[i, j] = matrix[oldRow, oldColumn];
                    }
                }
            }
        }

        private static T ComputeDeterminant(ReadOnlySpan2D<T> value)
        {
            using var owner = CommunityToolkit.HighPerformance.Buffers.SpanOwner<T>.Allocate(value.Height * value.Width);
            value.CopyTo(owner.Span);
            var working = Span2D<T>.DangerousCreate(ref owner.Span[0], value.Height, value.Width, 0);
            return ReadOnlySpan2D<T>.ComputeDeterminant(working, value.Height);
        }

        private static T ComputeDeterminant(Span2D<T> value, int size)
        {
            var zeroBasedSize = size - 1;
            var determinant = T.One;

            for (var k = 0; k < size; k++)
            {
                if (T.IsZero(value[k, k]))
                {
                    var j = k;
                    while ((j < zeroBasedSize) && T.IsZero(value[k, j]))
                    {
                        j++;
                    }

                    if (T.IsZero(value[k, j]))
                    {
                        return T.Zero;
                    }

                    for (var i = k; i < size; i++)
                    {
                        (value[i, j], value[i, k]) = (value[i, k], value[i, j]);
                    }

                    determinant = -determinant;
                }

                var arrayK = value[k, k];
                determinant *= arrayK;
                if (k >= size)
                {
                    continue;
                }

                for (var i = k + 1; i < size; i++)
                {
                    for (var j = k + 1; j < size; j++)
                    {
                        value[i, j] -= value[i, k] * (value[k, j] / arrayK);
                    }
                }
            }

            return determinant;
        }
    }
}