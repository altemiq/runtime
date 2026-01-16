namespace Altemiq.Numerics;

using System.Globalization;
using System.Runtime.InteropServices;

[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
public sealed class Vector3DTests
{
#if NET10_OR_GREATER
    private const int ElementCount = 3;
#endif

    [Test]
    public async Task Vector3DMarshalSizeTest()
    {
        await Assert.That(Marshal.SizeOf<Vector3D>()).IsEqualTo(24);
        await Assert.That(Marshal.SizeOf<Vector3D>(new())).IsEqualTo(24);
    }

    [Test]
    [Arguments(0.0, 1.0, 0.0)]
    [Arguments(1.0, 0.0, 1.0)]
    [Arguments(3.1434343, 1.1234123, 0.1234123)]
    [Arguments(1.0000001, 0.0000001, 2.0000001)]
    public async Task Vector3DIndexerGetTest(double x, double y, double z)
    {
        var vector = new Vector3D(x, y, z);

        await Assert.That(vector[0]).IsEqualTo(x);
        await Assert.That(vector[1]).IsEqualTo(y);
        await Assert.That(vector[2]).IsEqualTo(z);
    }

    [Test]
    [Arguments(0.0, 1.0, 0.0)]
    [Arguments(1.0, 0.0, 1.0)]
    [Arguments(3.1434343, 1.1234123, 0.1234123)]
    [Arguments(1.0000001, 0.0000001, 2.0000001)]
    public async Task Vector3DIndexerSetTest(double x, double y, double z)
    {
        var vector = new Vector3D(0.0, 0.0, 0.0)
        {
            [0] = x,
            [1] = y,
            [2] = z,
        };

        await Assert.That(vector[0]).IsEqualTo(x);
        await Assert.That(vector[1]).IsEqualTo(y);
        await Assert.That(vector[2]).IsEqualTo(z);
    }

    [Test]
    public async Task Vector3DCopyToTest()
    {
        var v1 = new Vector3D(2.0, 3.0, 3.3);

        var a = new double[4];
        var b = new double[3];

        await Assert.That(() => v1.CopyTo(null!, 0)).ThrowsExactly<NullReferenceException>();
        await Assert.That(() => v1.CopyTo(a, -1)).ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert.That(() => v1.CopyTo(a, a.Length)).ThrowsExactly<ArgumentOutOfRangeException>();
        await Assert.That(() => v1.CopyTo(a, a.Length - 2)).ThrowsExactly<ArgumentException>();

        v1.CopyTo(a, 1);
        v1.CopyTo(b);
        await Assert.That(a[0]).IsEqualTo(0.0);
        await Assert.That(a[1]).IsEqualTo(2.0);
        await Assert.That(a[2]).IsEqualTo(3.0);
        await Assert.That(a[3]).IsEqualTo(3.3);
        await Assert.That(b[0]).IsEqualTo(2.0);
        await Assert.That(b[1]).IsEqualTo(3.0);
        await Assert.That(b[2]).IsEqualTo(3.3);
    }

    [Test]
    public async Task Vector3DCopyToSpanTest()
    {
        var vector = new Vector3D(1.0, 2.0, 3.0);
        var destination = new double[3];

        await Assert.That(() => vector.CopyTo(new Span<double>(new double[2]))).ThrowsExactly<ArgumentException>();
        vector.CopyTo(destination.AsSpan());

        await Assert.That(vector.X).IsEqualTo(1.0);
        await Assert.That(vector.Y).IsEqualTo(2.0);
        await Assert.That(vector.Z).IsEqualTo(3.0);
        await Assert.That(destination[0]).IsEqualTo(vector.X);
        await Assert.That(destination[1]).IsEqualTo(vector.Y);
        await Assert.That(destination[2]).IsEqualTo(vector.Z);
    }

    [Test]
    public async Task Vector3DTryCopyToTest()
    {
        var vector = new Vector3D(1.0, 2.0, 3.0);
        var destination = new double[3];

        await Assert.That(vector.TryCopyTo(new(new double[2]))).IsFalse();
        await Assert.That(vector.TryCopyTo(destination.AsSpan())).IsTrue();

        await Assert.That(vector.X).IsEqualTo(1.0);
        await Assert.That(vector.Y).IsEqualTo(2.0);
        await Assert.That(vector.Z).IsEqualTo(3.0);
        await Assert.That(destination[0]).IsEqualTo(vector.X);
        await Assert.That(destination[1]).IsEqualTo(vector.Y);
        await Assert.That(destination[2]).IsEqualTo(vector.Z);
    }

    [Test]
    public async Task Vector3DGetHashCodeTest()
    {
        var v1 = new Vector3D(2.0, 3.0, 3.3);
        var v2 = new Vector3D(2.0, 3.0, 3.3);
        var v3 = new Vector3D(2.0, 3.0, 3.3);
        var v5 = new Vector3D(3.0, 2.0, 3.3);
        await Assert.That(v1.GetHashCode()).IsEqualTo(v1.GetHashCode());
        await Assert.That(v2.GetHashCode()).IsEqualTo(v1.GetHashCode());
        await Assert.That(v5.GetHashCode()).IsNotEqualTo(v1.GetHashCode());
        await Assert.That(v3.GetHashCode()).IsEqualTo(v1.GetHashCode());
        var v4 = new Vector3D(0.0, 0.0, 0.0);
        var v6 = new Vector3D(1.0, 0.0, 0.0);
        var v7 = new Vector3D(0.0, 1.0, 0.0);
        var v8 = new Vector3D(1.0, 1.0, 1.0);
        var v9 = new Vector3D(1.0, 1.0, 0.0);
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v7.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v8.GetHashCode()).IsNotEqualTo(v4.GetHashCode());
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v7.GetHashCode());
        await Assert.That(v6.GetHashCode()).IsNotEqualTo(v8.GetHashCode());
        await Assert.That(v9.GetHashCode()).IsNotEqualTo(v8.GetHashCode());
        await Assert.That(v9.GetHashCode()).IsNotEqualTo(v7.GetHashCode());
    }

    [Test]
    public async Task Vector3DToStringTest()
    {
        var separator = CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator;
        var enUsCultureInfo = new CultureInfo("en-US");

        var v1 = new Vector3D(2.0, 3.0, 3.3);
        var v1Str = v1.ToString();
        var expectedV1 = string.Format(CultureInfo.CurrentCulture
            , "<{1:G}{0} {2:G}{0} {3:G}>"
            , separator, 2, 3, 3.3);
        await Assert.That(v1Str).IsEquivalentTo(expectedV1);

        var v1StrFormatted = v1.ToString("c", CultureInfo.CurrentCulture);
        var expectedV1Formatted = string.Format(CultureInfo.CurrentCulture
            , "<{1:c}{0} {2:c}{0} {3:c}>"
            , separator, 2, 3, 3.3);
        await Assert.That(v1StrFormatted).IsEquivalentTo(expectedV1Formatted);

        var v2StrFormatted = v1.ToString("c", enUsCultureInfo);
        var expectedV2Formatted = string.Format(enUsCultureInfo
            , "<{1:c}{0} {2:c}{0} {3:c}>"
            , enUsCultureInfo.NumberFormat.NumberGroupSeparator, 2, 3, 3.3);
        await Assert.That(v2StrFormatted).IsEquivalentTo(expectedV2Formatted);

        var v3StrFormatted = v1.ToString("c");
        var expectedV3Formatted = string.Format(CultureInfo.CurrentCulture
            , "<{1:c}{0} {2:c}{0} {3:c}>"
            , separator, 2, 3, 3.3);
        await Assert.That(v3StrFormatted).IsEquivalentTo(expectedV3Formatted);
    }

    // A test for Cross (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DCrossTest()
    {
        var a = new Vector3D(1.0, 0.0, 0.0);
        var b = new Vector3D(0.0, 1.0, 0.0);

        var expected = new Vector3D(0.0, 0.0, 1.0);

        var actual = Vector3D.Cross(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Cross (Vector3Df, Vector3Df)
    // Cross test of the same vector
    [Test]
    public async Task Vector3DCrossTest1()
    {
        var a = new Vector3D(0.0, 1.0, 0.0);
        var b = new Vector3D(0.0, 1.0, 0.0);

        var expected = new Vector3D(0.0, 0.0, 0.0);
        var actual = Vector3D.Cross(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Cross (Vector3Df, Vector3Df)
    // Cross test of the same parallel vector
    [Test]
    public async Task Vector3DCrossSameParallelVectors()
    {
        var v = new Vector3D(-1, 1, 0);
        var n = Vector3D.Normalize(v);
        var actual = Vector3D.Cross(n, n);
        await Assert.That(actual).IsEqualTo(Vector3D.Zero);
    }

    // A test for Distance (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DDistanceTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var expected = Math.Sqrt(27);

        var actual = Vector3D.Distance(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Distance (Vector3Df, Vector3Df)
    // Distance from the same point
    [Test]
    public async Task Vector3DDistanceTest1()
    {
        var a = new Vector3D(1.051, 2.05, 3.478);
        var b = new Vector3D(new(1.051, 0.0), 1)
        {
            Y = 2.05,
            Z = 3.478,
        };

        var actual = Vector3D.Distance(a, b);
        await Assert.That(actual).IsEqualTo(0.0);
    }

    // A test for DistanceSquared (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DDistanceSquaredTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var expected = 27.0;

        var actual = Vector3D.DistanceSquared(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Dot (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DDotTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var expected = 32.0;

        var actual = Vector3D.Dot(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Dot (Vector3Df, Vector3Df)
    // Dot test for perpendicular vector
    [Test]
    public async Task Vector3DDotTest1()
    {
        var a = new Vector3D(1.55, 1.55, 1.0);
        var b = new Vector3D(2.5, 3.0, 1.5);
        var c = Vector3D.Cross(a, b);

        var expected = 0.0;
        var actual1 = Vector3D.Dot(a, c);
        var actual2 = Vector3D.Dot(b, c);
        await Assert.That(actual1).IsEqualTo(expected).Within(0.00000000001);
        await Assert.That(actual2).IsEqualTo(expected).Within(0.00000000001);
    }

    // A test for Length ()
    [Test]
    public async Task Vector3DLengthTest()
    {
        var a = new Vector2D(1.0, 2.0);

        var z = 3.0;

        var target = new Vector3D(a, z);

        var expected = Math.Sqrt(14.0);

        var actual = target.Length();
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Length ()
    // Length test where length is zero
    [Test]
    public async Task Vector3DLengthTest1()
    {
        var target = new Vector3D();

        var expected = 0.0;
        var actual = target.Length();
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for LengthSquared ()
    [Test]
    public async Task Vector3DLengthSquaredTest()
    {
        var a = new Vector2D(1.0, 2.0);

        var z = 3.0;

        var target = new Vector3D(a, z);

        var expected = 14.0;

        var actual = target.LengthSquared();
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Min (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DMinTest()
    {
        var a = new Vector3D(-1.0, 4.0, -3.0);
        var b = new Vector3D(2.0, 1.0, -1.0);

        var expected = new Vector3D(-1.0, 1.0, -3.0);
        var actual = Vector3D.Min(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Max (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DMaxTest()
    {
        var a = new Vector3D(-1.0, 4.0, -3.0);
        var b = new Vector3D(2.0, 1.0, -1.0);

        var expected = new Vector3D(2.0, 4.0, -1.0);
        var actual = Vector3D.Max(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Vector3DMinMaxCodeCoverageTest()
    {
        var min = Vector3D.Zero;
        var max = Vector3D.One;

        var actual =
            // Min.
            Vector3D.Min(min, max);
        await Assert.That(min).IsEqualTo(actual);

        actual = Vector3D.Min(max, min);
        await Assert.That(min).IsEqualTo(actual);

        // Max.
        actual = Vector3D.Max(min, max);
        await Assert.That(max).IsEqualTo(actual);

        actual = Vector3D.Max(max, min);
        await Assert.That(max).IsEqualTo(actual);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    [Test]
    public async Task Vector3DLerpTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var t = 0.5;

        var expected = new Vector3D(2.5, 3.5, 4.5);

        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test with factor zero
    [Test]
    public async Task Vector3DLerpTest1()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var t = 0.0;
        var expected = new Vector3D(1.0, 2.0, 3.0);
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test with factor one
    [Test]
    public async Task Vector3DLerpTest2()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var t = 1.0;
        var expected = new Vector3D(4.0, 5.0, 6.0);
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test with factor > 1
    [Test]
    public async Task Vector3DLerpTest3()
    {
        var a = new Vector3D(0.0, 0.0, 0.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var t = 2.0;
        var expected = new Vector3D(8.0, 10.0, 12.0);
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test with factor < 0
    [Test]
    public async Task Vector3DLerpTest4()
    {
        var a = new Vector3D(0.0, 0.0, 0.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var t = -2.0;
        var expected = new Vector3D(-8.0, -10.0, -12.0);
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test with special double value
    [Test]
    public async Task Vector3DLerpTest5()
    {
        var a = new Vector3D(45.67, 90.0, 0);
        var b = new Vector3D(double.PositiveInfinity, double.NegativeInfinity, 0);

        var t = 0.408;
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual.X).IsPositiveInfinity();
        await Assert.That(actual.Y).IsNegativeInfinity();
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test from the same point
    [Test]
    public async Task Vector3DLerpTest6()
    {
        var a = new Vector3D(1.68, 2.34, 5.43);
        var b = a;

        var t = 0.18;
        var expected = new Vector3D(1.68, 2.34, 5.43);
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected).Within(0.01);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test with values known to be inaccurate with the old lerp impl
    [Test]
    public async Task Vector3DLerpTest7()
    {
        var a = new Vector3D(0.44728136);
        var b = new Vector3D(0.46345946);

        var t = 0.26402435;

        var expected = new Vector3D(0.45155275);
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected).Within(0.0000001);
    }

    // A test for Lerp (Vector3Df, Vector3Df, double)
    // Lerp test with values known to be inaccurate with the old lerp impl
    // (Old code incorrectly gets 0.33333588)
    [Test]
    public async Task Vector3DLerpTest8()
    {
        var a = new Vector3D(-100);
        var b = new Vector3D(0.33333334);

        double t = 1;

        var expected = new Vector3D(0.33333334);
        var actual = Vector3D.Lerp(a, b, t);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Reflect (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DReflectTest()
    {
        var a = Vector3D.Normalize(new(1.0, 1.0, 1.0));

        // Reflect on XZ plane.
        var n = new Vector3D(0.0, 1.0, 0.0);
        var expected = new Vector3D(a.X, -a.Y, a.Z);
        var actual = Vector3D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected);

        // Reflect on XY plane.
        n = new(0.0, 0.0, 1.0);
        expected = new(a.X, a.Y, -a.Z);
        actual = Vector3D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected);

        // Reflect on YZ plane.
        n = new(1.0, 0.0, 0.0);
        expected = new(-a.X, a.Y, a.Z);
        actual = Vector3D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Reflect (Vector3Df, Vector3Df)
    // Reflection when normal and source are the same
    [Test]
    public async Task Vector3DReflectTest1()
    {
        var n = new Vector3D(0.45, 1.28, 0.86);
        n = Vector3D.Normalize(n);
        var a = n;

        var expected = -n;
        var actual = Vector3D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);
    }

    // A test for Reflect (Vector3Df, Vector3Df)
    // Reflection when normal and source are negation
    [Test]
    public async Task Vector3DReflectTest2()
    {
        var n = new Vector3D(0.45, 1.28, 0.86);
        n = Vector3D.Normalize(n);
        var a = -n;

        var expected = n;
        var actual = Vector3D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);
    }

    // A test for Reflect (Vector3Df, Vector3Df)
    // Reflection when normal and source are perpendicular (a dot n = 0)
    [Test]
    public async Task Vector3DReflectTest3()
    {
        var n = new Vector3D(0.45, 1.28, 0.86);
        var temp = new Vector3D(1.28, 0.45, 0.01);
        // find a perpendicular vector of n
        var a = Vector3D.Cross(temp, n);

        var expected = a;
        var actual = Vector3D.Reflect(a, n);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform(Vector3Df, Matrix4x4D)
    [Test]
    public async Task Vector3DTransformTest()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        var expected = new Vector3D(12.191987, 21.533493, 32.616024);

        var actual = Vector3D.Transform(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.LowQualityTolerance);
    }

    // A test for Clamp (Vector3Df, Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DClampTest()
    {
        var a = new Vector3D(0.5, 0.3, 0.33);
        var min = new Vector3D(0.0, 0.1, 0.13);
        var max = new Vector3D(1.0, 1.1, 1.13);

        // Normal case.
        // Case N1: specified value is in the range.
        var expected = new Vector3D(0.5, 0.3, 0.33);
        var actual = Vector3D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Normal case.
        // Case N2: specified value is bigger than max value.
        a = new(2.0, 3.0, 4.0);
        expected = max;
        actual = Vector3D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Case N3: specified value is smaller than max value.
        a = new(-2.0, -3.0, -4.0);
        expected = min;
        actual = Vector3D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Case N4: combination case.
        a = new(-2.0, 0.5, 4.0);
        expected = new(min.X, a.Y, max.Z);
        actual = Vector3D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // User specified min value is bigger than max value.
        max = new(0.0, 0.1, 0.13);
        min = new(1.0, 1.1, 1.13);

        // Case W1: specified value is in the range.
        a = new(0.5, 0.3, 0.33);
        expected = max;
        actual = Vector3D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Normal case.
        // Case W2: specified value is bigger than max and min value.
        a = new(2.0, 3.0, 4.0);
        expected = max;
        actual = Vector3D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);

        // Case W3: specified value is smaller than min and max value.
        a = new(-2.0, -3.0, -4.0);
        expected = max;
        actual = Vector3D.Clamp(a, min, max);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for TransformNormal (Vector3Df, Matrix4x4D)
    [Test]
    public async Task Vector3DTransformNormalTest()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        m.M41 = 10.0;
        m.M42 = 20.0;
        m.M43 = 30.0;

        var expected = new Vector3D(2.19198728, 1.53349364, 2.61602545);

        var actual = Vector3D.TransformNormal(v, m);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.MidQualityTolerance);
    }

    // A test for Transform (Vector3Df, QuaternionD)
    [Test]
    public async Task Vector3DTransformByQuaternionDTest()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);

        var m =
            Matrix4x4D.CreateRotationX(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationY(MathHelper.ToRadians(30.0)) *
            Matrix4x4D.CreateRotationZ(MathHelper.ToRadians(30.0));
        var q = QuaternionD.CreateFromRotationMatrix(m);

        var expected = Vector3D.Transform(v, m);
        var actual = Vector3D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected).Within(MathHelper.HighQualityTolerance);
    }

    // A test for Transform (Vector3Df, QuaternionD)
    // Transform vector3 with zero quaternion
    [Test]
    public async Task Vector3DTransformByQuaternionDTest1()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var q = new QuaternionD();
        var expected = Vector3D.Zero;

        var actual = Vector3D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Transform (Vector3Df, QuaternionD)
    // Transform vector3 with identity quaternion
    [Test]
    public async Task Vector3DTransformByQuaternionDTest2()
    {
        var v = new Vector3D(1.0, 2.0, 3.0);
        var q = QuaternionD.Identity;
        var expected = v;

        var actual = Vector3D.Transform(v, q);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector3Df)
    [Test]
    public async Task Vector3DNormalizeTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);

        var expected = new Vector3D(
            0.26726124191242438468455348087975,
            0.53452248382484876936910696175951,
            0.80178372573727315405366044263926);

        var actual = Vector3D.Normalize(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector3Df)
    // Normalize vector of length one
    [Test]
    public async Task Vector3DNormalizeTest1()
    {
        var a = new Vector3D(1.0, 0.0, 0.0);

        var expected = new Vector3D(1.0, 0.0, 0.0);
        var actual = Vector3D.Normalize(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Normalize (Vector3Df)
    // Normalize vector of length zero
    [Test]
    public async Task Vector3DNormalizeTest2()
    {
        var a = new Vector3D(0.0, 0.0, 0.0);

        var actual = Vector3D.Normalize(a);
        await Assert.That(actual)
            .Member(x => x.X, x => x.IsNaN()).And
            .Member(x => x.Y, x => x.IsNaN()).And
            .Member(x => x.Z, x => x.IsNaN());
    }

    // A test for operator - (Vector3Df)
    [Test]
    public async Task Vector3DUnaryNegationTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);

        var expected = new Vector3D(-1.0, -2.0, -3.0);

        var actual = -a;

        await Assert.That(actual).IsEqualTo(expected);
    }

    [Test]
    public async Task Vector3DUnaryNegationTest1()
    {
        var a = -new Vector3D(double.NaN, double.PositiveInfinity, double.NegativeInfinity);
        var b = -new Vector3D(0.0, 0.0, 0.0);
        await Assert.That(a.X).IsNaN();
        await Assert.That(a.Y).IsNegativeInfinity();
        await Assert.That(a.Z).IsPositiveInfinity();
        await Assert.That(b.X).IsEqualTo(0.0);
        await Assert.That(b.Y).IsEqualTo(0.0);
        await Assert.That(b.Z).IsEqualTo(0.0);
    }

    // A test for operator - (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DSubtractionTest()
    {
        var a = new Vector3D(4.0, 2.0, 3.0);

        var b = new Vector3D(1.0, 5.0, 7.0);

        var expected = new Vector3D(3.0, -3.0, -4.0);

        var actual = a - b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Vector3Df, double)
    [Test]
    public async Task Vector3DMultiplyOperatorTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);

        var factor = 2.0;

        var expected = new Vector3D(2.0, 4.0, 6.0);

        var actual = a * factor;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (double, Vector3Df)
    [Test]
    public async Task Vector3DMultiplyOperatorTest2()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);

        const double Factor = 2.0;

        var expected = new Vector3D(2.0, 4.0, 6.0);

        var actual = Factor * a;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator * (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DMultiplyOperatorTest3()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);

        var b = new Vector3D(4.0, 5.0, 6.0);

        var expected = new Vector3D(4.0, 10.0, 18.0);

        var actual = a * b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector3Df, double)
    [Test]
    public async Task Vector3DDivisionTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);

        var div = 2.0;

        var expected = new Vector3D(0.5, 1.0, 1.5);

        var actual = a / div;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DDivisionTest1()
    {
        var a = new Vector3D(4.0, 2.0, 3.0);

        var b = new Vector3D(1.0, 5.0, 6.0);

        var expected = new Vector3D(4.0, 0.4, 0.5);

        var actual = a / b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator / (Vector3Df, Vector3Df)
    // Divide by zero
    [Test]
    public async Task Vector3DDivisionTest2()
    {
        var a = new Vector3D(-2.0, 3.0, double.MaxValue);

        var div = 0.0;

        var actual = a / div;

        await Assert.That(actual.X).IsNegativeInfinity();
        await Assert.That(actual.Y).IsPositiveInfinity();
        await Assert.That(actual.Z).IsPositiveInfinity();
    }

    // A test for operator / (Vector3Df, Vector3Df)
    // Divide by zero
    [Test]
    public async Task Vector3DDivisionTest3()
    {
        var a = new Vector3D(0.047, -3.0, double.NegativeInfinity);
        var b = new Vector3D();

        var actual = a / b;

        await Assert.That(actual.X).IsPositiveInfinity();
        await Assert.That(actual.Y).IsNegativeInfinity();
        await Assert.That(actual.Z).IsNegativeInfinity();
    }

    // A test for operator + (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DAdditionTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(4.0, 5.0, 6.0);

        var expected = new Vector3D(5.0, 7.0, 9.0);

        var actual = a + b;

        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Vector3Df (double, double, double)
    [Test]
    public async Task Vector3DConstructorTest()
    {
        var x = 1.0;
        var y = 2.0;
        var z = 3.0;

        var target = new Vector3D(x, y, z);
        await Assert.That(target.X).IsEqualTo(x);
        await Assert.That(target.Y).IsEqualTo(y);
        await Assert.That(target.Z).IsEqualTo(z);
    }

    // A test for Vector3Df (Vector2D, double)
    [Test]
    public async Task Vector3DConstructorTest1()
    {
        var a = new Vector2D(1.0, 2.0);

        var z = 3.0;

        var target = new Vector3D(a, z);
        await Assert.That(target.X).IsEqualTo(a.X);
        await Assert.That(target.Y).IsEqualTo(a.Y);
        await Assert.That(target.Z).IsEqualTo(z);
    }

    // A test for Vector3Df ()
    // Constructor with no parameter
    [Test]
    public async Task Vector3DConstructorTest3()
    {
        var a = new Vector3D();

        await Assert.That(a.X).IsEqualTo(0.0);
        await Assert.That(a.Y).IsEqualTo(0.0);
        await Assert.That(a.Z).IsEqualTo(0.0);
    }

    // A test for Vector2D (double, double)
    // Constructor with special floating values
    [Test]
    public async Task Vector3DConstructorTest4()
    {
        var target = new Vector3D(double.NaN, double.MaxValue, double.PositiveInfinity);

        await Assert.That(target.X).IsNaN();
        await Assert.That(target.Y).IsEqualTo(double.MaxValue);
        await Assert.That(target.Z).IsPositiveInfinity();
    }

    // A test for Vector3Df (ReadOnlySpan<double>)
    [Test]
    public async Task Vector3DConstructorTest6()
    {
        var value = 1.0;
        var target = new Vector3D([value, value, value]);
        var expected = new Vector3D(value);

        await Assert.That(target).IsEqualTo(expected);
        await Assert.That(() => new Vector3D(new double[2])).Throws<ArgumentOutOfRangeException>();
    }

    // A test for Add (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DAddTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(5.0, 6.0, 7.0);

        var expected = new Vector3D(6.0, 8.0, 10.0);

        var actual = Vector3D.Add(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Divide (Vector3Df, double)
    [Test]
    public async Task Vector3DDivideTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var div = 2.0;
        var expected = new Vector3D(0.5, 1.0, 1.5);
        var actual = Vector3D.Divide(a, div);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Divide (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DDivideTest1()
    {
        var a = new Vector3D(1.0, 6.0, 7.0);
        var b = new Vector3D(5.0, 2.0, 3.0);

        var expected = new Vector3D(1.0 / 5.0, 6.0 / 2.0, 7.0 / 3.0);

        var actual = Vector3D.Divide(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Equals (object)
    [Test]
    public async Task Vector3DEqualsTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(1.0, 2.0, 3.0);

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

    // A test for Multiply (Vector3Df, double)
    [Test]
    public async Task Vector3DMultiplyTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        const double Factor = 2.0;
        var expected = new Vector3D(2.0, 4.0, 6.0);
        var actual = Vector3D.Multiply(a, Factor);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (double, Vector3Df)
    [Test]
    public async Task Vector3DMultiplyTest2()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        const double Factor = 2.0;
        var expected = new Vector3D(2.0, 4.0, 6.0);
        var actual = Vector3D.Multiply(Factor, a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Multiply (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DMultiplyTest3()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(5.0, 6.0, 7.0);

        var expected = new Vector3D(5.0, 12.0, 21.0);

        var actual = Vector3D.Multiply(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Negate (Vector3Df)
    [Test]
    public async Task Vector3DNegateTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);

        var expected = new Vector3D(-1.0, -2.0, -3.0);

        var actual = Vector3D.Negate(a);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for operator != (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DInequalityTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(1.0, 2.0, 3.0);

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

    // A test for operator == (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DEqualityTest()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(1.0, 2.0, 3.0);

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

    // A test for Subtract (Vector3Df, Vector3Df)
    [Test]
    public async Task Vector3DSubtractTest()
    {
        var a = new Vector3D(1.0, 6.0, 3.0);
        var b = new Vector3D(5.0, 2.0, 3.0);

        var expected = new Vector3D(-4.0, 4.0, 0.0);

        var actual = Vector3D.Subtract(a, b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for One
    [Test]
    public async Task Vector3DOneTest()
    {
        var val = new Vector3D(1.0, 1.0, 1.0);
        await Assert.That(Vector3D.One).IsEqualTo(val);
    }

    // A test for UnitX
    [Test]
    public async Task Vector3DUnitXTest()
    {
        var val = new Vector3D(1.0, 0.0, 0.0);
        await Assert.That(Vector3D.UnitX).IsEqualTo(val);
    }

    // A test for UnitY
    [Test]
    public async Task Vector3DUnitYTest()
    {
        var val = new Vector3D(0.0, 1.0, 0.0);
        await Assert.That(Vector3D.UnitY).IsEqualTo(val);
    }

    // A test for UnitZ
    [Test]
    public async Task Vector3DUnitZTest()
    {
        var val = new Vector3D(0.0, 0.0, 1.0);
        await Assert.That(Vector3D.UnitZ).IsEqualTo(val);
    }

    // A test for Zero
    [Test]
    public async Task Vector3DZeroTest()
    {
        var val = new Vector3D(0.0, 0.0, 0.0);
        await Assert.That(Vector3D.Zero).IsEqualTo(val);
    }

    // A test for Equals (Vector3Df)
    [Test]
    public async Task Vector3DEqualsTest1()
    {
        var a = new Vector3D(1.0, 2.0, 3.0);
        var b = new Vector3D(1.0, 2.0, 3.0);

        // case 1: compare between same values
        var expected = true;
        var actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);

        // case 2: compare between different values
        b.X = 10.0;
        expected = false;
        actual = a.Equals(b);
        await Assert.That(actual).IsEqualTo(expected);
    }

    // A test for Vector3Df (double)
    [Test]
    public async Task Vector3DConstructorTest5()
    {
        var value = 1.0;
        var target = new Vector3D(value);

        var expected = new Vector3D(value, value, value);
        await Assert.That(target).IsEqualTo(expected);

        value = 2.0;
        target = new(value);
        expected = new(value, value, value);
        await Assert.That(target).IsEqualTo(expected);
    }

    // A test for Vector3Df comparison involving NaN values
    [Test]
    public async Task Vector3DEqualsNaNTest()
    {
        var a = new Vector3D(double.NaN, 0, 0);
        var b = new Vector3D(0, double.NaN, 0);
        var c = new Vector3D(0, 0, double.NaN);

        await Assert.That(a == Vector3D.Zero).IsFalse();
        await Assert.That(b == Vector3D.Zero).IsFalse();
        await Assert.That(c == Vector3D.Zero).IsFalse();

        await Assert.That(a != Vector3D.Zero).IsTrue();
        await Assert.That(b != Vector3D.Zero).IsTrue();
        await Assert.That(c != Vector3D.Zero).IsTrue();

        await Assert.That(a.Equals(Vector3D.Zero)).IsFalse();
        await Assert.That(b.Equals(Vector3D.Zero)).IsFalse();
        await Assert.That(c.Equals(Vector3D.Zero)).IsFalse();

        await Assert.That(a.Equals(a)).IsTrue();
        await Assert.That(b.Equals(b)).IsTrue();
        await Assert.That(c.Equals(c)).IsTrue();
    }

    [Test]
    public async Task Vector3DAbsTest()
    {
        var v1 = new Vector3D(-2.5, 2.0, 0.5);
        var v3 = Vector3D.Abs(new(0.0, double.NegativeInfinity, double.NaN));
        var v = Vector3D.Abs(v1);
        await Assert.That(v.X).IsEqualTo(2.5);
        await Assert.That(v.Y).IsEqualTo(2.0);
        await Assert.That(v.Z).IsEqualTo(0.5);
        await Assert.That(v3.X).IsEqualTo(0.0);
        await Assert.That(v3.Y).IsPositiveInfinity();
        await Assert.That(v3.Z).IsNaN();
    }

    [Test]
    public async Task Vector3DSqrtTest()
    {
        var a = new Vector3D(-2.5, 2.0, 0.5);
        var b = new Vector3D(5.5, 4.5, 16.5);
        await Assert.That((int)Vector3D.SquareRoot(b).X).IsEqualTo(2);
        await Assert.That((int)Vector3D.SquareRoot(b).Y).IsEqualTo(2);
        await Assert.That((int)Vector3D.SquareRoot(b).Z).IsEqualTo(4);
        await Assert.That(Vector3D.SquareRoot(a).X).IsNaN();
    }

    // A test to make sure these types are blittable directly into GPU buffer memory layouts
    [Test]
    public async Task Vector3DSizeofTest()
    {
        int sizeofVector3D;
        int sizeofVector3D2;
        int sizeofVector3DPlusDouble;
        int sizeofVector3DPlusDouble2;

        unsafe
        {
            sizeofVector3D = sizeof(Vector3D);
            sizeofVector3D2 = sizeof(Vector3D_2x);
            sizeofVector3DPlusDouble = sizeof(Vector3DPlusDouble);
            sizeofVector3DPlusDouble2 = sizeof(Vector3DPlusDouble_2x);
        }

        await Assert.That(sizeofVector3D).IsEqualTo(24);
        await Assert.That(sizeofVector3D2).IsEqualTo(48);
        await Assert.That(sizeofVector3DPlusDouble).IsEqualTo(32);
        await Assert.That(sizeofVector3DPlusDouble2).IsEqualTo(64);
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector3D_2x
    {
        private Vector3D _a;
        private Vector3D _b;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector3DPlusDouble
    {
        private Vector3D _v;
        private readonly double _f;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Vector3DPlusDouble_2x
    {
        private Vector3DPlusDouble _a;
        private Vector3DPlusDouble _b;
    }

    [Test]
    public async Task SetFieldsTest()
    {
        var v3 = new Vector3D(4, 5, 6)
        {
            X = 1.0,
            Y = 2.0,
            Z = 3.0,
        };
        await Assert.That(v3.X).IsEqualTo(1.0);
        await Assert.That(v3.Y).IsEqualTo(2.0);
        await Assert.That(v3.Z).IsEqualTo(3.0);
        var v4 = v3;
        v4.Y = 0.5;
        v4.Z = 2.2;
        await Assert.That(v4.X).IsEqualTo(1.0);
        await Assert.That(v4.Y).IsEqualTo(0.5);
        await Assert.That(v4.Z).IsEqualTo(2.2);
        await Assert.That(v3.Y).IsEqualTo(2.0);

        var before = new Vector3D(1, 2, 3);
        var after = before;
        after.X = 500.0;
        await Assert.That(after).IsNotEqualTo(before);
    }

    [Test]
    public async Task EmbeddedVectorSetFields()
    {
        var evo = new EmbeddedVectorObject();
        evo.FieldVector.X = 5.0;
        evo.FieldVector.Y = 5.0;
        evo.FieldVector.Z = 5.0;
        await Assert.That(evo.FieldVector.X).IsEqualTo(5.0);
        await Assert.That(evo.FieldVector.Y).IsEqualTo(5.0);
        await Assert.That(evo.FieldVector.Z).IsEqualTo(5.0);
    }

    private class EmbeddedVectorObject
    {
        public Vector3D FieldVector;
    }

#if NET9_0_OR_GREATER
    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.CosDouble))]
    public async Task CosDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector3D.Cos(Vector3D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.ExpDouble))]
    public async Task ExpDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector3D.Exp(Vector3D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.LogDouble))]
    public async Task LogDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector3D.Log(Vector3D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.Log2Double))]
    public async Task Log2DoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector3D.Log2(Vector3D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.FusedMultiplyAddDouble))]
    public async Task FusedMultiplyAddDoubleTest(double left, double right, double addend, double expectedResult)
    {
        await Assert.That(Vector3D.FusedMultiplyAdd(Vector3D.Create(left), Vector3D.Create(right), Vector3D.Create(addend))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
        await Assert.That(Vector3D.MultiplyAddEstimate(Vector3D.Create(left), Vector3D.Create(right), Vector3D.Create(addend))).IsEqualTo(Vector3D.Create(double.MultiplyAddEstimate(left, right, addend))).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.ClampDouble))]
    public async Task ClampDoubleTest(double x, double min, double max, double expectedResult)
    {
        var actualResult = Vector3D.Clamp(Vector3D.Create(x), Vector3D.Create(min), Vector3D.Create(max));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.CopySignDouble))]
    public async Task CopySignDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.CopySign(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.DegreesToRadiansDouble))]
    public async Task DegreesToRadiansDoubleTest(double value, double expectedResult, double variance)
    {
        await Assert.That(Vector3D.DegreesToRadians(Vector3D.Create(-value))).IsEqualTo(Vector3D.Create(-expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.DegreesToRadians(Vector3D.Create(+value))).IsEqualTo(Vector3D.Create(+expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.HypotDouble))]
    public async Task HypotDoubleTest(double x, double y, double expectedResult, double variance)
    {
        await Assert.That(Vector3D.Hypot(Vector3D.Create(-x), Vector3D.Create(-y))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.Hypot(Vector3D.Create(-x), Vector3D.Create(+y))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.Hypot(Vector3D.Create(+x), Vector3D.Create(-y))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.Hypot(Vector3D.Create(+x), Vector3D.Create(+y))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));

        await Assert.That(Vector3D.Hypot(Vector3D.Create(-y), Vector3D.Create(-x))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.Hypot(Vector3D.Create(-y), Vector3D.Create(+x))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.Hypot(Vector3D.Create(+y), Vector3D.Create(-x))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.Hypot(Vector3D.Create(+y), Vector3D.Create(+x))).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.LerpDouble))]
    public async Task LerpDoubleTest(double x, double y, double amount, double expectedResult)
    {
        await Assert.That(Vector3D.Lerp(Vector3D.Create(+x), Vector3D.Create(+y), Vector3D.Create(amount))).IsEqualTo(Vector3D.Create(+expectedResult)).Within(Vector3D.Zero);
        await Assert.That(Vector3D.Lerp(Vector3D.Create(-x), Vector3D.Create(-y), Vector3D.Create(amount))).IsEqualTo(Vector3D.Create((expectedResult == 0.0) ? expectedResult : -expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxDouble))]
    public async Task MaxDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.Max(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxMagnitudeDouble))]
    public async Task MaxMagnitudeDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.MaxMagnitude(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxMagnitudeNumberDouble))]
    public async Task MaxMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.MaxMagnitudeNumber(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MaxNumberDouble))]
    public async Task MaxNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.MaxNumber(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinDouble))]
    public async Task MinDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.Min(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinMagnitudeDouble))]
    public async Task MinMagnitudeDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.MinMagnitude(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinMagnitudeNumberDouble))]
    public async Task MinMagnitudeNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.MinMagnitudeNumber(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.MinNumberDouble))]
    public async Task MinNumberDoubleTest(double x, double y, double expectedResult)
    {
        var actualResult = Vector3D.MinNumber(Vector3D.Create(x), Vector3D.Create(y));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RadiansToDegreesDouble))]
    public async Task RadiansToDegreesDoubleTest(double value, double expectedResult, double variance)
    {
        await Assert.That(Vector3D.RadiansToDegrees(Vector3D.Create(-value))).IsEqualTo(Vector3D.Create(-expectedResult)).Within(Vector3D.Create(variance));
        await Assert.That(Vector3D.RadiansToDegrees(Vector3D.Create(+value))).IsEqualTo(Vector3D.Create(+expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundDouble))]
    public async Task RoundDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector3D.Round(Vector3D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundAwayFromZeroDouble))]
    public async Task RoundAwayFromZeroDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector3D.Round(Vector3D.Create(value), MidpointRounding.AwayFromZero);
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.RoundToEvenDouble))]
    public async Task RoundToEvenDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector3D.Round(Vector3D.Create(value), MidpointRounding.ToEven);
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.SinDouble))]
    public async Task SinDoubleTest(double value, double expectedResult, double variance)
    {
        var actualResult = Vector3D.Sin(Vector3D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Create(variance));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.SinCosDouble))]
    public async Task SinCosDoubleTest(double value, double expectedResultSin, double expectedResultCos, double allowedVarianceSin, double allowedVarianceCos)
    {
        var (resultSin, resultCos) = Vector3D.SinCos(Vector3D.Create(value));
        await Assert.That(resultSin).IsEqualTo(Vector3D.Create(expectedResultSin)).Within(Vector3D.Create(allowedVarianceSin));
        await Assert.That(resultCos).IsEqualTo(Vector3D.Create(expectedResultCos)).Within(Vector3D.Create(allowedVarianceCos));
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.TruncateDouble))]
    public async Task TruncateDoubleTest(double value, double expectedResult)
    {
        var actualResult = Vector3D.Truncate(Vector3D.Create(value));
        await Assert.That(actualResult).IsEqualTo(Vector3D.Create(expectedResult)).Within(Vector3D.Zero);
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
            var input1 = Vector3D.Create(value1);
            var input2 = Vector3D.Create(value2);

            await Assert.That(Vector3D.All(input1, value1)).IsTrue();
            await Assert.That(Vector3D.All(input2, value2)).IsTrue();
            await Assert.That(Vector3D.All(input1.WithElement(0, value2), value1)).IsFalse();
            await Assert.That(Vector3D.All(input2.WithElement(0, value1), value2)).IsFalse();
            await Assert.That(Vector3D.All(input1, value2)).IsFalse();
            await Assert.That(Vector3D.All(input2, value1)).IsFalse();
            await Assert.That(Vector3D.All(input1.WithElement(0, value2), value2)).IsFalse();
            await Assert.That(Vector3D.All(input2.WithElement(0, value1), value1)).IsFalse();

            await Assert.That(Vector3D.Any(input1, value1)).IsTrue();
            await Assert.That(Vector3D.Any(input2, value2)).IsTrue();
            await Assert.That(Vector3D.Any(input1.WithElement(0, value2), value1)).IsTrue();
            await Assert.That(Vector3D.Any(input2.WithElement(0, value1), value2)).IsTrue();
            await Assert.That(Vector3D.Any(input1, value2)).IsFalse();
            await Assert.That(Vector3D.Any(input2, value1)).IsFalse();
            await Assert.That(Vector3D.Any(input1.WithElement(0, value2), value2)).IsTrue();
            await Assert.That(Vector3D.Any(input2.WithElement(0, value1), value1)).IsTrue();

            await Assert.That(Vector3D.None(input1, value1)).IsFalse();
            await Assert.That(Vector3D.None(input2, value2)).IsFalse();
            await Assert.That(Vector3D.None(input1.WithElement(0, value2), value1)).IsFalse();
            await Assert.That(Vector3D.None(input2.WithElement(0, value1), value2)).IsFalse();
            await Assert.That(Vector3D.None(input1, value2)).IsTrue();
            await Assert.That(Vector3D.None(input2, value1)).IsTrue();
            await Assert.That(Vector3D.None(input1.WithElement(0, value2), value2)).IsFalse();
            await Assert.That(Vector3D.None(input2.WithElement(0, value1), value1)).IsFalse();
        }
    }

    [Test]
    public async Task AllAnyNoneTest_AllBitsSet()
    {
        await Test(BitConverter.Int64BitsToDouble(-1));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value)
        {
            var input = Vector3D.Create(value);

            await Assert.That(Vector3D.All(input, value)).IsFalse();
            await Assert.That(Vector3D.Any(input, value)).IsFalse();
            await Assert.That(Vector3D.None(input, value)).IsTrue();
        }
    }

    [Test]
    public async Task AllAnyNoneWhereAllBitsSetTest()
    {
        await Test(BitConverter.Int64BitsToDouble(-1), 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double allBitsSet, double value2)
        {
            var input1 = Vector3D.Create(allBitsSet);
            var input2 = Vector3D.Create(value2);

            await Assert.That(Vector3D.AllWhereAllBitsSet(input1)).IsTrue();
            await Assert.That(Vector3D.AllWhereAllBitsSet(input2)).IsFalse();
            await Assert.That(Vector3D.AllWhereAllBitsSet(input1.WithElement(0, value2))).IsFalse();
            await Assert.That(Vector3D.AllWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsFalse();

            await Assert.That(Vector3D.AnyWhereAllBitsSet(input1)).IsTrue();
            await Assert.That(Vector3D.AnyWhereAllBitsSet(input2)).IsFalse();
            await Assert.That(Vector3D.AnyWhereAllBitsSet(input1.WithElement(0, value2))).IsTrue();
            await Assert.That(Vector3D.AnyWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsTrue();

            await Assert.That(Vector3D.NoneWhereAllBitsSet(input1)).IsFalse();
            await Assert.That(Vector3D.NoneWhereAllBitsSet(input2)).IsTrue();
            await Assert.That(Vector3D.NoneWhereAllBitsSet(input1.WithElement(0, value2))).IsFalse();
            await Assert.That(Vector3D.NoneWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsFalse();
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfDoubleTest()
    {
        await Test(3, 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value1, double value2)
        {
            var input1 = Vector3D.Create(value1);
            var input2 = Vector3D.Create(value2);

            await Assert.That(Vector3D.Count(input1, value1)).IsEqualTo(ElementCount);
            await Assert.That(Vector3D.Count(input2, value2)).IsEqualTo(ElementCount);
            await Assert.That(Vector3D.Count(input1.WithElement(0, value2), value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.Count(input2.WithElement(0, value1), value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.Count(input1, value2)).IsEqualTo(0);
            await Assert.That(Vector3D.Count(input2, value1)).IsEqualTo(0);
            await Assert.That(Vector3D.Count(input1.WithElement(0, value2), value2)).IsEqualTo(1);
            await Assert.That(Vector3D.Count(input2.WithElement(0, value1), value1)).IsEqualTo(1);

            await Assert.That(Vector3D.IndexOf(input1, value1)).IsEqualTo(0);
            await Assert.That(Vector3D.IndexOf(input2, value2)).IsEqualTo(0);
            await Assert.That(Vector3D.IndexOf(input1.WithElement(0, value2), value1)).IsEqualTo(1);
            await Assert.That(Vector3D.IndexOf(input2.WithElement(0, value1), value2)).IsEqualTo(1);
            await Assert.That(Vector3D.IndexOf(input1, value2)).IsEqualTo(-1);
            await Assert.That(Vector3D.IndexOf(input2, value1)).IsEqualTo(-1);
            await Assert.That(Vector3D.IndexOf(input1.WithElement(0, value2), value2)).IsEqualTo(0);
            await Assert.That(Vector3D.IndexOf(input2.WithElement(0, value1), value1)).IsEqualTo(0);

            await Assert.That(Vector3D.LastIndexOf(input1, value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.LastIndexOf(input2, value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.LastIndexOf(input1.WithElement(0, value2), value1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.LastIndexOf(input2.WithElement(0, value1), value2)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.LastIndexOf(input1, value2)).IsEqualTo(-1);
            await Assert.That(Vector3D.LastIndexOf(input2, value1)).IsEqualTo(-1);
            await Assert.That(Vector3D.LastIndexOf(input1.WithElement(0, value2), value2)).IsEqualTo(0);
            await Assert.That(Vector3D.LastIndexOf(input2.WithElement(0, value1), value1)).IsEqualTo(0);
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfDoubleTest_AllBitsSet()
    {
        await  Test(BitConverter.Int64BitsToDouble(-1));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double value)
        {
            var input = Vector3D.Create(value);

            await Assert.That(Vector3D.Count(input, value)).IsEqualTo(0);
            await Assert.That(Vector3D.IndexOf(input, value)).IsEqualTo(-1);
            await Assert.That(Vector3D.LastIndexOf(input, value)).IsEqualTo(-1);
        }
    }

    [Test]
    public async Task CountIndexOfLastIndexOfWhereAllBitsSetDoubleTest()
    {
        await  Test(BitConverter.Int64BitsToDouble(-1), 2);

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(double allBitsSet, double value2)
        {
            var input1 = Vector3D.Create(allBitsSet);
            var input2 = Vector3D.Create(value2);

            await Assert.That(Vector3D.CountWhereAllBitsSet(input1)).IsEqualTo(ElementCount);
            await Assert.That(Vector3D.CountWhereAllBitsSet(input2)).IsEqualTo(0);
            await Assert.That(Vector3D.CountWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.CountWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(1);

            await Assert.That(Vector3D.IndexOfWhereAllBitsSet(input1)).IsEqualTo(0);
            await Assert.That(Vector3D.IndexOfWhereAllBitsSet(input2)).IsEqualTo(-1);
            await Assert.That(Vector3D.IndexOfWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(1);
            await Assert.That(Vector3D.IndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(0);

            await Assert.That(Vector3D.LastIndexOfWhereAllBitsSet(input1)).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.LastIndexOfWhereAllBitsSet(input2)).IsEqualTo(-1);
            await Assert.That(Vector3D.LastIndexOfWhereAllBitsSet(input1.WithElement(0, value2))).IsEqualTo(ElementCount - 1);
            await Assert.That(Vector3D.LastIndexOfWhereAllBitsSet(input2.WithElement(0, allBitsSet))).IsEqualTo(0);
        }
    }

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsEvenIntegerTest(double value) => await Assert.That(Vector3D.IsEvenInteger(Vector3D.Create(value))).IsEqualTo(double.IsEvenInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsFiniteTest(double value) => await Assert.That(Vector3D.IsFinite(Vector3D.Create(value))).IsEqualTo(double.IsFinite(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsInfinityTest(double value) => await Assert.That(Vector3D.IsInfinity(Vector3D.Create(value))).IsEqualTo(double.IsInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsIntegerTest(double value) => await Assert.That(Vector3D.IsInteger(Vector3D.Create(value))).IsEqualTo(double.IsInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNaNTest(double value) => await Assert.That(Vector3D.IsNaN(Vector3D.Create(value))).IsEqualTo(double.IsNaN(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNegativeTest(double value) => await Assert.That(Vector3D.IsNegative(Vector3D.Create(value))).IsEqualTo(double.IsNegative(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNegativeInfinityTest(double value) => await Assert.That(Vector3D.IsNegativeInfinity(Vector3D.Create(value))).IsEqualTo(double.IsNegativeInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsNormalTest(double value) => await Assert.That(Vector3D.IsNormal(Vector3D.Create(value))).IsEqualTo(double.IsNormal(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsOddIntegerTest(double value) => await Assert.That(Vector3D.IsOddInteger(Vector3D.Create(value))).IsEqualTo(double.IsOddInteger(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsPositiveTest(double value) => await Assert.That(Vector3D.IsPositive(Vector3D.Create(value))).IsEqualTo(double.IsPositive(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsPositiveInfinityTest(double value) => await Assert.That(Vector3D.IsPositiveInfinity(Vector3D.Create(value))).IsEqualTo(double.IsPositiveInfinity(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsSubnormalTest(double value) => await Assert.That(Vector3D.IsSubnormal(Vector3D.Create(value))).IsEqualTo(double.IsSubnormal(value) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    [MethodDataSource(typeof(GenericMathTestMemberData), nameof(GenericMathTestMemberData.IsTestDouble))]
    public async Task IsZeroDoubleTest(double value) => await Assert.That(Vector3D.IsZero(Vector3D.Create(value))).IsEqualTo((value == 0) ? Vector3D.AllBitsSet : Vector3D.Zero);

    [Test]
    public async Task AllBitsSetTest()
    {
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector3D.AllBitsSet.X)).IsEqualTo(-1);
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector3D.AllBitsSet.Y)).IsEqualTo(-1);
        await Assert.That(BitConverter.DoubleToInt64Bits(Vector3D.AllBitsSet.Z)).IsEqualTo(-1);
    }

    [Test]
    public async Task ConditionalSelectTest()
    {
        await Test(Vector3D.Create(1, 2, 3), Vector3D.AllBitsSet, Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));
        await  Test(Vector3D.Create(5, 6, 7), Vector3D.Zero, Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));
        await  Test(Vector3D.Create(1, 6, 3), Vector256.Create(-1, 0, -1, 0).AsDouble().AsVector3D(), Vector3D.Create(1, 2, 3), Vector3D.Create(5, 6, 7));

        [MethodImpl(MethodImplOptions.NoInlining)]
        async Task Test(Vector3D expectedResult, Vector3D condition, Vector3D left, Vector3D right)
        {
            await Assert.That(Vector3D.ConditionalSelect(condition, left, right)).IsEqualTo(expectedResult);
        }
    }
#endif

    [Test]
    [Arguments(+0.0, +0.0, +0.0, 0b000)]
    [Arguments(-0.0, +1.0, -0.0, 0b101)]
    [Arguments(-0.0, -0.0, -0.0, 0b111)]
    public async Task ExtractMostSignificantBitsTest(double x, double y, double z, uint expectedResult)
    {
        await Assert.That(Vector3D.Create(x, y, z).ExtractMostSignificantBits()).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0)]
    [Arguments(5.0, 6.0, 7.0)]
    public async Task GetElementTest(double x, double y, double z)
    {
        await Assert.That(Vector3D.Create(x, y, z).GetElement(0)).IsEqualTo(x);
        await Assert.That(Vector3D.Create(x, y, z).GetElement(1)).IsEqualTo(y);
        await Assert.That(Vector3D.Create(x, y, z).GetElement(2)).IsEqualTo(z);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0)]
    [Arguments(5.0, 6.0, 7.0)]
    public async Task ShuffleTest(double x, double y, double z)
    {
        await Assert.That(Vector3D.Shuffle(Vector3D.Create(x, y, z), 2, 1, 0)).IsEqualTo(Vector3D.Create(z, y, x));
        await Assert.That(Vector3D.Shuffle(Vector3D.Create(x, y, z), 1, 0, 2)).IsEqualTo(Vector3D.Create(y, x, z));
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0, 6.0)]
    [Arguments(5.0, 6.0, 7.0, 18.0)]
    public async Task SumTest(double x, double y, double z, double expectedResult)
    {
        await Assert.That(Vector3D.Sum(Vector3D.Create(x, y, z))).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0)]
    [Arguments(5.0, 6.0, 7.0)]
    public async Task ToScalarTest(double x, double y, double z)
    {
        await Assert.That(Vector3D.Create(x, y, z).ToScalar()).IsEqualTo(x);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0)]
    [Arguments(5.0, 6.0, 7.0)]
    public async Task WithElementTest(double x, double y, double z)
    {
        var vector = Vector3D.Create(10);

        await Assert.That(vector.X).IsEqualTo(10);
        await Assert.That(vector.Y).IsEqualTo(10);
        await Assert.That(vector.Z).IsEqualTo(10);

        vector = vector.WithElement(0, x);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(10);
        await Assert.That(vector.Z).IsEqualTo(10);

        vector = vector.WithElement(1, y);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
        await Assert.That(vector.Z).IsEqualTo(10);

        vector = vector.WithElement(2, z);

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
        await Assert.That(vector.Z).IsEqualTo(z);
    }

    [Test]
    [Arguments(1.0, 2.0, 3.0)]
    [Arguments(5.0, 6.0, 7.0)]
    public async Task AsVector2DTest(double x, double y, double z)
    {
        var vector = Vector3D.Create(x, y, z).AsVector2D();

        await Assert.That(vector.X).IsEqualTo(x);
        await Assert.That(vector.Y).IsEqualTo(y);
    }

    [Test]
    public async Task CreateScalarTest()
    {
        var vector = Vector3D.CreateScalar(double.Pi);

        await Assert.That(vector.X).IsEqualTo(double.Pi);
        await Assert.That(vector.Y).IsEqualTo(0.0);
        await Assert.That(vector.Z).IsEqualTo(0.0);

        vector = Vector3D.CreateScalar(double.E);

        await Assert.That(vector.X).IsEqualTo(double.E);
        await Assert.That(vector.Y).IsEqualTo(0.0);
        await Assert.That(vector.Z).IsEqualTo(0.0);
    }

    [Test]
    public async Task CreateScalarUnsafeTest()
    {
        var vector = Vector3D.CreateScalarUnsafe(double.Pi);
        await Assert.That(vector.X).IsEqualTo(double.Pi);

        vector = Vector3D.CreateScalarUnsafe(double.E);
        await Assert.That(vector.X).IsEqualTo(double.E);
    }
}