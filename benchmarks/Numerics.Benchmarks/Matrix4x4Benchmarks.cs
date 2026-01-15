namespace Altemiq.Numerics;

using Altemiq.Math;

public class Matrix4x4Benchmarks
{
    private readonly Matrix4x4D altemiqMatrix = new(1, 0, 2, -1, 3, 0, 0, 5, 2, 1, 4, -3, 1, 0, 5, 0);

    private readonly System.Numerics.Matrix4x4 systemMatrix = new(1, 0, 2, -1, 3, 0, 0, 5, 2, 1, 4, -3, 1, 0, 5, 0);

    private readonly double[] doubleArray = [1, 0, 2, -1, 3, 0, 0, 5, 2, 1, 4, -3, 1, 0, 5, 0];

    [Benchmark]
    public Matrix4x4D InvertAltemiq() => Matrix4x4D.Invert(this.altemiqMatrix, out var temp) ? temp : default;

    [Benchmark]
    public System.Numerics.Matrix4x4 InvertSystem() => System.Numerics.Matrix4x4.Invert(this.systemMatrix, out var temp) ? temp : default;

    [Benchmark]
    public CommunityToolkit.HighPerformance.ReadOnlySpan2D<double> InvertMath()
    {
        var matrix = new CommunityToolkit.HighPerformance.ReadOnlySpan2D<double>(this.doubleArray, 4, 4);
        return matrix.Invert();
    }
}