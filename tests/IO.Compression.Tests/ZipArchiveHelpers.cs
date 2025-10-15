namespace Altemiq.IO.Compression;

internal class ZipArchiveHelpers
{
    public class ZipArchiveShim(Stream stream, System.IO.Compression.ZipArchiveMode mode) : System.IO.Compression.ZipArchive(stream, mode)
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.IsDisposed = true;
        }
    }

    public static ZipArchiveShim CreateArchiveShim(System.IO.Compression.ZipArchiveMode mode = System.IO.Compression.ZipArchiveMode.Read, bool fillData = false)
    {
        var memoryStream = new MemoryStream();

        using (var archive = new System.IO.Compression.ZipArchive(memoryStream, System.IO.Compression.ZipArchiveMode.Create, true))
        {
            var entry = archive.CreateEntry("Single");
            if (fillData)
            {
                using var stream = entry.Open();
                var buffer = new byte[1024];
                var random = new Random();
                random.NextBytes(buffer);
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        memoryStream.Position = 0;

        return new(memoryStream, mode);
    }
}