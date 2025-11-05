// -----------------------------------------------------------------------
// <copyright file="ArrayTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class ArrayTests
{
    [Test]
    public async Task PadLeft()
    {
        var array = new[] { 1, 2, 3, 4 };
        Array.PadLeft(ref array, 10);
        await Assert.That(array).IsEquivalentTo([0, 0, 0, 0, 0, 0, 1, 2, 3, 4]);
    }

    [Test]
    public async Task ThrowOnNullLeft()
    {
        await Assert.That(static () =>
        {
            int[]? array = default;
            Array.PadLeft(ref array!, default);
        }).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EqualLengthLeft()
    {
        var array = new[] { 1, 2, 3, 4 };
        Array.PadLeft(ref array, 4);
        await Assert.That(array).IsEquivalentTo([1, 2, 3, 4]);
    }

    [Test]
    public async Task ShortenLeft()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Array.PadLeft(ref array, 10);
        await Assert.That(array).IsEquivalentTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
    }

    [Test]
    public async Task PadRight()
    {
        var array = new[] { 1, 2, 3, 4 };
        Array.PadRight(ref array, 10);
        await Assert.That(array).IsEquivalentTo([1, 2, 3, 4, 0, 0, 0, 0, 0, 0]);
    }

    [Test]
    public async Task ThrowOnNullRight()
    {
        await Assert.That(static () =>
        {
            int[]? array = default;
            Array.PadRight(ref array!, default);
        }).Throws<ArgumentNullException>();
    }

    [Test]
    public async Task EqualLengthRight()
    {
        var array = new[] { 1, 2, 3, 4 };
        Array.PadRight(ref array, 4);
        await Assert.That(array).IsEquivalentTo([1, 2, 3, 4]);
    }

    [Test]
    public async Task ShortenRight()
    {
        var array = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Array.PadRight(ref array, 10);
        await Assert.That(array).IsEquivalentTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
    }
}