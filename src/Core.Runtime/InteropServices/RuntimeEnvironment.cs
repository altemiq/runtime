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
    private static readonly string PathVariableName = GetPathVariableName();

    private static IReadOnlyList<RuntimeFallbacks>? runtimeGraph;

    /// <summary>
    /// Gets the runtime native directory.
    /// </summary>
    /// <returns>The runtime native directory.</returns>
    public static string? GetRuntimeNativeDirectory() => GetRuntimeDirectory("native");

    /// <summary>
    /// Gets the runtime library directory.
    /// </summary>
    /// <returns>The runtime library directory.</returns>
    /// <exception cref="InvalidOperationException">Unable to get the current target framework.</exception>
    public static string? GetRuntimeLibraryDirectory()
    {
        var runtimesLibraryDirectory = GetRuntimeDirectory("lib");
        if (runtimesLibraryDirectory is null || !Directory.Exists(runtimesLibraryDirectory))
        {
            return default;
        }

        // get all the tfms
        var availableTfms = Directory.GetDirectories(runtimesLibraryDirectory).Select(Path.GetFileName).ToArray();
        if (availableTfms.Length == 0)
        {
            return runtimesLibraryDirectory;
        }

        // get the closest TFM from the list
        return NuGet.Frameworks.NuGetFrameworkUtility.GetNearest(availableTfms, NuGet.Frameworks.NuGetFramework.ParseFrameworkName(RuntimeInformation.TargetFramework, NuGet.Frameworks.DefaultFrameworkNameProvider.Instance), NuGet.Frameworks.NuGetFramework.ParseFolder) is string nearest
            ? Path.Combine(runtimesLibraryDirectory, nearest)
            : runtimesLibraryDirectory;
    }

#if NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    public static void AddRuntimeNativeDirectory() => AddNativeRuntimeDirectory(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime native directory to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddNativeRuntimeDirectory(EnvironmentVariableTarget target)
    {
        if (GetRuntimeNativeDirectory() is string nativeDirectory
            && Directory.Exists(nativeDirectory)
            && !IsAlreadyInAppContext(nativeDirectory, "NATIVE_DLL_SEARCH_DIRECTORIES"))
        {
            AddDirectoryToPath(nativeDirectory, target);
        }
    }

    /// <summary>
    /// Adds the runtime library directory to the path environment variable if required.
    /// </summary>
    public static void AddRuntimeLibraryDirectory() => AddLibraryRuntimeDirectory(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime library directory to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddLibraryRuntimeDirectory(EnvironmentVariableTarget target)
    {
        if (GetRuntimeLibraryDirectory() is string libraryDirectory
            && Directory.Exists(libraryDirectory)
            && !IsAlreadyInAppContext(libraryDirectory, "APP_PATHS"))
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
        AddLibraryRuntimeDirectory(target);
        AddNativeRuntimeDirectory(target);
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
    public static void AddDirectoryToPath(string directory, EnvironmentVariableTarget target)
    {
        var path = Environment.GetEnvironmentVariable(PathVariableName, target);
        if (path is null)
        {
            Environment.SetEnvironmentVariable(PathVariableName, directory);
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

        Environment.SetEnvironmentVariable(PathVariableName, directory + Path.PathSeparator + path, target);
    }
#else
    /// <summary>
    /// Adds the runtime native directory to the path variable if required.
    /// </summary>
    public static void AddRuntimeNativeDirectory()
    {
        if (GetRuntimeNativeDirectory() is string nativeDirectory
            && Directory.Exists(nativeDirectory)
            && !IsAlreadyInAppContext(nativeDirectory, "NATIVE_DLL_SEARCH_DIRECTORIES"))
        {
            AddDirectoryToPath(nativeDirectory);
        }
    }

    /// <summary>
    /// Adds the runtime library directory to the path variable if required.
    /// </summary>
    public static void AddLibraryRuntimeDirectory()
    {
        if (GetRuntimeLibraryDirectory() is string libraryDirectory
            && Directory.Exists(libraryDirectory)
            && !IsAlreadyInAppContext(libraryDirectory, "APP_PATHS"))
        {
            AddDirectoryToPath(libraryDirectory);
        }
    }

    /// <summary>
    /// Adds the runtime directories to the path variable if required.
    /// </summary>
    public static void AddRuntimeDirectories()
    {
        AddLibraryRuntimeDirectory();
        AddRuntimeNativeDirectory();
    }

    /// <summary>
    /// Adds a directory to the path environment variable.
    /// </summary>
    /// <param name="directory">The directory to add.</param>
    public static void AddDirectoryToPath(string directory)
    {
        var path = Environment.GetEnvironmentVariable(PathVariableName);
        if (path is null)
        {
            Environment.SetEnvironmentVariable(PathVariableName, directory);
            return;
        }

        if (path.Contains(directory))
        {
            return;
        }

        Environment.SetEnvironmentVariable(PathVariableName, directory + Path.PathSeparator + path);
    }
#endif

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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Major Code Smell", "S1172:Unused method parameters should be removed", Justification = "This is required for a common API")]
    private static bool IsAlreadyInAppContext(string value, string variable) => false;
#endif

    private static string GetPathVariableName()
    {
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            return "PATH";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            return "LD_LIBRARY_PATH";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
        {
            return "DYLD_LIBRARY_PATH";
        }

        throw new InvalidOperationException();
    }

    private static string? GetRuntimeDirectory(string name)
    {
        var baseDirectory =
#if NET451
            AppDomain.CurrentDomain.BaseDirectory;
#else
            AppContext.BaseDirectory;
#endif
        var runtimesDirectory = Path.Combine(baseDirectory, "runtimes");
        if (!Directory.Exists(runtimesDirectory))
        {
            return default;
        }

        // get the rids
        var availableRids = Directory.GetDirectories(runtimesDirectory).Select(Path.GetFileName).ToArray();
        if (availableRids.Length == 0)
        {
            return runtimesDirectory;
        }

        if (GetRuntimeRids().FirstOrDefault(availableRids.Contains) is string rid)
        {
            return Path.Combine(runtimesDirectory, rid, name);
        }

        return Path.Combine(runtimesDirectory, name);

        static IEnumerable<string> GetRuntimeRids()
        {
            runtimeGraph ??= GetRuntimeGraph();
            if (runtimeGraph.Count > 0)
            {
                var rid = RuntimeInformation.RuntimeIdentifier;
                var rids = runtimeGraph.First(g => string.Equals(g.Runtime, rid, StringComparison.Ordinal)).Fallbacks.ToList();
                rids.Insert(0, rid);
                return rids;
            }

            return Enumerable.Empty<string>();

            static IReadOnlyList<RuntimeFallbacks> GetRuntimeGraph()
            {
                DependencyContext? dependencyContext;
                try
                {
                    dependencyContext = DependencyContext.Default;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Fail(ex.ToString());
                    dependencyContext = null;
                }

                if (dependencyContext?.RuntimeGraph is IReadOnlyList<RuntimeFallbacks> runtimeFallbacks && runtimeFallbacks.Count != 0)
                {
                    return runtimeFallbacks;
                }

                using var stream = new System.IO.Compression.GZipStream(GetManifestStream(), System.IO.Compression.CompressionMode.Decompress, leaveOpen: false);
                return JsonRuntimeFormat.ReadRuntimeGraph(stream).ToList();

                static Stream GetManifestStream()
                {
                    const string RuntimeJson = "runtime.json";
                    var assembly = typeof(RuntimeInformation).GetTypeInfo().Assembly;
                    return assembly.GetManifestResourceStream(assembly.GetManifestResourceNames()
#if NETCOREAPP2_1_OR_GREATER || NETSTANDARD2_1_OR_GREATER
                        .First(n => n.Contains(RuntimeJson, StringComparison.Ordinal)))!;
#else
                        .First(n => n.Contains(RuntimeJson)))!;
#endif
                }
            }
        }
    }
}