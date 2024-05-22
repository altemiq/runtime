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
        _ = first.Should().BeGreaterThanOrEqualTo(0D).And.BeLessThanOrEqualTo(1D);
        var second = random.NextDouble();
        _ = second.Should().BeGreaterThanOrEqualTo(0D).And.BeLessThanOrEqualTo(1D).And.NotBe(first);
    }

    [Fact]
    public void Next()
    {
        var random = new Random();
        var first = random.Next();
        _ = first.Should().BeGreaterThanOrEqualTo(0).And.BeLessThanOrEqualTo(int.MaxValue);
        var second = random.Next();
        _ = second.Should().BeGreaterThanOrEqualTo(0).And.BeLessThanOrEqualTo(int.MaxValue).And.NotBe(first);
    }

    [Fact]
    public void NextWithMax()
    {
        const int Max = 10000;
        var random = new Random();
        var first = random.Next(Max);
        _ = first.Should().BeGreaterThanOrEqualTo(0).And.BeLessThanOrEqualTo(Max);
        var second = random.Next(Max);
        _ = second.Should().BeGreaterThanOrEqualTo(0).And.BeLessThanOrEqualTo(Max).And.NotBe(first);
    }

    [Fact]
    public void NextWithRange()
    {
        const int Min = 1234;
        const int Max = 10000;
        var random = new Random();
        var first = random.Next(Min, Max);
        _ = first.Should().BeGreaterThanOrEqualTo(Min).And.BeLessThanOrEqualTo(Max);
        var second = random.Next(Min, Max);
        _ = second.Should().BeGreaterThanOrEqualTo(Min).And.BeLessThanOrEqualTo(Max).And.NotBe(first);
    }

    [Fact]
    public void NextWithInvalidMax()
    {
        const int Max = -1;
        var random = new Random();
        random.Invoking(r => r.Next(Max)).Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void NextWithInvalidMinMax()
    {
        const int Max = 100;
        const int Min = 1000;
        var random = new Random();
        random.Invoking(r => r.Next(Min, Max)).Should().Throw<ArgumentOutOfRangeException>();
    }

    [Fact]
    public void NextWithSameMinMax()
    {
        const int MinMax = 1000;
        var random = new Random();
        random.Next(MinMax, MinMax).Should().Be(MinMax);
    }
}
