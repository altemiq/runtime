namespace Altemiq.IO.Compression;

public class SeekableStreamTests
{
    [Fact]
    public void ShouldDisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0]);
        stream.Dispose();
        _ = archive.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0], true);
        stream.Dispose();
        _ = archive.IsDisposed.Should().BeFalse();
    }

#if NETCOREAPP
    [Fact]
    public async Task ShouldDisposeTheEntryAsync()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0]);
        await stream.DisposeAsync();
        _ = archive.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public async Task ShouldNotDisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0], true);
        await stream.DisposeAsync();
        _ = archive.IsDisposed.Should().BeFalse();
    }
#endif

    [Fact]
    public void ShouldBeAbleToSeekForward()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(fillData: true);
        using var stream = new SeekableStream(archive.Entries[0], false);
        _ = stream.Length.Should().Be(1024);
        _ = stream.Seek(512, SeekOrigin.Begin).Should().Be(512);
    }
}