// -----------------------------------------------------------------------
// <copyright file="EchoStream.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altavec.IO;

/// <summary>
/// A stream that echos writes back to read.
/// </summary>
public class EchoStream : Stream
{
    private readonly object lockObject = new();

    // Default underlying mechanism for BlockingCollection is ConcurrentQueue<T>, which is what we want
    private readonly System.Collections.Concurrent.BlockingCollection<byte[]> buffers;

    private byte[]? currentBuffer;
    private int offset;
    private int count;

    private bool closed;

    // after the stream is closed, set to true after returning a 0 for read()
    private bool finalZero;

    private long length;
    private long position;

    /// <summary>
    /// Initialises a new instance of the <see cref="EchoStream"/> class.
    /// </summary>
    public EchoStream() => this.buffers = [];

    /// <summary>
    /// Initialises a new instance of the <see cref="EchoStream"/> class with the specified upper-bound.
    /// </summary>
    /// <param name="boundedCapacity">The bounded size of the collection.</param>
    public EchoStream(int boundedCapacity) => this.buffers = new(boundedCapacity);

    /// <inheritdoc/>
    public override bool CanTimeout { get; } = true;

    /// <inheritdoc/>
    public override int ReadTimeout { get; set; } = Timeout.Infinite;

    /// <inheritdoc/>
    public override int WriteTimeout { get; set; } = Timeout.Infinite;

    /// <inheritdoc/>
    public override bool CanRead { get; } = true;

    /// <inheritdoc/>
    public override bool CanSeek { get; }

    /// <inheritdoc/>
    public override bool CanWrite { get; } = true;

    /// <inheritdoc/>
    public override long Length => this.length;

    /// <inheritdoc/>
    public override long Position
    {
        get => this.position;
        set => throw new NotSupportedException();
    }

    /// <summary>
    /// Gets a value indicating whether there is data available.
    /// </summary>
    public bool DataAvailable => this.buffers.Count > 0;

    /// <summary>
    /// Gets or sets a value indicating whether to copy the buffer on write.
    /// </summary>
    public bool CopyBufferOnWrite { get; set; }

#if NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_3
    /// <summary>
    /// Closes the current stream and releases any resources (such as sockets and file handles) associated with the current stream. Instead of calling this method, ensure that the stream is properly disposed.
    /// </summary>
    public void Close()
#else
    /// <inheritdoc/>
    public override void Close()
#endif
    {
        this.closed = true;

        // release any waiting writes
        this.buffers.CompleteAdding();
    }

    /// <inheritdoc/>
    /// <remarks>We override the xxxxAsync functions because the default base class shares state between ReadAsync and WriteAsync, which causes a hang if both are called at once.</remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0042:Do not use blocking calls in an async method", Justification = "This would cause recursion")]
    public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => Task.Run(() => this.Write(buffer, offset, count), cancellationToken);

#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    /// <remarks>We override the xxxxAsync functions because the default base class shares state between ReadAsync and WriteAsync, which causes a hang if both are called at once.</remarks>
    public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default) => new(Task.Run(() => this.Write(buffer.Span), cancellationToken));
#endif

    /// <inheritdoc/>
    /// <remarks>We override the xxxxAsync functions because the default base class shares state between ReadAsync and WriteAsync, which causes a hang if both are called at once.</remarks>
    public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken) => Task.Run(() => this.Read(buffer, offset, count), cancellationToken);

#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    /// <remarks>We override the xxxxAsync functions because the default base class shares state between ReadAsync and WriteAsync, which causes a hang if both are called at once.</remarks>
    public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default) => new(Task.Run(() => this.Read(buffer.Span), cancellationToken));
#endif

    /// <inheritdoc/>
    public override void Write(byte[] buffer, int offset, int count)
    {
        if (this.closed || buffer.Length - offset < count || count <= 0)
        {
            return;
        }

        byte[] newBuffer;
        if (!this.CopyBufferOnWrite && offset is 0 && count == buffer.Length)
        {
            newBuffer = buffer;
        }
        else
        {
            newBuffer = new byte[count];
            Buffer.BlockCopy(buffer, offset, newBuffer, 0, count);
        }

        if (!this.buffers.TryAdd(newBuffer, this.WriteTimeout))
        {
            throw new TimeoutException($"{nameof(EchoStream)} {nameof(Stream.Write)}() {nameof(Timeout)}");
        }

        this.length += count;
    }

    /// <inheritdoc/>
    public override int Read(byte[] buffer, int offset, int count)
    {
        if (count is 0)
        {
            return 0;
        }

        lock (this.lockObject)
        {
            if (this.count is 0 && this.buffers.Count is 0)
            {
                if (this.closed)
                {
                    if (!this.finalZero)
                    {
                        this.finalZero = true;
                        return 0;
                    }

                    return -1;
                }

                if (this.buffers.TryTake(out this.currentBuffer, this.ReadTimeout) && this.currentBuffer is not null)
                {
                    this.offset = 0;
                    this.count = this.currentBuffer.Length;
                }
                else
                {
                    if (this.closed)
                    {
                        if (!this.finalZero)
                        {
                            this.finalZero = true;
                            return 0;
                        }

                        return -1;
                    }

                    return 0;
                }
            }

            var returnBytes = 0;
            while (count > 0)
            {
                if (this.count is 0)
                {
                    if (this.buffers.TryTake(out this.currentBuffer, this.ReadTimeout) && this.currentBuffer is not null)
                    {
                        this.offset = 0;
                        this.count = this.currentBuffer.Length;
                    }
                    else
                    {
                        break;
                    }
                }

                var bytesToCopy = Math.Min(count, this.count);
                if (this.currentBuffer is not null)
                {
                    Buffer.BlockCopy(this.currentBuffer, this.offset, buffer, offset, bytesToCopy);
                }

                this.offset += bytesToCopy;
                this.count -= bytesToCopy;
                offset += bytesToCopy;
                count -= bytesToCopy;

                returnBytes += bytesToCopy;
            }

            this.position += returnBytes;
            return returnBytes;
        }
    }

    /// <inheritdoc/>
    public override int ReadByte()
    {
        var returnValue = new byte[1];
        return this.Read(returnValue, 0, 1) <= 0 ? -1 : returnValue[0];
    }

    /// <inheritdoc/>
    public override void Flush()
    {
    }

    /// <inheritdoc/>
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();

    /// <inheritdoc/>
    public override void SetLength(long value) => throw new NotSupportedException();

    /// <inheritdoc/>
    protected override void Dispose(bool disposing)
    {
        if (disposing && !this.buffers.IsCompleted)
        {
            this.buffers.CompleteAdding();
        }

        base.Dispose(disposing);
    }
}