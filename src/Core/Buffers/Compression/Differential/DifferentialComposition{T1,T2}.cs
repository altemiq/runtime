// -----------------------------------------------------------------------
// <copyright file="DifferentialComposition{T1,T2}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// An differential composition.
/// </summary>
/// <typeparam name="T1">The first type.</typeparam>
/// <typeparam name="T2">The second type.</typeparam>
internal sealed class DifferentialComposition<T1, T2>() : DifferentialComposition(new T1(), new T2())
    where T1 : IDifferentialInt32Codec, new()
    where T2 : IDifferentialInt32Codec, new();