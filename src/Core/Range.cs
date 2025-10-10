// -----------------------------------------------------------------------
// <copyright file="Range.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETCOREAPP3_0_OR_GREATER && !NETSTANDARD2_1_OR_GREATER
#pragma warning disable IDE0130, CheckNamespace
namespace System;
#pragma warning restore IDE0130, CheckNamespace

#pragma warning disable CA2231, CS0659, MA0008, MA0084, S1206
/// <summary>Represent a range has start and end indexes.</summary>
/// <param name="start">Represent the inclusive start index of the range.</param>
/// <param name="end">Represent the exclusive end index of the range.</param>
[Diagnostics.DebuggerNonUserCode]
public readonly struct Range(Index start, Index end) : IEquatable<Range>
{
    /// <summary>
    /// Gets a Range object starting from first element to the end.
    /// </summary>
    public static Range All => new(Index.Start, Index.End);

    /// <summary>Gets the inclusive start index of the Range.</summary>
    public Index Start { get; } = start;

    /// <summary>Gets the exclusive end index of the Range.</summary>
    public Index End { get; } = end;

    /// <summary>
    /// Create a Range object starting from start index to the end of the collection.
    /// </summary>
    /// <param name="start">The start.</param>
    /// <returns>The range.</returns>
    public static Range StartAt(Index start) => new(start, Index.End);

    /// <summary>
    /// Create a Range object starting from first element in the collection to the end Index.
    /// </summary>
    /// <param name="end">The end.</param>
    /// <returns>The range.</returns>
    public static Range EndAt(Index end) => new(Index.Start, end);

    /// <inheritdoc/>
    public override bool Equals([Diagnostics.CodeAnalysis.NotNullWhen(true)] object? obj) =>
        obj is Range r &&
        r.Start.Equals(this.Start) &&
        r.End.Equals(this.End);

    /// <inheritdoc/>
    public bool Equals(Range other) => other.Start.Equals(this.Start) && other.End.Equals(this.End);

    /// <inheritdoc />
    public override string ToString() => $"{this.Start}..{this.End}";

    /// <summary>
    /// Calculate the start offset and length of range object using a collection length.
    /// </summary>
    /// <param name="length">The length.</param>
    /// <returns>The offset and length.</returns>
    [Runtime.CompilerServices.MethodImpl(Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public (int Offset, int Length) GetOffsetAndLength(int length)
    {
        var startIndex = this.Start;
        var start = startIndex.IsFromEnd ? length - startIndex.Value : startIndex.Value;

        var endIndex = this.End;
        var end = endIndex.IsFromEnd ? length - endIndex.Value : endIndex.Value;

        return (uint)end > (uint)length || (uint)start > (uint)end
            ? throw new ArgumentOutOfRangeException(nameof(length))
            : (start, end - start);
    }
}
#pragma warning restore CA2231, CS0659, MA0008, S1206
#endif