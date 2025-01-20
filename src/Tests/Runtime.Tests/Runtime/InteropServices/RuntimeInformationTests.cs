// -----------------------------------------------------------------------
// <copyright file="RuntimeInformationTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeInformationTests
{
    [Fact]
    public void GetRuntimeIdentifier()
    {
        var rid = RuntimeInformation.RuntimeIdentifier;
        Assert.NotNull(rid);
        Assert.NotEmpty(rid);
    }

    [Fact]
    public void GetTargetFramework()
    {
        var targetFramework = RuntimeInformation.TargetFramework;
        Assert.NotNull(targetFramework);
        Assert.NotEmpty(targetFramework);
    }

    [Fact]
    public void GetTargetPlatform()
    {
        var targetPlatform = RuntimeInformation.TargetPlatform;
        Assert.NotNull(targetPlatform);
#if NET5_0_OR_GREATER && WINDOWS
        Assert.NotEmpty(targetPlatform);
        Assert.Contains("Windows", targetPlatform);
#else
        Assert.Empty(targetPlatform);
#endif
    }
}