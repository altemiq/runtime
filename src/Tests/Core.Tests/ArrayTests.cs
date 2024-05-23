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
    public void ThrowOnNullLeft()
    {
        var action = () =>
        {
            int[]? array = default;
            Array.PadLeft(ref array!, default);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void EqualLengthLeft()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadLeft(ref array, 4);
        array.Should().HaveCount(4);
        array.Should().BeEquivalentTo(array);
    }

    [Fact]
    public void ShortenLeft()
    {
        var @array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Array.PadLeft(ref array, 10);
        array.Should().HaveCount(10);
        array.Should().BeEquivalentTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
    }

    [Fact]
    public void PadRight()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadRight(ref array, 10);
        array.Should().HaveCount(10);
        array.Should().BeEquivalentTo([1, 2, 3, 4, 0, 0, 0, 0, 0, 0]);
    }

    [Fact]
    public void ThrowOnNullRight()
    {
        var action = () =>
        {
            int[]? array = default;
            Array.PadRight(ref array!, default);
        };

        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void EqualLengthRight()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadRight(ref array, 4);
        array.Should().HaveCount(4);
        array.Should().BeEquivalentTo(array);
    }

    [Fact]
    public void ShortenRight()
    {
        var @array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Array.PadRight(ref array, 10);
        array.Should().HaveCount(10);
        array.Should().BeEquivalentTo([1, 2, 3, 4, 5, 6, 7, 8, 9, 10]);
    }
}