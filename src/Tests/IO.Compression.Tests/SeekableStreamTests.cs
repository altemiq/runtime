namespace Altemiq.IO.Compression;

public class SeekableStreamTests
{
    [Fact]
    public void ShouldDisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();

        using (var stream = new SeekableStream(archive.Entries[0]))
        {
        }

        _ = archive.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();

        using (var stream = new SeekableStream(archive.Entries[0], true))
        {
        }

        _ = archive.IsDisposed.Should().BeFalse();
    }

    [Fact]
    public void ShouldBeAbleToSeekForward()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(true);
        using var stream = new SeekableStream(archive.Entries[0], false);
        _ = stream.Length.Should().Be(1024);
        _ = stream.Seek(512, SeekOrigin.Begin).Should().Be(512);
    }
}