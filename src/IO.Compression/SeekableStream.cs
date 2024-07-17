// -----------------------------------------------------------------------
// <copyright file="SeekableStream.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Compression;

/// <summary>
/// A seekable <see cref="Stream"/> that can dispose of the <see cref="System.IO.Compression.ZipArchive"/>.
/// </summary>
public class SeekableStream : IO.SeekableStream
{
    private readonly System.IO.Compression.ZipArchive archive;

    private readonly long length;

    private readonly bool closeArchive;

    /// <summary>
    /// Initialises a new instance of the <see cref="SeekableStream"/> class.
    /// </summary>
    /// <param name="archiveEntry">The archive entry.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="SeekableStream"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public SeekableStream(System.IO.Compression.ZipArchiveEntry archiveEntry, bool leaveOpen = false)
        : this(archiveEntry.Archive, archiveEntry.Open(), archiveEntry.Length, leaveOpen)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SeekableStream"/> class.
    /// </summary>
    /// <param name="archive">The archive.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="length">The stream length.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="SeekableStream"/> object is disposed; otherwise, <see langword="false"/>.</param>
    internal SeekableStream(System.IO.Compression.ZipArchive archive, Stream stream, long length, bool leaveOpen = false)
        : this(archive, stream, length, 0, leaveOpen)
    {
    }

    private SeekableStream(System.IO.Compression.ZipArchive archive, Stream stream, long length, long initialPosition = 0L, bool leaveOpen = false)
        : base(stream, initialPosition, leaveOpen: false)
    {
        this.archive = archive;
        this.closeArchive = !leaveOpen;
        this.length = length;
    }

    /// <inheritdoc/>
    public override long Length => this.length;

#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER
    /// <inheritdoc/>
    public override void Close()
    {
        base.Close();
        if (this.closeArchive && this.archive is not null)
        {
            this.archive.Dispose();
        }
    }
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
    /// <inheritdoc/>
    public override async ValueTask DisposeAsync()
    {
        await base.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
        if (this.closeArchive)
        {
            if (this.archive is IAsyncDisposable asyncDisposable)
            {
                await asyncDisposable.DisposeAsync().ConfigureAwait(false);
            }
            else if (this.archive is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
        GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
    }
#endif

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing && this.closeArchive && this.archive is not null)
        {
            this.archive.Dispose();
        }
    }
}