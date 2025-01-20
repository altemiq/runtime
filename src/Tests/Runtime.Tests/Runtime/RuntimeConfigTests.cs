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
    public void ReadRuntimeConfig() => Assert.NotNull(RuntimeEnvironment.GetRuntimeConfig());

    [Fact]
    public void SaveRuntimeConfig()
    {
        var runtimeConfigRaw = NormalizeLineEndings(File.ReadAllText(RuntimeEnvironment.GetRuntimeConfigFileName()!));
        Assert.Equal(runtimeConfigRaw, NormalizeLineEndings(RuntimeEnvironment.GetRuntimeConfig()!.ToString()));

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