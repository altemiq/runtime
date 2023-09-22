// -----------------------------------------------------------------------
// <copyright file="RuntimeInformationTests.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altavec.Runtime;

public class RuntimeInformationTests
{
    [Fact]
    public void GetRuntimeIdentifier() => RuntimeInformation.RuntimeIdentifier.Should().NotBeNullOrEmpty();

    [Fact]
    public void GetRuntimeNativePath() => RuntimeInformation.GetRuntimeNativePath().Should().NotBeNull();

    [Fact]
    public void GetRuntimeLibraryPath() => RuntimeInformation.GetRuntimeLibraryPath().Should().NotBeNull();
}
