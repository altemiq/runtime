// -----------------------------------------------------------------------
// <copyright file="ResolveTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

public class ResolveTests
{
    [Fact]
    public void FromLoaded()
    {
        var assemblyName = new System.Reflection.AssemblyName
        {
            Name = "xunit.v3.core",
            Version = new Version(1, 0),
        };

        Assert.NotNull(Resolve.ResolveRuntimeAssembly(assemblyName));
    }

    [Fact]
    public void LoadFromLib()
    {
        var assemblyName = new System.Reflection.AssemblyName { Name = "Altemiq.Dummy" };
        Assert.NotNull(Resolve.ResolveRuntimeAssembly(assemblyName));
    }
}