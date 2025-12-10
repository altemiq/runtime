// -----------------------------------------------------------------------
// <copyright file="JsonRuntimeFormatTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

#if DEBUG
public class JsonRuntimeFormatTests
{
    [Test]
    [Arguments("PortableRuntimeIdentifierGraph.json.gz")]
    [Arguments("runtime.json.gz")]
    public async Task GetManifestStream(string name)
    {
        await Assert.That(() => GetManifestStreamFromAssembly(name)).ThrowsNothing();
    }

    [Test]
    [Arguments("PortableRuntimeIdentifierGraph.json.gz")]
    [Arguments("runtime.json.gz")]
    public async Task ReadJsonFromAssembly(string name)
    {
        var stream = new System.IO.Compression.GZipStream(GetManifestStreamFromAssembly(name), System.IO.Compression.CompressionMode.Decompress, false);
#if NETCOREAPP3_0_OR_GREATER
        await using (stream.ConfigureAwait(false))
#else
        using (stream)
#endif
        {
            var json = JsonRuntimeFormat.ReadRuntimeGraph(stream);
            await Assert.That(json)
                .Member(
                    static j => j.Select(static x => x.Runtime),
                    static runtimes => runtimes
                        .Contains("linux")
                        .And.Count().IsGreaterThanOrEqualTo(10));
        }
    }

    private static Stream GetManifestStreamFromAssembly(string name) => typeof(JsonRuntimeFormat).Assembly.GetManifestResourceStream(typeof(JsonRuntimeFormat), name) ?? throw new InvalidOperationException();
}
#endif