// -----------------------------------------------------------------------
// <copyright file="JsonRuntimeFormatTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

#if DEBUG
public class JsonRuntimeFormatTests
{
    [Fact]
    public void GetManifestStream()
    {
        Func<Stream> func = GetManifestStreamFromAssembly;
        func.Should().NotThrow<InvalidOperationException>();
    }

    [Fact]
    public void ReadJsonFromAssembly()
    {
        using var stream = new System.IO.Compression.GZipStream(GetManifestStreamFromAssembly(), System.IO.Compression.CompressionMode.Decompress, false);
        var json = JsonRuntimeFormat.ReadRuntimeGraph(stream!);
        json.Should().NotBeNull().And.HaveCountGreaterThan(10);

        json[0].Runtime.Should().Be("alpine");
    }

    private static Stream GetManifestStreamFromAssembly() => typeof(JsonRuntimeFormat).Assembly.GetManifestResourceStream(typeof(JsonRuntimeFormat), "runtime.json.gz") ?? throw new InvalidOperationException();
}
#endif