namespace Altemiq.IO.Compression;

public class SeekableStreamTests
{
    [Test]
    public async Task ShouldDisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0]);
        stream.Dispose();
        await Assert.That(archive.IsDisposed).IsTrue();
    }

    [Test]
    public async Task ShouldNotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0], true);
        stream.Dispose();
        await Assert.That(archive.IsDisposed).IsFalse();
    }

#if NETCOREAPP
    [Test]
    public async Task ShouldDisposeTheEntryAsync()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0]);
        await stream.DisposeAsync();
        await Assert.That(archive.IsDisposed).IsTrue();
    }

    [Test]
    public async Task ShouldNotDisposeTheEntryAsync()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new SeekableStream(archive.Entries[0], true);
        await stream.DisposeAsync();
        await Assert.That(archive.IsDisposed).IsFalse();
    }
#endif

    [Test]
    public async Task ShouldBeAbleToSeekForward()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(fillData: true);
        using var stream = new SeekableStream(archive.Entries[0], false);
        await Assert.That(stream.Length).IsEqualTo(1024);
        await Assert.That(stream.Seek(512, SeekOrigin.Begin)).IsEqualTo(512);
    }
}