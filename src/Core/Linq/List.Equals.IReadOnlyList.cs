// -----------------------------------------------------------------------
// <copyright file="List.Equals.IReadOnlyList.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

/// <content>
/// <see cref="IReadOnlyList{T}"/> extensions to test equality.
/// </content>
public static partial class List
{
    /// <summary>
    /// Determines if the specified lists are equal in the specific locations in each list, and for the specified length.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="first">The first list.</param>
    /// <param name="firstIndex">The first index.</param>
    /// <param name="second">The second list.</param>
    /// <param name="secondIndex">The second index.</param>
    /// <param name="count">The number of bytes to compare.</param>
    /// <returns><see langword="true"/> if the sections of each list are equal; otherwise <see langword="false"/>.</returns>
    public static bool Equals<T>(this IReadOnlyList<T> first, int firstIndex, IReadOnlyList<T> second, int secondIndex, int count)
        where T : IEquatable<T>
    {
        return (first, second) switch
        {
            (null, not null) or (not null, null) => false,
            (null, null) => true,
            (not null, not null) => EqualsCore(first, firstIndex, second, secondIndex, count),
        };

        static bool EqualsCore(IReadOnlyList<T> first, int firstIndex, IReadOnlyList<T> second, int secondIndex, int count)
        {
            for (int i = firstIndex, j = secondIndex; i < firstIndex + count && j < secondIndex + count; i++, j++)
            {
                if (!first[i].Equals(second[j]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <summary>
    /// Determines if the specified lists are equal in the specific locations in each list, and for the specified length.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="first">The first list.</param>
    /// <param name="firstIndex">The first index.</param>
    /// <param name="second">The second list.</param>
    /// <param name="secondIndex">The second index.</param>
    /// <param name="count">The number of bytes to compare.</param>
    /// <returns><see langword="true"/> if the sections of each list are equal; otherwise <see langword="false"/>.</returns>
    public static bool Equals<T>(this IReadOnlyList<T> first, int firstIndex, IList<T> second, int secondIndex, int count)
        where T : IEquatable<T>
    {
        return (first, second) switch
        {
            (null, not null) or (not null, null) => false,
            (null, null) => true,
            (not null, not null) => EqualsCore(first, firstIndex, second, secondIndex, count),
        };

        static bool EqualsCore(IReadOnlyList<T> first, int firstIndex, IList<T> second, int secondIndex, int count)
        {
            for (int i = firstIndex, j = secondIndex; i < firstIndex + count && j < secondIndex + count; i++, j++)
            {
                if (!first[i].Equals(second[j]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}