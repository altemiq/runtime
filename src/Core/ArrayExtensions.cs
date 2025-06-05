// -----------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System;
#pragma warning restore IDE0130, CheckNamespace

/// <summary>
/// <see cref="System.Array"/> extensions.
/// </summary>
public static class ArrayExtensions
{
    /// <summary>
    /// The <see cref="Array"/> extensions.
    /// </summary>
    extension(Array)
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
            ArgumentNullException.ThrowIfNull(array);
            if (array.Length == totalWidth)
            {
                return;
            }

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
            ArgumentNullException.ThrowIfNull(array);
            if (array.Length == totalWidth)
            {
                return;
            }

            System.Array.Resize(ref array, totalWidth);
        }

#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_0_OR_GREATER
        /// <summary>
        /// Assigns the given value of type <typeparamref name="T"/> to the elements of the specified <paramref name="array"/> which are within the range of <paramref name="startIndex"/> (inclusive) and the next <paramref name="count"/> number of indices.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The <see cref="System.Array"/> to fill.</param>
        /// <param name="value">The new value for the elements in the specified range.</param>
        /// <param name="startIndex">A 32-bit integer that represents the index in the <see cref="System.Array"/> at which filling begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        public static void Fill<T>(T[] array, T value, int startIndex, int count)
        {
            ArgumentNullException.ThrowIfNull(array);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(startIndex, array.Length);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(count, array.Length - startIndex);

            for (var i = startIndex; i < startIndex + count; i++)
            {
                array[i] = value;
            }
        }
#endif
    }
}