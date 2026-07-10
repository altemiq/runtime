// -----------------------------------------------------------------------
// <copyright file="RuntimeInformationExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0079
#pragma warning disable RCS1222
#pragma warning disable CA1050, MA0047, RCS1110, CheckNamespace

using System.Reflection;

/// <summary>
/// <see cref="System.Runtime.InteropServices.RuntimeInformation"/> extensions.
/// </summary>
public static class RuntimeInformationExtensions
{
#pragma warning disable SA1137, SA1400, S1144
    /// <summary>
    /// <see cref="System.Runtime.InteropServices.RuntimeInformation"/> extensions.
    /// </summary>
    extension(System.Runtime.InteropServices.RuntimeInformation)
    {
        /// <summary>
        /// Gets the path variable name.
        /// </summary>
        public static string PathVariable => PathVariable;

        /// <summary>
        /// Gets the shared library extension.
        /// </summary>
        public static string SharedLibraryExtension => SharedLibraryExtension;

        /// <summary>
        /// Gets the executable extension.
        /// </summary>
        public static string ExecutableExtension => ExecutableExtension;

        /// <summary>
        /// Gets the shared library prefix.
        /// </summary>
        public static string SharedLibraryPrefix => SharedLibraryPrefix;

        /// <summary>
        /// Gets the target framework.
        /// </summary>
        /// <exception cref="InvalidOperationException">Failed to get the framework name.</exception>
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
                        return GetFrameworkNameFromAssembly(Assembly.GetActualEntryAssembly()) ?? GetFrameworkNameFromAssembly(typeof(object).GetTypeInfo().Assembly);

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
                    return GetPlatformNameFromAssembly(Assembly.GetActualEntryAssembly()) ?? GetPlatformNameFromAssembly(typeof(object).GetTypeInfo().Assembly);

                    static string? GetPlatformNameFromAssembly(Assembly? assembly)
                    {
#if NET5_0_OR_GREATER
                        return assembly?.GetCustomAttribute(typeof(System.Runtime.Versioning.TargetPlatformAttribute)) is System.Runtime.Versioning.TargetPlatformAttribute attribute
                            ? attribute.PlatformName
                            : default;
#else
                        return assembly?.GetCustomAttributes().FirstOrDefault(static a => a.GetType().Name.Contains("TargetPlatformAttribute", StringComparison.Ordinal)) is { } customAttribute
                            ? customAttribute.GetType().GetTypeInfo().GetProperty("PlatformName")?.GetValue(customAttribute) as string
                            : default;
#endif
                    }
                }
            }
        }

#if !NET5_0_OR_GREATER
        /// <summary>
        /// Gets the platform for which the runtime was built (or on which an app is running).
        /// </summary>
        public static string RuntimeIdentifier
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
    }
#pragma warning restore SA1137, SA1400, S1144

    private static readonly string PathVariable = GetPathVariable();
    private static readonly string SharedLibraryExtension = GetSharedLibraryExtension();
    private static readonly string ExecutableExtension = GetExecutableExtension();
    private static readonly string SharedLibraryPrefix = GetSharedLibraryPrefix();

    private static string? targetFramework;
    private static string? targetPlatform;
#if !NET5_0_OR_GREATER
    private static string? runtimeIdentifier;
#endif

    /// <summary>
    /// Gets the naïve RID.
    /// </summary>
    /// <returns>The naïve RID.</returns>
    /// <exception cref="InvalidOperationException">Invalid operating system.</exception>
#if NETSTANDARD2_0_OR_GREATER || NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    internal static string GetNaïveRid() => $"{GetNaïveRidFront()}-{GetNaïveRidBack()}";

    /// <summary>
    /// Gets the possible naïve RIDs.
    /// </summary>
    /// <returns>The possible naïve RIDs.</returns>
    internal static IEnumerable<string> GetNaïveRids()
    {
        var back = GetNaïveRidBack();
        return GetNaïveRidFronts().Select(front => $"{front}-{back}");
    }

    private static IEnumerable<string> GetNaïveRidFronts()
    {
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
        if (OperatingSystem.IsLinux())
        {
            foreach (var variant in GetLinuxVariant())
            {
                yield return variant;
            }
        }
#else
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            foreach (var variant in GetLinuxVariant())
            {
                yield return variant;
            }
        }
#endif

        yield return GetNaïveRidFront();

        [System.Runtime.Versioning.SupportedOSPlatform("linux")]
        static IEnumerable<string> GetLinuxVariant()
        {
            const string FilePath = "/etc/os-release";

            if (!File.Exists(FilePath))
            {
                yield break;
            }

            using var reader = new StreamReader(File.OpenRead(FilePath));
            while (reader.ReadLine() is { } line)
            {
                if (line.StartsWith("ID", StringComparison.Ordinal) && line.IndexOf('=', StringComparison.Ordinal) is > 0 and var equalsPosition)
                {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_0_OR_GREATER
                    yield return line[(equalsPosition + 1)..];
#else
                    yield return line.Substring(equalsPosition + 1);
#endif
                }
            }
        }
    }

    private static string GetNaïveRidFront()
    {
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
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

    private static string GetNaïveRidBack()
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
#if NET6_0_OR_GREATER
            System.Runtime.InteropServices.Architecture.S390x => throw new NotSupportedException(),
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

#if NETSTANDARD2_0_OR_GREATER || NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    private static string GetPathVariable()
    {
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
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

#if NETSTANDARD2_0_OR_GREATER || NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    private static string GetSharedLibraryExtension()
    {
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
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

#if NETSTANDARD2_0_OR_GREATER || NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    private static string GetExecutableExtension()
    {
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
        if (OperatingSystem.IsWindows())
        {
            return ".exe";
        }

        if (OperatingSystem.IsLinux())
        {
            return string.Empty;
        }

        if (OperatingSystem.IsMacOS())
        {
            return string.Empty;
        }
#else
        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
        {
            return ".exe";
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
        {
            return string.Empty;
        }

        if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
        {
            return string.Empty;
        }
#endif

        throw new InvalidOperationException();
    }

#if NETSTANDARD2_0_OR_GREATER || NET40_OR_GREATER || NETCOREAPP2_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
#endif
    private static string GetSharedLibraryPrefix()
    {
#if NETCOREAPP2_0_OR_GREATER || NETSTANDARD2_0_OR_GREATER || NETFRAMEWORK
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

#pragma warning restore CA1050, MA0047, RCS1110, RCS1222, IDE0079, CheckNamespace