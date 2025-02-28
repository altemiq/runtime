// -----------------------------------------------------------------------
// <copyright file="CancellationTokenAwaiter.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.CompilerServices;

/// <summary>
/// The awaiter for cancellation tokens.
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct CancellationTokenAwaiter : System.Runtime.CompilerServices.ICriticalNotifyCompletion
{
    /// <summary>
    /// The cancellation token.
    /// </summary>
    internal readonly CancellationToken CancellationToken;

    /// <summary>
    /// The options.
    /// </summary>
    internal readonly Threading.CancellationTokenConfigureAwaitOptions Options;

    /// <summary>
    /// Initializes a new instance of the <see cref="CancellationTokenAwaiter"/> struct.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="options">The options.</param>
    internal CancellationTokenAwaiter(CancellationToken cancellationToken, Threading.CancellationTokenConfigureAwaitOptions options = Threading.CancellationTokenConfigureAwaitOptions.ThrowOnResult)
    {
        this.CancellationToken = cancellationToken;
        this.Options = options;
    }

    /// <inheritdoc cref="System.Runtime.CompilerServices.TaskAwaiter.IsCompleted" />
    /// <remarks>
    /// Called by compiler generated/.net internals to check if the task has completed.
    /// </remarks>
    public bool IsCompleted => this.CancellationToken.IsCancellationRequested;

    /// <inheritdoc cref="System.Runtime.CompilerServices.TaskAwaiter.GetResult" />
    public void GetResult()
    {
        // This is called by compiler generated methods when the task has completed.
        // Instead of returning a result, we just throw an exception.
        if (!this.IsCompleted)
        {
            throw new InvalidOperationException("The cancellation token has not yet been cancelled.");
        }

        if (this.Options.HasFlag(Threading.CancellationTokenConfigureAwaitOptions.ThrowOnResult))
        {
            throw new OperationCanceledException();
        }
    }

    /// <inheritdoc cref="System.Runtime.CompilerServices.TaskAwaiter.OnCompleted(Action)" />
    /// <remarks>
    /// The compiler will generate stuff that hooks in here. We hook those methods directly into the cancellation token.
    /// </remarks>
    public void OnCompleted(Action continuation) => this.CancellationToken.Register(continuation);

    /// <inheritdoc cref="System.Runtime.CompilerServices.TaskAwaiter.UnsafeOnCompleted(Action)" />
    /// <remarks>
    /// The compiler will generate stuff that hooks in here. We hook those methods directly into the cancellation token.
    /// </remarks>
    public void UnsafeOnCompleted(Action continuation) => this.CancellationToken.Register(continuation);
}