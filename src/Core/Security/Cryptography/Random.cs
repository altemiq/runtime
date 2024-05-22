// -----------------------------------------------------------------------
// <copyright file="Random.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

#if NETSTANDARD1_3_OR_GREATER || NET || NETFRAMEWORK
namespace Altemiq.Security.Cryptography;

/// <summary>
/// Implementation of <see cref="System.Random"/> using <see cref="System.Security.Cryptography.RandomNumberGenerator"/>.
/// </summary>
public class Random : System.Random
{
    private readonly System.Security.Cryptography.RandomNumberGenerator randomNumberGenerator = System.Security.Cryptography.RandomNumberGenerator.Create();

#if !NETSTANDARD2_1_OR_GREATER && !NETCOREAPP2_1_OR_GREATER
    private readonly byte[] buffer = new byte[4];
#endif

    /// <inheritdoc/>
    public override void NextBytes(byte[] buffer)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(buffer);
        this.randomNumberGenerator.GetBytes(buffer);
    }

#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
    /// <inheritdoc/>
    public override void NextBytes(Span<byte> buffer) => System.Security.Cryptography.RandomNumberGenerator.Fill(buffer);
#endif

    /// <inheritdoc/>
    public override double NextDouble()
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        Span<byte> buffer = stackalloc byte[4];
        this.NextBytes(buffer);
        return BitConverter.ToUInt32(buffer) / (1.0 + uint.MaxValue);
#else
        this.NextBytes(this.buffer);
        return BitConverter.ToUInt32(this.buffer, 0) / (1.0 + uint.MaxValue);
#endif
    }

    /// <inheritdoc/>
    public override int Next(int minValue, int maxValue)
    {
        ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan(minValue, maxValue);
        if (minValue == maxValue)
        {
            return minValue;
        }

        var range = (long)maxValue - minValue;
        return (int)((long)Math.Floor(this.NextDouble() * range) + minValue);
    }

    /// <inheritdoc />
    public override int Next() => this.Next(0, int.MaxValue);

    /// <inheritdoc />
    public override int Next(int maxValue)
    {
        ArgumentOutOfRangeExceptionThrower.ThrowIfNegative(maxValue);
        return this.Next(0, maxValue);
    }
}
#endif