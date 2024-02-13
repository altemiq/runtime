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
    public void GetRuntimeNativeDirectoryWithModule() => RuntimeEnvironment.GetRuntimeNativeDirectory(RuntimeEnvironment.CreateModuleName("e_sqlite3")).Should().NotBeNull();

    [Fact]
    public void AddLibraryDirectory()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        _ = Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable).Should().Contain(RuntimeEnvironment.GetRuntimeLibraryDirectory());
    }

    [Fact]
    public void AddNativeDirectory()
    {
        RuntimeEnvironment.AddRuntimeNativeDirectory();
        _ = Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable).Should().Contain(RuntimeEnvironment.GetRuntimeNativeDirectory());
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