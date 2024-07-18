namespace Altemiq.IO.Compression;

public class DisposableStreamTests
{
    [Fact]
    public void DisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0]);
        stream.Dispose();
        _ = archive.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void NotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0], true);
        stream.Dispose();
        _ = archive.IsDisposed.Should().BeFalse();
    }

#if NETCOREAPP
    [Fact]
    public async Task DisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0]);
        await stream.DisposeAsync();
        _ = archive.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public async Task NotDisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0], true);
        await stream.DisposeAsync();
        _ = archive.IsDisposed.Should().BeFalse();
    }
#endif
}