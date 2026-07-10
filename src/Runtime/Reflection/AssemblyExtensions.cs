// -----------------------------------------------------------------------
// <copyright file="AssemblyExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable CA1050, CA1708, MA0047, RCS1110, RCS1263, SA1101, CheckNamespace

/// <summary>
/// <see cref="System.Reflection.Assembly"/> extensions.
/// </summary>
public static class AssemblyExtensions
{
    extension(System.Reflection.Assembly)
    {
        /// <summary>
        /// Gets the entry assembly, taking into account <c>testhost</c> to make sure we get the required assembly.
        /// </summary>
        /// <returns>The entry assembly.</returns>
        public static System.Reflection.Assembly? GetActualEntryAssembly()
#if NETCOREAPP2_0_OR_GREATER || NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
        {
            return entryAssembly ??= GetEntryAssemblyCore();

            static System.Reflection.Assembly? GetEntryAssemblyCore()
            {
                if (System.Reflection.Assembly.GetEntryAssembly() is { FullName: { } fullName } assembly)
                {
                    return IsTestAssembly(fullName) && GetTestAssembly() is { } testAssembly
                        ? testAssembly
                        : assembly;
                }
                else if (System.Diagnostics.Process.GetCurrentProcess() is { } process
                    && IsTestAssembly(process.ProcessName)
                    && GetTestAssembly() is { } testAssembly)
                {
                    return testAssembly;
                }

                return default;

                static bool IsTestAssembly(string name)
                {
                    return name.Contains("testhost", StringComparison.OrdinalIgnoreCase);
                }

                static System.Reflection.Assembly? GetTestAssembly()
                {
                    return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), static a => a.GetName().Name?.EndsWith(".Tests", StringComparison.OrdinalIgnoreCase) is true);
                }
            }
        }
#else
        => entryAssembly ??= System.Reflection.Assembly.GetEntryAssembly();
#endif
    }

    /// <content>
    /// <see cref="System.Reflection.Assembly"/> extensions.
    /// </content>
    /// <param name="assembly">The assembly.</param>
    extension(System.Reflection.Assembly assembly)
    {
        /// <summary>
        /// Checks to see if the supplied assembly name is valid for the required assembly name.
        /// </summary>
        /// <param name="requiredAssemblyName">The required assembly name.</param>
        /// <returns><see langword="true"/> if this instance is valid for <paramref name="requiredAssemblyName"/>; otherwise <see langword="false"/>.</returns>
        public bool IsCompatible(System.Reflection.AssemblyName requiredAssemblyName) => assembly.FullName is { } fullName && new System.Reflection.AssemblyName(fullName).IsCompatible(requiredAssemblyName);
    }

    /// <content>
    /// <see cref="System.Reflection.AssemblyName"/> extensions.
    /// </content>
    /// <param name="assemblyName">The assembly name.</param>
    extension(System.Reflection.AssemblyName assemblyName)
    {
        /// <summary>
        /// Checks to see if the supplied assembly name is valid for the required assembly name.
        /// </summary>
        /// <param name="requiredAssemblyName">The required assembly name.</param>
        /// <returns><see langword="true"/> if this instance is valid for <paramref name="requiredAssemblyName"/>; otherwise <see langword="false"/>.</returns>
        public bool IsCompatible(System.Reflection.AssemblyName requiredAssemblyName)
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
    }

    private static System.Reflection.Assembly? entryAssembly;

    private readonly struct CheckValue<T>(T actual, Func<T, T?, bool> check)
    {
        public static readonly CheckValue<T> True = new(default!, static (_, _) => true);

        public bool To(T? expected) => check(actual, expected);
    }
}