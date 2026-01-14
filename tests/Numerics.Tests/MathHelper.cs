namespace Altemiq.Numerics;

internal static class MathHelper
{
    public const double HighQualityTolerance = 1e-13;
    public const double MidQualityTolerance = 1e-07;
    public const double LowQualityTolerance = 1e-05;

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