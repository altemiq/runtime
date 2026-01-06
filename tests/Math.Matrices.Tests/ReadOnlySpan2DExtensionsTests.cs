namespace Altemiq.Math.Matrices;

using CommunityToolkit.HighPerformance;

public class ReadOnlySpan2DExtensionsTests
{
    [Test]
    public async Task IsIdentity()
    {
        var matrix = new ReadOnlySpan2D<double>([1D, 0D, 0D, 1D], 2, 2);
        await Assert.That(matrix.IsIdentity).IsTrue();
    }
    
    [Test]
    [MethodDataSource(nameof(GetNotIdentityData))]
    public async Task IsNotIdentity(double[,] matrix)
    {
        await Assert.That(new ReadOnlySpan2D<double>(matrix).IsIdentity).IsFalse();
    }
    
    [Test]
    public async Task IsNotSquare()
    {
        await Assert.That(new ReadOnlySpan2D<double>([0, 0, 0, 0, 0, 0], 3, 2).IsSquare).IsFalse();
    }
    
    [Test]
    public async Task Add()
    {
        var first = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 3, 2);
        var second = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 3, 2);

        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };
        
        await Assert.That(first.Add(second).ToArray()).IsEquivalentTo(expected);
    }
    
    [Test]
    public async Task Subtract()
    {
        var first = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 3, 2);
        var second = new ReadOnlySpan2D<double>([6, 5, 4, 3, 2, 1], 3, 2);

        double[,] expected =
        {
            { -5, -3 },
            { -1, 1 },
            { 3, 5 },
        };
        
        await Assert.That(first.Subtract(second).ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task Multiply()
    {
        var first = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 3, 2);
        var second = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 2, 3);

        double[,] expected =
        {
            { 9, 12, 15 },
            { 19, 26, 33 },
            { 29, 40, 51 },
        };
        
        await Assert.That(first.Multiply(second).ToArray()).IsEquivalentTo(expected);
    }
    
    [Test]
    public async Task Scale()
    {
        var first = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 3, 2);
        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };
        
        await Assert.That(first.Scale(2D).ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task Transpose()
    {
        var first = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 3, 2);
        var expected = new ReadOnlySpan2D<double>([1, 3, 5, 2, 4, 6], 2, 3);
        
        await Assert.That(first.Transpose().ToArray()).IsEquivalentTo(expected.ToArray());
    }

    [Test]
    [MethodDataSource(nameof(GetDeterminantData))]
    public async Task Determinant(double[,] matrix, double determinant)
    {
        ReadOnlySpan2D<double> input = matrix;
        await Assert.That(input.GetDeterminant()).IsEqualTo(determinant);
    }

    [Test]
    public async Task Invert()
    {
        var matrix = new ReadOnlySpan2D<double>([1, 3, 3, 1, 4, 4, 1, 3, 4], 3, 3);
        double[,] expected =
        {
            { 4, -3, 0 },
            { 0, 1, -1 },
            { -1, 0, 1 },
        };
        await Assert.That(matrix.Invert().ToArray()).IsEquivalentTo(expected);
    }
    
    [Test]
    public async Task InvertGaussian()
    {
        var matrix = new ReadOnlySpan2D<double>([1, 3, 3, 1, 4, 4, 1, 3, 4], 3, 3);
        double[,] expected =
        {
            { 4, -3, 0 },
            { 0, 1, -1 },
            { -1, 0, 1 },
        };
        await Assert.That(matrix.InvertGaussian().ToArray()).IsEquivalentTo(expected);
    }

    public static IEnumerable<Func<double[,]>> GetNotIdentityData()
    {
        yield return () => new double[,] { { 1, 0 }, { 0, 0 } };
        yield return () => new double[,] { { 1, 1 }, { 0, 1 } };
    }

    public static IEnumerable<Func<(double[,], double)>> GetDeterminantData()
    {
        yield return () => (new double[,] { { 3, 2 }, { 1, 4 } }, 10);
        yield return () => (new double[,] { { 1, 2, 3 }, { 0, 4, 5 }, { 1, 0, 6 } }, 22);
    }
}