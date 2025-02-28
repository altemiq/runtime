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
    /// Avoids throwing an exception at the completion of awaiting a <see cref="CancellationToken"/>.
    /// </summary>
    SuppressThrowing = 1 << 1,
}