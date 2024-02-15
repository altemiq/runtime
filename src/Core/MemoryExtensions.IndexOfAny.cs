// -----------------------------------------------------------------------
// <copyright file="MemoryExtensions.IndexOfAny.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

/// <content>
/// Extensions for getting the index of any value.
/// </content>
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
        where T : IEquatable<T> => IndexOfAny(buffer, new Spans<T>(first, second));

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
        where T : IEquatable<T> => IndexOfAny(buffer, new Spans<T>(first, second, third));

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
        where T : IEquatable<T> => IndexOfAny(buffer, new Spans<T>(first, second, third, forth));

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
        where T : IEquatable<T> => IndexOfAny(buffer, new Spans<T>(first, second, third, forth, fifth));

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
        where T : IEquatable<T> => IndexOfAny(buffer, new Spans<T>(first, second, third, forth, fifth, sixth));

    private static int IndexOfAny<T>(ReadOnlySpan<T> buffer, Spans<T> spans)
        where T : IEquatable<T>
    {
        // Check to see if the buffer is applicable
        if (buffer.Length == 0)
        {
            return -1;
        }

        var indexes = new int[spans.Count];
        for (var i = 0; i < buffer.Length; i++)
        {
            var value = buffer[i];
            for (var j = 0; j < spans.Count; j++)
            {
                var index = indexes[j];
                var span = spans.GetSpan(j);
                if (TestValue(value, span, ref index))
                {
                    return i - span.Length + 1;
                }

                indexes[j] = index;
            }
        }

        return -1;

        static bool TestValue(T value, ReadOnlySpan<T> test, ref int index)
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

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
    private readonly ref struct Spans<T>
    {
        private readonly ReadOnlySpan<T> first;
        private readonly ReadOnlySpan<T> second;
        private readonly ReadOnlySpan<T> third;
        private readonly ReadOnlySpan<T> forth;
        private readonly ReadOnlySpan<T> fifth;
        private readonly ReadOnlySpan<T> sixth;

        public Spans(ReadOnlySpan<T> first, ReadOnlySpan<T> second)
        {
            this.first = first;
            this.second = second;
            this.Count = 2;
        }

        public Spans(ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third)
        {
            this.first = first;
            this.second = second;
            this.third = third;
            this.Count = 3;
        }

        public Spans(ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third, ReadOnlySpan<T> forth)
        {
            this.first = first;
            this.second = second;
            this.third = third;
            this.forth = forth;
            this.Count = 4;
        }

        public Spans(ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third, ReadOnlySpan<T> forth, ReadOnlySpan<T> fifth)
        {
            this.first = first;
            this.second = second;
            this.third = third;
            this.forth = forth;
            this.fifth = fifth;
            this.Count = 5;
        }

        public Spans(ReadOnlySpan<T> first, ReadOnlySpan<T> second, ReadOnlySpan<T> third, ReadOnlySpan<T> forth, ReadOnlySpan<T> fifth, ReadOnlySpan<T> sixth)
        {
            this.first = first;
            this.second = second;
            this.third = third;
            this.forth = forth;
            this.fifth = fifth;
            this.sixth = sixth;
            this.Count = 6;
        }

        public int Count { get; }

        public readonly ReadOnlySpan<T> GetSpan(int i) => i switch
        {
            0 => this.first,
            1 => this.second,
            2 => this.third,
            3 => this.forth,
            4 => this.fifth,
            5 => this.sixth,
            _ => throw new InvalidOperationException(),
        };
    }
}