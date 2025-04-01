// -----------------------------------------------------------------------
// <copyright file="ConfiguredCancellationTokenAwaitable.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.CompilerServices;

/// <summary>
/// The configured <see cref="CancellationToken"/>.
/// </summary>
[System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct ConfiguredCancellationTokenAwaitable
{
    private readonly CancellationToken cancellationToken;
    private readonly Threading.CancellationTokenConfigureAwaitOptions options;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfiguredCancellationTokenAwaitable"/> struct.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <param name="options">The configuration options.</param>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "Checked")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1068:CancellationToken parameters must come last", Justification = "This is not used to cancel something")]
    internal ConfiguredCancellationTokenAwaitable(CancellationToken cancellationToken, Threading.CancellationTokenConfigureAwaitOptions options)
    {
        this.cancellationToken = cancellationToken;
        this.options = options;
    }

    /// <inheritdoc cref="Task.GetAwaiter" />
    public CancellationTokenAwaiter GetAwaiter() => new(this.cancellationToken, this.options);
}