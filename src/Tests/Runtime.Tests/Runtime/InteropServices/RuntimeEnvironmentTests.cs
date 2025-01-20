// -----------------------------------------------------------------------
// <copyright file="RuntimeEnvironmentTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeEnvironmentTests
{
    [Fact]
    public void GetRuntimeNativeDirectory() => Assert.NotNull(RuntimeEnvironment.GetRuntimeNativeDirectory());

    [Fact]
    public void GetRuntimeLibraryDirectory() => Assert.NotNull(RuntimeEnvironment.GetRuntimeLibraryDirectory());

    [Fact]
    public void GetToolsDirectories()
    {
        var toolsDirectories = RuntimeEnvironment.GetToolsDirectories();
        Assert.NotNull(toolsDirectories);
        Assert.NotEmpty(toolsDirectories);
    }

    [Fact]
    public void GetToolsDirectory() => Assert.NotNull(RuntimeEnvironment.GetToolsDirectory());

    [Fact]
    public void GetToolDirectory() => Assert.NotNull(RuntimeEnvironment.GetToolDirectory("_._"));

    [Fact]
    public void GetRuntimeNativeDirectoryWithModule() => Assert.NotNull(RuntimeEnvironment.GetRuntimeNativeDirectory(RuntimeEnvironment.CreateModuleName("e_sqlite3")));

    [Fact]
    public void AddRuntimeLibraryDirectory()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        Assert.False(RuntimeEnvironment.ShouldAddLibraryDirectory(RuntimeEnvironment.GetRuntimeLibraryDirectory()));
    }

    [Fact]
    public void AddRuntimeNativeDirectory()
    {
        RuntimeEnvironment.AddRuntimeNativeDirectory();
        Assert.False(RuntimeEnvironment.ShouldAddNativeDirectory(RuntimeEnvironment.GetRuntimeNativeDirectory()));
    }

    [Fact]
    public void AddPathTwice()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        var path = Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable);
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        Assert.Equal(path, Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable));
    }
}