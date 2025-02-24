namespace Altemiq.Buffers.Compression.Differential;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class DeltaTests
{
    [Test]
    public async Task Forward()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        Delta.Forward(data);
        await Assert.That(data).IsEquivalentTo([1, 1, 1, 1, 1]);
    }

    [Test]
    public async Task ForwardWithInitial()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        Delta.Forward(data, 1);
        await Assert.That(data).IsEquivalentTo([0, 1, 1, 1, 1]);
    }


    [Test]
    public async Task ForwardToDestination()
    {
        var data = new int[] { 1, 2, 3, 4, 5 };
        var destination = new int[data.Length];
        Delta.Forward(data, 1, destination);
        await Assert.That(destination).IsEquivalentTo([0, 1, 1, 1, 1]);
    }

    [Test]
    public async Task Inverse()
    {
        var data = new int[] { 1, 1, 1, 1, 1 };
        Delta.Inverse(data);
        await Assert.That(data).IsEquivalentTo([1, 2, 3, 4, 5]);
    }

    [Test]
    public async Task InverseWithInitial()
    {
        var data = new int[] { 0, 1, 1, 1, 1 };
        Delta.Inverse(data, 1);
        await Assert.That(data).IsEquivalentTo([1, 2, 3, 4, 5]);
    }
}