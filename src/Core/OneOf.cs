// -----------------------------------------------------------------------
// <copyright file="OneOf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <summary>
/// <see cref="IOneOf"/> methods.
/// </summary>
public static class OneOf
{
    /// <summary>
    /// Gets a new instance of <see cref="OneOf{T0}"/> from the specified <typeparamref name="T0"/>.
    /// </summary>
    /// <typeparam name="T0">The type of value.</typeparam>
    /// <param name="input">The input.</param>
    /// <returns>The new instance of <see cref="OneOf{T0}"/>.</returns>
    public static OneOf<T0> From<T0>(T0 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T0"/> to a <see cref="OneOf{T0,T1}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1}"/>.</returns>
    public static OneOf<T0, T1> From<T0, T1>(T0 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T1"/> to a <see cref="OneOf{T0,T1}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1}"/>.</returns>
    public static OneOf<T0, T1> From<T0, T1>(T1 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T0"/> to a <see cref="OneOf{T0,T1,T2}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2}"/>.</returns>
    public static OneOf<T0, T1, T2> From<T0, T1, T2>(T0 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T1"/> to a <see cref="OneOf{T0,T1,T2}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2}"/>.</returns>
    public static OneOf<T0, T1, T2> From<T0, T1, T2>(T1 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T2"/> to a <see cref="OneOf{T0,T1,T2}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2}"/>.</returns>
    public static OneOf<T0, T1, T2> From<T0, T1, T2>(T2 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T0"/> to a <see cref="OneOf{T0,T1,T2,T3}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3}"/>.</returns>
    public static OneOf<T0, T1, T2, T3> From<T0, T1, T2, T3>(T0 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T1"/> to a <see cref="OneOf{T0,T1,T2,T3}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3}"/>.</returns>
    public static OneOf<T0, T1, T2, T3> From<T0, T1, T2, T3>(T1 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T2"/> to a <see cref="OneOf{T0,T1,T2,T3}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3}"/>.</returns>
    public static OneOf<T0, T1, T2, T3> From<T0, T1, T2, T3>(T2 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T3"/> to a <see cref="OneOf{T0,T1,T2,T3}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3}"/>.</returns>
    public static OneOf<T0, T1, T2, T3> From<T0, T1, T2, T3>(T3 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T0"/> to a <see cref="OneOf{T0,T1,T2,T3,T4}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <typeparam name="T4">The fifth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3,T4}"/>.</returns>
    public static OneOf<T0, T1, T2, T3, T4> From<T0, T1, T2, T3, T4>(T0 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T1"/> to a <see cref="OneOf{T0,T1,T2,T3,T4}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <typeparam name="T4">The fifth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3,T4}"/>.</returns>
    public static OneOf<T0, T1, T2, T3, T4> From<T0, T1, T2, T3, T4>(T1 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T2"/> to a <see cref="OneOf{T0,T1,T2,T3,T4}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <typeparam name="T4">The fifth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3,T4}"/>.</returns>
    public static OneOf<T0, T1, T2, T3, T4> From<T0, T1, T2, T3, T4>(T2 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T3"/> to a <see cref="OneOf{T0,T1,T2,T3,T4}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <typeparam name="T4">The fifth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3,T4}"/>.</returns>
    public static OneOf<T0, T1, T2, T3, T4> From<T0, T1, T2, T3, T4>(T3 input) => input;

    /// <summary>
    /// Converts a <typeparamref name="T4"/> to a <see cref="OneOf{T0,T1,T2,T3,T4}"/>.
    /// </summary>
    /// <typeparam name="T0">The first type parameter.</typeparam>
    /// <typeparam name="T1">The second type parameter.</typeparam>
    /// <typeparam name="T2">The third type parameter.</typeparam>
    /// <typeparam name="T3">The forth type parameter.</typeparam>
    /// <typeparam name="T4">The fifth type parameter.</typeparam>
    /// <param name="input">The value.</param>
    /// <returns>The <see cref="OneOf{T0,T1,T2,T3,T4}"/>.</returns>
    public static OneOf<T0, T1, T2, T3, T4> From<T0, T1, T2, T3, T4>(T4 input) => input;

    /// <summary>
    /// Formats the value.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="value">The value.</param>
    /// <returns>The formatted value.</returns>
    internal static string FormatValue<T>(T value) => $"{typeof(T).FullName}: {value?.ToString()}";

    /// <summary>
    /// Formats the value.
    /// </summary>
    /// <typeparam name="T">The type of value.</typeparam>
    /// <param name="this">The instance.</param>
    /// <param name="base">The base.</param>
    /// <param name="value">The value.</param>
    /// <returns>The formatted value.</returns>
    internal static string? FormatValue<T>(object @this, object @base, T value) => ReferenceEquals(@this, value) ? @base.ToString() : $"{typeof(T).FullName}: {value?.ToString()}";

    /// <summary>
    /// Value of none.
    /// </summary>
    public readonly struct None
    {
        /// <summary>
        /// Creates a new instance of <see cref="OneOf{T0,None}"/> set to <see cref="None"/>.
        /// </summary>
        /// <typeparam name="T0">The type in the <see cref="OneOf{T0,None}"/>.</typeparam>
        /// <returns>The created instance <see cref="OneOf{T0,None}"/>.</returns>
        public static OneOf<T0, None> Of<T0>() => default(None);

        /// <summary>
        /// Creates a new instance of <see cref="OneOf{T0,T1,None}"/> set to <see cref="None"/>.
        /// </summary>
        /// <typeparam name="T0">The first type in the <see cref="OneOf{T0,T1,None}"/>.</typeparam>
        /// <typeparam name="T1">The second type in the <see cref="OneOf{T0,T1,None}"/>.</typeparam>
        /// <returns>The created instance <see cref="OneOf{T0,T1,None}"/>.</returns>
        public static OneOf<T0, T1, None> Of<T0, T1>() => default(None);

        /// <summary>
        /// Creates a new instance of <see cref="OneOf{T0,T1,T2,None}"/> set to <see cref="None"/>.
        /// </summary>
        /// <typeparam name="T0">The first type in the <see cref="OneOf{T0,T1,T2,None}"/>.</typeparam>
        /// <typeparam name="T1">The second type in the <see cref="OneOf{T0,T1,T2,None}"/>.</typeparam>
        /// <typeparam name="T2">The third in the <see cref="OneOf{T0,T1,T2,None}"/>.</typeparam>
        /// <returns>The created instance <see cref="OneOf{T0,T1,T2,None}"/>.</returns>
        public static OneOf<T0, T1, T2, None> Of<T0, T1, T2>() => default(None);

        /// <summary>
        /// Creates a new instance of <see cref="OneOf{T0,T1,T2,T3,None}"/> set to <see cref="None"/>.
        /// </summary>
        /// <typeparam name="T0">The first type in the <see cref="OneOf{T0,T1,T2,T3,None}"/>.</typeparam>
        /// <typeparam name="T1">The second type in the <see cref="OneOf{T0,T1,T2,T3,None}"/>.</typeparam>
        /// <typeparam name="T2">The third type in the <see cref="OneOf{T0,T1,T2,T3,None}"/>.</typeparam>
        /// <typeparam name="T3">The forth type in the <see cref="OneOf{T0,T1,T2,T3,None}"/>.</typeparam>
        /// <returns>The created instance <see cref="OneOf{T0,T1,T2,T3,None}"/>.</returns>
        public static OneOf<T0, T1, T2, T3, None> Of<T0, T1, T2, T3>() => default(None);
    }
}