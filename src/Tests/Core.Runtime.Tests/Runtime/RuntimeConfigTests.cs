// -----------------------------------------------------------------------
// <copyright file="RuntimeConfigTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

using Altemiq.Runtime.InteropServices;

public class RuntimeConfigTests
{
    [Fact]
    public void ReadRuntimeConfig() => RuntimeEnvironment.GetRuntimeConfig().Should().NotBeNull();

    [Fact]
    public void SaveRuntimeConfig()
    {
        var runtimeConfigRaw = File.ReadAllText(RuntimeEnvironment.GetRuntimeConfigFileName()!);
        RuntimeEnvironment.GetRuntimeConfig()!.ToString().Should().Be(runtimeConfigRaw);
    }
}
