namespace Altemiq.Numerics;

public static class VectorEqualsExtensions
{
    public static Vector2DEqualsAssertion IsEqualTo(this TUnit.Assertions.Core.IAssertionSource<Vector2D> source, Vector2D expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
    {
        source.Context.ExpressionBuilder.Append(".IsEqualTo(" + string.Join(", ", new[] { expectedExpression }.Where(e => e != null)) + ")");
        return new(source.Context, expected);
    }
    
    public static Vector3DEqualsAssertion IsEqualTo(this TUnit.Assertions.Core.IAssertionSource<Vector3D> source, Vector3D expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
    {
        source.Context.ExpressionBuilder.Append(".IsEqualTo(" + string.Join(", ", new[] { expectedExpression }.Where(e => e != null)) + ")");
        return new(source.Context, expected);
    }
    
    public static Vector4DEqualsAssertion IsEqualTo(this TUnit.Assertions.Core.IAssertionSource<Vector4D> source, Vector4D expected, [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expectedExpression = null)
    {
        source.Context.ExpressionBuilder.Append(".IsEqualTo(" + string.Join(", ", new[] { expectedExpression }.Where(e => e != null)) + ")");
        return new(source.Context, expected);
    }
}

public class Vector2DEqualsAssertion(TUnit.Assertions.Core.AssertionContext<Vector2D> context, Vector2D expected)
    : TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Vector2D, Vector2D>(context, expected)
{
    public TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Vector2D, Vector2D> Within(double tolerance) =>
        this.Within(Vector2D.Create(tolerance));

    protected override bool HasToleranceValue() => true;

    protected override bool IsWithinTolerance(Vector2D actual, Vector2D expected, Vector2D tolerance) =>
        VectorHelpers.IsWithinToleranceCore(actual.X, expected.X, tolerance.X)
        && VectorHelpers.IsWithinToleranceCore(actual.Y, expected.Y, tolerance.Y);

    protected override object CalculateDifference(Vector2D actual, Vector2D expected) => Vector2D.Abs(actual - expected);

    protected override bool AreExactlyEqual(Vector2D actual, Vector2D expected) => actual == expected;
}

public class Vector3DEqualsAssertion(TUnit.Assertions.Core.AssertionContext<Vector3D> context, Vector3D expected)
    : TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Vector3D, Vector3D>(context, expected)
{
    public TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Vector3D, Vector3D> Within(double tolerance) =>
        this.Within(Vector3D.Create(tolerance));

    protected override bool HasToleranceValue() => true;

    protected override bool IsWithinTolerance(Vector3D actual, Vector3D expected, Vector3D tolerance) =>
        VectorHelpers.IsWithinToleranceCore(actual.X, expected.X, tolerance.X)
        && VectorHelpers.IsWithinToleranceCore(actual.Y, expected.Y, tolerance.Y)
        && VectorHelpers.IsWithinToleranceCore(actual.Z, expected.Z, tolerance.Z);

    protected override object CalculateDifference(Vector3D actual, Vector3D expected) => Vector3D.Abs(actual - expected);

    protected override bool AreExactlyEqual(Vector3D actual, Vector3D expected) => actual == expected;
}

public class Vector4DEqualsAssertion(TUnit.Assertions.Core.AssertionContext<Vector4D> context, Vector4D expected)
    : TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Vector4D, Vector4D>(context, expected)
{
    public TUnit.Assertions.Conditions.ToleranceBasedEqualsAssertion<Vector4D, Vector4D> Within(double tolerance) =>
        this.Within(Vector4D.Create(tolerance));
    
    protected override bool HasToleranceValue() => true;

    protected override bool IsWithinTolerance(Vector4D actual, Vector4D expected, Vector4D tolerance) =>
        VectorHelpers.IsWithinToleranceCore(actual.X, expected.X, tolerance.X)
        && VectorHelpers.IsWithinToleranceCore(actual.Y, expected.Y, tolerance.Y)
        && VectorHelpers.IsWithinToleranceCore(actual.Z, expected.Z, tolerance.Z)
        && VectorHelpers.IsWithinToleranceCore(actual.W, expected.W, tolerance.W);

    protected override object CalculateDifference(Vector4D actual, Vector4D expected) => Vector4D.Abs(actual - expected);

    protected override bool AreExactlyEqual(Vector4D actual, Vector4D expected) => actual == expected;
}

internal static class VectorHelpers
{
    public static bool IsWithinToleranceCore(double actual, double expected, double tolerance)
    {
        // Handle NaN comparisons: NaN is only equal to NaN
        if (double.IsNaN(actual) && double.IsNaN(expected))
        {
            return true;
        }

        if (double.IsNaN(actual) || double.IsNaN(expected))
        {
            return false;
        }

        // Handle infinity: infinity equals infinity
        if (double.IsPositiveInfinity(actual) && double.IsPositiveInfinity(expected))
        {
            return true;
        }

        if (double.IsNegativeInfinity(actual) && double.IsNegativeInfinity(expected))
        {
            return true;
        }

        var diff = Math.Abs(actual - expected);
        return diff <= tolerance;
    }
}