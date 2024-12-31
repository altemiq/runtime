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
        data.Should().AllBeEquivalentTo(1);
    }

    [Fact]
    public void ForwardWithInitial()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        Delta.Forward(data, 1);
        data.Should().BeEquivalentTo([0, 1, 1, 1, 1]);
    }


    [Fact]
    public void ForwardToDestination()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        var destination = new int[data.Length];
        Delta.Forward(data, 1, destination);
        destination.Should().BeEquivalentTo([0, 1, 1, 1, 1]);
    }

    [Fact]
    public void Inverse()
    {
        var data = new int[] { 1, 1, 1, 1, 1 };
        Delta.Inverse(data);
        data.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
    }

    [Fact]
    public void InverseWithInitial()
    {
        var data = new int[] { 0, 1, 1, 1, 1 };
        Delta.Inverse(data, 1);
        data.Should().BeEquivalentTo([1, 2, 3, 4, 5]);
    }
}