// -----------------------------------------------------------------------
// <copyright file="IHeadlessInt32Codec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The headless <see cref="int"/> codec.
/// </summary>
internal interface IHeadlessInt32Codec : ICompressHeadlessCodec<int, int>, IDecompressHeadlessCodec<int, int>;