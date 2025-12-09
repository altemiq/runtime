# Altemiq Runtime library

This is the code library for base classes, extension methods, etc for runtime operations.

This uses runtime graphs from
https://github.com/dotnet/runtime/blob/main/src/libraries/Microsoft.NETCore.Platforms

## Reflection

These are designed to the classes in [`System.Reflection`](https://learn.microsoft.com/en-us/dotnet/api/system.reflection)

### Assembly Extensions

These provide extensions to [`System.Reflection.Assembly`](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly) and [`System.Reflrection.AssemblyName`](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assemblyname)

#### IsCompatible

Determines if the supplied assembly(name) is compatible with the required assembly names.

This is useful when trying to resolve assemblies.

```csharp
var assembly = Assembly.GetEntryAssembly();
var isCompatible = assembly.ToCompatible(requiredAssemblyName);
```

## Runtime

### Interop Services

These are designed to augment the classes in [`System.Runtime.InteropServices`](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.interopservices)

#### Runtime Information

This provides information about the runtime

`TargetFramework` gets the target framework  
`RuntimeIdentifier` gets the runtime identifier

#### Runtime Environment

This provides classes for interactive with the runtime environment

`GetRuntimeLibraryDirectory` gets the runtime folder for managed assemblies  
`GetRuntimeNativeDirectory` gets the runtime folder for native assets  

`AddRuntimeLibraryDirectory` adds the runtime folder for managed assemblies to the PATH environment variable  
`AddRuntimeNativeDirectory` adds the runtime folder for native assets to the PATH environment variable  
`AddRuntimeDirectories` combines the above methods

##### Tools

When there are tools associated with a nuget package, these can be accessed via `RuntimeEnvironment`

`GetToolsDirectory` gets the runtime folder for the tools

> ⚠️ ***NOTE:*** from .NET 10 SDK onwards, `TrimDepsJsonLibrariesWithoutAssets` must be set to `false` or else these tools will not be added to the `deps.json` file

```xml
  <PropertyGroup>
    <TrimDepsJsonLibrariesWithoutAssets>false</TrimDepsJsonLibrariesWithoutAssets>
  </PropertyGroup>
```

### Resolution

The `Resolve` class provides functions for resolving assemblies

`RuntimeAssemblies` adds handlers to resolve assemblies in the path provided by `RuntimeEnvironment.GetRuntimeLibraryPath()`
