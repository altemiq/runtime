// -----------------------------------------------------------------------
// <copyright file="ResolvedTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Reflection;

public class AssemblyExtensionsTests
{
    [Theory]
    [InlineData("System, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    [InlineData("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0, Culture=neutral", true)]
    [InlineData("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0", true)]
    [InlineData("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq", true)]
    [InlineData("Altemiq, Version=1.0.0.0", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    [InlineData("Altemiq", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    [InlineData("Altemiq, Version=1.0.0.0", "Altemiq, Version=1.0.0.0", true)]
    [InlineData("Altemiq", "Altemiq, Version=1.0.0.0", false)]
    [InlineData("Altemiq", "Altemiq, Culture=neutral", false)]
    [InlineData("Altemiq, Version=2.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0, Culture=neutral", true)]
    [InlineData("Altemiq, Version=2.0.0.0, Culture=neutral", "Altemiq, Version=1.0.0.0", true)]
    [InlineData("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=2.0.0.0, Culture=neutral", false)]
    [InlineData("Altemiq, Version=1.0.0.0, Culture=neutral", "Altemiq, Version=2.0.0.0", false)]
    [InlineData("Altemiq, Version=2.0.0.0, Culture=en-US", "Altemiq, Version=1.0.0.0, Culture=neutral", false)]
    public void IsCompatible(string assemblyName, string requiredAssemblyName, bool result) => new System.Reflection.AssemblyName(assemblyName).IsCompatible(new System.Reflection.AssemblyName(requiredAssemblyName)).Should().Be(result);
}