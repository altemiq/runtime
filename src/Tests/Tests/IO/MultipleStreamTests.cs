// -----------------------------------------------------------------------
// <copyright file="MultipleStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

using NSubstitute;

public class MultipleStreamTests
{
    [Test]
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

    [Test]
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

    [Test]
    public async Task FlushAllStreamsAsync()
    {
        var first = Substitute.For<Stream>();
        var second = Substitute.For<Stream>();
        var third = Substitute.For<Stream>();
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();

        multipleStream.Add(nameof(first), first);
        multipleStream.Add(nameof(second), second);
        multipleStream.Add(nameof(third), third);

        var cancellationToken = TestContext.Current?.CancellationToken ?? CancellationToken.None;
        await multipleStream.FlushAsync(cancellationToken);

        Received.InOrder(async () =>
        {
            await first.FlushAsync(cancellationToken);
            await second.FlushAsync(cancellationToken);
            await third.FlushAsync(cancellationToken);
        });
    }

#if NET
    [Test]
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

    [Test]
    public async Task SwitchStream()
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

        await Assert.That(first.ToArray()).IsEmpty();
        await Assert.That(second.ToArray()).IsEquivalentTo(secondBytes);
        await Assert.That(third.ToArray()).IsEquivalentTo(thirdBytes);
    }

    [Test]
    public async Task TryAddTwice()
    {
        var multipleStream = Substitute.ForPartsOf<MultipleStream>();
        await Assert.That(multipleStream.TryAdd(nameof(multipleStream), static () => default!)).IsTrue();
        await Assert.That(multipleStream.TryAdd(nameof(multipleStream), static () => default!)).IsFalse();
    }

    [Test]
    public async Task ReadAcrossStreamsUsingArray()
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

        await Assert.That(stream.Read(third, 0, ReadSize)).IsEqualTo(ReadSize);

        await Assert.That(new ArraySegment<byte>(third, 0, FirstSize)).IsEquivalentTo(new ArraySegment<byte>(first));
        await Assert.That(new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize)).IsEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }

    [Test]
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

        await Assert.That(await stream.ReadAsync(third, 0, ReadSize, TestContext.Current?.CancellationToken ?? CancellationToken.None)).IsEqualTo(ReadSize);

        await Assert.That(new ArraySegment<byte>(third, 0, FirstSize)).IsEquivalentTo(new ArraySegment<byte>(first));
        await Assert.That(new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize)).IsEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }

#if NET
    [Test]
    public async Task ReadAcrossStreamsUsingSpan()
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

        await Assert.That(stream.Read(third.AsSpan())).IsEqualTo(ReadSize);

        await Assert.That(new ArraySegment<byte>(third, 0, FirstSize)).IsEquivalentTo(new ArraySegment<byte>(first));
        await Assert.That(new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize)).IsEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }

    [Test]
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

        await Assert.That(await stream.ReadAsync(third.AsMemory(), TestContext.Current?.CancellationToken ?? CancellationToken.None)).IsEqualTo(ReadSize);

        await Assert.That(new ArraySegment<byte>(third, 0, FirstSize)).IsEquivalentTo(new ArraySegment<byte>(first));
        await Assert.That(new ArraySegment<byte>(third, FirstSize, ReadSize - FirstSize)).IsEquivalentTo(new ArraySegment<byte>(second, 0, ReadSize - FirstSize));
    }
#endif

    [Test]
    public async Task ReadFromSecondStream()
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

        await Assert.That(bytes).IsEquivalentTo(expectedBytes);

        stream.Dispose();
    }

    [Test]
    public async Task CopyTo()
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

        await Assert.That(new ArraySegment<byte>(destination, 0, FirstSize)).IsEquivalentTo(new ArraySegment<byte>(first));
        await Assert.That(new ArraySegment<byte>(destination, FirstSize, SecondSize)).IsEquivalentTo(new ArraySegment<byte>(second));
        await Assert.That(new ArraySegment<byte>(destination, FirstSize + SecondSize, ThirdSize)).IsEquivalentTo(new ArraySegment<byte>(third));
    }

#if NET
    [Test]
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
            await stream.CopyToAsync(memoryStream, TestContext.Current?.CancellationToken ?? CancellationToken.None);
        }

        await Assert.That(new ArraySegment<byte>(destination, 0, FirstSize)).IsEquivalentTo(first);
        await Assert.That(new ArraySegment<byte>(destination, FirstSize, SecondSize)).IsEquivalentTo(second);
        await Assert.That(new ArraySegment<byte>(destination, FirstSize + SecondSize, ThirdSize)).IsEquivalentTo(third);
    }
#endif

    private sealed class BasicMultipleStream(params Stream[] streams) : MultipleStream(streams.Select(static (s, i) => (s, i)).ToDictionary(static x => x.i.ToString(), static x => x.s))
    {
        protected override Stream CreateStream(string name) => throw new NotImplementedException();
    }
}