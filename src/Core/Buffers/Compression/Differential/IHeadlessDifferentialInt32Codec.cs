// -----------------------------------------------------------------------
// <copyright file="IHeadlessDifferentialInt32Codec.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// <para>Interface describing a standard codec to compress integers.</para>
/// <para>This is a variation on the <see cref="IInt32Codec"/> interface meant to be used for random access and with integrated differential coding (i.e., given a large array, you can segment it and decode just the subarray you need).</para>
/// </summary>
/// <remarks>
/// <para>The main differences are that we must specify the number of integers we wish to decode as well as the initial value (for differential coding).</para>
/// <para>This information might be stored elsewhere.</para>
/// </remarks>
internal interface IHeadlessDifferentialInt32Codec : ICompressHeadlessDifferentialCodec<int, int>, IDecompressHeadlessDifferentialCodec<int, int>;