// -----------------------------------------------------------------------
// <copyright file="SeekableStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class SeekableStreamTests
{
    [Fact]
    public void ShouldBeAbleToSeekForward()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        Assert.Equal(1024, stream.Length);
        Assert.Equal(512, stream.Seek(512, SeekOrigin.Begin));
    }

    [Fact]
    public void ShouldNotBeAbleToSeekBackwards()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        Assert.Equal(1024, stream.Length);
        Assert.Equal(512, stream.Seek(-512, SeekOrigin.End));
        Assert.Throws<InvalidOperationException>(() => stream.Seek(-10, SeekOrigin.Current));
    }

    [Fact]
    public void SeekOnAlreadySeekableStream()
    {
        using var stream = new SeekableStream(new MemoryStream(512));
        Assert.Equal(256, stream.Seek(256, SeekOrigin.Begin));
    }

    [Fact]
    public void SeekWithInvalidEnum()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        Assert.Throws<ArgumentOutOfRangeException>(() => stream.Seek(0, (SeekOrigin)(-1)));
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