namespace Altemiq.Numerics;

using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics;
using Altemiq.Runtime.Intrinsics;

public class Vector2DTests
{
    private const int ElementCount = 2;
    
    [Test]
    public async Task Vector2DMarshalSizeTest()
    {
        await Assert.That(Marshal.SizeOf<Vector2D>()).IsEqualTo(16);
        await Assert.That(Marshal.SizeOf<Vector2D>(new())).IsEqualTo(16);
    }

    [Test]
    [Arguments(0.0, 1.0)]
    [Arguments(1.0, 0.0)]
    [Arguments(3.1434343, 1.1234123)]
    [Arguments(1.0000001, 0.0000001)]
    public async Task Vector2DIndexerGetTest(double x, double y)
    {
        var vector = new Vector2D(x, y);

        await Assert.That(vector[0]).IsEqualTo(x);
        await Assert.That(vector[1]).IsEqualTo(y);
    }

    [Test]
    [Arguments(0.0, 1.0)]
    [Arguments(1.0, 0.0)]
    [Arguments(3.1434343, 1.1234123)]
    [Arguments(1.0000001, 0.0000001)]
    public async Task Vector2DIndexerSetTest(double x, double y)
    {
        var vector = new Vector2D(0.0, 0.0);

        vector[0] = x;
        vector[1] = y;

        await Assert.That(vector[0]).IsEqualTo(x);
        await Assert.That(vector[1]).IsEqualTo(y);
    }

    [Test]
    public async Task Vector2DCopyToTest()
    {
        Vector2D v1 = new Vector2D(2.0, 3.0);

        double[] a = new double[3];
        double[] b = new double[2];

        Assert.Throws<NullReferenceException>(() => v1.CopyTo(null!, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, -1));
        Assert.Throws<ArgumentOutOfRangeException>(() => v1.CopyTo(a, a.Length));
        Assert.Throws<ArgumentException>(() => v1.CopyTo(a, 2));

        v1.CopyTo(a, 1);
        v1.CopyTo(b);
        await Assert.That(a[0]).IsEqualTo(0.0);
        await Assert.That(a[1]).IsEqualTo(2.0);
        await Assert.That(a[2]).IsEqualTo(3.0);
        await Assert.That(b[0]).IsEqualTo(2.0);
        await Assert.That(b[1]).IsEqualTo(3.0);
    }

    [Test]
    public async Task Vector2DCopyToSpanTest()
    {
        Vector2D vector = new Vector2D(1.0, 2.0);
        var destination = new double[2];

        Assert.Throws<ArgumentException>(() => vector.CopyTo(new Span<double>(new double[1])));
        vector.CopyTo(destination.AsSpan());

        await Assert.That(vector.X).IsEqualTo(1.0);
        await Assert.That(vector.Y).IsEqualTo(2.0);
        await Assert.That(destination[0]).IsEqualTo(vector.X);
        await Assert.That(destination[1]).IsEqualTo(vector.Y);
    }

    [Test]
    public async Task Vector2DTryCopyToTest()
    {
        Vector2D vector = new Vector2D(1.0, 2.0);
        var destination = new double[2];

        await Assert.That(vector.TryCopyTo(new(new double[1]))).IsFalse();
        await Assert.That(vector.TryCopyTo(destination.AsSpan())).IsTrue();

        await Assert.That(vector.X).IsEqualTo(1.0);
        await Assert.That(vector.Y).IsEqualTo(2.0);
        await Assert.That(destination[0]).IsEqualTo(vector.X);
        await Assert.That(destination[1]).IsEqualTo(vector.Y);
    }

    [Test]
    public async Task Vector2DGetHashCodeTest()
    {
        Vector2D v1 = new Vector2D(2.0, 3.0);
        Vector2D v2 = new Vector2D(2.0, 3.0);
        Vector2D v3 = new Vector2D(3.0, 2.0);
        await Assert.That(v1.GetHashCode()).IsEqualTo(v1.GetHashCode());
        await Assert.That(v2.GetHashCode()).IsEqualTo(v1.GetHashCode());
        await Assert.That(v3.GetHashCode()).IsNotEqualTo(v1.GetHashCode());
        Vector2D v4 = new Vector2D(0.0, 0.0);
        Vector2D v6 = new Vector2D(1.0, 0.0);
        Vector2D v7 = new Vector2D(0.0, 1.0);
        Vector2D v8 = new Vector2D(1.0, 1.0);
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v7.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v8.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v7.GetHashCode());
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v8.GetHashCode());
        await Assert.That(v7.GetHashCode()).IsNotEqualTo(v8.GetHashCode());
    }

    [Test]
    public async Task Vector2DToStringTest()
    {
        string separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
        CultureInfo enUsCultureInfo = new CultureInfo("en-US");

        Vector2D v1 = new Vector2D(2.0, 3.0);

        string v1str = v1.ToString();
        string expectedv1 = string.Format(CultureInfo.CurrentCulture
            , "<{1:G}{0} {2:G}>"
            , new object[] { separator, 2, 3 });
        await Assert.That(v1str).IsEqualTo(expectedv1);

        string v1strformatted = v1.ToString("c", CultureInfo.CurrentCulture);
        string expectedv1formatted = string.Format(CultureInfo.CurrentCulture
            , "<{1:c}{0} {2:c}>"
            , new object[] { separator, 2, 3 });
        await Assert.That(v1strformatted).IsEqualTo(expectedv1formatted);

        string v2strformatted = v1.ToString("c", enUsCultureInfo);
        string expectedv2formatted = string.Format(enUsCultureInfo
            , "<{1:c}{0} {2:c}>"
            , new object[] { enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3 });
        await Assert.That(v2strformatted).IsEqualTo(expectedv2formatted);

        string v3strformatted = v1.ToString("c");
        string expectedv3formatted = string.Format(CultureInfo.CurrentCulture
            , "<{1:c}{0} {2:c}>"
            , new object[] { separator, 2, 3 });
        await Assert.That(v3strformatted).IsEqualTo(expectedv3formatted);
    }

    // A test for Distance (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DDistanceTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(3.0, 4.0);

        await Assert.That(Vector2D.Distance(a, b)).IsEqualTo(Math.Sqrt(8));
    }

    // A test for Distance (Vector2D, Vector2D)
    // Distance from the same point
    [Test]
    public async Task Vector2DDistanceTest2()
    {
        Vector2D a = new Vector2D(1.051, 2.05);
        Vector2D b = new Vector2D(1.051, 2.05);

        await Assert.That(Vector2D.Distance(a, b)).IsEqualTo(0.0);
    }

    // A test for DistanceSquared (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DDistanceSquaredTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(3.0, 4.0);

        await Assert.That(Vector2D.DistanceSquared(a, b)).IsEqualTo(8);
    }

    // A test for Dot (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DDotTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(3.0, 4.0);

        await Assert.That(Vector2D.Dot(a, b)).IsEqualTo(11.0);
    }

    // A test for Dot (Vector2D, Vector2D)
    // Dot test for perpendicular vector
    [Test]
    public async Task Vector2DDotTest1()
    {
        Vector2D a = new Vector2D(1.55, 1.55);
        Vector2D b = new Vector2D(-1.55, 1.55);

        await Assert.That(Vector2D.Dot(a, b)).IsEqualTo(0.0);
    }

    // A test for Dot (Vector2D, Vector2D)
    // Dot test with specail double values
    [Test]
    public async Task Vector2DDotTest2()
    {
        Vector2D a = new Vector2D(double.MinValue, double.MinValue);
        Vector2D b = new Vector2D(double.MaxValue, double.MaxValue);

        await Assert.That(Vector2D.Dot(a, b)).IsNegativeInfinity();
    }

    [Test]
    public async Task Vector2DCrossTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(-4.0, 3.0);

        await Assert.That(Vector2D.Cross(a, b)).IsEqualTo(11.0);
    }

    [Test]
    public async Task Vector2DCrossTest1()
    {
        // Cross test for parallel vector
        Vector2D a = new Vector2D(1.55, 1.55);
        Vector2D b = new Vector2D(-1.55, -1.55);

        await Assert.That(Vector2D.Cross(a, b)).IsEqualTo(0.0);
    }

    [Test]
    public async Task Vector2DCrossTest2()
    {
        // Cross test with specail double values
        Vector2D a = new Vector2D(double.MinValue, double.MinValue);
        Vector2D b = new Vector2D(double.MinValue, double.MaxValue);

        await Assert.That(Vector2D.Cross(a, b)).IsNegativeInfinity();
    }

    // A test for Length ()
    [Test]
    public async Task Vector2DLengthTest()
    {
        Vector2D a = new Vector2D(2.0, 4.0);

        await Assert.That(a.Length()).IsEqualTo(Math.Sqrt(20));
    }

    // A test for Length ()
    // Length test where length is zero
    [Test]
    public async Task Vector2DLengthTest1()
    {
        Vector2D target = new Vector2D();
        target.X = 0.0;
        target.Y = 0.0;

        await Assert.That(target.Length()).IsEqualTo(0.0);
    }

    // A test for LengthSquared ()
    [Test]
    public async Task Vector2DLengthSquaredTest()
    {
        Vector2D a = new Vector2D(2.0, 4.0);

        await Assert.That(a.LengthSquared()).IsEqualTo(20.0);
    }

    // A test for LengthSquared ()
    // LengthSquared test where the result is zero
    [Test]
    public async Task Vector2DLengthSquaredTest1()
    {
        Vector2D a = new Vector2D(0.0, 0.0);

        await Assert.That(a.LengthSquared()).IsEqualTo(0.0);
    }

    // A test for Min (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DMinTest()
    {
        Vector2D a = new Vector2D(-1.0, 4.0);
        Vector2D b = new Vector2D(2.0, 1.0);

        await Assert.That(Vector2D.Min(a, b)).IsEqualTo(new Vector2D(-1.0, 1.0));
    }

    [Test]
    public async Task Vector2DMinMaxCodeCoverageTest()
    {
        Vector2D min = new Vector2D(0, 0);
        Vector2D max = new Vector2D(1, 1);

        // Min.
        await Assert.That(min).IsEqualTo(Vector2D.Min(min, max));
        await Assert.That(min).IsEqualTo(Vector2D.Min(max, min));

        // Max.
        await Assert.That(max).IsEqualTo(Vector2D.Max(min, max));
        await Assert.That(max).IsEqualTo(Vector2D.Max(max, min));
    }

    // A test for Max (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DMaxTest()
    {
        Vector2D a = new Vector2D(-1.0, 4.0);
        Vector2D b = new Vector2D(2.0, 1.0);

        await Assert.That(Vector2D.Max(a, b)).IsEqualTo(new Vector2D(2.0, 4.0));
    }

    // A test for Clamp (Vector2D, Vector2D, Vector2D)
    [Test]
    public async Task Vector2DClampTest()
    {
        Vector2D a = new Vector2D(0.5, 0.3);
        Vector2D min = new Vector2D(0.0, 0.1);
        Vector2D max = new Vector2D(1.0, 1.1);

        // Normal case.
        // Case N1: specified value is in the range.
        Vector2D expected = new Vector2D(0.5, 0.3);
        Vector2D actual = Vector2D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);
        // Normal case.
        // Case N2: specified value is bigger than max value.
        a = new Vector2D(2.0, 3.0);
        expected = max;
        actual = Vector2D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);
        // Case N3: specified value is smaller than max value.
        a = new Vector2D(-1.0, -2.0);
        expected = min;
        actual = Vector2D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);
        // Case N4: combination case.
        a = new Vector2D(-2.0, 4.0);
        expected = new Vector2D(min.X, max.Y);
        actual = Vector2D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);
        // User specified min value is bigger than max value.
        max = new Vector2D(0.0, 0.1);
        min = new Vector2D(1.0, 1.1);

        // Case W1: specified value is in the range.
        a = new Vector2D(0.5, 0.3);
        expected = max;
        actual = Vector2D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Normal case.
        // Case W2: specified value is bigger than max and min value.
        a = new Vector2D(2.0, 3.0);
        expected = max;
        actual = Vector2D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Case W3: specified value is smaller than min and max value.
        a = new Vector2D(-1.0, -2.0);
        expected = max;
        actual = Vector2D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    [Test]
    public async Task Vector2DLerpTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(3.0, 4.0);

        double t = 0.5;

        await Assert.That(Vector2D.Lerp(a, b, t)).IsEqualTo(new Vector2D(2.0, 3.0));
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test with factor zero
    [Test]
    public async Task Vector2DLerpTest1()
    {
        Vector2D a = new Vector2D(0.0, 0.0);
        Vector2D b = new Vector2D(3.18, 4.25);

        double t = 0.0;
        await Assert.That(Vector2D.Lerp(a, b, t)).IsEqualTo(Vector2D.Zero);
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test with factor one
    [Test]
    public async Task Vector2DLerpTest2()
    {
        Vector2D a = new Vector2D(0.0, 0.0);
        Vector2D b = new Vector2D(3.18, 4.25);

        double t = 1.0;
        Vector2D expected = new Vector2D(3.18, 4.25);
        Vector2D actual = Vector2D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test with factor > 1
    [Test]
    public async Task Vector2DLerpTest3()
    {
        Vector2D a = new Vector2D(0.0, 0.0);
        Vector2D b = new Vector2D(3.18, 4.25);

        double t = 2.0;
        Vector2D expected = b * 2.0;
        Vector2D actual = Vector2D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test with factor < 0
    [Test]
    public async Task Vector2DLerpTest4()
    {
        Vector2D a = new Vector2D(0.0, 0.0);
        Vector2D b = new Vector2D(3.18, 4.25);

        double t = -2.0;
        Vector2D expected = -(b * 2.0);
        Vector2D actual = Vector2D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test with special double value
    [Test]
    public async Task Vector2DLerpTest5()
    {
        Vector2D a = new Vector2D(45.67, 90.0);
        Vector2D b = new Vector2D(double.PositiveInfinity, double.NegativeInfinity);

        double t = 0.408;
        Vector2D actual = Vector2D.Lerp(a, b, t);
        await Assert.That(actual.X).IsPositiveInfinity();
        await Assert.That(actual.Y).IsNegativeInfinity();
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test from the same point
    [Test]
    public async Task Vector2DLerpTest6()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(1.0, 2.0);

        double t = 0.5;

        Vector2D expected = new Vector2D(1.0, 2.0);
        Vector2D actual = Vector2D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test with values known to be inaccurate with the old lerp impl
    [Test]
    public async Task Vector2DLerpTest7()
    {
        Vector2D a = new Vector2D(0.44728136);
        Vector2D b = new Vector2D(0.46345946);

        double t = 0.26402435;

        Vector2D expected = new Vector2D(0.45155275);
        Vector2D actual = Vector2D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected).Within(0.0000001);
    }

    // A test for Lerp (Vector2D, Vector2D, double)
    // Lerp test with values known to be inaccurate with the old lerp impl
    // (Old code incorrectly gets 0.33333588)
    [Test]
    public async Task Vector2DLerpTest8()
    {
        Vector2D a = new Vector2D(-100);
        Vector2D b = new Vector2D(0.33333334);

        double t = 1;

        Vector2D expected = new Vector2D(0.33333334);
        Vector2D actual = Vector2D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform(Vector2D, Matrix4x4)
    [Test]
    public async Task Vector2DTransformTest()
    {
        Vector2D v = new Vector2D(1.0, 2.0);
        Matrix4x4D m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        Vector2D expected = new Vector2D(10.316987, 22.183012);
        Vector2D actual;

        actual = Vector2D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(0.000001);
    }

    // A test for Transform(Vector2D, Matrix3x2)
    [Test]
    public async Task Vector2DTransform3x2Test()
    {
        Vector2D v = new Vector2D(1.0, 2.0);
        Matrix3x2D m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0));
        m.M31 = 10.0;
        m.M32 = 20.0;

        Vector2D expected = new Vector2D(9.866025, 22.23205);
        Vector2D actual;

        actual = Vector2D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(0.000001);
    }

    // A test for TransformNormal (Vector2D, Matrix4x4)
    [Test]
    public async Task Vector2DTransformNormalTest()
    {
        Vector2D v = new Vector2D(1.0, 2.0);
        Matrix4x4D m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        Vector2D expected = new Vector2D(0.3169873, 2.18301272);
        Vector2D actual;

        actual = Vector2D.TransformNormal(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(0.000001);
    }

    // A test for TransformNormal (Vector2D, Matrix3x2)
    [Test]
    public async Task Vector2DTransformNormal3x2Test()
    {
        Vector2D v = new Vector2D(1.0, 2.0);
        Matrix3x2D m = Matrix3x2D.CreateRotation(MathHelper.ToRadians(30.0));
        m.M31 = 10.0;
        m.M32 = 20.0;

        Vector2D expected = new Vector2D(-0.133974612, 2.232051);
        var actual = Vector2D.TransformNormal(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(0.000001);
    }

    // A test for Transform (Vector2D, Quaternion)
    [Test]
    public async Task Vector2DTransformByQuaternionTest()
    {
        Vector2D v = new Vector2D(1.0, 2.0);

        Matrix4x4D m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        QuaternionD q = QuaternionD.CreateFromRotationMatrix(m);

        Vector2D expected = Vector2D.Transform(v, m);
        Vector2D actual = Vector2D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2D, Quaternion)
    // Transform Vector2D with zero quaternion
    [Test]
    public async Task Vector2DTransformByQuaternionTest1()
    {
        Vector2D v = new Vector2D(1.0, 2.0);
        QuaternionD q = new QuaternionD();
        Vector2D expected = Vector2D.Zero;

        Vector2D actual = Vector2D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2D, Quaternion)
    // Transform Vector2D with identity quaternion
    [Test]
    public async Task Vector2DTransformByQuaternionTest2()
    {
        Vector2D v = new Vector2D(1.0, 2.0);
        QuaternionD q = QuaternionD.Identity;
        Vector2D expected = v;

        Vector2D actual = Vector2D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector2D)
    [Test]
    public async Task Vector2DNormalizeTest()
    {
        Vector2D a = new Vector2D(2.0, 3.0);
        Vector2D expected = new Vector2D(0.554700196225229122018341733457, 0.8320502943378436830275126001855);

        var actual = Vector2D.Normalize(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector2D)
    // Normalize zero length vector
    [Test]
    public async Task Vector2DNormalizeTest1()
    {
        Vector2D a = new Vector2D(); // no parameter, default to 0.0
        Vector2D actual = Vector2D.Normalize(a);
        await Assert.That(actual.X).IsNaN();
        await Assert.That(actual.Y).IsNaN();
    }

    // A test for Normalize (Vector2D)
    // Normalize infinite length vector
    [Test]
    public async Task Vector2DNormalizeTest2()
    {
        Vector2D a = new Vector2D(double.MaxValue, double.MaxValue);
        Vector2D actual = Vector2D.Normalize(a);
        Vector2D expected = new Vector2D(0, 0);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator - (Vector2D)
    [Test]
    public async Task Vector2DUnaryNegationTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);

        Vector2D expected = new Vector2D(-1.0, -2.0);
        Vector2D actual;

        actual = -a;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator - (Vector2D)
    // Negate test with special double value
    [Test]
    public async Task Vector2DUnaryNegationTest1()
    {
        Vector2D a = new Vector2D(double.PositiveInfinity, double.NegativeInfinity);

        Vector2D actual = -a;

        await Assert.That(actual.X).IsNegativeInfinity();
        await Assert.That(actual.Y).IsPositiveInfinity();
    }

    // A test for operator - (Vector2D)
    // Negate test with special double value
    [Test]
    public async Task Vector2DUnaryNegationTest2()
    {
        Vector2D a = new Vector2D(double.NaN, 0.0);
        Vector2D actual = -a;

        await Assert.That(actual.X).IsNaN();
        await Assert.That(actual.Y).IsEqualTo(0.0);
    }

    // A test for operator - (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DSubtractionTest()
    {
        Vector2D a = new Vector2D(1.0, 3.0);
        Vector2D b = new Vector2D(2.0, 1.5);

        Vector2D expected = new Vector2D(-1.0, 1.5);
        Vector2D actual;

        actual = a - b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Vector2D, double)
    [Test]
    public async Task Vector2DMultiplyOperatorTest()
    {
        Vector2D a = new Vector2D(2.0, 3.0);
        const double factor = 2.0;

        Vector2D expected = new Vector2D(4.0, 6.0);
        Vector2D actual;

        actual = a * factor;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (double, Vector2D)
    [Test]
    public async Task Vector2DMultiplyOperatorTest2()
    {
        Vector2D a = new Vector2D(2.0, 3.0);
        const double factor = 2.0;

        Vector2D expected = new Vector2D(4.0, 6.0);
        Vector2D actual;

        actual = factor * a;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DMultiplyOperatorTest3()
    {
        Vector2D a = new Vector2D(2.0, 3.0);
        Vector2D b = new Vector2D(4.0, 5.0);

        Vector2D expected = new Vector2D(8.0, 15.0);
        Vector2D actual;

        actual = a * b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector2D, double)
    [Test]
    public async Task Vector2DivisionTest()
    {
        Vector2D a = new Vector2D(2.0, 3.0);

        double div = 2.0;

        Vector2D expected = new Vector2D(1.0, 1.5);
        Vector2D actual;

        actual = a / div;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DivisionTest1()
    {
        Vector2D a = new Vector2D(2.0, 3.0);
        Vector2D b = new Vector2D(4.0, 5.0);

        Vector2D expected = new Vector2D(2.0 / 4.0, 3.0 / 5.0);
        Vector2D actual;

        actual = a / b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector2D, double)
    // Divide by zero
    [Test]
    public async Task Vector2DivisionTest2()
    {
        Vector2D a = new Vector2D(-2.0, 3.0);

        double div = 0.0;

        Vector2D actual = a / div;

        await Assert.That(actual.X).IsNegativeInfinity();
        await Assert.That(actual.Y).IsPositiveInfinity();
    }

    // A test for operator / (Vector2D, Vector2D)
    // Divide by zero
    [Test]
    public async Task Vector2DivisionTest3()
    {
        Vector2D a = new Vector2D(0.047, -3.0);
        Vector2D b = new Vector2D();

        Vector2D actual = a / b;

        await Assert.That(actual.X).IsInfinity();
        await Assert.That(actual.Y).IsInfinity();
    }

    // A test for operator + (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DAdditionTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(3.0, 4.0);

        Vector2D expected = new Vector2D(4.0, 6.0);
        Vector2D actual;

        actual = a + b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Vector2D (double, double)
    [Test]
    public async Task Vector2DConstructorTest()
    {
        double x = 1.0;
        double y = 2.0;

        Vector2D target = new Vector2D(x, y);
        await Assert.That(target.X).IsEqualTo(x);
        await Assert.That(target.Y).IsEqualTo(y);
    }

    // A test for Vector2D ()
    // Constructor with no parameter
    [Test]
    public async Task Vector2DConstructorTest2()
    {
        Vector2D target = new Vector2D();
        await Assert.That(target.X).IsEqualTo(0.0);
        await Assert.That(target.Y).IsEqualTo(0.0);
    }

    // A test for Vector2D (double, double)
    // Constructor with special doubleing values
    [Test]
    public async Task Vector2DConstructorTest3()
    {
        Vector2D target = new Vector2D(double.NaN, double.MaxValue);
        await Assert.That(target.X).IsNaN();
        await Assert.That(target.Y).IsEqualTo(double.MaxValue);
    }

    // A test for Vector2D (double)
    [Test]
    public async Task Vector2DConstructorTest4()
    {
        double value = 1.0;
        Vector2D target = new Vector2D(value);

        Vector2D expected = new Vector2D(value, value);
        await Assert.That(target).IsEqualTo(expected);

        value = 2.0;
        target = new Vector2D(value);
        expected = new Vector2D(value, value);
        await Assert.That(target).IsEqualTo(expected);
    }

    // A test for Vector2D (ReadOnlySpan<double>)
    [Test]
    public async Task Vector2DConstructorTest5()
    {
        double value = 1.0;
        Vector2D target = new Vector2D(new[] { value, value });
        Vector2D expected = new Vector2D(value);

        await Assert.That(target).IsEqualTo(expected);
        await Assert.That(() => new Vector2D(new double[1])).Throws<ArgumentOutOfRangeException>();
    }

    // A test for Add (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DAddTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(5.0, 6.0);

        Vector2D expected = new Vector2D(6.0, 8.0);
        Vector2D actual;

        actual = Vector2D.Add(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Divide (Vector2D, double)
    [Test]
    public async Task Vector2DivideTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        double div = 2.0;
        Vector2D expected = new Vector2D(0.5, 1.0);
        Vector2D actual;
        actual = Vector2D.Divide(a, div);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Divide (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DivideTest1()
    {
        Vector2D a = new Vector2D(1.0, 6.0);
        Vector2D b = new Vector2D(5.0, 2.0);

        Vector2D expected = new Vector2D(1.0 / 5.0, 6.0 / 2.0);
        Vector2D actual;

        actual = Vector2D.Divide(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Equals (object)
    [Test]
    public async Task Vector2DEqualsTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(1.0, 2.0);

        // case 1: compare between same values
        object obj = b;

        bool expected = true;
        bool actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.X = 10.0;
        obj = b;
        expected = false;
        actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);

        // case 3: compare between different types.
        obj = new QuaternionD();
        expected = false;
        actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);

        // case 3: compare against null.
        obj = null!;
        expected = false;
        actual = a.Equals(obj);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Vector2D, double)
    [Test]
    public async Task Vector2DMultiplyTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        const double factor = 2.0;
        Vector2D expected = new Vector2D(2.0, 4.0);
        Vector2D actual = Vector2D.Multiply(a, factor);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (double, Vector2D)
    [Test]
    public async Task Vector2DMultiplyTest2()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        const double factor = 2.0;
        Vector2D expected = new Vector2D(2.0, 4.0);
        Vector2D actual = Vector2D.Multiply(factor, a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DMultiplyTest3()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(5.0, 6.0);

        Vector2D expected = new Vector2D(5.0, 12.0);
        Vector2D actual;

        actual = Vector2D.Multiply(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Negate (Vector2D)
    [Test]
    public async Task Vector2DNegateTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);

        Vector2D expected = new Vector2D(-1.0, -2.0);
        Vector2D actual;

        actual = Vector2D.Negate(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator != (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DInequalityTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(1.0, 2.0);

        // case 1: compare between same values
        bool expected = false;
        bool actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.X = 10.0;
        expected = true;
        actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator == (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DEqualityTest()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(1.0, 2.0);

        // case 1: compare between same values
        bool expected = true;
        bool actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.X = 10.0;
        expected = false;
        actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Subtract (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DSubtractTest()
    {
        Vector2D a = new Vector2D(1.0, 6.0);
        Vector2D b = new Vector2D(5.0, 2.0);

        Vector2D expected = new Vector2D(-4.0, 4.0);
        Vector2D actual;

        actual = Vector2D.Subtract(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for UnitX
    [Test]
    public async Task Vector2DUnitXTest()
    {
        Vector2D val = new Vector2D(1.0, 0.0);
        await Assert.That(Vector2D.UnitX).IsEqualTo(val);
    }

    // A test for UnitY
    [Test]
    public async Task Vector2DUnitYTest()
    {
        Vector2D val = new Vector2D(0.0, 1.0);
        await Assert.That(Vector2D.UnitY).IsEqualTo(val);
    }

    // A test for One
    [Test]
    public async Task Vector2DOneTest()
    {
        Vector2D val = new Vector2D(1.0, 1.0);
        await Assert.That(Vector2D.One).IsEqualTo(val);
    }

    // A test for Zero
    [Test]
    public async Task Vector2DZeroTest()
    {
        Vector2D val = new Vector2D(0.0, 0.0);
        await Assert.That(Vector2D.Zero).IsEqualTo(val);
    }

    // A test for Equals (Vector2D)
    [Test]
    public async Task Vector2DEqualsTest1()
    {
        Vector2D a = new Vector2D(1.0, 2.0);
        Vector2D b = new Vector2D(1.0, 2.0);

        // case 1: compare between same values
        bool expected = true;
        bool actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.X = 10.0;
        expected = false;
        actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Vector2D comparison involving NaN values
    [Test]
    public async Task Vector2DEqualsNaNTest()
    {
        Vector2D a = new Vector2D(double.NaN, 0);
        Vector2D b = new Vector2D(0, double.NaN);

        await Assert.That(a).IsNotEqualTo(Vector2D.Zero);
        await Assert.That(b).IsNotEqualTo(Vector2D.Zero);
    }

    // A test for Reflect (Vector2D, Vector2D)
    [Test]
    public async Task Vector2DReflectTest()
    {
        Vector2D a = Vector2D.Normalize(new Vector2D(1.0, 1.0));

        // Reflect on XZ plane.
        Vector2D n = new Vector2D(0.0, 1.0);
        Vector2D expected = new Vector2D(a.X, -a.Y);
        Vector2D actual = Vector2D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected);

        // Reflect on XY plane.
        n = new Vector2D(0.0, 0.0);
        expected = new Vector2D(a.X, a.Y);
        actual = Vector2D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected);

        // Reflect on YZ plane.
        n = new Vector2D(1.0, 0.0);
        expected = new Vector2D(-a.X, a.Y);
        actual = Vector2D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Reflect (Vector2D, Vector2D)
    // Reflection when normal and source are the same
    [Test]
    public async Task Vector2DReflectTest1()
    {
        Vector2D n = new Vector2D(0.45, 1.28);
        n = Vector2D.Normalize(n);
        Vector2D a = n;

        Vector2D expected = -n;
        Vector2D actual = Vector2D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected).Within(0.00000000001);
    }

    // A test for Reflect (Vector2D, Vector2D)
    // Reflection when normal and source are negation
    [Test]
    public async Task Vector2DReflectTest2()
    {
        Vector2D n = new Vector2D(0.45, 1.28);
        n = Vector2D.Normalize(n);
        Vector2D a = -n;

        Vector2D expected = n;
        Vector2D actual = Vector2D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected).Within(0.00000000001);
    }

    [Test]
    public async Task Vector2DAbsTest()
    {
        Vector2D v1 = new Vector2D(-2.5, 2.0);
        Vector2D v3 = Vector2D.Abs(new Vector2D(0.0, double.NegativeInfinity));
        Vector2D v = Vector2D.Abs(v1);
        await Assert.That(v.X).IsEqualTo(2.5);
        await Assert.That(v.Y).IsEqualTo(2.0);
        await Assert.That(v3.X).IsEqualTo(0.0);
        await Assert.That(v3.Y).IsPositiveInfinity();
    }

    [Test]
    public async Task Vector2DSqrtTest()
    {
        Vector2D v1 = new Vector2D(-2.5, 2.0);
        Vector2D v2 = new Vector2D(5.5, 4.5);
        await Assert.That((int)Vector2D.SquareRoot(v2).X).IsEqualTo(2);
        await Assert.That((int)Vector2D.SquareRoot(v2).Y).IsEqualTo(2);
        await Assert.That(Vector2D.SquareRoot(v1).X).IsNaN();
    }

    // A test to make sure these types are blittable directly into GPU buffer memory layouts
    [Test]
    public async Task Vector2DSizeofTest()
    {
        int sizeofVector2D;
        int sizeofVector2D2;
        int sizeofVector2DPlusDouble;
        int sizeofVector2DPlusDouble2;

        unsafe
        {
            sizeofVector2D = sizeof(Vector2D);
            sizeofVector2D2 = sizeof(Vector2D_2x);
            sizeofVector2DPlusDouble = sizeof(Vector2DPlusDouble);
            sizeofVector2DPlusDouble2 = sizeof(Vector2DPlusDouble_2x);
        }

        await Assert.That(sizeofVector2D).IsEqualTo(16);
        await Assert.That(sizeofVector2D2).IsEqualTo(32);
        await Assert.That(sizeofVector2DPlusDouble).IsEqualTo(24);
        await Assert.That(sizeofVector2DPlusDouble2).IsEqualTo(48);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector2D_2x
    {
        private Vector2D _a;
        private Vector2D _b;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector2DPlusDouble
    {
        private Vector2D _v;
        private double _f;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector2DPlusDouble_2x
    {
        private Vector2DPlusDouble _a;
        private Vector2DPlusDouble _b;
    }

    [Test]
    public async Task SetFieldsTest()
    {
        Vector2D v3 = new Vector2D(4, 5);
        v3.X = 1.0;
        v3.Y = 2.0;
        await Assert.That(v3.X).IsEqualTo(1.0);
        await Assert.That(v3.Y).IsEqualTo(2.0);
        Vector2D v4 = v3;
        v4.Y = 0.5;
        await Assert.That(v4.X).IsEqualTo(1.0);
        await Assert.That(v4.Y).IsEqualTo(0.5);
        await Assert.That(v3.Y).IsEqualTo(2.0);
    }

    [Test]
    public async Task EmbeddedVectorSetFields()
    {
        EmbeddedVectorObject evo = new EmbeddedVectorObject();
        evo.FieldVector.X = 5.0;
        evo.FieldVector.Y = 5.0;
        await Assert.That(evo.FieldVector.X).IsEqualTo(5.0);
        await Assert.That(evo.FieldVector.Y).IsEqualTo(5.0);
    }

    private class EmbeddedVectorObject
    {
        public Vector2D FieldVector;
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.CosDouble))]
    public async Task CosDoubleTest(double value, double expectedResult, double variance)
    {
        Vector2D actualResult = Vector2D.Cos(Vector2D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Create(variance));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.ExpDouble))]
    public async Task ExpDoubleTest(double value, double expectedResult, double variance)
    {
        Vector2D actualResult = Vector2D.Exp(Vector2D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.LogDouble))]
    public async Task LogDoubleTest(double value, double expectedResult, double variance)
    {
        Vector2D actualResult = Vector2D.Log(Vector2D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.Log2Double))]
    public async Task Log2DoubleTest(double value, double expectedResult, double variance)
    {
        Vector2D actualResult = Vector2D.Log2(Vector2D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.FusedMultiplyAddDouble))]
    public async Task FusedMultiplyAddDoubleTest(double left, double right, double addend, double expectedResult)
    {
        await Assert.That(Vector2D.FusedMultiplyAdd(Vector2D.Create(left), Vector2D.Create(right), Vector2D.Create(addend))).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
        await Assert.That(Vector2D.MultiplyAddEstimate(Vector2D.Create(left), Vector2D.Create(right), Vector2D.Create(addend))).IsEqualTo(Vector2D.Create(double.MultiplyAddEstimate(left, right, addend))).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.ClampDouble))]
    public async Task ClampDoubleTest(double x, double min, double max, double expectedResult)
    {
        Vector2D actualResult = Vector2D.Clamp(Vector2D.Create(x), Vector2D.Create(min), Vector2D.Create(max));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.CopySignDouble))]
    public async Task CopySignDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.CopySign(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.DegreesToRadiansDouble))]
    public async Task DegreesToRadiansDoubleTest(double value, double expectedResult, double variance)
    {
        await Assert.That(Vector2D.DegreesToRadians(Vector2D.Create(-value))).IsEqualTo(Vector2D.Create(-expectedResult)).Within(Vector2D.Create(variance));
        await Assert.That(Vector2D.DegreesToRadians(Vector2D.Create(+value))).IsEqualTo(Vector2D.Create(+expectedResult)).Within(Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.HypotDouble))]
    public async Task HypotDoubleTest(double x, double y, double expectedResult, double variance)
    {
        await Assert.That(Vector2D.Hypot(Vector2D.Create(-x), Vector2D.Create(-y))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
        await Assert.That(Vector2D.Hypot(Vector2D.Create(-x), Vector2D.Create(+y))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
        await Assert.That(Vector2D.Hypot(Vector2D.Create(+x), Vector2D.Create(-y))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
        await Assert.That(Vector2D.Hypot(Vector2D.Create(+x), Vector2D.Create(+y))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
        await Assert.That(Vector2D.Hypot(Vector2D.Create(-y), Vector2D.Create(-x))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
        await Assert.That(Vector2D.Hypot(Vector2D.Create(-y), Vector2D.Create(+x))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
        await Assert.That(Vector2D.Hypot(Vector2D.Create(+y), Vector2D.Create(-x))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
        await Assert.That(Vector2D.Hypot(Vector2D.Create(+y), Vector2D.Create(+x))).IsEqualTo(Vector2D.Create(expectedResult)).Within( Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.LerpDouble))]
    public async Task LerpDoubleTest(double x, double y, double amount, double expectedResult)
    {
        await Assert.That(Vector2D.Lerp(Vector2D.Create(+x), Vector2D.Create(+y), Vector2D.Create(amount))).IsEqualTo(Vector2D.Create(+expectedResult)).Within(Vector2D.Zero);
        await Assert.That(Vector2D.Lerp(Vector2D.Create(-x), Vector2D.Create(-y), Vector2D.Create(amount))).IsEqualTo(Vector2D.Create((expectedResult == 0.0) ? expectedResult : -expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxDouble))]
    public async Task MaxDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.Max(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxMagnitudeDouble))]
    public async Task MaxMagnitudeDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.MaxMagnitude(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxMagnitudeNumberDouble))]
    public async Task MaxMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.MaxMagnitudeNumber(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxNumberDouble))]
    public async Task MaxNumberDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.MaxNumber(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinDouble))]
    public async Task MinDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.Min(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinMagnitudeDouble))]
    public async Task MinMagnitudeDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.MinMagnitude(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinMagnitudeNumberDouble))]
    public async Task MinMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.MinMagnitudeNumber(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinNumberDouble))]
    public async Task MinNumberDoubleTest(double x, double y, double expectedResult)
    {
        Vector2D actualResult = Vector2D.MinNumber(Vector2D.Create(x), Vector2D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RadiansToDegreesDouble))]
    public async Task RadiansToDegreesDoubleTest(double value, double expectedResult, double variance)
    {
        await Assert.That(Vector2D.RadiansToDegrees(Vector2D.Create(-value))).IsEqualTo(Vector2D.Create(-expectedResult)).Within(Vector2D.Create(variance));
        await Assert.That(Vector2D.RadiansToDegrees(Vector2D.Create(+value))).IsEqualTo(Vector2D.Create(+expectedResult)).Within(Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundDouble))]
    public async Task RoundDoubleTest(double value, double expectedResult)
    {
        Vector2D actualResult = Vector2D.Round(Vector2D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundAwayFromZeroDouble))]
    public async Task RoundAwayFromZeroDoubleTest(double value, double expectedResult)
    {
        Vector2D actualResult = Vector2D.Round(Vector2D.Create(value), MidpointRounding.AwayFromZero);
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundToEvenDouble))]
    public async Task RoundToEvenDoubleTest(double value, double expectedResult)
    {
        Vector2D actualResult = Vector2D.Round(Vector2D.Create(value), MidpointRounding.ToEven);
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.SinDouble))]
    public async Task SinDoubleTest(double value, double expectedResult, double variance)
    {
        Vector2D actualResult = Vector2D.Sin(Vector2D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Create(variance));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.SinCosDouble))]
    public async Task SinCosDoubleTest(double value, double expectedResultSin, double expectedResultCos, double allowedVarianceSin, double allowedVarianceCos)
    {
        (Vector2D resultSin, Vector2D resultCos) = Vector2D.SinCos(Vector2D.Create(value));
        await Assert.That(resultSin).IsEqualTo(Vector2D.Create(expectedResultSin)).Within(Vector2D.Create(allowedVarianceSin));
        await Assert.That(resultCos).IsEqualTo(Vector2D.Create(expectedResultCos)).Within(Vector2D.Create(allowedVarianceCos));
    }
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.TruncateDouble))]
    public async Task TruncateDoubleTest(double value, double expectedResult)
    {
        Vector2D actualResult = Vector2D.Truncate(Vector2D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector2D.Create(expectedResult)).Within(Vector2D.Zero);
    }
    
#if NET10_OR_GREATER
    [Test]
    public async Task AllAnyNoneTest()
    {
        await Test(3, 2);
    
        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task  Test(double value1, double value2)
        {
            var input1 = Vector2D.Create(value1);
            var input2 = Vector2D.Create(value2);
    
            await Assert.That(Vector2D.All(input1, value1)).IsTrue();
            await Assert.That(Vector2D.All(input2, value2)).IsTrue();
            await Assert.That(Vector2D.All(input1.WithElement(0, value2), value1)).IsFalse();
            await Assert.That(Vector2D.All(input2.WithElement(0, value1), value2)).IsFalse();
            await Assert.That(Vector2D.All(input1, value2)).IsFalse();
            await Assert.That(Vector2D.All(input2, value1)).IsFalse();
            await Assert.That(Vector2D.All(input1.WithElement(0, value2), value2)).IsFalse();
            await Assert.That(Vector2D.All(input2.WithElement(0, value1), value1)).IsFalse();
    
            await Assert.That(Vector2D.Any(input1, value1)).IsTrue();
            await Assert.That(Vector2D.Any(input2, value2)).IsTrue();
            await Assert.That(Vector2D.Any(input1.WithElement(0, value2), value1)).IsTrue();
            await Assert.That(Vector2D.Any(input2.WithElement(0, value1), value2)).IsTrue();
            await Assert.That(Vector2D.Any(input1, value2)).IsFalse();
            await Assert.That(Vector2D.Any(input2, value1)).IsFalse();
            await Assert.That(Vector2D.Any(input1.WithElement(0, value2), value2)).IsTrue();
            await Assert.That(Vector2D.Any(input2.WithElement(0, value1), value1)).IsTrue();
    
            await Assert.That(Vector2D.None(input1, value1)).IsFalse();
            await Assert.That(Vector2D.None(input2, value2)).IsFalse();
            await Assert.That(Vector2D.None(input1.WithElement(0, value2), value1)).IsFalse();
            await Assert.That(Vector2D.None(input2.WithElement(0, value1), value2)).IsFalse();
            await Assert.That(Vector2D.None(input1, value2)).IsTrue();
            await Assert.That(Vector2D.None(input2, value1)).IsTrue();
            await Assert.That(Vector2D.None(input1.WithElement(0, value2), value2)).IsFalse();
            await Assert.That(Vector2D.None(input2.WithElement(0, value1), value1)).IsFalse();
        }
    }
    
    [Test]
    public async Task AllAnyNoneTest_AllBitsSet()
    {
        await Test(BitConverter.Int64BitsToDouble(-1));
    
        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value)
        {
            var input = Vector2D.Create(value);
    
            await Assert.That(Vector2D.All(input, value)).IsFalse();
            await Assert.That(Vector2D.Any(input, value)).IsFalse();
            await Assert.That(Vector2D.None(input, value)).IsTrue();
        }
    }
    
    [Test]
    public async Task AllAnyNoneWhereAllBitsSetTest()
    {
        await Test(BitConverter.Int64BitsToDouble(-1), 2);
    
        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double allBitsSet, double value2)
        {
            var input1 = Vector2D.Create(allBitsSet);
            var input2 = Vector2D.Create(value2);
    
            await Assert.That(Vector2D.AllWhereAllBitsSet(input1)).IsTrue();
            await Assert.That(Vector2D.AllWhereAllBitsSet(input2)).IsFalse();
            await Assert.That(Vector2D.AllWhereAllBitsSet(input1.WithElement(0, value2))).IsFalse();
            await Assert.That(Vector2D.AllWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsFalse();
    
            await Assert.That(Vector2D.AnyWhereAllBitsSet(input1)).IsTrue();
            await Assert.That(Vector2D.AnyWhereAllBitsSet(input2)).IsFalse();
            await Assert.That(Vector2D.AnyWhereAllBitsSet(input1.WithElement(0, value2))).IsTrue();
            await  Assert.That(Vector2D.AnyWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsTrue();
    
            await Assert.That(Vector2D.NoneWhereAllBitsSet(input1)).IsFalse();
            await Assert.That(Vector2D.NoneWhereAllBitsSet(input2)).IsTrue();
            await Assert.That(Vector2D.NoneWhereAllBitsSet(input1.WithElement(0, value2))).IsFalse();
            await Assert.That(Vector2D.NoneWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsFalse();
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfDoubleTest()
    {
        await Test(3, 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value1, double value2)
        {
            var input1 = Vector2D.Create(value1);
            var input2 = Vector2D.Create(value2);

            await Assert.That(Vector2D.Count(input1, value1)).IsEqualTo(ElementCount);
            await Assert.That(Vector2D.Count(input2, value2)).IsEqualTo(ElementCount);
            await Assert.That(Vector2D.Count(input1.WithElement(0, value2), value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.Count(input2.WithElement(0, value1), value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.Count(input1, value2)).IsEqualTo(0);
            await Assert.That(Vector2D.Count(input2, value1)).IsEqualTo(0);
            await Assert.That(Vector2D.Count(input1.WithElement(0, value2), value2)).IsEqualTo(1);
            await Assert.That(Vector2D.Count(input2.WithElement(0, value1), value1)).IsEqualTo(1);

            await Assert.That(Vector2D.IndexOf(input1, value1)).IsEqualTo(0);
            await Assert.That(Vector2D.IndexOf(input2, value2)).IsEqualTo(0);
            await Assert.That(Vector2D.IndexOf(input1.WithElement(0, value2), value1)).IsEqualTo(1);
            await Assert.That(Vector2D.IndexOf(input2.WithElement(0, value1), value2)).IsEqualTo(1);
            await Assert.That(Vector2D.IndexOf(input1, value2)).IsEqualTo(-1);
            await Assert.That(Vector2D.IndexOf(input2, value1)).IsEqualTo(-1);
            await Assert.That(Vector2D.IndexOf(input1.WithElement(0, value2), value2)).IsEqualTo(0);
            await Assert.That(Vector2D.IndexOf(input2.WithElement(0, value1), value1)).IsEqualTo(0);

            await Assert.That(Vector2D.LastIndexOf(input1, value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.LastIndexOf(input2, value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.LastIndexOf(input1.WithElement(0, value2), value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.LastIndexOf(input2.WithElement(0, value1), value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.LastIndexOf(input1, value2)).IsEqualTo(-1);
            await Assert.That(Vector2D.LastIndexOf(input2, value1)).IsEqualTo(-1);
            await Assert.That(Vector2D.LastIndexOf(input1.WithElement(0, value2), value2)).IsEqualTo(0);
            await Assert.That(Vector2D.LastIndexOf(input2.WithElement(0, value1), value1)).IsEqualTo(0);
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfDoubleTest_AllBitsSet()
    {
        await Test(BitConverter.Int64BitsToDouble(-1));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value)
        {
            var input = Vector2D.Create(value);

            await Assert.That(Vector2D.Count(input, value)).IsEqualTo(0);
            await Assert.That(Vector2D.IndexOf(input, value)).IsEqualTo(-1);
            await Assert.That(Vector2D.LastIndexOf(input, value)).IsEqualTo(-1);
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfWhereAllBitsSetDoubleTest()
    {
        await Test(BitConverter.Int64BitsToDouble(-1), 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double allBitsSet, double value2)
        {
            var input1 = Vector2D.Create(allBitsSet);
            var input2 = Vector2D.Create(value2);

            await Assert.That(Vector2D.CountWhereAllBitsSet(input1)).IsEqualTo(ElementCount);
            await Assert.That(Vector2D.CountWhereAllBitsSet(input2)).IsEqualTo(0);
            await Assert.That(Vector2D.CountWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.CountWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(1);

            await Assert.That(Vector2D.IndexOfWhereAllBitsSet(input1)).IsEqualTo(0);
            await Assert.That(Vector2D.IndexOfWhereAllBitsSet(input2)).IsEqualTo(-1);
            await Assert.That(Vector2D.IndexOfWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(1);
            await Assert.That(Vector2D.IndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(0);

            await Assert.That(Vector2D.LastIndexOfWhereAllBitsSet(input1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.LastIndexOfWhereAllBitsSet(input2)).IsEqualTo(-1);
            await Assert.That(Vector2D.LastIndexOfWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector2D.LastIndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(0);
        }
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsEvenIntegerTest(double value) => await Assert.That(Vector2D.IsEvenInteger(Vector2D.Create(value))).IsEqualTo(double.IsEvenInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsFiniteTest(double value) => await Assert.That(Vector2D.IsFinite(Vector2D.Create(value))).IsEqualTo(double.IsFinite(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsInfinityTest(double value) => await Assert.That(Vector2D.IsInfinity(Vector2D.Create(value))).IsEqualTo(double.IsInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsIntegerTest(double value) => await Assert.That(Vector2D.IsInteger(Vector2D.Create(value))).IsEqualTo(double.IsInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNaNTest(double value) => await Assert.That(Vector2D.IsNaN(Vector2D.Create(value))).IsEqualTo(double.IsNaN(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNegativeTest(double value) => await Assert.That(Vector2D.IsNegative(Vector2D.Create(value))).IsEqualTo(double.IsNegative(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNegativeInfinityTest(double value) => await Assert.That(Vector2D.IsNegativeInfinity(Vector2D.Create(value))).IsEqualTo(double.IsNegativeInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNormalTest(double value) => await Assert.That(Vector2D.IsNormal(Vector2D.Create(value))).IsEqualTo(double.IsNormal(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsOddIntegerTest(double value) => await Assert.That(Vector2D.IsOddInteger(Vector2D.Create(value))).IsEqualTo(double.IsOddInteger(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsPositiveTest(double value) => await Assert.That(Vector2D.IsPositive(Vector2D.Create(value))).IsEqualTo(double.IsPositive(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsPositiveInfinityTest(double value) => await Assert.That(Vector2D.IsPositiveInfinity(Vector2D.Create(value))).IsEqualTo(double.IsPositiveInfinity(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsSubnormalTest(double value) => await Assert.That(Vector2D.IsSubnormal(Vector2D.Create(value))).IsEqualTo(double.IsSubnormal(value) ? Vector2D.AllBitsSet : Vector2D.Zero);
    
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsZeroDoubleTest(double value) => await Assert.That(Vector2D.IsZero(Vector2D.Create(value))).IsEqualTo((value == 0) ? Vector2D.AllBitsSet : Vector2D.Zero);

    [Test]
    public async Task AllBitsSetTest()
    {
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector2D.AllBitsSet.X)).IsEqualTo(-1L);
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector2D.AllBitsSet.Y)).IsEqualTo(-1L);
    }

    [Test]
    public async Task ConditionalSelectTest()
    {
        await Test(Vector2D.Create(1, 2), Vector2D.AllBitsSet, Vector2D.Create(1, 2), Vector2D.Create(5, 6));
        await Test(Vector2D.Create(5, 6), Vector2D.Zero, Vector2D.Create(1, 2), Vector2D.Create(5, 6));
        await Test(Vector2D.Create(1, 6), Vector256.Create(-1, 0, -1, 0).AsDouble().AsVector2D(), Vector2D.Create(1, 2), Vector2D.Create(5, 6));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(Vector2D expectedResult, Vector2D condition, Vector2D left, Vector2D right)
        {
            await Assert.That(Vector2D.ConditionalSelect(condition, left, right)).IsEqualTo(expectedResult);
        }
    }
#endif

    [Test]
    [Arguments(+0.0, +0.0, 0b00)]
    [Arguments(-0.0, +1.0, 0b01)]
    [Arguments(-0.0, -0.0, 0b11)]
    public async Task ExtractMostSignificantBitsTest(double x, double y, uint expectedResult)
    {
        await Assert.That(Vector2D.Create(x, y).ExtractMostSignificantBits()).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(1.0, 2.0)]
    [Arguments(5.0, 6.0)]
    public async Task GetElementTest(double x, double y)
    {
        await Assert.That(Vector2D.Create(x, y).GetElement(0)).IsEqualTo(x);
        await Assert.That(Vector2D.Create(x, y).GetElement(1)).IsEqualTo(y);
    }

    [Test]
    [Arguments(1.0, 2.0)]
    [Arguments(5.0, 6.0)]
    public async Task ShuffleTest(double x, double y)
    {
        await Assert.That(Vector2D.Shuffle(Vector2D.Create(x, y), 1, 0)).IsEqualTo(Vector2D.Create(y, x));
        await Assert.That(Vector2D.Shuffle(Vector2D.Create(x, y), 0, 0)).IsEqualTo(Vector2D.Create(x, x));
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0)]
    [Arguments(5.0, 6.0, 11.0)]
    public async Task SumTest(double x, double y, double expectedResult)
    {
        await Assert.That(Vector2D.Sum(Vector2D.Create(x, y))).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(1.0, 2.0)]
    [Arguments(5.0, 6.0)]
    public async Task ToScalarTest(double x, double y)
    {
        await Assert.That(Vector2D.Create(x, y).ToScalar()).IsEqualTo(x);
    }

    [Test]
    [Arguments(1.0, 2.0)]
    [Arguments(5.0, 6.0)]
    public async Task WithElementTest(double x, double y)
    {
        var vector = Vector2D.Create(10);

        await Assert.That(vector.X).IsEqualTo(10);
        await Assert.That(vector.Y).IsEqualTo(10);

        vector = vector.WithElement(0, x);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(10);

        vector = vector.WithElement(1, y);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
    }

    [Test]
    [Arguments(1.0, 2.0)]
    [Arguments(5.0, 6.0)]
    public async Task AsVector3Test(double x, double y)
    {
        var vector = Vector2D.Create(x, y).AsVector3D();

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
        await Assert.That(vector.Z).IsEqualTo(0);
    }

    [Test]
    [Arguments(1.0, 2.0)]
    [Arguments(5.0, 6.0)]
    public async Task AsVector3UnsafeTest(double x, double y)
    {
        var vector = Vector2D.Create(x, y).AsVector3DUnsafe();

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
    }

    [Test]
    public async Task CreateScalarTest()
    {
        var vector = Vector2D.CreateScalar(double.Pi);

        await Assert.That(vector.X).IsEqualTo(double.Pi);
        await Assert.That(vector.Y).IsEqualTo(0D);

        vector = Vector2D.CreateScalar(double.E);

        await Assert.That(vector.X).IsEqualTo(double.E);
        await Assert.That(vector.Y).IsEqualTo(0D);
    }

    [Test]
    public async Task CreateScalarUnsafeTest()
    {
        var vector = Vector2D.CreateScalarUnsafe(double.Pi);
        await Assert.That(vector.X).IsEqualTo(double.Pi);

        vector = Vector2D.CreateScalarUnsafe(double.E);
        await Assert.That(vector.X).IsEqualTo(double.E);
    }
}