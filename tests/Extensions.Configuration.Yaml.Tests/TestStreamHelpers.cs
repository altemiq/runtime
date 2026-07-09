namespace Altemiq.Extensions.Configuration.Yaml;

public class TestStreamHelpers
{
    public static readonly string ArbitraryFilePath = "Unit tests do not touch file system";

    public static Microsoft.Extensions.FileProviders.IFileProvider StringToFileProvider(string str)
    {
        return new TestFileProvider(str);

    }

    private sealed class TestFile(string str) : Microsoft.Extensions.FileProviders.IFileInfo
    {
        public bool Exists => true;
        public bool IsDirectory => false;
        public DateTimeOffset LastModified => throw new InvalidOperationException();
        public long Length => 0;
        public string Name => null!;
        public string PhysicalPath => null!;
        public Stream CreateReadStream()
        {
            return StringToStream(str);
        }
    }

    private sealed class TestFileProvider(string str) : Microsoft.Extensions.FileProviders.IFileProvider
    {
        public Microsoft.Extensions.FileProviders.IDirectoryContents GetDirectoryContents(string subpath)
        {
            throw new InvalidOperationException();
        }

        public Microsoft.Extensions.FileProviders.IFileInfo GetFileInfo(string subpath)
        {
            return new TestFile(str);
        }

        public Microsoft.Extensions.Primitives.IChangeToken Watch(string filter)
        {
            throw new InvalidOperationException();
        }
    }

    public static Stream StringToStream(string str)
    {
        var memStream = new MemoryStream();
        var textWriter = new StreamWriter(memStream);
        textWriter.Write(str);
        textWriter.Flush();
        _ = memStream.Seek(0, SeekOrigin.Begin);

        return memStream;
    }

    public static string StreamToString(Stream stream)
    {
        _ = stream.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}