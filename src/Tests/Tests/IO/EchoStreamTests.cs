// -----------------------------------------------------------------------
// <copyright file="EchoStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

using System.Text;
using System.Threading.Tasks;

public class EchoStreamTests
{
    [Test]
    public async Task ReadFromWrite()
    {
        const string Text = "text to read and write";
        using var stream = new EchoStream();

        var task = Task.Run(
            () =>
            {
                Thread.Sleep(50);
                var bytes = Encoding.UTF8.GetBytes(Text);
                stream.Write(bytes, 0, bytes.Length);
            },
            TestContext.Current?.CancellationToken ?? CancellationToken.None);

        var expectedLength = Encoding.UTF8.GetByteCount(Text);
        var bytes = new byte[expectedLength];
        var read = stream.Read(bytes, 0, expectedLength);

        await Assert.That(read).IsEqualTo(expectedLength);
        await Assert.That(Encoding.UTF8.GetString(bytes)).IsEqualTo(Text);
    }

    [Test]
    public async Task ReadFromMultipleWrite()
    {
        const string Text1 = "text to re";
        const string Text2 = "ad an";
        const string Text3 = "d write";
        const string Text = Text1 + Text2 + Text3;
        using var stream = new EchoStream();

        var task = Task.Run(
            () =>
            {
                Write(stream, Text1);
                Write(stream, Text2);
                Write(stream, Text3);
                static void Write(Stream stream, string text)
                {
                    Thread.Sleep(50);
                    var bytes = Encoding.UTF8.GetBytes(text);
                    stream.Write(bytes, 0, bytes.Length);
                }
            },
            TestContext.Current?.CancellationToken ?? CancellationToken.None);

        var expectedLength = Encoding.UTF8.GetByteCount(Text);
        var bytes = new byte[expectedLength];
        var read = stream.Read(bytes, 0, expectedLength);

        await Assert.That(read).IsEqualTo(expectedLength);
        await Assert.That(Encoding.UTF8.GetString(bytes)).IsEqualTo(Text);
    }

    [Test]
    public async Task TimeoutReadFromMultipleWrite()
    {
        const string Text1 = "text to re";
        const string Text2 = "ad an";
        const string Text3 = "d write";
        const string Text = Text1 + Text2 + Text3;
        using var stream = new EchoStream { ReadTimeout = 1 };

        var bytes = Encoding.UTF8.GetBytes(Text1);
        stream.Write(bytes, 0, bytes.Length);
        var expectedLength = Encoding.UTF8.GetByteCount(Text);
        bytes = new byte[expectedLength];
        var read = stream.Read(bytes, 0, expectedLength);

        await Assert.That(read).IsEqualTo(Encoding.UTF8.GetByteCount(Text1));
        await Assert.That(Encoding.UTF8.GetString(bytes, 0, read)).IsEqualTo(Text1);
    }
}