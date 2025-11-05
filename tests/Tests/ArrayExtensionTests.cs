// -----------------------------------------------------------------------
// <copyright file="ArrayExtensionTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class ArrayExtensionTests
{
    [Test]
    public async Task PadLeft()
    {
        var array = new[] { 1, 2, 3, 4 };
        var result = array.PadLeft(10);
        await Assert.That(result).IsEquivalentTo([0, 0, 0, 0, 0, 0, 1, 2, 3, 4]);
    }

    [Test]
    public async Task NullPadLeft()
    {
        int[]? array = default;
        var result = array.PadLeft(10);
        await Assert.That(result).IsEquivalentTo([0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }

    [Test]
    public async Task PadRight()
    {
        var array = new[] { 1, 2, 3, 4 };
        var result = array.PadRight(10);
        await Assert.That(result).IsEquivalentTo([1, 2, 3, 4, 0, 0, 0, 0, 0, 0]);
    }

    [Test]
    public async Task NullPadRight()
    {
        int[]? array = default;
        var result = array.PadRight(10);
        await Assert.That(result).IsEquivalentTo([0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }
}