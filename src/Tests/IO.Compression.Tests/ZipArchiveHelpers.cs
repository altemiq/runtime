namespace Altemiq.IO.Compression;

internal class ZipArchiveHelpers
{
    public class ZipArchiveShim(Stream stream) : System.IO.Compression.ZipArchive(stream)
    {
        public bool IsDisposed { get; private set; }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            this.IsDisposed = true;
        }
    }

    public static ZipArchiveShim CreateArchiveShim(bool fillData = false)
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

        return new ZipArchiveShim(memoryStream);
    }
}