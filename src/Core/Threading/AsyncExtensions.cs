// -----------------------------------------------------------------------
// <copyright file="AsyncExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System.Threading;
#pragma warning restore IDE0130, CheckNamespace

/// <summary>
/// Async extensions.
/// </summary>
public static class AsyncExtensions
{
    /// <summary>
    /// Allows a cancellation token to be awaited.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The awaiter for <paramref name="cancellationToken"/>.</returns>
    [ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)]
    public static Altemiq.Runtime.CompilerServices.CancellationTokenAwaiter GetAwaiter(this CancellationToken cancellationToken) => new(cancellationToken);

    /// <summary>
    /// Configures the <see cref="CancellationToken"/> to not throw when awaited.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="throwOnResult">Set to <see langword="true"/> to throw an <see cref="OperationCanceledException"/> when awaited.</param>
    /// <returns>An object used to await this <paramref name="cancellationToken"/>.</returns>
    public static Altemiq.Runtime.CompilerServices.ConfiguredCancellationTokenAwaitable ConfigureAwait(this CancellationToken cancellationToken, bool throwOnResult) => new(cancellationToken, throwOnResult ? Altemiq.Threading.CancellationTokenConfigureAwaitOptions.None : Altemiq.Threading.CancellationTokenConfigureAwaitOptions.SuppressThrowing);

    /// <summary>
    /// Configures the <see cref="CancellationToken"/> to not throw when awaited.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="options">Options used to configure how awaits on this cancellation token are performed.</param>
    /// <returns>An object used to await this <paramref name="cancellationToken"/>.</returns>
    public static Altemiq.Runtime.CompilerServices.ConfiguredCancellationTokenAwaitable ConfigureAwait(this CancellationToken cancellationToken, Altemiq.Threading.CancellationTokenConfigureAwaitOptions options) => new(cancellationToken, options);
}