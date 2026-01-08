namespace Altemiq.Numerics;

internal static class MathHelper
{
    public static T ToRadians<T>(T value)
        where T : System.Numerics.IFloatingPointConstants<T>
    {
        return value * T.Pi / T.CreateChecked(180);
    }
}