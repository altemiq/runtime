namespace Altemiq.Numerics;

public static class QuaternionEqualsExtensions
{
    extension(TUnit.Assertions.Core.IAssertionSource<QuaternionD> source)
    {
        public QuaternionDEqualsAssertion IsEqualTo(QuaternionD expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
        {
            source.Context.ExpressionBuilder.Append(".IsEqualTo(" + string.Join(", ", new[] { expectedExpression }.Where(e => e != null)) + ")");
            return new(source.Context, expected);
        }

        public QuaternionDEqualsAssertion IsEqualTo(Vector3D expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
        {
            source.Context.ExpressionBuilder.Append(".IsEqualTo(" + string.Join(", ", new[] { expectedExpression }.Where(e => e != null)) + ")");
            return new(source.Context, new(expected.X, expected.Y, expected.Z, default), true);
        }
    }
}

public class QuaternionDEqualsAssertion(TUnit.Assertions.Core.AssertionContext<QuaternionD> context, QuaternionD expected, bool ignoreW = false)
    : TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<QuaternionD, QuaternionD>(context, expected)
{
    public TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<QuaternionD, QuaternionD> Within(double tolerance) =>
        this.Within(QuaternionD.Create(tolerance, tolerance, tolerance, tolerance));

    protected override bool HasToleranceValue() => true;

    protected override bool IsWithinTolerance(QuaternionD actual, QuaternionD expected, QuaternionD tolerance) =>
        MathHelper.IsWithinToleranceCore(actual.X, expected.X, tolerance.X)
        && MathHelper.IsWithinToleranceCore(actual.Y, expected.Y, tolerance.Y)
        && MathHelper.IsWithinToleranceCore(actual.Z, expected.Z, tolerance.Z)
        && (ignoreW || MathHelper.IsWithinToleranceCore(actual.W, expected.W, tolerance.W));

    protected override object CalculateDifference(QuaternionD actual, QuaternionD expected) => QuaternionD.Create(
        double.Abs(actual.X - expected.X),
        double.Abs(actual.Y - expected.Y),
        double.Abs(actual.Z - expected.Z),
        ignoreW ? 0.0 : double.Abs(actual.W - expected.W));

    protected override bool AreExactlyEqual(QuaternionD actual, QuaternionD expected)
    {
        if (ignoreW)
        {
            return new Vector3D(actual.X, actual.Y, actual.Z) == new Vector3D(expected.X, expected.Y, expected.Z);
        }

        return actual == expected;
    }
}