// -----------------------------------------------------------------------
// <copyright file="Index.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER

#pragma warning disable IDE0130, CheckNamespace
namespace System;
#pragma warning restore IDE0130, CheckNamespace

#pragma warning disable CA2231
/// <summary>
/// Represent a type can be used to index a collection either from the start or the end.
/// </summary>
[Diagnostics.DebuggerNonUserCode]
public readonly struct Index : IEquatable<Index>
{
    private readonly int value;

    /// <summary>
    /// Construct an Index using a value and indicating if the index is from the start or from the end.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="fromEnd">Set to <see langword="true"/> if <paramref name="value"/> is from the end.</param>
    [Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public Index(int value, bool fromEnd = false)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value));
        }

        this.value = fromEnd ? ~value : value;
    }

    private Index(int value) =>
        this.value = value;

    /// <summary>Gets an Index pointing at first element.</summary>
    public static Index Start => new(0);

    /// <summary>Gets an Index pointing at beyond last element.</summary>
    public static Index End => new(~0);

    /// <summary>Gets the index value.</summary>
    public int Value => this.value < 0 ? ~this.value : this.value;

    /// <summary>Gets a value indicating whether the index is from the start or the end.</summary>
    public bool IsFromEnd => this.value < 0;

    /// <summary>
    /// Converts integer number to an Index.
    /// </summary>
    /// <param name="value">The index value.</param>
    public static implicit operator Index(int value) => FromStart(value);

    /// <summary>
    /// Create an Index from the start at the position indicated by the value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The index from the start.</returns>
    [Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0012:Do not raise reserved exception type", Justification = "Checked")]
    [Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Checked")]
    public static Index FromStart(int value) =>
        value < 0
            ? throw new IndexOutOfRangeException(nameof(value))
            : new(value);

    /// <summary>
    /// Create an Index from the end at the position indicated by the value.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <returns>The index from the start.</returns>
    [Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    [Diagnostics.CodeAnalysis.SuppressMessage("Design", "MA0012:Do not raise reserved exception type", Justification = "Checked")]
    [Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "Checked")]
    public static Index FromEnd(int value) =>
        value < 0
            ? throw new IndexOutOfRangeException(nameof(value))
            : new(~value);

    /// <summary>
    /// Calculate the offset from the start using the giving collection length.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns>The offset.</returns>
    [Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public int GetOffset(int length)
    {
        var offset = this.value;
        if (this.IsFromEnd)
        {
            offset += length + 1;
        }

        return offset;
    }

    /// <inheritdoc />
    public override bool Equals([Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) => obj is Index index && this.value == index.value;

    /// <inheritdoc />
    public bool Equals(Index other) => this.value == other.value;

    /// <inheritdoc />
    public override int GetHashCode() => this.value;

    /// <inheritdoc />
    public override string ToString() => this.IsFromEnd ? this.ToStringFromEnd() : ((uint)this.Value).ToString(Globalization.CultureInfo.CurrentCulture);

    private string ToStringFromEnd() => '^' + this.Value.ToString(Globalization.CultureInfo.CurrentCulture);
}
#pragma warning restore CA2231
#endif