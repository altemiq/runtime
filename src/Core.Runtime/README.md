# Altavec Runtime library

This is the code library for base classes, extension methods, etc for runtime operations.

## Interop Services

These are designed to augment the classes `System.Runtime.InteropServices`

### Runtime Information

This provides information about the runtime

`TargetFramework` gets the target framework  
`RuntimeIdentifier` gets the runtime identifier

### Runtime Environment

This provides classes for interactive with the runtime environment

`GetRuntimeLibraryPath`{:.language-csharp} gets the runtime folder for managed assemblies  
`GetRuntimeNative`{:.language-csharp} gets the runtime folder for native assets  

`AddRuntimeLibraryDirectory`{:.language-csharp} adds the runtime folder for managed assemblies to the PATH environment variable  
`AddRuntimeNativeDirectory`{:.language-csharp} adds the runtime folder for native assets to the PATH environment variable  
`AddRuntimeDirectories`{:.language-csharp} combines the above methods  

## Resolution

The `Resolve` class provides functions for resolving assemblies

`RuntimeAssemblies`{:.language-csharp} adds handlers to resolve assemblies in the path provided by `RuntimeEnvironment.GetRuntimeLibraryPath()`{:.language-csharp}
