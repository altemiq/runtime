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
    private readonly UniformDataGenerator generator = new(123456);

    /// <summary>
    /// Generates clustered data.
    /// </summary>
    /// <param name="size">The size.</param>
    /// <param name="maximum">The maximum.</param>
    /// <returns>The clustered data.</returns>
    public int[] GenerateClustered(int size, int maximum)
    {
        var clustered = new int[size];
        FillClustered(clustered, 0, size, 0, maximum);
        return clustered;

        void FillClustered(int[] array, int offset, int length, int min, int max)
        {
            var range = max - min;
            if (range == length || length <= 10)
            {
                FillUniform(this.generator, array, offset, length, min, max);
                return;
            }

            var cut = length / 2 + (range - length > 1 ? this.generator.Random.Next(range - length - 1) : 0);
            var p = this.generator.Random.NextDouble();
            switch (p)
            {
                case < 0.25:
                    FillUniform(this.generator, array, offset, length / 2, min, min + cut);
                    FillClustered(array, offset + length / 2, length - length / 2, min + cut, max);
                    break;
                case < 0.5:
                    FillClustered(array, offset, length / 2, min, min + cut);
                    FillUniform(this.generator, array, offset + length / 2, length - length / 2, min + cut, max);
                    break;
                default:
                    FillClustered(array, offset, length / 2, min, min + cut);
                    FillClustered(array, offset + length / 2, length - length / 2, min + cut, max);
                    break;
            }

            static void FillUniform(UniformDataGenerator generator, int[] array, int offset, int length, int min, int max)
            {
                var v = generator.GenerateUniform(length, max - min);
                for (var k = 0; k < v.Length; ++k)
                {
                    array[k + offset] = min + v[k];
                }
            }
        }
    }
}