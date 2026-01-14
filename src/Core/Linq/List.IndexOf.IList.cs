// -----------------------------------------------------------------------
// <copyright file="List.IndexOf.IList.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

#pragma warning disable RCS1263, SA1101, S2325

/// <content>
/// <see cref="IList{T}"/> extensions for getting the index.
/// </content>
public static partial class List
{
    /// <content>
    /// <see cref="IList{T}"/> extensions for getting the index.
    /// </content>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="buffer">The list to search through.</param>
    extension<T>(IList<T>? buffer)
        where T : IEquatable<T>
    {
        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified value in the specified list.
        /// </summary>
        /// <param name="value">The value to seek.</param>
        /// <returns>The zero-based index position of <paramref name="value"/> from the start of this instance if found, or -1 if it is not. If <paramref name="value"/> is an empty list, the return value is 0.</returns>
        public int IndexOf(IList<T> value) => buffer?.IndexOf(value, 0) ?? -1;

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position.
        /// </summary>
        /// <param name="value">The value to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>The zero-based index position of <paramref name="value"/> from the start of this instance if found, or -1 if it is not. If <paramref name="value"/> is an empty list, the return value is <paramref name="startIndex"/>.</returns>
        public int IndexOf(IList<T> value, int startIndex) => buffer.IndexOf(value, startIndex, (buffer?.Count ?? 0) - startIndex);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position and examines a specified number of values.
        /// </summary>
        /// <param name="value">The list to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of values to examine.</param>
        /// <returns>The zero-based index position of <paramref name="value"/> from the start of this instance if found, or -1 if it is not. If <paramref name="value"/> is an empty list, the return value is <paramref name="startIndex"/>.</returns>
        public int IndexOf(IList<T> value, int startIndex, int count)
        {
            return (buffer, value) switch
            {
                (null, _) => -1,
                (_, null or { Count: 0 }) => startIndex,
                _ => IndexOfCore(buffer, value, startIndex, count),
            };

            static int IndexOfCore(IList<T> buffer, IList<T> value, int startIndex, int count)
            {
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
        }

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified value in the specified list.
        /// </summary>
        /// <param name="value">The value to seek.</param>
        /// <returns>The zero-based index position of <paramref name="value"/> from the start of this instance if found, or -1 if it is not.</returns>
        public int IndexOf(T value) => buffer?.IndexOf(value, 0) ?? -1;

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position.
        /// </summary>
        /// <param name="value">The value to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <returns>The zero-based index position of <paramref name="value"/> from the start of this instance if found, or -1 if it is not.</returns>
        public int IndexOf(T value, int startIndex) => buffer.IndexOf(value, startIndex, (buffer?.Count ?? 0) - startIndex);

        /// <summary>
        /// Reports the zero-based index of the first occurrence of the specified value in the specified list. The search starts at a specified position and examines a specified number values.
        /// </summary>
        /// <param name="value">The value to seek.</param>
        /// <param name="startIndex">The search starting position.</param>
        /// <param name="count">The number of values to examine.</param>
        /// <returns>The zero-based index position of <paramref name="value"/> from the start of this instance if found, or -1 if it is not.</returns>
        public int IndexOf(T value, int startIndex, int count)
        {
            return buffer is null || count < 0
                ? -1
                : IndexOfCore(buffer, value, startIndex, count);

            static int IndexOfCore(IList<T> buffer, T value, int startIndex, int count)
            {
                // Search until we get to the value
                var endIndex = Math.Min(buffer.Count, startIndex + count);
                for (var i = startIndex; i < endIndex; i++)
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
    }
}