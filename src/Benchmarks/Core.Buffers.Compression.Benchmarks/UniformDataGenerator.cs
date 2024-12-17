// -----------------------------------------------------------------------
// <copyright file="UniformDataGenerator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The uniform data generator.
/// </summary>
internal class UniformDataGenerator
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UniformDataGenerator"/> class.
    /// </summary>
    public UniformDataGenerator()
    {
        Random = new Random();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="UniformDataGenerator"/> class.
    /// </summary>
    /// <param name="seed">The initial seed.</param>
    public UniformDataGenerator(int seed)
    {
        Random = new Random(seed);
    }

    /// <summary>
    /// Gets the random number generator.
    /// </summary>
    internal Random Random { get; }

    /// <summary>
    /// Generates uniform data.
    /// </summary>
    /// <param name="n">The size.</param>
    /// <param name="max">The maximum.</param>
    /// <returns>The uniform data.</returns>
    public int[] GenerateUniform(int n, int max)
    {
        return n switch
        {
            var _ when n * 2 > max => Negate(GenerateUniform(max - n, max), max),
            _ when 2048 * n > max => GenerateUniformBitmap(n, max),
            _ => GenerateUniformHash(n, max),
        };

        static int[] Negate(int[] x, int max)
        {
            var ans = new int[max - x.Length];
            var i = 0;
            var c = 0;
            for (var j = 0; j < x.Length; ++j)
            {
                var v = x[j];
                for (; i < v; ++i)
                {
                    ans[c++] = i;
                }

                ++i;
            }

            while (c < ans.Length)
            {
                ans[c++] = i++;
            }

            return ans;
        }

        int[] GenerateUniformBitmap(int n, int max)
        {
            if (n > max)
            {
                throw new InvalidDataException("not possible");
            }

            var ans = new int[n];
            var bs = new System.Collections.BitArray(max);
            var cardinality = 0;
            while (cardinality < n)
            {
                var v = Random.Next(max);
                if (!bs[v])
                {
                    bs[v] = true;
                    cardinality++;
                }
            }

            var pos = 0;
            for (var i = NextSetBit(bs, 0); i >= 0; i = NextSetBit(bs, i + 1))
            {
                ans[pos++] = i;
            }

            return ans;

            static int NextSetBit(System.Collections.BitArray bitArray, int fromIndex)
            {
                for (var j = fromIndex; j < bitArray.Length; j++)
                {
                    if (bitArray[j])
                    {
                        return j;
                    }
                }

                return -1;
            }
        }

        int[] GenerateUniformHash(int n, int max)
        {
            if (n > max)
            {
                throw new InvalidDataException("not possible");
            }

            var ans = new int[n];
            HashSet<int> s = [];
            while (s.Count < n)
            {
                _ = s.Add(Random.Next(max));
            }

            var i = s.GetEnumerator();

            for (var k = 0; k < n; ++k)
            {
                ans[k] = i.Current;
                _ = i.MoveNext();
            }

            System.Array.Sort(ans);
            return ans;
        }
    }
}