// -----------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETCOREAPP && !NETSTANDARD1_3_OR_GREATER && !NET46_OR_GREATER

#pragma warning disable IDE0130, MA0047, RCS1110, CheckNamespace

/// <summary>
/// The <see cref="Array"/> extensions.
/// </summary>
// ReSharper disable once CheckNamespace
internal static class ArrayExtensions
{
    extension(Array)
    {
        /// <summary>
        /// Returns an empty array.
        /// </summary>
        /// <typeparam name="T">The type of the elements of the array.</typeparam>
        /// <returns>An empty array.</returns>
        public static T[] Empty<T>() => [];
    }
}
#endif