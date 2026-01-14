// -----------------------------------------------------------------------
// <copyright file="ZipArchiveEntryExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System.IO.Compression;
#pragma warning restore IDE0130, CheckNamespace

#pragma warning disable RCS1263, SA1101, S2325

/// <summary>
/// <see cref="ZipArchiveEntry"/> extension methods.
/// </summary>
public static class ZipArchiveEntryExtensions
{
    /// <content>
    /// <see cref="ZipArchiveEntry"/> extension methods.
    /// </content>
    /// <param name="entry">The entry.</param>
    extension(ZipArchiveEntry entry)
    {
        /// <summary>
        /// Opens the entry from the zip archive as a forward only seekable stream.
        /// </summary>
        /// <returns>The seekable stream that represents the contents of the entry.</returns>
        /// <param name="leaveOpen"><see langword="true"/> to leave this entry open after the <see cref="Stream"/> object is disposed; otherwise, <see langword="false"/>.</param>
        public Stream OpenSeekable(bool leaveOpen = true)
        {
            if (entry is not { Archive: { } archive, Length: var length })
            {
#pragma warning disable CA2208
                throw new ArgumentNullException(nameof(entry));
#pragma warning restore CA2208
            }

            var stream = entry.Open();
            return stream.CanSeek
                ? new Altemiq.IO.Compression.DisposableStream(archive, stream, leaveOpen)
                : new Altemiq.IO.Compression.SeekableStream(archive, stream, length, leaveOpen);
        }
    }
}