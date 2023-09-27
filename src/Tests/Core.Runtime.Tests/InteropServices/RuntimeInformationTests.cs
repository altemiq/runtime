// -----------------------------------------------------------------------
// <copyright file="RuntimeInformationTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeInformationTests
{
    [Fact]
    public void GetRuntimeIdentifier() => RuntimeInformation.RuntimeIdentifier.Should().NotBeNullOrEmpty();

    [Fact]
    public void GetTargetFramework() => RuntimeInformation.TargetFramework.Should().NotBeNullOrEmpty();

    [Fact]
    public void GetTargetPlatform()
#if NET5_0_OR_GREATER && WINDOWS
        => RuntimeInformation.TargetPlatform.Should().NotBeNullOrEmpty().And.Contain("Windows");
#else
        => RuntimeInformation.TargetPlatform.Should().NotBeNull().And.BeEmpty();
#endif
}