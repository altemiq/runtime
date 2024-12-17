// -----------------------------------------------------------------------
// <copyright file="Delta.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Buffers.Compression.Differential;

/// <summary>
/// Generic class to compute differential coding.
/// </summary>
internal static class Delta
{
    /// <summary>
    /// Apply differential coding (in-place).
    /// </summary>
    /// <param name="data">The data to be modified.</param>
    public static void Forward(Span<int> data)
    {
        var optimalSize = data.Length / 4 * 4;
        var i = data.Length - 1;

        if (optimalSize > 4)
        {
            for (; i > 3; i -= 4)
            {
                data[i] -= data[i - 1];
                data[i - 1] -= data[i - 2];
                data[i - 2] -= data[i - 3];
                data[i - 3] -= data[i - 4];
            }
        }

        for (; i > 0; i--)
        {
            data[i] -= data[i - 1];
        }
    }

    /// <summary>
    /// Apply differential coding (in-place) given an initial value.
    /// </summary>
    /// <param name="data">The data to be modified.</param>
    /// <param name="initialValue">The initial value.</param>
    /// <returns>The next initial vale.</returns>
    public static int Forward(Span<int> data, int initialValue)
    {
        var nextInitialValue = data[^1];
        var optimalSize = data.Length / 4 * 4;
        var i = data.Length - 1;

        if (optimalSize > 4)
        {
            for (; i > 3; i -= 4)
            {
                data[i] -= data[i - 1];
                data[i - 1] -= data[i - 2];
                data[i - 2] -= data[i - 3];
                data[i - 3] -= data[i - 4];
            }
        }

        for (; i > 0; i--)
        {
            data[i] -= data[i - 1];
        }

        data[0] -= initialValue;
        return nextInitialValue;
    }

    /// <summary>
    /// Compute differential coding given an initial value. Output is written to a provided array: must have length "length" or better.
    /// </summary>
    /// <param name="source">The data.</param>
    /// <param name="initialValue">The initial value.</param>
    /// <param name="destination">The output array.</param>
    /// <returns>The next initial vale.</returns>
    public static int Forward(ReadOnlySpan<int> source, int initialValue, Span<int> destination)
    {
        for (var i = source.Length - 1; i > 0; --i)
        {
            destination[i] = source[i] - source[i - 1];
        }

        destination[0] = source[0] - initialValue;
        return source[^1];
    }

    /// <summary>
    /// Undo differential coding (in-place). Effectively computes a prefix sum.
    /// </summary>
    /// <param name="data">The data to be modified.</param>
    public static void Inverse(Span<int> data)
    {
        var optimalSize = data.Length / 4 * 4;
        var i = 1;
        if (optimalSize >= 4)
        {
            var adjustment = data[0];
            for (; i < optimalSize - 4; i += 4)
            {
                adjustment = data[i] += adjustment;
                adjustment = data[i + 1] += adjustment;
                adjustment = data[i + 2] += adjustment;
                adjustment = data[i + 3] += adjustment;
            }
        }

        for (; i < data.Length; i++)
        {
            data[i] += data[i - 1];
        }
    }

    /// <summary>
    /// Undo differential coding (in-place) with an initial value. Effectively computes a prefix sum.
    /// </summary>
    /// <param name="data">The data to be modified.</param>
    /// <param name="init">The initial value.</param>
    /// <returns>The next initial value.</returns>
    public static int Inverse(Span<int> data, int init)
    {
        data[0] += init;
        var optimalSize = data.Length / 4 * 4;
        var i = 1;
        if (optimalSize >= 4)
        {
            var adjustment = data[0];
            for (; i < optimalSize - 4; i += 4)
            {
                adjustment = data[i] += adjustment;
                adjustment = data[i + 1] += adjustment;
                adjustment = data[i + 2] += adjustment;
                adjustment = data[i + 3] += adjustment;
            }
        }

        for (; i < data.Length; i++)
        {
            data[i] += data[i - 1];
        }

        return data[^1];
    }
}