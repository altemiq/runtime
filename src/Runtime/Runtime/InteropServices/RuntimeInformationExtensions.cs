// -----------------------------------------------------------------------
// <copyright file="RuntimeInformationExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable CA1050, MA0047, RCS1110, CheckNamespace

/// <summary>
/// <see cref="System.Runtime.InteropServices.RuntimeInformation"/> extensions.
/// </summary>
public static class RuntimeInformationExtensions
{
#pragma warning disable SA1137, SA1400, S1144
    /// <summary>
    /// <see cref="System.Runtime.InteropServices.RuntimeInformation"/> extensions.
    /// </summary>
    extension(System.Runtime.InteropServices.RuntimeInformation)
    {
        /// <summary>
        /// Gets the target framework.
        /// </summary>
        public static string TargetFramework => Altemiq.Runtime.InteropServices.RuntimeInformation.TargetFramework;

        /// <summary>
        /// Gets the target platform, or an empty string.
        /// </summary>
        public static string TargetPlatform => Altemiq.Runtime.InteropServices.RuntimeInformation.TargetPlatform;

#if !NET5_0_OR_GREATER
        /// <summary>
        /// Gets the platform for which the runtime was built (or on which an app is running).
        /// </summary>
        public static string RuntimeIdentifier => Altemiq.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;
#endif
    }
#pragma warning restore SA1137, SA1400, S1144
}

#pragma warning restore CA1050, MA0047, RCS1110, RCS1222, IDE0079, CheckNamespace