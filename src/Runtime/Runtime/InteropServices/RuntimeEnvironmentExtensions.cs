// -----------------------------------------------------------------------
// <copyright file="RuntimeEnvironmentExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NETFRAMEWORK

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable CA1050, MA0047, RCS1110

/// <summary>
/// <see cref="System.Runtime.InteropServices.RuntimeEnvironment"/> extensions.
/// </summary>
public static class RuntimeEnvironmentExtensions
{
    /// <summary>
    /// <see cref="System.Runtime.InteropServices.RuntimeEnvironment"/> extensions.
    /// </summary>
    extension(System.Runtime.InteropServices.RuntimeEnvironment)
    {
        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeNativeDirectory()" />
        public static string? GetRuntimeNativeDirectory() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeNativeDirectory();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeNativeDirectory(string)" />
        public static string? GetRuntimeNativeDirectory(string name) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeNativeDirectory(name);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeNativeDirectories" />
        public static IEnumerable<string> GetRuntimeNativeDirectories() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeNativeDirectories();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeLibraryDirectory" />
        public static string? GetRuntimeLibraryDirectory() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeLibraryDirectory();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeLibraryDirectories" />
        public static IEnumerable<string> GetRuntimeLibraryDirectories() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeLibraryDirectories();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetToolDirectory" />
        public static string? GetToolDirectory(string name) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetToolDirectory(name);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetToolsDirectory" />
        public static string? GetToolsDirectory() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetToolsDirectory();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetToolsDirectories" />
        public static IEnumerable<string> GetToolsDirectories() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.GetToolsDirectories();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory()" />
        public static void AddRuntimeNativeDirectory() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory(EnvironmentVariableTarget)" />
        public static void AddRuntimeNativeDirectory(EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory(target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory(string)" />
        public static void AddRuntimeNativeDirectory(string moduleName) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory(moduleName);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory(string,EnvironmentVariableTarget)" />
        public static void AddRuntimeNativeDirectory(string moduleName, EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeNativeDirectory(moduleName, target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeLibraryDirectory()" />
        public static void AddRuntimeLibraryDirectory() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeLibraryDirectory();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeLibraryDirectory(EnvironmentVariableTarget)" />
        public static void AddRuntimeLibraryDirectory(EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeLibraryDirectory(target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddToolsDirectory()" />
        public static void AddToolsDirectory() => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddToolsDirectory();

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddToolsDirectory(EnvironmentVariableTarget)" />
        public static void AddToolsDirectory(EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddToolsDirectory(target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeDirectories()" />
        public static void AddRuntimeDirectories() => AddRuntimeDirectories(EnvironmentVariableTarget.Process);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeDirectories(EnvironmentVariableTarget)" />
        public static void AddRuntimeDirectories(EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddRuntimeDirectories(target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectoryToPath(string)" />
        public static void AddDirectoryToPath(string directory) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectoryToPath(directory);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectoryToPath(string,EnvironmentVariableTarget)" />
        public static void AddDirectoryToPath(string directory, EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectoryToPath(directory, target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectory(string,string)" />
        public static void AddDirectory(string directory, string variable) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectory(directory, variable);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectory(string,string,EnvironmentVariableTarget)" />
        public static void AddDirectory(string directory, string variable, EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.AddDirectory(directory, variable, target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddNativeDirectory(string)" />
        public static bool ShouldAddNativeDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddNativeDirectory(path);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddNativeDirectory(string,EnvironmentVariableTarget)" />
        public static bool ShouldAddNativeDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path, EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddNativeDirectory(path, target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddLibraryDirectory(string)" />
        public static bool ShouldAddLibraryDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddLibraryDirectory(path);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddLibraryDirectory(string,EnvironmentVariableTarget)" />
        public static bool ShouldAddLibraryDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path, EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddLibraryDirectory(path, target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddToolsDirectory(string)" />
        public static bool ShouldAddToolsDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddToolsDirectory(path);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddToolsDirectory(string,EnvironmentVariableTarget)" />
        public static bool ShouldAddToolsDirectory([System.Diagnostics.CodeAnalysis.NotNullWhen(true)] string? path, EnvironmentVariableTarget target) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.ShouldAddToolsDirectory(path, target);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.CreateModuleName" />
        public static string CreateModuleName(string name) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.CreateModuleName(name);

        /// <inheritdoc cref="Altemiq.Runtime.InteropServices.RuntimeEnvironment.CreateExecutableName" />
        public static string CreateExecutableName(string name) => Altemiq.Runtime.InteropServices.RuntimeEnvironment.CreateExecutableName(name);
    }
}

#pragma warning restore CA1050, MA0047, RCS1110, RCS1222, IDE0079
#endif