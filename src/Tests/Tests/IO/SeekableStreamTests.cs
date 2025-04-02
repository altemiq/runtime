// -----------------------------------------------------------------------
// <copyright file="SeekableStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

using TUnit.Assertions.AssertConditions.Throws;

public class SeekableStreamTests
{
    [Test]
    public async Task ShouldBeAbleToSeekForward()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        await Assert.That(stream.Length).IsEqualTo(1024);
        await Assert.That(stream.Seek(512, SeekOrigin.Begin)).IsEqualTo(512);
    }

    [Test]
    public async Task ShouldNotBeAbleToSeekBackwards()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        await Assert.That(stream.Length).IsEqualTo(1024);
        await Assert.That(stream.Seek(-512, SeekOrigin.End)).IsEqualTo(512);
        await Assert.That(() => stream.Seek(-10, SeekOrigin.Current)).Throws<InvalidOperationException>();
    }

    [Test]
    public async Task SeekOnAlreadySeekableStream()
    {
        using var stream = new SeekableStream(new MemoryStream(512));
        await Assert.That(stream.Seek(256, SeekOrigin.Begin)).IsEqualTo(256);
    }

    [Test]
    public async Task SeekWithInvalidEnum()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        await Assert.That(() => stream.Seek(0, (SeekOrigin)(-1))).Throws<ArgumentOutOfRangeException>();
    }

    private class ForwardOnlyStream : MemoryStream
    {
        public ForwardOnlyStream()
            : base(new byte[1024])
        {
        }

        public override bool CanSeek => false;
    }
}