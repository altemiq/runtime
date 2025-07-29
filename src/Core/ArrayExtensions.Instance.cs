// -----------------------------------------------------------------------
// <copyright file="ArrayExtensions.Instance.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// <see cref="ArrayExtensions"/> extensions.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// Returns a new array that right-aligns the values in this array by padding them with default values on the left, for a specified total length.
    /// </summary>
    /// <typeparam name="T">The type of data in the array.</typeparam>
    /// <param name="array">The array to pad.</param>
    /// <param name="totalWidth">The number of values in the resulting array, equal to the number of original values plus any additional padding values.</param>
    /// <returns>A new array that is equivalent to <paramref name="array"/>, but right-aligned and padded on the left with as many <see langword="default"/> values as needed to create a length of <paramref name="totalWidth"/>.
    /// However, if <paramref name="totalWidth"/> is less than the length of <paramref name="array"/>, this method returns a new array with only <paramref name="totalWidth"/> values.</returns>
    public static T[] PadLeft<T>(this T[]? array, int totalWidth)
    {
        var newArray = new T[totalWidth];
        if (array is null)
        {
            return newArray;
        }

        var length = Math.Min(array.Length, newArray.Length);
        var start = totalWidth - length;
        Array.Copy(array, 0, newArray, start, length);
        return newArray;
    }

    /// <summary>
    /// Returns a new array that left-aligns the values in this array by padding them with default values on the right, for a specified total length.
    /// </summary>
    /// <typeparam name="T">The type of data in the array.</typeparam>
    /// <param name="array">The array to pad.</param>
    /// <param name="totalWidth">The number of values in the resulting array, equal to the number of original values plus any additional padding values.</param>
    /// <returns>A new array that is equivalent to <paramref name="array"/>, but left-aligned and padded on the right with as many <see langword="default"/> values as needed to create a length of <paramref name="totalWidth"/>.
    /// However, if <paramref name="totalWidth"/> is less than the length of <paramref name="array"/>, this method returns a new array with only <paramref name="totalWidth"/> values.</returns>
    public static T[] PadRight<T>(this T[]? array, int totalWidth)
    {
        var newArray = new T[totalWidth];
        if (array is null)
        {
            return newArray;
        }

        Array.Copy(array, 0, newArray, 0, Math.Min(array.Length, newArray.Length));
        return newArray;
    }
}