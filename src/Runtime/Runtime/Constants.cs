// -----------------------------------------------------------------------
// <copyright file="Constants.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

/// <summary>
/// Constant values.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The additional probing path constants.
    /// </summary>
    public static class AdditionalProbingPath
    {
        /// <summary>
        /// The runtime config property name.
        /// </summary>
        public const string RuntimeConfigPropertyName = "additionalProbingPaths";
    }

    /// <summary>
    /// The apply patches setting constants.
    /// </summary>
    public static class ApplyPatchesSetting
    {
        /// <summary>
        /// The runtime config property name.
        /// </summary>
        public const string RuntimeConfigPropertyName = "applyPatches";
    }

    /// <summary>
    /// The roll forward on no candidate framework setting constants.
    /// </summary>
    public static class RollForwardOnNoCandidateFxSetting
    {
        /// <summary>
        /// The runtime config property name.
        /// </summary>
        public const string RuntimeConfigPropertyName = "rollForwardOnNoCandidateFx";
    }

    /// <summary>
    /// The roll forward setting constants.
    /// </summary>
    public static class RollForwardSetting
    {
        /// <summary>
        /// The runtime config property name.
        /// </summary>
        public const string RuntimeConfigPropertyName = "rollForward";
    }
}