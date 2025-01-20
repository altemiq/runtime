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
        Assert.Equal([0, 0, 0, 0, 0, 0, 1, 2, 3, 4], array);
    }

    [Fact]
    public void ThrowOnNullLeft()
    {
        Assert.Throws<ArgumentNullException>(static () =>
        {
            int[]? array = default;
            Array.PadLeft(ref array!, default);
        });
    }

    [Fact]
    public void EqualLengthLeft()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadLeft(ref array, 4);
        Assert.Equal([1, 2, 3, 4], @array);
    }

    [Fact]
    public void ShortenLeft()
    {
        var @array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Array.PadLeft(ref array, 10);
        Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], array);
    }

    [Fact]
    public void PadRight()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadRight(ref array, 10);
        Assert.Equal([1, 2, 3, 4, 0, 0, 0, 0, 0, 0], array);
    }

    [Fact]
    public void ThrowOnNullRight()
    {
        Assert.Throws<ArgumentNullException>(static () =>
        {
            int[]? array = default;
            Array.PadRight(ref array!, default);
        });
    }

    [Fact]
    public void EqualLengthRight()
    {
        var @array = new int[] { 1, 2, 3, 4 };
        Array.PadRight(ref array, 4);
        Assert.Equal([1, 2, 3, 4], array);
    }

    [Fact]
    public void ShortenRight()
    {
        var @array = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };
        Array.PadRight(ref array, 10);
        Assert.Equal([1, 2, 3, 4, 5, 6, 7, 8, 9, 10], array);
    }
}