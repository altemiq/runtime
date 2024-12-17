// -----------------------------------------------------------------------
// <copyright file="Int32Compressor{T}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The <see cref="int"/> compressor.
/// </summary>
/// <typeparam name="T">The type of codec.</typeparam>
internal sealed class Int32Compressor<T>() : Int32Compressor(new T())
    where T : IHeadlessInt32Codec, new();