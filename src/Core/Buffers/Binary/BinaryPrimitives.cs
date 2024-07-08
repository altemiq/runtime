// -----------------------------------------------------------------------
// <copyright file="BinaryPrimitives.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Binary;

/// <summary>
/// Extensions for the binary primitives class.
/// </summary>
public static class BinaryPrimitives
{
#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="ulong" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ReverseEndianness(ulong value)
        => (value >> 56)
            | ((value & 0xFF000000000000) >> 40)
            | ((value & 0xFF0000000000) >> 24)
            | ((value & 0xFF00000000) >> 8)
            | ((value & 0xFF000000) << 8)
            | ((value & 0xFF0000) << 24)
            | ((value & 0xFF00) << 40)
            | (value << 56);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(ulong)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ulong ReverseEndianness(ulong value)
        => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="uint" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ReverseEndianness(uint value) => (value >> 24)
            | ((value & 0xFF0000) >> 8)
            | ((value & 0xFF00) << 8)
            | (value << 24);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(uint)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static uint ReverseEndianness(uint value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="ushort" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ReverseEndianness(ushort value) => (ushort)((value >> 8) + (value << 8));
#else

    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(ushort)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static ushort ReverseEndianness(ushort value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="sbyte"/> value, which effectively does nothing for a <see cref="sbyte"/>.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The passed-in value, unmodified.</returns>
    /// <remarks>This method effectively does nothing and was added only for consistency.</remarks>
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static sbyte ReverseEndianness(sbyte value) => value;
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(sbyte)" />
    [CLSCompliant(false)]
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static sbyte ReverseEndianness(sbyte value) => value;
#endif

#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="short" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ReverseEndianness(short value) => (short)ReverseEndianness((ushort)value);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(short)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static short ReverseEndianness(short value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="int" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ReverseEndianness(int value) => (int)ReverseEndianness((uint)value);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(int)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static int ReverseEndianness(int value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif

#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="byte"/> value, which effectively does nothing for a <see cref="byte"/>.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The passed-in value, unmodified.</returns>
    /// <remarks>This method effectively does nothing and was added only for consistency.</remarks>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte ReverseEndianness(byte value) => value;
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(byte)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static byte ReverseEndianness(byte value) => value;
#endif

#if NETSTANDARD1_0
    /// <summary>
    /// Reverses a primitive value by performing an endianness swap of the specified <see cref="long" /> value.
    /// </summary>
    /// <param name="value">The value to reverse.</param>
    /// <returns>The reversed value.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ReverseEndianness(long value) => (long)ReverseEndianness((ulong)value);
#else
    /// <inheritdoc cref="System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(long)" />
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static long ReverseEndianness(long value) => System.Buffers.Binary.BinaryPrimitives.ReverseEndianness(value);
#endif
}