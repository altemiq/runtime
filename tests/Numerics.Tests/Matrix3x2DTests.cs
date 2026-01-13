namespace Altemiq.Numerics;

using System.Runtime.InteropServices;

[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
public sealed class Matrix3x2DTests
{
    private static Matrix3x2D GenerateIncrementalMatrixNumber(double value = 0.0d)
    {
        var a = new Matrix3x2D
        {
            M11 = value + 1.0d,
            M12 = value + 2.0d,
            M21 = value + 3.0d,
            M22 = value + 4.0d,
            M31 = value + 5.0d,
            M32 = value + 6.0d,
        };
        return a;
    }

    private static Matrix3x2D GenerateTestMatrix()
    {
        var m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0d));
        m.Translation = new(111.0d, 222.0d);
        return m;
    }

    [Test]
    [Arguments(0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d)]
    [Arguments(1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d)]
    [Arguments(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d, 3.1434343d, 1.1234123d)]
    [Arguments(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d, 1.0000001d, 0.0000001d)]
    public async Task Matrix3x2DIndexerGetTest(double m11, double m12, double m21, double m22, double m31, double m32)
    {
        var matrix = new Matrix3x2D(m11, m12, m21, m22, m31, m32);

        await Assert.That(matrix[0, 0]).IsEqualTo(m11);
        await Assert.That(matrix[0, 1]).IsEqualTo(m12);
        await Assert.That(matrix[1, 0]).IsEqualTo(m21);
        await Assert.That(matrix[1, 1]).IsEqualTo(m22);
        await Assert.That(matrix[2, 0]).IsEqualTo(m31);
        await Assert.That(matrix[2, 1]).IsEqualTo(m32);
    }

    [Test]
    [Arguments(0.0d, 1.0d, 0.0d, 1.0d, 0.0d, 1.0d)]
    [Arguments(1.0d, 0.0d, 1.0d, 0.0d, 1.0d, 0.0d)]
    [Arguments(3.1434343d, 1.1234123d, 0.1234123d, -0.1234123d, 3.1434343d, 1.1234123d)]
    [Arguments(1.0000001d, 0.0000001d, 2.0000001d, 0.0000002d, 1.0000001d, 0.0000001d)]
    public async Task Matrix3x2DIndexerSetTest(double m11, double m12, double m21, double m22, double m31, double m32)
    {
        var matrix = new Matrix3x2D(0.0d, 0.0d, 0.0d, 0.0d, 0.0d, 0.0d)
        {
            [0, 0] = m11,
            [0, 1] = m12,
            [1, 0] = m21,
            [1, 1] = m22,
            [2, 0] = m31,
            [2, 1] = m32,
        };

        await Assert.That(matrix[0, 0]).IsEqualTo(m11);
        await Assert.That(matrix[0, 1]).IsEqualTo(m12);
        await Assert.That(matrix[1, 0]).IsEqualTo(m21);
        await Assert.That(matrix[1, 1]).IsEqualTo(m22);
        await Assert.That(matrix[2, 0]).IsEqualTo(m31);
        await Assert.That(matrix[2, 1]).IsEqualTo(m32);
    }

    // A test for Identity
    [Test]
    public async Task Matrix3x2DIdentityTest()
    {
        var val = new Matrix3x2D();
        val.M11 = val.M22 = 1.0d;

        await Assert.That(Matrix3x2D.Identity).IsEqualTo(val);
    }

    // A test for Determinant
    [Test]
    public async Task Matrix3x2DDeterminantTest()
    {
        var target = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0d));

        var val = 1.0d;
        var det = target.GetDeterminant();

        await Assert.That(det).IsEqualTo(val);
    }

    // Determinant test |A| = 1 / |A'|
    [Test]
    public async Task Matrix3x2DDeterminantTest1()
    {
        var a = new Matrix3x2D
        {
            M11 = 5.0d,
            M12 = 2.0d,
            M21 = 12.0d,
            M22 = 6.8d,
            M31 = 6.5d,
            M32 = 1.0d,
        };
        await Assert.That(Matrix3x2D.Invert(a, out var i)).IsTrue();

        var detA = a.GetDeterminant();
        var detI = i.GetDeterminant();
        var t = 1.0d / detI;

        // only accurate to 3 precision
        await Assert.That(detA).IsEqualTo(t).Within(1e-3);

        // sanity check against 4x4 version
        await Assert.That(detA).IsEqualTo(new Matrix4x4D(a).GetDeterminant());
        await Assert.That(detI).IsEqualTo(new Matrix4x4D(i).GetDeterminant());
    }

    // A test for Invert (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DInvertTest()
    {
        var mtx = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0d));

        var expected = new Matrix3x2D
        {
            M11 = 0.8660254d,
            M12 = -0.5d,
            M21 = 0.5d,
            M22 = 0.8660254d,
            M31 = 0,
            M32 = 0,
        };

        await Assert.That(Matrix3x2D.Invert(mtx, out var actual)).IsTrue();
        await Assert.That(actual).IsEqualTo(expected).Within(1E-8);

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix3x2D.Identity);
    }

    // A test for Invert (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DInvertIdentityTest()
    {
        var mtx = Matrix3x2D.Identity;

        await Assert.That(Matrix3x2D.Invert(mtx, out var actual)).IsTrue();

        await Assert.That(actual).IsEqualTo(Matrix3x2D.Identity);
    }

    // A test for Invert (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DInvertTranslationTest()
    {
        var mtx = Matrix3x2D.CreateTranslation(23, 42);

        await Assert.That(Matrix3x2D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo( Matrix3x2D.Identity);
    }

    // A test for Invert (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DInvertRotationTest()
    {
        var mtx = Matrix3x2D.CreateRotation(2);

        await Assert.That(Matrix3x2D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix3x2D.Identity);
    }

    // A test for Invert (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DInvertScaleTest()
    {
        var mtx = Matrix3x2D.CreateScale(23, -42);

        await Assert.That(Matrix3x2D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix3x2D.Identity).Within(1E-15);
    }

    // A test for Invert (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DInvertAffineTest()
    {
        var mtx = Matrix3x2D.CreateRotation(2) *
                  Matrix3x2D.CreateScale(23, -42) *
                  Matrix3x2D.CreateTranslation(17, 53);

        await Assert.That(Matrix3x2D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix3x2D.Identity).Within(1E-15);
    }



    // A test for CreateRotation (double)
    [Test]
    public async Task Matrix3x2DCreateRotationTest()
    {
        var radians = MathHelper.ToRadians(50.0d);

        var expected = new Matrix3x2D
        {
            M11 = 0.642787635d,
            M12 = 0.766044438d,
            M21 = -0.766044438d,
            M22 = 0.642787635d,
        };

        var actual = Matrix3x2D.CreateRotation(radians);
        await Assert.That(actual).IsEqualTo(expected).Within(1e-7);
    }

    // A test for CreateRotation (double, Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateRotationCenterTest()
    {
        var radians = MathHelper.ToRadians(30.0d);
        var center = new Vector2D(23, 42);

        var rotateAroundZero = Matrix3x2D.CreateRotation(radians, Vector2D.Zero);
        var rotateAroundZeroExpected = Matrix3x2D.CreateRotation(radians);
        await Assert.That(rotateAroundZero).IsEqualTo(rotateAroundZeroExpected);

        var rotateAroundCenter = Matrix3x2D.CreateRotation(radians, center);
        var rotateAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateRotation(radians) * Matrix3x2D.CreateTranslation(center);
        await Assert.That(rotateAroundCenter).IsEqualTo(rotateAroundCenterExpected);
    }

    // A test for CreateRotation (double)
    [Test]
    public async Task Matrix3x2DCreateRotationRightAngleTest()
    {
        // 90 degree rotations must be exact!
        var actual = Matrix3x2D.CreateRotation(0);
        await Assert.That(actual).IsEqualTo(new(1, 0, 0, 1, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI / 2);
        await Assert.That(actual).IsEqualTo(new(0, 1, -1, 0, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI);
        await Assert.That(actual).IsEqualTo(new(-1, 0, 0, -1, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI * 3 / 2);
        await Assert.That(actual).IsEqualTo(new(0, -1, 1, 0, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI * 2);
        await Assert.That(actual).IsEqualTo(new(1, 0, 0, 1, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI * 5 / 2);
        await Assert.That(actual).IsEqualTo(new(0, 1, -1, 0, 0, 0));

        actual = Matrix3x2D.CreateRotation(-Math.PI / 2);
        await Assert.That(actual).IsEqualTo(new(0, -1, 1, 0, 0, 0));

        // But merely close-to-90 rotations should not be excessively clamped.
        var delta = MathHelper.ToRadians(0.01d);

        actual = Matrix3x2D.CreateRotation(Math.PI + delta);
        await Assert.That(actual).IsNotEqualTo(new(-1, 0, 0, -1, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI - delta);
        await Assert.That(actual).IsNotEqualTo(new(-1, 0, 0, -1, 0, 0));
    }

    // A test for CreateRotation (double, Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateRotationRightAngleCenterTest()
    {
        var center = new Vector2D(3, 7);

        // 90 degree rotations must be exact!
        var actual = Matrix3x2D.CreateRotation(0, center);
        await Assert.That(actual).IsEqualTo(new(1, 0, 0, 1, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI / 2, center);
        await Assert.That(actual).IsEqualTo(new(0, 1, -1, 0, 10, 4));

        actual = Matrix3x2D.CreateRotation(Math.PI, center);
        await Assert.That(actual).IsEqualTo(new(-1, 0, 0, -1, 6, 14));

        actual = Matrix3x2D.CreateRotation(Math.PI * 3 / 2, center);
        await Assert.That(actual).IsEqualTo(new(0, -1, 1, 0, -4, 10));

        actual = Matrix3x2D.CreateRotation(Math.PI * 2, center);
        await Assert.That(actual).IsEqualTo(new(1, 0, 0, 1, 0, 0));

        actual = Matrix3x2D.CreateRotation(Math.PI * 5 / 2, center);
        await Assert.That(actual).IsEqualTo(new(0, 1, -1, 0, 10, 4));

        actual = Matrix3x2D.CreateRotation(-Math.PI / 2, center);
        await Assert.That(actual).IsEqualTo(new(0, -1, 1, 0, -4, 10));

        // But merely close-to-90 rotations should not be excessively clamped.
        var delta = MathHelper.ToRadians(0.01d);

        actual = Matrix3x2D.CreateRotation(Math.PI + delta, center);
        await Assert.That(actual).IsNotEqualTo(new(-1, 0, 0, -1, 6, 14));

        actual = Matrix3x2D.CreateRotation(Math.PI - delta, center);
        await Assert.That(actual).IsNotEqualTo(new(-1, 0, 0, -1, 6, 14));
    }

    // Non-invertible matrix - determinant is zero - singular matrix
    [Test]
    public async Task Matrix3x2DInvertTest1()
    {
        var a = new Matrix3x2D
        {
            M11 = 0.0d,
            M12 = 2.0d,
            M21 = 0.0d,
            M22 = 4.0d,
            M31 = 5.0d,
            M32 = 6.0d,
        };

        var detA = a.GetDeterminant();
        await Assert.That(detA).IsEqualTo(0.0d);

        await Assert.That(Matrix3x2D.Invert(a, out var actual)).IsFalse();

        // all the elements in Actual is NaN
        await Assert.That(actual)
            .Member(static actual => actual.M11, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M11, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M12, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M21, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M22, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M31, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M32, static actual => actual.IsNaN());
    }

    // A test for Lerp (Matrix3x2D, Matrix3x2D, double)
    [Test]
    public async Task Matrix3x2DLerpTest()
    {
        var a = new Matrix3x2D
        {
            M11 = 11.0d,
            M12 = 12.0d,
            M21 = 21.0d,
            M22 = 22.0d,
            M31 = 31.0d,
            M32 = 32.0d,
        };

        var b = GenerateIncrementalMatrixNumber();

        var t = 0.5d;

        var expected = new Matrix3x2D
        {
            M11 = a.M11 + (b.M11 - a.M11) * t,
            M12 = a.M12 + (b.M12 - a.M12) * t,
            M21 = a.M21 + (b.M21 - a.M21) * t,
            M22 = a.M22 + (b.M22 - a.M22) * t,
            M31 = a.M31 + (b.M31 - a.M31) * t,
            M32 = a.M32 + (b.M32 - a.M32) * t,
        };

        var actual = Matrix3x2D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator - (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DUnaryNegationTest()
    {
        var a = GenerateIncrementalMatrixNumber();

        var expected = new Matrix3x2D
        {
            M11 = -1.0d,
            M12 = -2.0d,
            M21 = -3.0d,
            M22 = -4.0d,
            M31 = -5.0d,
            M32 = -6.0d,
        };

        var actual = -a;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator - (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DSubtractionTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-3.0d);
        var expected = new Matrix3x2D
        {
            M11 = a.M11 - b.M11,
            M12 = a.M12 - b.M12,
            M21 = a.M21 - b.M21,
            M22 = a.M22 - b.M22,
            M31 = a.M31 - b.M31,
            M32 = a.M32 - b.M32,
        };

        var actual = a - b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DMultiplyTest1()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-3.0d);

        var expected = new Matrix3x2D
        {
            M11 = a.M11 * b.M11 + a.M12 * b.M21,
            M12 = a.M11 * b.M12 + a.M12 * b.M22,
            M21 = a.M21 * b.M11 + a.M22 * b.M21,
            M22 = a.M21 * b.M12 + a.M22 * b.M22,
            M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31,
            M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32,
        };

        var actual = a * b;
        await Assert.That(actual).IsEqualTo(expected);

        // Sanity check by comparison with 4x4 multiply.
        a = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30D)) * Matrix3x2D.CreateTranslation(23, 42);
        b = Matrix3x2D.CreateScale(3, 7) * Matrix3x2D.CreateTranslation(666, -1);

        actual = a * b;

        var a44 = new Matrix4x4D(a);
        var b44 = new Matrix4x4D(b);
        var expected44 = a44 * b44;
        var actual44 = new Matrix4x4D(actual);

        await Assert.That(actual44).IsEqualTo(expected44);
    }

    // A test for operator * (Matrix3x2D, Matrix3x2D)
    // Multiply with identity matrix
    [Test]
    public async Task Matrix3x2DMultiplyTest4()
    {
        var a = new Matrix3x2D
        {
            M11 = 1.0d,
            M12 = 2.0d,
            M21 = 5.0d,
            M22 = -6.0d,
            M31 = 9.0d,
            M32 = 10.0d,
        };

        var b = Matrix3x2D.Identity;

        var expected = a;
        var actual = a * b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator + (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DAdditionTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-3.0d);

        var expected = new Matrix3x2D
        {
            M11 = a.M11 + b.M11,
            M12 = a.M12 + b.M12,
            M21 = a.M21 + b.M21,
            M22 = a.M22 + b.M22,
            M31 = a.M31 + b.M31,
            M32 = a.M32 + b.M32,
        };

        var actual = a + b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for ToString ()
    [Test]
    public async Task Matrix3x2DToStringTest()
    {
        var a = new Matrix3x2D
        {
            M11 = 11.0d,
            M12 = -12.0d,
            M21 = 21.0d,
            M22 = 22.0d,
            M31 = 31.0d,
            M32 = 32.0d,
        };

        var expected = "{ {M11:11 M12:-12} " +
                       "{M21:21 M22:22} " +
                       "{M31:31 M32:32} }";

        var actual = a.ToString();
        await Assert.That(actual).IsEquivalentTo(expected);
    }

    // A test for Add (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DAddTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-3.0d);

        var expected = new Matrix3x2D
        {
            M11 = a.M11 + b.M11,
            M12 = a.M12 + b.M12,
            M21 = a.M21 + b.M21,
            M22 = a.M22 + b.M22,
            M31 = a.M31 + b.M31,
            M32 = a.M32 + b.M32,
        };

        var actual = Matrix3x2D.Add(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Equals (object)
    [Test]
    public async Task Matrix3x2DEqualsTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        object obj = b;

        var expected = true;
        var actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0d;
        obj = b;
        expected = false;
        actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);

        // case 3: compare between different types.
        obj = new Vector4D();
        expected = false;
        actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);

        // case 3: compare against null.
        obj = null!;
        expected = false;
        actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for GetHashCode ()
    [Test]
    public async Task Matrix3x2DGetHashCodeTest()
    {
        var target = GenerateIncrementalMatrixNumber();

        var expected = HashCode.Combine(
            new Vector2D(target.M11, target.M12),
            new Vector2D(target.M21, target.M22),
            new Vector2D(target.M31, target.M32)
        );

        var actual = target.GetHashCode();

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DMultiplyTest3()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-3.0d);

        var expected = new Matrix3x2D
        {
            M11 = a.M11 * b.M11 + a.M12 * b.M21,
            M12 = a.M11 * b.M12 + a.M12 * b.M22,
            M21 = a.M21 * b.M11 + a.M22 * b.M21,
            M22 = a.M21 * b.M12 + a.M22 * b.M22,
            M31 = a.M31 * b.M11 + a.M32 * b.M21 + b.M31,
            M32 = a.M31 * b.M12 + a.M32 * b.M22 + b.M32,
        };

        var actual = Matrix3x2D.Multiply(a, b);

        await Assert.That(actual).IsEqualTo(expected);

        // Sanity check by comparison with 4x4 multiply.
        a = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30D)) * Matrix3x2D.CreateTranslation(23, 42);
        b = Matrix3x2D.CreateScale(3, 7) * Matrix3x2D.CreateTranslation(666, -1);

        actual = Matrix3x2D.Multiply(a, b);

        var a44 = new Matrix4x4D(a);
        var b44 = new Matrix4x4D(b);
        var expected44 = Matrix4x4D.Multiply(a44, b44);
        var actual44 = new Matrix4x4D(actual);

        await Assert.That(actual44).IsEqualTo(expected44);
    }

    // A test for Multiply (Matrix3x2D, double)
    [Test]
    public async Task Matrix3x2DMultiplyTest5()
    {
        var a = GenerateIncrementalMatrixNumber();
        var expected = new Matrix3x2D(3, 6, 9, 12, 15, 18);
        var actual = Matrix3x2D.Multiply(a, 3);

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Matrix3x2D, double)
    [Test]
    public async Task Matrix3x2DMultiplyTest6()
    {
        var a = GenerateIncrementalMatrixNumber();
        var expected = new Matrix3x2D(3, 6, 9, 12, 15, 18);
        var actual = a * 3;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Negate (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DNegateTest()
    {
        var m = GenerateIncrementalMatrixNumber();

        var expected = new Matrix3x2D
        {
            M11 = -1.0d,
            M12 = -2.0d,
            M21 = -3.0d,
            M22 = -4.0d,
            M31 = -5.0d,
            M32 = -6.0d,
        };

        var actual = Matrix3x2D.Negate(m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator != (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DInequalityTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        var expected = false;
        var actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0d;
        expected = true;
        actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator == (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DEqualityTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        var expected = true;
        var actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0d;
        expected = false;
        actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Subtract (Matrix3x2D, Matrix3x2D)
    [Test]
    public async Task Matrix3x2DSubtractTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-3.0d);
        var expected = new Matrix3x2D
        {
            M11 = a.M11 - b.M11,
            M12 = a.M12 - b.M12,
            M21 = a.M21 - b.M21,
            M22 = a.M22 - b.M22,
            M31 = a.M31 - b.M31,
            M32 = a.M32 - b.M32,
        };

        var actual = Matrix3x2D.Subtract(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateScale (Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateScaleTest1()
    {
        var scales = new Vector2D(2.0d, 3.0d);
        var expected = new Matrix3x2D(
            2.0d, 0.0d,
            0.0d, 3.0d,
            0.0d, 0.0d);
        var actual = Matrix3x2D.CreateScale(scales);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateScale (Vector2Df, Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateScaleCenterTest1()
    {
        var scale = new Vector2D(3, 4);
        var center = new Vector2D(23, 42);

        var scaleAroundZero = Matrix3x2D.CreateScale(scale, Vector2D.Zero);
        var scaleAroundZeroExpected = Matrix3x2D.CreateScale(scale);
        await Assert.That(scaleAroundZero).IsEqualTo(scaleAroundZeroExpected);

        var scaleAroundCenter = Matrix3x2D.CreateScale(scale, center);
        var scaleAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateScale(scale) * Matrix3x2D.CreateTranslation(center);
        await Assert.That(scaleAroundCenter).IsEqualTo(scaleAroundCenterExpected);
    }

    // A test for CreateScale (double)
    [Test]
    public async Task Matrix3x2DCreateScaleTest2()
    {
        var scale = 2.0d;
        var expected = new Matrix3x2D(
            2.0d, 0.0d,
            0.0d, 2.0d,
            0.0d, 0.0d);
        var actual = Matrix3x2D.CreateScale(scale);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateScale (double, Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateScaleCenterTest2()
    {
        double scale = 5;
        var center = new Vector2D(23, 42);

        var scaleAroundZero = Matrix3x2D.CreateScale(scale, Vector2D.Zero);
        var scaleAroundZeroExpected = Matrix3x2D.CreateScale(scale);
        await Assert.That(scaleAroundZero).IsEqualTo(scaleAroundZeroExpected);

        var scaleAroundCenter = Matrix3x2D.CreateScale(scale, center);
        var scaleAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateScale(scale) * Matrix3x2D.CreateTranslation(center);
        await Assert.That(scaleAroundCenter).IsEqualTo(scaleAroundCenterExpected);
    }

    // A test for CreateScale (double, double)
    [Test]
    public async Task Matrix3x2DCreateScaleTest3()
    {
        var xScale = 2.0d;
        var yScale = 3.0d;
        var expected = new Matrix3x2D(
            2.0d, 0.0d,
            0.0d, 3.0d,
            0.0d, 0.0d);
        var actual = Matrix3x2D.CreateScale(xScale, yScale);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateScale (double, double, Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateScaleCenterTest3()
    {
        var scale = new Vector2D(3, 4);
        var center = new Vector2D(23, 42);

        var scaleAroundZero = Matrix3x2D.CreateScale(scale.X, scale.Y, Vector2D.Zero);
        var scaleAroundZeroExpected = Matrix3x2D.CreateScale(scale.X, scale.Y);
        await Assert.That(scaleAroundZero).IsEqualTo(scaleAroundZeroExpected);

        var scaleAroundCenter = Matrix3x2D.CreateScale(scale.X, scale.Y, center);
        var scaleAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateScale(scale.X, scale.Y) * Matrix3x2D.CreateTranslation(center);
        await Assert.That(scaleAroundCenter).IsEqualTo(scaleAroundCenterExpected);
    }

    // A test for CreateTranslation (Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateTranslationTest1()
    {
        var position = new Vector2D(2.0d, 3.0d);
        var expected = new Matrix3x2D(
            1.0d, 0.0d,
            0.0d, 1.0d,
            2.0d, 3.0d);

        var actual = Matrix3x2D.CreateTranslation(position);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateTranslation (double, double)
    [Test]
    public async Task Matrix3x2DCreateTranslationTest2()
    {
        var xPosition = 2.0d;
        var yPosition = 3.0d;

        var expected = new Matrix3x2D(
            1.0d, 0.0d,
            0.0d, 1.0d,
            2.0d, 3.0d);

        var actual = Matrix3x2D.CreateTranslation(xPosition, yPosition);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Translation
    [Test]
    public async Task Matrix3x2DTranslationTest()
    {
        var a = GenerateTestMatrix();
        var b = a;

        // Transformed vector that has same semantics of property must be same.
        var val = new Vector2D(a.M31, a.M32);
        await Assert.That(a.Translation).IsEqualTo(val);

        // Set value and get value must be same.
        val = new(1.0d, 2.0d);
        a.Translation = val;
        await Assert.That(a.Translation).IsEqualTo(val);

        // Make sure it only modifies expected value of matrix.
        
        await Assert.That(a.M11).IsEqualTo(b.M11);
        await Assert.That(a.M12).IsEqualTo(b.M12);
        await Assert.That(a.M21).IsEqualTo(b.M21); 
        await Assert.That(a.M22).IsEqualTo(b.M22);
        await Assert.That(a.M31).IsNotEqualTo(b.M31);
        await Assert.That(a.M32).IsNotEqualTo(b.M32);
    }

    // A test for Equals (Matrix3x2D)
    [Test]
    public async Task Matrix3x2DEqualsTest1()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        var expected = true;
        var actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0d;
        expected = false;
        actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateSkew (double, double)
    [Test]
    public async Task Matrix3x2DCreateSkewIdentityTest()
    {
        var expected = Matrix3x2D.Identity;
        var actual = Matrix3x2D.CreateSkew(0, 0);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateSkew (double, double)
    [Test]
    public async Task Matrix3x2DCreateSkewXTest()
    {
        var expected = new Matrix3x2D(1, 0, -0.414213562373095d, 1, 0, 0);
        var actual = Matrix3x2D.CreateSkew(-Math.PI / 8, 0);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);

        expected = new(1, 0, 0.414213562373095d, 1, 0, 0);
        actual = Matrix3x2D.CreateSkew(Math.PI / 8, 0);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);

        var result = Vector2D.Transform(new(0, 0), actual);
        await Assert.That(result).IsEqualTo(new(0, 0));

        result = Vector2D.Transform(new(0, 1), actual);
        await Assert.That(result).IsEqualTo(new(0.414213568d, 1)).Within(1E-8);

        result = Vector2D.Transform(new(0, -1), actual);
        await Assert.That(result).IsEqualTo(new(-0.414213568d, -1)).Within(1E-8);

        result = Vector2D.Transform(new(3, 10), actual);
        await Assert.That(result).IsEqualTo(new(7.14213568d, 10)).Within(1E-7);
    }

    // A test for CreateSkew (double, double)
    [Test]
    public async Task Matrix3x2DCreateSkewYTest()
    {
        var expected = new Matrix3x2D(1, -0.414213562373095d, 0, 1, 0, 0);
        var actual = Matrix3x2D.CreateSkew(0, -Math.PI / 8);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);

        expected = new(1, 0.414213562373095d, 0, 1, 0, 0);
        actual = Matrix3x2D.CreateSkew(0, Math.PI / 8);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);

        var result = Vector2D.Transform(new(0, 0), actual);
        await Assert.That(result).IsEqualTo(new(0, 0));

        result = Vector2D.Transform(new(1, 0), actual);
        await Assert.That(result).IsEqualTo(new(1, 0.414213568d)).Within(1E-8);

        result = Vector2D.Transform(new(-1, 0), actual);
        await Assert.That(result).IsEqualTo(new(-1, -0.414213568d)).Within(1E-8);

        result = Vector2D.Transform(new(10, 3), actual);
        await Assert.That(result).IsEqualTo(new(10, 7.14213568d)).Within(1E-7);
    }

    // A test for CreateSkew (double, double)
    [Test]
    public async Task Matrix3x2DCreateSkewXYTest()
    {
        var expected = new Matrix3x2D(1, -0.414213562373095d, 1, 1, 0, 0);
        var actual = Matrix3x2D.CreateSkew(Math.PI / 4, -Math.PI / 8);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);

        var result = Vector2D.Transform(new(0, 0), actual);
        await Assert.That(result).IsEqualTo(new(0, 0));

        result = Vector2D.Transform(new(1, 0), actual);
        await Assert.That(result).IsEqualTo(new(1, -0.414213562373095d)).Within(1E-15);

        result = Vector2D.Transform(new(0, 1), actual);
        await Assert.That(result).IsEqualTo(new(1, 1)).Within(1E-15);

        result = Vector2D.Transform(new(1, 1), actual);
        await Assert.That(result).IsEqualTo(new(2, 0.585786437626905d));
    }

    // A test for CreateSkew (double, double, Vector2Df)
    [Test]
    public async Task Matrix3x2DCreateSkewCenterTest()
    {
        double skewX = 1, skewY = 2;
        var center = new Vector2D(23, 42);

        var skewAroundZero = Matrix3x2D.CreateSkew(skewX, skewY, Vector2D.Zero);
        var skewAroundZeroExpected = Matrix3x2D.CreateSkew(skewX, skewY);
        await Assert.That(skewAroundZero).IsEqualTo(skewAroundZeroExpected);

        var skewAroundCenter = Matrix3x2D.CreateSkew(skewX, skewY, center);
        var skewAroundCenterExpected = Matrix3x2D.CreateTranslation(-center) * Matrix3x2D.CreateSkew(skewX, skewY) * Matrix3x2D.CreateTranslation(center);
        await Assert.That(skewAroundCenter).IsEqualTo(skewAroundCenterExpected);
    }

    // A test for IsIdentity
    [Test]
    public async Task Matrix3x2DIsIdentityTest()
    {
        await Assert.That(Matrix3x2D.Identity.IsIdentity).IsTrue();
        await Assert.That(new Matrix3x2D(1, 0, 0, 1, 0, 0).IsIdentity).IsTrue();
        await Assert.That(new Matrix3x2D(0, 0, 0, 1, 0, 0).IsIdentity).IsFalse();
        await Assert.That(new Matrix3x2D(1, 1, 0, 1, 0, 0).IsIdentity).IsFalse();
        await Assert.That(new Matrix3x2D(1, 0, 1, 1, 0, 0).IsIdentity).IsFalse();
        await Assert.That(new Matrix3x2D(1, 0, 0, 0, 0, 0).IsIdentity).IsFalse();
        await Assert.That(new Matrix3x2D(1, 0, 0, 1, 1, 0).IsIdentity).IsFalse();
        await Assert.That(new Matrix3x2D(1, 0, 0, 1, 0, 1).IsIdentity).IsFalse();
    }

    // A test for Matrix3x2D comparison involving NaN values
    [Test]
    public async Task Matrix3x2DEqualsNaNTest()
    {
        var a = new Matrix3x2D(double.NaN, 0, 0, 0, 0, 0);
        var b = new Matrix3x2D(0, double.NaN, 0, 0, 0, 0);
        var c = new Matrix3x2D(0, 0, double.NaN, 0, 0, 0);
        var d = new Matrix3x2D(0, 0, 0, double.NaN, 0, 0);
        var e = new Matrix3x2D(0, 0, 0, 0, double.NaN, 0);
        var f = new Matrix3x2D(0, 0, 0, 0, 0, double.NaN);

        await Assert.That(a == new Matrix3x2D()).IsFalse();
        await Assert.That(b == new Matrix3x2D()).IsFalse();
        await Assert.That(c == new Matrix3x2D()).IsFalse();
        await Assert.That(d == new Matrix3x2D()).IsFalse();
        await Assert.That(e == new Matrix3x2D()).IsFalse();
        await Assert.That(f == new Matrix3x2D()).IsFalse();

        await Assert.That(a != new Matrix3x2D()).IsTrue();
        await Assert.That(b != new Matrix3x2D()).IsTrue();
        await Assert.That(c != new Matrix3x2D()).IsTrue();
        await Assert.That(d != new Matrix3x2D()).IsTrue();
        await Assert.That(e != new Matrix3x2D()).IsTrue();
        await Assert.That(f != new Matrix3x2D()).IsTrue();

        await Assert.That(a.Equals(new())).IsFalse();
        await Assert.That(b.Equals(new())).IsFalse();
        await Assert.That(c.Equals(new())).IsFalse();
        await Assert.That(d.Equals(new())).IsFalse();
        await Assert.That(e.Equals(new())).IsFalse();
        await Assert.That(f.Equals(new())).IsFalse();

        await Assert.That(a.IsIdentity).IsFalse();
        await Assert.That(b.IsIdentity).IsFalse();
        await Assert.That(c.IsIdentity).IsFalse();
        await Assert.That(d.IsIdentity).IsFalse();
        await Assert.That(e.IsIdentity).IsFalse();
        await Assert.That(f.IsIdentity).IsFalse();

        await Assert.That(a.Equals(a)).IsTrue();
        await Assert.That(b.Equals(b)).IsTrue();
        await Assert.That(c.Equals(c)).IsTrue();
        await Assert.That(d.Equals(d)).IsTrue();
        await Assert.That(e.Equals(e)).IsTrue();
        await Assert.That(f.Equals(f)).IsTrue();
    }

    // A test to make sure these types are blittable directly into GPU buffer memory layouts
    [Test]
    public async Task Matrix3x2DSizeofTest()
    {
        int sizeofMatrix3x2D;
        int sizeofMatrix3x2D2;
        int sizeofMatrix3x2DPlusDouble;
        int sizeofMatrix3x2DPlusDouble2;
        
        unsafe
        {
            sizeofMatrix3x2D = sizeof(Matrix3x2D);
            sizeofMatrix3x2D2 = sizeof(Matrix3x2D_2x);
            sizeofMatrix3x2DPlusDouble = sizeof(Matrix3x2DPlusDouble);
            sizeofMatrix3x2DPlusDouble2 = sizeof(Matrix3x2DPlusDouble_2x);
        }
        
        await Assert.That(sizeofMatrix3x2D).IsEqualTo(48);
        await Assert.That(sizeofMatrix3x2D2).IsEqualTo(96);
        await Assert.That(sizeofMatrix3x2DPlusDouble).IsEqualTo(56);
        await Assert.That(sizeofMatrix3x2DPlusDouble2).IsEqualTo(112);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Matrix3x2D_2x
    {
        private Matrix3x2D _a;
        private Matrix3x2D _b;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Matrix3x2DPlusDouble
    {
        private Matrix3x2D _v;
        private double _f;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct Matrix3x2DPlusDouble_2x
    {
        private Matrix3x2DPlusDouble _a;
        private Matrix3x2DPlusDouble _b;
    }

    // A test to make sure the fields are laid out how we expect
    [Test]
    public async Task Matrix3x2DFieldOffsetTest()
    {
        IntPtr basePtr;
        IntPtr intPtrMat;
        IntPtr intPtrMatM11;
        IntPtr intPtrMatM12;
        IntPtr intPtrMatM21;
        IntPtr intPtrMatM22;
        IntPtr intPtrMatM31;
        IntPtr intPtrMatM32;
        
        unsafe
        {
#pragma warning disable CS9123 // The '&' operator should not be used on parameters or local variables in async methods.
            var mat = new Matrix3x2D();
            basePtr = new(&mat.M11); // Take address of first element
            intPtrMat = new(&mat);
            intPtrMatM11 = new(&mat.M11);
            intPtrMatM12 = new(&mat.M12);
            intPtrMatM21 = new(&mat.M21);
            intPtrMatM22 = new(&mat.M22);
            intPtrMatM31 = new(&mat.M31);
            intPtrMatM32 = new(&mat.M32);
#pragma warning restore CS9123 // The '&' operator should not be used on parameters or local variables in async methods.
        }

        await Assert.That(intPtrMat).IsEqualTo(basePtr);
        await Assert.That(intPtrMatM11).IsEqualTo(basePtr + 0);
        await Assert.That(intPtrMatM12).IsEqualTo(basePtr + 8);
        await Assert.That(intPtrMatM21).IsEqualTo(basePtr + 16);
        await Assert.That(intPtrMatM22).IsEqualTo(basePtr + 24);
        await Assert.That(intPtrMatM31).IsEqualTo(basePtr + 32);
        await Assert.That(intPtrMatM32).IsEqualTo(basePtr + 40);
    }

    [Test]
    public async Task Matrix3x2DCreateBroadcastScalarTest()
    {
        var a = Matrix3x2D.Create(double.Pi);

        await Assert.That(a.X).IsEqualTo(Vector2D.Pi);
        await Assert.That(a.Y).IsEqualTo(Vector2D.Pi);
        await Assert.That(a.Z).IsEqualTo(Vector2D.Pi);
    }

    [Test]
    public async Task Matrix3x2DCreateBroadcastVectorTest()
    {
        var a = Matrix3x2D.Create(Vector2D.Create(double.Pi, double.E));

        await Assert.That(a.X).IsEqualTo(Vector2D.Create(double.Pi, double.E));
        await Assert.That(a.Y).IsEqualTo(Vector2D.Create(double.Pi, double.E));
        await Assert.That(a.Z).IsEqualTo(Vector2D.Create(double.Pi, double.E));
    }

    [Test]
    public async Task Matrix3x2DCreateVectorsTest()
    {
        var a = Matrix3x2D.Create(
            Vector2D.Create(11.0d, 12.0d),
            Vector2D.Create(21.0d, 22.0d),
            Vector2D.Create(31.0d, 32.0d)
        );

        await Assert.That(a.X).IsEqualTo(Vector2D.Create(11.0d, 12.0d));
        await Assert.That(a.Y).IsEqualTo(Vector2D.Create(21.0d, 22.0d));
        await Assert.That(a.Z).IsEqualTo(Vector2D.Create(31.0d, 32.0d));
    }

    [Test]
    public async Task Matrix3x2DGetElementTest()
    {
        var a = GenerateTestMatrix();

        await Assert.That(a.X.X).IsEqualTo(a.M11);
        await Assert.That(a[0, 0]).IsEqualTo(a.M11);
        await Assert.That(a.GetElement(0, 0)).IsEqualTo(a.M11);

        await Assert.That(a.X.Y).IsEqualTo(a.M12);
        await Assert.That(a[0, 1]).IsEqualTo(a.M12);
        await Assert.That(a.GetElement(0, 1)).IsEqualTo(a.M12);

        await Assert.That(a.Y.X).IsEqualTo(a.M21);
        await Assert.That(a[1, 0]).IsEqualTo(a.M21);
        await Assert.That(a.GetElement(1, 0)).IsEqualTo(a.M21);

        await Assert.That(a.Y.Y).IsEqualTo(a.M22);
        await Assert.That(a[1, 1]).IsEqualTo(a.M22);
        await Assert.That(a.GetElement(1, 1)).IsEqualTo(a.M22);

        await Assert.That(a.Z.X).IsEqualTo(a.M31);
        await Assert.That(a[2, 0]).IsEqualTo(a.M31);
        await Assert.That(a.GetElement(2, 0)).IsEqualTo(a.M31);

        await Assert.That(a.Z.Y).IsEqualTo(a.M32);
        await Assert.That(a[2, 1]).IsEqualTo(a.M32);
        await Assert.That(a.GetElement(2, 1)).IsEqualTo(a.M32);
    }

    [Test]
    public async Task Matrix3x2DGetRowTest()
    {
        var a = GenerateTestMatrix();

        var vx = new Vector2D(a.M11, a.M12);
        await Assert.That(a.X).IsEqualTo(vx);
        await Assert.That(a[0]).IsEqualTo(vx);
        await Assert.That(a.GetRow(0)).IsEqualTo(vx);

        var vy = new Vector2D(a.M21, a.M22);
        await Assert.That(a.Y).IsEqualTo(vy);
        await Assert.That(a[1]).IsEqualTo(vy);
        await Assert.That(a.GetRow(1)).IsEqualTo(vy);

        var vz = new Vector2D(a.M31, a.M32);
        await Assert.That(a.Z).IsEqualTo(vz);
        await Assert.That(a[2]).IsEqualTo(vz);
        await Assert.That(a.GetRow(2)).IsEqualTo(vz);
    }

    [Test]
    public async Task Matrix3x2DWithElementTest()
    {
        var a = Matrix3x2D.Identity;

        a[0, 0] = 11.0d;
        await Assert.That(a.WithElement(0, 0, 11.5d).M11).IsEqualTo(11.5d);
        await Assert.That(a.M11).IsEqualTo(11.0d);

        a[0, 1] = 12.0d;
        await Assert.That(a.WithElement(0, 1, 12.5d).M12).IsEqualTo(12.5d);
        await Assert.That(a.M12).IsEqualTo(12.0d);

        a[1, 0] = 21.0d;
        await Assert.That(a.WithElement(1, 0, 21.5d).M21).IsEqualTo(21.5d);
        await Assert.That(a.M21).IsEqualTo(21.0d);

        a[1, 1] = 22.0d;
        await Assert.That(a.WithElement(1, 1, 22.5d).M22).IsEqualTo(22.5d);
        await Assert.That(a.M22).IsEqualTo(22.0d);

        a[2, 0] = 31.0d;
        await Assert.That(a.WithElement(2, 0, 31.5d).M31).IsEqualTo(31.5d);
        await Assert.That(a.M31).IsEqualTo(31.0d);

        a[2, 1] = 32.0d;
        await Assert.That(a.WithElement(2, 1, 32.5d).M32).IsEqualTo(32.5d);
        await Assert.That(a.M32).IsEqualTo(32.0d);
    }

    [Test]
    public async Task Matrix3x2DWithRowTest()
    {
        var a = Matrix3x2D.Identity;

        a[0] = Vector2D.Create(11.0d, 12.0d);
        await Assert.That(a.WithRow(0, Vector2D.Create(11.5d, 12.5d)).X).IsEqualTo(Vector2D.Create(11.5d, 12.5d));
        await Assert.That(a.X).IsEqualTo(Vector2D.Create(11.0d, 12.0d));

        a[1] = Vector2D.Create(21.0d, 22.0d);
        await Assert.That(a.WithRow(1, Vector2D.Create(21.5d, 22.5d)).Y).IsEqualTo(Vector2D.Create(21.5d, 22.5d));
        await Assert.That(a.Y).IsEqualTo(Vector2D.Create(21.0d, 22.0d));

        a[2] = Vector2D.Create(31.0d, 32.0d);
        await Assert.That(a.WithRow(2, Vector2D.Create(31.5d, 32.5d)).Z).IsEqualTo(Vector2D.Create(31.5d, 32.5d));
        await Assert.That(a.Z).IsEqualTo(Vector2D.Create(31.0d, 32.0d));
    }
}