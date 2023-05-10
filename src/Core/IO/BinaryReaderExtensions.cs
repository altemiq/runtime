// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

/// <summary>
/// <see cref="BinaryReader"/> extensions.
/// </summary>
public static class BinaryReaderExtensions
{
    /// <summary>
    /// Reads the specified characters from the string.
    /// </summary>
    /// <param name="reader">The reader.</param>
    /// <param name="count">The number of characters.</param>
    /// <returns>The string from the reader.</returns>
    public static string ReadString(this BinaryReader reader, int count) => new(reader.ReadChars(count));
}