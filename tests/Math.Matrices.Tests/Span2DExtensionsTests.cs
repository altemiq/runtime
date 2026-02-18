namespace Altemiq.Math.Matrices;

using CommunityToolkit.HighPerformance;

public class Span2DExtensionsTests
{
    [Test]
    public async Task CompoundAdd()
    {
        var first = Create();
        var second = new ReadOnlySpan2D<double>([1, 2, 3, 4, 5, 6], 3, 2);

        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };

        first += second;
        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundAddFirstSparse()
    {
        var first = CreateSparse();
        var second = Create();

        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };

        first += second;
        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundAddSecondSparse()
    {
        var first = Create();
        var second = CreateSparse();

        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };

        first += second;
        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundAddBothSparse()
    {
        var first = CreateSparse();
        var second = CreateSparse();

        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };

        first += second;
        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundSubtract()
    {
        var first = Create();
        var second = new ReadOnlySpan2D<double>([6, 5, 4, 3, 2, 1], 3, 2);

        double[,] expected =
        {
            { -5, -3 },
            { -1, 1 },
            { 3, 5 },
        };

        first -= second;

        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundSubtractFirstSparse()
    {
        var first = CreateSparse();
        var second = new ReadOnlySpan2D<double>([6, 5, 4, 3, 2, 1], 3, 2);

        double[,] expected =
        {
            { -5, -3 },
            { -1, 1 },
            { 3, 5 },
        };

        first -= second;
        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundSubtractSecondSparse()
    {
        var first = Create();
        double[,] values =
        {
            { 0, 0, 0, 0 },
            { 0, 6, 5, 0 },
            { 0, 4, 3, 0 },
            { 0, 2, 1, 0 },
            { 0, 0, 0, 0 },
        };
        var second = new ReadOnlySpan2D<double>(values, 1, 1, 3, 2);

        double[,] expected =
        {
            { -5, -3 },
            { -1, 1 },
            { 3, 5 },
        };

        first -= second;
        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundSubtractBothSparse()
    {
        var first = CreateSparse();
        double[,] values =
        {
            { 0, 0, 0, 0 },
            { 0, 6, 5, 0 },
            { 0, 4, 3, 0 },
            { 0, 2, 1, 0 },
            { 0, 0, 0, 0 },
        };
        var second = new ReadOnlySpan2D<double>(values, 1, 1, 3, 2);

        double[,] expected =
        {
            { -5, -3 },
            { -1, 1 },
            { 3, 5 },
        };

        first -= second;
        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    [MethodDataSource(nameof(GetMatricesToMultiply))]
    public async Task CompoundMultiply(double[,] firstArray, double[,] secondArray, double[,] expected)
    {
        var first = new Span2D<double>(firstArray);
        var second = new ReadOnlySpan2D<double>(secondArray);

        first *= second;

        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundScale()
    {
        var first = Create();
        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };

        first *= 2D;

        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    [Test]
    public async Task CompoundScaleSparse()
    {
        var first = CreateSparse();
        double[,] expected =
        {
            { 2, 4 },
            { 6, 8 },
            { 10, 12 },
        };

        first *= 2D;

        await Assert.That(first.ToArray()).IsEquivalentTo(expected);
    }

    public static IEnumerable<Func<(double[,], double[,], double[,])>> GetMatricesToMultiply()
    {
        yield return () => (new double[,] { { 1, 2 }, { 3, 4 } }, new double[,] { { 1, 2 }, { 3, 4 } }, new double[,] { { 7, 10 }, { 15, 22 }, });
        yield return () => (new double[,] { { 1, 2 }, { 3, 4 }, { 5, 6 } }, new double[,] { { 1, 2, 3 }, { 4, 5, 6 } }, new double[,] { { 9, 12, 15 }, { 19, 26, 33 }, { 29, 40, 51 }, });
    }

    private static Span2D<double> Create()
    {
        return new([1, 2, 3, 4, 5, 6], 3, 2);
    }

    private static Span2D<double> CreateSparse()
    {
        double[,] array =
        {
            { 0, 0, 0, 0 },
            { 0, 1, 2, 0 },
            { 0, 3, 4, 0 },
            { 0, 5, 6, 0 },
            { 0, 0, 0, 0 },
        };

        return new(array, 1, 1, 3, 2);
    }
}