// -----------------------------------------------------------------------
// <copyright file="Resolve.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if NETSTANDARD2_0_OR_GREATER || NET20_OR_GREATER || NETCOREAPP2_0_OR_GREATER
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

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S3885:\"Assembly.Load\" should be used", Justification = "We are trying to load an exact path")]
    private static System.Reflection.Assembly? ResolveRuntimeAssembliesHandler(object? sender, ResolveEventArgs args)
    {
        System.Reflection.AssemblyName requiredAssemblyName = new(args.Name);

        // check to see if this is in the currently loaded assemblies
        if (FromLoaded(requiredAssemblyName) is System.Reflection.Assembly alreadyLoadedAssembly)
        {
            return alreadyLoadedAssembly;
        }

        if (GetPaths()
            .Select(p => FindPath(p, requiredAssemblyName.Name))
            .FirstOrDefault(p => p is not null) is string path)
        {
            var assembly = System.Reflection.Assembly.LoadFile(path);
            if (assembly.IsCompatible(requiredAssemblyName))
            {
                return System.Reflection.Assembly.LoadFrom(assembly.Location);
            }
        }

        return default;

        static System.Reflection.Assembly? FromLoaded(System.Reflection.AssemblyName requiredAssemblyName)
        {
            return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), assembly => assembly.IsCompatible(requiredAssemblyName));
        }

        static IEnumerable<string?> GetPaths()
        {
            yield return InteropServices.RuntimeEnvironment.GetRuntimeLibraryDirectory();
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
#endif