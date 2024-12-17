// -----------------------------------------------------------------------
// <copyright file="ClusteredDataGenerator.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression;

/// <summary>
/// The clustered data generator.
/// </summary>
internal class ClusteredDataGenerator
{
    private readonly UniformDataGenerator unidg = new(123456);

    /// <summary>
    /// Generates clustered data.
    /// </summary>
    /// <param name="n">The size.</param>
    /// <param name="max">The maximum.</param>
    /// <returns>The clustered data.</returns>
    public int[] GenerateClustered(int n, int max)
    {
        var array = new int[n];
        FillClustered(array, 0, n, 0, max);
        return array;

        void FillClustered(int[] array, int offset, int length, int min, int max)
        {
            var range = max - min;
            if ((range == length) || (length <= 10))
            {
                FillUniform(array, offset, length, min, max);
                return;
            }

            var cut = (length / 2) + ((range - length > 1) ? this.unidg.Random.Next(range - length - 1) : 0);
            var p = this.unidg.Random.NextDouble();
            if (p < 0.25)
            {
                FillUniform(array, offset, length / 2, min, min + cut);
                FillClustered(array, offset + (length / 2), length - (length / 2), min + cut, max);
            }
            else if (p < 0.5)
            {
                FillClustered(array, offset, length / 2, min, min + cut);
                FillUniform(array, offset + (length / 2), length - (length / 2), min + cut, max);
            }
            else
            {
                FillClustered(array, offset, length / 2, min, min + cut);
                FillClustered(array, offset + (length / 2), length - (length / 2), min + cut, max);
            }

            void FillUniform(int[] array, int offset, int length, int min, int max)
            {
                var v = this.unidg.GenerateUniform(length, max - min);
                for (var k = 0; k < v.Length; ++k)
                {
                    array[k + offset] = min + v[k];
                }
            }
        }
    }
}