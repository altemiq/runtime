// -----------------------------------------------------------------------
// <copyright file="BinaryWriterExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

using System.Reflection;

/// <summary>
/// <see cref="BinaryWriter"/> extensions.
/// </summary>
public static class BinaryWriterExtensions
{
    private static readonly FieldInfo EncodingFieldInfo = typeof(BinaryWriter).GetTypeInfo().DeclaredFields.Single(static f => f.FieldType.Equals(typeof(System.Text.Encoding)));

#if NET5_0
    /// <summary>
    /// Writes a two-byte floating-point value to the current stream and advances the stream position by two bytes.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="value">The two-byte floating-point value to write.</param>
    public static void Write(this BinaryWriter writer, Half value) => writer.Write(BitConverter.HalfToInt16Bits(value));
#endif

    /// <inheritdoc cref="BinaryWriter.Write(byte)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static void Write(this BinaryWriter writer, byte value, ByteOrder byteOrder) => writer.Write(value);

    /// <inheritdoc cref="BinaryWriter.Write(byte[])" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static void Write(this BinaryWriter writer, byte[] buffer, ByteOrder byteOrder) => writer.Write(buffer);

    /// <inheritdoc cref="BinaryWriter.Write(char)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, char value, ByteOrder byteOrder) => Write(writer, value, Cast<System.Text.Encoding>(EncodingFieldInfo.GetValue(writer)), byteOrder);

    /// <inheritdoc cref="BinaryWriter.Write(char)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, char value, System.Text.Encoding encoding, ByteOrder byteOrder)
    {
        writer.Write(GetValue(value, encoding, byteOrder));

        static char GetValue(char value, System.Text.Encoding encoding, ByteOrder byteOrder)
        {
            return encoding.GetByteCount([value]) <= 1
                ? value
                : ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness);
        }
    }

    /// <inheritdoc cref="BinaryWriter.Write(char[])" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, char[] buffer, ByteOrder byteOrder) => Write(writer, buffer, Cast<System.Text.Encoding>(EncodingFieldInfo.GetValue(writer)), byteOrder);

    /// <inheritdoc cref="BinaryWriter.Write(char[])" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, char[] buffer, System.Text.Encoding encoding, ByteOrder byteOrder)
    {
        // we must not mutate the original array, so we need to copy it
        if (IsBigEndian(byteOrder))
        {
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            Span<char> copy = stackalloc char[buffer.Length];
            buffer.CopyTo(copy);

            for (var i = 0; i < copy.Length; i++)
            {
                var c = copy[i];
                if (encoding.GetByteCount([c]) > 1)
                {
                    copy[i] = ReverseEndianness(c);
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
                    copy[i] = ReverseEndianness(c);
                }
            }

            buffer = copy;
#endif
        }

        writer.Write(buffer);
    }

    /// <inheritdoc cref="BinaryWriter.Write(short)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, short value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(short)];
            System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(buffer, value);
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#else
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

    /// <inheritdoc cref="BinaryWriter.Write(int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, int value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(int)];
            System.Buffers.Binary.BinaryPrimitives.WriteInt32BigEndian(buffer, value);
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#else
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

    /// <inheritdoc cref="BinaryWriter.Write(long)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, long value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(long)];
            System.Buffers.Binary.BinaryPrimitives.WriteInt64BigEndian(buffer, value);
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#else
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

    /// <inheritdoc cref="BinaryWriter.Write(sbyte)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static void Write(this BinaryWriter writer, sbyte value, ByteOrder byteOrder) => writer.Write(value);

    /// <inheritdoc cref="BinaryWriter.Write(ushort)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, ushort value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(ushort)];
            System.Buffers.Binary.BinaryPrimitives.WriteUInt16BigEndian(buffer, value);
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#else
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

    /// <inheritdoc cref="BinaryWriter.Write(uint)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, uint value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(uint)];
            System.Buffers.Binary.BinaryPrimitives.WriteUInt32BigEndian(buffer, value);
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#else
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

    /// <inheritdoc cref="BinaryWriter.Write(ulong)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, ulong value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(ulong)];
            System.Buffers.Binary.BinaryPrimitives.WriteUInt64BigEndian(buffer, value);
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#else
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, Buffers.Binary.BinaryPrimitives.ReverseEndianness));
#endif

#if NET6_0_OR_GREATER
    /// <inheritdoc cref="BinaryWriter.Write(Half)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, Half value, ByteOrder byteOrder)
    {
        if (IsBigEndian(byteOrder))
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
    public static void Write(this BinaryWriter writer, Half value, ByteOrder byteOrder)
    {
        if (IsBigEndian(byteOrder))
        {
            Span<byte> buffer = stackalloc byte[sizeof(short)];
            System.Buffers.Binary.BinaryPrimitives.WriteInt16BigEndian(buffer, BitConverter.HalfToInt16Bits(value));
            writer.BaseStream.Write(buffer);
            return;
        }

        writer.Write(value);
    }
#endif

    /// <inheritdoc cref="BinaryWriter.Write(float)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, float value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
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
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

    /// <inheritdoc cref="BinaryWriter.Write(double)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, double value, ByteOrder byteOrder)
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    {
        if (IsBigEndian(byteOrder))
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
        => writer.Write(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
#endif

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static T ReverseEndiannessIfRequired<T>(T value, ByteOrder byteOrder, Func<T, T> reverseEndiannessFunction) => IsBigEndian(byteOrder) ? reverseEndiannessFunction(value) : value;

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

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static T Cast<T>(object? value)
        where T : class => value as T ?? throw new ArgumentNullException(nameof(value));
}