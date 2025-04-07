// -----------------------------------------------------------------------
// <copyright file="ZipArchiveEntryExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130
namespace System.IO.Compression;
#pragma warning restore IDE0130

/// <summary>
/// <see cref="ZipArchiveEntry"/> extension methods.
/// </summary>
public static class ZipArchiveEntryExtensions
{
    /// <summary>
    /// Opens the entry from the zip archive as a forward only seekable stream.
    /// </summary>
    /// <param name="entry">The entry.</param>
    /// <returns>The seekable stream that represents the contents of the entry.</returns>
    /// <param name="leaveOpen"><see langword="true"/> to leave the <paramref name="entry"/> open after the <see cref="Stream"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public static Stream OpenSeekable(this ZipArchiveEntry entry, bool leaveOpen = true)
    {
        if (entry?.Archive is null)
        {
            throw new ArgumentNullException(nameof(entry));
        }

        var stream = entry.Open();
        return stream.CanSeek
            ? new Altemiq.IO.Compression.DisposableStream(entry.Archive, stream, leaveOpen)
            : new Altemiq.IO.Compression.SeekableStream(entry.Archive, stream, entry.Length, leaveOpen);
    }
}