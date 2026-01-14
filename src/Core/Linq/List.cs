// -----------------------------------------------------------------------
// <copyright file="List.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

using System.Reflection;

#pragma warning disable RCS1263, SA1101

/// <summary>
/// Provides a set of static methods for querying objects that implement <see cref="IList{T}"/>.
/// </summary>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Naming", "CA1708:Identifiers should differ by more than case", Justification = "Extention Blocks")]
public static partial class List
{
    /// <content>
    /// The <see cref="IList{T}"/> extensions.
    /// </content>
    /// <typeparam name="T">The type of items to sort.</typeparam>
    /// <param name="values">The values.</param>
    extension<T>(IList<T> values)
    {
        /// <summary>
        /// Sorts the specified values.
        /// </summary>
        /// <param name="comparer">The comparer.</param>
        public void QuickSort(IComparer<T> comparer)
        {
            ArgumentNullException.ThrowIfNull(values);

            if (values is { Count: <= 1 })
            {
                return;
            }

            values.QuickSort(0, values.Count, comparer);
        }

        /// <summary>
        /// Sorts the specified values.
        /// </summary>
        /// <param name="comparison">The comparison.</param>
        public void QuickSort(Comparison<T> comparison)
        {
            ArgumentNullException.ThrowIfNull(values);

            switch (values)
            {
                case { Count: <= 1 }:
                    return;
                case T[] array:
                    Array.Sort(array, comparison);
                    break;
                case List<T> list:
                    list.Sort(comparison);
                    break;
                default:
                    values.QuickSort(0, values.Count, comparison);
                    break;
            }
        }

        /// <summary>
        /// Sorts the specified values.
        /// </summary>
        /// <param name="index">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="comparer">The comparer.</param>
        public void QuickSort(int index, int length, IComparer<T>? comparer)
        {
            ArgumentNullException.ThrowIfNull(values);

            if (values is { Count: <= 1 } || length <= 1)
            {
                return;
            }

            if (values is T[] array)
            {
                Array.Sort(array, index, length, comparer);
            }
            else if (values is List<T> list)
            {
                list.Sort(index, length, comparer);
            }
            else
            {
                comparer ??= Comparer<T>.Default;
                values.QuickSort(
                    index,
                    length,
                    comparer.Compare);
            }
        }

        /// <summary>
        /// Sorts the specified values.
        /// </summary>
        /// <param name="index">The start index.</param>
        /// <param name="length">The length.</param>
        /// <param name="comparison">The comparison.</param>
        public void QuickSort(
            int index,
            int length,
            Comparison<T> comparison)
        {
            ArgumentNullException.ThrowIfNull(values);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfNegative(length);
            ArgumentNullException.ThrowIfNull(comparison);

            switch (values)
            {
                case T[] array:
                    Array.Sort(array, index, length, new ComparisonWrapper<T>(comparison));
                    break;
                case List<T> list:
                    list.Sort(index, length, new ComparisonWrapper<T>(comparison));
                    break;
                default:
                    QuickSortCore(values, index, Math.Min(index + length, values.Count) - 1, comparison);
                    break;
            }

            static void QuickSortCore(IList<T> values, int left, int right, Comparison<T> comparer)
            {
                // The code in this function looks very similar to QuickSort in ArraySortHelper<T> class.
                // So the IL code will be different. This function is faster than the one in ArraySortHelper<T>.
                do
                {
                    var i = left;
                    var j = right;

                    // pre-sort the low, middle (pivot), and high values in place.
                    // this improves performance in the face of already sorted data, or
                    // data that is made up of multiple sorted runs appended together.
                    var middle = i + ((j - i) >> 1);
                    SwapIfGreaterWithItems(i, middle); // swap the low with the mid-point
                    SwapIfGreaterWithItems(i, j); // swap the low with the high
                    SwapIfGreaterWithItems(middle, j); // swap the middle with the high

                    var x = values[middle];
                    do
                    {
                        if (x is null)
                        {
                            // if x null, the loop to find two elements to be switched can be reduced.
                            while (j >= 0 && values[j] is not null)
                            {
                                j--;
                            }
                        }
                        else
                        {
                            while (comparer(x, values[i]) > 0)
                            {
                                i++;
                            }

                            while (j >= 0 && comparer(x, values[j]) < 0)
                            {
                                j--;
                            }
                        }

                        if (i > j)
                        {
                            break;
                        }

                        if (i < j)
                        {
                            (values[j], values[i]) = (values[i], values[j]);
                        }

                        i++;
                        j--;
                    }
                    while (i <= j);

                    if (j - left <= right - i)
                    {
                        if (left < j)
                        {
                            QuickSortCore(values, left, j, comparer);
                        }

                        left = i;
                    }
                    else
                    {
                        if (i < right)
                        {
                            QuickSortCore(values, i, right, comparer);
                        }

                        right = j;
                    }
                }
                while (left < right);

                void SwapIfGreaterWithItems(int firstIndex, int secondIndex)
                {
                    if (firstIndex == secondIndex
                        || comparer(values[firstIndex], values[secondIndex]) <= 0)
                    {
                        return;
                    }

                    (values[secondIndex], values[firstIndex]) = (values[firstIndex], values[secondIndex]);
                }
            }
        }
    }

    /// <content>
    /// The <see cref="IList{T}"/> extensions.
    /// </content>
    /// <param name="values">The values.</param>
    /// <typeparam name="T">The type of items to sort.</typeparam>
    extension<T>(IList<T?> values)
        where T : IComparable<T>
    {
        /// <summary>
        /// Sorts the specified values.
        /// </summary>
        public void QuickSort()
        {
            ArgumentNullException.ThrowIfNull(values);

            switch (values)
            {
                case { Count: <= 1 }:
                    return;
                case T[] array:
                    Array.Sort(array);
                    break;
                case List<T> list:
                    list.Sort();
                    break;
                default:
                    values.QuickSort(0, values.Count);
                    break;
            }
        }

        /// <summary>
        /// Sorts the specified values.
        /// </summary>
        /// <param name="index">The start index.</param>
        /// <param name="length">The length.</param>
        public void QuickSort(int index, int length)
        {
            ArgumentNullException.ThrowIfNull(values);
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfNegative(length);

            if (length is 0)
            {
                return;
            }

            QuickSortCore(values, index, Math.Min(index + length, values.Count) - 1);

            static void QuickSortCore(IList<T?> values, int left, int right)
            {
                // The code in this function looks very similar to QuickSort in ArraySortHelper<T> class.
                // The difference is that T is constrained to IComparable<T> here.
                // So the IL code will be different. This function is faster than the one in ArraySortHelper<T>.
                do
                {
                    var i = left;
                    var j = right;

                    // pre-sort the low, middle (pivot), and high values in place.
                    // this improves performance in the face of already sorted data, or
                    // data that is made up of multiple sorted runs appended together.
                    var middle = i + ((j - i) >> 1);
                    SwapIfGreaterWithItems(i, middle); // swap the low with the mid-point
                    SwapIfGreaterWithItems(i, j); // swap the low with the high
                    SwapIfGreaterWithItems(middle, j); // swap the middle with the high

                    var x = values[middle];
                    do
                    {
                        if (x is null)
                        {
                            // if x null, the loop to find two elements to be switched can be reduced.
                            while (j >= 0 && values[j] is not null)
                            {
                                j--;
                            }
                        }
                        else
                        {
                            while (values[i] is { } v && x.CompareTo(v) > 0)
                            {
                                i++;
                            }

                            while (j >= 0 && values[j] is { } v && x.CompareTo(v) < 0)
                            {
                                j--;
                            }
                        }

                        if (i > j)
                        {
                            break;
                        }

                        if (i < j)
                        {
                            (values[j], values[i]) = (values[i], values[j]);
                        }

                        i++;
                        j--;
                    }
                    while (i <= j);

                    if (j - left <= right - i)
                    {
                        if (left < j)
                        {
                            QuickSortCore(values, left, j);
                        }

                        left = i;
                    }
                    else
                    {
                        if (i < right)
                        {
                            QuickSortCore(values, i, right);
                        }

                        right = j;
                    }
                }
                while (left < right);

                void SwapIfGreaterWithItems(int firstIndex, int secondIndex)
                {
                    if (firstIndex == secondIndex
                        || values[firstIndex] is not { } first
                        || values[secondIndex] is not { } second
                        || first.CompareTo(second) <= 0)
                    {
                        return;
                    }

                    values[firstIndex] = second;
                    values[secondIndex] = first;
                }
            }
        }
    }

    /// <summary>
    /// Casts the elements of an <see cref="IList{T}"/> into a base type.
    /// </summary>
    /// <typeparam name="TSource">The derived type to cast the elements of <paramref name="source"/> from.</typeparam>
    /// <typeparam name="TResult">The base type to cast the elements of <paramref name="source"/> to.</typeparam>
    /// <param name="source">The <see cref="IList{T}"/> that contains the elements to be cast.</param>
    /// <returns>An <see cref="IList{T}"/> that contains each element of <paramref name="source"/> cast to <typeparamref name="TResult"/>.</returns>
    [System.Diagnostics.CodeAnalysis.RequiresDynamicCode($"Requires reflection on {nameof(source)}")]
    [return: System.Diagnostics.CodeAnalysis.NotNullIfNotNull(nameof(source))]
    public static IList<TResult>? Cast<TSource, TResult>(this IList<TSource>? source)
        where TSource : class, TResult
        where TResult : class
    {
        return source switch
        {
            TSource[] array => CreateArray(array),
            { } list => CreateList(list),
            _ => null,
        };

        static TResult[] CreateArray(TSource[] source)
        {
            return [.. source];
        }

        static IList<TResult> CreateList(IList<TSource> source)
        {
            var destination = CreateListCore(source) ?? throw new InvalidOperationException($"Failed to create instance of {source.GetType().Name}");

            if (destination.IsReadOnly || destination.Count == source.Count)
            {
                // it is read-only or we have already filled it
                return destination;
            }

            if (destination is List<TResult> list)
            {
                list.AddRange(source);
            }
            else
            {
                // Go through each item
                foreach (var item in source)
                {
                    // Cast the list item
                    destination.Add(item);
                }
            }

            return destination;

            static IList<TResult>? CreateListCore(IList<TSource> source)
            {
                return GetGenericTypeDefinition(source.GetType()) is { } type
                    ? CreateList(source, type.MakeGenericType(typeof(TResult)))
                    : default;

                static Type? GetGenericTypeDefinition(Type type)
                {
                    var typeInfo = type.GetTypeInfo();

                    while (true)
                    {
                        // If this is a generic type definition, then use that.
                        if (typeInfo.IsGenericTypeDefinition)
                        {
                            return typeInfo.AsType();
                        }

                        // If this is a generic type, then return the generic type definition for it.
                        if (typeInfo.IsGenericType)
                        {
                            return typeInfo.GetGenericTypeDefinition();
                        }

                        // If this has no base type, then return null.
                        if (typeInfo.BaseType is null)
                        {
                            return default;
                        }

                        typeInfo = typeInfo.BaseType.GetTypeInfo();
                    }
                }

                static IList<TResult> CreateList(IList<TSource> source, [System.Diagnostics.CodeAnalysis.DynamicallyAccessedMembers(System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes.PublicConstructors)] Type genericType)
                {
                    // Get the constructor info
                    var constructors =
#if NETSTANDARD1_3_OR_GREATER || NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER
                        genericType.GetConstructors();
#else
                        genericType.GetTypeInfo().DeclaredConstructors;
#endif

                    var parameterObjects = GetParameters(constructors, source);

                    // Create the type
                    return Activator.CreateInstance(genericType, parameterObjects) as IList<TResult> ?? throw new InvalidOperationException($"Failed to create instance of {genericType}");

                    static object?[] GetParameters(IEnumerable<ConstructorInfo> constructors, IEnumerable<TSource> source)
                    {
                        // Get the best constructor
                        return constructors
                            .Select(static constructor => constructor.GetParameters())
                            .SingleOrDefault(static parameters => parameters.Length is 1 && typeof(IEnumerable<TResult>).GetTypeInfo().IsAssignableFrom(parameters[0].ParameterType.GetTypeInfo())) is not null
                            ? [source]
                            : [];
                    }
                }
            }
        }
    }

#if NETCOREAPP2_0_OR_GREATER || NET40_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    private readonly struct ComparisonWrapper<T>(Comparison<T> comparison) : IComparer<T>
    {
        public int Compare(T? x, T? y) => (x, y) switch
        {
            (null, null) => 0,
            (null, not null) => 1,
            (not null, null) => -1,
            _ => comparison(x, y),
        };
    }
}