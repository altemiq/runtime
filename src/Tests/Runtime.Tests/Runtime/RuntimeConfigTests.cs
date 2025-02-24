// -----------------------------------------------------------------------
// <copyright file="RuntimeConfigTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

using Altemiq.Runtime.InteropServices;

public class RuntimeConfigTests
{
    [Test]
    public async Task ReadRuntimeConfig() => await Assert.That(RuntimeEnvironment.GetRuntimeConfig()).IsNotNull();

    [Test]
    public async Task SaveRuntimeConfig()
    {
        var runtimeConfigRaw = NormalizeLineEndings(File.ReadAllText(RuntimeEnvironment.GetRuntimeConfigFileName()!));
        await Assert.That(NormalizeLineEndings(RuntimeEnvironment.GetRuntimeConfig()!.ToString())).IsEqualTo(runtimeConfigRaw);

        static string NormalizeLineEndings(string input)
        {
#if NET6_0_OR_GREATER
            return input.ReplaceLineEndings();
#else
            return input.Replace("\r\n", Environment.NewLine);
#endif
        }
    }
}