// -----------------------------------------------------------------------
// <copyright file="SeekableStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class SeekableStreamTests
{
    [Test]
    public async Task ShouldBeAbleToSeekForward()
    {
#if NET
        await
#endif
        using var stream = new SeekableStream(new ForwardOnlyStream());
        await Assert.That(stream.Length).IsEqualTo(1024);
        await Assert.That(stream.Seek(512, SeekOrigin.Begin)).IsEqualTo(512);
    }

    [Test]
    public async Task ShouldNotBeAbleToSeekBackwards()
    {
        var stream = new SeekableStream(new ForwardOnlyStream());
        await Assert.That(stream.Length).IsEqualTo(1024);
        await Assert.That(stream.Seek(-512, SeekOrigin.End)).IsEqualTo(512);
        await Assert.That(() => stream.Seek(-10, SeekOrigin.Current)).Throws<InvalidOperationException>();
#if NET
        await stream.DisposeAsync();
#else
        stream.Dispose();
#endif
    }

    [Test]
    public async Task SeekOnAlreadySeekableStream()
    {
#if NET
        await
#endif
        using var stream = new SeekableStream(new MemoryStream(512));
        await Assert.That(stream.Seek(256, SeekOrigin.Begin)).IsEqualTo(256);
    }

    [Test]
    public async Task SeekWithInvalidEnum()
    {
        var stream = new SeekableStream(new ForwardOnlyStream());
        await Assert.That(() => stream.Seek(0, (SeekOrigin)(-1))).Throws<ArgumentOutOfRangeException>();
#if NET
        await stream.DisposeAsync();
#else
        stream.Dispose();
#endif
    }

    private class ForwardOnlyStream() : MemoryStream(new byte[1024])
    {
        public override bool CanSeek => false;
    }
}