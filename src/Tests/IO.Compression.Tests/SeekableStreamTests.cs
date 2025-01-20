namespace Altemiq.IO.Compression;

public class SeekableStreamTests
{
    [Fact]
    public void ShouldDisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0]);
        stream.Dispose();
        Assert.True(archive.IsDisposed);
    }

    [Fact]
    public void ShouldNotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0], true);
        stream.Dispose();
        Assert.False(archive.IsDisposed);
    }

#if NETCOREAPP
    [Fact]
    public async Task ShouldDisposeTheEntryAsync()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0]);
        await stream.DisposeAsync();
        Assert.True(archive.IsDisposed);
    }

    [Fact]
    public async Task ShouldNotDisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0], true);
        await stream.DisposeAsync();
        Assert.False(archive.IsDisposed);
    }
#endif

    [Fact]
    public void ShouldBeAbleToSeekForward()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(fillData: true);
        using var stream = new SeekableStream(archive.Entries[0], false);
        Assert.Equal(1024, stream.Length);
        Assert.Equal(512, stream.Seek(512, SeekOrigin.Begin));
    }
}