// -----------------------------------------------------------------------
// <copyright file="RuntimeEnvironmentTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeEnvironmentTests
{
    [Test]
    public async Task GetRuntimeNativeDirectory() => await Assert.That(RuntimeEnvironment.GetRuntimeNativeDirectory()).IsNotNull();

    [Test]
    public async Task GetRuntimeLibraryDirectory() => await Assert.That(RuntimeEnvironment.GetRuntimeLibraryDirectory()).IsNotNull();

    [Test]
    public async Task GetToolsDirectories() => await Assert.That(RuntimeEnvironment.GetToolsDirectories()).IsNotEmpty();

    [Test]
    public async Task GetToolsDirectory() => await Assert.That(RuntimeEnvironment.GetToolsDirectory()).IsNotNull();

    [Test]
    public async Task GetToolDirectory() => await Assert.That(RuntimeEnvironment.GetToolDirectory("_._")).IsNotNull();

    [Test]
    public async Task GetRuntimeNativeDirectoryWithModule() => await Assert.That(RuntimeEnvironment.GetRuntimeNativeDirectory(RuntimeEnvironment.CreateModuleName("e_sqlite3"))).IsNotNull();

    [Test]
    public async Task AddRuntimeLibraryDirectory()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        await Assert.That(RuntimeEnvironment.ShouldAddLibraryDirectory(RuntimeEnvironment.GetRuntimeLibraryDirectory())).IsFalse();
    }

    [Test]
    public async Task AddRuntimeNativeDirectory()
    {
        RuntimeEnvironment.AddRuntimeNativeDirectory();
        await Assert.That(RuntimeEnvironment.ShouldAddNativeDirectory(RuntimeEnvironment.GetRuntimeNativeDirectory())).IsFalse();
    }

    [Test]
    public async Task AddPathTwice()
    {
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        var path = Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable);
        RuntimeEnvironment.AddRuntimeLibraryDirectory();
        await Assert.That(Environment.GetEnvironmentVariable(RuntimeInformation.PathVariable)).IsEqualTo(path);
    }
}