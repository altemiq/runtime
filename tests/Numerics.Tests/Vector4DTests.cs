namespace Altemiq.Numerics;

using System.Globalization;
using System.Runtime.InteropServices;

//[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
public sealed class Vector4DTests
{
#if NET10_OR_GREATER
    private const int ElementCount = 4;
#endif

    [Test]
    public async Task Vector4DMarshalSizeTest()
    {
        await Assert.That(Marshal.SizeOf<Vector4D>()).IsEqualTo(32);
        await Assert.That(Marshal.SizeOf<Vector4D>(new())).IsEqualTo(32);
    }

    [Test]
    [Arguments(0.0, 1.0, 0.0, 1.0)]
    [Arguments(1.0, 0.0, 1.0, 0.0)]
    [Arguments(3.1434343, 1.1234123, 0.1234123, -0.1234123)]
    [Arguments(1.0000001, 0.0000001, 2.0000001, 0.0000002)]
    public async Task Vector4DIndexerGetTest(double x, double y, double z, double w)
    {
        var vector = new Vector4D(x, y, z, w);

        await Assert.That(vector[0]).IsEqualTo(x);
        await Assert.That(vector[1]).IsEqualTo(y);
        await Assert.That(vector[2]).IsEqualTo(z);
        await Assert.That(vector[3]).IsEqualTo(w);
    }

    [Test]
    [Arguments(0.0, 1.0, 0.0, 1.0)]
    [Arguments(1.0, 0.0, 1.0, 0.0)]
    [Arguments(3.1434343, 1.1234123, 0.1234123, -0.1234123)]
    [Arguments(1.0000001, 0.0000001, 2.0000001, 0.0000002)]
    public async Task Vector4DIndexerSetTest(double x, double y, double z, double w)
    {
        var vector = new Vector4D(0.0, 0.0, 0.0, 0.0)
        {
            [0] = x,
            [1] = y,
            [2] = z,
            [3] = w,
        };

        await Assert.That(vector[0]).IsEqualTo(x);
        await Assert.That(vector[1]).IsEqualTo(y);
        await Assert.That(vector[2]).IsEqualTo(z);
        await Assert.That(vector[3]).IsEqualTo(w);
    }

    [Test]
    public async Task Vector4DCopyToTest()
    {
        var v1 = new Vector4D(2.5, 2.0, 3.0, 3.3);

        var a = new double[5];
        var b = new double[4];

        await Assert.That(() => v1.CopyTo(null!, 0)).Throws<NullReferenceException>();
        await Assert.That(() => v1.CopyTo(a, -1)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => v1.CopyTo(a, a.Length)).Throws<ArgumentOutOfRangeException>();
        await Assert.That(() => v1.CopyTo(a, a.Length - 2)).Throws<ArgumentException>();

        v1.CopyTo(a, 1);
        v1.CopyTo(b);
        await Assert.That(a[0]).IsEqualTo(0.0);
        await Assert.That(a[1]).IsEqualTo(2.5);
        await Assert.That(a[2]).IsEqualTo(2.0);
        await Assert.That(a[3]).IsEqualTo(3.0);
        await Assert.That(a[4]).IsEqualTo(3.3);
        await Assert.That(b[0]).IsEqualTo(2.5);
        await Assert.That(b[1]).IsEqualTo(2.0);
        await Assert.That(b[2]).IsEqualTo(3.0);
        await Assert.That(b[3]).IsEqualTo(3.3);
    }

    [Test]
    public async Task Vector4DCopyToSpanTest()
    {
        var vector = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var destination = new double[4];

        await Assert.That(() => vector.CopyTo(new Span<double>(new double[3]))).Throws<ArgumentException>();
        vector.CopyTo(destination.AsSpan());

        await Assert.That(vector.X).IsEqualTo(1.0);
        await Assert.That(vector.Y).IsEqualTo(2.0);
        await Assert.That(vector.Z).IsEqualTo(3.0);
        await Assert.That(vector.W).IsEqualTo(4.0);
        await Assert.That(destination[0]).IsEqualTo(vector.X);
        await Assert.That(destination[1]).IsEqualTo(vector.Y);
        await Assert.That(destination[2]).IsEqualTo(vector.Z);
        await Assert.That(destination[3]).IsEqualTo(vector.W);
    }

    [Test]
    public async Task Vector4DTryCopyToTest()
    {
        var vector = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var destination = new double[4];

        await Assert.That(vector.TryCopyTo(new(new double[3]))).IsFalse();
        await Assert.That(vector.TryCopyTo(destination.AsSpan())).IsTrue();

        await Assert.That(vector.X).IsEqualTo(1.0);
        await Assert.That(vector.Y).IsEqualTo(2.0);
        await Assert.That(vector.Z).IsEqualTo(3.0);
        await Assert.That(vector.W).IsEqualTo(4.0);
        await Assert.That(destination[0]).IsEqualTo(vector.X);
        await Assert.That(destination[1]).IsEqualTo(vector.Y);
        await Assert.That(destination[2]).IsEqualTo(vector.Z);
        await Assert.That(destination[3]).IsEqualTo(vector.W);
    }

    [Test]
    public async Task Vector4DGetHashCodeTest()
    {
        var v1 = new Vector4D(2.5, 2.0, 3.0, 3.3);
        var v2 = new Vector4D(2.5, 2.0, 3.0, 3.3);
        var v3 = new Vector4D(2.5, 2.0, 3.0, 3.3);
        var v5 = new Vector4D(3.3, 3.0, 2.0, 2.5);
        await Assert.That(v1.GetHashCode()).IsEqualTo(v1.GetHashCode());
        await Assert.That(v2.GetHashCode()).IsEqualTo(v1.GetHashCode());
        await Assert.That(v5.GetHashCode()).IsNotEqualTo(v1.GetHashCode());
        await Assert.That(v3.GetHashCode()).IsEqualTo(v1.GetHashCode());
        var v4 = new Vector4D(0.0, 0.0, 0.0, 0.0);
        var v6 = new Vector4D(1.0, 0.0, 0.0, 0.0);
        var v7 = new Vector4D(0.0, 1.0, 0.0, 0.0);
        var v8 = new Vector4D(1.0, 1.0, 1.0, 1.0);
        var v9 = new Vector4D(1.0, 1.0, 0.0, 0.0);
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v7.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v8.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v7.GetHashCode());
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v8.GetHashCode());
        await Assert.That(v7.GetHashCode()).IsNotEqualTo(v8.GetHashCode());
        await Assert.That(v7.GetHashCode()).IsNotEqualTo(v9.GetHashCode());
    }

    [Test]
    public async Task Vector4DToStringTest()
    {
        var separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
        var enUsCultureInfo = new CultureInfo("en-US");

        var v1 = new Vector4D(2.5, 2.0, 3.0, 3.3);

        var v1Str = v1.ToString();
        var expectedV1 = string.Format(CultureInfo.CurrentCulture
            , "<{1:G}{0} {2:G}{0} {3:G}{0} {4:G}>"
            , separator, 2.5, 2, 3, 3.3);
        await Assert.That(v1Str).IsEquivalentTo(expectedV1);

        var v1StrFormatted = v1.ToString("c", CultureInfo.CurrentCulture);
        var expectedV1Formatted = string.Format(CultureInfo.CurrentCulture
            , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
            , separator, 2.5, 2, 3, 3.3);
        await Assert.That(v1StrFormatted).IsEquivalentTo(expectedV1Formatted);

        var v2StrFormatted = v1.ToString("c", enUsCultureInfo);
        var expectedV2Formatted = string.Format(enUsCultureInfo
            , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
            , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2.5, 2, 3, 3.3);
        await Assert.That(v2StrFormatted).IsEquivalentTo(expectedV2Formatted);

        var v3StrFormatted = v1.ToString("c");
        var expectedV3Formatted = string.Format(CultureInfo.CurrentCulture
            , "<{1:c}{0} {2:c}{0} {3:c}{0} {4:c}>"
            , separator, 2.5, 2, 3, 3.3);
        await Assert.That(v3StrFormatted).IsEquivalentTo(expectedV3Formatted);
    }

    // A test for DistanceSquared (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DDistanceSquaredTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var expected = 64.0;

        var actual = Vector4D.DistanceSquared(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Distance (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DDistanceTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var expected = 8.0;

        var actual = Vector4D.Distance(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Distance (Vector4Df, Vector4Df)
    // Distance from the same point
    [Test]
    public async Task Vector4DDistanceTest1()
    {
        var a = new Vector4D(new(1.051, 2.05), 3.478, 1.0);
        var b = new Vector4D(new(1.051, 2.05, 3.478), 0.0)
        {
            W = 1.0,
        };

        var actual = Vector4D.Distance(a, b);
        await Assert.That(actual).IsEqualTo(0.0);
    }

    // A test for Dot (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DDotTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var expected = 70.0;

        var actual = Vector4D.Dot(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Dot (Vector4Df, Vector4Df)
    // Dot test for perpendicular vector
    [Test]
    public async Task Vector4DDotTest1()
    {
        var a = new Vector3D(1.55, 1.55, 1);
        var b = new Vector3D(2.5, 3, 1.5);
        var c = Vector3D.Cross(a, b);

        var d = new Vector4D(a, 0);
        var e = new Vector4D(c, 0);

        var actual = Vector4D.Dot(d, e);
        await Assert.That(actual).IsEqualTo(0.0).Within(1E-15);
    }

    [Test]
    public async Task Vector4DCrossTest()
    {
        var a3 = new Vector3D(1.0, 0.0, 0.0);
        var b3 = new Vector3D(0.0, 1.0, 0.0);
        var e3 = Vector3D.Cross(a3, b3);

        var a4 = new Vector4D(a3, 2.0);
        var b4 = new Vector4D(b3, 3.0);
        var e4 = new Vector4D(e3, a4.W * b4.W);

        var actual = Vector4D.Cross(a4, b4);
        await Assert.That(actual).IsEqualTo(e4);
    }

    [Test]
    public async Task Vector4DCrossTest1()
    {
        // Cross test of the same vector
        var a3 = new Vector3D(0.0, 1.0, 0.0);
        var b3 = new Vector3D(0.0, 1.0, 0.0);
        var e3 = Vector3D.Cross(a3, b3);

        var a4 = new Vector4D(a3, 3.0);
        var b4 = new Vector4D(b3, 3.0);
        var e4 = new Vector4D(e3, a4.W * b4.W);

        var actual = Vector4D.Cross(a4, b4);
        await Assert.That(actual).IsEqualTo(e4);
    }

    // A test for Length ()
    [Test]
    public async Task Vector4DLengthTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var w = 4.0;

        var target = new Vector4D(a, w);

        var expected = Math.Sqrt(30.0);

        var actual = target.Length();

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Length ()
    // Length test where length is zero
    [Test]
    public async Task Vector4DLengthTest1()
    {
        var target = new Vector4D();

        var expected = 0.0;
        var actual = target.Length();

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for LengthSquared ()
    [Test]
    public async Task Vector4DLengthSquaredTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var w = 4.0;

        var target = new Vector4D(a, w);

        double expected = 30;

        var actual = target.LengthSquared();

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Min (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DMinTest()
    {
        var a = new Vector4D(-1.0, 4.0, -3.0, 1000.0);
        var b = new Vector4D(2.0, 1.0, -1.0, 0.0);

        var expected = new Vector4D(-1.0, 1.0, -3.0, 0.0);
        var actual = Vector4D.Min(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Max (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DMaxTest()
    {
        var a = new Vector4D(-1.0, 4.0, -3.0, 1000.0);
        var b = new Vector4D(2.0, 1.0, -1.0, 0.0);

        var expected = new Vector4D(2.0, 4.0, -1.0, 1000.0);
        var actual = Vector4D.Max(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Vector4DMinMaxCodeCoverageTest()
    {
        var min = Vector4D.Zero;
        var max = Vector4D.One;

        var actual =
            // Min.
            Vector4D.Min(min, max);
        await Assert.That(min).IsEqualTo(actual);

        actual = Vector4D.Min(max, min);
        await Assert.That(min).IsEqualTo(actual);

        // Max.
        actual = Vector4D.Max(min, max);
        await Assert.That(max).IsEqualTo(actual);

        actual = Vector4D.Max(max, min);
        await Assert.That(max).IsEqualTo(actual);
    }

    // A test for Clamp (Vector4Df, Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DClampTest()
    {
        var a = new Vector4D(0.5, 0.3, 0.33, 0.44);
        var min = new Vector4D(0.0, 0.1, 0.13, 0.14);
        var max = new Vector4D(1.0, 1.1, 1.13, 1.14);

        // Normal case.
        // Case N1: specified value is in the range.
        var expected = new Vector4D(0.5, 0.3, 0.33, 0.44);
        var actual = Vector4D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);


        // Normal case.
        // Case N2: specified value is bigger than max value.
        a = new(2.0, 3.0, 4.0, 5.0);
        expected = max;
        actual = Vector4D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Case N3: specified value is smaller than max value.
        a = new(-2.0, -3.0, -4.0, -5.0);
        expected = min;
        actual = Vector4D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Case N4: combination case.
        a = new(-2.0, 0.5, 4.0, -5.0);
        expected = new(min.X, a.Y, max.Z, min.W);
        actual = Vector4D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // User specified min value is bigger than max value.
        max = new(0.0, 0.1, 0.13, 0.14);
        min = new(1.0, 1.1, 1.13, 1.14);

        // Case W1: specified value is in the range.
        a = new(0.5, 0.3, 0.33, 0.44);
        expected = max;
        actual = Vector4D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Normal case.
        // Case W2: specified value is bigger than max and min value.
        a = new(2.0, 3.0, 4.0, 5.0);
        expected = max;
        actual = Vector4D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Case W3: specified value is smaller than min and max value.
        a = new(-2.0, -3.0, -4.0, -5.0);
        expected = max;
        actual = Vector4D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    [Test]
    public async Task Vector4DLerpTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var t = 0.5;

        var expected = new Vector4D(3.0, 4.0, 5.0, 6.0);

        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test with factor zero
    [Test]
    public async Task Vector4DLerpTest1()
    {
        var a = new Vector4D(new(1.0, 2.0, 3.0), 4.0);
        var b = new Vector4D(4.0, 5.0, 6.0, 7.0);

        var t = 0.0;
        var expected = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test with factor one
    [Test]
    public async Task Vector4DLerpTest2()
    {
        var a = new Vector4D(new(1.0, 2.0, 3.0), 4.0);
        var b = new Vector4D(4.0, 5.0, 6.0, 7.0);

        var t = 1.0;
        var expected = new Vector4D(4.0, 5.0, 6.0, 7.0);
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test with factor > 1
    [Test]
    public async Task Vector4DLerpTest3()
    {
        var a = new Vector4D(new(0.0, 0.0, 0.0), 0.0);
        var b = new Vector4D(4.0, 5.0, 6.0, 7.0);

        var t = 2.0;
        var expected = new Vector4D(8.0, 10.0, 12.0, 14.0);
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test with factor < 0
    [Test]
    public async Task Vector4DLerpTest4()
    {
        var a = new Vector4D(new(0.0, 0.0, 0.0), 0.0);
        var b = new Vector4D(4.0, 5.0, 6.0, 7.0);

        var t = -2.0;
        var expected = -(b * 2);
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test with special double value
    [Test]
    public async Task Vector4DLerpTest5()
    {
        var a = new Vector4D(45.67, 90.0, 0, 0);
        var b = new Vector4D(double.PositiveInfinity, double.NegativeInfinity, 0, 0);

        var t = 0.408;
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual.X).IsPositiveInfinity();
        await Assert.That(actual.Y).IsNegativeInfinity();
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test from the same point
    [Test]
    public async Task Vector4DLerpTest6()
    {
        var a = new Vector4D(4.0, 5.0, 6.0, 7.0);
        var b = new Vector4D(4.0, 5.0, 6.0, 7.0);

        var t = 0.85;
        var expected = a;
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test with values known to be inaccurate with the old lerp impl
    [Test]
    public async Task Vector4DLerpTest7()
    {
        var a = new Vector4D(0.44728136);
        var b = new Vector4D(0.46345946);

        var t = 0.26402435;

        var expected = new Vector4D(0.45155275);
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-7);
    }

    // A test for Lerp (Vector4Df, Vector4Df, double)
    // Lerp test with values known to be inaccurate with the old lerp impl
    // (Old code incorrectly gets 0.33333588)
    [Test]
    public async Task Vector4DLerpTest8()
    {
        var a = new Vector4D(-100);
        var b = new Vector4D(0.33333334);

        double t = 1;

        var expected = new Vector4D(0.33333334);
        var actual = Vector4D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2Df, Matrix4x4D)
    [Test]
    public async Task Vector4DTransformTest1()
    {
        var v = new Vector2D(1.0, 2.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        var expected = new Vector4D(10.316987, 22.183012, 30.3660259, 1.0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-6);
    }

    // A test for Transform (Vector3Df, Matrix4x4D)
    [Test]
    public async Task Vector4DTransformTest2()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        var expected = new Vector4D(12.19198728, 21.53349376, 32.61602545, 1.0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-6);
    }

    // A test for Transform (Vector4Df, Matrix4x4D)
    [Test]
    public async Task Vector4DTransformVector4DTest()
    {
        var v = new Vector4D(1.0, 2.0, 3.0, 0.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        var expected = new Vector4D(2.19198728, 1.53349376, 2.61602545, 0.0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-6);

        //
        v.W = 1.0;

        expected = new(12.19198728, 21.53349376, 32.61602545, 1.0);
        actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-6);
    }

    // A test for Transform (Vector4Df, Matrix4x4D)
    // Transform vector4 with zero matrix
    [Test]
    public async Task Vector4DTransformVector4DTest1()
    {
        var v = new Vector4D(1.0, 2.0, 3.0, 0.0);
        var m = new Matrix4x4D();
        var expected = new Vector4D(0, 0, 0, 0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector4Df, Matrix4x4D)
    // Transform vector4 with identity matrix
    [Test]
    public async Task Vector4DTransformVector4DTest2()
    {
        var v = new Vector4D(1.0, 2.0, 3.0, 0.0);
        var m = Matrix4x4D.Identity;
        var expected = new Vector4D(1.0, 2.0, 3.0, 0.0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector3Df, Matrix4x4D)
    // Transform Vector3Df test
    [Test]
    public async Task Vector4DTransformVector3DTest()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        var expected = Vector4D.Transform(new Vector4D(v, 1.0), m);
        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector3Df, Matrix4x4D)
    // Transform vector3 with zero matrix
    [Test]
    public async Task Vector4DTransformVector3DTest1()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var m = new Matrix4x4D();
        var expected = new Vector4D(0, 0, 0, 0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector3Df, Matrix4x4D)
    // Transform vector3 with identity matrix
    [Test]
    public async Task Vector4DTransformVector3DTest2()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var m = Matrix4x4D.Identity;
        var expected = new Vector4D(1.0, 2.0, 3.0, 1.0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2Df, Matrix4x4D)
    // Transform Vector2Df test
    [Test]
    public async Task Vector4DTransformVector2DTest()
    {
        var v = new Vector2D(1.0, 2.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        var expected = Vector4D.Transform(new Vector4D(v, 0.0, 1.0), m);
        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2Df, Matrix4x4D)
    // Transform Vector2Df with zero matrix
    [Test]
    public async Task Vector4DTransformVector2DTest1()
    {
        var v = new Vector2D(1.0, 2.0);
        var m = new Matrix4x4D();
        var expected = new Vector4D(0, 0, 0, 0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2Df, Matrix4x4D)
    // Transform vector2 with identity matrix
    [Test]
    public async Task Vector4DTransformVector2DTest2()
    {
        var v = new Vector2D(1.0, 2.0);
        var m = Matrix4x4D.Identity;
        var expected = new Vector4D(1.0, 2.0, 0, 1.0);

        var actual = Vector4D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector3Df, QuaternionD)
    [Test]
    public async Task Vector4DTransformVector3DQuaternion()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        var q = QuaternionD.CreateFromRotationMatrix(m);

        var expected = Vector4D.Transform(v, m);

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);
    }

    // A test for Transform (Vector4Df, QuaternionD)
    [Test]
    public async Task Vector4DTransformVector4DQuaternionTest()
    {
        var v = new Vector4D(1.0, 2.0, 3.0, 0.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        var q = QuaternionD.CreateFromRotationMatrix(m);

        var expected = Vector4D.Transform(v, m);

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);

        //
        v.W = 1.0;
        expected.W = 1.0;
        actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);
    }

    // A test for Transform (Vector4Df, QuaternionD)
    // Transform vector4 with zero quaternion
    [Test]
    public async Task Vector4DTransformVector4DQuaternionTest1()
    {
        var v = new Vector4D(1.0, 2.0, 3.0, 0.0);
        var q = new QuaternionD();
        var expected = Vector4D.Zero;

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector4Df, QuaternionD)
    // Transform vector4 with identity matrix
    [Test]
    public async Task Vector4DTransformVector4DQuaternionTest2()
    {
        var v = new Vector4D(1.0, 2.0, 3.0, 0.0);
        var q = QuaternionD.Identity;
        var expected = new Vector4D(1.0, 2.0, 3.0, 0.0);

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector3Df, QuaternionD)
    // Transform Vector3Df test
    [Test]
    public async Task Vector4DTransformVector3DQuaternionTest()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        var q = QuaternionD.CreateFromRotationMatrix(m);

        var expected = Vector4D.Transform(v, m);
        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);
    }

    // A test for Transform (Vector3Df, QuaternionD)
    // Transform vector3 with zero quaternion
    [Test]
    public async Task Vector4DTransformVector3DQuaternionTest1()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var q = new QuaternionD();
        var expected = Vector4D.Zero;

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector3Df, QuaternionD)
    // Transform vector3 with identity quaternion
    [Test]
    public async Task Vector4DTransformVector3DQuaternionTest2()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var q = QuaternionD.Identity;
        var expected = new Vector4D(1.0, 2.0, 3.0, 1.0);

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2Df, QuaternionD)
    // Transform Vector2Df by quaternion test
    [Test]
    public async Task Vector4DTransformVector2DQuaternionTest()
    {
        var v = new Vector2D(1.0, 2.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        var q = QuaternionD.CreateFromRotationMatrix(m);

        var expected = Vector4D.Transform(v, m);
        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected).Within(1E-15);
    }

    // A test for Transform (Vector2Df, QuaternionD)
    // Transform Vector2Df with zero quaternion
    [Test]
    public async Task Vector4DTransformVector2DQuaternionTest1()
    {
        var v = new Vector2D(1.0, 2.0);
        var q = new QuaternionD();
        var expected = Vector4D.Zero;

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector2Df, Matrix4x4D)
    // Transform vector2 with identity QuaternionD
    [Test]
    public async Task Vector4DTransformVector2DQuaternionTest2()
    {
        var v = new Vector2D(1.0, 2.0);
        var q = QuaternionD.Identity;
        var expected = new Vector4D(1.0, 2.0, 0, 1.0);

        var actual = Vector4D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector4Df)
    [Test]
    public async Task Vector4DNormalizeTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);

        var expected = new Vector4D(
            0.1825741858350553711523232609336,
            0.3651483716701107423046465218672,
            0.5477225575051661134569697828008,
            0.7302967433402214846092930437344);

        var actual = Vector4D.Normalize(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector4Df)
    // Normalize vector of length one
    [Test]
    public async Task Vector4DNormalizeTest1()
    {
        var a = new Vector4D(1.0, 0.0, 0.0, 0.0);

        var expected = new Vector4D(1.0, 0.0, 0.0, 0.0);
        var actual = Vector4D.Normalize(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector4Df)
    // Normalize vector of length zero
    [Test]
    public async Task Vector4DNormalizeTest2()
    {
        var a = new Vector4D(0.0, 0.0, 0.0, 0.0);

        var actual = Vector4D.Normalize(a);
        await Assert.That(actual.X).IsNaN();
        await Assert.That(actual.Y).IsNaN();
        await Assert.That(actual.Z).IsNaN();
        await Assert.That(actual.W).IsNaN();
    }

    // A test for operator - (Vector4Df)
    [Test]
    public async Task Vector4DUnaryNegationTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);

        var expected = new Vector4D(-1.0, -2.0, -3.0, -4.0);

        var actual = -a;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator - (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DSubtractionTest()
    {
        var a = new Vector4D(1.0, 6.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 2.0, 3.0, 9.0);

        var expected = new Vector4D(-4.0, 4.0, 0.0, -5.0);

        var actual = a - b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Vector4Df, double)
    [Test]
    public async Task Vector4DMultiplyOperatorTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);

        const double Factor = 2.0;

        var expected = new Vector4D(2.0, 4.0, 6.0, 8.0);

        var actual = a * Factor;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (double, Vector4Df)
    [Test]
    public async Task Vector4DMultiplyOperatorTest2()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);

        const double Factor = 2.0;
        var expected = new Vector4D(2.0, 4.0, 6.0, 8.0);

        var actual = Factor * a;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DMultiplyOperatorTest3()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var expected = new Vector4D(5.0, 12.0, 21.0, 32.0);

        var actual = a * b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector4Df, double)
    [Test]
    public async Task Vector4DDivisionTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);

        var div = 2.0;

        var expected = new Vector4D(0.5, 1.0, 1.5, 2.0);

        var actual = a / div;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DDivisionTest1()
    {
        var a = new Vector4D(1.0, 6.0, 7.0, 4.0);
        var b = new Vector4D(5.0, 2.0, 3.0, 8.0);

        var expected = new Vector4D(1.0 / 5.0, 6.0 / 2.0, 7.0 / 3.0, 4.0 / 8.0);

        var actual = a / b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector4Df, Vector4Df)
    // Divide by zero
    [Test]
    public async Task Vector4DDivisionTest2()
    {
        var a = new Vector4D(-2.0, 3.0, double.MaxValue, double.NaN);

        var div = 0.0;

        var actual = a / div;

        await Assert.That(actual.X).IsNegativeInfinity();
        await Assert.That(actual.Y).IsPositiveInfinity();
        await Assert.That(actual.Z).IsPositiveInfinity();
        await Assert.That(actual.W).IsNaN();
    }

    // A test for operator / (Vector4Df, Vector4Df)
    // Divide by zero
    [Test]
    public async Task Vector4DDivisionTest3()
    {
        var a = new Vector4D(0.047, -3.0, double.NegativeInfinity, double.MinValue);
        var b = new Vector4D();

        var actual = a / b;

        await Assert.That(actual.X).IsPositiveInfinity();
        await Assert.That(actual.Y).IsNegativeInfinity();
        await Assert.That(actual.Z).IsNegativeInfinity();
        await Assert.That(actual.W).IsNegativeInfinity();
    }

    // A test for operator + (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DAdditionTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var expected = new Vector4D(6.0, 8.0, 10.0, 12.0);

        var actual = a + b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task OperatorAddTest()
    {
        var v1 = new Vector4D(2.5, 2.0, 3.0, 3.3);
        var v2 = new Vector4D(5.5, 4.5, 6.5, 7.5);

        var v3 = v1 + v2;
        var v5 = new Vector4D(-1.0, 0.0, 0.0, double.NaN);
        var v4 = v1 + v5;
        await Assert.That(v3.X).IsEqualTo(8.0);
        await Assert.That(v3.Y).IsEqualTo(6.5);
        await Assert.That(v3.Z).IsEqualTo(9.5);
        await Assert.That(v3.W).IsEqualTo(10.8);
        await Assert.That(v4.X).IsEqualTo(1.5);
        await Assert.That(v4.Y).IsEqualTo(2.0);
        await Assert.That(v4.Z).IsEqualTo(3.0);
        await Assert.That(v4.W).IsEqualTo(double.NaN);
    }

    // A test for Vector4Df (double, double, double, double)
    [Test]
    public async Task Vector4DConstructorTest()
    {
        var x = 1.0;
        var y = 2.0;
        var z = 3.0;
        var w = 4.0;

        var target = new Vector4D(x, y, z, w);

        await Assert.That(target.X).IsEqualTo(x);
        await Assert.That(target.Y).IsEqualTo(y);
        await Assert.That(target.Z).IsEqualTo(z);
        await Assert.That(target.W).IsEqualTo(w);
    }

    // A test for Vector4Df (Vector2Df, double, double)
    [Test]
    public async Task Vector4DConstructorTest1()
    {
        var a = new Vector2D(1.0, 2.0);
        var z = 3.0;
        var w = 4.0;

        var target = new Vector4D(a, z, w);

        await Assert.That(target.X).IsEqualTo(a.X);
        await Assert.That(target.Y).IsEqualTo(a.Y);
        await Assert.That(target.Z).IsEqualTo(z);
        await Assert.That(target.W).IsEqualTo(w);
    }

    // A test for Vector4Df (Vector3Df, double)
    [Test]
    public async Task Vector4DConstructorTest2()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var w = 4.0;

        var target = new Vector4D(a, w);

        await Assert.That(target.X).IsEqualTo(a.X);
        await Assert.That(target.Y).IsEqualTo(a.Y);
        await Assert.That(target.Z).IsEqualTo(a.Z);
        await Assert.That(target.W).IsEqualTo(w);
    }

    // A test for Vector4Df ()
    // Constructor with no parameter
    [Test]
    public async Task Vector4DConstructorTest4()
    {
        var a = new Vector4D();

        await Assert.That(a.X).IsEqualTo(0.0);
        await Assert.That(a.Y).IsEqualTo(0.0);
        await Assert.That(a.Z).IsEqualTo(0.0);
        await Assert.That(a.W).IsEqualTo(0.0);
    }

    // A test for Vector4Df ()
    // Constructor with special floating values
    [Test]
    public async Task Vector4DConstructorTest5()
    {
        var target = new Vector4D(double.NaN, double.MaxValue, double.PositiveInfinity, double.Epsilon);

        await Assert.That(target.X).IsNaN();
        await Assert.That(target.Y).IsEqualTo(double.MaxValue);
        await Assert.That(target.Z).IsPositiveInfinity();
        await Assert.That(target.W).IsEqualTo(double.Epsilon);
    }

    // A test for Vector4Df (ReadOnlySpan<double>)
    [Test]
    public async Task Vector4DConstructorTest7()
    {
        var value = 1.0;
        var target = new Vector4D(new[] { value, value, value, value });
        var expected = new Vector4D(value);

        await Assert.That(target).IsEqualTo(expected);
        await Assert.That(() => new Vector4D(new double[3])).Throws<ArgumentOutOfRangeException>();
    }

    // A test for Add (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DAddTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var expected = new Vector4D(6.0, 8.0, 10.0, 12.0);

        var actual = Vector4D.Add(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Divide (Vector4Df, double)
    [Test]
    public async Task Vector4DDivideTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var div = 2.0;
        var expected = new Vector4D(0.5, 1.0, 1.5, 2.0);
        var actual = Vector4D.Divide(a, div);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Divide (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DDivideTest1()
    {
        var a = new Vector4D(1.0, 6.0, 7.0, 4.0);
        var b = new Vector4D(5.0, 2.0, 3.0, 8.0);

        var expected = new Vector4D(1.0 / 5.0, 6.0 / 2.0, 7.0 / 3.0, 4.0 / 8.0);

        var actual = Vector4D.Divide(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Equals (object)
    [Test]
    public async Task Vector4DEqualsTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(1.0, 2.0, 3.0, 4.0);

        // case 1: compare between same values
        object obj = b;

        var expected = true;
        var actual = a.Equals(obj);
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

    // A test for Multiply (double, Vector4Df)
    [Test]
    public async Task Vector4DMultiplyTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        const double Factor = 2.0;
        var expected = new Vector4D(2.0, 4.0, 6.0, 8.0);
        var actual = Vector4D.Multiply(Factor, a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Vector4Df, double)
    [Test]
    public async Task Vector4DMultiplyTest2()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        const double Factor = 2.0;
        var expected = new Vector4D(2.0, 4.0, 6.0, 8.0);
        var actual = Vector4D.Multiply(a, Factor);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DMultiplyTest3()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 6.0, 7.0, 8.0);

        var expected = new Vector4D(5.0, 12.0, 21.0, 32.0);

        var actual = Vector4D.Multiply(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Negate (Vector4Df)
    [Test]
    public async Task Vector4DNegateTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);

        var expected = new Vector4D(-1.0, -2.0, -3.0, -4.0);

        var actual = Vector4D.Negate(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator != (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DInequalityTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(1.0, 2.0, 3.0, 4.0);

        // case 1: compare between same values
        var expected = false;
        var actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.X = 10.0;
        expected = true;
        actual = a != b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator == (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DEqualityTest()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(1.0, 2.0, 3.0, 4.0);

        // case 1: compare between same values
        var expected = true;
        var actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.X = 10.0;
        expected = false;
        actual = a == b;
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Subtract (Vector4Df, Vector4Df)
    [Test]
    public async Task Vector4DSubtractTest()
    {
        var a = new Vector4D(1.0, 6.0, 3.0, 4.0);
        var b = new Vector4D(5.0, 2.0, 3.0, 9.0);

        var expected = new Vector4D(-4.0, 4.0, 0.0, -5.0);

        var actual = Vector4D.Subtract(a, b);

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for UnitW
    [Test]
    public async Task Vector4DUnitWTest()
    {
        var val = new Vector4D(0.0, 0.0, 0.0, 1.0);
        await Assert.That(Vector4D.UnitW).IsEqualTo(val);
    }

    // A test for UnitX
    [Test]
    public async Task Vector4DUnitXTest()
    {
        var val = new Vector4D(1.0, 0.0, 0.0, 0.0);
        await Assert.That(Vector4D.UnitX).IsEqualTo(val);
    }

    // A test for UnitY
    [Test]
    public async Task Vector4DUnitYTest()
    {
        var val = new Vector4D(0.0, 1.0, 0.0, 0.0);
        await Assert.That(Vector4D.UnitY).IsEqualTo(val);
    }

    // A test for UnitZ
    [Test]
    public async Task Vector4DUnitZTest()
    {
        var val = new Vector4D(0.0, 0.0, 1.0, 0.0);
        await Assert.That(Vector4D.UnitZ).IsEqualTo(val);
    }

    // A test for One
    [Test]
    public async Task Vector4DOneTest()
    {
        var val = new Vector4D(1.0, 1.0, 1.0, 1.0);
        await Assert.That(Vector4D.One).IsEqualTo(val);
    }

    // A test for Zero
    [Test]
    public async Task Vector4DZeroTest()
    {
        var val = new Vector4D(0.0, 0.0, 0.0, 0.0);
        await Assert.That(Vector4D.Zero).IsEqualTo(val);
    }

    // A test for Equals (Vector4Df)
    [Test]
    public async Task Vector4DEqualsTest1()
    {
        var a = new Vector4D(1.0, 2.0, 3.0, 4.0);
        var b = new Vector4D(1.0, 2.0, 3.0, 4.0);

        // case 1: compare between same values
        await Assert.That(a.Equals(b)).IsTrue();

        // case 2: compare between different values
        b.X = 10.0;
        await Assert.That(a.Equals(b)).IsFalse();
    }

    // A test for Vector4Df (double)
    [Test]
    public async Task Vector4DConstructorTest6()
    {
        var value = 1.0;
        var target = new Vector4D(value);

        var expected = new Vector4D(value, value, value, value);
        await Assert.That(target).IsEqualTo(expected);

        value = 2.0;
        target = new(value);
        expected = new(value, value, value, value);
        await Assert.That(target).IsEqualTo(expected);
    }

    // A test for Vector4Df comparison involving NaN values
    [Test]
    public async Task Vector4DEqualsNaNTest()
    {
        var a = new Vector4D(double.NaN, 0, 0, 0);
        var b = new Vector4D(0, double.NaN, 0, 0);
        var c = new Vector4D(0, 0, double.NaN, 0);
        var d = new Vector4D(0, 0, 0, double.NaN);

        await Assert.That(a == Vector4D.Zero).IsFalse();
        await Assert.That(b == Vector4D.Zero).IsFalse();
        await Assert.That(c == Vector4D.Zero).IsFalse();
        await Assert.That(d == Vector4D.Zero).IsFalse();

        await Assert.That(a != Vector4D.Zero).IsTrue();
        await Assert.That(b != Vector4D.Zero).IsTrue();
        await Assert.That(c != Vector4D.Zero).IsTrue();
        await Assert.That(d != Vector4D.Zero).IsTrue();

        await Assert.That(a.Equals(Vector4D.Zero)).IsFalse();
        await Assert.That(b.Equals(Vector4D.Zero)).IsFalse();
        await Assert.That(c.Equals(Vector4D.Zero)).IsFalse();
        await Assert.That(d.Equals(Vector4D.Zero)).IsFalse();

        await Assert.That(a.Equals(a)).IsTrue();
        await Assert.That(b.Equals(b)).IsTrue();
        await Assert.That(c.Equals(c)).IsTrue();
        await Assert.That(d.Equals(d)).IsTrue();
    }

    [Test]
    public async Task Vector4DAbsTest()
    {
        var v1 = new Vector4D(-2.5, 2.0, 3.0, 3.3);
        var v3 = Vector4D.Abs(new(double.PositiveInfinity, 0.0, double.NegativeInfinity, double.NaN));
        var v = Vector4D.Abs(v1);
        await Assert.That(v.X).IsEqualTo(2.5);
        await Assert.That(v.Y).IsEqualTo(2.0);
        await Assert.That(v.Z).IsEqualTo(3.0);
        await Assert.That(v.W).IsEqualTo(3.3);
        await Assert.That(v3.X).IsEqualTo(double.PositiveInfinity);
        await Assert.That(v3.Y).IsEqualTo(0.0);
        await Assert.That(v3.Z).IsEqualTo(double.PositiveInfinity);
        await Assert.That(v3.W).IsEqualTo(double.NaN);
    }

    [Test]
    public async Task Vector4DSqrtTest()
    {
        var v1 = new Vector4D(-2.5, 2.0, 3.0, 3.3);
        var v2 = new Vector4D(5.5, 4.5, 6.5, 7.5);
        await Assert.That((int)Vector4D.SquareRoot(v2).X).IsEqualTo(2);
        await Assert.That((int)Vector4D.SquareRoot(v2).Y).IsEqualTo(2);
        await Assert.That((int)Vector4D.SquareRoot(v2).Z).IsEqualTo(2);
        await Assert.That((int)Vector4D.SquareRoot(v2).W).IsEqualTo(2);
        await Assert.That(Vector4D.SquareRoot(v1).X).IsEqualTo(double.NaN);
    }

    // A test to make sure these types are blittable directly into GPU buffer memory layouts
    [Test]
    public async Task Vector4DSizeofTest()
    {
        int sizeofVector4D;
        int sizeofVector4D2;
        int sizeofVector4DPlusDouble;
        int sizeofVector4DPlusDouble2;

        unsafe
        {
            sizeofVector4D = sizeof(Vector4D);
            sizeofVector4D2 = sizeof(Vector4D2X);
            sizeofVector4DPlusDouble = sizeof(Vector4DPlusDouble);
            sizeofVector4DPlusDouble2 = sizeof(Vector4DPlusDouble2X);
        }

        await Assert.That(sizeofVector4D).IsEqualTo(32);
        await Assert.That(sizeofVector4D2).IsEqualTo(64);
        await Assert.That(sizeofVector4DPlusDouble).IsEqualTo(40);
        await Assert.That(sizeofVector4DPlusDouble2).IsEqualTo(80);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector4D2X
    {
        private Vector4D _a;
        private Vector4D _b;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector4DPlusDouble
    {
        private Vector4D _v;
        private double _f;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector4DPlusDouble2X
    {
        private Vector4DPlusDouble _a;
        private Vector4DPlusDouble _b;
    }

    [Test]
    public async Task SetFieldsTest()
    {
        var v3 = new Vector4D(4, 5, 6, 7)
        {
            X = 1.0,
            Y = 2.0,
            Z = 3.0,
            W = 4.0,
        };
        await Assert.That(v3.X).IsEqualTo(1.0);
        await Assert.That(v3.Y).IsEqualTo(2.0);
        await Assert.That(v3.Z).IsEqualTo(3.0);
        await Assert.That(v3.W).IsEqualTo(4.0);
        var v4 = v3;
        v4.Y = 0.5;
        v4.Z = 2.2;
        v4.W = 3.5;
        await Assert.That(v4.X).IsEqualTo(1.0);
        await Assert.That(v4.Y).IsEqualTo(0.5);
        await Assert.That(v4.Z).IsEqualTo(2.2);
        await Assert.That(v4.W).IsEqualTo(3.5);
        await Assert.That(v3.Y).IsEqualTo(2.0);
    }

    [Test]
    public async Task EmbeddedVectorSetFields()
    {
        var evo = new EmbeddedVectorObject();
        evo.FieldVector.X = 5.0;
        evo.FieldVector.Y = 5.0;
        evo.FieldVector.Z = 5.0;
        evo.FieldVector.W = 5.0;
        await Assert.That(evo.FieldVector.X).IsEqualTo(5.0);
        await Assert.That(evo.FieldVector.Y).IsEqualTo(5.0);
        await Assert.That(evo.FieldVector.Z).IsEqualTo(5.0);
        await Assert.That(evo.FieldVector.W).IsEqualTo(5.0);
    }

    [Test]
    public async Task DeeplyEmbeddedObjectTest()
    {
        var obj = new DeeplyEmbeddedClass();
        obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector.X = 5;
        await Assert.That(obj.RootEmbeddedObject.X).IsEqualTo(5);
        await Assert.That(obj.RootEmbeddedObject.Y).IsEqualTo(5);
        await Assert.That(obj.RootEmbeddedObject.Z).IsEqualTo(1);
        await Assert.That( obj.RootEmbeddedObject.W).IsEqualTo(-5);
        obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector = new(1, 2, 3, 4);
        await Assert.That(obj.RootEmbeddedObject.X).IsEqualTo(1);
        await Assert.That(obj.RootEmbeddedObject.Y).IsEqualTo(2);
        await Assert.That(obj.RootEmbeddedObject.Z).IsEqualTo(3);
        await Assert.That(obj.RootEmbeddedObject.W).IsEqualTo(4);
    }

    [Test]
    public async Task DeeplyEmbeddedStructTest()
    {
        var obj = DeeplyEmbeddedStruct.Create();
        obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector.X = 5;
        await Assert.That(obj.RootEmbeddedObject.X).IsEqualTo(5);
        await Assert.That(obj.RootEmbeddedObject.Y).IsEqualTo(5);
        await Assert.That(obj.RootEmbeddedObject.Z).IsEqualTo(1);
        await Assert.That(obj.RootEmbeddedObject.W).IsEqualTo(-5);
        obj.L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector = new(1, 2, 3, 4);
        await Assert.That(obj.RootEmbeddedObject.X).IsEqualTo(1);
        await Assert.That(obj.RootEmbeddedObject.Y).IsEqualTo(2);
        await Assert.That(obj.RootEmbeddedObject.Z).IsEqualTo(3);
        await Assert.That(obj.RootEmbeddedObject.W).IsEqualTo(4);
    }

    private class EmbeddedVectorObject
    {
        public Vector4D FieldVector;
    }

    private class DeeplyEmbeddedClass
    {
        public readonly Level0 L0 = new();

        public Vector4D RootEmbeddedObject => L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector;

        public class Level0
        {
            public readonly Level1 L1 = new();

            public class Level1
            {
                public readonly Level2 L2 = new();

                public class Level2
                {
                    public readonly Level3 L3 = new();

                    public class Level3
                    {
                        public readonly Level4 L4 = new();

                        public class Level4
                        {
                            public readonly Level5 L5 = new();

                            public class Level5
                            {
                                public readonly Level6 L6 = new();

                                public class Level6
                                {
                                    public readonly Level7 L7 = new();

                                    public class Level7
                                    {
                                        public Vector4D EmbeddedVector = new(1, 5, 1, -5);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    // Contrived test for strangely-sized and shaped embedded structures, with unused buffer fields.
#pragma warning disable 0169
    private struct DeeplyEmbeddedStruct
    {
        public static DeeplyEmbeddedStruct Create()
        {
            var obj = new DeeplyEmbeddedStruct
            {
                L0 = new()
                {
                    L1 = new()
                    {
                        L2 = new()
                        {
                            L3 = new()
                            {
                                L4 = new()
                                {
                                    L5 = new()
                                    {
                                        L6 = new()
                                        {
                                            L7 = new()
                                            {
                                                EmbeddedVector = new(1, 5, 1, -5),
                                            },
                                        },
                                    },
                                },
                            },
                        },
                    },
                },
            };

            return obj;
        }

        public Level0 L0;

        public Vector4D RootEmbeddedObject => L0.L1.L2.L3.L4.L5.L6.L7.EmbeddedVector;

        public struct Level0
        {
            private double buffer0, buffer1;
            public Level1 L1;
            private double buffer2;

            public struct Level1
            {
                private double buffer0, buffer1;
                public Level2 L2;
                private byte buffer2;

                public struct Level2
                {
                    public Level3 L3;
                    private double buffer0;
                    private byte buffer1;

                    public struct Level3
                    {
                        public Level4 L4;

                        public struct Level4
                        {
                            private double buffer0;
                            public Level5 L5;
                            private long buffer1;
                            private byte buffer2;
                            private double buffer3;

                            public struct Level5
                            {
                                private byte buffer0;
                                public Level6 L6;

                                public struct Level6
                                {
                                    private byte buffer0;
                                    public Level7 L7;
                                    private byte buffer1, buffer2;

                                    public struct Level7
                                    {
                                        public Vector4D EmbeddedVector;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
#pragma warning restore 0169

#if NET9_0_OR_GREATER
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.CosDouble))]
    public async Task CosDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector4D.Cos(Vector4D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.ExpDouble))]
    public async Task ExpDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector4D.Exp(Vector4D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.LogDouble))]
    public async Task LogDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector4D.Log(Vector4D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.Log2Double))]
    public async Task Log2DoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector4D.Log2(Vector4D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.FusedMultiplyAddDouble))]
    public async Task FusedMultiplyAddDoubleTest(double left, double right, double addend, double expectedResult)
    {
        await Assert.That(Vector4D.FusedMultiplyAdd(Vector4D.Create(left), Vector4D.Create(right), Vector4D.Create(addend))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
        await Assert.That(Vector4D.MultiplyAddEstimate(Vector4D.Create(left), Vector4D.Create(right), Vector4D.Create(addend))).IsEqualTo(Vector4D.Create(double.MultiplyAddEstimate(left, right, addend))).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.ClampDouble))]
    public async Task ClampDoubleTest(double x, double min, double max, double expectedResult)
    {
        var actualResult = Vector4D.Clamp(Vector4D.Create(x), Vector4D.Create(min), Vector4D.Create(max));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.CopySignDouble))]
    public async Task CopySignDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.CopySign(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.DegreesToRadiansDouble))]
    public async Task DegreesToRadiansDoubleTest(double value, double expectedResult, double variance)
    {
        await Assert.That(Vector4D.DegreesToRadians(Vector4D.Create(-value))).IsEqualTo(Vector4D.Create(-expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.DegreesToRadians(Vector4D.Create(+value))).IsEqualTo(Vector4D.Create(+expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.HypotDouble))]
    public async Task HypotDoubleTest(double x, double y, double expectedResult, double variance)
    {
        await Assert.That(Vector4D.Hypot(Vector4D.Create(-x), Vector4D.Create(-y))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.Hypot(Vector4D.Create(-x), Vector4D.Create(+y))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.Hypot(Vector4D.Create(+x), Vector4D.Create(-y))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.Hypot(Vector4D.Create(+x), Vector4D.Create(+y))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));

        await Assert.That(Vector4D.Hypot(Vector4D.Create(-y), Vector4D.Create(-x))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.Hypot(Vector4D.Create(-y), Vector4D.Create(+x))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.Hypot(Vector4D.Create(+y), Vector4D.Create(-x))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.Hypot(Vector4D.Create(+y), Vector4D.Create(+x))).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.LerpDouble))]
    public async Task LerpDoubleTest(double x, double y, double amount, double expectedResult)
    {
        await Assert.That(Vector4D.Lerp(Vector4D.Create(+x), Vector4D.Create(+y), Vector4D.Create(amount))).IsEqualTo(Vector4D.Create(+expectedResult)).Within(Vector4D.Zero);
        await Assert.That(Vector4D.Lerp(Vector4D.Create(-x), Vector4D.Create(-y), Vector4D.Create(amount))).IsEqualTo(Vector4D.Create((expectedResult == 0.0) ? expectedResult : -expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxDouble))]
    public async Task MaxDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.Max(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxMagnitudeDouble))]
    public async Task MaxMagnitudeDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.MaxMagnitude(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxMagnitudeNumberDouble))]
    public async Task MaxMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.MaxMagnitudeNumber(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxNumberDouble))]
    public async Task MaxNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.MaxNumber(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinDouble))]
    public async Task MinDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.Min(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinMagnitudeDouble))]
    public async Task MinMagnitudeDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.MinMagnitude(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinMagnitudeNumberDouble))]
    public async Task MinMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.MinMagnitudeNumber(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinNumberDouble))]
    public async Task MinNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector4D.MinNumber(Vector4D.Create(x), Vector4D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RadiansToDegreesDouble))]
    public async Task RadiansToDegreesDoubleTest(double value, double expectedResult, double variance)
    {
        await Assert.That(Vector4D.RadiansToDegrees(Vector4D.Create(-value))).IsEqualTo(Vector4D.Create(-expectedResult)).Within(Vector4D.Create(variance));
        await Assert.That(Vector4D.RadiansToDegrees(Vector4D.Create(+value))).IsEqualTo(Vector4D.Create(+expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundDouble))]
    public async Task RoundDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector4D.Round(Vector4D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundAwayFromZeroDouble))]
    public async Task RoundAwayFromZeroDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector4D.Round(Vector4D.Create(value), MidpointRounding.AwayFromZero);
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundToEvenDouble))]
    public async Task RoundToEvenDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector4D.Round(Vector4D.Create(value), MidpointRounding.ToEven);
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.SinDouble))]
    public async Task SinDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector4D.Sin(Vector4D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.SinCosDouble))]
    public async Task SinCosDoubleTest(double value, double expectedResultSin, double expectedResultCos, double allowedVarianceSin, double allowedVarianceCos)
    {
        var (resultSin, resultCos) = Vector4D.SinCos(Vector4D.Create(value));
        await Assert.That(resultSin).IsEqualTo(Vector4D.Create(expectedResultSin)).Within(Vector4D.Create(allowedVarianceSin));
        await Assert.That(resultCos).IsEqualTo(Vector4D.Create(expectedResultCos)).Within(Vector4D.Create(allowedVarianceCos));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.TruncateDouble))]
    public async Task TruncateDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector4D.Truncate(Vector4D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector4D.Create(expectedResult)).Within(Vector4D.Zero);
    }
#endif
    
#if NET10_OR_GREATER
    [Test]
    public async Task AllAnyNoneTest()
    {
        await Test(3, 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value1, double value2)
        {
            var input1 = Vector4D.Create(value1);
            var input2 = Vector4D.Create(value2);

            await Assert.That(Vector4D.All(input1, value1)).IsTrue();
            await Assert.That(Vector4D.All(input2, value2)).IsTrue();
            await Assert.That(Vector4D.All(input1.WithElement(0, value2), value1)).IsFalse();
            await Assert.That(Vector4D.All(input2.WithElement(0, value1), value2)).IsFalse();
            await Assert.That(Vector4D.All(input1, value2)).IsFalse();
            await Assert.That(Vector4D.All(input2, value1)).IsFalse();
            await Assert.That(Vector4D.All(input1.WithElement(0, value2), value2)).IsFalse();
            await Assert.That(Vector4D.All(input2.WithElement(0, value1), value1)).IsFalse();

            await Assert.That(Vector4D.Any(input1, value1)).IsTrue();
            await Assert.That(Vector4D.Any(input2, value2)).IsTrue();
            await Assert.That(Vector4D.Any(input1.WithElement(0, value2), value1)).IsTrue();
            await Assert.That(Vector4D.Any(input2.WithElement(0, value1), value2)).IsTrue();
            await Assert.That(Vector4D.Any(input1, value2)).IsFalse();
            await Assert.That(Vector4D.Any(input2, value1)).IsFalse();
            await Assert.That(Vector4D.Any(input1.WithElement(0, value2), value2)).IsTrue();
            await Assert.That(Vector4D.Any(input2.WithElement(0, value1), value1)).IsTrue();

            await Assert.That(Vector4D.None(input1, value1)).IsFalse();
            await Assert.That(Vector4D.None(input2, value2)).IsFalse();
            await Assert.That(Vector4D.None(input1.WithElement(0, value2), value1)).IsFalse();
            await Assert.That(Vector4D.None(input2.WithElement(0, value1), value2)).IsFalse();
            await Assert.That(Vector4D.None(input1, value2)).IsTrue();
            await Assert.That(Vector4D.None(input2, value1)).IsTrue();
            await Assert.That(Vector4D.None(input1.WithElement(0, value2), value2)).IsFalse();
            await Assert.That(Vector4D.None(input2.WithElement(0, value1), value1)).IsFalse();
        }
    }

    [Test]
    public async Task AllAnyNoneTest_AllBitsSet()
    {
        await Test(BitConverter.Int64BitsToDouble(-1));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value)
        {
            var input = Vector4D.Create(value);

            await Assert.That(Vector4D.All(input, value)).IsFalse();
            await Assert.That(Vector4D.Any(input, value)).IsFalse();
            await Assert.That(Vector4D.None(input, value)).IsTrue();
        }
    }

    [Test]
    public async Task AllAnyNoneWhereAllBitsSetTest()
    {
        await  Test(BitConverter.Int64BitsToDouble(-1), 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double allBitsSet, double value2)
        {
            var input1 = Vector4D.Create(allBitsSet);
            var input2 = Vector4D.Create(value2);

            await Assert.That(Vector4D.AllWhereAllBitsSet(input1)).IsTrue();
            await Assert.That(Vector4D.AllWhereAllBitsSet(input2)).IsFalse();
            await Assert.That(Vector4D.AllWhereAllBitsSet(input1.WithElement(0, value2))).IsFalse();
            await Assert.That(Vector4D.AllWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsFalse();

            await Assert.That(Vector4D.AnyWhereAllBitsSet(input1)).IsTrue();
            await Assert.That(Vector4D.AnyWhereAllBitsSet(input2)).IsFalse();
            await Assert.That(Vector4D.AnyWhereAllBitsSet(input1.WithElement(0, value2))).IsTrue();
            await Assert.That(Vector4D.AnyWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsTrue();

            await Assert.That(Vector4D.NoneWhereAllBitsSet(input1)).IsFalse();
            await Assert.That(Vector4D.NoneWhereAllBitsSet(input2)).IsTrue();
            await Assert.That(Vector4D.NoneWhereAllBitsSet(input1.WithElement(0, value2))).IsFalse();
            await Assert.That(Vector4D.NoneWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsFalse();
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfDoubleTest()
    {
        await  Test(3, 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value1, double value2)
        {
            var input1 = Vector4D.Create(value1);
            var input2 = Vector4D.Create(value2);

            await Assert.That(Vector4D.Count(input1, value1)).IsEqualTo(ElementCount);
            await Assert.That(Vector4D.Count(input2, value2)).IsEqualTo(ElementCount);
            await Assert.That(Vector4D.Count(input1.WithElement(0, value2), value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector4D.Count(input2.WithElement(0, value1), value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector4D.Count(input1, value2)).IsEqualTo(0);
            await Assert.That(Vector4D.Count(input2, value1)).IsEqualTo(0);
            await Assert.That(Vector4D.Count(input1.WithElement(0, value2), value2)).IsEqualTo(1);
            await Assert.That(Vector4D.Count(input2.WithElement(0, value1), value1)).IsEqualTo(1);

            await Assert.That(Vector4D.IndexOf(input1, value1)).IsEqualTo(0);
            await Assert.That(Vector4D.IndexOf(input2, value2)).IsEqualTo(0);
            await Assert.That(Vector4D.IndexOf(input1.WithElement(0, value2), value1)).IsEqualTo(1);
            await Assert.That(Vector4D.IndexOf(input2.WithElement(0, value1), value2)).IsEqualTo(1);
            await Assert.That(Vector4D.IndexOf(input1, value2)).IsEqualTo(-1);
            await Assert.That(Vector4D.IndexOf(input2, value1)).IsEqualTo(-1);
            await Assert.That(Vector4D.IndexOf(input1.WithElement(0, value2), value2)).IsEqualTo(0);
            await Assert.That(Vector4D.IndexOf(input2.WithElement(0, value1), value1)).IsEqualTo(0);

            await Assert.That(Vector4D.LastIndexOf(input1, value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector4D.LastIndexOf(input2, value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector4D.LastIndexOf(input1.WithElement(0, value2), value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector4D.LastIndexOf(input2.WithElement(0, value1), value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector4D.LastIndexOf(input1, value2)).IsEqualTo(-1);
            await Assert.That(Vector4D.LastIndexOf(input2, value1)).IsEqualTo(-1);
            await Assert.That(Vector4D.LastIndexOf(input1.WithElement(0, value2), value2)).IsEqualTo(0);
            await Assert.That(Vector4D.LastIndexOf(input2.WithElement(0, value1), value1)).IsEqualTo(0);
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfDoubleTest_AllBitsSet()
    {
        await  Test(BitConverter.Int64BitsToDouble(-1));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value)
        {
            var input = Vector4D.Create(value);

            await Assert.That(Vector4D.Count(input, value)).IsEqualTo(0);
            await Assert.That(Vector4D.IndexOf(input, value)).IsEqualTo(-1);
            await Assert.That(Vector4D.LastIndexOf(input, value)).IsEqualTo(-1);
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfWhereAllBitsSetDoubleTest()
    {
        await    Test(BitConverter.Int64BitsToDouble(-1), 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double allBitsSet, double value2)
        {
            var input1 = Vector4D.Create(allBitsSet);
            var input2 = Vector4D.Create(value2);

            await Assert.That( Vector4D.CountWhereAllBitsSet(input1)).IsEqualTo(ElementCount);
            await Assert.That( Vector4D.CountWhereAllBitsSet(input2)).IsEqualTo(0);
            await Assert.That(Vector4D.CountWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(ElementCount- 1);
            await Assert.That( Vector4D.CountWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(1);

            await Assert.That(Vector4D.IndexOfWhereAllBitsSet(input1)).IsEqualTo(0);
            await Assert.That( Vector4D.IndexOfWhereAllBitsSet(input2)).IsEqualTo(-1);
            await Assert.That(Vector4D.IndexOfWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(1);
            await Assert.That(Vector4D.IndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(0);

            await Assert.That(  Vector4D.LastIndexOfWhereAllBitsSet(input1)).IsEqualTo(ElementCount - 1);
            await Assert.That( Vector4D.LastIndexOfWhereAllBitsSet(input2)).IsEqualTo(-1);
            await Assert.That( Vector4D.LastIndexOfWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(ElementCount -1);
            await Assert.That( Vector4D.LastIndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(0);
        }
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsEvenIntegerTest(double value) => await Assert.That(Vector4D.IsEvenInteger(Vector4D.Create(value))).IsEqualTo(double.IsEvenInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsFiniteTest(double value) => await Assert.That(Vector4D.IsFinite(Vector4D.Create(value))).IsEqualTo(double.IsFinite(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsInfinityTest(double value) => await Assert.That(Vector4D.IsInfinity(Vector4D.Create(value))).IsEqualTo(double.IsInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsIntegerTest(double value) => await Assert.That(Vector4D.IsInteger(Vector4D.Create(value))).IsEqualTo(double.IsInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNaNTest(double value) => await Assert.That(Vector4D.IsNaN(Vector4D.Create(value))).IsEqualTo(double.IsNaN(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNegativeTest(double value) => await Assert.That(Vector4D.IsNegative(Vector4D.Create(value))).IsEqualTo(double.IsNegative(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNegativeInfinityTest(double value) => await Assert.That(Vector4D.IsNegativeInfinity(Vector4D.Create(value))).IsEqualTo(double.IsNegativeInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNormalTest(double value) => await Assert.That(Vector4D.IsNormal(Vector4D.Create(value))).IsEqualTo(double.IsNormal(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsOddIntegerTest(double value) => await Assert.That(Vector4D.IsOddInteger(Vector4D.Create(value))).IsEqualTo(double.IsOddInteger(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsPositiveTest(double value) => await Assert.That(Vector4D.IsPositive(Vector4D.Create(value))).IsEqualTo(double.IsPositive(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsPositiveInfinityTest(double value) => await Assert.That(Vector4D.IsPositiveInfinity(Vector4D.Create(value))).IsEqualTo(double.IsPositiveInfinity(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsSubnormalTest(double value) => await Assert.That(Vector4D.IsSubnormal(Vector4D.Create(value))).IsEqualTo(double.IsSubnormal(value) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsZeroDoubleTest(double value) => await Assert.That(Vector4D.IsZero(Vector4D.Create(value))).IsEqualTo((value == 0) ? Vector4D.AllBitsSet : Vector4D.Zero);

    [Test]
    public async Task AllBitsSetTest()
    {
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.X)).IsEqualTo(-1);
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.Y)).IsEqualTo(-1);
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.Z)).IsEqualTo(-1);
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector4D.AllBitsSet.W)).IsEqualTo(-1);
    }

    [Test]
    public async Task ConditionalSelectTest()
    {
        await Test(Vector4D.Create(1, 2, 3, 4), Vector4D.AllBitsSet, Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));
        await Test(Vector4D.Create(5, 6, 7, 8), Vector4D.Zero, Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));
        await Test(Vector4D.Create(1, 6, 3, 8), Vector256.Create(-1, 0, -1, 0).AsDouble().AsVector4D(), Vector4D.Create(1, 2, 3, 4), Vector4D.Create(5, 6, 7, 8));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(Vector4D expectedResult, Vector4D condition, Vector4D left, Vector4D right)
        {
            await Assert.That(Vector4D.ConditionalSelect(condition, left, right)).IsEqualTo(expectedResult);
        }
    }
#endif

    [Test]
    [Arguments(+0.0, +0.0, +0.0, +0.0, 0b0000)]
    [Arguments(-0.0, +1.0, -0.0, +0.0, 0b0101)]
    [Arguments(-0.0, -0.0, -0.0, -0.0, 0b1111)]
    public async Task ExtractMostSignificantBitsTest(double x, double y, double z, double w, uint expectedResult)
    {
        await Assert.That(Vector4D.Create(x, y, z, w).ExtractMostSignificantBits()).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0, 4.0)]
    [Arguments(5.0, 6.0, 7.0, 8.0)]
    public async Task GetElementTest(double x, double y, double z, double w)
    {
        await Assert.That(Vector4D.Create(x, y, z, w).GetElement(0)).IsEqualTo(x);
        await Assert.That(Vector4D.Create(x, y, z, w).GetElement(1)).IsEqualTo(y);
        await Assert.That(Vector4D.Create(x, y, z, w).GetElement(2)).IsEqualTo(z);
        await Assert.That(Vector4D.Create(x, y, z, w).GetElement(3)).IsEqualTo(w);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0, 4.0)]
    [Arguments(5.0, 6.0, 7.0, 8.0)]
    public async Task ShuffleTest(double x, double y, double z, double w)
    {
        await Assert.That(Vector4D.Shuffle(Vector4D.Create(x, y, z, w), 3, 2, 1, 0)).IsEqualTo(Vector4D.Create(w, z, y, x));
        await Assert.That(Vector4D.Shuffle(Vector4D.Create(x, y, z, w), 1, 0, 3, 2)).IsEqualTo(Vector4D.Create(y, x, w, z));
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0, 4.0, 10.0)]
    [Arguments(5.0, 6.0, 7.0, 8.0, 26.0)]
    public async Task SumTest(double x, double y, double z, double w, double expectedResult)
    {
        await Assert.That(Vector4D.Sum(Vector4D.Create(x, y, z, w))).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0, 4.0)]
    [Arguments(5.0, 6.0, 7.0, 8.0)]
    public async Task ToScalarTest(double x, double y, double z, double w)
    {
        await Assert.That(Vector4D.Create(x, y, z, w).ToScalar()).IsEqualTo(x);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0, 4.0)]
    [Arguments(5.0, 6.0, 7.0, 8.0)]
    public async Task WithElementTest(double x, double y, double z, double w)
    {
        var vector = Vector4D.Create(10);

        await Assert.That(vector.X).IsEqualTo(10);
        await Assert.That(vector.Y).IsEqualTo(10);
        await Assert.That(vector.Z).IsEqualTo(10);
        await Assert.That(vector.W).IsEqualTo(10);

        vector = vector.WithElement(0, x);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(10);
        await Assert.That(vector.Z).IsEqualTo(10);
        await Assert.That(vector.W).IsEqualTo(10);

        vector = vector.WithElement(1, y);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
        await Assert.That(vector.Z).IsEqualTo(10);
        await Assert.That(vector.W).IsEqualTo(10);

        vector = vector.WithElement(2, z);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
        await Assert.That(vector.Z).IsEqualTo(z);
        await Assert.That(vector.W).IsEqualTo(10);

        vector = vector.WithElement(3, w);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
        await Assert.That(vector.Z).IsEqualTo(z);
        await Assert.That(vector.W).IsEqualTo(w);
    }

    [Test]
    public async Task CreateScalarTest()
    {
        var vector = Vector4D.CreateScalar(double.Pi);

        await Assert.That(vector.X).IsEqualTo(double.Pi);
        await Assert.That(vector.Y).IsEqualTo(0);
        await Assert.That(vector.Z).IsEqualTo(0);
        await Assert.That(vector.W).IsEqualTo(0);

        vector = Vector4D.CreateScalar(double.E);

        await Assert.That(vector.X).IsEqualTo(double.E);
        await Assert.That(vector.Y).IsEqualTo(0);
        await Assert.That(vector.Z).IsEqualTo(0);
        await Assert.That(vector.W).IsEqualTo(0);
    }

    [Test]
    public async Task CreateScalarUnsafeTest()
    {
        var vector = Vector4D.CreateScalarUnsafe(double.Pi);
        await Assert.That(vector.X).IsEqualTo(double.Pi);

        vector = Vector4D.CreateScalarUnsafe(double.E);
        await Assert.That(vector.X).IsEqualTo(double.E);
    }
}