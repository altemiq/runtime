// -----------------------------------------------------------------------
// <copyright file="JsonRuntimeFormatTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

#if DEBUG
public class JsonRuntimeFormatTests
{
    [Theory]
    [InlineData("PortableRuntimeIdentifierGraph.json.gz")]
    [InlineData("runtime.json.gz")]
    public void GetManifestStream(string name)
    {
        Assert.Null(Record.Exception(() => GetManifestStreamFromAssembly(name)));
    }

    [Theory]
    [InlineData("PortableRuntimeIdentifierGraph.json.gz")]
    [InlineData("runtime.json.gz")]
    public void ReadJsonFromAssembly(string name)
    {
        using var stream = new System.IO.Compression.GZipStream(GetManifestStreamFromAssembly(name), System.IO.Compression.CompressionMode.Decompress, false);
        var json = JsonRuntimeFormat.ReadRuntimeGraph(stream!);
        Assert.NotNull(json);
        Assert.InRange(json.Count, 10, int.MaxValue);

        Assert.Contains("linux", json.Select(static x => x.Runtime));
    }

    private static Stream GetManifestStreamFromAssembly(string name) => typeof(JsonRuntimeFormat).Assembly.GetManifestResourceStream(typeof(JsonRuntimeFormat), name) ?? throw new InvalidOperationException();
}
#endif