// -----------------------------------------------------------------------
// <copyright file="EnumerableTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Linq;

public class EnumerableTests
{
    public class WhereNotNull
    {
        [Theory]
        [MemberData(nameof(GetClassSequences))]
        public void Class(string?[] sequence, int count) => sequence.WhereNotNull().Should().HaveCount(count);

        [Theory]
        [MemberData(nameof(GetStructSequences))]
        public void Struct(int?[] sequence, int count) => sequence.WhereNotNull().Should().HaveCount(count);

        public static TheoryData<string?[], int> GetClassSequences() => new()
        {
            { System.Linq.Enumerable.Repeat(string.Empty, 10).ToArray(), 10 },
            { System.Linq.Enumerable.Repeat(default(string), 10).ToArray(), 0 },
            { [string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default], 5 },
        };

        public static TheoryData<int?[], int> GetStructSequences() => new()
        {
            { System.Linq.Enumerable.Repeat((int?)1, 10).ToArray(), 10 },
            { System.Linq.Enumerable.Repeat(default(int?), 10).ToArray(), 0 },
            { [ default, 1, default, 2, default, 3, default, 4, default, 5], 5 },
        };
    }


    [Theory]
    [MemberData(nameof(GetClassSequences))]
    public void WhereNull(string?[] sequence, int count) => sequence.WhereNull().Should().HaveCount(count);

    public static TheoryData<string?[], int> GetClassSequences() => new()
    {
        { System.Linq.Enumerable.Repeat(string.Empty, 10).ToArray(), 0 },
        { System.Linq.Enumerable.Repeat(default(string), 10).ToArray(), 10 },
        { [string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default], 5 },
    };
}