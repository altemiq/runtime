// -----------------------------------------------------------------------
// <copyright file="RandomTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Security.Cryptography;

public class RandomTests
{
    [Fact]
    public void NextDouble()
    {
        var random = new Random();
        var first = random.NextDouble();
        Assert.InRange(first, 0D, 1D);
        var second = random.NextDouble();
        Assert.InRange(first, 0D, 1D);
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void Next()
    {
        var random = new Random();
        var first = random.Next();
        Assert.InRange(first, 0, int.MaxValue);
        var second = random.Next();
        Assert.InRange(first, 0, int.MaxValue);
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void NextWithMax()
    {
        const int Max = 10000;
        var random = new Random();
        var first = random.Next(Max);
        Assert.InRange(first, 0, Max);
        var second = random.Next(Max);
        Assert.InRange(second, 0, Max);
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void NextWithRange()
    {
        const int Min = 1234;
        const int Max = 10000;
        var random = new Random();
        var first = random.Next(Min, Max);
        Assert.InRange(first, Min, Max);
        var second = random.Next(Min, Max);
        Assert.InRange(second, Min, Max);
        Assert.NotEqual(first, second);
    }

    [Fact]
    public void NextWithInvalidMax()
    {
        const int Max = -1;
        var random = new Random();
        Assert.Throws<ArgumentOutOfRangeException>(() => random.Next(Max));
    }

    [Fact]
    public void NextWithInvalidMinMax()
    {
        const int Max = 100;
        const int Min = 1000;
        var random = new Random();
        Assert.Throws<ArgumentOutOfRangeException>(() => random.Next(Min, Max));
    }

    [Fact]
    public void NextWithSameMinMax()
    {
        const int MinMax = 1000;
        var random = new Random();
        Assert.Equal(MinMax, random.Next(MinMax, MinMax));
    }
}