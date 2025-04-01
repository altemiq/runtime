// -----------------------------------------------------------------------
// <copyright file="DeltaZigzagEncoding.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Delta+Zigzag Encoding.
/// </summary>
internal static class DeltaZigzagEncoding
{
    /// <summary>
    /// The context.
    /// </summary>
    /// <param name="contextValue">The context value.</param>
    public abstract class Context(int contextValue)
    {
        /// <summary>
        /// Gets or sets the context value.
        /// </summary>
        public int ContextValue { get; set; } = contextValue;
    }

    /// <summary>
    /// The encoder.
    /// </summary>
    /// <param name="contextValue">The context value.</param>
    public sealed class Encoder(int contextValue) : Context(contextValue)
    {
        /// <summary>
        /// Encodes the value.
        /// </summary>
        /// <param name="value">The value to encode.</param>
        /// <returns>The encoded value.</returns>
        public int Encode(int value)
        {
            var n = value - this.ContextValue;
            this.ContextValue = value;
            return (n << 1) ^ (n >> 31);
        }

        /// <summary>
        /// Encodes the array.
        /// </summary>
        /// <param name="source">The array to encode.</param>
        /// <param name="destination">The destination array.</param>
        public void Encode(ReadOnlySpan<int> source, Span<int> destination)
        {
            for (var i = 0; i < source.Length; i++)
            {
                destination[i] = this.Encode(source[i]);
            }
        }
    }

    /// <summary>
    /// The decoder.
    /// </summary>
    /// <param name="contextValue">The context value.</param>
    public sealed class Decoder(int contextValue) : Context(contextValue)
    {
        /// <summary>
        /// Decodes the value.
        /// </summary>
        /// <param name="value">The value to decode.</param>
        /// <returns>The decoded value.</returns>
        public int Decode(int value)
        {
            var n = value >>> 1 ^ (value << 31 >> 31);
            n += this.ContextValue;
            this.ContextValue = n;
            return n;
        }

        /// <summary>
        /// Decodes the array.
        /// </summary>
        /// <param name="source">The array to decode.</param>
        /// <param name="destination">The destination array.</param>
        public void Decode(ReadOnlySpan<int> source, Span<int> destination)
        {
            for (var i = 0; i < source.Length; i++)
            {
                destination[i] = this.Decode(source[i]);
            }
        }
    }
}