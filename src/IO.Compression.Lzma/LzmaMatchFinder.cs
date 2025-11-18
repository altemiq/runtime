// -----------------------------------------------------------------------
// <copyright file="LzmaMatchFinder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression;

/// <summary>
/// The LZMA match finders.
/// </summary>
public enum LzmaMatchFinder
{
    /// <summary>
    /// Binary Tree with 2.
    /// </summary>
    BinaryTree2,

    /// <summary>
    /// Binary Tree with 4.
    /// </summary>
    BinaryTree4,
}