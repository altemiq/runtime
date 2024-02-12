// -----------------------------------------------------------------------
// <copyright file="Array.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// <see cref="System.Array"/> extensions.
/// </summary>
public static class Array
{
    /// <summary>
    /// Returns a new array that right-aligns the values in this array by padding them with default values on the left, for a specified total length.
    /// </summary>
    /// <typeparam name="T">The type of data in the array.</typeparam>
    /// <param name="array">The array to pad.</param>
    /// <param name="totalWidth">The number of values in the resulting array, equal to the number of original values plus any additional padding values.</param>
    /// <remarks>If <paramref name="totalWidth"/> is less than the length of <paramref name="array"/>, this method returns a new array with only <paramref name="totalWidth"/> values.</remarks>
    public static void PadLeft<T>(ref T[] array, int totalWidth)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(array);
        if (totalWidth < array.Length)
        {
            System.Array.Resize(ref array, totalWidth);
            return;
        }

        var newArray = new T[totalWidth];
        var length = Math.Min(array.Length, newArray.Length);
        var start = totalWidth - length;
        System.Array.Copy(array, 0, newArray, start, length);
        array = newArray;
    }

    /// <summary>
    /// Returns a new array that left-aligns the values in this array by padding them with default values on the right, for a specified total length.
    /// </summary>
    /// <typeparam name="T">The type of data in the array.</typeparam>
    /// <param name="array">The array to pad.</param>
    /// <param name="totalWidth">The number of values in the resulting array, equal to the number of original values plus any additional padding values.</param>
    /// <remarks>If <paramref name="totalWidth"/> is less than the length of <paramref name="array"/>, this method returns a new array with only <paramref name="totalWidth"/> values.</remarks>
    public static void PadRight<T>(ref T[] array, int totalWidth)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(array);
        System.Array.Resize(ref array, totalWidth);
    }
}