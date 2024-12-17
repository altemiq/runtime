// -----------------------------------------------------------------------
// <copyright file="OptPfdS16.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// <see cref="OptPfd"/> based on <see cref="Simple16"/> by Yan et al.
/// </summary>
internal sealed class OptPfdS16() : OptPfd(S16.Compress, S16.Uncompress, S16.EstimateCompress)
{
    /// <inheritdoc/>
    public override string ToString() => nameof(OptPfdS16);
}