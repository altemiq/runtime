// -----------------------------------------------------------------------
// <copyright file="ExtensionMethods.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

/// <summary>
/// The extension methods.
/// </summary>
internal static class ExtensionMethods
{
    /// <summary>
    /// Gets the name.
    /// </summary>
    /// <param name="finder">The finder.</param>
    /// <returns>The name.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="finder"/> is not a valid value.</exception>
    public static string ToName(this LzmaMatchFinder finder) =>
        finder switch
        {
            LzmaMatchFinder.BinaryTree2 => "bt2",
            LzmaMatchFinder.BinaryTree4 => "bt4",
            _ => throw new ArgumentOutOfRangeException(nameof(finder)),
        };
}