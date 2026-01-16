// -----------------------------------------------------------------------
// <copyright file="ISByteCodec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The <see cref="sbyte"/> to <see cref="int"/> codec.
/// </summary>
internal interface ISByteCodec : ICompressCodec<int, sbyte>, IDecompressCodec<sbyte, int>;