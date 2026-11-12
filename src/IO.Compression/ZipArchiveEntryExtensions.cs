// -----------------------------------------------------------------------
// <copyright file="ZipArchiveEntryExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System.IO.Compression;
#pragma warning restore IDE0130, CheckNamespace

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
        /// <exception cref="ArgumentNullException"><paramref name="entry"/> is <see langword="null"/>.</exception>
        public Stream OpenSeekable(bool leaveOpen = true)
#pragma warning disable CA2208
            => (entry, entry?.Open(), leaveOpen) switch
            {
                (_, { CanSeek: true } seekable, true) => seekable,
                (_, { } stream, true) => new Altemiq.IO.SeekableStream(stream),
                ({ Archive: { } archive }, { CanSeek: true } seekable, false) => new Altemiq.IO.Compression.DisposableStream(archive, seekable, leaveOpen),
                ({ Archive: { } archive, Length: var length }, { } stream, false) => new Altemiq.IO.Compression.SeekableStream(archive, stream, length, leaveOpen),
                _ => throw new ArgumentNullException(nameof(entry)),
            };
#pragma warning restore CA2208
    }
}