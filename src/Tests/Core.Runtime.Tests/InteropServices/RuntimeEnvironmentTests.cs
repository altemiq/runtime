// -----------------------------------------------------------------------
// <copyright file="RuntimeConfigurationTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeEnvironmentTests
{
    [Fact]
    public void AddLibraryPath()
    {
        RuntimeEnvironment.AddLibraryRuntimeFolder();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeInformation.GetRuntimeLibraryPath());
    }

    [Fact]
    public void AddNativePath()
    {
        RuntimeEnvironment.AddNativeRuntimeFolder();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeInformation.GetRuntimeNativePath());
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
