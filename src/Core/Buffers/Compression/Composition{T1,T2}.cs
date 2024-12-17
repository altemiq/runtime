// -----------------------------------------------------------------------
// <copyright file="Composition{T1,T2}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Helper class to compose schemes.
/// </summary>
/// <typeparam name="T1">The first type.</typeparam>
/// <typeparam name="T2">The second type.</typeparam>
internal sealed class Composition<T1, T2>() : Composition(new T1(), new T2())
    where T1 : IInt32Codec, new()
    where T2 : IInt32Codec, new();