namespace Altemiq.IO.Compression;

public class DisposableStreamTests
{
    [Fact]
    public void DisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0]);
        stream.Dispose();
        Assert.True(archive.IsDisposed);
    }

    [Fact]
    public void NotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0], true);
        stream.Dispose();
        Assert.False(archive.IsDisposed);
    }

#if NETCOREAPP
    [Fact]
    public async Task DisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0]);
        await stream.DisposeAsync();
        Assert.True(archive.IsDisposed);
    }

    [Fact]
    public async Task NotDisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0], true);
        await stream.DisposeAsync();
        Assert.False(archive.IsDisposed);
    }
#endif
}