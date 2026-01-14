// -----------------------------------------------------------------------
// <copyright file="IntrinsicAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#pragma warning disable IDE0130, CheckNamespace
namespace System.Runtime.CompilerServices;
#pragma warning restore IDE0130, CheckNamespace

/// <summary>
/// Copied from <see href="https://github.com/dotnet/runtime/raw/refs/heads/main/src/libraries/System.Private.CoreLib/src/System/Runtime/CompilerServices/IntrinsicAttribute.cs"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method | AttributeTargets.Constructor | AttributeTargets.Field, Inherited = false)]
internal sealed class IntrinsicAttribute : Attribute;