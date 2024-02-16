// -----------------------------------------------------------------------
// <copyright file="Resolve.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

using Altemiq.Reflection;

/// <summary>
/// Class to help with assembly resolution.
/// </summary>
public static class Resolve
{
    private static readonly string[] Extensions = [".dll", ".DLL", ".exe", ".EXE"];

    /// <summary>
    /// Resolves runtime assemblies.
    /// </summary>
    public static void RuntimeAssemblies() => AppDomain.CurrentDomain.AssemblyResolve += ResolveRuntimeAssembliesHandler;

    /// <summary>
    /// Removes resolution overrides.
    /// </summary>
    public static void Remove() => AppDomain.CurrentDomain.AssemblyResolve -= ResolveRuntimeAssembliesHandler;

    /// <summary>
    /// Resolves the runtime assembly.
    /// </summary>
    /// <param name="requiredAssemblyName">The required assembly name.</param>
    /// <returns>The required assembly.</returns>
    internal static System.Reflection.Assembly? ResolveRuntimeAssembly(System.Reflection.AssemblyName requiredAssemblyName)
    {
        return FromLoaded(requiredAssemblyName) ?? FromPaths(requiredAssemblyName);

        static System.Reflection.Assembly? FromLoaded(System.Reflection.AssemblyName requiredAssemblyName)
        {
            return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), assembly => assembly.IsCompatible(requiredAssemblyName));
        }

        static System.Reflection.Assembly? FromPaths(System.Reflection.AssemblyName requiredAssemblyName)
        {
            return GetPaths()
                .Select(p => FindPath(p, requiredAssemblyName.Name))
                .Where(p => p is not null)
                .Cast<string>()
                .Select(System.Reflection.Assembly.LoadFile)
                .FirstOrDefault(a => a.IsCompatible(requiredAssemblyName));

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
                return basePath is not null && file is not null
                    ? Extensions.Select(extension => Path.Combine(basePath, file + extension)).FirstOrDefault(File.Exists)
                    : default;
            }
        }
    }

    private static System.Reflection.Assembly? ResolveRuntimeAssembliesHandler(object? sender, ResolveEventArgs args) => ResolveRuntimeAssembly(new System.Reflection.AssemblyName(args.Name));
}