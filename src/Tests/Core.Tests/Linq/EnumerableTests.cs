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
        public void Class(IEnumerable<object?> sequence, int count) => sequence.WhereNotNull().Should().HaveCount(count);

        [Theory]
        [MemberData(nameof(GetStructSequences))]
        public void Struct(IEnumerable<int?> sequence, int count) => sequence.WhereNotNull().Should().HaveCount(count);

        public static TheoryData<IEnumerable<object?>, int> GetClassSequences() => new()
        {
            { System.Linq.Enumerable.Repeat(new object(), 10), 10 },
            { System.Linq.Enumerable.Repeat(default(object), 10), 0 },
            { [ new object(), default, new object(), default, new object(), default, new object(), default, new object(), default], 5 },
        };

        public static TheoryData<IEnumerable<int?>, int> GetStructSequences() => new()
        {
            { System.Linq.Enumerable.Repeat((int?)1, 10), 10 },
            { System.Linq.Enumerable.Repeat(default(int?), 10), 0 },
            { [ default, 1, default, 2, default, 3, default, 4, default, 5], 5 },
        };
    }


    [Theory]
    [MemberData(nameof(GetClassSequences))]
    public void WhereNull(IEnumerable<object?> sequence, int count) => sequence.WhereNull().Should().HaveCount(count);

    public static TheoryData<IEnumerable<object?>, int> GetClassSequences() => new()
        {
            { System.Linq.Enumerable.Repeat(new object(), 10), 0 },
            { System.Linq.Enumerable.Repeat(default(object), 10), 10 },
            { [ new object(), default, new object(), default, new object(), default, new object(), default, new object(), default], 5 },
        };
}
