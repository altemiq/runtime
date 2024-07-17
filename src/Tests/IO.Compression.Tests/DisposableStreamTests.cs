namespace Altemiq.IO.Compression;

public class DisposableStreamTests
{
    [Fact]
    public void ShouldDisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0]);
        stream.Dispose();
        _ = archive.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        var stream = new DisposableStream(archive.Entries[0], true);
        stream.Dispose();
        _ = archive.IsDisposed.Should().BeFalse();
    }
}