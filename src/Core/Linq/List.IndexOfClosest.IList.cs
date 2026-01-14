// -----------------------------------------------------------------------
// <copyright file="List.IndexOfClosest.IList.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

/// <content>
/// <see cref="IList{T}"/> extensions for getting the closest value.
/// </content>
public static partial class List
{
    /// <summary>
    /// Searches an entire one-dimensional sorted <see cref="IList{T}"/> for a specific element, using the <see cref="IComparable{T}"/> generic interface implemented by each element of the <see cref="IList{T}"/> and by the specified <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="source">The sorted one-dimensional, zero-based <see cref="IEnumerable{T}"/> to search.</param>
    /// <param name="value">The <typeparamref name="T"/> to search for.</param>
    /// <returns>
    /// The index of the specified <paramref name="value"/> in the specified <paramref name="source"/>, if <paramref name="value"/> is found; otherwise if <paramref name="value"/> is not found and value is less than one or more elements in array, the index of the first element that is larger than value.
    /// </returns>
    public static int IndexOfClosest<T>(this IList<T> source, T value)
        where T : IComparable =>
        source.IndexOfClosest(0, source.Count, value);

    /// <summary>
    /// Searches an entire one-dimensional sorted <see cref="IList{T}"/> for a specific element, using the <see cref="IComparable{T}"/> generic interface implemented by each element of the <see cref="IList{T}"/> and by the specified <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the list.</typeparam>
    /// <param name="source">The sorted one-dimensional, zero-based <see cref="IEnumerable{T}"/> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The <typeparamref name="T"/> to search for.</param>
    /// <returns>
    /// The index of the specified <paramref name="value"/> in the specified <paramref name="source"/>, if <paramref name="value"/> is found; otherwise if <paramref name="value"/> is not found and value is less than one or more elements in array, the index of the first element that is larger than value.
    /// </returns>
    public static int IndexOfClosest<T>(this IList<T> source, int index, int length, T value)
        where T : IComparable =>
        source.IndexOfClosest(index, length, value, Comparer<T>.Default.Compare);

    /// <summary>
    /// Searches an entire one-dimensional sorted <see cref="IList{TSource}"/> for the closest value to the specified <typeparamref name="TValue"/> , using the specified <paramref name="comparison"/> function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the list.</typeparam>
    /// <typeparam name="TValue">The value to search for.</typeparam>
    /// <param name="source">The sorted one-dimensional, zero-based <see cref="IList{TSource}"/> to search.</param>
    /// <param name="value">The <typeparamref name="TValue"/> to search for.</param>
    /// <param name="comparison">The method that compares <typeparamref name="TSource"/> and <typeparamref name="TValue"/> values.</param>
    /// <returns>
    /// The index of the specified <paramref name="value"/> in the specified <paramref name="source"/>, if <paramref name="value"/> is found; otherwise if <paramref name="value"/> is not found and value is less than one or more elements in array, the index of the first element that is larger than value.
    /// </returns>
    public static int IndexOfClosest<TSource, TValue>(this IList<TSource> source, TValue value, Func<TSource, TValue, int> comparison) => source.IndexOfClosest(0, source.Count, value, comparison);

    /// <summary>
    /// Searches an entire one-dimensional sorted <see cref="IList{TSource}"/> for the closest value to the specified <typeparamref name="TValue"/>, using the specified <paramref name="comparison"/> function.
    /// </summary>
    /// <typeparam name="TSource">The type of the elements of the list.</typeparam>
    /// <typeparam name="TValue">The value to search for.</typeparam>
    /// <param name="source">The sorted one-dimensional, zero-based <see cref="IList{TSource}"/> to search.</param>
    /// <param name="index">The starting index of the range to search.</param>
    /// <param name="length">The length of the range to search.</param>
    /// <param name="value">The <typeparamref name="TValue"/> to search for.</param>
    /// <param name="comparison">The method that compares <typeparamref name="TSource"/> and <typeparamref name="TValue"/> values.</param>
    /// <returns>
    /// The index of the specified <paramref name="value"/> in the specified <paramref name="source"/>, if <paramref name="value"/> is found; otherwise if <paramref name="value"/> is not found and value is less than one or more elements in array, the index of the first element that is larger than value.
    /// </returns>
    public static int IndexOfClosest<TSource, TValue>(this IList<TSource> source, int index, int length, TValue value, Func<TSource, TValue, int> comparison)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentOutOfRangeException.ThrowIfNegative(index);
        ArgumentNullException.ThrowIfNull(comparison);

        var low = index;
        var high = index + length - 1;

        if (comparison(source[low], value) >= 0)
        {
            return low;
        }

        if (comparison(source[high], value) <= 0)
        {
            return high + 1;
        }

        while (low < high)
        {
            var median = low + ((high - low) >> 1);
            var compare = comparison(source[median], value);

            if (compare is 0)
            {
                return median;
            }

            if (compare < 0)
            {
                low = median + 1;
            }
            else
            {
                high = median - 1;
            }
        }

        return low;
    }
}