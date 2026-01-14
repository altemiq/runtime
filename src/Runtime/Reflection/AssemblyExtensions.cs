// -----------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Reflection;

/// <summary>
/// <see cref="System.Reflection.Assembly"/> extensions.
/// </summary>
public static class AssemblyExtensions
{
    /// <summary>
    /// Checks to see if the supplied assembly name is valid for the required assembly name.
    /// </summary>
    /// <param name="assembly">The assembly.</param>
    /// <param name="requiredAssemblyName">The required assembly name.</param>
    /// <returns><see langword="true"/> if <paramref name="assembly"/> is valid for <paramref name="requiredAssemblyName"/>; otherwise <see langword="false"/>.</returns>
    public static bool IsCompatible(this System.Reflection.Assembly assembly, System.Reflection.AssemblyName requiredAssemblyName) => assembly.FullName is { } fullName && new System.Reflection.AssemblyName(fullName).IsCompatible(requiredAssemblyName);

    /// <summary>
    /// Checks to see if the supplied assembly name is valid for the required assembly name.
    /// </summary>
    /// <param name="assemblyName">The assembly name.</param>
    /// <param name="requiredAssemblyName">The required assembly name.</param>
    /// <returns><see langword="true"/> if <paramref name="assemblyName"/> is valid for <paramref name="requiredAssemblyName"/>; otherwise <see langword="false"/>.</returns>
    public static bool IsCompatible(this System.Reflection.AssemblyName assemblyName, System.Reflection.AssemblyName requiredAssemblyName)
    {
        return IsNullOrEqual(requiredAssemblyName.Name).To(assemblyName.Name)
            && IsNullOrLessThanOrEqual(requiredAssemblyName.Version).To(assemblyName.Version)
            && IsNullOrEqual(requiredAssemblyName.CultureName).To(assemblyName.CultureName);

        static CheckValue<Version> IsNullOrLessThanOrEqual(Version? value)
        {
            return IsNullOr(value, static (v, o) => o is not null && v <= o);
        }

        static CheckValue<string> IsNullOrEqual(string? value)
        {
            return IsNullOr(value, static (v, o) => o is not null && string.Equals(v, o, StringComparison.OrdinalIgnoreCase));
        }

        static CheckValue<T> IsNullOr<T>(T? value, Func<T, T?, bool> check)
        {
            return value is null ? CheckValue<T>.True : new(value, check);
        }
    }

    private readonly struct CheckValue<T>(T actual, Func<T, T?, bool> check)
    {
        public static readonly CheckValue<T> True = new(default!, static (_, _) => true);

        private readonly T actual = actual;

        public bool To(T? expected) => check(this.actual, expected);
    }
}