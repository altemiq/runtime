// -----------------------------------------------------------------------
// <copyright file="BitArrayEqualityComparer.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Collections;

/// <summary>
/// The <see cref="System.Collections.BitArray"/> <see cref="EqualityComparer{T}"/>.
/// </summary>
public sealed class BitArrayEqualityComparer : EqualityComparer<System.Collections.BitArray>
{
    /// <summary>
    /// The instance of the <see cref="BitArrayEqualityComparer"/>.
    /// </summary>
    public static readonly EqualityComparer<System.Collections.BitArray> Instance = new BitArrayEqualityComparer();

    private BitArrayEqualityComparer()
    {
    }

    /// <inheritdoc/>
    public override bool Equals(System.Collections.BitArray? x, System.Collections.BitArray? y)
    {
        return (x, y) switch
        {
            (null, null) => true,
            (not null, not null) when x.Length == y.Length => BitsEqual(x, y, x.Length),
            _ => false,
        };

        static bool BitsEqual(System.Collections.BitArray x, System.Collections.BitArray y, int count)
        {
            for (var i = 0; i < count; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }

            return true;
        }
    }

    /// <inheritdoc/>
    public override int GetHashCode(System.Collections.BitArray obj)
    {
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER
        var hashCode = default(HashCode);
        for (var i = 0; i < obj.Length; i++)
        {
            hashCode.Add(obj[i]);
        }

        return hashCode.ToHashCode();
#else
        var hashCode = 0;
        var equalityComparer = EqualityComparer<bool>.Default;
        for (var i = 0; i < obj.Length; i++)
        {
            hashCode = CombineHashCodes(hashCode, equalityComparer.GetHashCode(obj[i]));
        }

        return hashCode;

        static int CombineHashCodes(int h1, int h2)
        {
            return ((h1 << 5) + h1) ^ h2;
        }
#endif
    }
}