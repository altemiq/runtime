// -----------------------------------------------------------------------
// <copyright file="RuntimeConfigurationTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeEnvironmentTests
{
    [Fact]
    public void GetRuntimeNativePath() => RuntimeEnvironment.GetRuntimeNativePath().Should().NotBeNull();

    [Fact]
    public void GetRuntimeLibraryPath() => RuntimeEnvironment.GetRuntimeLibraryPath().Should().NotBeNull();

    [Fact]
    public void AddLibraryPath()
    {
        RuntimeEnvironment.AddLibraryRuntimeFolder();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeEnvironment.GetRuntimeLibraryPath());
    }

    [Fact]
    public void AddNativePath()
    {
        RuntimeEnvironment.AddNativeRuntimeFolder();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeEnvironment.GetRuntimeNativePath());
    }

    [Fact]
    public void AddPathTwice()
    {
        RuntimeEnvironment.AddLibraryRuntimeFolder();
        var path = Environment.GetEnvironmentVariable("PATH");
        RuntimeEnvironment.AddLibraryRuntimeFolder();
        Environment.GetEnvironmentVariable("PATH").Should().Be(path);
    }
}
