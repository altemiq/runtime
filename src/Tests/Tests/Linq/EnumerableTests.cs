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
        [Test]
        [MethodDataSource(typeof(EnumerableTests), nameof(GetClassSequences))]
        public async Task Class(string?[] sequence, int count) => await Assert.That(sequence.WhereNotNull()).HasCount().EqualTo(sequence.Length - count);

        [Test]
        [MethodDataSource(nameof(GetStructSequences))]
        public async Task Struct(int?[] sequence, int count) => await Assert.That(sequence.WhereNotNull()).HasCount().EqualTo(count);

        public static IEnumerable<Func<(int?[], int)>> GetStructSequences()
        {
            yield return () => (System.Linq.Enumerable.Repeat((int?)1, 10).ToArray(), 10);
            yield return () => (System.Linq.Enumerable.Repeat(default(int?), 10).ToArray(), 0);
            yield return () => ([ default, 1, default, 2, default, 3, default, 4, default, 5], 5);
        }
    }

    [Test]
    [MethodDataSource(nameof(GetClassSequences))]
    public async Task WhereNull(string?[] sequence, int count) => await Assert.That(sequence.WhereNull()).HasCount().EqualTo(count);

    public static IEnumerable<Func<(string?[], int)>> GetClassSequences()
    {
        yield return () => (System.Linq.Enumerable.Repeat(string.Empty, 10).ToArray(), 0);
        yield return () => (System.Linq.Enumerable.Repeat(default(string), 10).ToArray(), 10);
        yield return () => ([string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default], 5);
    }
}