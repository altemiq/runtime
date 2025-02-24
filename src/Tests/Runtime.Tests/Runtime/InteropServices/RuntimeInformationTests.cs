// -----------------------------------------------------------------------
// <copyright file="RuntimeInformationTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeInformationTests
{
    [Test]
    public async Task GetRuntimeIdentifier()
    {
        await Assert.That(RuntimeInformation.RuntimeIdentifier).IsNotNull().And.IsNotEmpty();
    }

    [Test]
    public async Task GetTargetFramework()
    {
        await Assert.That(RuntimeInformation.TargetFramework).IsNotNull().And.IsNotEmpty();
    }

    [Test]
    public async Task GetTargetPlatform()
    {
        var targetPlatform = RuntimeInformation.TargetPlatform;
        await Assert.That(targetPlatform)
#if NET5_0_OR_GREATER && WINDOWS
            .IsNotEmpty()
            .And.Contains("Windows");
#else
            .IsEmpty();
#endif
    }
}