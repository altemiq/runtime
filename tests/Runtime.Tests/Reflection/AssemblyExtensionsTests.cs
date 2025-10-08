// -----------------------------------------------------------------------
// <copyright file="ResolvedTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Reflection;

public class AssemblyExtensionsTests
{
    [Test]
    [Arguments("System, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    [Arguments("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0, Culture=neutral", true)]
    [Arguments("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0", true)]
    [Arguments("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq", true)]
    [Arguments("Altemiq, Version=1.0.0.0", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    [Arguments("Altemiq", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    [Arguments("Altemiq, Version=1.0.0.0", "Altemiq, Version=1.0.0.0", true)]
    [Arguments("Altemiq", "Altemiq, Version=1.0.0.0", false)]
    [Arguments("Altemiq", "Altemiq, Culture=neutral", false)]
    [Arguments("Altemiq, Version=2.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0, Culture=neutral", true)]
    [Arguments("Altemiq, Version=2.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0", true)]
    [Arguments("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=2.0.0.0, Culture=neutral", false)]
    [Arguments("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=2.0.0.0", false)]
    [Arguments("Altemiq, Version=2.0.0.0, Culture=en-US", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    public async Task IsCompatible(string assemblyName, string requiredAssemblyName, bool expected) => await Assert.That(new System.Reflection.AssemblyName(assemblyName).IsCompatible(new System.Reflection.AssemblyName(requiredAssemblyName))).IsEqualTo(expected);
}