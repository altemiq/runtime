// -----------------------------------------------------------------------
// <copyright file="RuntimeInformation.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime;

using System.Reflection;
using Microsoft.Extensions.DependencyModel;

/// <summary>
/// Runtime information.
/// </summary>
public static class RuntimeInformation
{
    private static IReadOnlyList<RuntimeFallbacks>? runtimeGraph;

    /// <summary>
    /// Gets the runtime native path.
    /// </summary>
    /// <returns>The runtime native path.</returns>
    public static string? GetRuntimeNativePath() => GetRuntimePath("native");

    /// <summary>
    /// Gets the runtime library path.
    /// </summary>
    /// <returns>The runtime library path.</returns>
    /// <exception cref="InvalidOperationException">Unable to get the current target framework.</exception>
    public static string? GetRuntimeLibraryPath()
    {
        var runtimesLibraryDirectory = GetRuntimePath("lib");
        if (runtimesLibraryDirectory is null || !Directory.Exists(runtimesLibraryDirectory))
        {
            return default;
        }

        // get all the tfms
        var availableTfms = Directory.GetDirectories(runtimesLibraryDirectory).Select(Path.GetFileName).ToArray();
        if (availableTfms.Length == 0)
        {
            return runtimesLibraryDirectory;
        }

        // get the closest TFM from the list
        return NuGet.Frameworks.NuGetFrameworkUtility.GetNearest(availableTfms, GetTfm(), NuGet.Frameworks.NuGetFramework.ParseFolder) is string nearest
            ? Path.Combine(runtimesLibraryDirectory, nearest)
            : runtimesLibraryDirectory;

        static NuGet.Frameworks.NuGetFramework GetTfm()
        {
            return Assembly.GetEntryAssembly() is Assembly assembly && assembly.GetCustomAttribute(typeof(System.Runtime.Versioning.TargetFrameworkAttribute)) is System.Runtime.Versioning.TargetFrameworkAttribute attribute
                ? NuGet.Frameworks.NuGetFramework.ParseFrameworkName(attribute.FrameworkName, NuGet.Frameworks.DefaultFrameworkNameProvider.Instance)
                : throw new InvalidOperationException();
        }
    }

    private static string? GetRuntimePath(string name)
    {
        var baseDirectory =
#if NET451
            AppDomain.CurrentDomain.BaseDirectory;
#else
            AppContext.BaseDirectory;
#endif
        var runtimesDirectory = Path.Combine(baseDirectory, "runtimes");
        if (!Directory.Exists(runtimesDirectory))
        {
            return default;
        }

        // get the rids
        var availableRids = Directory.GetDirectories(runtimesDirectory).Select(Path.GetFileName).ToArray();
        if (availableRids.Length == 0)
        {
            return runtimesDirectory;
        }

        if (GetRuntimeRids().FirstOrDefault(availableRids.Contains) is string rid)
        {
            return Path.Combine(runtimesDirectory, rid, name);
        }

        return Path.Combine(runtimesDirectory, name);

        static IEnumerable<string> GetRuntimeRids()
        {
            runtimeGraph ??= GetRuntimeGraph();
            if (runtimeGraph.Count > 0)
            {
                var rid = GetRid();
                var rids = runtimeGraph.First(g => string.Equals(g.Runtime, rid, StringComparison.Ordinal)).Fallbacks.ToList();
                rids.Insert(0, rid);
                return rids;
            }

            return Enumerable.Empty<string>();

            static IReadOnlyList<RuntimeFallbacks> GetRuntimeGraph()
            {
                DependencyContext? dependencyContext;
                try
                {
                    dependencyContext = DependencyContext.Default;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Fail(ex.ToString());
                    dependencyContext = null;
                }

                if (dependencyContext?.RuntimeGraph is IReadOnlyList<RuntimeFallbacks> runtimeFallbacks && runtimeFallbacks.Count != 0)
                {
                    return runtimeFallbacks;
                }

                var assembly = typeof(RuntimeInformation).GetTypeInfo().Assembly;
                using var stream = assembly.GetManifestResourceStream(assembly.GetManifestResourceNames().First(n => n.EndsWith("runtime.json", StringComparison.OrdinalIgnoreCase)))!;
                return JsonRuntimeFormat.ReadRuntimeGraph(stream).ToList();
            }

            static string GetRid()
            {
#if NET5_0_OR_GREATER
                return System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;
#elif NETSTANDARD2_0_OR_GREATER || NET47_OR_GREATER
                return AppContext.GetData("RUNTIME_IDENTIFIER") as string ?? GetRidCore();
#else
                return GetRidCore();
#endif

#if !NET5_0_OR_GREATER
                static string GetRidCore()
                {
                    return Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier() ?? GetNaïveRid();

                    static string GetNaïveRid()
                    {
                        return $"{GetRidFront()}-{GetRidBack()}";

                        static string GetRidFront()
                        {
                            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                            {
                                return "win";
                            }

                            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                            {
                                return "linux";
                            }

                            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                            {
                                return "osx";
                            }

                            throw new InvalidOperationException();
                        }

                        static string GetRidBack()
                        {
                            const string Arm = "arm";
                            const string Arm64 = "arm64";
                            const string X86 = "x86";
                            const string X64 = "x64";

                            return System.Runtime.InteropServices.RuntimeInformation.OSArchitecture switch
                            {
                                System.Runtime.InteropServices.Architecture.Arm => Arm,
                                System.Runtime.InteropServices.Architecture.Arm64 when IntPtr.Size == 4 => Arm,
                                System.Runtime.InteropServices.Architecture.Arm64 => Arm64,
                                System.Runtime.InteropServices.Architecture.X86 => X86,
                                System.Runtime.InteropServices.Architecture.X64 when IntPtr.Size == 4 => X86,
                                System.Runtime.InteropServices.Architecture.X64 => X64,
                                _ => throw new InvalidOperationException(),
                            };
                        }
                    }
                }
#endif
            }
        }
    }
}