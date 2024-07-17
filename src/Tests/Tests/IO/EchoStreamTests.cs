// -----------------------------------------------------------------------
// <copyright file="EchoStreamTests.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altavec.IO;

using System.Text;
using System.Threading.Tasks;

public class EchoStreamTests
{
    [Fact]
    public void ReadFromWrite()
    {
        const string Text = "text to read and write";
        using var stream = new EchoStream();

        var task = Task.Run(() =>
        {
            Thread.Sleep(50);
            var bytes = Encoding.UTF8.GetBytes(Text);
            stream.Write(bytes, 0, bytes.Length);
        });

        var expectedLength = Encoding.UTF8.GetByteCount(Text);
        var bytes = new byte[expectedLength];
        var read = stream.Read(bytes, 0, expectedLength);

        _ = read.Should().Be(expectedLength);
        _ = Encoding.UTF8.GetString(bytes).Should().Be(Text);
    }

    [Fact]
    public void ReadFromMultipleWrite()
    {
        const string Text1 = "text to re";
        const string Text2 = "ad an";
        const string Text3 = "d write";
        const string Text = Text1 + Text2 + Text3;
        using var stream = new EchoStream();

        var task = Task.Run(() =>
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
        });

        var expectedLength = Encoding.UTF8.GetByteCount(Text);
        var bytes = new byte[expectedLength];
        var read = stream.Read(bytes, 0, expectedLength);

        _ = read.Should().Be(expectedLength);
        _ = Encoding.UTF8.GetString(bytes).Should().Be(Text);
    }

    [Fact]
    public void TimeoutReadFromMultipleWrite()
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

        _ = read.Should().Be(Encoding.UTF8.GetByteCount(Text1));
        _ = Encoding.UTF8.GetString(bytes, 0, read).Should().Be(Text1);
    }
}