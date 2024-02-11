// -----------------------------------------------------------------------
// <copyright file="Assembly.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Reflection;

/// <summary>
/// Extension methods for <see cref="System.Reflection.Assembly"/>.
/// </summary>
internal sealed class Assembly
{
    private static System.Reflection.Assembly? entryAssembly;

    private Assembly()
    {
    }

    /// <summary>
    /// Gets the entry assembly, taking into account <c>testhost</c> to make sure we get the required assembly.
    /// </summary>
    /// <returns>The entry assembly.</returns>
    internal static System.Reflection.Assembly? GetEntryAssembly()
#if NETCOREAPP2_0_OR_GREATER || NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
    {
        return entryAssembly ??= GetEntryAssemblyCore();

        static System.Reflection.Assembly? GetEntryAssemblyCore()
        {
            if (System.Reflection.Assembly.GetEntryAssembly() is { } assembly)
            {
                if (assembly.FullName is { } fullName
                    && IsTestAssembly(fullName)
                    && GetTestAssembly() is { } testAssembly)
                {
                    return testAssembly;
                }

                return assembly;
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
                const string TestHost = "testhost";

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                return name.Contains(TestHost, StringComparison.OrdinalIgnoreCase);
#else
                return name.IndexOf(TestHost, StringComparison.OrdinalIgnoreCase) >= 0;
#endif
            }

            static System.Reflection.Assembly? GetTestAssembly()
            {
                return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), a => a.GetName().Name?.EndsWith(".Tests", StringComparison.OrdinalIgnoreCase) == true);
            }
        }
    }
#else
        => entryAssembly ??= System.Reflection.Assembly.GetEntryAssembly();
#endif
}