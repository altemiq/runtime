// -----------------------------------------------------------------------
// <copyright file="RuntimeConfigurationTests.cs" company="Altemiq">
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
    public void AddLibraryDirectory()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeEnvironment.GetRuntimeLibraryDirectory());
    }

    [Fact]
    public void AddNativeDirectory()
    {
        RuntimeEnvironment.AddRuntimeNativeDirectory();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeEnvironment.GetRuntimeNativeDirectory());
    }

    [Fact]
    public void AddPathTwice()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        var path = Environment.GetEnvironmentVariable("PATH");
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        Environment.GetEnvironmentVariable("PATH").Should().Be(path);
    }
}
