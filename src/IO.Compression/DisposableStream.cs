// -----------------------------------------------------------------------
// <copyright file="DisposableStream.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO.Compression;

/// <summary>
/// A <see cref="Stream"/> that can dispose of the <see cref="System.IO.Compression.ZipArchive"/>.
/// </summary>
public class DisposableStream : Stream
{
    private readonly Stream stream;

    private readonly System.IO.Compression.ZipArchive archive;

    private readonly bool closeArchive;

    /// <summary>
    /// Initialises a new instance of the <see cref="DisposableStream"/> class.
    /// </summary>
    /// <param name="archiveEntry">The archive entry.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="SeekableStream"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public DisposableStream(System.IO.Compression.ZipArchiveEntry archiveEntry, bool leaveOpen = false)
        : this(archiveEntry.Archive, archiveEntry.Open(), leaveOpen)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="DisposableStream"/> class.
    /// </summary>
    /// <param name="archive">The archive.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="SeekableStream"/> object is disposed; otherwise, <see langword="false"/>.</param>
    internal DisposableStream(System.IO.Compression.ZipArchive archive, Stream stream, bool leaveOpen = false)
    {
        this.archive = archive;
        this.stream = stream;
        this.closeArchive = !leaveOpen;
    }

    /// <inheritdoc/>
    public override bool CanRead => this.stream.CanRead;

    /// <inheritdoc/>
    public override bool CanSeek => this.stream.CanSeek;

    /// <inheritdoc/>
    public override bool CanTimeout => this.stream.CanTimeout;

    /// <inheritdoc/>
    public override bool CanWrite => this.stream.CanWrite;

    /// <inheritdoc/>
    public override long Length => this.stream.Length;

    /// <inheritdoc/>
    public override long Position { get => this.stream.Position; set => this.stream.Position = value; }

    /// <inheritdoc/>
    public override int ReadTimeout { get => this.stream.ReadTimeout; set => this.stream.ReadTimeout = value; }

    /// <inheritdoc/>
    public override int WriteTimeout { get => this.stream.WriteTimeout; set => this.stream.WriteTimeout = value; }

    /// <inheritdoc/>
    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object? state) => this.stream.BeginRead(buffer, offset, count, callback, state);

    /// <inheritdoc/>
    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object? state) => this.stream.BeginWrite(buffer, offset, count, callback, state);

    /// <inheritdoc/>
    public override int EndRead(IAsyncResult asyncResult) => this.stream.EndRead(asyncResult);

    /// <inheritdoc/>
    public override void EndWrite(IAsyncResult asyncResult) => this.stream.EndWrite(asyncResult);

    /// <inheritdoc/>
    public override void Close()
    {
        if (this.closeArchive && this.archive is not null)
        {
            this.archive.Dispose();
        }
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP1_0_OR_GREATER
    /// <inheritdoc/>
    public override void CopyTo(Stream destination, int bufferSize) => this.stream.CopyTo(destination, bufferSize);
#endif

    /// <inheritdoc/>
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) => this.stream.CopyToAsync(destination, bufferSize, cancellationToken);

    /// <inheritdoc/>
    public override bool Equals(object? obj) => this.stream.Equals(obj);

    /// <inheritdoc/>
    public override void Flush() => this.stream.Flush();

    /// <inheritdoc/>
    public override Task FlushAsync(CancellationToken cancellationToken) => this.stream.FlushAsync(cancellationToken);

    /// <inheritdoc/>
    public override int GetHashCode() => this.stream.GetHashCode();

    /// <inheritdoc/>
    public override object InitializeLifetimeService() => this.stream.InitializeLifetimeService();

    /// <inheritdoc/>
    public override int Read(byte[] buffer, int offset, int count) => this.stream.Read(buffer, offset, count);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override int Read(Span<byte> buffer) => this.stream.Read(buffer);
#endif

    /// <inheritdoc/>
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => this.stream.ReadAsync(buffer, offset, count, cancellationToken);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => this.stream.ReadAsync(buffer, cancellationToken);
#endif

    /// <inheritdoc/>
    public override int ReadByte() => this.stream.ReadByte();

    /// <inheritdoc/>
    public override long Seek(long offset, SeekOrigin origin) => this.stream.Seek(offset, origin);

    /// <inheritdoc/>
    public override void SetLength(long value) => this.stream.SetLength(value);

    /// <inheritdoc/>
    public override string? ToString() => this.stream.ToString();

    /// <inheritdoc/>
    public override void Write(byte[] buffer, int offset, int count) => this.stream.Write(buffer, offset, count);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override void Write(ReadOnlySpan<byte> buffer) => this.stream.Write(buffer);
#endif

    /// <inheritdoc/>
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => this.stream.WriteAsync(buffer, offset, count, cancellationToken);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => this.stream.WriteAsync(buffer, cancellationToken);
#endif

    /// <inheritdoc/>
    public override void WriteByte(byte value) => this.stream.WriteByte(value);

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
    /// <inheritdoc/>
    public override async ValueTask DisposeAsync()
    {
        if (this.closeArchive && this.archive is not null)
        {
            this.archive.Dispose();
        }

        await base.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
        GC.SuppressFinalize(this);
    }
#endif

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing && this.closeArchive && this.archive is not null)
        {
            this.archive.Dispose();
        }

        base.Dispose(disposing);
    }
}