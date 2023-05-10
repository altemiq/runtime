// -----------------------------------------------------------------------
// <copyright file="List.IndexOfAny.IList.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

/// <content>
/// <see cref="IList{T}"/> extension for getting the index of any value.
/// </content>
public static partial class List
{
    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence of bytes in the specified arguments.
    /// </summary>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="anyOf">The array of byte sequences to seek.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any byte sequence in <paramref name="anyOf"/> was found; -1 if no byte sequence in <paramref name="anyOf"/> was found.</returns>
    public static int IndexOfAny(this IList<byte> buffer, params IList<byte>[] anyOf) => IndexOfAny(buffer, 0, anyOf);

    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence of bytes in the specified arguments.
    /// The search starts at a specified byte position.
    /// </summary>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <param name="anyOf">The array of byte sequences to seek.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any byte sequence in <paramref name="anyOf"/> was found; -1 if no byte sequence in <paramref name="anyOf"/> was found.</returns>
    public static int IndexOfAny(this IList<byte> buffer, int startIndex, params IList<byte>[] anyOf) => buffer is null || buffer.Count <= startIndex
        ? -1
        : IndexOfAny(buffer, startIndex, buffer.Count - startIndex, anyOf);

    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence of bytes in the specified arguments.
    /// The search starts at a specified byte position and examines a specified number of character positions.
    /// </summary>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <param name="count">The number of bytes to examine.</param>
    /// <param name="anyOf">The array of byte sequences to seek.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any byte sequence in <paramref name="anyOf"/> was found; -1 if no byte sequence in <paramref name="anyOf"/> was found.</returns>
    public static int IndexOfAny(this IList<byte> buffer, int startIndex, int count, params IList<byte>[] anyOf)
    {
        // Check to see if the buffer is applicable
        if (buffer is null || buffer.Count <= startIndex)
        {
            return -1;
        }

        if (anyOf is null || anyOf.Length == 0)
        {
            return startIndex;
        }

        var length = Math.Min(buffer.Count, startIndex + count);
        var indexes = new int[anyOf.Length];
        for (var i = startIndex; i < length; i++)
        {
            for (var j = 0; j < anyOf.Length; j++)
            {
                if (buffer[i] == anyOf[j][indexes[j]])
                {
                    indexes[j]++;
                    if (indexes[j] == anyOf[j].Count)
                    {
                        return i - anyOf[j].Count + 1;
                    }
                }
                else
                {
                    indexes[j] = 0;
                }
            }
        }

        return -1;
    }
}