namespace Altemiq.Numerics;

public static class MatrixEqualsAssertion
{
    public static Matrix3x2DEqualsAssertion IsEqualTo(this TUnit.Assertions.Core.IAssertionSource<Matrix3x2D> source, Matrix3x2D expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
    {
        source.Context.ExpressionBuilder.Append(".IsEqualTo(" + string.Join(", ", new[] { expectedExpression }.Where(e => e != null)) + ")");
        return new(source.Context, expected);
    }

    public static Matrix4x4DEqualsAssertion IsEqualTo(this TUnit.Assertions.Core.IAssertionSource<Matrix4x4D> source, Matrix4x4D expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
    {
        source.Context.ExpressionBuilder.Append(".IsEqualTo(" + string.Join(", ", new[] { expectedExpression }.Where(e => e != null)) + ")");
        return new(source.Context, expected);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
public class Matrix3x2DEqualsAssertion(TUnit.Assertions.Core.AssertionContext<Matrix3x2D> context, Matrix3x2D expected)
    : TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Matrix3x2D, Matrix3x2D>(context, expected)
{
    public TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Matrix3x2D, Matrix3x2D> Within(double tolerance) =>
        this.Within(new Matrix3x2D(tolerance, tolerance, tolerance, tolerance, tolerance, tolerance));

    protected override bool HasToleranceValue() => true;

    protected override bool IsWithinTolerance(Matrix3x2D actual, Matrix3x2D expected, Matrix3x2D tolerance) =>
        MathHelper.IsWithinToleranceCore(actual.M11, expected.M11, tolerance.M11)
        && MathHelper.IsWithinToleranceCore(actual.M12, expected.M12, tolerance.M12)
        && MathHelper.IsWithinToleranceCore(actual.M21, expected.M21, tolerance.M21)
        && MathHelper.IsWithinToleranceCore(actual.M22, expected.M22, tolerance.M22)
        && MathHelper.IsWithinToleranceCore(actual.M31, expected.M31, tolerance.M31)
        && MathHelper.IsWithinToleranceCore(actual.M32, expected.M32, tolerance.M32);

    protected override object CalculateDifference(Matrix3x2D actual, Matrix3x2D expected) =>
        new Matrix3x2D(
            double.Abs(actual.M11 - expected.M11),
            double.Abs(actual.M12 - expected.M12),
            double.Abs(actual.M21 - expected.M21),
            double.Abs(actual.M22 - expected.M22),
            double.Abs(actual.M31 - expected.M31),
            double.Abs(actual.M32 - expected.M32));


    protected override bool AreExactlyEqual(Matrix3x2D actual, Matrix3x2D expected) => actual == expected;
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("ReSharper", "InconsistentNaming", Justification = "Checked")]
public class Matrix4x4DEqualsAssertion(TUnit.Assertions.Core.AssertionContext<Matrix4x4D> context, Matrix4x4D expected)
    : TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Matrix4x4D, Matrix4x4D>(context, expected)
{
    public TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Matrix4x4D, Matrix4x4D> Within(double tolerance) =>
        this.Within(Matrix4x4D.Create(tolerance));

    protected override bool HasToleranceValue() => true;

    protected override bool IsWithinTolerance(Matrix4x4D actual, Matrix4x4D expected, Matrix4x4D tolerance) =>
           MathHelper.IsWithinToleranceCore(actual.M11, expected.M11, tolerance.M11)
        && MathHelper.IsWithinToleranceCore(actual.M12, expected.M12, tolerance.M12)
        && MathHelper.IsWithinToleranceCore(actual.M13, expected.M13, tolerance.M13)
        && MathHelper.IsWithinToleranceCore(actual.M14, expected.M14, tolerance.M14)
        && MathHelper.IsWithinToleranceCore(actual.M21, expected.M21, tolerance.M21)
        && MathHelper.IsWithinToleranceCore(actual.M22, expected.M22, tolerance.M22)
        && MathHelper.IsWithinToleranceCore(actual.M23, expected.M23, tolerance.M23)
        && MathHelper.IsWithinToleranceCore(actual.M24, expected.M24, tolerance.M24)
        && MathHelper.IsWithinToleranceCore(actual.M31, expected.M31, tolerance.M31)
        && MathHelper.IsWithinToleranceCore(actual.M32, expected.M32, tolerance.M32)
        && MathHelper.IsWithinToleranceCore(actual.M33, expected.M33, tolerance.M33)
        && MathHelper.IsWithinToleranceCore(actual.M34, expected.M34, tolerance.M34)
        && MathHelper.IsWithinToleranceCore(actual.M41, expected.M41, tolerance.M41)
        && MathHelper.IsWithinToleranceCore(actual.M42, expected.M42, tolerance.M42)
        && MathHelper.IsWithinToleranceCore(actual.M43, expected.M43, tolerance.M43)
        && MathHelper.IsWithinToleranceCore(actual.M44, expected.M44, tolerance.M44);

    protected override object CalculateDifference(Matrix4x4D actual, Matrix4x4D expected) =>
        new Matrix4x4D(
            double.Abs(actual.M11 - expected.M11),
            double.Abs(actual.M12 - expected.M12),
            double.Abs(actual.M13 - expected.M13),
            double.Abs(actual.M14 - expected.M14),
            double.Abs(actual.M21 - expected.M21),
            double.Abs(actual.M22 - expected.M22),
            double.Abs(actual.M23 - expected.M23),
            double.Abs(actual.M24 - expected.M24),
            double.Abs(actual.M31 - expected.M31),
            double.Abs(actual.M32 - expected.M32),
            double.Abs(actual.M33 - expected.M33),
            double.Abs(actual.M34 - expected.M34),
            double.Abs(actual.M41 - expected.M41),
            double.Abs(actual.M42 - expected.M42),
            double.Abs(actual.M43 - expected.M43),
            double.Abs(actual.M44 - expected.M44));

    protected override bool AreExactlyEqual(Matrix4x4D actual, Matrix4x4D expected) => actual == expected;
}