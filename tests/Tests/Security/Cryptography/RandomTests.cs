// -----------------------------------------------------------------------
// <copyright file="RandomTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Security.Cryptography;

using TUnit.Assertions.AssertConditions.Throws;

public class RandomTests
{
    [Test]
    public async Task NextDouble()
    {
        var random = new Random();
        var first = random.NextDouble();
        await Assert.That(first).IsBetween(0D, 1D);
        var second = random.NextDouble();
        await Assert.That(first).IsBetween(0D, 1D);
        await Assert.That(first).IsNotEqualTo(second);
    }

    [Test]
    public async Task Next()
    {
        var random = new Random();
        var first = random.Next();
        await Assert.That(first).IsBetween(0, int.MaxValue);
        var second = random.Next();
        await Assert.That(first).IsBetween(0, int.MaxValue);
        await Assert.That(first).IsNotEqualTo(second);
    }

    [Test]
    public async Task NextWithMax()
    {
        const int Max = 10000;
        var random = new Random();
        var first = random.Next(Max);
        await Assert.That(first).IsBetween(0, Max);
        var second = random.Next(Max);
        await Assert.That(second).IsBetween(0, Max);
        await Assert.That(first).IsNotEqualTo(second);
    }

    [Test]
    public async Task NextWithRange()
    {
        const int Min = 1234;
        const int Max = 10000;
        var random = new Random();
        var first = random.Next(Min, Max);
        await Assert.That(first).IsBetween(Min, Max);
        var second = random.Next(Min, Max);
        await Assert.That(second).IsBetween(Min, Max);
        await Assert.That(first).IsNotEqualTo(second);
    }

    [Test]
    public async Task NextWithInvalidMax()
    {
        const int Max = -1;
        var random = new Random();
        await Assert.That(() => random.Next(Max)).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task NextWithInvalidMinMax()
    {
        const int Max = 100;
        const int Min = 1000;
        var random = new Random();
        await Assert.That(() => random.Next(Min, Max)).Throws<ArgumentOutOfRangeException>();
    }

    [Test]
    public async Task NextWithSameMinMax()
    {
        const int MinMax = 1000;
        var random = new Random();
        await Assert.That(random.Next(MinMax, MinMax)).IsEqualTo(MinMax);
    }
}