// -----------------------------------------------------------------------
// <copyright file="ZipArchiveEntryExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------


namespace Altemiq.IO.Compression;

using System.IO.Compression;

public class ZipArchiveEntryExtensionsTests
{
    [Test]
    public async Task OpenSeekableOnNonSeekable()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        _ = await Assert.That(archive.Entries[0].OpenSeekable()).HasProperty(static x => x.CanSeek, true);
    }

    [Test]
    public async Task OpenSeekableOnNonSeekableWhenClosing()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        _ = await Assert.That(archive.Entries[0].OpenSeekable(false))
#if NET11_0_OR_GREATER
            .IsTypeOf<DisposableStream>();
#else
            .IsTypeOf<SeekableStream>();
#endif
    }

    [Test]
    public async Task OpenSeekableOnSeekable()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(ZipArchiveMode.Update);
        _ = await Assert.That(archive.Entries[0].OpenSeekable()).IsNotNull();
    }

    [Test]
    public async Task OpenSeekableOnSeekableWhenClosing()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(ZipArchiveMode.Update);
        _ = await Assert.That(archive.Entries[0].OpenSeekable(false)).IsTypeOf<DisposableStream>();
    }
}