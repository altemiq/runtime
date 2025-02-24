namespace Altemiq.IO.Compression;

public class DisposableStreamTests
{
    [Test]
    public async Task DisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0]);
        stream.Dispose();
        await Assert.That(archive.IsDisposed).IsTrue();
    }

    [Test]
    public async Task NotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0], true);
        stream.Dispose();
        await Assert.That(archive.IsDisposed).IsFalse();
    }

#if NETCOREAPP
    [Test]
    public async Task DisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0]);
        await stream.DisposeAsync();
        await Assert.That(archive.IsDisposed).IsTrue();
    }

    [Test]
    public async Task NotDisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0], true);
        await stream.DisposeAsync();
        await Assert.That(archive.IsDisposed).IsFalse();
    }
#endif
}