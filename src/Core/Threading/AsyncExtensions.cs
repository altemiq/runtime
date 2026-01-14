// -----------------------------------------------------------------------
// <copyright file="AsyncExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System.Threading;
#pragma warning restore IDE0130, CheckNamespace

#pragma warning disable RCS1263, SA1101, S2325

/// <summary>
/// Async extensions.
/// </summary>
public static class AsyncExtensions
{
    /// <content>
    /// Async extensions.
    /// </content>
    /// <param name="cancellationToken">The cancellation token.</param>
    extension(CancellationToken cancellationToken)
    {
        /// <summary>
        /// Allows a cancellation token to be awaited.
        /// </summary>
        /// <returns>The awaiter for this instance.</returns>
        [ComponentModel.EditorBrowsable(ComponentModel.EditorBrowsableState.Never)]
        public Altemiq.Runtime.CompilerServices.CancellationTokenAwaiter GetAwaiter() => new(cancellationToken);

        /// <summary>
        /// Configures the <see cref="CancellationToken"/> to not throw when awaited.
        /// </summary>
        /// <param name="throwOnResult">Set to <see langword="true"/> to throw an <see cref="OperationCanceledException"/> when awaited.</param>
        /// <returns>An object used to await this instance.</returns>
        public Altemiq.Runtime.CompilerServices.ConfiguredCancellationTokenAwaitable ConfigureAwait(bool throwOnResult) => new(cancellationToken, throwOnResult ? Altemiq.Threading.CancellationTokenConfigureAwaitOptions.None : Altemiq.Threading.CancellationTokenConfigureAwaitOptions.SuppressThrowing);

        /// <summary>
        /// Configures the <see cref="CancellationToken"/> to not throw when awaited.
        /// </summary>
        /// <param name="options">Options used to configure how awaits on this cancellation token are performed.</param>
        /// <returns>An object used to await this instance.</returns>
        public Altemiq.Runtime.CompilerServices.ConfiguredCancellationTokenAwaitable ConfigureAwait(Altemiq.Threading.CancellationTokenConfigureAwaitOptions options) => new(cancellationToken, options);
    }
}