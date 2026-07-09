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
        public async Task Class(string?[] sequence, int count)
        {
            _ = await Assert.That(sequence.WhereNotNull()).Count().IsEqualTo(sequence.Length - count);
        }

        [Test]
        [MethodDataSource(nameof(GetStructSequences))]
        public async Task Struct(int?[] sequence, int count)
        {
            _ = await Assert.That(sequence.WhereNotNull()).Count().IsEqualTo(count);
        }

        public static IEnumerable<Func<(int?[], int)>> GetStructSequences()
        {
            yield return static () => (System.Linq.Enumerable.Repeat((int?)1, 10).ToArray(), 10);
            yield return static () => (System.Linq.Enumerable.Repeat(default(int?), 10).ToArray(), 0);
            yield return static () => ([default, 1, default, 2, default, 3, default, 4, default, 5], 5);
        }
    }

    [Test]
    [MethodDataSource(nameof(GetClassSequences))]
    public async Task WhereNull(string?[] sequence, int count)
    {
        _ = await Assert.That(sequence.WhereNull()).Count().IsEqualTo(count);
    }

    public static IEnumerable<Func<(string?[], int)>> GetClassSequences()
    {
        yield return static () => (System.Linq.Enumerable.Repeat(string.Empty, 10).ToArray(), 0);
        yield return static () => (System.Linq.Enumerable.Repeat(default(string), 10).ToArray(), 10);
        yield return static () => ([string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default, string.Empty, default], 5);
    }
}