// -----------------------------------------------------------------------
// <copyright file="Enumerable.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

/// <summary>
/// Extensions for <see cref="IEnumerable{T}"/>.
/// </summary>
public static class Enumerable
{
    /// <summary>
    /// Filters a sequence of values based on the ones that are not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that are not <see langword="null"/>.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
    where T : class
    {
        ArgumentNullException.ThrowIfNull(source);
        return WhereNotNullCore(source);

        static IEnumerable<T> WhereNotNullCore(IEnumerable<T?> source)
        {
            foreach (var item in source.Where(static x => x is not null))
            {
                yield return item!;
            }
        }
    }

    /// <summary>
    /// Filters a sequence of values based on the ones that are not <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that are not <see langword="null"/>.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T?> source)
        where T : struct
    {
        ArgumentNullException.ThrowIfNull(source);
        return WhereNotNullCore(source);

        static IEnumerable<T> WhereNotNullCore(IEnumerable<T?> source)
        {
            foreach (var item in source)
            {
                if (item.HasValue)
                {
                    yield return item.Value;
                }
            }
        }
    }

    /// <summary>
    /// Filters a sequence of values based on the ones that are <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that are <see langword="null"/>.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T?> WhereNull<T>(this IEnumerable<T?> source)
        where T : class => source.Where(static item => item is null);

    /// <summary>
    /// Filters a sequence of values based on the ones that are <see langword="null"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of <paramref name="source"/>.</typeparam>
    /// <param name="source">An <see cref="IEnumerable{T}"/> to filter.</param>
    /// <returns>An <see cref="IEnumerable{T}"/> that contains elements from the input sequence that are <see langword="null"/>.</returns>
    [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T?> WhereNull<T>(this IEnumerable<T?> source)
        where T : struct => source.Where(static item => !item.HasValue);
}