// -----------------------------------------------------------------------
// <copyright file="ArrayExtensions.Instance.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

#pragma warning disable RCS1263, SA1101, S2325

/// <summary>
/// <see cref="ArrayExtensions"/> extensions.
/// </summary>
public static class ArrayExtensions
{
    /// <content>
    /// <see cref="ArrayExtensions"/> extensions.
    /// </content>
    /// <param name="array">The array to pad.</param>
    /// <typeparam name="T">The type of data in the array.</typeparam>
    extension<T>(T[]? array)
    {
        /// <summary>
        /// Returns a new array that right-aligns the values in this array by padding them with default values on the left, for a specified total length.
        /// </summary>
        /// <param name="totalWidth">The number of values in the resulting array, equal to the number of original values plus any additional padding values.</param>
        /// <returns>A new array that is equivalent to this instance, but right-aligned and padded on the left with as many <see langword="default"/> values as needed to create a length of <paramref name="totalWidth"/>.
        /// However, if <paramref name="totalWidth"/> is less than the length of this instance, this method returns a new array with only <paramref name="totalWidth"/> values.</returns>
        public T[] PadLeft(int totalWidth)
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
        /// <param name="totalWidth">The number of values in the resulting array, equal to the number of original values plus any additional padding values.</param>
        /// <returns>A new array that is equivalent to this instance, but left-aligned and padded on the right with as many <see langword="default"/> values as needed to create a length of <paramref name="totalWidth"/>.
        /// However, if <paramref name="totalWidth"/> is less than the length of this instance, this method returns a new array with only <paramref name="totalWidth"/> values.</returns>
        public T[] PadRight(int totalWidth)
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
}