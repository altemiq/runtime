// -----------------------------------------------------------------------
// <copyright file="ArrayTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class ArrayTests
{
    [Fact]
    public void PadLeft()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadLeft(ref array, 10);
        array.Should().HaveCount(10);
        array.Should().BeEquivalentTo([0, 0, 0, 0, 0, 0, 1, 2, 3, 4]);
    }

    [Fact]
    public void PadRight()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadRight(ref array, 10);
        array.Should().HaveCount(10);
        array.Should().BeEquivalentTo([1, 2, 3, 4, 0, 0, 0, 0, 0, 0]);
    }
}
