namespace Altemiq.Numerics;

using System.Globalization;
using System.Runtime.InteropServices;

[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
public sealed class Matrix4x4DTests
{
    private static readonly Matrix4x4D DefaultVarianceMatrix = GenerateFilledMatrix(1e-5d);

    private static Matrix4x4D GenerateIncrementalMatrixNumber(double value = 0.0)
    {
        var a = new Matrix4x4D
        {
            M11 = value + 1.0,
            M12 = value + 2.0,
            M13 = value + 3.0,
            M14 = value + 4.0,
            M21 = value + 5.0,
            M22 = value + 6.0,
            M23 = value + 7.0,
            M24 = value + 8.0,
            M31 = value + 9.0,
            M32 = value + 10.0,
            M33 = value + 11.0,
            M34 = value + 12.0,
            M41 = value + 13.0,
            M42 = value + 14.0,
            M43 = value + 15.0,
            M44 = value + 16.0,
        };
        return a;
    }

    private static Matrix4x4D GenerateTestMatrix()
    {
        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.Translation = new(111.0, 222.0, 333.0);
        return m;
    }

    private static Matrix4x4D GenerateFilledMatrix(double value) => new()
    {
        M11 = value,
        M12 = value,
        M13 = value,
        M14 = value,
        M21 = value,
        M22 = value,
        M23 = value,
        M24 = value,
        M31 = value,
        M32 = value,
        M33 = value,
        M34 = value,
        M41 = value,
        M42 = value,
        M43 = value,
        M44 = value
    };

    private static Vector3D InverseHandedness(Vector3D vector) => new(vector.X, vector.Y, -vector.Z);

    // The handedness-swapped matrix of matrix M is B^-1 * M * B where B is the change of handedness matrix.
    // Since only the Z coordinate is flipped when changing handedness,
    //
    // B = [ 1  0  0  0
    //       0  1  0  0
    //       0  0 -1  0
    //       0  0  0  1 ]
    //
    // and B is its own inverse. So the handedness swap can be simplified to
    //
    // B^-1 * M * B = [  m11  m12  -m13  m14
    //                   m21  m22  -m23  m24
    //                  -m31 -m32   m33 -m34
    //                   m41  m42  -m43  m44 ]
    private static Matrix4x4D InverseHandedness(Matrix4x4D matrix) => new(
        matrix.M11, matrix.M12, -matrix.M13, matrix.M14,
        matrix.M21, matrix.M22, -matrix.M23, matrix.M24,
        -matrix.M31, -matrix.M32, matrix.M33, -matrix.M34,
        matrix.M41, matrix.M42, -matrix.M43, matrix.M44);

    [Test]
    [Arguments(0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0)]
    [Arguments(1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0)]
    [Arguments(3.1434343, 1.1234123, 0.1234123, -0.1234123, 3.1434343, 1.1234123, 3.1434343, 1.1234123, 0.1234123, -0.1234123, 3.1434343, 1.1234123, 3.1434343, 1.1234123, 0.1234123, -0.1234123)]
    [Arguments(1.0000001, 0.0000001, 2.0000001, 0.0000002, 1.0000001, 0.0000001, 1.0000001, 0.0000001, 2.0000001, 0.0000002, 1.0000001, 0.0000001, 1.0000001, 0.0000001, 2.0000001, 0.0000002)]
    public async Task Matrix4x4DIndexerGetTest(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
    {
        var matrix = new Matrix4x4D(m11, m12, m13, m14, m21, m22, m23, m24, m31, m32, m33, m34, m41, m42, m43, m44);

        await Assert.That(matrix[0, 0]).IsEqualTo(m11);
        await Assert.That(matrix[0, 1]).IsEqualTo(m12);
        await Assert.That(matrix[0, 2]).IsEqualTo(m13);
        await Assert.That(matrix[0, 3]).IsEqualTo(m14);

        await Assert.That(matrix[1, 0]).IsEqualTo(m21);
        await Assert.That(matrix[1, 1]).IsEqualTo(m22);
        await Assert.That(matrix[1, 2]).IsEqualTo(m23);
        await Assert.That(matrix[1, 3]).IsEqualTo(m24);

        await Assert.That(matrix[2, 0]).IsEqualTo(m31);
        await Assert.That(matrix[2, 1]).IsEqualTo(m32);
        await Assert.That(matrix[2, 2]).IsEqualTo(m33);
        await Assert.That(matrix[2, 3]).IsEqualTo(m34);

        await Assert.That(matrix[3, 0]).IsEqualTo(m41);
        await Assert.That(matrix[3, 1]).IsEqualTo(m42);
        await Assert.That(matrix[3, 2]).IsEqualTo(m43);
        await Assert.That(matrix[3, 3]).IsEqualTo(m44);
    }

    [Test]
    [Arguments(0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0)]
    [Arguments(1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0, 1.0, 0.0)]
    [Arguments(3.1434343, 1.1234123, 0.1234123, -0.1234123, 3.1434343, 1.1234123, 3.1434343, 1.1234123, 0.1234123, -0.1234123, 3.1434343, 1.1234123, 3.1434343, 1.1234123, 0.1234123, -0.1234123)]
    [Arguments(1.0000001, 0.0000001, 2.0000001, 0.0000002, 1.0000001, 0.0000001, 1.0000001, 0.0000001, 2.0000001, 0.0000002, 1.0000001, 0.0000001, 1.0000001, 0.0000001, 2.0000001, 0.0000002)]
    public async Task Matrix4x4DIndexerSetTest(double m11, double m12, double m13, double m14, double m21, double m22, double m23, double m24, double m31, double m32, double m33, double m34, double m41, double m42, double m43, double m44)
    {
        var matrix = new Matrix4x4D(0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0)
        {
            [0, 0] = m11,
            [0, 1] = m12,
            [0, 2] = m13,
            [0, 3] = m14,
            [1, 0] = m21,
            [1, 1] = m22,
            [1, 2] = m23,
            [1, 3] = m24,
            [2, 0] = m31,
            [2, 1] = m32,
            [2, 2] = m33,
            [2, 3] = m34,
            [3, 0] = m41,
            [3, 1] = m42,
            [3, 2] = m43,
            [3, 3] = m44,
        };

        await Assert.That(matrix[0, 0]).IsEqualTo(m11);
        await Assert.That(matrix[0, 1]).IsEqualTo(m12);
        await Assert.That(matrix[0, 2]).IsEqualTo(m13);
        await Assert.That(matrix[0, 3]).IsEqualTo(m14);

        await Assert.That(matrix[1, 0]).IsEqualTo(m21);
        await Assert.That(matrix[1, 1]).IsEqualTo(m22);
        await Assert.That(matrix[1, 2]).IsEqualTo(m23);
        await Assert.That(matrix[1, 3]).IsEqualTo(m24);

        await Assert.That(matrix[2, 0]).IsEqualTo(m31);
        await Assert.That(matrix[2, 1]).IsEqualTo(m32);
        await Assert.That(matrix[2, 2]).IsEqualTo(m33);
        await Assert.That(matrix[2, 3]).IsEqualTo(m34);

        await Assert.That(matrix[3, 0]).IsEqualTo(m41);
        await Assert.That(matrix[3, 1]).IsEqualTo(m42);
        await Assert.That(matrix[3, 2]).IsEqualTo(m43);
        await Assert.That(matrix[3, 3]).IsEqualTo(m44);
    }

    // A test for Identity
    [Test]
    public async Task Matrix4x4DIdentityTest()
    {
        var val = new Matrix4x4D();
        val.M11 = val.M22 = val.M33 = val.M44 = 1.0;

        await Assert.That(val).IsEqualTo(Matrix4x4D.Identity);
    }

    // A test for Determinant
    [Test]
    public async Task Matrix4x4DDeterminantTest()
    {
        var target =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));

        var val = 1.0;
        var det = target.GetDeterminant();

        await Assert.That(det).IsEqualTo(val).Within(MathHelper.HighQualityTolerance);
    }

    // Determinant test |A| = 1 / |A'|
    [Test]
    public async Task Matrix4x4DDeterminantTest1()
    {
        var a = new Matrix4x4D
        {
            M11 = 5.0,
            M12 = 2.0,
            M13 = 8.25,
            M14 = 1.0,
            M21 = 12.0,
            M22 = 6.8,
            M23 = 2.14,
            M24 = 9.6,
            M31 = 6.5,
            M32 = 1.0,
            M33 = 3.14,
            M34 = 2.22,
            M41 = 0.0,
            M42 = 0.86,
            M43 = 4.0,
            M44 = 1.0,
        };
        await Assert.That(Matrix4x4D.Invert(a, out var i)).IsTrue();

        var detA = a.GetDeterminant();
        var detI = i.GetDeterminant();
        var t = 1.0 / detI;

        // only accurate to 3 precision
        await Assert.That(detA).IsEqualTo(t).Within(MathHelper.LowQualityTolerance);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertTest()
    {
        var mtx =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));

        var expected = new Matrix4x4D
        {
            M11 = 0.74999994,
            M12 = -0.216506317,
            M13 = 0.62499994,
            M14 = 0.0,
            M21 = 0.433012635,
            M22 = 0.87499994,
            M23 = -0.216506317,
            M24 = 0.0,
            M31 = -0.49999997,
            M32 = 0.433012635,
            M33 = 0.74999994,
            M34 = 0.0,
            M41 = 0.0,
            M42 = 0.0,
            M43 = 0.0,
            M44 = 0.99999994,
        };

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsTrue();
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);

        // Make sure M*M is identity matrix
        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix4x4D.Identity).Within(MathHelper.HighQualityTolerance);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertIdentityTest()
    {
        var mtx = Matrix4x4D.Identity;

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsTrue();

        await Assert.That(actual).IsEqualTo(Matrix4x4D.Identity);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertTranslationTest()
    {
        var mtx = Matrix4x4D.CreateTranslation(23, 42, 666);

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix4x4D.Identity);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertRotationTest()
    {
        var mtx = Matrix4x4D.CreateFromYawPitchRoll(3, 4, 5);

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix4x4D.Identity).Within(MathHelper.HighQualityTolerance);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertScaleTest()
    {
        var mtx = Matrix4x4D.CreateScale(23, 42, -666);

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix4x4D.Identity);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertProjectionTest()
    {
        var mtx = Matrix4x4D.CreatePerspectiveFieldOfView(1, 1.333, 0.1, 666);

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix4x4D.Identity).Within(MathHelper.HighQualityTolerance);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertAffineTest()
    {
        var mtx = Matrix4x4D.CreateFromYawPitchRoll(3, 4, 5) *
                  Matrix4x4D.CreateScale(23, 42, -666) *
                  Matrix4x4D.CreateTranslation(17, 53, 89);

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsTrue();

        var i = mtx * actual;
        await Assert.That(i).IsEqualTo(Matrix4x4D.Identity).Within(MathHelper.HighQualityTolerance);
    }

    // A test for Invert (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInvertRank3()
    {
        // A 4x4 Matrix having a rank of 3
        var mtx = new Matrix4x4D(1.0, 2.0, 3.0, 0.0,
            5.0, 1.0, 6.0, 0.0,
            8.0, 9.0, 1.0, 0.0,
            4.0, 7.0, 3.0, 0.0);

        await Assert.That(Matrix4x4D.Invert(mtx, out var actual)).IsFalse();

        var i = mtx * actual;
        await Assert.That(i).IsNotEqualTo(Matrix4x4D.Identity);
    }

    static async Task DecomposeTest(double yaw, double pitch, double roll, Vector3D expectedTranslation, Vector3D expectedScales)
    {
        var expectedRotation = QuaternionD.CreateFromYawPitchRoll(MathHelper.ToRadians(yaw),
            MathHelper.ToRadians(pitch),
            MathHelper.ToRadians(roll));

        var m = Matrix4x4D.CreateScale(expectedScales) *
                Matrix4x4D.CreateFromQuaternion(expectedRotation) *
                Matrix4x4D.CreateTranslation(expectedTranslation);

        var actualResult = Matrix4x4D.Decompose(m, out var scales, out var rotation, out var translation);
        await Assert.That(actualResult).IsTrue();

        var scaleIsZeroOrNegative = expectedScales.X <= 0 ||
                                    expectedScales.Y <= 0 ||
                                    expectedScales.Z <= 0;

        if (scaleIsZeroOrNegative)
        {
            await Assert.That(double.Abs(scales.X)).IsEqualTo(double.Abs(expectedScales.X)).Within(MathHelper.HighQualityTolerance);
            await Assert.That(double.Abs(scales.Y)).IsEqualTo(double.Abs(expectedScales.Y)).Within(MathHelper.HighQualityTolerance);
            await Assert.That(double.Abs(scales.Z)).IsEqualTo(double.Abs(expectedScales.Z)).Within(MathHelper.HighQualityTolerance);
        }
        else
        {
            await Assert.That(scales).IsEqualTo(expectedScales).Within(MathHelper.HighQualityTolerance);
            await Assert.That(rotation).IsEqualTo(expectedRotation).Within(MathHelper.HighQualityTolerance).Or.IsEqualTo(-expectedRotation).Within(MathHelper.HighQualityTolerance);
        }

        await Assert.That(translation).IsEqualTo(expectedTranslation).Within(MathHelper.HighQualityTolerance);
    }

    // Various rotation decompose test.
    [Test]
    public async Task Matrix4x4DDecomposeTest01()
    {
        await DecomposeTest(10.0, 20.0, 30.0, new(10, 20, 30), new(2, 3, 4));

        const double step = 35.0;

        for (var yawAngle = -720.0; yawAngle <= 720.0; yawAngle += step)
        {
            for (var pitchAngle = -720.0; pitchAngle <= 720.0; pitchAngle += step)
            {
                for (var rollAngle = -720.0; rollAngle <= 720.0; rollAngle += step)
                {
                    await DecomposeTest(yawAngle, pitchAngle, rollAngle, new(10, 20, 30), new(2, 3, 4));
                }
            }
        }
    }

    // Various scaled matrix decompose test.
    [Test]
    public async Task Matrix4x4DDecomposeTest02()
    {
        await DecomposeTest(10.0, 20.0, 30.0, new(10, 20, 30), new(2, 3, 4));

        // Various scales.
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(1, 2, 3));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(1, 3, 2));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(2, 1, 3));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(2, 3, 1));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(3, 1, 2));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(3, 2, 1));

        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(-2, 1, 1));

        // Small scales.
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(1e-4d, 2e-4d, 3e-4d));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(1e-4d, 3e-4d, 2e-4d));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(2e-4d, 1e-4d, 3e-4d));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(2e-4d, 3e-4d, 1e-4d));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(3e-4d, 1e-4d, 2e-4d));
        await DecomposeTest(0, 0, 0, Vector3D.Zero, new(3e-4d, 2e-4d, 1e-4d));

        // Zero scales.
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(0, 0, 0));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(1, 0, 0));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(0, 1, 0));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(0, 0, 1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(0, 1, 1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(1, 0, 1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(1, 1, 0));

        // Negative scales.
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(-1, -1, -1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(1, -1, -1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(-1, 1, -1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(-1, -1, 1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(-1, 1, 1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(1, -1, 1));
        await DecomposeTest(0, 0, 0, new(10, 20, 30), new(1, 1, -1));
    }

    static async Task DecomposeScaleTest(double sx, double sy, double sz)
    {
        var m = Matrix4x4D.CreateScale(sx, sy, sz);

        var expectedScales = new Vector3D(sx, sy, sz);

        var actualResult = Matrix4x4D.Decompose(m, out var scales, out var rotation, out var translation);
        await Assert.That(actualResult).IsTrue();
        await Assert.That(scales).IsEqualTo(expectedScales);
        await Assert.That(rotation).IsEqualTo(QuaternionD.Identity);
        await Assert.That(translation).IsEqualTo(Vector3D.Zero);
    }

    // Tiny scale decompose test.
    [Test]
    public async Task Matrix4x4DDecomposeTest03()
    {
        await DecomposeScaleTest(1, 2e-4d, 3e-4d);
        await DecomposeScaleTest(1, 3e-4d, 2e-4d);
        await DecomposeScaleTest(2e-4d, 1, 3e-4d);
        await DecomposeScaleTest(2e-4d, 3e-4d, 1);
        await DecomposeScaleTest(3e-4d, 1, 2e-4d);
        await DecomposeScaleTest(3e-4d, 2e-4d, 1);
    }

    [Test]
    public async Task Matrix4x4DDecomposeTest04()
    {
        await Assert.That(Matrix4x4D.Decompose(GenerateIncrementalMatrixNumber(), out _, out _, out _)).IsFalse();
        await Assert.That(Matrix4x4D.Decompose(new(Matrix3x2D.CreateSkew(1, 2)), out _, out _, out _)).IsFalse();
    }

    // Transform by quaternionD test
    [Test]
    public async Task Matrix4x4DTransformTest()
    {
        var target = GenerateIncrementalMatrixNumber();

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));

        var q = QuaternionD.CreateFromRotationMatrix(m);

        var expected = target * m;
        var actual = Matrix4x4D.Transform(target, q);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);
    }

    // A test for CreateRotationX (double)
    [Test]
    public async Task Matrix4x4DCreateRotationXTest()
    {
        var radians = MathHelper.ToRadians(30.0);

        var expected = new Matrix4x4D
        {
            M11 = 1.0,
            M22 = 0.8660254,
            M23 = 0.5,
            M32 = -0.5,
            M33 = 0.8660254,
            M44 = 1.0,
        };

        var actual = Matrix4x4D.CreateRotationX(radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    // A test for CreateRotationX (double)
    // CreateRotationX of zero degree
    [Test]
    public async Task Matrix4x4DCreateRotationXTest1()
    {
        double radians = 0;

        var expected = Matrix4x4D.Identity;
        var actual = Matrix4x4D.CreateRotationX(radians);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateRotationX (double, Vector3)
    [Test]
    public async Task Matrix4x4DCreateRotationXCenterTest()
    {
        var radians = MathHelper.ToRadians(30.0);
        var center = new Vector3D(23, 42, 66);

        var rotateAroundZero = Matrix4x4D.CreateRotationX(radians, Vector3D.Zero);
        var rotateAroundZeroExpected = Matrix4x4D.CreateRotationX(radians);
        await Assert.That(rotateAroundZero).IsEqualTo(rotateAroundZeroExpected);

        var rotateAroundCenter = Matrix4x4D.CreateRotationX(radians, center);
        var rotateAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateRotationX(radians) * Matrix4x4D.CreateTranslation(center);
        await Assert.That(rotateAroundCenter).IsEqualTo(rotateAroundCenterExpected).Within(MathHelper.HighQualityTolerance);
    }

    // A test for CreateRotationY (double)
    [Test]
    public async Task Matrix4x4DCreateRotationYTest()
    {
        var radians = MathHelper.ToRadians(60.0);

        var expected = new Matrix4x4D
        {
            M11 = 0.49999997,
            M13 = -0.866025448,
            M22 = 1.0,
            M31 = 0.866025448,
            M33 = 0.49999997,
            M44 = 1.0,
        };

        var actual = Matrix4x4D.CreateRotationY(radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    // A test for RotationY (double)
    // CreateRotationY test for negative angle
    [Test]
    public async Task Matrix4x4DCreateRotationYTest1()
    {
        var radians = MathHelper.ToRadians(-300.0);

        var expected = new Matrix4x4D
        {
            M11 = 0.49999997,
            M13 = -0.866025448,
            M22 = 1.0,
            M31 = 0.866025448,
            M33 = 0.49999997,
            M44 = 1.0,
        };

        var actual = Matrix4x4D.CreateRotationY(radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    // A test for CreateRotationY (double, Vector3)
    [Test]
    public async Task Matrix4x4DCreateRotationYCenterTest()
    {
        var radians = MathHelper.ToRadians(30.0);
        var center = new Vector3D(23, 42, 66);

        var rotateAroundZero = Matrix4x4D.CreateRotationY(radians, Vector3D.Zero);
        var rotateAroundZeroExpected = Matrix4x4D.CreateRotationY(radians);
        await Assert.That(rotateAroundZero).IsEqualTo(rotateAroundZeroExpected);

        var rotateAroundCenter = Matrix4x4D.CreateRotationY(radians, center);
        var rotateAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateRotationY(radians) * Matrix4x4D.CreateTranslation(center);
        await Assert.That(rotateAroundCenter).IsEqualTo(rotateAroundCenterExpected).Within(MathHelper.HighQualityTolerance);
    }

    // A test for CreateFromAxisAngle(Vector3,double)
    [Test]
    public async Task Matrix4x4DCreateFromAxisAngleTest()
    {
        var radians = MathHelper.ToRadians(-30.0);

        var expected = Matrix4x4D.CreateRotationX(radians);
        var actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitX, radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        expected = Matrix4x4D.CreateRotationY(radians);
        actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitY, radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        expected = Matrix4x4D.CreateRotationZ(radians);
        actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitZ, radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        expected = Matrix4x4D.CreateFromQuaternion(QuaternionD.CreateFromAxisAngle(Vector3D.Normalize(Vector3D.One), radians));
        actual = Matrix4x4D.CreateFromAxisAngle(Vector3D.Normalize(Vector3D.One), radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        const int rotCount = 16;
        for (var i = 0; i < rotCount; ++i)
        {
            var latitude = 2.0 * Math.PI * ((double)i / rotCount);
            for (var j = 0; j < rotCount; ++j)
            {
                var longitude = -(Math.PI * 0.5) + Math.PI * ((double)j / rotCount);

                var m = Matrix4x4D.CreateRotationZ(longitude) * Matrix4x4D.CreateRotationY(latitude);
                var axis = new Vector3D(m.M11, m.M12, m.M13);
                for (var k = 0; k < rotCount; ++k)
                {
                    var rot = (2.0 * Math.PI) * ((double)k / rotCount);
                    expected = Matrix4x4D.CreateFromQuaternion(QuaternionD.CreateFromAxisAngle(axis, rot));
                    actual = Matrix4x4D.CreateFromAxisAngle(axis, rot);
                    await Assert.That(actual).IsEqualTo(expected);
                }
            }
        }
    }

    [Test]
    public async Task Matrix4x4DCreateFromYawPitchRollTest1()
    {
        var yawAngle = MathHelper.ToRadians(30.0);
        var pitchAngle = MathHelper.ToRadians(40.0);
        var rollAngle = MathHelper.ToRadians(50.0);

        var yaw = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitY, yawAngle);
        var pitch = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitX, pitchAngle);
        var roll = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitZ, rollAngle);

        var expected = roll * pitch * yaw;
        var actual = Matrix4x4D.CreateFromYawPitchRoll(yawAngle, pitchAngle, rollAngle);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);
    }

    // Covers more numeric regions
    [Test]
    public async Task Matrix4x4DCreateFromYawPitchRollTest2()
    {
        const double step = 35.0;

        for (var yawAngle = -720.0; yawAngle <= 720.0; yawAngle += step)
        {
            for (var pitchAngle = -720.0; pitchAngle <= 720.0; pitchAngle += step)
            {
                for (var rollAngle = -720.0; rollAngle <= 720.0; rollAngle += step)
                {
                    var yawRad = MathHelper.ToRadians(yawAngle);
                    var pitchRad = MathHelper.ToRadians(pitchAngle);
                    var rollRad = MathHelper.ToRadians(rollAngle);
                    var yaw = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitY, yawRad);
                    var pitch = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitX, pitchRad);
                    var roll = Matrix4x4D.CreateFromAxisAngle(Vector3D.UnitZ, rollRad);

                    var expected = roll * pitch * yaw;
                    var actual = Matrix4x4D.CreateFromYawPitchRoll(yawRad, pitchRad, rollRad);
                    await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);
                }
            }
        }
    }

    // Simple shadow test.
    [Test]
    public async Task Matrix4x4DCreateShadowTest01()
    {
        var lightDir = Vector3D.UnitY;
        var plane = new PlaneD(Vector3D.UnitY, 0);

        var expected = Matrix4x4D.CreateScale(1, 0, 1);

        var actual = Matrix4x4D.CreateShadow(lightDir, plane);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // Various plane projections.
    [Test]
    public async Task Matrix4x4DCreateShadowTest02()
    {
        // Complex cases.
        PlaneD[] planes =
        [
            new(0, 1, 0, 0),
            new(1, 2, 3, 4),
            new(5, 6, 7, 8),
            new(-1, -2, -3, -4),
            new(-5, -6, -7, -8),
        ];

        Vector3D[] points =
        [
            new(1, 2, 3),
            new(5, 6, 7),
            new(8, 9, 10),
            new(-1, -2, -3),
            new(-5, -6, -7),
            new(-8, -9, -10),
        ];

        foreach (var p in planes)
        {
            var plane = PlaneD.Normalize(p);

            // Try various direction of light directions.
            var testDirections = new[]
            {
                new Vector3D(-1.0, 1.0, 1.0),
                new Vector3D(0.0, 1.0, 1.0),
                new Vector3D(1.0, 1.0, 1.0),
                new Vector3D(-1.0, 0.0, 1.0),
                new Vector3D(0.0, 0.0, 1.0),
                new Vector3D(1.0, 0.0, 1.0),
                new Vector3D(-1.0, -1.0, 1.0),
                new Vector3D(0.0, -1.0, 1.0),
                new Vector3D(1.0, -1.0, 1.0),

                new Vector3D(-1.0, 1.0, 0.0),
                new Vector3D(0.0, 1.0, 0.0),
                new Vector3D(1.0, 1.0, 0.0),
                new Vector3D(-1.0, 0.0, 0.0),
                new Vector3D(0.0, 0.0, 0.0),
                new Vector3D(1.0, 0.0, 0.0),
                new Vector3D(-1.0, -1.0, 0.0),
                new Vector3D(0.0, -1.0, 0.0),
                new Vector3D(1.0, -1.0, 0.0),

                new Vector3D(-1.0, 1.0, -1.0),
                new Vector3D(0.0, 1.0, -1.0),
                new Vector3D(1.0, 1.0, -1.0),
                new Vector3D(-1.0, 0.0, -1.0),
                new Vector3D(0.0, 0.0, -1.0),
                new Vector3D(1.0, 0.0, -1.0),
                new Vector3D(-1.0, -1.0, -1.0),
                new Vector3D(0.0, -1.0, -1.0),
                new Vector3D(1.0, -1.0, -1.0),
            };

            foreach (var lightDirInfo in testDirections)
            {
                if (lightDirInfo.Length() < 0.1)
                    continue;
                var lightDir = Vector3D.Normalize(lightDirInfo);

                if (PlaneD.DotNormal(plane, lightDir) < 0.1)
                    continue;

                var m = Matrix4x4D.CreateShadow(lightDir, plane);
                var pp = -plane.D * plane.Normal; // origin of the plane.

                //
                foreach (var point in points)
                {
                    var v4 = Vector4D.Transform(point, m);

                    var sp = new Vector3D(v4.X, v4.Y, v4.Z) / v4.W;

                    // Make sure transformed position is on the plane.
                    var v = sp - pp;
                    var d = Vector3D.Dot(v, plane.Normal);
                    await Assert.That(d).IsEqualTo(0).Within(MathHelper.HighQualityTolerance);

                    // make sure direction between transformed position and original position are same as light direction.
                    if (Vector3D.Dot(point - pp, plane.Normal) > 0.0001)
                    {
                        var dir = Vector3D.Normalize(point - sp);
                        await Assert.That(dir).IsEqualTo(lightDir).Within(MathHelper.HighQualityTolerance);
                    }
                }
            }
        }
    }

    static async Task CreateReflectionTest(PlaneD plane, Matrix4x4D expected)
    {
        var actual = Matrix4x4D.CreateReflection(plane);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Matrix4x4DCreateReflectionTest01()
    {
        // XY plane.
        await CreateReflectionTest(new(Vector3D.UnitZ, 0), Matrix4x4D.CreateScale(1, 1, -1));
        // XZ plane.
        await CreateReflectionTest(new(Vector3D.UnitY, 0), Matrix4x4D.CreateScale(1, -1, 1));
        // YZ plane.
        await CreateReflectionTest(new(Vector3D.UnitX, 0), Matrix4x4D.CreateScale(-1, 1, 1));

        // Complex cases.
        PlaneD[] planes =
        [
            new(0, 1, 0, 0),
            new(1, 2, 3, 4),
            new(5, 6, 7, 8),
            new(-1, -2, -3, -4),
            new(-5, -6, -7, -8),
        ];

        Vector3D[] points =
        [
            new(1, 2, 3),
            new(5, 6, 7),
            new(-1, -2, -3),
            new(-5, -6, -7),
        ];

        foreach (var p in planes)
        {
            var plane = PlaneD.Normalize(p);
            var m = Matrix4x4D.CreateReflection(plane);
            var pp = -plane.D * plane.Normal; // Position on the plane.

            //
            foreach (var point in points)
            {
                var rp = Vector3D.Transform(point, m);

                // Manually compute reflection point and compare results.
                var v = point - pp;
                var d = Vector3D.Dot(v, plane.Normal);
                var vp = point - 2.0 * d * plane.Normal;
                await Assert.That(rp).IsEqualTo(vp).Within(MathHelper.HighQualityTolerance);
            }
        }
    }

    [Test]
    public async Task Matrix4x4DCreateReflectionTest02()
    {
        var plane = new PlaneD(0, 1, 0, 60);
        var actual = Matrix4x4D.CreateReflection(plane);

        await Assert.That(actual.M11).IsEqualTo(1.0);
        await Assert.That(actual.M12).IsEqualTo(0.0);
        await Assert.That(actual.M13).IsEqualTo(0.0);
        await Assert.That(actual.M14).IsEqualTo(0.0);

        await Assert.That(actual.M21).IsEqualTo(0.0);
        await Assert.That(actual.M22).IsEqualTo(-1.0);
        await Assert.That(actual.M23).IsEqualTo(0.0);
        await Assert.That(actual.M24).IsEqualTo(0.0);

        await Assert.That(actual.M31).IsEqualTo(0.0);
        await Assert.That(actual.M32).IsEqualTo(0.0);
        await Assert.That(actual.M33).IsEqualTo(1.0);
        await Assert.That(actual.M34).IsEqualTo(0.0);

        await Assert.That(actual.M41).IsEqualTo(0.0);
        await Assert.That(actual.M42).IsEqualTo(-120.0);
        await Assert.That(actual.M43).IsEqualTo(0.0);
        await Assert.That(actual.M44).IsEqualTo(1.0);
    }

    // A test for CreateRotationZ (double)
    [Test]
    public async Task Matrix4x4DCreateRotationZTest()
    {
        var radians = MathHelper.ToRadians(50.0);

        var expected = new Matrix4x4D
        {
            M11 = 0.642787635,
            M12 = 0.766044438,
            M21 = -0.766044438,
            M22 = 0.642787635,
            M33 = 1.0,
            M44 = 1.0,
        };

        var actual = Matrix4x4D.CreateRotationZ(radians);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    // A test for CreateRotationZ (double, Vector3)
    [Test]
    public async Task Matrix4x4DCreateRotationZCenterTest()
    {
        var radians = MathHelper.ToRadians(30.0);
        var center = new Vector3D(23, 42, 66);

        var rotateAroundZero = Matrix4x4D.CreateRotationZ(radians, Vector3D.Zero);
        var rotateAroundZeroExpected = Matrix4x4D.CreateRotationZ(radians);
        await Assert.That(rotateAroundZero).IsEqualTo(rotateAroundZeroExpected);

        var rotateAroundCenter = Matrix4x4D.CreateRotationZ(radians, center);
        var rotateAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateRotationZ(radians) * Matrix4x4D.CreateTranslation(center);
        await Assert.That(rotateAroundCenter).IsEqualTo(rotateAroundCenterExpected).Within(MathHelper.HighQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateLookAtTest()
    {
        var cameraPosition = new Vector3D(10.0, 20.0, 30.0);
        var cameraTarget = new Vector3D(3.0, 2.0, -4.0);
        var cameraUpVector = new Vector3D(0.0, 1.0, 0.0);

        var expected = new Matrix4x4D
        {
            M11 = +0.979457,
            M12 = -0.0928268,
            M13 = +0.179017,
            M21 = +0.0,
            M22 = +0.887748,
            M23 = +0.460329,
            M31 = -0.201653,
            M32 = -0.450873,
            M33 = +0.869511,
            M41 = -3.74498,
            M42 = -3.30051,
            M43 = -37.0821,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateLookAt(cameraPosition, cameraTarget, cameraUpVector);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateLookAtLeftHandedTest()
    {
        var cameraPosition = new Vector3D(10.0, 20.0, 30.0);
        var cameraTarget = new Vector3D(3.0, 2.0, -4.0);
        var cameraUpVector = new Vector3D(0.0, 1.0, 0.0);

        var expected = new Matrix4x4D
        {
            M11 = -0.979457,
            M12 = -0.0928268,
            M13 = -0.179017,
            M21 = +0.0,
            M22 = +0.887748,
            M23 = -0.460329,
            M31 = +0.201653,
            M32 = -0.450873,
            M33 = -0.869511,
            M41 = +3.74498,
            M42 = -3.30051,
            M43 = +37.0821,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateLookAtLeftHanded(cameraPosition, cameraTarget, cameraUpVector);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateLookToTest()
    {
        var cameraPosition = new Vector3D(10.0, 20.0, 30.0);
        var cameraDirection = new Vector3D(-7.0, -18.0, -34.0);
        var cameraUpVector = new Vector3D(0.0, 1.0, 0.0);

        var expected = new Matrix4x4D
        {
            M11 = +0.979457,
            M12 = -0.0928268,
            M13 = +0.179017,
            M21 = +0.0,
            M22 = +0.887748,
            M23 = +0.460329,
            M31 = -0.201653,
            M32 = -0.450873,
            M33 = +0.869511,
            M41 = -3.74498,
            M42 = -3.30051,
            M43 = -37.0821,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateLookTo(cameraPosition, cameraDirection, cameraUpVector);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateLookToLeftHandedTest()
    {
        var cameraPosition = new Vector3D(10.0, 20.0, 30.0);
        var cameraDirection = new Vector3D(-7.0, -18.0, -34.0);
        var cameraUpVector = new Vector3D(0.0, 1.0, 0.0);

        var expected = new Matrix4x4D
        {
            M11 = -0.979457,
            M12 = -0.0928268,
            M13 = -0.179017,
            M21 = +0.0,
            M22 = +0.887748,
            M23 = -0.460329,
            M31 = +0.201653,
            M32 = -0.450873,
            M33 = -0.869511,
            M41 = +3.74498,
            M42 = -3.30051,
            M43 = +37.0821,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateLookToLeftHanded(cameraPosition, cameraDirection, cameraUpVector);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateViewportTest()
    {
        var x = 10.0;
        var y = 20.0;
        var width = 80.0;
        var height = 160.0;
        var minDepth = 1.5;
        var maxDepth = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +40.0,
            M22 = -80.0,
            M33 = -998.5,
            M41 = +50.0,
            M42 = +100.0,
            M43 = +1.5,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateViewport(x, y, width, height, minDepth, maxDepth);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Matrix4x4DCreateViewportLeftHandedTest()
    {
        double x = 10.0, y = 20.0;
        double width = 3.0, height = 4.0;
        double minDepth = 100.0, maxDepth = 200.0;

        var expected = Matrix4x4D.Identity;
        expected.M11 = width * 0.5;
        expected.M22 = -height * 0.5;
        expected.M33 = maxDepth - minDepth;
        expected.M41 = x + expected.M11;
        expected.M42 = y - expected.M22;
        expected.M43 = minDepth;

        var actual = Matrix4x4D.CreateViewportLeftHanded(x, y, width, height, minDepth, maxDepth);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateWorld (Vector3, Vector3, Vector3)
    [Test]
    public async Task Matrix4x4DCreateWorldTest()
    {
        var objectPosition = new Vector3D(10.0, 20.0, 30.0);
        var objectForwardDirection = new Vector3D(3.0, 2.0, -4.0);
        var objectUpVector = new Vector3D(0.0, 1.0, 0.0);

        var expected = new Matrix4x4D
        {
            M11 = 0.799999952,
            M12 = 0,
            M13 = 0.599999964,
            M14 = 0,
            M21 = -0.2228344,
            M22 = 0.928476632,
            M23 = 0.297112525,
            M24 = 0,
            M31 = -0.557086,
            M32 = -0.371390671,
            M33 = 0.742781341,
            M34 = 0,
            M41 = 10,
            M42 = 20,
            M43 = 30,
            M44 = 1.0,
        };

        var actual = Matrix4x4D.CreateWorld(objectPosition, objectForwardDirection, objectUpVector);
        await Assert.That(actual).IsEqualTo(expected).Within(1e-07);

        await Assert.That(actual.Translation).IsEqualTo(objectPosition);
        await Assert.That(Vector3D.Dot(Vector3D.Normalize(objectUpVector), new(actual.M21, actual.M22, actual.M23)) > 0).IsTrue();
        await Assert.That(Vector3D.Dot(Vector3D.Normalize(objectForwardDirection), new(-actual.M31, -actual.M32, -actual.M33)) > 0.999).IsTrue();
    }

    [Test]
    public async Task Matrix4x4DCreateOrthoTest()
    {
        var width = 100.0;
        var height = 200.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.02,
            M22 = +0.01,
            M33 = -0.0010015,
            M43 = -0.00150225,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateOrthographic(width, height, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateOrthoLeftHandedTest()
    {
        var width = 100.0;
        var height = 200.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.02,
            M22 = +0.01,
            M33 = +0.0010015,
            M43 = -0.00150225,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateOrthographicLeftHanded(width, height, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateOrthoOffCenterTest()
    {
        var left = 10.0;
        var right = 90.0;
        var bottom = 20.0;
        var top = 180.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.025,
            M22 = +0.0125,
            M33 = -0.0010015,
            M41 = -1.25,
            M42 = -1.25,
            M43 = -0.00150225,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateOrthographicOffCenter(left, right, bottom, top, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreateOrthoOffCenterLeftHandedTest()
    {
        var left = 10.0;
        var right = 90.0;
        var bottom = 20.0;
        var top = 180.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.025,
            M22 = +0.0125,
            M33 = +0.0010015,
            M41 = -1.25,
            M42 = -1.25,
            M43 = -0.00150225,
            M44 = +1.0,
        };

        var actual = Matrix4x4D.CreateOrthographicOffCenterLeftHanded(left, right, bottom, top, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreatePerspectiveTest()
    {
        var width = 100.0;
        var height = 200.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.03,
            M22 = +0.015,
            M33 = -1.0015,
            M34 = -1.0,
            M43 = -1.50225,
        };

        var actual = Matrix4x4D.CreatePerspective(width, height, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreatePerspectiveLeftHandedTest()
    {
        var width = 100.0;
        var height = 200.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.03,
            M22 = +0.015,
            M33 = +1.0015,
            M34 = +1.0,
            M43 = -1.50225,
        };

        var actual = Matrix4x4D.CreatePerspectiveLeftHanded(width, height, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    // A test for CreatePerspective (double, double, double, double)
    // CreatePerspective test where z-near = z-far
    [Test]
    public async Task Matrix4x4DCreatePerspectiveTest1()
    {
        await Assert.That(() =>
        {
            var width = 100.0;
            var height = 200.0;
            var zNearPlane = 0.0;
            var zFarPlane = 0.0;

            _ = Matrix4x4D.CreatePerspective(width, height, zNearPlane, zFarPlane);
        }).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspective (double, double, double, double)
    // CreatePerspective test where near plane is negative value
    [Test]
    public async Task Matrix4x4DCreatePerspectiveTest2()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspective(10, 10, -10, 10)).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspective (double, double, double, double)
    // CreatePerspective test where far plane is negative value
    [Test]
    public async Task Matrix4x4DCreatePerspectiveTest3()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspective(10, 10, 10, -10)).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspective (double, double, double, double)
    // CreatePerspective test where near plane is beyond far plane
    [Test]
    public async Task Matrix4x4DCreatePerspectiveTest4()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspective(10, 10, 10, 1)).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Matrix4x4DCreatePerspectiveFieldOfViewTest()
    {
        var fieldOfView = MathHelper.ToRadians(30.0);
        var aspectRatio = 1280.0 / 720.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +2.09928,
            M22 = +3.73205,
            M33 = -1.0015,
            M34 = -1.0,
            M43 = -1.50225,
        };

        var actual = Matrix4x4D.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreatePerspectiveFieldOfViewLeftHandedTest()
    {
        var fieldOfView = MathHelper.ToRadians(30.0);
        var aspectRatio = 1280.0 / 720.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +2.09928,
            M22 = +3.73205,
            M33 = +1.0015,
            M34 = +1.0,
            M43 = -1.50225,
        };

        var actual = Matrix4x4D.CreatePerspectiveFieldOfViewLeftHanded(fieldOfView, aspectRatio, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    // A test for CreatePerspectiveFieldOfView (double, double, double, double)
    // CreatePerspectiveFieldOfView test where filedOfView is negative value.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveFieldOfViewTest1()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspectiveFieldOfView(-1, 1, 1, 10)).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspectiveFieldOfView (double, double, double, double)
    // CreatePerspectiveFieldOfView test where filedOfView is more than pi.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveFieldOfViewTest2()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspectiveFieldOfView(Math.PI + 0.01, 1, 1, 10)).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspectiveFieldOfView (double, double, double, double)
    // CreatePerspectiveFieldOfView test where nearPlaneDistance is negative value.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveFieldOfViewTest3()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspectiveFieldOfView(Math.PI * 0.25, 1, -1, 10)).Throws<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspectiveFieldOfView (double, double, double, double)
    // CreatePerspectiveFieldOfView test where farPlaneDistance is negative value.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveFieldOfViewTest4()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspectiveFieldOfView(Math.PI * 0.25, 1, 1, -10)).Throws<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspectiveFieldOfView (double, double, double, double)
    // CreatePerspectiveFieldOfView test where nearPlaneDistance is larger than farPlaneDistance.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveFieldOfViewTest5()
    {
        await Assert.That(() => Matrix4x4D.CreatePerspectiveFieldOfView(Math.PI * 0.25, 1, 10, 1)).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task Matrix4x4DCreatePerspectiveOffCenterTest()
    {
        var left = 10.0;
        var right = 90.0;
        var bottom = 20.0;
        var top = 180.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.0375,
            M22 = +0.01875,
            M31 = +1.25,
            M32 = +1.25,
            M33 = -1.0015,
            M34 = -1.0,
            M43 = -1.50225,
        };

        var actual = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    [Test]
    public async Task Matrix4x4DCreatePerspectiveOffCenterLeftHandedTest()
    {
        var left = 10.0;
        var right = 90.0;
        var bottom = 20.0;
        var top = 180.0;
        var zNearPlane = 1.5;
        var zFarPlane = 1000.0;

        var expected = new Matrix4x4D
        {
            M11 = +0.0375,
            M22 = +0.01875,
            M31 = -1.25,
            M32 = -1.25,
            M33 = +1.0015,
            M34 = +1.0,
            M43 = -1.50225,
        };


        var actual = Matrix4x4D.CreatePerspectiveOffCenterLeftHanded(left, right, bottom, top, zNearPlane, zFarPlane);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    // A test for CreatePerspectiveOffCenter (double, double, double, double, double, double)
    // CreatePerspectiveOffCenter test where nearPlaneDistance is negative.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveOffCenterTest1()
    {
        await Assert.That(() =>
        {
            double left = 10.0, right = 90.0, bottom = 20.0, top = 180.0;
            _ = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, -1, 10);
        }).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspectiveOffCenter (double, double, double, double, double, double)
    // CreatePerspectiveOffCenter test where farPlaneDistance is negative.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveOffCenterTest2()
    {
        await Assert.That(() =>
        {
            double left = 10.0, right = 90.0, bottom = 20.0, top = 180.0;
            _ = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, 1, -10);
        }).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for CreatePerspectiveOffCenter (double, double, double, double, double, double)
    // CreatePerspectiveOffCenter test where nearPlaneDistance is larger than farPlaneDistance.
    [Test]
    public async Task Matrix4x4DCreatePerspectiveOffCenterTest3()
    {
        await Assert.That(() =>
        {
            double left = 10.0, right = 90.0, bottom = 20.0, top = 180.0;
            _ = Matrix4x4D.CreatePerspectiveOffCenter(left, right, bottom, top, 10, 1);
        }).ThrowsExactly<ArgumentOutOfRangeException>();
    }

    // A test for Invert (Matrix4x4D)
    // Non-invertible matrix - determinant is zero - singular matrix
    [Test]
    public async Task Matrix4x4DInvertTest1()
    {
        var a = new Matrix4x4D
        {
            M11 = 1.0,
            M12 = 2.0,
            M13 = 3.0,
            M14 = 4.0,
            M21 = 5.0,
            M22 = 6.0,
            M23 = 7.0,
            M24 = 8.0,
            M31 = 9.0,
            M32 = 10.0,
            M33 = 11.0,
            M34 = 12.0,
            M41 = 13.0,
            M42 = 14.0,
            M43 = 15.0,
            M44 = 16.0,
        };

        var detA = a.GetDeterminant();
        await Assert.That(detA).IsEqualTo(0.0);

        await Assert.That(Matrix4x4D.Invert(a, out var actual)).IsFalse();

        // all the elements in Actual is NaN
        await Assert.That(actual)
            .Member(static actual => actual.M11, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M12, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M13, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M14, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M21, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M22, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M23, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M24, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M31, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M32, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M33, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M34, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M41, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M42, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M43, static actual => actual.IsNaN()).And
            .Member(static actual => actual.M44, static actual => actual.IsNaN());
    }

    // A test for Lerp (Matrix4x4D, Matrix4x4D, double)
    [Test]
    public async Task Matrix4x4DLerpTest()
    {
        var a = new Matrix4x4D
        {
            M11 = 11.0,
            M12 = 12.0,
            M13 = 13.0,
            M14 = 14.0,
            M21 = 21.0,
            M22 = 22.0,
            M23 = 23.0,
            M24 = 24.0,
            M31 = 31.0,
            M32 = 32.0,
            M33 = 33.0,
            M34 = 34.0,
            M41 = 41.0,
            M42 = 42.0,
            M43 = 43.0,
            M44 = 44.0,
        };

        var b = GenerateIncrementalMatrixNumber();

        var t = 0.5;

        var expected = new Matrix4x4D
        {
            M11 = a.M11 + (b.M11 - a.M11) * t,
            M12 = a.M12 + (b.M12 - a.M12) * t,
            M13 = a.M13 + (b.M13 - a.M13) * t,
            M14 = a.M14 + (b.M14 - a.M14) * t,
            M21 = a.M21 + (b.M21 - a.M21) * t,
            M22 = a.M22 + (b.M22 - a.M22) * t,
            M23 = a.M23 + (b.M23 - a.M23) * t,
            M24 = a.M24 + (b.M24 - a.M24) * t,
            M31 = a.M31 + (b.M31 - a.M31) * t,
            M32 = a.M32 + (b.M32 - a.M32) * t,
            M33 = a.M33 + (b.M33 - a.M33) * t,
            M34 = a.M34 + (b.M34 - a.M34) * t,
            M41 = a.M41 + (b.M41 - a.M41) * t,
            M42 = a.M42 + (b.M42 - a.M42) * t,
            M43 = a.M43 + (b.M43 - a.M43) * t,
            M44 = a.M44 + (b.M44 - a.M44) * t,
        };

        var actual = Matrix4x4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator - (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DUnaryNegationTest()
    {
        var a = GenerateIncrementalMatrixNumber();

        var expected = new Matrix4x4D
        {
            M11 = -1.0,
            M12 = -2.0,
            M13 = -3.0,
            M14 = -4.0,
            M21 = -5.0,
            M22 = -6.0,
            M23 = -7.0,
            M24 = -8.0,
            M31 = -9.0,
            M32 = -10.0,
            M33 = -11.0,
            M34 = -12.0,
            M41 = -13.0,
            M42 = -14.0,
            M43 = -15.0,
            M44 = -16.0,
        };

        var actual = -a;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator - (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DSubtractionTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-8.0);

        var expected = new Matrix4x4D
        {
            M11 = a.M11 - b.M11,
            M12 = a.M12 - b.M12,
            M13 = a.M13 - b.M13,
            M14 = a.M14 - b.M14,
            M21 = a.M21 - b.M21,
            M22 = a.M22 - b.M22,
            M23 = a.M23 - b.M23,
            M24 = a.M24 - b.M24,
            M31 = a.M31 - b.M31,
            M32 = a.M32 - b.M32,
            M33 = a.M33 - b.M33,
            M34 = a.M34 - b.M34,
            M41 = a.M41 - b.M41,
            M42 = a.M42 - b.M42,
            M43 = a.M43 - b.M43,
            M44 = a.M44 - b.M44,
        };

        var actual = a - b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DMultiplyTest1()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-8.0);

        var expected = new Matrix4x4D
        {
            M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
            M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
            M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
            M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,
            M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
            M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
            M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
            M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,
            M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
            M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
            M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
            M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,
            M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
            M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
            M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
            M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44,
        };

        var actual = a * b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Matrix4x4D, Matrix4x4D)
    // Multiply with identity matrix
    [Test]
    public async Task Matrix4x4DMultiplyTest4()
    {
        var a = new Matrix4x4D
        {
            M11 = 1.0,
            M12 = 2.0,
            M13 = 3.0,
            M14 = 4.0,
            M21 = 5.0,
            M22 = -6.0,
            M23 = 7.0,
            M24 = -8.0,
            M31 = 9.0,
            M32 = 10.0,
            M33 = 11.0,
            M34 = 12.0,
            M41 = 13.0,
            M42 = -14.0,
            M43 = 15.0,
            M44 = -16.0,
        };

        var b = Matrix4x4D.Identity;

        var expected = a;
        var actual = a * b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator + (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DAdditionTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-8.0);

        var expected = new Matrix4x4D
        {
            M11 = a.M11 + b.M11,
            M12 = a.M12 + b.M12,
            M13 = a.M13 + b.M13,
            M14 = a.M14 + b.M14,
            M21 = a.M21 + b.M21,
            M22 = a.M22 + b.M22,
            M23 = a.M23 + b.M23,
            M24 = a.M24 + b.M24,
            M31 = a.M31 + b.M31,
            M32 = a.M32 + b.M32,
            M33 = a.M33 + b.M33,
            M34 = a.M34 + b.M34,
            M41 = a.M41 + b.M41,
            M42 = a.M42 + b.M42,
            M43 = a.M43 + b.M43,
            M44 = a.M44 + b.M44,
        };

        var actual = a + b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transpose (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DTransposeTest()
    {
        var a = GenerateIncrementalMatrixNumber();

        var expected = new Matrix4x4D
        {
            M11 = a.M11,
            M12 = a.M21,
            M13 = a.M31,
            M14 = a.M41,
            M21 = a.M12,
            M22 = a.M22,
            M23 = a.M32,
            M24 = a.M42,
            M31 = a.M13,
            M32 = a.M23,
            M33 = a.M33,
            M34 = a.M43,
            M41 = a.M14,
            M42 = a.M24,
            M43 = a.M34,
            M44 = a.M44,
        };

        var actual = Matrix4x4D.Transpose(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transpose (Matrix4x4D)
    // Transpose Identity matrix
    [Test]
    public async Task Matrix4x4DTransposeTest1()
    {
        var a = Matrix4x4D.Identity;
        var expected = Matrix4x4D.Identity;

        var actual = Matrix4x4D.Transpose(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Matrix4x4D (QuaternionD)
    [Test]
    public async Task Matrix4x4DFromQuaternionTest1()
    {
        var axis = Vector3D.Normalize(new(1.0, 2.0, 3.0));
        var q = QuaternionD.CreateFromAxisAngle(axis, MathHelper.ToRadians(30.0));

        var expected = new Matrix4x4D
        {
            M11 = 0.875595033,
            M12 = 0.420031041,
            M13 = -0.2385524,
            M14 = 0.0,
            M21 = -0.38175258,
            M22 = 0.904303849,
            M23 = 0.1910483,
            M24 = 0.0,
            M31 = 0.295970082,
            M32 = -0.07621294,
            M33 = 0.952151954,
            M34 = 0.0,
            M41 = 0.0,
            M42 = 0.0,
            M43 = 0.0,
            M44 = 1.0,
        };

        var target = Matrix4x4D.CreateFromQuaternion(q);
        await Assert.That(target).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    // A test for FromQuaternion (Matrix4x4D)
    // Convert X axis rotation matrix
    [Test]
    public async Task Matrix4x4DFromQuaternionTest2()
    {
        for (var angle = 0.0; angle < 720.0; angle += 10.0)
        {
            var quat = QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);

            var expected = Matrix4x4D.CreateRotationX(angle);
            var actual = Matrix4x4D.CreateFromQuaternion(quat);
            await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

            // make sure convert back to quaternionD is same as we passed quaternionD.
            var q2 = QuaternionD.CreateFromRotationMatrix(actual);
            await Assert.That(q2).IsEqualTo(quat).Within(MathHelper.HighQualityTolerance).Or.IsEqualTo(-quat).Within(MathHelper.HighQualityTolerance);
        }
    }

    // A test for FromQuaternion (Matrix4x4D)
    // Convert Y axis rotation matrix
    [Test]
    public async Task Matrix4x4DFromQuaternionTest3()
    {
        for (var angle = 0.0; angle < 720.0; angle += 10.0)
        {
            var quat = QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle);

            var expected = Matrix4x4D.CreateRotationY(angle);
            var actual = Matrix4x4D.CreateFromQuaternion(quat);
            await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

            // make sure convert back to quaternionD is same as we passed quaternionD.
            var q2 = QuaternionD.CreateFromRotationMatrix(actual);
            await Assert.That(q2).IsEqualTo(quat).Within(MathHelper.HighQualityTolerance).Or.IsEqualTo(-quat).Within(MathHelper.HighQualityTolerance);
        }
    }

    // A test for FromQuaternion (Matrix4x4D)
    // Convert Z axis rotation matrix
    [Test]
    public async Task Matrix4x4DFromQuaternionTest4()
    {
        for (var angle = 0.0; angle < 720.0; angle += 10.0)
        {
            var quat = QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle);

            var expected = Matrix4x4D.CreateRotationZ(angle);
            var actual = Matrix4x4D.CreateFromQuaternion(quat);
            await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

            // make sure convert back to quaternionD is same as we passed quaternionD.
            var q2 = QuaternionD.CreateFromRotationMatrix(actual);
            await Assert.That(q2).IsEqualTo(quat).Within(MathHelper.HighQualityTolerance).Or.IsEqualTo(-quat).Within(MathHelper.HighQualityTolerance);
        }
    }

    // A test for FromQuaternion (Matrix4x4D)
    // Convert XYZ axis rotation matrix
    [Test]
    public async Task Matrix4x4DFromQuaternionTest5()
    {
        for (var angle = 0.0; angle < 720.0; angle += 10.0)
        {
            var quat =
                QuaternionD.CreateFromAxisAngle(Vector3D.UnitZ, angle) *
                QuaternionD.CreateFromAxisAngle(Vector3D.UnitY, angle) *
                QuaternionD.CreateFromAxisAngle(Vector3D.UnitX, angle);

            var expected =
                Matrix4x4D.CreateRotationX(angle) *
                Matrix4x4D.CreateRotationY(angle) *
                Matrix4x4D.CreateRotationZ(angle);
            var actual = Matrix4x4D.CreateFromQuaternion(quat);
            await Assert.That(actual).IsEqualTo(expected).Within(1e-12);

            // make sure convert back to quaternionD is same as we passed quaternionD.
            var q2 = QuaternionD.CreateFromRotationMatrix(actual);
            await Assert.That(q2).IsEqualTo(quat).Within(MathHelper.HighQualityTolerance).Or.IsEqualTo(-quat).Within(MathHelper.HighQualityTolerance);
        }
    }

    // A test for ToString ()
    [Test]
    public async Task Matrix4x4DToStringTest()
    {
        var a = new Matrix4x4D
        {
            M11 = 11.0,
            M12 = -12.0,
            M13 = -13.3,
            M14 = 14.4,
            M21 = 21.0,
            M22 = 22.0,
            M23 = 23.0,
            M24 = 24.0,
            M31 = 31.0,
            M32 = 32.0,
            M33 = 33.0,
            M34 = 34.0,
            M41 = 41.0,
            M42 = 42.0,
            M43 = 43.0,
            M44 = 44.0,
        };

        var expected = string.Format(CultureInfo.CurrentCulture,
            "{{ {{M11:{0} M12:{1} M13:{2} M14:{3}}} {{M21:{4} M22:{5} M23:{6} M24:{7}}} {{M31:{8} M32:{9} M33:{10} M34:{11}}} {{M41:{12} M42:{13} M43:{14} M44:{15}}} }}",
            11.0, -12.0, -13.3, 14.4,
            21.0, 22.0, 23.0, 24.0,
            31.0, 32.0, 33.0, 34.0,
            41.0, 42.0, 43.0, 44.0);

        var actual = a.ToString();
        await Assert.That(actual).IsEquivalentTo(expected);
    }

    // A test for Add (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DAddTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-8.0);

        var expected = new Matrix4x4D
        {
            M11 = a.M11 + b.M11,
            M12 = a.M12 + b.M12,
            M13 = a.M13 + b.M13,
            M14 = a.M14 + b.M14,
            M21 = a.M21 + b.M21,
            M22 = a.M22 + b.M22,
            M23 = a.M23 + b.M23,
            M24 = a.M24 + b.M24,
            M31 = a.M31 + b.M31,
            M32 = a.M32 + b.M32,
            M33 = a.M33 + b.M33,
            M34 = a.M34 + b.M34,
            M41 = a.M41 + b.M41,
            M42 = a.M42 + b.M42,
            M43 = a.M43 + b.M43,
            M44 = a.M44 + b.M44,
        };

        var actual = Matrix4x4D.Add(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Equals (object)
    [Test]
    public async Task Matrix4x4DEqualsTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        object obj = b;

        var expected = true;
        var actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0;
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
    public async Task Matrix4x4DGetHashCodeTest()
    {
        var target = GenerateIncrementalMatrixNumber();

        var expected = HashCode.Combine(
            new Vector4D(target.M11, target.M12, target.M13, target.M14),
            new Vector4D(target.M21, target.M22, target.M23, target.M24),
            new Vector4D(target.M31, target.M32, target.M33, target.M34),
            new Vector4D(target.M41, target.M42, target.M43, target.M44)
        );

        var actual = target.GetHashCode();

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DMultiplyTest3()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-8.0);

        var expected = new Matrix4x4D
        {
            M11 = a.M11 * b.M11 + a.M12 * b.M21 + a.M13 * b.M31 + a.M14 * b.M41,
            M12 = a.M11 * b.M12 + a.M12 * b.M22 + a.M13 * b.M32 + a.M14 * b.M42,
            M13 = a.M11 * b.M13 + a.M12 * b.M23 + a.M13 * b.M33 + a.M14 * b.M43,
            M14 = a.M11 * b.M14 + a.M12 * b.M24 + a.M13 * b.M34 + a.M14 * b.M44,
            M21 = a.M21 * b.M11 + a.M22 * b.M21 + a.M23 * b.M31 + a.M24 * b.M41,
            M22 = a.M21 * b.M12 + a.M22 * b.M22 + a.M23 * b.M32 + a.M24 * b.M42,
            M23 = a.M21 * b.M13 + a.M22 * b.M23 + a.M23 * b.M33 + a.M24 * b.M43,
            M24 = a.M21 * b.M14 + a.M22 * b.M24 + a.M23 * b.M34 + a.M24 * b.M44,
            M31 = a.M31 * b.M11 + a.M32 * b.M21 + a.M33 * b.M31 + a.M34 * b.M41,
            M32 = a.M31 * b.M12 + a.M32 * b.M22 + a.M33 * b.M32 + a.M34 * b.M42,
            M33 = a.M31 * b.M13 + a.M32 * b.M23 + a.M33 * b.M33 + a.M34 * b.M43,
            M34 = a.M31 * b.M14 + a.M32 * b.M24 + a.M33 * b.M34 + a.M34 * b.M44,
            M41 = a.M41 * b.M11 + a.M42 * b.M21 + a.M43 * b.M31 + a.M44 * b.M41,
            M42 = a.M41 * b.M12 + a.M42 * b.M22 + a.M43 * b.M32 + a.M44 * b.M42,
            M43 = a.M41 * b.M13 + a.M42 * b.M23 + a.M43 * b.M33 + a.M44 * b.M43,
            M44 = a.M41 * b.M14 + a.M42 * b.M24 + a.M43 * b.M34 + a.M44 * b.M44,
        };

        var actual = Matrix4x4D.Multiply(a, b);

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Matrix4x4D, double)
    [Test]
    public async Task Matrix4x4DMultiplyTest5()
    {
        var a = GenerateIncrementalMatrixNumber();
        var expected = new Matrix4x4D(3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48);
        var actual = Matrix4x4D.Multiply(a, 3);

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Matrix4x4D, double)
    [Test]
    public async Task Matrix4x4DMultiplyTest6()
    {
        var a = GenerateIncrementalMatrixNumber();
        var expected = new Matrix4x4D(3, 6, 9, 12, 15, 18, 21, 24, 27, 30, 33, 36, 39, 42, 45, 48);
        var actual = a * 3;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Negate (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DNegateTest()
    {
        var m = GenerateIncrementalMatrixNumber();

        var expected = new Matrix4x4D
        {
            M11 = -1.0,
            M12 = -2.0,
            M13 = -3.0,
            M14 = -4.0,
            M21 = -5.0,
            M22 = -6.0,
            M23 = -7.0,
            M24 = -8.0,
            M31 = -9.0,
            M32 = -10.0,
            M33 = -11.0,
            M34 = -12.0,
            M41 = -13.0,
            M42 = -14.0,
            M43 = -15.0,
            M44 = -16.0,
        };

        var actual = Matrix4x4D.Negate(m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator != (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DInequalityTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        var expected = false;
        var actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0;
        expected = true;
        actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator == (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DEqualityTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        var expected = true;
        var actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0;
        expected = false;
        actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Subtract (Matrix4x4D, Matrix4x4D)
    [Test]
    public async Task Matrix4x4DSubtractTest()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber(-8.0);

        var expected = new Matrix4x4D
        {
            M11 = a.M11 - b.M11,
            M12 = a.M12 - b.M12,
            M13 = a.M13 - b.M13,
            M14 = a.M14 - b.M14,
            M21 = a.M21 - b.M21,
            M22 = a.M22 - b.M22,
            M23 = a.M23 - b.M23,
            M24 = a.M24 - b.M24,
            M31 = a.M31 - b.M31,
            M32 = a.M32 - b.M32,
            M33 = a.M33 - b.M33,
            M34 = a.M34 - b.M34,
            M41 = a.M41 - b.M41,
            M42 = a.M42 - b.M42,
            M43 = a.M43 - b.M43,
            M44 = a.M44 - b.M44,
        };

        var actual = Matrix4x4D.Subtract(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    private static async Task CreateBillboardTest(Vector3D placeDirection, Vector3D cameraUpVector, Matrix4x4D expectedRotationRightHanded, Matrix4x4D expectedRotationLeftHanded)
    {
        var cameraPosition = new Vector3D(3.0, 4.0, 5.0);
        var objectPosition = cameraPosition + placeDirection * 10.0;
        var expected = expectedRotationRightHanded * Matrix4x4D.CreateTranslation(objectPosition);
        var actualRH = Matrix4x4D.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, new(0, 0, -1));
        await Assert.That(actualRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        placeDirection = InverseHandedness(placeDirection);
        cameraUpVector = InverseHandedness(cameraUpVector);

        cameraPosition = new(3.0, 4.0, -5.0);
        objectPosition = cameraPosition + placeDirection * 10.0;
        expected = expectedRotationLeftHanded * Matrix4x4D.CreateTranslation(objectPosition);
        var actualLH = Matrix4x4D.CreateBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitZ);
        await Assert.That(actualLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        await Assert.That(actualRH).IsEqualTo(InverseHandedness(actualLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualRH)).IsEqualTo(actualLH).Within(DefaultVarianceMatrix);
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Forward side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest01()
    {
        // Object placed at Forward of camera. result must be same as 180 degrees rotate along y-axis.
        await CreateBillboardTest(
            new(0, 0, -1),
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Backward side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest02()
    {
        // Object placed at Backward of camera. This result must be same as 0 degrees rotate along y-axis.
        await CreateBillboardTest(
            Vector3D.UnitZ,
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0.0)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Right side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest03()
    {
        // Place object at Right side of camera. This result must be same as 90 degrees rotate along y-axis.
        await CreateBillboardTest(
            Vector3D.UnitX,
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90D)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90D)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Left side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest04()
    {
        // Place object at Left side of camera. This result must be same as -90 degrees rotate along y-axis.
        await CreateBillboardTest(
            new(-1, 0, 0),
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90D)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90D)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Up-side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest05()
    {
        // Place object at Up-side of camera. result must be same as 180 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateBillboardTest(
            Vector3D.UnitY,
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180D)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180D)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Down-side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest06()
    {
        // Place object at Down-side of camera. result must be same as 0 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateBillboardTest(
            new(0, -1, 0),
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Right side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest07()
    {
        // Place object at Right side of camera. result must be same as 90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateBillboardTest(
            Vector3D.UnitX,
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Left side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest08()
    {
        // Place object at Left side of camera. result must be same as -90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateBillboardTest(
            new(-1, 0, 0),
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Up-side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest09()
    {
        // Place object at Up-side of camera. result must be same as -90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateBillboardTest(
            Vector3D.UnitY,
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Down-side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest10()
    {
        // Place object at Down-side of camera. result must be same as 90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateBillboardTest(
            new(0, -1, 0),
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Forward side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest11()
    {
        // Place object at Forward side of camera. result must be same as 180 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateBillboardTest(
            new(0, 0, -1),
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Backward side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateBillboardTest12()
    {
        // Place object at Backward side of camera. result must be same as 0 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateBillboardTest(
            Vector3D.UnitZ,
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0)));
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Object and camera positions are too close and doesn't pass cameraForwardVector.
    [Test]
    public async Task Matrix4x4DCreateBillboardTooCloseTest1()
    {
        var objectPosition = new Vector3D(3.0, 4.0, 5.0);
        var cameraPosition = objectPosition;
        var cameraUpVector = Vector3D.UnitY;

        // Doesn't pass camera face direction. CreateBillboard uses new Vector3(0, 0, -1) direction. Result must be same as 180 degrees rotate along y-axis.
        var expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualRH = Matrix4x4D.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitZ);
        await Assert.That(actualRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        objectPosition = new(3.0, 4.0, -5.0);
        cameraPosition = objectPosition;
        cameraUpVector = Vector3D.UnitY;

        expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualLH = Matrix4x4D.CreateBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, new(0, 0, -1));
        await Assert.That(actualLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        await Assert.That(actualRH).IsEqualTo(InverseHandedness(actualLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualRH)).IsEqualTo(actualLH).Within(DefaultVarianceMatrix);
    }

    // A test for CreateBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Object and camera positions are too close and passed cameraForwardVector.
    [Test]
    public async Task Matrix4x4DCreateBillboardTooCloseTest2()
    {
        var objectPosition = new Vector3D(3.0, 4.0, 5.0);
        var cameraPosition = objectPosition;
        var cameraUpVector = Vector3D.UnitY;

        // Passes Vector3.Right as camera face direction. Result must be same as -90 degrees rotate along y-axis.
        var expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualRH = Matrix4x4D.CreateBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX);
        await Assert.That(actualRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        objectPosition = new(3.0, 4.0, -5.0);
        cameraPosition = objectPosition;
        cameraUpVector = Vector3D.UnitY;

        expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualLH = Matrix4x4D.CreateBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX);
        await Assert.That(actualLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);
    }

    private async Task CreateConstrainedBillboardTest(Vector3D placeDirection, Vector3D rotateAxis, Matrix4x4D expectedRotationRightHanded, Matrix4x4D expectedRotationLeftHanded)
    {
        var cameraPosition = new Vector3D(3.0, 4.0, 5.0);
        var objectPosition = cameraPosition + placeDirection * 10.0;
        var expected = expectedRotationRightHanded * Matrix4x4D.CreateTranslation(objectPosition);
        var actualRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new(0, 0, -1), new(0, 0, -1));
        await Assert.That(actualRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        // When you move camera along rotateAxis, result must be same.
        cameraPosition += rotateAxis * 10.0;
        var actualTranslatedUpRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new(0, 0, -1), new(0, 0, -1));
        await Assert.That(actualTranslatedUpRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        cameraPosition -= rotateAxis * 30.0;
        var actualTranslatedDownRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, new(0, 0, -1), new(0, 0, -1));
        await Assert.That(actualTranslatedDownRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        placeDirection = InverseHandedness(placeDirection);
        rotateAxis = InverseHandedness(rotateAxis);

        cameraPosition = new(3.0, 4.0, -5.0);
        objectPosition = cameraPosition + placeDirection * 10.0;
        expected = expectedRotationLeftHanded * Matrix4x4D.CreateTranslation(objectPosition);
        var actualLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, new(0, 0, -1), Vector3D.UnitZ);
        await Assert.That(actualLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        // When you move camera along rotateAxis, result must be same.
        cameraPosition += rotateAxis * 10.0;
        var actualTranslatedUpLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, new(0, 0, -1), Vector3D.UnitZ);
        await Assert.That(actualTranslatedUpLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        cameraPosition -= rotateAxis * 30.0;
        var actualTranslatedDownLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, new(0, 0, -1), Vector3D.UnitZ);
        await Assert.That(actualTranslatedDownLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        await Assert.That(actualRH).IsEqualTo(InverseHandedness(actualLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualRH)).IsEqualTo(actualLH).Within(DefaultVarianceMatrix);

        await Assert.That(actualTranslatedUpRH).IsEqualTo(InverseHandedness(actualTranslatedUpLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualTranslatedUpRH)).IsEqualTo(actualTranslatedUpLH).Within(DefaultVarianceMatrix);

        await Assert.That(actualTranslatedDownRH).IsEqualTo(InverseHandedness(actualTranslatedDownLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualTranslatedDownRH)).IsEqualTo(actualTranslatedDownLH).Within(DefaultVarianceMatrix);
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Forward side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest01()
    {
        // Object placed at Forward of camera. result must be same as 180 degrees rotate along y-axis.
        await CreateConstrainedBillboardTest(
            new(0, 0, -1),
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Backward side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest02()
    {
        // Object placed at Backward of camera. This result must be same as 0 degrees rotate along y-axis.
        await CreateConstrainedBillboardTest(
            Vector3D.UnitZ,
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0.0)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(0.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Right side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest03()
    {
        // Place object at Right side of camera. This result must be same as 90 degrees rotate along y-axis.
        await CreateConstrainedBillboardTest(
            Vector3D.UnitX,
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90D)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90D)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Left side of camera on XZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest04()
    {
        // Place object at Left side of camera. This result must be same as -90 degrees rotate along y-axis.
        await CreateConstrainedBillboardTest(
            new(-1, 0, 0),
            Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90D)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90D)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Up-side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest05()
    {
        // Place object at Up-side of camera. result must be same as 180 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateConstrainedBillboardTest(
            Vector3D.UnitY,
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180D)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(180D)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Down-side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest06()
    {
        // Place object at Down-side of camera. result must be same as 0 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateConstrainedBillboardTest(
            new(0, -1, 0),
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(0.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Right side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest07()
    {
        // Place object at Right side of camera. result must be same as 90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateConstrainedBillboardTest(
            Vector3D.UnitX,
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Left side of camera on XY-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest08()
    {
        // Place object at Left side of camera. result must be same as -90 degrees rotate along z-axis after 90 degrees rotate along x-axis.
        await CreateConstrainedBillboardTest(
            new(-1, 0, 0),
            Vector3D.UnitZ,
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Up-side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest09()
    {
        // Place object at Up-side of camera. result must be same as -90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateConstrainedBillboardTest(
            Vector3D.UnitY,
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Down-side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest10()
    {
        // Place object at Down-side of camera. result must be same as 90 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateConstrainedBillboardTest(
            new(0, -1, 0),
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Forward side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest11()
    {
        // Place object at Forward side of camera. result must be same as 180 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateConstrainedBillboardTest(
            new(0, 0, -1),
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(180.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Place object at Backward side of camera on YZ-plane
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTest12()
    {
        // Place object at Backward side of camera. result must be same as 0 degrees rotate along x-axis after 90 degrees rotate along z-axis.
        await CreateConstrainedBillboardTest(
            Vector3D.UnitZ,
            new(-1, 0, 0),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0)),
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationX(MathHelper.ToRadians(0.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Object and camera positions are too close and doesn't pass cameraForwardVector.
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTooCloseTest1()
    {
        var objectPosition = new Vector3D(3.0, 4.0, 5.0);
        var cameraPosition = objectPosition;
        var cameraUpVector = Vector3D.UnitY;

        // Doesn't pass camera face direction. CreateConstrainedBillboard uses new Vector3(0, 0, -1) direction. Result must be same as 180 degrees rotate along y-axis.
        var expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitZ, new(0, 0, -1));
        await Assert.That(actualRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        objectPosition = new(3.0, 4.0, -5.0);
        cameraPosition = objectPosition;
        cameraUpVector = Vector3D.UnitY;

        expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, new(0, 0, -1), Vector3D.UnitZ);
        await Assert.That(actualLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        await Assert.That(actualRH).IsEqualTo(InverseHandedness(actualLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualRH)).IsEqualTo(actualLH).Within(DefaultVarianceMatrix);
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Object and camera positions are too close and passed cameraForwardVector.
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardTooCloseTest2()
    {
        var objectPosition = new Vector3D(3.0, 4.0, 5.0);
        var cameraPosition = objectPosition;
        var cameraUpVector = Vector3D.UnitY;

        // Passes Vector3.Right as camera face direction. Result must be same as -90 degrees rotate along y-axis.
        var expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualRH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX, new(0, 0, -1));
        await Assert.That(actualRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        objectPosition = new(3.0, 4.0, -5.0);
        cameraPosition = objectPosition;
        cameraUpVector = Vector3D.UnitY;

        expected = Matrix4x4D.CreateRotationY(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateTranslation(objectPosition);
        var actualLH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, cameraUpVector, Vector3D.UnitX, Vector3D.UnitZ);
        await Assert.That(actualLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        await Assert.That(actualRH).IsEqualTo(InverseHandedness(actualLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualRH)).IsEqualTo(actualLH).Within(DefaultVarianceMatrix);
    }

    private static async Task Matrix4x4DCreateConstrainedBillboardAlongAxisTest(Vector3D rotateAxis, Vector3D cameraForward, Vector3D objectForward, Matrix4x4D expectedRotationRightHanded, Matrix4x4D expectedRotationLeftHanded)
    {
        // Place camera at Up-side of object.
        var objectPosition = new Vector3D(3.0, 4.0, 5.0);
        var cameraPosition = objectPosition + rotateAxis * 10.0;

        var expected = expectedRotationRightHanded * Matrix4x4D.CreateTranslation(objectPosition);
        var actualLH = Matrix4x4D.CreateConstrainedBillboard(objectPosition, cameraPosition, rotateAxis, cameraForward, objectForward);
        await Assert.That(actualLH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        rotateAxis = InverseHandedness(rotateAxis);
        cameraForward = InverseHandedness(cameraForward);
        objectForward = InverseHandedness(objectForward);

        objectPosition = new(3.0, 4.0, -5.0);
        cameraPosition = objectPosition + rotateAxis * 10.0;

        expected = expectedRotationLeftHanded * Matrix4x4D.CreateTranslation(objectPosition);
        var actualRH = Matrix4x4D.CreateConstrainedBillboardLeftHanded(objectPosition, cameraPosition, rotateAxis, cameraForward, objectForward);
        await Assert.That(actualRH).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);

        await Assert.That(actualRH).IsEqualTo(InverseHandedness(actualLH)).Within(DefaultVarianceMatrix);
        await Assert.That(InverseHandedness(actualRH)).IsEqualTo(actualLH).Within(DefaultVarianceMatrix);
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Angle between rotateAxis and camera to object vector is too small. And doesn't use passed objectForwardVector parameter.
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardAlongAxisTest1()
    {
        // In this case, CreateConstrainedBillboard picks new Vector3(0, 0, -1) as object forward vector.
        await Matrix4x4DCreateConstrainedBillboardAlongAxisTest(
            Vector3D.UnitY, new(0, 0, -1), new(0, 0, -1),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Angle between rotateAxis and camera to object vector is too small. And user doesn't pass objectForwardVector parameter.
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardAlongAxisTest2()
    {
        // In this case, CreateConstrainedBillboard picks new Vector3(1, 0, 0) as object forward vector.
        await Matrix4x4DCreateConstrainedBillboardAlongAxisTest(
            new(0, 0, -1), new(0, 0, -1), new(0, 0, -1),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Angle between rotateAxis and camera to object vector is too small. And user passed correct objectForwardVector parameter.
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardAlongAxisTest3()
    {
        // User passes correct objectForwardVector.
        await Matrix4x4DCreateConstrainedBillboardAlongAxisTest(
            Vector3D.UnitY, new(0, 0, -1), new(0, 0, -1),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Angle between rotateAxis and camera to object vector is too small. And user passed incorrect objectForwardVector parameter.
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardAlongAxisTest4()
    {
        // User passes correct objectForwardVector.
        await Matrix4x4DCreateConstrainedBillboardAlongAxisTest(
            Vector3D.UnitY, new(0, 0, -1), Vector3D.UnitY,
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)),
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(180.0)));
    }

    // A test for CreateConstrainedBillboard (Vector3, Vector3, Vector3, Vector3?)
    // Angle between rotateAxis and camera to object vector is too small. And user passed incorrect objectForwardVector parameter.
    [Test]
    public async Task Matrix4x4DCreateConstrainedBillboardAlongAxisTest5()
    {
        // In this case, CreateConstrainedBillboard picks Vector3.Right as object forward vector.
        await Matrix4x4DCreateConstrainedBillboardAlongAxisTest(
            new(0, 0, -1), new(0, 0, -1), new(0, 0, -1),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(-90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)),
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(90.0)) * Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(-90.0)));
    }

    // A test for CreateScale (Vector3)
    [Test]
    public async Task Matrix4x4DCreateScaleTest1()
    {
        var scales = new Vector3D(2.0, 3.0, 4.0);
        var expected = new Matrix4x4D(
            2.0, 0.0, 0.0, 0.0,
            0.0, 3.0, 0.0, 0.0,
            0.0, 0.0, 4.0, 0.0,
            0.0, 0.0, 0.0, 1.0);
        var actual = Matrix4x4D.CreateScale(scales);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateScale (Vector3, Vector3)
    [Test]
    public async Task Matrix4x4DCreateScaleCenterTest1()
    {
        var scale = new Vector3D(3, 4, 5);
        var center = new Vector3D(23, 42, 666);

        var scaleAroundZero = Matrix4x4D.CreateScale(scale, Vector3D.Zero);
        var scaleAroundZeroExpected = Matrix4x4D.CreateScale(scale);
        await Assert.That(scaleAroundZero).IsEqualTo(scaleAroundZeroExpected);

        var scaleAroundCenter = Matrix4x4D.CreateScale(scale, center);
        var scaleAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateTranslation(center);
        await Assert.That(scaleAroundCenter).IsEqualTo(scaleAroundCenterExpected);
    }

    // A test for CreateScale (double)
    [Test]
    public async Task Matrix4x4DCreateScaleTest2()
    {
        var scale = 2.0;
        var expected = new Matrix4x4D(
            2.0, 0.0, 0.0, 0.0,
            0.0, 2.0, 0.0, 0.0,
            0.0, 0.0, 2.0, 0.0,
            0.0, 0.0, 0.0, 1.0);
        var actual = Matrix4x4D.CreateScale(scale);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateScale (double, Vector3)
    [Test]
    public async Task Matrix4x4DCreateScaleCenterTest2()
    {
        double scale = 5;
        var center = new Vector3D(23, 42, 666);

        var scaleAroundZero = Matrix4x4D.CreateScale(scale, Vector3D.Zero);
        var scaleAroundZeroExpected = Matrix4x4D.CreateScale(scale);
        await Assert.That(scaleAroundZero).IsEqualTo(scaleAroundZeroExpected);

        var scaleAroundCenter = Matrix4x4D.CreateScale(scale, center);
        var scaleAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateScale(scale) * Matrix4x4D.CreateTranslation(center);
        await Assert.That(scaleAroundCenter).IsEqualTo(scaleAroundCenterExpected);
    }

    // A test for CreateScale (double, double, double)
    [Test]
    public async Task Matrix4x4DCreateScaleTest3()
    {
        var xScale = 2.0;
        var yScale = 3.0;
        var zScale = 4.0;
        var expected = new Matrix4x4D(
            2.0, 0.0, 0.0, 0.0,
            0.0, 3.0, 0.0, 0.0,
            0.0, 0.0, 4.0, 0.0,
            0.0, 0.0, 0.0, 1.0);
        var actual = Matrix4x4D.CreateScale(xScale, yScale, zScale);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateScale (double, double, double, Vector3)
    [Test]
    public async Task Matrix4x4DCreateScaleCenterTest3()
    {
        var scale = new Vector3D(3, 4, 5);
        var center = new Vector3D(23, 42, 666);

        var scaleAroundZero = Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z, Vector3D.Zero);
        var scaleAroundZeroExpected = Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z);
        await Assert.That(scaleAroundZero).IsEqualTo(scaleAroundZeroExpected);

        var scaleAroundCenter = Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z, center);
        var scaleAroundCenterExpected = Matrix4x4D.CreateTranslation(-center) * Matrix4x4D.CreateScale(scale.X, scale.Y, scale.Z) * Matrix4x4D.CreateTranslation(center);
        await Assert.That(scaleAroundCenter).IsEqualTo(scaleAroundCenterExpected);
    }

    // A test for CreateTranslation (Vector3)
    [Test]
    public async Task Matrix4x4DCreateTranslationTest1()
    {
        var position = new Vector3D(2.0, 3.0, 4.0);
        var expected = new Matrix4x4D(
            1.0, 0.0, 0.0, 0.0,
            0.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 1.0, 0.0,
            2.0, 3.0, 4.0, 1.0);

        var actual = Matrix4x4D.CreateTranslation(position);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for CreateTranslation (double, double, double)
    [Test]
    public async Task Matrix4x4DCreateTranslationTest2()
    {
        var xPosition = 2.0;
        var yPosition = 3.0;
        var zPosition = 4.0;

        var expected = new Matrix4x4D(
            1.0, 0.0, 0.0, 0.0,
            0.0, 1.0, 0.0, 0.0,
            0.0, 0.0, 1.0, 0.0,
            2.0, 3.0, 4.0, 1.0);

        var actual = Matrix4x4D.CreateTranslation(xPosition, yPosition, zPosition);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Translation
    [Test]
    public async Task Matrix4x4DTranslationTest()
    {
        var a = GenerateTestMatrix();
        var b = a;

        // Transformed vector that has same semantics of property must be same.
        var val = new Vector3D(a.M41, a.M42, a.M43);
        await Assert.That(a.Translation).IsEqualTo(val);

        // Set value and get value must be same.
        val = new(1.0, 2.0, 3.0);
        a.Translation = val;
        await Assert.That(a.Translation).IsEqualTo(val);

        // Make sure it only modifies expected value of matrix.
        await Assert.That(a)
            .Member(static a => a.M11, v => v.IsEqualTo(b.M11)).And
            .Member(static a => a.M12, v => v.IsEqualTo(b.M12)).And
            .Member(static a => a.M13, v => v.IsEqualTo(b.M13)).And
            .Member(static a => a.M14, v => v.IsEqualTo(b.M14)).And
            .Member(static a => a.M21, v => v.IsEqualTo(b.M21)).And
            .Member(static a => a.M22, v => v.IsEqualTo(b.M22)).And
            .Member(static a => a.M23, v => v.IsEqualTo(b.M23)).And
            .Member(static a => a.M24, v => v.IsEqualTo(b.M24)).And
            .Member(static a => a.M31, v => v.IsEqualTo(b.M31)).And
            .Member(static a => a.M32, v => v.IsEqualTo(b.M32)).And
            .Member(static a => a.M33, v => v.IsEqualTo(b.M33)).And
            .Member(static a => a.M34, v => v.IsEqualTo(b.M34)).And
            .Member(static a => a.M41, v => v.IsNotEqualTo(b.M41)).And
            .Member(static a => a.M42, v => v.IsNotEqualTo(b.M42)).And
            .Member(static a => a.M43, v => v.IsNotEqualTo(b.M43)).And
            .Member(static a => a.M44, v => v.IsEqualTo(b.M44));
    }

    // A test for Equals (Matrix4x4D)
    [Test]
    public async Task Matrix4x4DEqualsTest1()
    {
        var a = GenerateIncrementalMatrixNumber();
        var b = GenerateIncrementalMatrixNumber();

        // case 1: compare between same values
        var expected = true;
        var actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.M11 = 11.0;
        expected = false;
        actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for IsIdentity
    [Test]
    public async Task Matrix4x4DIsIdentityTest()
    {
        await Assert.That(Matrix4x4D.Identity.IsIdentity).IsTrue();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsTrue();
        await Assert.That(new Matrix4x4D(0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 1, 0, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 1, 0, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 1, 0, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 1, 1).IsIdentity).IsFalse();
        await Assert.That(new Matrix4x4D(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 0).IsIdentity).IsFalse();
    }

    // A test for Matrix4x4D (Matrix3x2D)
    [Test]
    public async Task Matrix4x4DFrom3x2Test()
    {
        var source = new Matrix3x2D(1, 2, 3, 4, 5, 6);
        var result = new Matrix4x4D(source);

        await Assert.That(result.M11).IsEqualTo(source.M11);
        await Assert.That(result.M12).IsEqualTo(source.M12);
        await Assert.That(result.M13).IsEqualTo(0.0);
        await Assert.That(result.M14).IsEqualTo(0.0);

        await Assert.That(result.M21).IsEqualTo(source.M21);
        await Assert.That(result.M22).IsEqualTo(source.M22);
        await Assert.That(result.M23).IsEqualTo(0.0);
        await Assert.That(result.M24).IsEqualTo(0.0);

        await Assert.That(result.M31).IsEqualTo(0.0);
        await Assert.That(result.M32).IsEqualTo(0.0);
        await Assert.That(result.M33).IsEqualTo(1d);
        await Assert.That(result.M34).IsEqualTo(0.0);

        await Assert.That(result.M41).IsEqualTo(source.M31);
        await Assert.That(result.M42).IsEqualTo(source.M32);
        await Assert.That(result.M43).IsEqualTo(0.0);
        await Assert.That(result.M44).IsEqualTo(1d);
    }

    // A test for Matrix4x4D comparison involving NaN values
    [Test]
    public async Task Matrix4x4DEqualsNaNTest()
    {
        var a = new Matrix4x4D(double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var b = new Matrix4x4D(0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var c = new Matrix4x4D(0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var d = new Matrix4x4D(0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var e = new Matrix4x4D(0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var f = new Matrix4x4D(0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var g = new Matrix4x4D(0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0, 0);
        var h = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0, 0);
        var i = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0, 0);
        var j = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0, 0);
        var k = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0, 0);
        var l = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0, 0);
        var m = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0, 0);
        var n = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0, 0);
        var o = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN, 0);
        var p = new Matrix4x4D(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, double.NaN);

        await Assert.That(a == new Matrix4x4D()).IsFalse();
        await Assert.That(b == new Matrix4x4D()).IsFalse();
        await Assert.That(c == new Matrix4x4D()).IsFalse();
        await Assert.That(d == new Matrix4x4D()).IsFalse();
        await Assert.That(e == new Matrix4x4D()).IsFalse();
        await Assert.That(f == new Matrix4x4D()).IsFalse();
        await Assert.That(g == new Matrix4x4D()).IsFalse();
        await Assert.That(h == new Matrix4x4D()).IsFalse();
        await Assert.That(i == new Matrix4x4D()).IsFalse();
        await Assert.That(j == new Matrix4x4D()).IsFalse();
        await Assert.That(k == new Matrix4x4D()).IsFalse();
        await Assert.That(l == new Matrix4x4D()).IsFalse();
        await Assert.That(m == new Matrix4x4D()).IsFalse();
        await Assert.That(n == new Matrix4x4D()).IsFalse();
        await Assert.That(o == new Matrix4x4D()).IsFalse();
        await Assert.That(p == new Matrix4x4D()).IsFalse();

        await Assert.That(a != new Matrix4x4D()).IsTrue();
        await Assert.That(b != new Matrix4x4D()).IsTrue();
        await Assert.That(c != new Matrix4x4D()).IsTrue();
        await Assert.That(d != new Matrix4x4D()).IsTrue();
        await Assert.That(e != new Matrix4x4D()).IsTrue();
        await Assert.That(f != new Matrix4x4D()).IsTrue();
        await Assert.That(g != new Matrix4x4D()).IsTrue();
        await Assert.That(h != new Matrix4x4D()).IsTrue();
        await Assert.That(i != new Matrix4x4D()).IsTrue();
        await Assert.That(j != new Matrix4x4D()).IsTrue();
        await Assert.That(k != new Matrix4x4D()).IsTrue();
        await Assert.That(l != new Matrix4x4D()).IsTrue();
        await Assert.That(m != new Matrix4x4D()).IsTrue();
        await Assert.That(n != new Matrix4x4D()).IsTrue();
        await Assert.That(o != new Matrix4x4D()).IsTrue();
        await Assert.That(p != new Matrix4x4D()).IsTrue();

        await Assert.That(a.Equals(new())).IsFalse();
        await Assert.That(b.Equals(new())).IsFalse();
        await Assert.That(c.Equals(new())).IsFalse();
        await Assert.That(d.Equals(new())).IsFalse();
        await Assert.That(e.Equals(new())).IsFalse();
        await Assert.That(f.Equals(new())).IsFalse();
        await Assert.That(g.Equals(new())).IsFalse();
        await Assert.That(h.Equals(new())).IsFalse();
        await Assert.That(i.Equals(new())).IsFalse();
        await Assert.That(j.Equals(new())).IsFalse();
        await Assert.That(k.Equals(new())).IsFalse();
        await Assert.That(l.Equals(new())).IsFalse();
        await Assert.That(m.Equals(new())).IsFalse();
        await Assert.That(n.Equals(new())).IsFalse();
        await Assert.That(o.Equals(new())).IsFalse();
        await Assert.That(p.Equals(new())).IsFalse();

        await Assert.That(a.IsIdentity).IsFalse();
        await Assert.That(b.IsIdentity).IsFalse();
        await Assert.That(c.IsIdentity).IsFalse();
        await Assert.That(d.IsIdentity).IsFalse();
        await Assert.That(e.IsIdentity).IsFalse();
        await Assert.That(f.IsIdentity).IsFalse();
        await Assert.That(g.IsIdentity).IsFalse();
        await Assert.That(h.IsIdentity).IsFalse();
        await Assert.That(i.IsIdentity).IsFalse();
        await Assert.That(j.IsIdentity).IsFalse();
        await Assert.That(k.IsIdentity).IsFalse();
        await Assert.That(l.IsIdentity).IsFalse();
        await Assert.That(m.IsIdentity).IsFalse();
        await Assert.That(n.IsIdentity).IsFalse();
        await Assert.That(o.IsIdentity).IsFalse();
        await Assert.That(p.IsIdentity).IsFalse();

        await Assert.That(a.Equals(a)).IsTrue();
        await Assert.That(b.Equals(b)).IsTrue();
        await Assert.That(c.Equals(c)).IsTrue();
        await Assert.That(d.Equals(d)).IsTrue();
        await Assert.That(e.Equals(e)).IsTrue();
        await Assert.That(f.Equals(f)).IsTrue();
        await Assert.That(g.Equals(g)).IsTrue();
        await Assert.That(h.Equals(h)).IsTrue();
        await Assert.That(i.Equals(i)).IsTrue();
        await Assert.That(j.Equals(j)).IsTrue();
        await Assert.That(k.Equals(k)).IsTrue();
        await Assert.That(l.Equals(l)).IsTrue();
        await Assert.That(m.Equals(m)).IsTrue();
        await Assert.That(n.Equals(n)).IsTrue();
        await Assert.That(o.Equals(o)).IsTrue();
        await Assert.That(p.Equals(p)).IsTrue();
    }

    // A test to make sure these types are blittable directly into GPU buffer memory layouts
    [Test]
    public async Task Matrix4x4DSizeofTest()
    {
        int sizeofMatrix4x4D;
        int sizeofMatrix4x4D_2x;
        int sizeofMatrix4x4DPlusDouble;
        int sizeofMatrix4x4DPlusDouble_2x;

        unsafe
        {
            sizeofMatrix4x4D = sizeof(Matrix4x4D);
            sizeofMatrix4x4D_2x = sizeof(Matrix4x4D_2x);
            sizeofMatrix4x4DPlusDouble = sizeof(Matrix4x4DPlusDouble);
            sizeofMatrix4x4DPlusDouble_2x = sizeof(Matrix4x4DPlusDouble_2x);
        }

        await Assert.That(sizeofMatrix4x4D).IsEqualTo(128);
        await Assert.That(sizeofMatrix4x4D_2x).IsEqualTo(256);
        await Assert.That(sizeofMatrix4x4DPlusDouble).IsEqualTo(136);
        await Assert.That(sizeofMatrix4x4DPlusDouble_2x).IsEqualTo(272);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Matrix4x4D_2x
    {
        private Matrix4x4D _a;
        private Matrix4x4D _b;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Matrix4x4DPlusDouble
    {
        private Matrix4x4D _v;
        private readonly double _f;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Matrix4x4DPlusDouble_2x
    {
        private Matrix4x4DPlusDouble _a;
        private Matrix4x4DPlusDouble _b;
    }

    // A test to make sure the fields are laid out how we expect
    [Test]
    public async Task Matrix4x4DFieldOffsetTest()
    {
        IntPtr basePtr;
        IntPtr matPtr;
        IntPtr intPtrMatM11;
        IntPtr intPtrMatM12;
        IntPtr intPtrMatM13;
        IntPtr intPtrMatM14;
        IntPtr intPtrMatM21;
        IntPtr intPtrMatM22;
        IntPtr intPtrMatM23;
        IntPtr intPtrMatM24;
        IntPtr intPtrMatM31;
        IntPtr intPtrMatM32;
        IntPtr intPtrMatM33;
        IntPtr intPtrMatM34;
        IntPtr intPtrMatM41;
        IntPtr intPtrMatM42;
        IntPtr intPtrMatM43;
        IntPtr intPtrMatM44;

        unsafe
        {
            var mat = new Matrix4x4D();

#pragma warning disable CS9123 // The '&' operator should not be used on parameters or local variables in async methods.
            basePtr = new(&mat.M11); // Take address of first element
            matPtr = new(&mat); // Take address of whole matrix

            intPtrMatM11 = new(&mat.M11);
            intPtrMatM12 = new(&mat.M12);
            intPtrMatM13 = new(&mat.M13);
            intPtrMatM14 = new(&mat.M14);
            intPtrMatM21 = new(&mat.M21);
            intPtrMatM22 = new(&mat.M22);
            intPtrMatM23 = new(&mat.M23);
            intPtrMatM24 = new(&mat.M24);
            intPtrMatM31 = new(&mat.M31);
            intPtrMatM32 = new(&mat.M32);
            intPtrMatM33 = new(&mat.M33);
            intPtrMatM34 = new(&mat.M34);
            intPtrMatM41 = new(&mat.M41);
            intPtrMatM42 = new(&mat.M42);
            intPtrMatM43 = new(&mat.M43);
            intPtrMatM44 = new(&mat.M44);
#pragma warning restore CS9123 // The '&' operator should not be used on parameters or local variables in async methods.
        }

        await Assert.That(matPtr).IsEqualTo(basePtr);

        await Assert.That(intPtrMatM11).IsEqualTo(basePtr + 0);
        await Assert.That(intPtrMatM12).IsEqualTo(basePtr + 8);
        await Assert.That(intPtrMatM13).IsEqualTo(basePtr + 16);
        await Assert.That(intPtrMatM14).IsEqualTo(basePtr + 24);

        await Assert.That(intPtrMatM21).IsEqualTo(basePtr + 32);
        await Assert.That(intPtrMatM22).IsEqualTo(basePtr + 40);
        await Assert.That(intPtrMatM23).IsEqualTo(basePtr + 48);
        await Assert.That(intPtrMatM24).IsEqualTo(basePtr + 56);

        await Assert.That(intPtrMatM31).IsEqualTo(basePtr + 64);
        await Assert.That(intPtrMatM32).IsEqualTo(basePtr + 72);
        await Assert.That(intPtrMatM33).IsEqualTo(basePtr + 80);
        await Assert.That(intPtrMatM34).IsEqualTo(basePtr + 88);

        await Assert.That(intPtrMatM41).IsEqualTo(basePtr + 96);
        await Assert.That(intPtrMatM42).IsEqualTo(basePtr + 104);
        await Assert.That(intPtrMatM43).IsEqualTo(basePtr + 112);
        await Assert.That(intPtrMatM44).IsEqualTo(basePtr + 120);
    }

    [Test]
    public async Task PerspectiveFarPlaneAtInfinityTest()
    {
        var nearPlaneDistance = 0.125;
        var m = Matrix4x4D.CreatePerspective(1.0, 1.0, nearPlaneDistance, double.PositiveInfinity);
        await Assert.That(m.M33).IsEqualTo(-1.0);
        await Assert.That(m.M43).IsEqualTo(-nearPlaneDistance);
    }

    [Test]
    public async Task PerspectiveFieldOfViewFarPlaneAtInfinityTest()
    {
        var nearPlaneDistance = 0.125;
        var m = Matrix4x4D.CreatePerspectiveFieldOfView(MathHelper.ToRadians(60.0), 1.5, nearPlaneDistance, double.PositiveInfinity);
        await Assert.That(m.M33).IsEqualTo(-1.0);
        await Assert.That(m.M43).IsEqualTo(-nearPlaneDistance);
    }

    [Test]
    public async Task PerspectiveOffCenterFarPlaneAtInfinityTest()
    {
        var nearPlaneDistance = 0.125;
        var m = Matrix4x4D.CreatePerspectiveOffCenter(0.0, 0.0, 1.0, 1.0, nearPlaneDistance, double.PositiveInfinity);
        await Assert.That(m.M33).IsEqualTo(-1.0);
        await Assert.That(m.M43).IsEqualTo(-nearPlaneDistance);
    }

    [Test]
    public async Task Matrix4x4DCreateBroadcastScalarTest()
    {
        var a = Matrix4x4D.Create(double.Pi);

        await Assert.That(a.X).IsEqualTo(Vector4D.Pi);
        await Assert.That(a.Y).IsEqualTo(Vector4D.Pi);
        await Assert.That(a.Z).IsEqualTo(Vector4D.Pi);
        await Assert.That(a.W).IsEqualTo(Vector4D.Pi);
    }

    [Test]
    public async Task Matrix4x4DCreateBroadcastVectorTest()
    {
        var a = Matrix4x4D.Create(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity));

        await Assert.That(a.X).IsEqualTo(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity));
        await Assert.That(a.Y).IsEqualTo(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity));
        await Assert.That(a.Z).IsEqualTo(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity));
        await Assert.That(a.W).IsEqualTo(Vector4D.Create(double.Pi, double.E, double.PositiveInfinity, double.NegativeInfinity));
    }

    [Test]
    public async Task Matrix4x4DCreateVectorsTest()
    {
        var a = Matrix4x4D.Create(
            Vector4D.Create(11.0, 12.0, 13.0, 14.0),
            Vector4D.Create(21.0, 22.0, 23.0, 24.0),
            Vector4D.Create(31.0, 32.0, 33.0, 34.0),
            Vector4D.Create(41.0, 42.0, 43.0, 44.0)
        );

        await Assert.That(a.X).IsEqualTo(Vector4D.Create(11.0, 12.0, 13.0, 14.0));
        await Assert.That(a.Y).IsEqualTo(Vector4D.Create(21.0, 22.0, 23.0, 24.0));
        await Assert.That(a.Z).IsEqualTo(Vector4D.Create(31.0, 32.0, 33.0, 34.0));
        await Assert.That(a.W).IsEqualTo(Vector4D.Create(41.0, 42.0, 43.0, 44.0));
    }

    [Test]
    public async Task Matrix4x4DGetElementTest()
    {
        var a = GenerateTestMatrix();

        await Assert.That(a.X.X).IsEqualTo(a.M11);
        await Assert.That(a[0, 0]).IsEqualTo(a.M11);
        await Assert.That(a.GetElement(0, 0)).IsEqualTo(a.M11);

        await Assert.That(a.X.Y).IsEqualTo(a.M12);
        await Assert.That(a[0, 1]).IsEqualTo(a.M12);
        await Assert.That(a.GetElement(0, 1)).IsEqualTo(a.M12);

        await Assert.That(a.X.Z).IsEqualTo(a.M13);
        await Assert.That(a[0, 2]).IsEqualTo(a.M13);
        await Assert.That(a.GetElement(0, 2)).IsEqualTo(a.M13);

        await Assert.That(a.X.W).IsEqualTo(a.M14);
        await Assert.That(a[0, 3]).IsEqualTo(a.M14);
        await Assert.That(a.GetElement(0, 3)).IsEqualTo(a.M14);

        await Assert.That(a.Y.X).IsEqualTo(a.M21);
        await Assert.That(a[1, 0]).IsEqualTo(a.M21);
        await Assert.That(a.GetElement(1, 0)).IsEqualTo(a.M21);

        await Assert.That(a.Y.Y).IsEqualTo(a.M22);
        await Assert.That(a[1, 1]).IsEqualTo(a.M22);
        await Assert.That(a.GetElement(1, 1)).IsEqualTo(a.M22);

        await Assert.That(a.Y.Z).IsEqualTo(a.M23);
        await Assert.That(a[1, 2]).IsEqualTo(a.M23);
        await Assert.That(a.GetElement(1, 2)).IsEqualTo(a.M23);

        await Assert.That(a.Y.W).IsEqualTo(a.M24);
        await Assert.That(a[1, 3]).IsEqualTo(a.M24);
        await Assert.That(a.GetElement(1, 3)).IsEqualTo(a.M24);

        await Assert.That(a.Z.X).IsEqualTo(a.M31);
        await Assert.That(a[2, 0]).IsEqualTo(a.M31);
        await Assert.That(a.GetElement(2, 0)).IsEqualTo(a.M31);

        await Assert.That(a.Z.Y).IsEqualTo(a.M32);
        await Assert.That(a[2, 1]).IsEqualTo(a.M32);
        await Assert.That(a.GetElement(2, 1)).IsEqualTo(a.M32);

        await Assert.That(a.Z.Z).IsEqualTo(a.M33);
        await Assert.That(a[2, 2]).IsEqualTo(a.M33);
        await Assert.That(a.GetElement(2, 2)).IsEqualTo(a.M33);

        await Assert.That(a.Z.W).IsEqualTo(a.M34);
        await Assert.That(a[2, 3]).IsEqualTo(a.M34);
        await Assert.That(a.GetElement(2, 3)).IsEqualTo(a.M34);

        await Assert.That(a.W.X).IsEqualTo(a.M41);
        await Assert.That(a[3, 0]).IsEqualTo(a.M41);
        await Assert.That(a.GetElement(3, 0)).IsEqualTo(a.M41);

        await Assert.That(a.W.Y).IsEqualTo(a.M42);
        await Assert.That(a[3, 1]).IsEqualTo(a.M42);
        await Assert.That(a.GetElement(3, 1)).IsEqualTo(a.M42);

        await Assert.That(a.W.Z).IsEqualTo(a.M43);
        await Assert.That(a[3, 2]).IsEqualTo(a.M43);
        await Assert.That(a.GetElement(3, 2)).IsEqualTo(a.M43);

        await Assert.That(a.W.W).IsEqualTo(a.M44);
        await Assert.That(a[3, 3]).IsEqualTo(a.M44);
        await Assert.That(a.GetElement(3, 3)).IsEqualTo(a.M44);
    }

    [Test]
    public async Task Matrix4x4DGetRowTest()
    {
        var a = GenerateTestMatrix();

        var vx = new Vector4D(a.M11, a.M12, a.M13, a.M14);
        await Assert.That(a.X).IsEqualTo(vx);
        await Assert.That(a[0]).IsEqualTo(vx);
        await Assert.That(a.GetRow(0)).IsEqualTo(vx);

        var vy = new Vector4D(a.M21, a.M22, a.M23, a.M24);
        await Assert.That(a.Y).IsEqualTo(vy);
        await Assert.That(a[1]).IsEqualTo(vy);
        await Assert.That(a.GetRow(1)).IsEqualTo(vy);

        var vz = new Vector4D(a.M31, a.M32, a.M33, a.M34);
        await Assert.That(a.Z).IsEqualTo(vz);
        await Assert.That(a[2]).IsEqualTo(vz);
        await Assert.That(a.GetRow(2)).IsEqualTo(vz);

        var vw = new Vector4D(a.M41, a.M42, a.M43, a.M44);
        await Assert.That(a.W).IsEqualTo(vw);
        await Assert.That(a[3]).IsEqualTo(vw);
        await Assert.That(a.GetRow(3)).IsEqualTo(vw);
    }

    [Test]
    public async Task Matrix4x4DWithElementTest()
    {
        var a = Matrix4x4D.Identity;

        a[0, 0] = 11.0;
        await Assert.That(a.WithElement(0, 0, 11.5).M11).IsEqualTo(11.5);
        await Assert.That(a.M11).IsEqualTo(11.0);

        a[0, 1] = 12.0;
        await Assert.That(a.WithElement(0, 1, 12.5).M12).IsEqualTo(12.5);
        await Assert.That(a.M12).IsEqualTo(12.0);

        a[0, 2] = 13.0;
        await Assert.That(a.WithElement(0, 2, 13.5).M13).IsEqualTo(13.5);
        await Assert.That(a.M13).IsEqualTo(13.0);

        a[0, 3] = 14.0;
        await Assert.That(a.WithElement(0, 3, 14.5).M14).IsEqualTo(14.5);
        await Assert.That(a.M14).IsEqualTo(14.0);

        a[1, 0] = 21.0;
        await Assert.That(a.WithElement(1, 0, 21.5).M21).IsEqualTo(21.5);
        await Assert.That(a.M21).IsEqualTo(21.0);

        a[1, 1] = 22.0;
        await Assert.That(a.WithElement(1, 1, 22.5).M22).IsEqualTo(22.5);
        await Assert.That(a.M22).IsEqualTo(22.0);

        a[1, 2] = 23.0;
        await Assert.That(a.WithElement(1, 2, 23.5).M23).IsEqualTo(23.5);
        await Assert.That(a.M23).IsEqualTo(23.0);

        a[1, 3] = 24.0;
        await Assert.That(a.WithElement(1, 3, 24.5).M24).IsEqualTo(24.5);
        await Assert.That(a.M24).IsEqualTo(24.0);

        a[2, 0] = 31.0;
        await Assert.That(a.WithElement(2, 0, 31.5).M31).IsEqualTo(31.5);
        await Assert.That(a.M31).IsEqualTo(31.0);

        a[2, 1] = 32.0;
        await Assert.That(a.WithElement(2, 1, 32.5).M32).IsEqualTo(32.5);
        await Assert.That(a.M32).IsEqualTo(32.0);

        a[2, 2] = 33.0;
        await Assert.That(a.WithElement(2, 2, 33.5).M33).IsEqualTo(33.5);
        await Assert.That(a.M33).IsEqualTo(33.0);

        a[2, 3] = 34.0;
        await Assert.That(a.WithElement(2, 3, 34.5).M34).IsEqualTo(34.5);
        await Assert.That(a.M34).IsEqualTo(34.0);

        a[3, 0] = 41.0;
        await Assert.That(a.WithElement(3, 0, 41.5).M41).IsEqualTo(41.5);
        await Assert.That(a.M41).IsEqualTo(41.0);

        a[3, 1] = 42.0;
        await Assert.That(a.WithElement(3, 1, 42.5).M42).IsEqualTo(42.5);
        await Assert.That(a.M42).IsEqualTo(42.0);

        a[3, 2] = 43.0;
        await Assert.That(a.WithElement(3, 2, 43.5).M43).IsEqualTo(43.5);
        await Assert.That(a.M43).IsEqualTo(43.0);

        a[3, 3] = 44.0;
        await Assert.That(a.WithElement(3, 3, 44.5).M44).IsEqualTo(44.5);
        await Assert.That(a.M44).IsEqualTo(44.0);
    }

    [Test]
    public async Task Matrix4x4DWithRowTest()
    {
        var a = Matrix4x4D.Identity;

        a[0] = Vector4D.Create(11.0, 12.0, 13.0, 14.0);
        await Assert.That(a.WithRow(0, Vector4D.Create(11.5, 12.5, 13.5, 14.5)).X).IsEqualTo(Vector4D.Create(11.5, 12.5, 13.5, 14.5));
        await Assert.That(a.X).IsEqualTo(Vector4D.Create(11.0, 12.0, 13.0, 14.0));

        a[1] = Vector4D.Create(21.0, 22.0, 23.0, 24.0);
        await Assert.That(a.WithRow(1, Vector4D.Create(21.5, 22.5, 23.5, 24.5)).Y).IsEqualTo(Vector4D.Create(21.5, 22.5, 23.5, 24.5));
        await Assert.That(a.Y).IsEqualTo(Vector4D.Create(21.0, 22.0, 23.0, 24.0));

        a[2] = Vector4D.Create(31.0, 32.0, 33.0, 34.0);
        await Assert.That(a.WithRow(2, Vector4D.Create(31.5, 32.5, 33.5, 34.5)).Z).IsEqualTo(Vector4D.Create(31.5, 32.5, 33.5, 34.5));
        await Assert.That(a.Z).IsEqualTo(Vector4D.Create(31.0, 32.0, 33.0, 34.0));

        a[3] = Vector4D.Create(41.0, 42.0, 43.0, 44.0);
        await Assert.That(a.WithRow(3, Vector4D.Create(41.5, 42.5, 43.5, 44.5)).W).IsEqualTo(Vector4D.Create(41.5, 42.5, 43.5, 44.5));
        await Assert.That(a.W).IsEqualTo(Vector4D.Create(41.0, 42.0, 43.0, 44.0));
    }
}