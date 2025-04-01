// -----------------------------------------------------------------------
// <copyright file="NewPfdS9.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// <see cref="NewPfd"/> based on <see cref="Simple9" />  by Yan et al.
/// </summary>
internal sealed class NewPfdS9() : NewPfd(S9.Compress, S9.Decompress)
{
    /// <inheritdoc/>
    public override string ToString() => nameof(NewPfdS9);
}