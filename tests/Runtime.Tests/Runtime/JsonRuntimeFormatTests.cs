// -----------------------------------------------------------------------
// <copyright file="JsonRuntimeFormatTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

using TUnit.Assertions.AssertConditions.Throws;

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
        using var stream = new System.IO.Compression.GZipStream(GetManifestStreamFromAssembly(name), System.IO.Compression.CompressionMode.Decompress, false);
        var json = JsonRuntimeFormat.ReadRuntimeGraph(stream!);
        await Assert.That(json).IsNotNull().And
            .HasCount().GreaterThan(10).And
            .HasCount().LessThan(int.MaxValue).And
            .Satisfies(static j => j.Select(static x => x.Runtime), runtimes => runtimes.Contains("linux"));
    }

    private static Stream GetManifestStreamFromAssembly(string name) => typeof(JsonRuntimeFormat).Assembly.GetManifestResourceStream(typeof(JsonRuntimeFormat), name) ?? throw new InvalidOperationException();
}
#endif