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
            if (IsValid(assembly.GetName(), requiredAssemblyName))
            {
                return System.Reflection.Assembly.LoadFrom(assembly.Location);
            }
        }

        return default;

        static System.Reflection.Assembly? FromLoaded(System.Reflection.AssemblyName assemblyName)
        {
            return Array.Find(AppDomain.CurrentDomain.GetAssemblies(), assembly => IsValid(assembly.GetName(), assemblyName));
        }

        static bool IsValid(System.Reflection.AssemblyName assemblyName, System.Reflection.AssemblyName requiredAssemblyName)
        {
            return IsNullOrEqual(assemblyName.Name, requiredAssemblyName.Name)
                && IsNullOrLess(assemblyName.Version, requiredAssemblyName.Version)
                && IsNullOrEqual(assemblyName.CultureName, requiredAssemblyName.CultureName);

            static bool IsNullOrLess(Version? value, Version? other)
            {
                return IsNullOr(value, other, (v, o) => v >= o);
            }

            static bool IsNullOrEqual(string? value, string? other)
            {
                return IsNullOr(value, other, (v, o) => string.Equals(v, o, StringComparison.OrdinalIgnoreCase));
            }

            static bool IsNullOr<T>(T? value, T? other, Func<T, T, bool> check)
            {
                return other is null || (value is not null && check(value, other));
            }
        }

        static IEnumerable<string?> GetPaths()
        {
            yield return InteropServices.RuntimeEnvironment.GetRuntimeLibraryDirectory();
#if NETCOREAPP1_0_OR_GREATER || NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER
            yield return AppContext.BaseDirectory;
#endif
#if NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER || NETSTANDARD2_0_OR_GREATER
            yield return AppDomain.CurrentDomain.BaseDirectory;
#endif
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