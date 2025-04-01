// -----------------------------------------------------------------------
// <copyright file="NewPfdS16.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// <see cref="NewPfd"/> based on <see cref="Simple16"/> by Yan et al.
/// </summary>
internal sealed class NewPfdS16() : NewPfd(S16.Compress, S16.Decompress)
{
    /// <inheritdoc/>
    public override string ToString() => nameof(NewPfdS16);
}