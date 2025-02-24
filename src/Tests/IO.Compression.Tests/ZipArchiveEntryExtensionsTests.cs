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
        await Assert.That(archive.Entries[0].OpenSeekable()).IsTypeOf<SeekableStream>();
    }

    [Test]
    public async Task OpenSeekableOnSeekable()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(ZipArchiveMode.Update);
        await Assert.That(archive.Entries[0].OpenSeekable()).IsTypeOf<DisposableStream>();
    }
}