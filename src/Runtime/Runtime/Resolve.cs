// -----------------------------------------------------------------------
// <copyright file="Resolve.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

/// <summary>
/// Class to help with assembly resolution.
/// </summary>
public static class Resolve
{
#if NET20_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    private static readonly string[] Extensions = [".dll", ".DLL", ".exe", ".EXE"];
#endif

    /// <summary>
    /// Resolves the specified executable tool.
    /// </summary>
    /// <param name="executable">The executable.</param>
    /// <returns>The tool path if found; otherwise <see langword="null"/>.</returns>
    public static string? Tool(string executable) =>
        InteropServices.RuntimeEnvironment.GetToolDirectory(executable) is { } path
            ? Path.Combine(path, executable)
            : default;

#if NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK || NETCOREAPP2_0_OR_GREATER
    /// <summary>
    /// Resolves runtime assemblies.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static void RuntimeAssemblies() => AppDomain.CurrentDomain.AssemblyResolve += ResolveRuntimeAssembliesHandler;

    /// <summary>
    /// Removes resolution overrides.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    public static void Remove() => AppDomain.CurrentDomain.AssemblyResolve -= ResolveRuntimeAssembliesHandler;

    /// <summary>
    /// Resolves the runtime assembly.
    /// </summary>
    /// <param name="requiredAssemblyName">The required assembly name.</param>
    /// <returns>The required assembly.</returns>
    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    internal static System.Reflection.Assembly? ResolveRuntimeAssembly(System.Reflection.AssemblyName requiredAssemblyName)
    {
        return FromLoaded(requiredAssemblyName) ?? FromPaths(requiredAssemblyName);

        static System.Reflection.Assembly? FromLoaded(System.Reflection.AssemblyName requiredAssemblyName)
        {
            return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), assembly => Reflection.AssemblyExtensions.IsCompatible(assembly, requiredAssemblyName));
        }

        static System.Reflection.Assembly? FromPaths(System.Reflection.AssemblyName requiredAssemblyName)
        {
            return GetPaths()
                .Select(p => FindPath(p, requiredAssemblyName.Name))
                .Where(p => p is not null)
                .Cast<string>()
                .Select(System.Reflection.Assembly.LoadFile)
                .FirstOrDefault(a => Reflection.AssemblyExtensions.IsCompatible(a, requiredAssemblyName));

            static IEnumerable<string?> GetPaths()
            {
                foreach (var directory in InteropServices.RuntimeEnvironment.GetRuntimeLibraryDirectories())
                {
                    yield return directory;
                }

                foreach (var directory in InteropServices.RuntimeInformation.GetBaseDirectories())
                {
                    yield return directory;
                }
            }

            static string? FindPath(string? basePath, string? file)
            {
                return basePath is { } bp && file is { } f
                    ? Extensions.Select(extension => Path.Combine(bp, f + extension)).FirstOrDefault(File.Exists)
                    : default;
            }
        }
    }

    [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("Types and members the loaded assembly depends on might be removed")]
    private static System.Reflection.Assembly? ResolveRuntimeAssembliesHandler(object? sender, ResolveEventArgs args) => ResolveRuntimeAssembly(new System.Reflection.AssemblyName(args.Name));
#endif
}