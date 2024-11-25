// -----------------------------------------------------------------------
// <copyright file="RuntimeEnvironmentTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeEnvironmentTests
{
    [Fact]
    public void GetRuntimeNativeDirectory() => RuntimeEnvironment.GetRuntimeNativeDirectory().Should().NotBeNull();

    [Fact]
    public void GetRuntimeLibraryDirectory() => RuntimeEnvironment.GetRuntimeLibraryDirectory().Should().NotBeNull();

    [Fact]
    public void GetToolsDirectory() => RuntimeEnvironment.GetToolsDirectory().Should().NotBeNull();

    [Fact]
    public void GetToolDirectory() => RuntimeEnvironment.GetToolDirectory("_._").Should().NotBeNull();

    [Fact]
    public void GetRuntimeNativeDirectoryWithModule() => RuntimeEnvironment.GetRuntimeNativeDirectory(RuntimeEnvironment.CreateModuleName("e_sqlite3")).Should().NotBeNull();

    [Fact]
    public void AddRuntimeLibraryDirectory()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        RuntimeEnvironment.ShouldAddLibraryDirectory(RuntimeEnvironment.GetRuntimeLibraryDirectory()).Should().Be(false);
    }

    [Fact]
    public void AddRuntimeNativeDirectory()
    {
        RuntimeEnvironment.AddRuntimeNativeDirectory();
        RuntimeEnvironment.ShouldAddNativeDirectory(RuntimeEnvironment.GetRuntimeNativeDirectory()).Should().Be(false);
    }

    [Fact]
    public void AddPathTwice()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        var path = Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable);
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        _ = Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable).Should().Be(path);
    }
}