// -----------------------------------------------------------------------
// <copyright file="ResolveTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

public class ResolveTests
{
    [Test]
    public async Task FromLoaded()
    {
        var actualAssemblyName = typeof(ArgumentsAttribute).Assembly.GetName();
        var assemblyName = new System.Reflection.AssemblyName
        {
            Name = actualAssemblyName.Name,
            Version = actualAssemblyName.Version,
        };

        await Assert.That(Resolve.ResolveRuntimeAssembly(assemblyName)).IsNotNull();
    }

    [Test]
    public async Task LoadFromLib()
    {
        var assemblyName = new System.Reflection.AssemblyName { Name = "Altemiq.Dummy" };
        await Assert.That(Resolve.ResolveRuntimeAssembly(assemblyName)).IsNotNull();
    }
}