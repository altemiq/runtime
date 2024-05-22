// -----------------------------------------------------------------------
// <copyright file="ArrayExtensionTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class ArrayExtensionTests
{
    [Fact]
    public void PadLeft()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        var result = array.PadLeft(10);
        result.Should().HaveCount(10);
        result.Should().BeEquivalentTo([0, 0, 0, 0, 0, 0, 1, 2, 3, 4]);
    }

    [Fact]
    public void NullPadLeft()
    {
        int[]? @array = default;
        var result = array.PadLeft(10);
        result.Should().HaveCount(10);
        result.Should().BeEquivalentTo([0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }

    [Fact]
    public void PadRight()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        var result = array.PadRight(10);
        result.Should().HaveCount(10);
        result.Should().BeEquivalentTo([1, 2, 3, 4, 0, 0, 0, 0, 0, 0]);
    }

    [Fact]
    public void NullPadRight()
    {
        int[]? @array = default;
        var result = array.PadRight(10);
        result.Should().HaveCount(10);
        result.Should().BeEquivalentTo([0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
    }
}
