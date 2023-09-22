// -----------------------------------------------------------------------
// <copyright file="RuntimeInformation.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Runtime.InteropServices;

using System.Reflection;

/// <summary>
/// Runtime information.
/// </summary>
public static class RuntimeInformation
{
#if !NET5_0_OR_GREATER
    private static string? runtimeIdentifier;
#endif

    private static string? targetFramework;

    /// <summary>
    /// Gets the target framework.
    /// </summary>
    public static string TargetFramework
    {
        get
        {
            return targetFramework ??= GetFrameworkName() ?? throw new InvalidOperationException();

            static string? GetFrameworkName()
            {
                var frameworkName = GetFrameworkNameFromAssembly(Assembly.GetEntryAssembly()) ?? GetFrameworkNameFromAssembly(typeof(object).GetTypeInfo().Assembly);

#if NETCOREAPP3_0_OR_GREATER || NET20_OR_GREATER
                frameworkName ??= AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
#elif NETSTANDARD2_0_OR_GREATER
                if (frameworkName is null)
                {
                    var setupInformationProperty = typeof(AppDomain).GetProperty("SetupInformation") ?? throw new InvalidOperationException();
                    var setupInformation = setupInformationProperty.GetValue(AppDomain.CurrentDomain, null);
                    frameworkName = setupInformation?
                        .GetType()
                        .GetProperty("TargetFrameworkName")?
                        .GetValue(setupInformation, null) as string;
                }
#endif
                return frameworkName;

                static string? GetFrameworkNameFromAssembly(Assembly? assembly)
                {
                    return assembly?.GetCustomAttribute(typeof(System.Runtime.Versioning.TargetFrameworkAttribute)) is System.Runtime.Versioning.TargetFrameworkAttribute attribute
                        ? attribute.FrameworkName
                        : default;
                }
            }
        }
    }

    /// <summary>
    /// Gets the platform for which the runtime was built (or on which an app is running).
    /// </summary>
    public static string RuntimeIdentifier
#if NET5_0_OR_GREATER
        => System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier;
#else
    {
        get
        {
#if NETSTANDARD2_0_OR_GREATER || NET47_OR_GREATER
            return runtimeIdentifier ??= AppContext.GetData("RUNTIME_IDENTIFIER") as string ?? GetRidCore();
#else
            return runtimeIdentifier ??= GetRidCore();
#endif

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
        }
    }
#endif
}