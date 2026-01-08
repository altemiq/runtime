// -----------------------------------------------------------------------
// <copyright file="Vector4D.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Numerics;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using Altemiq.Runtime.Intrinsics;

/// <summary>Represents a vector with four double-precision floating-point values.</summary>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct Vector4D : IEquatable<Vector4D>, IFormattable
{
    /// <summary>The X component of the vector.</summary>
    public double X;

    /// <summary>The Y component of the vector.</summary>
    public double Y;

    /// <summary>The Z component of the vector.</summary>
    public double Z;

    /// <summary>The W component of the vector.</summary>
    public double W;

    /// <summary>Specifies the alignment of the vector as used by the <see cref="LoadAligned(double*)"/> and <see cref="VectorD.StoreAligned(Vector4D, double*)"/> APIs.</summary>
    internal const int Alignment = 32;

    /// <summary>The element count.</summary>
    internal const int ElementCount = 4;

    /// <summary>Creates a new <see cref="Vector4D"/> object whose four elements have the same value.</summary>
    /// <param name="value">The value to assign to all four elements.</param>
    public Vector4D(double value) => this = Create(value);

    /// <summary>Creates a new <see cref="Vector4D" /> object from the specified <see cref="Vector2D" /> object and a Z and a W component.</summary>
    /// <param name="value">The vector to use for the X and Y components.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">The W component.</param>
    public Vector4D(Vector2D value, double z, double w) => this = Create(value, z, w);

    /// <summary>Constructs a new <see cref="Vector4D" /> object from the specified <see cref="Vector3D" /> object and a W component.</summary>
    /// <param name="value">The vector to use for the X, Y, and Z components.</param>
    /// <param name="w">The W component.</param>
    public Vector4D(Vector3D value, double w) => this = Create(value, w);

    /// <summary>Creates a vector whose elements have the specified values.</summary>
    /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
    /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
    /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
    /// <param name="w">The value to assign to the <see cref="W" /> field.</param>
    public Vector4D(double x, double y, double z, double w) => this = Create(x, y, z, w);

    /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{Single}" />. The span must contain at least 4 elements.</summary>
    /// <param name="values">The span of elements to assign to the vector.</param>
    public Vector4D(ReadOnlySpan<double> values) => this = Create(values);

    /// <summary>Gets a vector where all bits are set to <c>1</c>.</summary>
    /// <value>A vector where all bits are set to <c>1</c>.</value>
    public static Vector4D AllBitsSet => Vector256<double>.AllBitsSet.AsVector4D();

    /// <summary>Gets a vector whose elements are equal to <see cref="double.E" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.E" /> (that is, it returns the vector <c>Create(double.E)</c>).</value>
    public static Vector4D E => Create(double.E);

    /// <summary>Gets a vector whose elements are equal to <see cref="double.Epsilon" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.Epsilon" /> (that is, it returns the vector <c>Create(double.Epsilon)</c>).</value>
    public static Vector4D Epsilon => Create(double.Epsilon);

    /// <summary>Gets a vector whose elements are equal to <see cref="double.NaN" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.NaN" /> (that is, it returns the vector <c>Create(double.NaN)</c>).</value>
    public static Vector4D NaN => Create(double.NaN);

    /// <summary>Gets a vector whose elements are equal to <see cref="double.NegativeInfinity" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.NegativeInfinity" /> (that is, it returns the vector <c>Create(double.NegativeInfinity)</c>).</value>
    public static Vector4D NegativeInfinity => Create(double.NegativeInfinity);

    /// <summary>Gets a vector whose elements are equal to <see cref="double.NegativeZero" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.NegativeZero" /> (that is, it returns the vector <c>Create(double.NegativeZero)</c>).</value>
    public static Vector4D NegativeZero => Create(double.NegativeZero);

    /// <summary>Gets a vector whose elements are equal to one.</summary>
    /// <value>A vector whose elements are equal to one (that is, it returns the vector <c>Create(1)</c>).</value>
    public static Vector4D One => Create(1);

    /// <summary>Gets a vector whose elements are equal to <see cref="double.Pi" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.Pi" /> (that is, it returns the vector <c>Create(double.Pi)</c>).</value>
    public static Vector4D Pi => Create(double.Pi);

    /// <summary>Gets a vector whose elements are equal to <see cref="double.PositiveInfinity" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.PositiveInfinity" /> (that is, it returns the vector <c>Create(double.PositiveInfinity)</c>).</value>
    public static Vector4D PositiveInfinity => Create(double.PositiveInfinity);

    /// <summary>Gets a vector whose elements are equal to <see cref="double.Tau" />.</summary>
    /// <value>A vector whose elements are equal to <see cref="double.Tau" /> (that is, it returns the vector <c>Create(double.Tau)</c>).</value>
    public static Vector4D Tau => Create(double.Tau);

    /// <summary>Gets the vector (1,0,0,0).</summary>
    /// <value>The vector <c>(1,0,0,0)</c>.</value>
    public static Vector4D UnitX => CreateScalar(1.0);

    /// <summary>Gets the vector (0,1,0,0).</summary>
    /// <value>The vector <c>(0,1,0,0)</c>.</value>
    public static Vector4D UnitY => Create(0.0, 1.0, 0.0, 0.0);

    /// <summary>Gets the vector (0,0,1,0).</summary>
    /// <value>The vector <c>(0,0,1,0)</c>.</value>
    public static Vector4D UnitZ => Create(0.0, 0.0, 1.0, 0.0);

    /// <summary>Gets the vector (0,0,0,1).</summary>
    /// <value>The vector <c>(0,0,0,1)</c>.</value>
    public static Vector4D UnitW => Create(0.0, 0.0, 0.0, 1.0);

    /// <summary>Gets a vector whose elements are equal to zero.</summary>
    /// <value>A vector whose elements are equal to zero (that is, it returns the vector <c>Create(0)</c>).</value>
    public static Vector4D Zero => default;

    /// <summary>Gets or sets the element at the specified index.</summary>
    /// <param name="index">The index of the element to get or set.</param>
    /// <returns>The element at <paramref name="index" />.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> was less than zero or greater than the number of elements.</exception>
    public double this[int index]
    {
        readonly get => this.GetElement(index); set => this = this.WithElement(index, value);
    }

    /// <summary>Adds two vectors together.</summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The summed vector.</returns>
    /// <remarks>The <see cref="op_Addition" /> method defines the addition operation for <see cref="Vector4D" /> objects.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator +(Vector4D left, Vector4D right) => (left.AsVector256() + right.AsVector256()).AsVector4D();

    /// <summary>Divides the first vector by the second.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The vector that results from dividing <paramref name="left" /> by <paramref name="right" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator /(Vector4D left, Vector4D right) => (left.AsVector256() / right.AsVector256()).AsVector4D();

    /// <summary>Divides the specified vector by a specified scalar value.</summary>
    /// <param name="value1">The vector.</param>
    /// <param name="value2">The scalar value.</param>
    /// <returns>The result of the division.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator /(Vector4D value1, double value2) => (value1.AsVector256() / value2).AsVector4D();

    /// <summary>Returns a value that indicates whether each pair of elements in two specified vectors is equal.</summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are equal; otherwise, <see langword="false" />.</returns>
    /// <remarks>Two <see cref="Vector4D" /> objects are equal if each element in <paramref name="left" /> is equal to the corresponding element in <paramref name="right" />.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Vector4D left, Vector4D right) => left.AsVector256() == right.AsVector256();

    /// <summary>Returns a value that indicates whether two specified vectors are not equal.</summary>
    /// <param name="left">The first vector to compare.</param>
    /// <param name="right">The second vector to compare.</param>
    /// <returns><see langword="true" /> if <paramref name="left" /> and <paramref name="right" /> are not equal; otherwise, <see langword="false" />.</returns>
    public static bool operator !=(Vector4D left, Vector4D right) => !(left == right);

    /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The element-wise product vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator *(Vector4D left, Vector4D right) => (left.AsVector256() * right.AsVector256()).AsVector4D();

    /// <summary>Multiplies the specified vector by the specified scalar value.</summary>
    /// <param name="left">The vector.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator *(Vector4D left, double right) => (left.AsVector256() * right).AsVector4D();

    /// <summary>Multiplies the scalar value by the specified vector.</summary>
    /// <param name="left">The vector.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector4D operator *(double left, Vector4D right) => right * left;

    /// <summary>Subtracts the second vector from the first.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The vector that results from subtracting <paramref name="right" /> from <paramref name="left" />.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator -(Vector4D left, Vector4D right) => (left.AsVector256() - right.AsVector256()).AsVector4D();

    /// <summary>Negates the specified vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator -(Vector4D value) => (-value.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256{T}.op_BitwiseAnd(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator &(Vector4D left, Vector4D right) => (left.AsVector256() & right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256{T}.op_BitwiseOr(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator |(Vector4D left, Vector4D right) => (left.AsVector256() | right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256{T}.op_ExclusiveOr(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator ^(Vector4D left, Vector4D right) => (left.AsVector256() ^ right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256{T}.op_LeftShift(Vector256{T}, int)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator <<(Vector4D value, int shiftAmount) => (value.AsVector256() << shiftAmount).AsVector4D();

    /// <inheritdoc cref="Vector256{T}.op_OnesComplement(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator ~(Vector4D value) => (~value.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256{T}.op_RightShift(Vector256{T}, int)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator >>(Vector4D value, int shiftAmount) => (value.AsVector256() >> shiftAmount).AsVector4D();

    /// <inheritdoc cref="Vector256{T}.op_UnaryPlus(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator +(Vector4D value) => value;

    /// <inheritdoc cref="Vector256{T}.op_UnsignedRightShift(Vector256{T}, int)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D operator >>>(Vector4D value, int shiftAmount) => (value.AsVector256() >>> shiftAmount).AsVector4D();

    /// <summary>Returns a vector whose elements are the absolute values of each of the specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The absolute value vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Abs(Vector4D value) => Vector256.Abs(value.AsVector256()).AsVector4D();

    /// <summary>Adds two vectors together.</summary>
    /// <param name="left">The first vector to add.</param>
    /// <param name="right">The second vector to add.</param>
    /// <returns>The summed vector.</returns>
    public static Vector4D Add(Vector4D left, Vector4D right) => left + right;

    /// <inheritdoc cref="Vector256.All{T}(Vector256{T}, T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool All(Vector4D vector, double value) => Vector256.All(vector.AsVector256(), value);

    /// <inheritdoc cref="Vector256.AllWhereAllBitsSet{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AllWhereAllBitsSet(Vector4D vector) => Vector256.AllWhereAllBitsSet(vector.AsVector256());

    /// <inheritdoc cref="Vector256.AndNot{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D AndNot(Vector4D left, Vector4D right) => Vector256.AndNot(left.AsVector256(), right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Any{T}(Vector256{T}, T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Any(Vector4D vector, double value) => Vector256.Any(vector.AsVector256(), value);

    /// <inheritdoc cref="Vector256.AnyWhereAllBitsSet{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AnyWhereAllBitsSet(Vector4D vector) => Vector256.AnyWhereAllBitsSet(vector.AsVector256());

    /// <inheritdoc cref="Vector256.BitwiseAnd{T}(Vector256{T}, Vector256{T})" />
    public static Vector4D BitwiseAnd(Vector4D left, Vector4D right) => left & right;

    /// <inheritdoc cref="Vector256.BitwiseOr{T}(Vector256{T}, Vector256{T})" />
    public static Vector4D BitwiseOr(Vector4D left, Vector4D right) => left | right;

    /// <inheritdoc cref="Vector256.Clamp{T}(Vector256{T}, Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Clamp(Vector4D value1, Vector4D min, Vector4D max) => Vector256.Clamp(value1.AsVector256(), min.AsVector256(), max.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.ClampNative{T}(Vector256{T}, Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D ClampNative(Vector4D value1, Vector4D min, Vector4D max) => Vector256.ClampNative(value1.AsVector256(), min.AsVector256(), max.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.ConditionalSelect{T}(Vector256{T}, Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D ConditionalSelect(Vector4D condition, Vector4D left, Vector4D right) => Vector256.ConditionalSelect(condition.AsVector256(), left.AsVector256(), right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.CopySign{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D CopySign(Vector4D value, Vector4D sign) => Vector256.CopySign(value.AsVector256(), sign.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Cos(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Cos(Vector4D vector) => Vector256.Cos(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Count{T}(Vector256{T}, T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Count(Vector4D vector, double value) => Vector256.Count(vector.AsVector256(), value);

    /// <inheritdoc cref="Vector256.CountWhereAllBitsSet{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int CountWhereAllBitsSet(Vector4D vector) => Vector256.CountWhereAllBitsSet(vector.AsVector256());

    /// <summary>Creates a new <see cref="Vector4D" /> object whose four elements have the same value.</summary>
    /// <param name="value">The value to assign to all four elements.</param>
    /// <returns>A new <see cref="Vector4D" /> whose four elements have the same value.</returns>
    public static Vector4D Create(double value) => Vector256.Create(value).AsVector4D();

    /// <summary>Creates a new <see cref="Vector4D" /> object from the specified <see cref="Vector2D" /> object and a Z and a W component.</summary>
    /// <param name="vector">The vector to use for the X and Y components.</param>
    /// <param name="z">The Z component.</param>
    /// <param name="w">The W component.</param>
    /// <returns>A new <see cref="Vector4D" /> from the specified <see cref="Vector2D" /> object and a Z and a W component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Create(Vector2D vector, double z, double w) => vector.AsVector256Unsafe()
            .WithElement(2, z)
            .WithElement(3, w)
            .AsVector4D();

    /// <summary>Constructs a new <see cref="Vector4D" /> object from the specified <see cref="Vector3D" /> object and a W component.</summary>
    /// <param name="vector">The vector to use for the X, Y, and Z components.</param>
    /// <param name="w">The W component.</param>
    /// <returns>A new <see cref="Vector4D" /> from the specified <see cref="Vector3D" /> object and a W component.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Create(Vector3D vector, double w) => vector.AsVector256Unsafe()
            .WithElement(3, w)
            .AsVector4D();

    /// <summary>Creates a vector whose elements have the specified values.</summary>
    /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
    /// <param name="y">The value to assign to the <see cref="Y" /> field.</param>
    /// <param name="z">The value to assign to the <see cref="Z" /> field.</param>
    /// <param name="w">The value to assign to the <see cref="W" /> field.</param>
    /// <returns>A new <see cref="Vector4D" /> whose elements have the specified values.</returns>
    public static Vector4D Create(double x, double y, double z, double w) => Vector256.Create(x, y, z, w).AsVector4D();

    /// <summary>Constructs a vector from the given <see cref="ReadOnlySpan{Single}" />. The span must contain at least 4 elements.</summary>
    /// <param name="values">The span of elements to assign to the vector.</param>
    /// <returns>A new <see cref="Vector4D" /> whose elements have the specified values.</returns>
    public static Vector4D Create(ReadOnlySpan<double> values) => Vector256.Create(values).AsVector4D();

    /// <summary>Creates a vector with <see cref="X" /> initialized to the specified value and the remaining elements initialized to zero.</summary>
    /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
    /// <returns>A <see cref="Vector4D" /> with <see cref="X" /> initialized <paramref name="x" /> and the remaining elements initialized to zero.</returns>
    public static Vector4D CreateScalar(double x) => Vector256.CreateScalar(x).AsVector4D();

    /// <summary>Creates a vector with <see cref="X" /> initialized to the specified value and the remaining elements left uninitialized.</summary>
    /// <param name="x">The value to assign to the <see cref="X" /> field.</param>
    /// <returns>A <see cref="Vector4D" /> with <see cref="X" /> initialized <paramref name="x" /> and the remaining elements left uninitialized.</returns>
    public static Vector4D CreateScalarUnsafe(double x) => Vector256.CreateScalarUnsafe(x).AsVector4D();

    /// <summary>
    /// Computes the cross product of two vectors. For homogeneous coordinates,
    /// the product of the weights is the new weight for the resulting product.
    /// </summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The cross product.</returns>
    /// <remarks>
    /// The proposed Cross function for <see cref="Vector4D"/> is nearly the same as that for
    /// <see cref="Vector3D.Cross"/> with the addition of the fourth value which is
    /// the product of the original two w's. This can be derived by symbolically performing
    /// the cross product for <see cref="Vector3D"/> with values [x_1/w_1, y_1/w_1, z_1/w_1]
    /// and [x_2/w_2, y_2/w_2, z_2/w_2].
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Cross(Vector4D vector1, Vector4D vector2)
    {
        // This implementation is based on the DirectX Math Library XMVector3DCross method https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
        var v1 = vector1.AsVector256();
        var v2 = vector2.AsVector256();

        var m2 = Vector256.Shuffle(v1, Vector256.Create(2, 0, 1, 3)) *
                 Vector256.Shuffle(v2, Vector256.Create(1, 2, 0, 3));

        return Vector256.MultiplyAddEstimate(
            Vector256.Shuffle(v1, Vector256.Create(1, 2, 0, 3)),
            Vector256.Shuffle(v2, Vector256.Create(2, 0, 1, 3)),
            -m2.WithElement(3, 0)).AsVector4D();
    }

    /// <inheritdoc cref="Vector256.DegreesToRadians(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D DegreesToRadians(Vector4D degrees) => Vector256.DegreesToRadians(degrees.AsVector256()).AsVector4D();

    /// <summary>Computes the Euclidean distance between the two given points.</summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance.</returns>
    public static double Distance(Vector4D value1, Vector4D value2) => double.Sqrt(DistanceSquared(value1, value2));

    /// <summary>Returns the Euclidean distance squared between two specified points.</summary>
    /// <param name="value1">The first point.</param>
    /// <param name="value2">The second point.</param>
    /// <returns>The distance squared.</returns>
    public static double DistanceSquared(Vector4D value1, Vector4D value2) => (value1 - value2).LengthSquared();

    /// <summary>Divides the first vector by the second.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The vector resulting from the division.</returns>
    public static Vector4D Divide(Vector4D left, Vector4D right) => left / right;

    /// <summary>Divides the specified vector by a specified scalar value.</summary>
    /// <param name="left">The vector.</param>
    /// <param name="divisor">The scalar value.</param>
    /// <returns>The vector that results from the division.</returns>
    public static Vector4D Divide(Vector4D left, double divisor) => left / divisor;

    /// <summary>Returns the dot product of two vectors.</summary>
    /// <param name="vector1">The first vector.</param>
    /// <param name="vector2">The second vector.</param>
    /// <returns>The dot product.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Dot(Vector4D vector1, Vector4D vector2) => Vector256.Dot(vector1.AsVector256(), vector2.AsVector256());

    /// <inheritdoc cref="Vector256.Exp(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Exp(Vector4D vector) => Vector256.Exp(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Equals{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Equals(Vector4D left, Vector4D right) => Vector256.Equals(left.AsVector256(), right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.EqualsAll{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAll(Vector4D left, Vector4D right) => Vector256.EqualsAll(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.EqualsAny{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool EqualsAny(Vector4D left, Vector4D right) => Vector256.EqualsAny(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.MultiplyAddEstimate(Vector256{double}, Vector256{double}, Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D FusedMultiplyAdd(Vector4D left, Vector4D right, Vector4D addend) => Vector256.FusedMultiplyAdd(left.AsVector256(), right.AsVector256(), addend.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.GreaterThan{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D GreaterThan(Vector4D left, Vector4D right) => Vector256.GreaterThan(left.AsVector256(), right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.GreaterThanAll{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAll(Vector4D left, Vector4D right) => Vector256.GreaterThanAll(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.GreaterThanAny{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanAny(Vector4D left, Vector4D right) => Vector256.GreaterThanAny(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.GreaterThanOrEqual{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D GreaterThanOrEqual(Vector4D left, Vector4D right) => Vector256.GreaterThanOrEqual(left.AsVector256(), right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.GreaterThanOrEqualAll{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAll(Vector4D left, Vector4D right) => Vector256.GreaterThanOrEqualAll(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.GreaterThanOrEqualAny{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool GreaterThanOrEqualAny(Vector4D left, Vector4D right) => Vector256.GreaterThanOrEqualAny(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.Hypot(Vector256{double}, Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Hypot(Vector4D x, Vector4D y) => Vector256.Hypot(x.AsVector256(), y.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IndexOf{T}(Vector256{T}, T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf(Vector4D vector, double value) => Vector256.IndexOf(vector.AsVector256(), value);

    /// <inheritdoc cref="Vector256.IndexOfWhereAllBitsSet{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOfWhereAllBitsSet(Vector4D vector) => Vector256.IndexOfWhereAllBitsSet(vector.AsVector256());

    /// <inheritdoc cref="Vector256.IsEvenInteger{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsEvenInteger(Vector4D vector) => Vector256.IsEvenInteger(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsFinite{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsFinite(Vector4D vector) => Vector256.IsFinite(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsInfinity{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsInfinity(Vector4D vector) => Vector256.IsInfinity(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsInteger{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsInteger(Vector4D vector) => Vector256.IsInteger(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsNaN{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsNaN(Vector4D vector) => Vector256.IsNaN(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsNegative{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsNegative(Vector4D vector) => Vector256.IsNegative(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsNegativeInfinity{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsNegativeInfinity(Vector4D vector) => Vector256.IsNegativeInfinity(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsNormal{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsNormal(Vector4D vector) => Vector256.IsNormal(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsOddInteger{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsOddInteger(Vector4D vector) => Vector256.IsOddInteger(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsPositive{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsPositive(Vector4D vector) => Vector256.IsPositive(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsPositiveInfinity{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsPositiveInfinity(Vector4D vector) => Vector256.IsPositiveInfinity(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsSubnormal{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsSubnormal(Vector4D vector) => Vector256.IsSubnormal(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.IsZero{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D IsZero(Vector4D vector) => Vector256.IsZero(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.LastIndexOf{T}(Vector256{T}, T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOf(Vector4D vector, double value) => Vector256.LastIndexOf(vector.AsVector256(), value);

    /// <inheritdoc cref="Vector256.LastIndexOfWhereAllBitsSet{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int LastIndexOfWhereAllBitsSet(Vector4D vector) => Vector256.LastIndexOfWhereAllBitsSet(vector.AsVector256());

    /// <inheritdoc cref="Lerp(Vector4D, Vector4D, Vector4D)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Lerp(Vector4D value1, Vector4D value2, double amount) => Lerp(value1, value2, Create(amount));

    /// <inheritdoc cref="Vector256.Lerp(Vector256{double}, Vector256{double}, Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Lerp(Vector4D value1, Vector4D value2, Vector4D amount) => Vector256.Lerp(value1.AsVector256(), value2.AsVector256(), amount.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.LessThan{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D LessThan(Vector4D left, Vector4D right) => Vector256.LessThan(left.AsVector256(), right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.LessThanAll{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAll(Vector4D left, Vector4D right) => Vector256.LessThanAll(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.LessThanAny{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanAny(Vector4D left, Vector4D right) => Vector256.LessThanAny(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.LessThanOrEqual{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D LessThanOrEqual(Vector4D left, Vector4D right) => Vector256.LessThanOrEqual(left.AsVector256(), right.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.LessThanOrEqualAll{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAll(Vector4D left, Vector4D right) => Vector256.LessThanOrEqualAll(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.LessThanOrEqualAny{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool LessThanOrEqualAny(Vector4D left, Vector4D right) => Vector256.LessThanOrEqualAny(left.AsVector256(), right.AsVector256());

    /// <inheritdoc cref="Vector256.Load{T}(T*)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vector4D Load(double* source) => Vector256.Load(source).AsVector4D();

    /// <inheritdoc cref="Vector256.LoadAligned{T}(T*)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vector4D LoadAligned(double* source) => Vector256.LoadAligned(source).AsVector4D();

    /// <inheritdoc cref="Vector256.LoadAlignedNonTemporal{T}(T*)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static unsafe Vector4D LoadAlignedNonTemporal(double* source) => Vector256.LoadAlignedNonTemporal(source).AsVector4D();

    /// <inheritdoc cref="Vector256.LoadUnsafe{T}(ref readonly T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D LoadUnsafe(ref readonly double source) => Vector256.LoadUnsafe(in source).AsVector4D();

    /// <inheritdoc cref="Vector256.LoadUnsafe{T}(ref readonly T, nuint)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D LoadUnsafe(ref readonly double source, nuint elementOffset) => Vector256.LoadUnsafe(in source, elementOffset).AsVector4D();

    /// <inheritdoc cref="Vector256.Log(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Log(Vector4D vector) => Vector256.Log(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Log2(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Log2(Vector4D vector) => Vector256.Log2(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Max{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Max(Vector4D value1, Vector4D value2) => Vector256.Max(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MaxMagnitude{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MaxMagnitude(Vector4D value1, Vector4D value2) => Vector256.MaxMagnitude(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MaxMagnitudeNumber{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MaxMagnitudeNumber(Vector4D value1, Vector4D value2) => Vector256.MaxMagnitudeNumber(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MaxNative{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MaxNative(Vector4D value1, Vector4D value2) => Vector256.MaxNative(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MaxNumber{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MaxNumber(Vector4D value1, Vector4D value2) => Vector256.MaxNumber(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Min{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Min(Vector4D value1, Vector4D value2) => Vector256.Min(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MinMagnitude{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MinMagnitude(Vector4D value1, Vector4D value2) => Vector256.MinMagnitude(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MinMagnitudeNumber{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MinMagnitudeNumber(Vector4D value1, Vector4D value2) => Vector256.MinMagnitudeNumber(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MinNative{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MinNative(Vector4D value1, Vector4D value2) => Vector256.MinNative(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.MinNumber{T}(Vector256{T}, Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MinNumber(Vector4D value1, Vector4D value2) => Vector256.MinNumber(value1.AsVector256(), value2.AsVector256()).AsVector4D();

    /// <summary>Returns a new vector whose values are the product of each pair of elements in two specified vectors.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The element-wise product vector.</returns>
    public static Vector4D Multiply(Vector4D left, Vector4D right) => left * right;

    /// <summary>Multiplies a vector by a specified scalar.</summary>
    /// <param name="left">The vector to multiply.</param>
    /// <param name="right">The scalar value.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector4D Multiply(Vector4D left, double right) => left * right;

    /// <summary>Multiplies a scalar value by a specified vector.</summary>
    /// <param name="left">The scaled value.</param>
    /// <param name="right">The vector.</param>
    /// <returns>The scaled vector.</returns>
    public static Vector4D Multiply(double left, Vector4D right) => left * right;

    /// <inheritdoc cref="Vector256.MultiplyAddEstimate(Vector256{double}, Vector256{double}, Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D MultiplyAddEstimate(Vector4D left, Vector4D right, Vector4D addend) => Vector256.MultiplyAddEstimate(left.AsVector256(), right.AsVector256(), addend.AsVector256()).AsVector4D();

    /// <summary>Negates a specified vector.</summary>
    /// <param name="value">The vector to negate.</param>
    /// <returns>The negated vector.</returns>
    public static Vector4D Negate(Vector4D value) => -value;

    /// <inheritdoc cref="Vector256.None{T}(Vector256{T}, T)" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool None(Vector4D vector, double value) => Vector256.None(vector.AsVector256(), value);

    /// <inheritdoc cref="Vector256.NoneWhereAllBitsSet{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool NoneWhereAllBitsSet(Vector4D vector) => Vector256.NoneWhereAllBitsSet(vector.AsVector256());

    /// <summary>Returns a vector with the same direction as the specified vector, but with a length of one.</summary>
    /// <param name="vector">The vector to normalize.</param>
    /// <returns>The normalized vector.</returns>
    public static Vector4D Normalize(Vector4D vector) => vector / vector.Length();

    /// <inheritdoc cref="Vector256.OnesComplement{T}(Vector256{T})" />
    public static Vector4D OnesComplement(Vector4D value) => ~value;

    /// <inheritdoc cref="Vector256.RadiansToDegrees(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D RadiansToDegrees(Vector4D radians) => Vector256.RadiansToDegrees(radians.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Round(Vector256{double})" />
    public static Vector4D Round(Vector4D vector) => Vector256.Round(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Round(Vector256{double}, MidpointRounding)" />
    public static Vector4D Round(Vector4D vector, MidpointRounding mode) => Vector256.Round(vector.AsVector256(), mode).AsVector4D();

    /// <summary>Creates a new vector by selecting values from an input vector using a set of indices.</summary>
    /// <param name="vector">The input vector from which values are selected.</param>
    /// <param name="xIndex">The index used to select a value from <paramref name="vector" /> to be used as the value of <see cref="X" /> in the result.</param>
    /// <param name="yIndex">The index used to select a value from <paramref name="vector" /> to be used as the value of <see cref="Y" /> in the result.</param>
    /// <param name="zIndex">The index used to select a value from <paramref name="vector" /> to be used as the value of <see cref="Z" /> in the result.</param>
    /// <param name="wIndex">The index used to select a value from <paramref name="vector" /> to be used as the value of <see cref="W" /> in the result.</param>
    /// <returns>A new vector containing the values from <paramref name="vector" /> selected by the given indices.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Shuffle(Vector4D vector, byte xIndex, byte yIndex, byte zIndex, byte wIndex) => Vector256.Shuffle(vector.AsVector256(), Vector256.Create(xIndex, yIndex, zIndex, wIndex)).AsVector4D();

    /// <inheritdoc cref="Vector256.Sin(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Sin(Vector4D vector) => Vector256.Sin(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.SinCos(Vector256{double})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static (Vector4D Sin, Vector4D Cos) SinCos(Vector4D vector)
    {
        var (sin, cos) = Vector256.SinCos(vector.AsVector256());
        return (sin.AsVector4D(), cos.AsVector4D());
    }

    /// <summary>Returns a vector whose elements are the square root of each of a specified vector's elements.</summary>
    /// <param name="value">A vector.</param>
    /// <returns>The square root vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D SquareRoot(Vector4D value) => Vector256.Sqrt(value.AsVector256()).AsVector4D();

    /// <summary>Subtracts the second vector from the first.</summary>
    /// <param name="left">The first vector.</param>
    /// <param name="right">The second vector.</param>
    /// <returns>The difference vector.</returns>
    public static Vector4D Subtract(Vector4D left, Vector4D right) => left - right;

    /// <inheritdoc cref="Vector256.Sum{T}(Vector256{T})" />
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Sum(Vector4D value) => Vector256.Sum(value.AsVector256());

    // /// <summary>Transforms a two-dimensional vector by a specified 4x4 matrix.</summary>
    // /// <param name="position">The vector to transform.</param>
    // /// <param name="matrix">The transformation matrix.</param>
    // /// <returns>The transformed vector.</returns>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static Vector4D Transform(Vector2D position, Matrix4x4D matrix)
    // {
    //     // This implementation is based on the DirectX Math Library XMVector2DTransform method
    //     // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
    //
    //     Vector4D result = matrix.X * position.X;
    //     result = MultiplyAddEstimate(matrix.Y, Create(position.Y), result);
    //     return result + matrix.W;
    // }
    /// <summary>Transforms a two-dimensional vector by a specified 4x4 matrix.</summary>
    /// <param name="position">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Transform(Vector2D position, Matrix4x4D matrix)
    {
        // This implementation is based on the DirectX Math Library XMVector2DTransform method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
        var result = matrix.X * position.X;
        result = MultiplyAddEstimate(matrix.Y, Create(position.Y), result);
        return result + matrix.W;
    }

    /// <summary>Transforms a two-dimensional vector by the specified QuaternionD rotation value.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Transform(Vector2D value, QuaternionD rotation) => Transform(Create(value, 0.0, 1.0), rotation);

    // /// <summary>Transforms a three-dimensional vector by a specified 4x4 matrix.</summary>
    // /// <param name="position">The vector to transform.</param>
    // /// <param name="matrix">The transformation matrix.</param>
    // /// <returns>The transformed vector.</returns>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static Vector4D Transform(Vector3D position, Matrix4x4D matrix)
    // {
    //     // This implementation is based on the DirectX Math Library XMVector3DTransform method
    //     // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
    //
    //     Vector4D result = matrix.X * position.X;
    //     result = MultiplyAddEstimate(matrix.Y, Create(position.Y), result);
    //     result = MultiplyAddEstimate(matrix.Z, Create(position.Z), result);
    //     return result + matrix.W;
    // }
    /// <summary>Transforms a three-dimensional vector by a specified 4x4 matrix.</summary>
    /// <param name="position">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Transform(Vector3D position, Matrix4x4D matrix)
    {
        // This implementation is based on the DirectX Math Library XMVector3DTransform method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
        var result = matrix.X * position.X;
        result = MultiplyAddEstimate(matrix.Y, Create(position.Y), result);
        result = MultiplyAddEstimate(matrix.Z, Create(position.Z), result);
        return result + matrix.W;
    }

    /// <summary>Transforms a three-dimensional vector by the specified QuaternionD rotation value.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Transform(Vector3D value, QuaternionD rotation) => Transform(Create(value, 1.0), rotation);

    // /// <summary>Transforms a four-dimensional vector by a specified 4x4 matrix.</summary>
    // /// <param name="vector">The vector to transform.</param>
    // /// <param name="matrix">The transformation matrix.</param>
    // /// <returns>The transformed vector.</returns>
    // [MethodImpl(MethodImplOptions.AggressiveInlining)]
    // public static Vector4D Transform(Vector4D vector, Matrix4x4D matrix)
    // {
    //     // This implementation is based on the DirectX Math Library XMVector4Transform method
    //     // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
    //
    //     Vector4D result = matrix.X * vector.X;
    //     result = MultiplyAddEstimate(matrix.Y, Create(vector.Y), result);
    //     result = MultiplyAddEstimate(matrix.Z, Create(vector.Z), result);
    //     result = MultiplyAddEstimate(matrix.W, Create(vector.W), result);
    //     return result;
    // }
    /// <summary>Transforms a four-dimensional vector by a specified 4x4 matrix.</summary>
    /// <param name="vector">The vector to transform.</param>
    /// <param name="matrix">The transformation matrix.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Transform(Vector4D vector, Matrix4x4D matrix)
    {
        // This implementation is based on the DirectX Math Library XMVector4Transform method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
        var result = matrix.X * vector.X;
        result = MultiplyAddEstimate(matrix.Y, Create(vector.Y), result);
        result = MultiplyAddEstimate(matrix.Z, Create(vector.Z), result);
        result = MultiplyAddEstimate(matrix.W, Create(vector.W), result);
        return result;
    }

    /// <summary>Transforms a four-dimensional vector by the specified QuaternionD rotation value.</summary>
    /// <param name="value">The vector to rotate.</param>
    /// <param name="rotation">The rotation to apply.</param>
    /// <returns>The transformed vector.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector4D Transform(Vector4D value, QuaternionD rotation)
    {
        // This implementation is based on the DirectX Math Library XMVector3DRotate method
        // https://github.com/microsoft/DirectXMath/blob/master/Inc/DirectXMathVector.inl
        var conjuagate = QuaternionD.Conjugate(rotation);
        var temp = QuaternionD.Concatenate(conjuagate, value.AsQuaternionD());
        return QuaternionD.Concatenate(temp, rotation).AsVector4D();
    }

    /// <inheritdoc cref="Vector256.Truncate(Vector256{double})" />
    public static Vector4D Truncate(Vector4D vector) => Vector256.Truncate(vector.AsVector256()).AsVector4D();

    /// <inheritdoc cref="Vector256.Xor{T}(Vector256{T}, Vector256{T})" />
    public static Vector4D Xor(Vector4D left, Vector4D right) => left ^ right;

    /// <summary>Copies the elements of the vector to a specified array.</summary>
    /// <param name="array">The destination array.</param>
    /// <remarks><paramref name="array" /> must have at least four elements. The method copies the vector's elements starting at index 0.</remarks>
    /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
    /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
    public readonly void CopyTo(double[] array) => this.AsVector256().CopyTo(array);

    /// <summary>Copies the elements of the vector to a specified array starting at a specified index position.</summary>
    /// <param name="array">The destination array.</param>
    /// <param name="index">The index at which to copy the first element of the vector.</param>
    /// <remarks><paramref name="array" /> must have a sufficient number of elements to accommodate the four vector elements. In other words, elements <paramref name="index" /> through <paramref name="index" /> + 3 must already exist in <paramref name="array" />.</remarks>
    /// <exception cref="NullReferenceException"><paramref name="array" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">The number of elements in the current instance is greater than in the array.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index" /> is less than zero.
    /// -or-
    /// <paramref name="index" /> is greater than or equal to the array length.</exception>
    /// <exception cref="RankException"><paramref name="array" /> is multidimensional.</exception>
    public readonly void CopyTo(double[] array, int index) => this.AsVector256().CopyTo(array, index);

    /// <summary>Copies the vector to the given <see cref="Span{T}" />. The length of the destination span must be at least 4.</summary>
    /// <param name="destination">The destination span which the values are copied into.</param>
    /// <exception cref="ArgumentException">If number of elements in source vector is greater than those available in destination span.</exception>
    public readonly void CopyTo(Span<double> destination) => this.AsVector256().CopyTo(destination);

    /// <summary>Attempts to copy the vector to the given <see cref="Span{Single}" />. The length of the destination span must be at least 4.</summary>
    /// <param name="destination">The destination span which the values are copied into.</param>
    /// <returns><see langword="true" /> if the source vector was successfully copied to <paramref name="destination" />. <see langword="false" /> if <paramref name="destination" /> is not large enough to hold the source vector.</returns>
    public readonly bool TryCopyTo(Span<double> destination) => this.AsVector256().TryCopyTo(destination);

    /// <summary>Returns a value that indicates whether this instance and another vector are equal.</summary>
    /// <param name="other">The other vector.</param>
    /// <returns><see langword="true" /> if the two vectors are equal; otherwise, <see langword="false" />.</returns>
    /// <remarks>Two vectors are equal if their <see cref="X" />, <see cref="Y" />, <see cref="Z" />, and <see cref="W" /> elements are equal.</remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Vector4D other) => this.AsVector256().Equals(other.AsVector256());

    /// <summary>Returns a value that indicates whether this instance and a specified object are equal.</summary>
    /// <param name="obj">The object to compare with the current instance.</param>
    /// <returns><see langword="true" /> if the current instance and <paramref name="obj" /> are equal; otherwise, <see langword="false" />. If <paramref name="obj" /> is <see langword="null" />, the method returns <see langword="false" />.</returns>
    /// <remarks>The current instance and <paramref name="obj" /> are equal if <paramref name="obj" /> is a <see cref="Vector4D" /> object and their corresponding elements are equal.</remarks>
    public override readonly bool Equals([NotNullWhen(true)] object? obj) => (obj is Vector4D other) && this.Equals(other);

    /// <summary>Returns the hash code for this instance.</summary>
    /// <returns>The hash code.</returns>
    public override readonly int GetHashCode() => HashCode.Combine(this.X, this.Y, this.Z, this.W);

    /// <summary>Returns the length of this vector object.</summary>
    /// <returns>The vector's length.</returns>
    /// <altmember cref="LengthSquared" />
    public readonly double Length() => double.Sqrt(this.LengthSquared());

    /// <summary>Returns the length of the vector squared.</summary>
    /// <returns>The vector's length squared.</returns>
    /// <remarks>This operation offers better performance than a call to the <see cref="Length" /> method.</remarks>
    /// <altmember cref="Length" />
    public readonly double LengthSquared() => Dot(this, this);

    /// <summary>Returns the string representation of the current instance using default formatting.</summary>
    /// <returns>The string representation of the current instance.</returns>
    /// <remarks>This method returns a string in which each element of the vector is formatted using the "G" (general) format string and the formatting conventions of the current thread culture. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="System.Globalization.NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
    public override readonly string ToString() => this.ToString("G", System.Globalization.CultureInfo.CurrentCulture);

    /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements.</summary>
    /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
    /// <returns>The string representation of the current instance.</returns>
    /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and the current culture's formatting conventions. The "&lt;" and "&gt;" characters are used to begin and end the string, and the current culture's <see cref="System.Globalization.NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format) => this.ToString(format, System.Globalization.CultureInfo.CurrentCulture);

    /// <summary>Returns the string representation of the current instance using the specified format string to format individual elements and the specified format provider to define culture-specific formatting.</summary>
    /// <param name="format">A standard or custom numeric format string that defines the format of individual elements.</param>
    /// <param name="formatProvider">A format provider that supplies culture-specific formatting information.</param>
    /// <returns>The string representation of the current instance.</returns>
    /// <remarks>This method returns a string in which each element of the vector is formatted using <paramref name="format" /> and <paramref name="formatProvider" />. The "&lt;" and "&gt;" characters are used to begin and end the string, and the format provider's <see cref="System.Globalization.NumberFormatInfo.NumberGroupSeparator" /> property followed by a space is used to separate each element.</remarks>
    public readonly string ToString([StringSyntax(StringSyntaxAttribute.NumericFormat)] string? format, IFormatProvider? formatProvider)
    {
        var separator = System.Globalization.NumberFormatInfo.GetInstance(formatProvider).NumberGroupSeparator;

        return $"<{this.X.ToString(format, formatProvider)}{separator} {this.Y.ToString(format, formatProvider)}{separator} {this.Z.ToString(format, formatProvider)}{separator} {this.W.ToString(format, formatProvider)}>";
    }
}