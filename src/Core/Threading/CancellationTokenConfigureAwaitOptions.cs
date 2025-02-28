// -----------------------------------------------------------------------
// <copyright file="CancellationTokenConfigureAwaitOptions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Threading;

/// <summary>
/// The <see cref="CancellationToken"/> configure await options.
/// </summary>
[Flags]
public enum CancellationTokenConfigureAwaitOptions
{
    /// <summary>
    /// No options specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Throw an <see cref="OperationCanceledException"/> when getting the result.
    /// </summary>
    ThrowOnResult = 1 << 0,
}