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
    /// Binary Tree with 2 hash bytes.
    /// </summary>
    [Runtime.Serialization.EnumMember(Value = "bt2")]
    BinaryTree2,

    /// <summary>
    /// Binary Tree with 4 hash bytes.
    /// </summary>
    [Runtime.Serialization.EnumMember(Value = "bt4")]
    BinaryTree4,
}