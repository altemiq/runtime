// -----------------------------------------------------------------------
// <copyright file="RuntimeEnvironment.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

using System.Reflection;
using Microsoft.Extensions.DependencyModel;

/// <summary>
/// Class for runtime configuration.
/// </summary>
public static class RuntimeEnvironment
{
    private const string NativeDllSearchDirectories = "NATIVE_DLL_SEARCH_DIRECTORIES";
    private const string AppPaths = "APP_PATHS";
    private const string RuntimesDirectory = "runtimes";
    private const string NativeDirectory = "native";
    private const string LibraryDirectory = "lib";

    private static IReadOnlyList<RuntimeFallbacks>? runtimeGraph;

    /// <summary>
    /// Gets the runtime native directory.
    /// </summary>
    /// <returns>The runtime native directory.</returns>
    public static string? GetRuntimeNativeDirectory() => GetRuntimeDirectory(NativeDirectory);

    /// <summary>
    /// Gets the runtime native directory that contains the specified module name.
    /// </summary>
    /// <param name="name">The module name.</param>
    /// <returns>The directory containing <paramref name="name"/> if found; otherwise <see langword="null"/>.</returns>
    public static string? GetRuntimeNativeDirectory(string name)
    {
        var candidateAssets = new Dictionary<string, int>(StringComparer.Ordinal);
        var rids = GetRuntimeRids().ToList();

        if (GetDependencyContext() is { } dependencyContext && dependencyContext.RuntimeLibraries is { Count: > 0 } runtimeLibraries)
        {
            foreach (var library in runtimeLibraries)
            {
                foreach (var group in library.NativeLibraryGroups)
                {
                    foreach (var path in group
                        .RuntimeFiles
                        .Where(runtimeFile => string.Equals(
                            Path.GetFileName(runtimeFile.Path),
                            name,
                            StringComparison.OrdinalIgnoreCase))
                        .Select(runtimeFile => runtimeFile.Path))
                    {
                        var fallbacks = rids.IndexOf(group.Runtime);
                        if (fallbacks is not -1)
                        {
                            TryAdd(candidateAssets, library.Path + "/" + path, fallbacks);
                        }
                    }
                }
            }
        }

        TryAdd(candidateAssets, CreateAssetPath(RuntimeInformation.RuntimeIdentifier, name), 10001);
        TryAdd(candidateAssets, CreateAssetPath(RuntimeInformation.GetNaïveRid(), name), 10001);

#if NETCOREAPP2_1_OR_GREATER || NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
        var probingDirectories = ((string?)AppDomain.CurrentDomain.GetData("PROBING_DIRECTORIES"))?.Split(Path.PathSeparator) ?? RuntimeInformation.GetBaseDirectories().Distinct(StringComparer.Ordinal).ToArray();
        ReplaceEmpty(probingDirectories, AppDomain.CurrentDomain.BaseDirectory);
#else
        var probingDirectories = RuntimeInformation.GetBaseDirectories().Distinct(StringComparer.Ordinal).ToArray();
#endif
        foreach (var assetPath in candidateAssets
           .OrderBy(p => p.Value)
           .Select(p => p.Key.Replace('/', Path.DirectorySeparatorChar)))
        {
            var assetFullPath = probingDirectories
                .Select(directory => Path.Combine(directory, assetPath))
                .LastOrDefault(File.Exists);

            if (assetFullPath is not null)
            {
                return Path.GetDirectoryName(assetFullPath);
            }
        }

        return default;

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static string CreateAssetPath(string rid, string name)
        {
            return $"runtimes/{rid}/native/{name}";
        }

#if NETCOREAPP2_1_OR_GREATER || NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
        static void ReplaceEmpty(IList<string> list, string value)
        {
            for (var i = 0; i < list.Count; i++)
            {
                if (string.IsNullOrEmpty(list[i]))
                {
                    list[i] = value;
                }
            }
        }
#endif

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static void TryAdd<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, TValue value)
            where TKey : notnull
        {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_0_OR_GREATER
            dictionary.TryAdd(key, value);
#else
            if (!dictionary.ContainsKey(key))
            {
                dictionary.Add(key, value);
            }
#endif
        }
    }

    /// <summary>
    /// Gets the runtime library directory.
    /// </summary>
    /// <returns>The runtime library directory.</returns>
    public static string? GetRuntimeLibraryDirectory() => GetRuntimeLibraryDirectories().FirstOrDefault();

    /// <summary>
    /// Gets all possible runtime library directories.
    /// </summary>
    /// <returns>The runtime library directories.</returns>
    public static IEnumerable<string> GetRuntimeLibraryDirectories()
    {
        var runtimesLibraryDirectory = GetRuntimeDirectory(LibraryDirectory);
        if (runtimesLibraryDirectory is null || !Directory.Exists(runtimesLibraryDirectory))
        {
            yield break;
        }

        var frameworkNameProvider = NuGet.Frameworks.DefaultFrameworkNameProvider.Instance;

        // get all the available TFMs
        var availableFramework = Directory.GetDirectories(runtimesLibraryDirectory)
            .Select(Path.GetFileName)
            .Select(folder => NuGet.Frameworks.NuGetFramework.ParseFolder(folder, frameworkNameProvider))
            .ToList();
        if (availableFramework.Count is 0)
        {
            yield return runtimesLibraryDirectory;
            yield break;
        }

        // get the current TFM
        var currentFramework = NuGet.Frameworks.NuGetFramework.ParseFrameworkName(RuntimeInformation.TargetFramework, frameworkNameProvider);
        var currentFrameworkWithProfile = RuntimeInformation.TargetPlatform is { Length: > 0 } targetPlatform
            ? new NuGet.Frameworks.NuGetFramework(currentFramework.Framework, currentFramework.Version, targetPlatform)
            : default;

        var frameworkReducer = new NuGet.Frameworks.FrameworkReducer(frameworkNameProvider, NuGet.Frameworks.DefaultCompatibilityProvider.Instance);
        while (true)
        {
            if (currentFrameworkWithProfile is not null && frameworkReducer.GetNearest(currentFrameworkWithProfile, availableFramework) is { } nearestFrameworkWithProfile)
            {
                yield return Path.Combine(runtimesLibraryDirectory, nearestFrameworkWithProfile.GetShortFolderName());
                availableFramework.Remove(nearestFrameworkWithProfile);
            }
            else if (frameworkReducer.GetNearest(currentFramework, availableFramework) is { } nearestFramework)
            {
                yield return Path.Combine(runtimesLibraryDirectory, nearestFramework.GetShortFolderName());
                availableFramework.Remove(nearestFramework);
                currentFrameworkWithProfile = default;
            }
            else
            {
                yield break;
            }
        }
    }

#if NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    public static void AddRuntimeNativeDirectory() => AddRuntimeNativeDirectory(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddRuntimeNativeDirectory(EnvironmentVariableTarget target)
    {
        if (GetRuntimeNativeDirectory() is string nativeDirectory
            && Directory.Exists(nativeDirectory)
            && !IsAlreadyInAppContext(nativeDirectory, NativeDllSearchDirectories))
        {
            AddDirectoryToPath(nativeDirectory, target);
        }
    }

    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    /// <param name="moduleName">The module name.</param>
    public static void AddRuntimeNativeDirectory(string moduleName) => AddRuntimeNativeDirectory(moduleName, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    /// <param name="moduleName">The module name.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddRuntimeNativeDirectory(string moduleName, EnvironmentVariableTarget target)
    {
        if (GetRuntimeNativeDirectory(moduleName) is string nativeDirectory
            && Directory.Exists(nativeDirectory)
            && !IsAlreadyInAppContext(nativeDirectory, NativeDllSearchDirectories))
        {
            AddDirectoryToPath(nativeDirectory, target);
        }
    }

    /// <summary>
    /// Adds the runtime library directory to the path environment variable if required.
    /// </summary>
    public static void AddRuntimeLibraryDirectory() => AddRuntimeLibraryDirectory(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime library directory to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddRuntimeLibraryDirectory(EnvironmentVariableTarget target)
    {
        if (GetRuntimeLibraryDirectory() is string libraryDirectory
            && Directory.Exists(libraryDirectory)
            && !IsAlreadyInAppContext(libraryDirectory, AppPaths))
        {
            AddDirectoryToPath(libraryDirectory, target);
        }
    }

    /// <summary>
    /// Adds the runtime directories to the path variable if required.
    /// </summary>
    public static void AddRuntimeDirectories() => AddRuntimeDirectories(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime directories to the path variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddRuntimeDirectories(EnvironmentVariableTarget target)
    {
        AddRuntimeLibraryDirectory(target);
        AddRuntimeNativeDirectory(target);
    }

    /// <summary>
    /// Adds a directory to the path environment variable in the current process.
    /// </summary>
    /// <param name="directory">The directory to add.</param>
    public static void AddDirectoryToPath(string directory) => AddDirectoryToPath(directory, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds a directory to the path environment variable in the current process or from the Windows operating system registry key for the current user or local machine.
    /// </summary>
    /// <param name="directory">The directory to add.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddDirectoryToPath(string directory, EnvironmentVariableTarget target) => AddDirectory(directory, RuntimeInformation.PathVariable, target);

    /// <summary>
    /// Adds a directory to the environment variable in the current process.
    /// </summary>
    /// <param name="directory">The directory to add.</param>
    /// <param name="variable">The name of an environment variable.</param>
    public static void AddDirectory(string directory, string variable) => AddDirectory(directory, variable, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds a directory to the environment variable in the current process or from the Windows operating system registry key for the current user or local machine.
    /// </summary>
    /// <param name="directory">The directory to add.</param>
    /// <param name="variable">The name of an environment variable.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddDirectory(string directory, string variable, EnvironmentVariableTarget target)
    {
        var path = Environment.GetEnvironmentVariable(variable, target);
        if (path is null)
        {
            Environment.SetEnvironmentVariable(variable, directory);
            return;
        }

#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        if (path.Contains(directory, StringComparison.Ordinal))
#else
        if (path.Contains(directory))
#endif
        {
            return;
        }

        Environment.SetEnvironmentVariable(variable, directory + Path.PathSeparator + path, target);
    }

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddNativeDirectory(string? path) => ShouldAddNativeDirectory(path, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddNativeDirectory(string? path, EnvironmentVariableTarget target) => path is not null && !IsAlreadyInAppContext(path, NativeDllSearchDirectories) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, target, path);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddLibraryDirectory(string? path) => ShouldAddLibraryDirectory(path, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddLibraryDirectory(string? path, EnvironmentVariableTarget target) => path is not null && !IsAlreadyInAppContext(path, AppPaths) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, target, path);
#else
    /// <summary>
    /// Adds the runtime native directory to the path variable if required.
    /// </summary>
    public static void AddRuntimeNativeDirectory()
    {
        if (GetRuntimeNativeDirectory() is string nativeDirectory
            && Directory.Exists(nativeDirectory)
            && !IsAlreadyInAppContext(nativeDirectory, NativeDllSearchDirectories))
        {
            AddDirectoryToPath(nativeDirectory);
        }
    }

    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    /// <param name="moduleName">The module name.</param>
    public static void AddRuntimeNativeDirectory(string moduleName)
    {
        if (GetRuntimeNativeDirectory(moduleName) is string nativeDirectory
            && Directory.Exists(nativeDirectory)
            && !IsAlreadyInAppContext(nativeDirectory, NativeDllSearchDirectories))
        {
            AddDirectoryToPath(nativeDirectory);
        }
    }

    /// <summary>
    /// Adds the runtime library directory to the path variable if required.
    /// </summary>
    public static void AddRuntimeLibraryDirectory()
    {
        if (GetRuntimeLibraryDirectory() is string libraryDirectory
            && Directory.Exists(libraryDirectory)
            && !IsAlreadyInAppContext(libraryDirectory, AppPaths))
        {
            AddDirectoryToPath(libraryDirectory);
        }
    }

    /// <summary>
    /// Adds the runtime directories to the path variable if required.
    /// </summary>
    public static void AddRuntimeDirectories()
    {
        AddRuntimeLibraryDirectory();
        AddRuntimeNativeDirectory();
    }

    /// <summary>
    /// Adds a directory to the path environment variable.
    /// </summary>
    /// <param name="directory">The directory to add.</param>
    public static void AddDirectoryToPath(string directory) => AddDirectory(directory, RuntimeInformation.PathVariable);

    /// <summary>
    /// Adds a directory to the path variable.
    /// </summary>
    /// <param name="directory">The directory to add.</param>
    /// <param name="variable">The name of an environment variable.</param>
    public static void AddDirectory(string directory, string variable)
    {
        var path = Environment.GetEnvironmentVariable(variable);
        if (path is null)
        {
            Environment.SetEnvironmentVariable(variable, directory);
            return;
        }

        if (path.Contains(directory))
        {
            return;
        }

        Environment.SetEnvironmentVariable(variable, directory + Path.PathSeparator + path);
    }

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddNativeDirectory(string? path) => path is not null && !IsAlreadyInAppContext(path, NativeDllSearchDirectories) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, path);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddLibraryDirectory(string? path) => path is not null && !IsAlreadyInAppContext(path, AppPaths) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, path);
#endif

    /// <summary>
    /// Creates the module name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The module name.</returns>
    public static string CreateModuleName(string name) => $"{RuntimeInformation.SharedLibraryPrefix}{name}{RuntimeInformation.SharedLibraryExtension}";

    /// <summary>
    /// Gets the runtime config file name.
    /// </summary>
    /// <returns>The runtime config file name.</returns>
    internal static string? GetRuntimeConfigFileName()
    {
        const string RuntimeConfigJson = "runtimeconfig.json";

        if (Reflection.Assembly.GetEntryAssembly() is { } assembly
            && assembly.FullName is { } fullName)
        {
            var name = new AssemblyName(fullName);
            var fileName = $"{name.Name}.{RuntimeConfigJson}";

            return RuntimeInformation.GetBaseDirectories().Select(baseDirectory => Path.Combine(baseDirectory, fileName)).FirstOrDefault(File.Exists);
        }

        return default;
    }

    /// <summary>
    /// Gets the runtime config.
    /// </summary>
    /// <returns>The runtime config.</returns>
    internal static RuntimeConfig? GetRuntimeConfig() => GetRuntimeConfigFileName() is { } path ? RuntimeConfig.FromFile(path) : default;

#if NETCOREAPP1_0_OR_GREATER || NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER
    private static bool IsAlreadyInAppContext(string value, string variable) => AppContext.GetData(variable) is string data
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        && data.Contains(value, StringComparison.Ordinal);
#else
        && data.Contains(value);
#endif
#else
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1163:Unused parameter.", Justification = "This is required for a common API")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is required for a common API")]
    private static bool IsAlreadyInAppContext(string value, string variable) => false;
#endif

    private static string? GetRuntimeDirectory(string name)
    {
        if (RuntimeInformation.GetBaseDirectories().Select(baseDirectory => Path.Combine(baseDirectory, RuntimesDirectory)).FirstOrDefault(Directory.Exists) is { } runtimesDirectory)
        {
            // get the rids
            return Directory.GetDirectories(runtimesDirectory).Select(Path.GetFileName).ToList() switch
            {
                { Count: 0 } => runtimesDirectory,
                var availableRids when GetRuntimeRids().Find(availableRids.Contains) is string rid => Path.Combine(runtimesDirectory, rid, name),
                _ => Path.Combine(runtimesDirectory, name),
            };
        }

        return default;
    }

    private static List<string> GetRuntimeRids()
    {
        runtimeGraph ??= GetRuntimeGraph();
        if (runtimeGraph.Count > 0)
        {
            var rid = RuntimeInformation.RuntimeIdentifier;
            var rids = runtimeGraph.First(g => string.Equals(g.Runtime, rid, StringComparison.Ordinal)).Fallbacks.ToList();
            rids.Insert(0, rid);
            return rids;
        }

        return [];

        static IReadOnlyList<RuntimeFallbacks> GetRuntimeGraph()
        {
            if (GetDependencyContext() is { } dependencyContext && dependencyContext.RuntimeGraph is IReadOnlyList<RuntimeFallbacks> runtimeFallbacks && runtimeFallbacks.Count != 0)
            {
                return runtimeFallbacks;
            }

            using var stream = new System.IO.Compression.GZipStream(GetManifestStream(!ShouldUseRidGraph()), System.IO.Compression.CompressionMode.Decompress, leaveOpen: false);
            return JsonRuntimeFormat.ReadRuntimeGraph(stream);

            static bool ShouldUseRidGraph()
            {
                if (GetRuntimeConfig() is { } runtimeConfig)
                {
                    if (runtimeConfig.GetPropertyValue("System.Runtime.Loader.UseRidGraph") is { } value && bool.TryParse(value, out var result))
                    {
                        return result;
                    }

                    // if we have a runtime config, then return false
                    return false;
                }

                // no runtime config to read, then return true
                return true;
            }

            static Stream GetManifestStream(bool portable = true)
            {
                return portable
                    ? GetManifestStream("PortableRuntimeIdentifierGraph.json")
                    : GetManifestStream("runtime.json");

                static Stream GetManifestStream(string name)
                {
                    var assembly = typeof(RuntimeInformation).GetTypeInfo().Assembly;
                    return assembly.GetManifestResourceStream(assembly.GetManifestResourceNames()
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                        .First(n => n.Contains(name, StringComparison.Ordinal)))!;
#else
                            .First(n => n.Contains(name)))!;
#endif
                }
            }
        }
    }

    private static DependencyContext? GetDependencyContext()
    {
        try
        {
            return DependencyContext.Default;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Fail(ex.ToString());
        }

        return default;
    }

#if NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    private static bool EnvironmentVariableContains(string variable, EnvironmentVariableTarget target, string? value)
    {
        if (value is null)
        {
            return false;
        }

        return Environment.GetEnvironmentVariable(variable, target) is { } variableValue
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_2_OR_GREATER
            && variableValue.Contains(value, StringComparison.Ordinal);
#else
            && variableValue.Contains(value);
#endif
    }
#else
    private static bool EnvironmentVariableContains(string variable, string? value)
    {
        if (value is null)
        {
            return false;
        }

        return Environment.GetEnvironmentVariable(variable) is { } variableValue
            && variableValue.Contains(value);
    }
#endif
}