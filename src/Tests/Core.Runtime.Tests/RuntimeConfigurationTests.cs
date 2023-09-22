// -----------------------------------------------------------------------
// <copyright file="RuntimeConfigurationTests.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altavec.Runtime;

public class RuntimeConfigurationTests
{
    [Fact]
    public void AddLibraryPath()
    {
        RuntimeConfiguration.AddLibraryRuntimeFolder();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeInformation.GetRuntimeLibraryPath());
    }

    [Fact]
    public void AddNativePath()
    {
        RuntimeConfiguration.AddNativeRuntimeFolder();
        Environment.GetEnvironmentVariable("PATH").Should().Contain(RuntimeInformation.GetRuntimeNativePath());
    }
}
