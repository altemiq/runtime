// -----------------------------------------------------------------------
// <copyright file="Vector128Extensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if !NET9_0_OR_GREATER
namespace Altemiq.Runtime.Intrinsics;

using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;

/// <summary>
/// <see cref="Vector128"/> extensions.
/// </summary>
public static class Vector128Extensions
{
    extension(Vector128)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static Vector128<double> MultiplyAddEstimate(Vector128<double> left, Vector128<double> right, Vector128<double> addend) =>
            Vector128.Create(
                Vector64.MultiplyAddEstimate(left.GetLower(), right.GetLower(), addend.GetLower()),
                Vector64.MultiplyAddEstimate(left.GetUpper(), right.GetUpper(), addend.GetUpper()));
    }
}
#endif