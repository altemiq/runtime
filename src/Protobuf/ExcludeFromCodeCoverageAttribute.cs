// -----------------------------------------------------------------------
// <copyright file="ExcludeFromCodeCoverageAttribute.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NETCOREAPP2_0_OR_GREATER && !NET40_OR_GREATER && !NETSTANDARD2_0_OR_GREATER
#pragma warning disable IDE0130, CheckNamespace
namespace System.Diagnostics.CodeAnalysis;
#pragma warning restore IDE0130, CheckNamespace

/// <summary>
/// Specifies that the attributed code should be excluded from code coverage information.
/// </summary>
[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Constructor | AttributeTargets.Event | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Struct, Inherited=false)]
internal sealed class ExcludeFromCodeCoverageAttribute : Attribute;
#endif