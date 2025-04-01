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
    private const string ToolsDirectory = "tools";

    private static IReadOnlyList<RuntimeFallbacks>? runtimeGraph;

    /// <summary>
    /// Gets the runtime native directory.
    /// </summary>
    /// <returns>The runtime native directory.</returns>
    public static string? GetRuntimeNativeDirectory() => GetRuntimeNativeDirectories().FirstOrDefault();

    /// <summary>
    /// Gets the runtime native directory that contains the specified module name.
    /// </summary>
    /// <param name="name">The module name.</param>
    /// <returns>The directory containing <paramref name="name"/> if found; otherwise <see langword="null"/>.</returns>
    public static string? GetRuntimeNativeDirectory(string name)
    {
        var candidateAssets = GetCandidateAssets(name);

        TryAdd(candidateAssets, CreateAssetPath(RuntimeInformation.RuntimeIdentifier, name), 10001);
        TryAdd(candidateAssets, CreateAssetPath(RuntimeInformation.GetNaïveRid(), name), 10001);

        var probingDirectories = GetProbingDirectories();

        return candidateAssets
            .OrderBy(p => p.Value)
            .Select(p => p.Key.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar))
            .SelectMany(assetPath => probingDirectories.Select(directory => Path.Combine(directory, assetPath)))
            .Where(File.Exists)
            .Select(Path.GetDirectoryName)
            .FirstOrDefault();

        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        static string CreateAssetPath(string rid, string name)
        {
            return $"runtimes{Path.AltDirectorySeparatorChar}{rid}{Path.AltDirectorySeparatorChar}{NativeDirectory}{Path.AltDirectorySeparatorChar}{name}";
        }

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

        static Dictionary<string, int> GetCandidateAssets(string name)
        {
            if (GetDependencyContext() is { RuntimeLibraries: { Count: > 0 } runtimeLibraries })
            {
                var candidateAssets = new Dictionary<string, int>(StringComparer.Ordinal);
                var rids = GetRuntimeRids().ToList();
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
                                TryAdd(candidateAssets, $"{library.Path}{Path.AltDirectorySeparatorChar}{path}", fallbacks);
                            }
                        }
                    }
                }

                return candidateAssets;
            }

            return [];
        }
    }

    /// <summary>
    /// Gets the runtime native directories.
    /// </summary>
    /// <returns>The runtime native directories.</returns>
    public static IEnumerable<string> GetRuntimeNativeDirectories() => GetRuntimeDirectories(NativeDirectory);

    /// <summary>
    /// Gets the runtime library directory.
    /// </summary>
    /// <returns>The runtime library directory.</returns>
    public static string? GetRuntimeLibraryDirectory() => GetRuntimeLibraryDirectories().FirstOrDefault();

    /// <summary>
    /// Gets all possible runtime library directories.
    /// </summary>
    /// <returns>The runtime library directories.</returns>
    public static IEnumerable<string> GetRuntimeLibraryDirectories() => GetRuntimeDirectories(LibraryDirectory).SelectMany(GetFrameworkDirectories);

    /// <summary>
    /// Gets the tools directory that contains the specified tool name.
    /// </summary>
    /// <param name="name">The tool name.</param>
    /// <returns>The directory containing <paramref name="name"/> if found; otherwise <see langword="null"/>.</returns>
    public static string? GetToolDirectory(string name) => GetToolsDirectories().FirstOrDefault(p => File.Exists(Path.Combine(p, name)));

    /// <summary>
    /// Gets the directory containing tools.
    /// </summary>
    /// <returns>The directory containing tools.</returns>
    public static string? GetToolsDirectory() => GetToolsDirectories().FirstOrDefault();

    /// <summary>
    /// Gets all possible tool directories.
    /// </summary>
    /// <returns>The tool directories.</returns>
    public static IEnumerable<string> GetToolsDirectories()
    {
        return FilterProbingPaths(GetProbingDirectoriesCore())
            .Select(static probingDirectory => Path.Combine(probingDirectory, ToolsDirectory))
            .Where(Directory.Exists)
            .SelectMany(GetFrameworkDirectories)
            .SelectMany(GetRidDirectories);

        static IEnumerable<string> GetRidDirectories(string directory)
        {
            if (Directory.GetDirectories(directory)
                .Select(Path.GetFileName)
                .OfType<string>()
                .ToList() is { Count: not 0 } availableRids)
            {
                return GetRuntimeRids()
                    .Intersect(availableRids, GetComparer())
                    .Select(rid => Path.Combine(directory, rid))
                    .Where(Directory.Exists);

                static IEqualityComparer<string?> GetComparer()
                {
#if NET5_0_OR_GREATER
                    return OperatingSystem.IsWindows()
#else
                    return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows)
#endif
                        ? StringComparer.OrdinalIgnoreCase
                        : StringComparer.Ordinal;
                }
            }

            return [];
        }

        static IEnumerable<string> GetProbingDirectoriesCore()
        {
            foreach (var probingDirectory in GetProbingDirectories())
            {
                yield return probingDirectory;
            }

            var context = NuGet.Configuration.NuGetPathContext.Create(RuntimeInformation.GetBaseDirectories().FirstOrDefault() ?? string.Empty);
            if (context.UserPackageFolder is { } userPackageFolder)
            {
                yield return userPackageFolder;
            }

            if (context.FallbackPackageFolders is { Count: not 0 } fallbackPackageFolders)
            {
                foreach (var fallbackPackageFolder in fallbackPackageFolders)
                {
                    yield return fallbackPackageFolder;
                }
            }
        }

        static IEnumerable<string> FilterProbingPaths(IEnumerable<string> probingPaths)
        {
            var libraryPaths = GetDependencyContext()?.RuntimeLibraries?
                .Where(library => library.Type is "package")
                .Select(library => library.Path.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar))
                .ToArray() ?? [];

            foreach (var probingPath in probingPaths)
            {
                // return any probing path that has a tools directory
                yield return probingPath;

                foreach (var libraryPath in libraryPaths)
                {
                    var path = Path.Combine(probingPath, libraryPath);
                    if (Directory.Exists(path))
                    {
                        yield return path;
                    }
                }
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
    public static void AddRuntimeNativeDirectory(EnvironmentVariableTarget target) => AddRuntimeDirectory(GetRuntimeNativeDirectory(), NativeDllSearchDirectories, target);

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
    public static void AddRuntimeNativeDirectory(string moduleName, EnvironmentVariableTarget target) => AddRuntimeDirectory(GetRuntimeNativeDirectory(moduleName), NativeDllSearchDirectories, target);

    /// <summary>
    /// Adds the runtime library directory to the path environment variable if required.
    /// </summary>
    public static void AddRuntimeLibraryDirectory() => AddRuntimeLibraryDirectory(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime library directory to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddRuntimeLibraryDirectory(EnvironmentVariableTarget target) => AddRuntimeDirectory(GetRuntimeLibraryDirectory(), AppPaths, target);

    /// <summary>
    /// Adds the tools directory to the path environment variable if required.
    /// </summary>
    public static void AddToolsDirectory() => AddToolsDirectory(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the tools directory to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddToolsDirectory(EnvironmentVariableTarget target) => AddRuntimeDirectory(GetToolsDirectory(), AppPaths, target);

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
        if (Environment.GetEnvironmentVariable(variable, target) is { } path)
        {
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
        else
        {
            Environment.SetEnvironmentVariable(variable, directory, target);
        }
    }

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddNativeDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => ShouldAddNativeDirectory(path, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddNativeDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path, EnvironmentVariableTarget target) => path is { } p && !IsAlreadyInAppContext(p, NativeDllSearchDirectories) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, target, p);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddLibraryDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => ShouldAddLibraryDirectory(path, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddLibraryDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path, EnvironmentVariableTarget target) => path is { } p && !IsAlreadyInAppContext(p, AppPaths) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, target, p);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddToolsDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => ShouldAddToolsDirectory(path, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddToolsDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path, EnvironmentVariableTarget target) => path is { } p && !EnvironmentVariableContains(RuntimeInformation.PathVariable, target, p);
#else
    /// <summary>
    /// Adds the runtime native directory to the path variable if required.
    /// </summary>
    public static void AddRuntimeNativeDirectory() => AddRuntimeDirectory(GetRuntimeNativeDirectory(), NativeDllSearchDirectories);

    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    /// <param name="moduleName">The module name.</param>
    public static void AddRuntimeNativeDirectory(string moduleName) => AddRuntimeDirectory(GetRuntimeNativeDirectory(moduleName), NativeDllSearchDirectories);

    /// <summary>
    /// Adds the runtime library directory to the path variable if required.
    /// </summary>
    public static void AddRuntimeLibraryDirectory() => AddRuntimeDirectory(GetRuntimeLibraryDirectory(), AppPaths);

    /// <summary>
    /// Adds the tool directory to the path variable if required.
    /// </summary>
    public static void AddToolsDirectory()
    {
        if (GetToolsDirectory() is { } toolsDirectory)
        {
            AddDirectoryToPath(toolsDirectory);
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
        if (Environment.GetEnvironmentVariable(variable) is { } path)
        {
            if (path.Contains(directory))
            {
                return;
            }

            Environment.SetEnvironmentVariable(variable, directory + Path.PathSeparator + path);
        }
        else
        {
            Environment.SetEnvironmentVariable(variable, directory);
        }
    }

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddNativeDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => path is { } p && !IsAlreadyInAppContext(p, NativeDllSearchDirectories) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, p);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddLibraryDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => path is { } p && !IsAlreadyInAppContext(p, AppPaths) && !EnvironmentVariableContains(RuntimeInformation.PathVariable, p);

    /// <summary>
    /// Returns whether the specified path should be added.
    /// </summary>
    /// <param name="path">The path to check.</param>
    /// <returns><see langword="true"/> if <paramref name="path"/> should be added; otherwise <see langword="false"/>.</returns>
    public static bool ShouldAddToolsDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => path is { } p && !EnvironmentVariableContains(RuntimeInformation.PathVariable, p);
#endif

    /// <summary>
    /// Creates the module name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The module name.</returns>
    public static string CreateModuleName(string name) => $"{RuntimeInformation.SharedLibraryPrefix}{name}{RuntimeInformation.SharedLibraryExtension}";

    /// <summary>
    /// Creates the executable name.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <returns>The executable name.</returns>
    public static string CreateExecutableName(string name) => $"{name}{RuntimeInformation.ExecutableExtension}";

    /// <summary>
    /// Gets the runtime config file name.
    /// </summary>
    /// <returns>The runtime config file name.</returns>
    internal static string? GetRuntimeConfigFileName()
    {
        if (Reflection.Assembly.GetEntryAssembly() is not { FullName: { } fullName })
        {
            return default;
        }

        var name = new AssemblyName(fullName);
        var fileName = $"{name.Name}.runtimeconfig.json";

        return RuntimeInformation.GetBaseDirectories().Select(baseDirectory => Path.Combine(baseDirectory, fileName)).FirstOrDefault(File.Exists);
    }

    /// <summary>
    /// Gets the runtime config.
    /// </summary>
    /// <returns>The runtime config.</returns>
    internal static RuntimeConfig? GetRuntimeConfig() => GetRuntimeConfigFileName() is { } path ? RuntimeConfig.FromFile(path) : default;

#if NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    private static void AddRuntimeDirectory(string? directory, string variable, EnvironmentVariableTarget target)
    {
        if (DirectoryExists(directory) && !IsAlreadyInAppContext(directory, variable))
        {
            AddDirectoryToPath(directory, target);
        }
    }
#else
    private static void AddRuntimeDirectory(string? directory, string variable)
    {
        if (DirectoryExists(directory) && !IsAlreadyInAppContext(directory, variable))
        {
            AddDirectoryToPath(directory);
        }
    }
#endif

    private static bool DirectoryExists([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? directory) => Directory.Exists(directory);

#if NETCOREAPP1_0_OR_GREATER || NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER
    private static bool IsAlreadyInAppContext(string value, string variable) => AppContext.GetData(variable) is string data
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
        && data.Contains(value, StringComparison.Ordinal);
#else
        && data.Contains(value);
#endif
#else
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "This is required for a common API")]
    private static bool IsAlreadyInAppContext(string value, string variable) => false;
#endif

    private static IEnumerable<string> GetRuntimeDirectories(string name)
    {
        if (RuntimeInformation.GetBaseDirectories().Select(static baseDirectory => Path.Combine(baseDirectory, RuntimesDirectory)).FirstOrDefault(Directory.Exists) is { } runtimesDirectory)
        {
            // get the rids
            if (Directory.GetDirectories(runtimesDirectory)
                .Select(Path.GetFileName)
                .OfType<string>()
                .ToList() is { Count: not 0 } availableRids)
            {
                if (GetRuntimeRids().Intersect(availableRids, GetPathComparer()).ToList() is { Capacity: not 0 } rids)
                {
                    foreach (var path in rids.Select(rid => Path.Combine(runtimesDirectory, rid, name)).Where(Directory.Exists))
                    {
                        yield return path;
                    }

                    yield break;
                }

                yield return Path.Combine(runtimesDirectory, name);
            }
            else
            {
                yield return runtimesDirectory;
            }
        }
    }

    private static StringComparer GetPathComparer()
    {
#if NET5_0_OR_GREATER
        return OperatingSystem.IsWindows()
#else
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows)
#endif
            ? StringComparer.OrdinalIgnoreCase
            : StringComparer.Ordinal;
    }

    private static List<string> GetRuntimeRids()
    {
        runtimeGraph ??= GetRuntimeGraph();
        if (runtimeGraph.Count > 0)
        {
            var rid = RuntimeInformation.RuntimeIdentifier;
            if (runtimeGraph.FirstOrDefault(g => string.Equals(g.Runtime, rid, StringComparison.Ordinal)) is { } runtimeFallbacks)
            {
                var rids = runtimeFallbacks.Fallbacks.ToList();
                rids.Insert(0, rid);
                return rids;
            }
        }

        return [RuntimeInformation.RuntimeIdentifier, RuntimeInformation.GetNaïveRid()];

        static IReadOnlyList<RuntimeFallbacks> GetRuntimeGraph()
        {
            if (GetDependencyContext() is { RuntimeGraph: { Count: not 0 } runtimeFallbacks })
            {
                return runtimeFallbacks;
            }

            using var stream = new System.IO.Compression.GZipStream(GetManifestStream(!ShouldUseRidGraph()), System.IO.Compression.CompressionMode.Decompress, leaveOpen: false);
            return JsonRuntimeFormat.ReadRuntimeGraph(stream);

            static bool ShouldUseRidGraph()
            {
                return GetRuntimeConfig() switch
                {
                    { } runtimeConfig => UseRidGraph(runtimeConfig),
                    _ => true,
                };

                static bool UseRidGraph(RuntimeConfig runtimeConfig)
                {
                    return runtimeConfig.GetPropertyValue("System.Runtime.Loader.UseRidGraph") is { } value
                        && bool.TryParse(value, out var result)
                        && result;
                }
            }

            static Stream GetManifestStream(bool portable = true)
            {
                return portable
                    ? GetManifestStream("PortableRuntimeIdentifierGraph.json")
                    : GetManifestStream("runtime.json");

                static Stream GetManifestStream(string name)
                {
                    var assembly = typeof(RuntimeEnvironment).GetTypeInfo().Assembly;
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
    private static bool EnvironmentVariableContains(string variable, EnvironmentVariableTarget target, string value) =>
        Environment.GetEnvironmentVariable(variable, target) is { } variableValue
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
            && variableValue.Contains(value, StringComparison.Ordinal);
#else
            && variableValue.Contains(value);
#endif
#else
    private static bool EnvironmentVariableContains(string variable, string value) => Environment.GetEnvironmentVariable(variable) is { } variableValue && variableValue.Contains(value);
#endif

    private static IEnumerable<string> GetFrameworkDirectories(string directory)
    {
        var frameworkNameProvider = NuGet.Frameworks.DefaultFrameworkNameProvider.Instance;

        // get all the available TFMs
        if (Directory.GetDirectories(directory)
            .Select(Path.GetFileName)
            .Select(folder => NuGet.Frameworks.NuGetFramework.ParseFolder(folder, frameworkNameProvider))
            .ToList() is { Count: not 0 } availableFrameworks)
        {
            // get the current TFM
            var targetFramework = NuGet.Frameworks.NuGetFramework.ParseFrameworkName(RuntimeInformation.TargetFramework, frameworkNameProvider);
            var targetFrameworkWithProfile = RuntimeInformation.TargetPlatform is { Length: > 0 } targetPlatform
                ? new NuGet.Frameworks.NuGetFramework(targetFramework.Framework, targetFramework.Version, targetPlatform)
                : default;

            var frameworkReducer = new NuGet.Frameworks.FrameworkReducer(frameworkNameProvider, NuGet.Frameworks.DefaultCompatibilityProvider.Instance);
            while (true)
            {
                if (targetFrameworkWithProfile is { } framework && frameworkReducer.GetNearest(framework, availableFrameworks) is { } nearestFrameworkWithProfile)
                {
                    yield return Path.Combine(directory, nearestFrameworkWithProfile.GetShortFolderName());
                    _ = availableFrameworks.Remove(nearestFrameworkWithProfile);
                }
                else if (frameworkReducer.GetNearest(targetFramework, availableFrameworks) is { } nearestFramework)
                {
                    yield return Path.Combine(directory, nearestFramework.GetShortFolderName());
                    _ = availableFrameworks.Remove(nearestFramework);
                    targetFrameworkWithProfile = default;
                }
                else
                {
                    yield break;
                }
            }
        }

        yield return directory;
    }

    private static string[] GetProbingDirectories()
    {
#if NETCOREAPP2_1_OR_GREATER || NETFRAMEWORK || NETSTANDARD2_0_OR_GREATER
        if (AppDomain.CurrentDomain.GetData("PROBING_DIRECTORIES") is string probingDirectories
            && !string.IsNullOrEmpty(probingDirectories))
        {
            return probingDirectories.Split(Path.PathSeparator);
        }
#endif

        return [.. RuntimeInformation.GetBaseDirectories().Distinct(StringComparer.Ordinal)];
    }
}