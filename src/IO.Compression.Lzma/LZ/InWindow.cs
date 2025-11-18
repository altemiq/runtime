// -----------------------------------------------------------------------
// <copyright file="InWindow.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression.LZ;

/// <summary>
/// The input window.
/// </summary>
internal class InWindow : IInWindowStream
{
    // size of allocated memory block.
    private uint blockSize;

    private Stream? input;

    // offset (from _buffer) of first byte when new block reading must be done
    private uint posLimit;

    // if (true) then _streamPos shows real end of stream
    private bool streamEndWasReached;

    private uint pointerToLastSafePosition;

    // how many BYTEs must be kept in buffer before _pos
    private uint keepSizeBefore;

    // how many BYTEs must be kept buffer after _pos
    private uint keepSizeAfter;

    /// <inheritdoc/>
    public uint AvailableBytes => this.StreamPosition - this.Position;

    /// <summary>
    /// Gets ths pointer to buffer with data.
    /// </summary>
    protected byte[] Buffer { get; private set; } = [];

    /// <summary>
    /// Gets the buffer offset.
    /// </summary>
    protected uint BufferOffset { get; private set; }

    /// <summary>
    /// Gets the offset (from _buffer) of current byte.
    /// </summary>
    protected uint Position { get; private set; }

    /// <summary>
    /// Gets the offset (from _buffer) of first not read byte from Stream.
    /// </summary>
    protected uint StreamPosition { get; private set; }

    /// <inheritdoc />
    public void ReleaseStream() => this.input = null;

    /// <inheritdoc />
    public virtual void Init(Stream stream)
    {
        this.input = stream;
        this.BufferOffset = 0;
        this.Position = 0;
        this.StreamPosition = 0;
        this.streamEndWasReached = false;
        this.ReadBlock();
    }

    /// <inheritdoc/>
    public byte GetIndexByte(int index) => this.Buffer[this.BufferOffset + this.Position + index];

    /// <inheritdoc/>
    public uint GetMatchLength(int index, uint distance, uint limit)
    {
        if (this.streamEndWasReached
            && this.Position + index + limit > this.StreamPosition)
        {
            limit = this.StreamPosition - (uint)(this.Position + index);
        }

        distance++;

        var pby = this.BufferOffset + this.Position + (uint)index;

        uint i;
        for (i = 0U; i < limit && this.Buffer[pby + i] == this.Buffer[pby + i - distance]; i++)
        {
            // this is fine.
        }

        return i;
    }

    /// <summary>
    /// Moves the position.
    /// </summary>
    protected virtual void MovePosition()
    {
        this.Position++;
        if (this.Position <= this.posLimit)
        {
            return;
        }

        var pointerToPosition = this.BufferOffset + this.Position;
        if (pointerToPosition > this.pointerToLastSafePosition)
        {
            this.MoveBlock();
        }

        this.ReadBlock();
    }

    /// <summary>
    /// REduce the offsets.
    /// </summary>
    /// <param name="subValue">The sub-value.</param>
    protected void ReduceOffsets(int subValue)
    {
        this.BufferOffset += (uint)subValue;
        this.posLimit -= (uint)subValue;
        this.Position -= (uint)subValue;
        this.StreamPosition -= (uint)subValue;
    }

    /// <summary>
    /// Creates this instance.
    /// </summary>
    /// <param name="sizeBefore">The keep size before.</param>
    /// <param name="sizeAfter">The keep size after.</param>
    /// <param name="sizeReserve">The keep size reserve.</param>
    protected void Create(uint sizeBefore, uint sizeAfter, uint sizeReserve)
    {
        this.keepSizeBefore = sizeBefore;
        this.keepSizeAfter = sizeAfter;
        var totalBlockSize = sizeBefore + sizeAfter + sizeReserve;
        if (this.blockSize != totalBlockSize)
        {
            this.blockSize = totalBlockSize;
            this.Buffer = new byte[this.blockSize];
        }

        this.pointerToLastSafePosition = this.blockSize - sizeAfter;
    }

    private void MoveBlock()
    {
        var offset = this.BufferOffset + this.Position - this.keepSizeBefore;

        // we need one additional byte, since MovePos moves on 1 byte.
        if (offset > 0)
        {
            offset--;
        }

        var numBytes = this.BufferOffset + this.StreamPosition - offset;

        // check negative offset ????
        for (var i = 0U; i < numBytes; i++)
        {
            this.Buffer[i] = this.Buffer[offset + i];
        }

        this.BufferOffset -= offset;
    }

    private void ReadBlock()
    {
        if (this.streamEndWasReached)
        {
            return;
        }

        if (this.input is null)
        {
            throw new InvalidOperationException();
        }

        while (true)
        {
            var size = (int)(0 - this.BufferOffset + this.blockSize - this.StreamPosition);
            if (size is 0)
            {
                return;
            }

            var bytesRead = this.input.Read(this.Buffer, (int)(this.BufferOffset + this.StreamPosition), size);
            if (bytesRead is 0)
            {
                this.posLimit = this.StreamPosition;
                if ((this.BufferOffset + this.posLimit) > this.pointerToLastSafePosition)
                {
                    this.posLimit = this.pointerToLastSafePosition - this.BufferOffset;
                }

                this.streamEndWasReached = true;
                return;
            }

            this.StreamPosition += (uint)bytesRead;
            if (this.StreamPosition >= this.Position + this.keepSizeAfter)
            {
                this.posLimit = this.StreamPosition - this.keepSizeAfter;
            }
        }
    }
}