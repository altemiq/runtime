// -----------------------------------------------------------------------
// <copyright file="OutWindow.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression.LZ;

/// <summary>
/// The output window.
/// </summary>
internal sealed class OutWindow
{
    private const string ExpandableFieldName = "_expandable";
    private byte[] buffer = [];
    private int pos;
    private int startPos;
    private Stream? outputStream;
    private bool expandable;

    /// <summary>
    /// Gets the number of bytes to flush.
    /// </summary>
    public int BytesToWrite => this.pos - this.startPos;

    /// <summary>
    /// Creates the out window.
    /// </summary>
    /// <param name="windowSize">The window size.</param>
    public void Create(int windowSize)
    {
        if (this.buffer.Length != windowSize)
        {
            this.buffer = new byte[windowSize];
        }

        this.startPos = this.pos = 0;
    }

    /// <summary>
    /// Initializes the out window with the stream.
    /// </summary>
    /// <param name="stream">The output stream.</param>
    public void Init(Stream stream)
    {
        this.ReleaseStream();
        this.outputStream = stream;
        this.expandable = IsExpandable(this.outputStream);

#if NET8_0_OR_GREATER
        static bool IsExpandable(Stream stream)
        {
            return stream is not MemoryStream memoryStream || GetExpandable(memoryStream);
        }
#else
        [Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3011:Reflection should not be used to increase accessibility of classes, methods, or fields", Justification = "Checked")]
        static bool IsExpandable(Stream stream)
        {
            if (stream is MemoryStream memoryStream)
            {
                var field = memoryStream.GetType().GetField(ExpandableFieldName, Reflection.BindingFlags.Instance | Reflection.BindingFlags.NonPublic);
                return field?.GetValue(memoryStream) is not bool b || b;
            }

            return true;
        }
#endif
    }

    /// <summary>
    /// Releases the stream.
    /// </summary>
    public void ReleaseStream()
    {
        this.Flush();
        this.outputStream = null;
    }

    /// <summary>
    /// Copies the block.
    /// </summary>
    /// <param name="distance">The distance.</param>
    /// <param name="length">The length.</param>
    /// <returns>The number of bytes copied.</returns>
    public uint CopyBlock(uint distance, uint length)
    {
        var currentPosition = this.RollOverIfRequired(distance);

        for (var i = length; i > 0; i--)
        {
            if (currentPosition >= this.buffer.Length)
            {
                currentPosition = 0;
            }

            this.buffer[this.pos++] = this.buffer[currentPosition++];
            this.FlushIfRequired();
        }

        return length;
    }

    /// <summary>
    /// Writes the byte.
    /// </summary>
    /// <param name="b">The byte to write.</param>
    public void Write(byte b)
    {
        this.buffer[this.pos++] = b;
        this.FlushIfRequired();
    }

    /// <summary>
    /// Reads the byte.
    /// </summary>
    /// <param name="distance">The distance.</param>
    /// <returns>The byte that was read.</returns>
    public byte ReadByte(uint distance) => this.buffer[this.RollOverIfRequired(distance)];

#if NET8_0_OR_GREATER
    [Runtime.CompilerServices.UnsafeAccessor(Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = ExpandableFieldName)]
    private static extern ref bool GetExpandable(MemoryStream stream);
#endif

    private void FlushIfRequired()
    {
        if (this.pos >= this.buffer.Length)
        {
            this.Flush();
        }
    }

    private void Flush()
    {
        if (this.outputStream is null)
        {
            return;
        }

        var size = this.expandable
            ? this.pos - this.startPos
            : Math.Min(this.pos - this.startPos, (int)(this.outputStream.Length - this.outputStream.Position));

        if (size is 0)
        {
            return;
        }

        this.outputStream.Write(this.buffer, this.startPos, size);
        if (this.pos >= this.buffer.Length)
        {
            this.startPos = this.pos = 0;
        }
        else
        {
            this.startPos += size;
        }
    }

    private int RollOverIfRequired(uint distance)
    {
        var requiredPosition = this.pos - distance - 1;
        if (requiredPosition < 0)
        {
            requiredPosition += this.buffer.Length;
        }

        return (int)requiredPosition;
    }
}