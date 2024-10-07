// -----------------------------------------------------------------------
// <copyright file="MultipleStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

using NSubstitute;

public class MultipleStreamTests
{
    [Fact]
    public void FlushAllStreams()
    {
        var first = Substitute.For<Stream>();
        var second = Substitute.For<Stream>();
        var third = Substitute.For<Stream>();
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();

        multipleStream.Add(nameof(first), first);
        multipleStream.Add(nameof(second), second);
        multipleStream.Add(nameof(third), third);

        multipleStream.Flush();

        first.Received().Flush();
        second.Received().Flush();
        third.Received().Flush();
    }

    [Fact]
    public void DisposeAllStreams()
    {
        var first = Substitute.For<Stream>();
        var second = Substitute.For<Stream>();
        var third = Substitute.For<Stream>();
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();

        multipleStream.Add(nameof(first), first);
        multipleStream.Add(nameof(second), second);
        multipleStream.Add(nameof(third), third);

        multipleStream.Dispose();

        first.Received().Dispose();
        second.Received().Dispose();
        third.Received().Dispose();
    }

    [Fact]
    public async Task FlushAllStreamsAsync()
    {
        var first = Substitute.For<Stream>();
        var second = Substitute.For<Stream>();
        var third = Substitute.For<Stream>();
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();

        multipleStream.Add(nameof(first), first);
        multipleStream.Add(nameof(second), second);
        multipleStream.Add(nameof(third), third);

        await multipleStream.FlushAsync();

        Received.InOrder(async () =>
        {
            await first.FlushAsync();
            await second.FlushAsync();
            await third.FlushAsync();
        });
    }

#if NET
    [Fact]
    public async Task DisposeAllStreamsAsync()
    {
        var first = Substitute.For<Stream>();
        var second = Substitute.For<Stream>();
        var third = Substitute.For<Stream>();
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();

        multipleStream.Add(nameof(first), first);
        multipleStream.Add(nameof(second), second);
        multipleStream.Add(nameof(third), third);

        await multipleStream.DisposeAsync();

        Received.InOrder(async () =>
        {
            await first.DisposeAsync();
            await second.DisposeAsync();
            await third.DisposeAsync();
        });
    }
#endif

    [Fact]
    public void SwitchStream()
    {
        var first = new MemoryStream();
        var second = new MemoryStream();
        var third = new MemoryStream();
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();

        multipleStream.Add(nameof(first), first);
        multipleStream.Add(nameof(second), second);
        multipleStream.Add(nameof(third), third);

        var random = new Random();
        var secondBytes = new byte[byte.MaxValue];
        random.NextBytes(secondBytes);
        multipleStream.SwitchTo(nameof(second));
        multipleStream.Write(secondBytes, 0, secondBytes.Length);

        var thirdBytes = new byte[2 * byte.MaxValue];
        random.NextBytes(thirdBytes);
        multipleStream.SwitchTo(nameof(third));
        multipleStream.Write(thirdBytes, 0, thirdBytes.Length);

        first.ToArray().Should().BeEmpty();
        second.ToArray().Should().BeEquivalentTo(secondBytes);
        third.ToArray().Should().BeEquivalentTo(thirdBytes);
    }

    [Fact]
    public void TryAddTwice()
    {
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();
        multipleStream.TryAdd(nameof(multipleStream), static () => default!).Should().BeTrue();
        multipleStream.TryAdd(nameof(multipleStream), static () => default!).Should().BeFalse();
    }

    [Fact]
    public void ReadAcrossStreamsUsingArray()
    {
        const int FirstSize = 1024;
        const int SecondSize = 2048;
        const int ReadSize = FirstSize + ((SecondSize - FirstSize) / 2);

        var random = new Random();
        var first = new byte[FirstSize];
        random.NextBytes(first);
        var second = new byte[SecondSize];
        random.NextBytes(second);

        using var stream = new BasicMultipleStream(new MemoryStream(first), new MemoryStream(second));

        stream.Reset();

        var third = new byte[ReadSize];

        _ = stream.Read(third, 0, ReadSize).Should().Be(ReadSize);

        _ = new ArraySegment<byte>(third, 0, FirstSize).Should().BeEquivalentTo(first);
        _ = new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize).Should().BeEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }

    [Fact]
#if NET
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1835:Prefer the 'Memory'-based overloads for 'ReadAsync' and 'WriteAsync'", Justification = "We are trying to test an overload.")]
#endif
    public async Task ReadAcrossStreamsUsingArrayAsync()
    {
        const int FirstSize = 1024;
        const int SecondSize = 2048;
        const int ReadSize = FirstSize + ((SecondSize - FirstSize) / 2);

        var random = new Random();
        var first = new byte[FirstSize];
        random.NextBytes(first);
        var second = new byte[SecondSize];
        random.NextBytes(second);

        using var stream = new BasicMultipleStream(new MemoryStream(first), new MemoryStream(second));

        stream.Reset();

        var third = new byte[ReadSize];

        _ = (await stream.ReadAsync(third, 0, ReadSize, default)).Should().Be(ReadSize);

        _ = new ArraySegment<byte>(third, 0, FirstSize).Should().BeEquivalentTo(first);
        _ = new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize).Should().BeEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }

#if NET
    [Fact]
    public void ReadAcrossStreamsUsingSpan()
    {
        const int FirstSize = 1024;
        const int SecondSize = 2048;
        const int ReadSize = FirstSize + ((SecondSize - FirstSize) / 2);

        var random = new Random();
        var first = new byte[FirstSize];
        random.NextBytes(first);
        var second = new byte[SecondSize];
        random.NextBytes(second);

        using var stream = new BasicMultipleStream(new MemoryStream(first), new MemoryStream(second));

        stream.Reset();

        var third = new byte[ReadSize];

        _ = stream.Read(third.AsSpan()).Should().Be(ReadSize);

        _ = new ArraySegment<byte>(third, 0, FirstSize).Should().BeEquivalentTo(first);
        _ = new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize).Should().BeEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }

    [Fact]
    public async Task ReadAcrossStreamsUsingMemoryAsync()
    {
        const int FirstSize = 1024;
        const int SecondSize = 2048;
        const int ReadSize = FirstSize + ((SecondSize - FirstSize) / 2);

        var random = new Random();
        var first = new byte[FirstSize];
        random.NextBytes(first);
        var second = new byte[SecondSize];
        random.NextBytes(second);

        using var stream = new BasicMultipleStream(new MemoryStream(first), new MemoryStream(second));

        stream.Reset();

        var third = new byte[ReadSize];

        _ = (await stream.ReadAsync(third.AsMemory(), default)).Should().Be(ReadSize);

        _ = new ArraySegment<byte>(third, 0, FirstSize).Should().BeEquivalentTo(first);
        _ = new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize).Should().BeEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }
#endif

    [Fact]
    public void ReadFromSecondStream()
    {
        const int FirstSize = 1024;
        const int SecondSize = 2048;
        const int ThirdSize = 512;
        const int PositionToRead = 2100;
        var random = new Random();
        var first = new byte[FirstSize];
        random.NextBytes(first);
        var second = new byte[SecondSize];
        random.NextBytes(second);
        var third = new byte[ThirdSize];
        random.NextBytes(third);

        using var stream = new BasicMultipleStream(new MemoryStream(first), new MemoryStream(second), new MemoryStream(third));
        stream.Position = PositionToRead;

        var bytes = new byte[20];
        _ = stream.Read(bytes, 0, bytes.Length);

        var expectedBytes = new byte[20];
        System.Array.Copy(second, PositionToRead - FirstSize, expectedBytes, 0, expectedBytes.Length);

        _ = bytes.Should().BeEquivalentTo(expectedBytes);

        stream.Dispose();
    }

    [Fact]
    public void CopyTo()
    {
        const int FirstSize = 1024;
        const int SecondSize = 2048;
        const int ThirdSize = 512;
        var random = new Random();
        var first = new byte[FirstSize];
        random.NextBytes(first);
        var second = new byte[SecondSize];
        random.NextBytes(second);
        var third = new byte[ThirdSize];
        random.NextBytes(third);

        using var stream = new BasicMultipleStream(new MemoryStream(first), new MemoryStream(second), new MemoryStream(third));
        stream.Reset();

        var destination = new byte[FirstSize + SecondSize + ThirdSize];
        using (var memoryStream = new MemoryStream(destination))
        {
            stream.CopyTo(memoryStream);
        }

        _ = new ArraySegment<byte>(destination, 0, FirstSize).Should().BeEquivalentTo(first);
        _ = new ArraySegment<byte>(destination, FirstSize, SecondSize).Should().BeEquivalentTo(second);
        _ = new ArraySegment<byte>(destination, FirstSize + SecondSize, ThirdSize).Should().BeEquivalentTo(third);
    }

#if NET
    [Fact]
    public async Task CopyToAsync()
    {
        const int FirstSize = 1024;
        const int SecondSize = 2048;
        const int ThirdSize = 512;
        var random = new Random();
        var first = new byte[FirstSize];
        random.NextBytes(first);
        var second = new byte[SecondSize];
        random.NextBytes(second);
        var third = new byte[ThirdSize];
        random.NextBytes(third);

        using var stream = new BasicMultipleStream(new MemoryStream(first), new MemoryStream(second), new MemoryStream(third));
        stream.Reset();

        var destination = new byte[FirstSize + SecondSize + ThirdSize];
        using (var memoryStream = new MemoryStream(destination))
        {
            await stream.CopyToAsync(memoryStream, default(CancellationToken));
        }

        _ = new ArraySegment<byte>(destination, 0, FirstSize).Should().BeEquivalentTo(first);
        _ = new ArraySegment<byte>(destination, FirstSize, SecondSize).Should().BeEquivalentTo(second);
        _ = new ArraySegment<byte>(destination, FirstSize + SecondSize, ThirdSize).Should().BeEquivalentTo(third);
    }
#endif

    private sealed class BasicMultipleStream(params Stream[] streams) : MultipleStream(streams.Select(static (s, i) => (s, i)).ToDictionary(static x => x.i.ToString(), static x => x.s))
    {
        protected override Stream CreateStream(string name) => throw new NotImplementedException();
    }
}