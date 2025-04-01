// -----------------------------------------------------------------------
// <copyright file="OptPfdS9.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// <see cref="OptPfd"/> based on <see cref="Simple9"/> by Yan et al.
/// </summary>
internal sealed class OptPfdS9() : OptPfd(S9.Compress, S9.Decompress, S9.EstimateCompress)
{
    /// <inheritdoc/>
    public override string ToString() => nameof(OptPfdS9);
}