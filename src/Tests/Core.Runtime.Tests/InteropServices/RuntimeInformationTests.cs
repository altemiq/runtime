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
    public void GetRuntimeNativePath() => RuntimeInformation.GetRuntimeNativePath().Should().NotBeNull();

    [Fact]
    public void GetRuntimeLibraryPath() => RuntimeInformation.GetRuntimeLibraryPath().Should().NotBeNull();
}
