// -----------------------------------------------------------------------
// <copyright file="HeadlessDifferentialComposition{T1,T2}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// A differential composition.
/// </summary>
/// <typeparam name="T1">The first type.</typeparam>
/// <typeparam name="T2">The second type.</typeparam>
internal sealed class HeadlessDifferentialComposition<T1, T2>() : HeadlessDifferentialComposition(new T1(), new T2())
    where T1 : IHeadlessDifferentialInt32Codec, new()
    where T2 : IHeadlessDifferentialInt32Codec, new();