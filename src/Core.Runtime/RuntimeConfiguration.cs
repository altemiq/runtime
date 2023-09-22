// -----------------------------------------------------------------------
// <copyright file="RuntimeConfiguration.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

/// <summary>
/// Class for runtime configuration.
/// </summary>
public static class RuntimeConfiguration
{
    private static readonly string PathVariableName = GetPathVariableName();

#if NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER || NETSTANDARD2_0_OR_GREATER
    /// <summary>
    /// Adds the runtime native path to the path environment variable if required.
    /// </summary>
    public static void AddNativeRuntimeFolder() => AddNativeRuntimeFolder(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime native path to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddNativeRuntimeFolder(EnvironmentVariableTarget target)
    {
        if (RuntimeInformation.GetRuntimeNativePath() is string nativeFolder
            && Directory.Exists(nativeFolder)
            && !IsAlreadyInAppContext(nativeFolder, "NATIVE_DLL_SEARCH_DIRECTORIES"))
        {
            AddFolderToPath(nativeFolder, target);
        }
    }

    /// <summary>
    /// Adds the runtime library path to the path environment variable if required.
    /// </summary>
    public static void AddLibraryRuntimeFolder() => AddLibraryRuntimeFolder(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime library path to the path environment variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddLibraryRuntimeFolder(EnvironmentVariableTarget target)
    {
        if (RuntimeInformation.GetRuntimeLibraryPath() is string libraryFolder
            && Directory.Exists(libraryFolder)
            && !IsAlreadyInAppContext(libraryFolder, "APP_PATHS"))
        {
            AddFolderToPath(libraryFolder, target);
        }
    }

    /// <summary>
    /// Adds the runtime paths to the path variable if required.
    /// </summary>
    public static void AddRuntimeFolders() => AddRuntimeFolders(EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds the runtime paths to the path variable if required.
    /// </summary>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddRuntimeFolders(EnvironmentVariableTarget target)
    {
        AddLibraryRuntimeFolder(target);
        AddNativeRuntimeFolder(target);
    }

    /// <summary>
    /// Adds a folder to the path environment variable in the current process.
    /// </summary>
    /// <param name="folder">The folder to add.</param>
    public static void AddFolderToPath(string folder) => AddFolderToPath(folder, EnvironmentVariableTarget.Process);

    /// <summary>
    /// Adds a folder to the path environment variable in the current process or from the Windows operating system registry key for the current user or local machine.
    /// </summary>
    /// <param name="folder">The folder to add.</param>
    /// <param name="target">One of the <see cref="EnvironmentVariableTarget"/> values. Only <see cref="EnvironmentVariableTarget.Process"/> is supported on .NET running of Unix-based systems.</param>
    public static void AddFolderToPath(string folder, EnvironmentVariableTarget target)
    {
        var path = Environment.GetEnvironmentVariable(PathVariableName, target);
        if (path is null)
        {
            Environment.SetEnvironmentVariable(PathVariableName, folder);
            return;
        }

        if (path.Contains(folder))
        {
            return;
        }

        Environment.SetEnvironmentVariable(PathVariableName, folder + Path.PathSeparator + path, target);
    }
#else
    /// <summary>
    /// Adds the runtime native path to the path variable if required.
    /// </summary>
    public static void AddNativeRuntimeFolder()
    {
        if (RuntimeInformation.GetRuntimeNativePath() is string nativeFolder
            && Directory.Exists(nativeFolder)
            && !IsAlreadyInAppContext(nativeFolder, "NATIVE_DLL_SEARCH_DIRECTORIES"))
        {
            AddFolderToPath(nativeFolder);
        }
    }

    /// <summary>
    /// Adds the runtime library path to the path variable if required.
    /// </summary>
    public static void AddLibraryRuntimeFolder()
    {
        if (RuntimeInformation.GetRuntimeLibraryPath() is string libraryFolder
            && Directory.Exists(libraryFolder)
            && !IsAlreadyInAppContext(libraryFolder, "APP_PATHS"))
        {
            AddFolderToPath(libraryFolder);
        }
    }

    /// <summary>
    /// Adds the runtime paths to the path variable if required.
    /// </summary>
    public static void AddRuntimeFolders()
    {
        AddLibraryRuntimeFolder();
        AddNativeRuntimeFolder();
    }

    /// <summary>
    /// Adds a folder to the path environment variable.
    /// </summary>
    /// <param name="folder">The folder to add.</param>
    public static void AddFolderToPath(string folder)
    {
        var path = Environment.GetEnvironmentVariable(PathVariableName);
        if (path is null)
        {
            Environment.SetEnvironmentVariable(PathVariableName, folder);
            return;
        }

        if (path.Contains(folder))
        {
            return;
        }

        Environment.SetEnvironmentVariable(PathVariableName, folder + Path.PathSeparator + path);
    }
#endif

#if NETCOREAPP1_0_OR_GREATER || NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER
    private static bool IsAlreadyInAppContext(string value, string variable) => AppContext.GetData(variable) is string data && data.Contains(value);
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
}