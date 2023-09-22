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
        Environment.GetEnvironmentVariable(PathVariable()).Should().Contain(RuntimeEnvironment.GetRuntimeLibraryDirectory());
    }

    [Fact]
    public void AddNativeDirectory()
    {
        RuntimeEnvironment.AddRuntimeNativeDirectory();
        Environment.GetEnvironmentVariable(PathVariable()).Should().Contain(RuntimeEnvironment.GetRuntimeNativeDirectory());
    }

    [Fact]
    public void AddPathTwice()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        var path = Environment.GetEnvironmentVariable(PathVariable());
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        Environment.GetEnvironmentVariable(PathVariable()).Should().Be(path);
    }

    private static string PathVariable()
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
