// -----------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

#pragma warning disable RCS1263, SA1101, S2325

/// <summary>
/// <see cref="BinaryWriter"/> extensions.
/// </summary>
public static class BinaryWriterExtensions
{
    extension(BinaryWriter writer)
    {
#if NET5_0
        /// <summary>
        /// Writes a two-byte floating-point value to the current stream and advances the stream position by two bytes.
        /// </summary>
        /// <param name="value">The two-byte floating-point value to write.</param>
        public void Write(Half value) => writer.Write(System.BitConverter.HalfToInt16Bits(value));
#endif

        /// <inheritdoc cref="BinaryWriter.Write(byte)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public void Write(byte value, ByteOrder byteOrder) => writer.Write(value);

        /// <inheritdoc cref="BinaryWriter.Write(byte[])" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public void Write(byte[] buffer, ByteOrder byteOrder) => writer.Write(buffer);

        /// <inheritdoc cref="BinaryWriter.Write(char)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(char value, ByteOrder byteOrder) => writer.Write(value, Accessor.GetEncoding(writer), byteOrder);

        /// <inheritdoc cref="BinaryWriter.Write(char)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(char value, System.Text.Encoding encoding, ByteOrder byteOrder)
        {
            writer.Write(GetValue(value, encoding, byteOrder));

            static char GetValue(char value, System.Text.Encoding encoding, ByteOrder byteOrder)
            {
                return encoding.GetByteCount([value]) <= 1
                    ? value
                    : BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness);
            }
        }

        /// <inheritdoc cref="BinaryWriter.Write(char[])" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(char[] buffer, ByteOrder byteOrder) => writer.Write(buffer, Accessor.GetEncoding(writer), byteOrder);

        /// <inheritdoc cref="BinaryWriter.Write(char[])" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(char[] buffer, System.Text.Encoding encoding, ByteOrder byteOrder)
        {
            // we must not mutate the original array, so we need to copy it
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                Span<char> copy = stackalloc char[buffer.Length];
                buffer.CopyTo(copy);

                for (var i = 0; i < copy.Length; i++)
                {
                    var c = copy[i];
                    if (encoding.GetByteCount([c]) > 1)
                    {
                        copy[i] = BinaryWriter.ReverseEndianness(c);
                    }
                }

                writer.Write(copy);
                return;
#else
                var copy = new char[buffer.Length];
                buffer.CopyTo(copy, 0);

                for (var i = 0; i < copy.Length; i++)
                {
                    var c = copy[i];
                    if (encoding.GetByteCount([c]) > 1)
                    {
                        copy[i] = BinaryWriter.ReverseEndianness(c);
                    }
                }

                buffer = copy;
#endif
            }

            writer.Write(buffer);
        }

        /// <inheritdoc cref="BinaryWriter.Write(short)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(short value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(short)];
                System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(buffer, value);
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

        /// <inheritdoc cref="BinaryWriter.Write(int)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(int value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(int)];
                System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(buffer, value);
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

        /// <inheritdoc cref="BinaryWriter.Write(long)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(long value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(long)];
                System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(buffer, value);
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

        /// <inheritdoc cref="BinaryWriter.Write(sbyte)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
        public void Write(sbyte value, ByteOrder byteOrder) => writer.Write(value);

        /// <inheritdoc cref="BinaryWriter.Write(ushort)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(ushort value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(ushort)];
                System.Buffers.Binary.BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

        /// <inheritdoc cref="BinaryWriter.Write(uint)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(uint value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(uint)];
                System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

        /// <inheritdoc cref="BinaryWriter.Write(ulong)" />
        [CLSCompliant(false)]
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(ulong value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(ulong)];
                System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="BinaryWriter.Write(Half)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public void Write(Half value, ByteOrder byteOrder)
    {
        if (BinaryWriter.IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(short)];
            System.Buffers.Binary.BinaryPrimitives.WriteHalfBigEndian(buffer, value);
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#elif NET5_0_OR_GREATER
        /// <inheritdoc cref="Write(BinaryWriter, Half)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(Half value, ByteOrder byteOrder)
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(short)];
                System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(buffer, System.BitConverter.HalfToInt16Bits(value));
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#endif

        /// <inheritdoc cref="BinaryWriter.Write(float)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(float value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(float)];
#if NET5_0_OR_GREATER
                System.Buffers.Binary.BinaryPrimitives.WriteSingleBigEndian(buffer, value);
#else
                System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(buffer, BitConverter.SingleToInt32Bits(value));
#endif
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

        /// <inheritdoc cref="BinaryWriter.Write(double)" />
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public void Write(double value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        {
            if (BinaryWriter.IsBigEndian(byteOrder))
            {
                Span<byte> buffer = stackalloc byte[sizeof(double)];
#if NET5_0_OR_GREATER
                System.Buffers.Binary.BinaryPrimitives.WriteDoubleBigEndian(buffer, value);
#else
                System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(buffer, BitConverter.DoubleToInt64Bits(value));
#endif
                writer.BaseStream.Write(buffer);
                return;
            }

            writer.Write(value);
        }
#else
            => writer.Write(BinaryWriter.ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static T ReverseEndiannessIfRequired<T>(T value, ByteOrder byteOrder, Func<T, T> reverseEndiannessFunction) => BinaryWriter.IsBigEndian(byteOrder) ? reverseEndiannessFunction(value) : value;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static char ReverseEndianness(char value) => (char)Buffers.Binary.BinaryPrimitives.ReverseEndianness(unchecked((short)value));

#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_1_OR_GREATER
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static float ReverseEndianness(float value) => BitConverter.Int32BitsToSingle(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value)));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static double ReverseEndianness(double value) => BitConverter.Int64BitsToDouble(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));
#endif

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static bool IsBigEndian(ByteOrder byteOrder) => byteOrder is not ByteOrder.LittleEndian;
    }

    private static class Accessor
    {
#if NET8_0_OR_GREATER
        [System.Runtime.CompilerServices.UnsafeAccessor(System.Runtime.CompilerServices.UnsafeAccessorKind.Field, Name = "_encoding")]
        public static extern ref System.Text.Encoding GetEncoding(BinaryWriter writer);
#else
#pragma warning disable MA0169
        private static readonly System.Reflection.FieldInfo EncodingFieldInfo = System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(BinaryWriter)).DeclaredFields.Single(static f => f.FieldType == typeof(System.Text.Encoding));
#pragma warning restore MA0169

        public static System.Text.Encoding GetEncoding(BinaryWriter writer) => Cast<System.Text.Encoding>(EncodingFieldInfo.GetValue(writer));

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        private static T Cast<T>(object? value)
            where T : class => value as T ?? throw new ArgumentNullException(nameof(value));
#endif
    }
}