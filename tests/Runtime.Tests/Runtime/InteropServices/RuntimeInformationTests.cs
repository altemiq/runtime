// -----------------------------------------------------------------------
// <copyright file="RuntimeInformationTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

public class RuntimeInformationTests
{
    [Test]
    public async Task SystemVsAltemiq()
    {
        _ = await Assert.That(System.Runtime.InteropServices.RuntimeInformation.TargetFramework).IsEquivalentTo(RuntimeInformation.TargetFramework);
    }

    [Test]
    public async Task GetRuntimeIdentifier()
    {
        _ = await Assert.That(RuntimeInformation.RuntimeIdentifier).IsNotEmpty();
    }

    [Test]
    public async Task GetTargetFramework()
    {
        _ = await Assert.That(RuntimeInformation.TargetFramework).IsNotEmpty();
    }

    [Test]
    public async Task GetTargetPlatform()
    {
        await Assert.That(RuntimeInformation.TargetPlatform)
#if NET5_0_OR_GREATER && WINDOWS
            .IsNotEmpty()
            .And.Contains("Windows");
#else
            .IsEmpty();
#endif
    }
}