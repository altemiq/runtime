// -----------------------------------------------------------------------
// <copyright file="List.IndexOf.IList.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

/// <content>
/// <see cref="IList{T}"/> extensions for getting the index.
/// </content>
public static partial class List
{
    /// <summary>
    /// Reports the zero-based index of the first occurrence of the specified value in the specified list.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="buffer">The list to search through.</param>
    /// <param name="value">The value to seek.</param>
    /// <returns>The zero-based index position of <paramref name="value"/> from the start of <paramref name="buffer"/> if found, or -1 if it is not. If <paramref name="value"/> is an empty list, the return value is 0.</returns>
    public static int IndexOf<T>(this IList<T> buffer, IList<T> value)
        where T : IEquatable<T> => buffer is not null ? IndexOf(buffer, value, 0) : -1;

    /// <summary>
    /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="buffer">The list to search through.</param>
    /// <param name="value">The value to seek.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <returns>The zero-based index position of <paramref name="value"/> from the start of <paramref name="buffer"/> if found, or -1 if it is not. If <paramref name="value"/> is an empty list, the return value is <paramref name="startIndex"/>.</returns>
    public static int IndexOf<T>(this IList<T> buffer, IList<T> value, int startIndex)
        where T : IEquatable<T> => buffer is not null && buffer.Count > startIndex ? IndexOf(buffer, value, startIndex, buffer.Count - startIndex) : -1;

    /// <summary>
    /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position and examines a specified number of values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="buffer">The list to search through.</param>
    /// <param name="value">The list to seek.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <param name="count">The number of values to examine.</param>
    /// <returns>The zero-based index position of <paramref name="value"/> from the start of <paramref name="buffer"/> if found, or -1 if it is not. If <paramref name="value"/> is an empty list, the return value is <paramref name="startIndex"/>.</returns>
    public static int IndexOf<T>(this IList<T> buffer, IList<T> value, int startIndex, int count)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer is null)
        {
            return -1;
        }

        // If we've got no values to test with
        if (value is null || value.Count == 0)
        {
            return startIndex;
        }

        var j = 0;

        // Search until we get to the start or end value
        var length = Math.Min(buffer.Count, startIndex + count);
        for (var i = startIndex; i < length; i++)
        {
            if (buffer[i].Equals(value[j]))
            {
                j++;

                if (j == value.Count)
                {
                    // We have the start
                    return i - value.Count + 1;
                }
            }
            else
            {
                j = 0;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the zero-based index of the first occurrence of the specified value in the specified list.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="buffer">The list to search through.</param>
    /// <param name="value">The value to seek.</param>
    /// <returns>The zero-based index position of <paramref name="value"/> from the start of <paramref name="buffer"/> if found, or -1 if it is not.</returns>
    public static int IndexOf<T>(this IList<T> buffer, T value)
        where T : IEquatable<T> => buffer is not null ? IndexOf(buffer, value, 0) : -1;

    /// <summary>
    /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="buffer">The list to search through.</param>
    /// <param name="value">The value to seek.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <returns>The zero-based index position of <paramref name="value"/> from the start of <paramref name="buffer"/> if found, or -1 if it is not.</returns>
    public static int IndexOf<T>(this IList<T> buffer, T value, int startIndex)
        where T : IEquatable<T> => buffer is not null && buffer.Count > startIndex ? IndexOf(buffer, value, startIndex, buffer.Count - startIndex) : -1;

    /// <summary>
    /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position and examines a specified number values.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="buffer">The list to search through.</param>
    /// <param name="value">The value to seek.</param>
    /// <param name="startIndex">The search starting position.</param>
    /// <param name="count">The number of values to examine.</param>
    /// <returns>The zero-based index position of <paramref name="value"/> from the start of <paramref name="buffer"/> if found, or -1 if it is not.</returns>
    public static int IndexOf<T>(this IList<T> buffer, T value, int startIndex, int count)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer is null)
        {
            return -1;
        }

        // Search until we get to the value
        var length = Math.Min(buffer.Count, startIndex + count);
        for (var i = startIndex; i < length; i++)
        {
            if (buffer[i].Equals(value))
            {
                // We have the start
                return i;
            }
        }

        return -1;
    }
}