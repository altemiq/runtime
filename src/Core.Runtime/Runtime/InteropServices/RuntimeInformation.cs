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
    /// <summary>
    /// The path variable name.
    /// </summary>
    public static readonly string PathVariable = GetPathVariable();

    /// <summary>
    /// The shared library extension.
    /// </summary>
    public static readonly string SharedLibraryExtension = GetSharedLibraryExtension();

    /// <summary>
    /// The shared library prefix.
    /// </summary>
    public static readonly string SharedLibraryPrefix = GetSharedLibraryPrefix();

#if !NET5_0_OR_GREATER
    private static string? runtimeIdentifier;
#endif

    private static string? targetFramework;
    private static string? targetPlatform;

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
#if NETCOREAPP3_0_OR_GREATER || NET20_OR_GREATER
                return GetFrameworkNameCore() ?? AppDomain.CurrentDomain.SetupInformation.TargetFrameworkName;
#elif NETSTANDARD2_0_OR_GREATER
                return GetFrameworkNameCore() ?? GetFrameworkNameFromSetupInformation();
#else
                return GetFrameworkNameCore();
#endif

                static string? GetFrameworkNameCore()
                {
                    return GetFrameworkNameFromAssembly(Reflection.Assembly.GetEntryAssembly()) ?? GetFrameworkNameFromAssembly(typeof(object).GetTypeInfo().Assembly);

                    static string? GetFrameworkNameFromAssembly(Assembly? assembly)
                    {
                        return assembly?.GetCustomAttribute(typeof(System.Runtime.Versioning.TargetFrameworkAttribute)) is System.Runtime.Versioning.TargetFrameworkAttribute attribute
                            ? attribute.FrameworkName
                            : default;
                    }
                }

#if NETSTANDARD2_0_OR_GREATER
                static string? GetFrameworkNameFromSetupInformation()
                {
                    var setupInformationProperty = typeof(AppDomain).GetProperty("SetupInformation") ?? throw new InvalidOperationException();
                    var setupInformation = setupInformationProperty.GetValue(AppDomain.CurrentDomain, index: null);
                    return setupInformation?
                        .GetType()
                        .GetProperty("TargetFrameworkName")?
                        .GetValue(setupInformation, index: null) as string;
                }
#endif
            }
        }
    }

    /// <summary>
    /// Gets the target platform, or an empty string.
    /// </summary>
    public static string TargetPlatform
    {
        get
        {
            return targetPlatform ??= GetPlatformName() ?? string.Empty;

            static string? GetPlatformName()
            {
                return GetPlatformNameFromAssembly(Reflection.Assembly.GetEntryAssembly()) ?? GetPlatformNameFromAssembly(typeof(object).GetTypeInfo().Assembly);

                static string? GetPlatformNameFromAssembly(Assembly? assembly)
                {
#if NET5_0_OR_GREATER
                    return assembly?.GetCustomAttribute(typeof(System.Runtime.Versioning.TargetPlatformAttribute)) is System.Runtime.Versioning.TargetPlatformAttribute attribute
                        ? attribute.PlatformName
                        : default;
#else
                    return assembly?.GetCustomAttributes().FirstOrDefault(a => a.GetType().Name.Contains("TargetPlatformAttribute")) is { } customAttribute
                        ? customAttribute.GetType().GetTypeInfo().GetProperty("PlatformName").GetValue(customAttribute) as string
                        : default;
#endif
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
            }
        }
    }
#endif

    /// <summary>
    /// Gets the file path of the base directories that the assembly resolver uses to probe for assemblies.
    /// </summary>
    /// <returns>The base directories.</returns>
    internal static IEnumerable<string> GetBaseDirectories()
    {
#if NETCOREAPP1_0_OR_GREATER || NET46_OR_GREATER || NETSTANDARD1_3_OR_GREATER
        yield return AppContext.BaseDirectory;
#endif
#if NETCOREAPP2_0_OR_GREATER || NET20_OR_GREATER || NETSTANDARD2_0_OR_GREATER
        yield return AppDomain.CurrentDomain.BaseDirectory;
#endif
    }

    /// <summary>
    /// Gets the naïve RID.
    /// </summary>
    /// <returns>The naïve RID.</returns>
    /// <exception cref="InvalidOperationException">Invalid operating system.</exception>
    internal static string GetNaïveRid()
    {
        return $"{GetRidFront()}-{GetRidBack()}";

        static string GetRidFront()
        {
#if NET5_0_OR_GREATER
            if (OperatingSystem.IsWindows())
            {
                return "win";
            }

            if (OperatingSystem.IsLinux())
            {
                return "linux";
            }

            if (OperatingSystem.IsMacOS())
            {
                return "osx";
            }
#else
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
#endif

            throw new InvalidOperationException();
        }

        static string GetRidBack()
        {
            const string Arm = "arm";
            const string Arm64 = "arm64";
            const string X86 = "x86";
            const string X64 = "x64";
#if NET5_0_OR_GREATER
            const string Wasm = "wasm";
#endif

#if NETCOREAPP1_0_OR_GREATER || NET471_OR_GREATER || NETSTANDARD1_1_OR_GREATER
            return System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture switch
            {
                System.Runtime.InteropServices.Architecture.Arm => Arm,
                System.Runtime.InteropServices.Architecture.Arm64 => Arm64,
                System.Runtime.InteropServices.Architecture.X86 => X86,
                System.Runtime.InteropServices.Architecture.X64 => X64,
#if NET5_0_OR_GREATER
                System.Runtime.InteropServices.Architecture.Wasm => Wasm,
#endif
                _ => throw new InvalidOperationException(),
            };
#else
            return System.Runtime.InteropServices.RuntimeInformation.OSArchitecture switch
            {
                System.Runtime.InteropServices.Architecture.Arm or System.Runtime.InteropServices.Architecture.Arm64 when IntPtr.Size is 4 => Arm,
                System.Runtime.InteropServices.Architecture.Arm64 => Arm64,
                System.Runtime.InteropServices.Architecture.X86 or System.Runtime.InteropServices.Architecture.X64 when IntPtr.Size is 4 => X86,
                System.Runtime.InteropServices.Architecture.X64 => X64,
                _ => throw new InvalidOperationException(),
            };
#endif
        }
    }

    private static string GetPathVariable()
    {
#if NET5_0_OR_GREATER
        if (OperatingSystem.IsWindows())
        {
            return "PATH";
        }

        if (OperatingSystem.IsLinux())
        {
            return "LD_LIBRARY_PATH";
        }

        if (OperatingSystem.IsMacOS())
        {
            return "DYLD_LIBRARY_PATH";
        }
#else
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            return "PATH";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            return "LD_LIBRARY_PATH";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
        {
            return "DYLD_LIBRARY_PATH";
        }
#endif

        throw new InvalidOperationException();
    }

    private static string GetSharedLibraryExtension()
    {
#if NET5_0_OR_GREATER
        if (OperatingSystem.IsWindows())
        {
            return ".dll";
        }

        if (OperatingSystem.IsLinux())
        {
            return ".so";
        }

        if (OperatingSystem.IsMacOS())
        {
            return ".dylib";
        }
#else
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            return ".dll";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            return ".so";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
        {
            return ".dylib";
        }
#endif

        throw new InvalidOperationException();
    }

    private static string GetSharedLibraryPrefix()
    {
#if NET5_0_OR_GREATER
        if (OperatingSystem.IsWindows())
        {
            return string.Empty;
        }

        if (OperatingSystem.IsLinux())
        {
            return "lib";
        }

        if (OperatingSystem.IsMacOS())
        {
            return "lib";
        }
#else
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            return string.Empty;
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            return "lib";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
        {
            return "lib";
        }
#endif

        throw new InvalidOperationException();
    }
}