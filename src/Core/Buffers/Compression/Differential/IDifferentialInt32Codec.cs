// -----------------------------------------------------------------------
// <copyright file="IDifferentialInt32Codec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// This is just like <see cref="IInt32Codec"/>, except that it indicates that delta coding is integrated, so that you don't need a separate step for delta coding.
/// </summary>
internal interface IDifferentialInt32Codec : IInt32Codec;