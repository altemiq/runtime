// -----------------------------------------------------------------------
// <copyright file="ZipArchiveEntryExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Compression;

using System.IO.Compression;

public class ZipArchiveEntryExtensionsTests
{
    [Fact]
    public void OpenSeekableOnNonSeekable()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim();
        Assert.IsType<SeekableStream>(archive.Entries[0].OpenSeekable());
    }

    [Fact]
    public void OpenSeekableOnSeekable()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(ZipArchiveMode.Update);
        Assert.IsType<DisposableStream>(archive.Entries[0].OpenSeekable());
    }
}