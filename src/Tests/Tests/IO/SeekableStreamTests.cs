// -----------------------------------------------------------------------
// <copyright file="SeekableStreamTests.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altavec.IO;

public class SeekableStreamTests
{
    [Fact]
    public void ShouldBeAbleToSeekForward()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        _ = stream.Length.Should().Be(1024);
        _ = stream.Seek(512, SeekOrigin.Begin).Should().Be(512);
    }

    [Fact]
    public void ShouldNotBeAbleToSeekBackwards()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        _ = stream.Length.Should().Be(1024);
        _ = stream.Seek(-512, SeekOrigin.End).Should().Be(512);
        stream.Invoking(s => s.Seek(-10, SeekOrigin.Current)).Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void SeekOnAlreadySeekableStream()
    {
        using var stream = new SeekableStream(new MemoryStream(512));
        stream.Seek(256, SeekOrigin.Begin).Should().Be(256);
    }

    [Fact]
    public void SeekWithInvalidEnum()
    {
        using var stream = new SeekableStream(new ForwardOnlyStream());
        stream.Invoking(s => s.Seek(0, (SeekOrigin)(-1))).Should().Throw<ArgumentOutOfRangeException>();
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
