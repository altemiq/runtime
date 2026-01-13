namespace Altemiq.Numerics;

internal static class MathHelper
{
    public static T ToRadians<T>(T value)
        where T : System.Numerics.IFloatingPointConstants<T>
    {
        return value * T.Pi / T.CreateChecked(180);
    }

    public static bool IsWithinToleranceCore<T>(T actual, T expected, T tolerance)
        where T : System.Numerics.INumberBase<T>, System.Numerics.IComparisonOperators<T, T, bool>
    {
        // Handle NaN comparisons: NaN is only equal to NaN
        if (T.IsNaN(actual) && T.IsNaN(expected))
        {
            return true;
        }

        if (T.IsNaN(actual) || T.IsNaN(expected))
        {
            return false;
        }

        // Handle infinity: infinity equals infinity
        if (T.IsPositiveInfinity(actual) && T.IsPositiveInfinity(expected))
        {
            return true;
        }

        if (T.IsNegativeInfinity(actual) && T.IsNegativeInfinity(expected))
        {
            return true;
        }

        var diff = T.Abs(actual - expected);
        return diff <= tolerance;
    }
}