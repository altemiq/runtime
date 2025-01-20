namespace Altemiq.Buffers.Compression.Differential;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeltaTests
{
    [Fact]
    public void Forward()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        Delta.Forward(data);
        Assert.Equal([1, 1, 1, 1, 1], data);
    }

    [Fact]
    public void ForwardWithInitial()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        Delta.Forward(data, 1);
        Assert.Equal([0, 1, 1, 1, 1], data);
    }


    [Fact]
    public void ForwardToDestination()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        var destination = new int[data.Length];
        Delta.Forward(data, 1, destination);
        Assert.Equal([0, 1, 1, 1, 1], destination);
    }

    [Fact]
    public void Inverse()
    {
        var data = new int[] { 1, 1, 1, 1, 1 };
        Delta.Inverse(data);
        Assert.Equal([1, 2, 3, 4, 5], data);
    }

    [Fact]
    public void InverseWithInitial()
    {
        var data = new int[] { 0, 1, 1, 1, 1 };
        Delta.Inverse(data, 1);
        Assert.Equal([1, 2, 3, 4, 5], data);
    }
}