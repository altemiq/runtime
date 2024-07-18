// -----------------------------------------------------------------------
// <copyright file="SeekableStream.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

/// <summary>
/// A wrapper that makes a <see cref="Stream"/> seekable, when <see cref="Stream.CanSeek"/> is <see langword="false"/>.
/// </summary>
public class SeekableStream : Stream
{
    private readonly Stream stream;

    private readonly bool closeStream;

    private long position;

    /// <summary>
    /// Initialises a new instance of the <see cref="SeekableStream"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="SeekableStream"/> object is disposed; otherwise, <see langword="false"/>.</param>
    public SeekableStream(Stream stream, bool leaveOpen = false)
        : this(stream, 0L, leaveOpen: leaveOpen)
    {
    }

    /// <summary>
    /// Initialises a new instance of the <see cref="SeekableStream"/> class.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="initialPosition">The initial position.</param>
    /// <param name="leaveOpen"><see langword="true"/> to leave the stream open after the <see cref="SeekableStream"/> object is disposed; otherwise, <see langword="false"/>.</param>
    protected SeekableStream(Stream stream, long initialPosition = 0L, bool leaveOpen = false)
    {
        this.stream = stream;
        this.position = initialPosition;
        this.closeStream = !leaveOpen;
    }

    /// <inheritdoc/>
    public override bool CanRead => this.stream.CanRead;

    /// <inheritdoc/>
    public override bool CanWrite => false;

    /// <inheritdoc/>
    public override bool CanTimeout => this.stream.CanTimeout;

    /// <inheritdoc/>
    public override bool CanSeek => true;

    /// <inheritdoc/>
    public override long Position { get => this.position; set => this.position = this.ForwardPosition(value); }

    /// <inheritdoc/>
    public override long Length => this.stream.Length;

    /// <inheritdoc/>
    public override int ReadTimeout { get => this.stream.ReadTimeout; set => this.stream.ReadTimeout = value; }

    /// <inheritdoc/>
    public override int WriteTimeout { get => this.stream.WriteTimeout; set => this.stream.WriteTimeout = value; }

    /// <summary>
    /// Wraps the <see cref="Stream"/> if <see cref="Stream.CanSeek"/> is <see langword="false"/>.
    /// </summary>
    /// <param name="stream">The stream to wrap.</param>
    /// <returns>And instance of <see cref="SeekableStream"/> if <see cref="Stream.CanSeek"/> is <see langword="false"/> for <paramref name="stream"/>; otherwise <paramref name="stream"/>.</returns>
    public static Stream WrapIfRequired(Stream stream) => stream.CanSeek ? stream : new SeekableStream(stream, leaveOpen: false);

#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override void Close()
    {
        if (this.closeStream)
        {
            this.stream.Close();
        }

        base.Close();
    }

    /// <inheritdoc/>
    public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) => this.stream.BeginRead(buffer, offset, count, callback, state);

    /// <inheritdoc/>
    public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state) => throw new NotSupportedException();

    /// <inheritdoc/>
    public override int EndRead(IAsyncResult asyncResult) => this.stream.EndRead(asyncResult);

    /// <inheritdoc/>
    public override void EndWrite(IAsyncResult asyncResult) => throw new NotSupportedException();
#endif

    /// <inheritdoc/>
    public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken) => this.stream.CopyToAsync(destination, bufferSize, cancellationToken);

    /// <inheritdoc/>
    public override void Flush() => this.stream.Flush();

    /// <inheritdoc/>
    public override Task FlushAsync(CancellationToken cancellationToken) => base.FlushAsync(cancellationToken);

    /// <inheritdoc/>
    public override int Read(byte[] buffer, int offset, int count)
    {
        var value = this.stream.Read(buffer, offset, count);
        this.position += value;
        return value;
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override int Read(Span<byte> buffer)
    {
        var value = this.stream.Read(buffer);
        this.position += value;
        return value;
    }
#endif

    /// <inheritdoc/>
    public override int ReadByte()
    {
        var value = this.stream.ReadByte();
        this.position++;
        return value;
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override void CopyTo(Stream destination, int bufferSize) => this.stream.CopyTo(destination, bufferSize);

    /// <inheritdoc/>
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => await this.ReadAsync(buffer.AsMemory(offset, count), cancellationToken).ConfigureAwait(continueOnCapturedContext: false);

    /// <inheritdoc/>
    public override async ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
    {
        var value = await this.stream.ReadAsync(buffer, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        this.position += value;
        return value;
    }
#else
    /// <inheritdoc/>
    public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
    {
        var value = await this.stream.ReadAsync(buffer, offset, count, cancellationToken).ConfigureAwait(continueOnCapturedContext: false);
        this.position += value;
        return value;
    }
#endif

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
    /// <inheritdoc/>
    public override async ValueTask DisposeAsync()
    {
        if (this.closeStream)
        {
            await this.stream.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
        }

        await base.DisposeAsync().ConfigureAwait(continueOnCapturedContext: false);
#pragma warning disable S3971 // "GC.SuppressFinalize" should not be called
        GC.SuppressFinalize(this);
#pragma warning restore S3971 // "GC.SuppressFinalize" should not be called
    }
#endif

    /// <inheritdoc/>
    public override long Seek(long offset, SeekOrigin origin)
    {
        if (this.stream.CanSeek)
        {
            return this.stream.Seek(offset, origin);
        }

        var desiredPosition = origin switch
        {
            SeekOrigin.Begin => offset,
            SeekOrigin.Current => this.position + offset,
            SeekOrigin.End => this.Length - offset,
            _ => throw new ArgumentOutOfRangeException(nameof(origin)),
        };

        return this.ForwardPosition(desiredPosition);
    }

    /// <inheritdoc/>
    public override void SetLength(long value) => this.stream.SetLength(value);

    /// <inheritdoc/>
    public override void Write(byte[] buffer, int offset, int count) => throw new InvalidOperationException();

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override void Write(ReadOnlySpan<byte> buffer) => throw new NotSupportedException();
#endif

    /// <inheritdoc/>
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => throw new NotSupportedException();

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => throw new NotSupportedException();
#endif

    /// <inheritdoc/>
    public override void WriteByte(byte value) => throw new NotSupportedException();

    /// <inheritdoc/>
    public override int GetHashCode() => this.stream.GetHashCode();

    /// <inheritdoc/>
    public override bool Equals(object? obj) => this.stream.Equals(obj);

#if NET5_0_OR_GREATER
#pragma warning disable S1133 // Deprecated code should be removed
    /// <inheritdoc/>
    [Obsolete($"This Remoting API is not supported and throws {nameof(PlatformNotSupportedException)}.")]
    public override object InitializeLifetimeService() => this.stream.InitializeLifetimeService();
#pragma warning restore S1133 // Deprecated code should be removed
#elif NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER
    /// <inheritdoc/>
    public override object InitializeLifetimeService() => this.stream.InitializeLifetimeService();
#endif

    /// <inheritdoc/>
    public override string? ToString() => base.ToString();

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing && this.closeStream)
        {
            this.stream.Dispose();
        }

        base.Dispose(disposing);
    }

    private long ForwardPosition(long desiredPosition)
    {
        if (desiredPosition < this.position)
        {
            throw new InvalidOperationException(Properties.Resources.ForwardOnlySeekingAllowed);
        }

        while (this.position < desiredPosition)
        {
            _ = this.ReadByte();
        }

        return this.position;
    }
}