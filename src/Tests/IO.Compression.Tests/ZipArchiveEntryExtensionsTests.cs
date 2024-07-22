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
        archive.Entries[0].OpenSeekable().Should().BeOfType<SeekableStream>();
    }

    [Fact]
    public void OpenSeekableOnSeekable()
    {
        using var archive = ZipArchiveHelpers.CreateArchiveShim(ZipArchiveMode.Update);
        archive.Entries[0].OpenSeekable().Should().BeOfType<DisposableStream>();
    }
}