// -----------------------------------------------------------------------
// <copyright file="Resolve.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if NETSTANDARD2_0_OR_GREATER || NET20_OR_GREATER || NETCOREAPP2_0_OR_GREATER
namespace Altemiq.Runtime;

/// <summary>
/// Class to help with assembly resolution.
/// </summary>
public static class Resolve
{
    private static readonly IEnumerable<string> Extensions = new[] { ".dll", ".DLL", ".exe", ".EXE" };

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
        if (FindPath(InteropServices.RuntimeInformation.GetRuntimeLibraryPath(), requiredAssemblyName.Name) is string path)
        {
            var assembly = System.Reflection.Assembly.LoadFile(path);
            var assemblyName = assembly.GetName();
            if (string.Equals(assemblyName.Name, requiredAssemblyName.Name, StringComparison.OrdinalIgnoreCase)
                && assemblyName.Version >= requiredAssemblyName.Version
                && string.Equals(assemblyName.CultureName, requiredAssemblyName.CultureName, StringComparison.OrdinalIgnoreCase))
            {
                return System.Reflection.Assembly.LoadFrom(path);
            }
        }

        return default;

        static string? FindPath(string? basePath, string? file)
        {
            return basePath is null || file is null
                ? default
                : Extensions.Select(extension => Path.Combine(basePath, file + extension)).FirstOrDefault(File.Exists);
        }
    }
}
#endif