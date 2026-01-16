// -----------------------------------------------------------------------
// <copyright file="IInt32Codec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The <see cref="int"/> codec.
/// </summary>
internal interface IInt32Codec : ICompressCodec<int, int>, IDecompressCodec<int, int>;