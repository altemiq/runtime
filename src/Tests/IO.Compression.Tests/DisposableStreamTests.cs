namespace Altemiq.IO.Compression;

public class DisposableStreamTests
{
    [Fact]
    public void ShouldDisposeTheEntry()
    {
        var archive = ZipArchiveHelpers.CreateArchiveShim();

        using (var stream = new DisposableStream(archive.Entries[0]))
        {
        }

        _ = archive.IsDisposed.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotDisposeTheEntry()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();

        using (var stream = new DisposableStream(archive.Entries[0], true))
        {
        }

        _ = archive.IsDisposed.Should().BeFalse();
    }
}