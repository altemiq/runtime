// -----------------------------------------------------------------------
// <copyright file="MemoryExtensions.IndexOfAny.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <contents>
/// Extensions for getting the index of any value.
/// </contents>
public static partial class MemoryExtensions
{
    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence of in the specified arguments.
    /// The search starts at a specified byte position and examines a specified number of positions.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="buffer"/>.</typeparam>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any sequence in any of <paramref name="first"/> or <paramref name="second"/>; -1 if no sequence was found.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> buffer, ReadOnlySpan<T> first, ReadOnlySpan<T> second)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer.Length == 0)
        {
            return -1;
        }

        var firstIndex = 0;
        var secondIndex = 0;
        for (var i = 0; i < buffer.Length; i++)
        {
            if (TestValue(buffer[i], ref first, ref firstIndex))
            {
                return i - first.Length + 1;
            }

            if (TestValue(buffer[i], ref second, ref secondIndex))
            {
                return i - second.Length + 1;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence of in the specified arguments.
    /// The search starts at a specified byte position and examines a specified number of positions.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="buffer"/>.</typeparam>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <param name="third">The third value.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any sequence in any of <paramref name="first"/>, <paramref name="second"/>, or <paramref name="third"/>; -1 if no sequence was found.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> buffer, ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer.Length == 0)
        {
            return -1;
        }

        var firstIndex = 0;
        var secondIndex = 0;
        var thirdIndex = 0;
        for (var i = 0; i < buffer.Length; i++)
        {
            if (TestValue(buffer[i], ref first, ref firstIndex))
            {
                return i - first.Length + 1;
            }

            if (TestValue(buffer[i], ref second, ref secondIndex))
            {
                return i - second.Length + 1;
            }

            if (TestValue(buffer[i], ref third, ref thirdIndex))
            {
                return i - third.Length + 1;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence in the specified arguments.
    /// The search starts at a specified byte position and examines a specified number of positions.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="buffer"/>.</typeparam>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <param name="third">The third value.</param>
    /// <param name="forth">The forth value.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any sequence in any of <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, or <paramref name="forth"/>; -1 if no sequence was found.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> buffer, ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third, ReadOnlySpan<T> forth)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer.Length == 0)
        {
            return -1;
        }

        var firstIndex = 0;
        var secondIndex = 0;
        var thirdIndex = 0;
        var forthIndex = 0;
        for (var i = 0; i < buffer.Length; i++)
        {
            if (TestValue(buffer[i], ref first, ref firstIndex))
            {
                return i - first.Length + 1;
            }

            if (TestValue(buffer[i], ref second, ref secondIndex))
            {
                return i - second.Length + 1;
            }

            if (TestValue(buffer[i], ref third, ref thirdIndex))
            {
                return i - third.Length + 1;
            }

            if (TestValue(buffer[i], ref forth, ref forthIndex))
            {
                return i - forth.Length + 1;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence in the specified arguments.
    /// The search starts at a specified byte position and examines a specified number of positions.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="buffer"/>.</typeparam>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <param name="third">The third value.</param>
    /// <param name="forth">The forth value.</param>
    /// <param name="fifth">The fifth value.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any sequence in any of <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="forth"/>, or <paramref name="fifth"/>; -1 if no value sequence was found.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> buffer, ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third, ReadOnlySpan<T> forth, ReadOnlySpan<T> fifth)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer.Length == 0)
        {
            return -1;
        }

        var firstIndex = 0;
        var secondIndex = 0;
        var thirdIndex = 0;
        var forthIndex = 0;
        var fifthIndex = 0;
        for (var i = 0; i < buffer.Length; i++)
        {
            if (TestValue(buffer[i], ref first, ref firstIndex))
            {
                return i - first.Length + 1;
            }

            if (TestValue(buffer[i], ref second, ref secondIndex))
            {
                return i - second.Length + 1;
            }

            if (TestValue(buffer[i], ref third, ref thirdIndex))
            {
                return i - third.Length + 1;
            }

            if (TestValue(buffer[i], ref forth, ref forthIndex))
            {
                return i - forth.Length + 1;
            }

            if (TestValue(buffer[i], ref fifth, ref fifthIndex))
            {
                return i - fifth.Length + 1;
            }
        }

        return -1;
    }

    /// <summary>
    /// Reports the zero-based index of the first occurrence in this specified instance of any sequence in the specified arguments.
    /// The search starts at a specified byte position and examines a specified number of positions.
    /// </summary>
    /// <typeparam name="T">The type of elements in <paramref name="buffer"/>.</typeparam>
    /// <param name="buffer">The buffer to search.</param>
    /// <param name="first">The first value.</param>
    /// <param name="second">The second value.</param>
    /// <param name="third">The third value.</param>
    /// <param name="forth">The forth value.</param>
    /// <param name="fifth">The fifth value.</param>
    /// <param name="sixth">The sixth value.</param>
    /// <returns>The zero-based index position of the first occurrence in this instance where any sequence in any of <paramref name="first"/>, <paramref name="second"/>, <paramref name="third"/>, <paramref name="forth"/>, <paramref name="fifth"/>, or <paramref name="sixth"/>; -1 if no byte sequence was found.</returns>
    public static int IndexOfAny<T>(this ReadOnlySpan<T> buffer, ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third, ReadOnlySpan<T> forth, ReadOnlySpan<T> fifth, ReadOnlySpan<T> sixth)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer.Length == 0)
        {
            return -1;
        }

        var firstIndex = 0;
        var secondIndex = 0;
        var thirdIndex = 0;
        var forthIndex = 0;
        var fifthIndex = 0;
        var sixthIndex = 0;
        for (var i = 0; i < buffer.Length; i++)
        {
            if (TestValue(buffer[i], ref first, ref firstIndex))
            {
                return i - first.Length + 1;
            }

            if (TestValue(buffer[i], ref second, ref secondIndex))
            {
                return i - second.Length + 1;
            }

            if (TestValue(buffer[i], ref third, ref thirdIndex))
            {
                return i - third.Length + 1;
            }

            if (TestValue(buffer[i], ref forth, ref forthIndex))
            {
                return i - forth.Length + 1;
            }

            if (TestValue(buffer[i], ref fifth, ref fifthIndex))
            {
                return i - fifth.Length + 1;
            }

            if (TestValue(buffer[i], ref sixth, ref sixthIndex))
            {
                return i - sixth.Length + 1;
            }
        }

        return -1;
    }

    private static bool TestValue<T>(T value, ref ReadOnlySpan<T> test, ref int index)
        where T : IEquatable<T>
    {
        if (value.Equals(test[index]))
        {
            index++;
            if (index == test.Length)
            {
                return true;
            }
        }
        else
        {
            index = 0;
        }

        return false;
    }
}