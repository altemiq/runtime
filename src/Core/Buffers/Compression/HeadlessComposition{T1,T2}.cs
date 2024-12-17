// -----------------------------------------------------------------------
// <copyright file="HeadlessComposition{T1,T2}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// Helper class to compose headless schemes.
/// </summary>
/// <typeparam name="T1">The first type.</typeparam>
/// <typeparam name="T2">The second type.</typeparam>
internal sealed class HeadlessComposition<T1, T2>() : HeadlessComposition(new T1(), new T2())
    where T1 : IHeadlessInt32Codec, new()
    where T2 : IHeadlessInt32Codec, new();