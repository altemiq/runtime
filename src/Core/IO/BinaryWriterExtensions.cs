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
    private static readonly FieldInfo EncodingFieldInfo = typeof(BinaryWriter).GetTypeInfo().DeclaredFields.Single(f => f.FieldType == typeof(System.Text.Encoding));

    /// <inheritdoc cref="BinaryWriter.Write(byte)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static void Write(this BinaryWriter writer, byte value, ByteOrder byteOrder) => writer.Write(value);

    /// <inheritdoc cref="BinaryWriter.Write(byte[])" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This parameter is not used.")]
    public static void Write(this BinaryWriter writer, byte[] buffer, ByteOrder byteOrder) => writer.Write(buffer);

    /// <inheritdoc cref="BinaryWriter.Write(char)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, char value, ByteOrder byteOrder) => Write(writer, value, Cast<System.Text.Encoding>(EncodingFieldInfo.GetValue(writer)), byteOrder);

    /// <inheritdoc cref="BinaryWriter.Write(char)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static void Write(this BinaryWriter writer, char value, System.Text.Encoding encoding, ByteOrder byteOrder)
    {
        if (encoding.GetByteCount([value]) <= 1)
        {
            writer.Write(value);
        }
        else
        {
            writer.Write(ReverseEndiannessIfRequired(value, byteOrder, ReverseEndianness));
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
        if (byteOrder is not ByteOrder.LittleEndian)
        {
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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter", Justification = "This parameter is not used.")]
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

#if NET5_0_OR_GREATER
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
    private static T ReverseEndiannessIfRequired<T>(T value, ByteOrder byteOrder, Func<T, T> reverseEndianessFunction) => byteOrder is not ByteOrder.LittleEndian ? reverseEndianessFunction(value) : value;

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static char ReverseEndianness(char value) => (char)Buffers.Binary.BinaryPrimitives.ReverseEndianness(unchecked((short)value));

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static float ReverseEndianness(float value) => BitConverter.Int32BitsToSingle(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.SingleToInt32Bits(value)));

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static double ReverseEndianness(double value) => BitConverter.Int64BitsToDouble(Buffers.Binary.BinaryPrimitives.ReverseEndianness(BitConverter.DoubleToInt64Bits(value)));

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static bool IsBigEndian(ByteOrder byteOrder) => byteOrder is not ByteOrder.LittleEndian;
#endif

    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    private static T Cast<T>(object? value)
        where T : class => value as T ?? throw new ArgumentNullException(nameof(value));
}