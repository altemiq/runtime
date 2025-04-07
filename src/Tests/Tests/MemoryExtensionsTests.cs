// -----------------------------------------------------------------------
// <copyright file="MemoryExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using TUnit.Assertions.AssertConditions.Throws;

public class MemoryExtensionsTests
{
    [Test]
    [Arguments("This is an old string", "an old", "a new", "This is a new string")]
    [Arguments("This is an old string", "a new", "blah", "This is an old string")]
    public async Task Replace(string input, string old, string @new, string expected) => await Assert.That(input.AsSpan().Replace(old.AsSpan(), @new.AsSpan()).ToString()).IsEqualTo(expected);

    [Test]
    public async Task IndexOfAny()
    {
        await Assert.That(Span("This is an old string").IndexOfAny(Span("is"), Span("an"), Span("old"), Span("string"))).IsEqualTo(2);
        await Assert.That(default(ReadOnlySpan<char>).IndexOfAny(default, default)).IsEqualTo(-1);

        static ReadOnlySpan<char> Span(string input)
        {
            return input.AsSpan();
        }
    }

    [Test]
    public async Task MoveNextOrThrowOnEmptyString() => await Assert.That(() =>
    {
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(string.Empty.AsSpan(), ' ');
#else
            string.Empty.AsSpan().Split();
#endif
        enumerator.MoveNextOrThrow();
        enumerator.MoveNextOrThrow();
    }).Throws<InvalidOperationException>();

    [Test]
    public async Task SplitOnChar()
    {
        const string Value = "This is a split string";
        var span = Value.AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, ' ');
#else
            span.Split();
#endif
        await Assert.That(GetStringValues(span, enumerator)).IsEquivalentTo(["This", "is", "a", "split", "string"]);
    }

    [Test]
    public async Task SplitEmptyValuesOnChar()
    {
        const string EmptyValues = ",,,,";
        var span = EmptyValues.AsSpan();
        var enumerator = span.Split(',', StringSplitOptions.RemoveEmptyEntries);
        await Assert.That(enumerator.MoveNext()).IsFalse();
    }

    [Test]
    [Arguments(" ")]
    [Arguments(",")]
    [Arguments("||")]
    public async Task SplitOnString(string separator)
    {
        var values = new[] { "This", "is", "a", "split", "string" };
        var value = string.Join(separator, values);
        var span = value.AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, separator.AsSpan());
#else
            span.Split(separator.AsSpan());
#endif
        await Assert.That(GetStringValues(span, enumerator)).IsEquivalentTo(values);
    }

    [Test]
    public async Task SplitOnMultipleString()
    {
        const string First = "This,is";
        const string Second = "a";
        const string Third = "split";
        const string Forth = "string";
        const string Space = " ";
        const string Pipe = "|";
        const string Value = $"{First}{Pipe}{Second}{Space}{Third}{Pipe}{Forth}";
        var span = Value.AsSpan();
        await Assert.That(GetStringValues(span, span.Split(Space, Pipe))).IsEquivalentTo([First, Second, Third, Forth]);
    }

    [Test]
    public async Task SplitEmptyValuesOnString()
    {
        const string EmptyValues = ",,,,";
        var span = EmptyValues.AsSpan();
        var enumerator = span.Split(",".AsSpan(), StringSplitOptions.RemoveEmptyEntries);
        await Assert.That(enumerator.MoveNext()).IsFalse();
    }

    [Test]
    [Arguments(' ')]
    [Arguments(',')]
    [Arguments('|')]
    public async Task SplitQuotedOnChar(char separator)
    {
        var stringSeparator = new string(separator, 1);
        var values = new[] { "This", "\"is", "a\"", "split", "string" };
        var value = string.Join(stringSeparator, values);
        var span = value.AsSpan();
        await Assert.That(GetStringValues(span, span.SplitQuoted(separator))).IsEquivalentTo(
        [
            values[0],
            string.Join(stringSeparator, values[1], values[2]),
            values[3],
            values[4],
        ]);
    }

    [Test]
    [Arguments(' ')]
    [Arguments(',')]
    [Arguments('|')]
    public async Task SplitQuotedOnCharWithMultiple(char separator)
    {
        var stringSeparator = new string(separator, 1);
        var values = new[] { "This", "\"is", "a", "split\"", "string" };
        var value = string.Join(stringSeparator, values);
        var span = value.AsSpan();
        await Assert.That(GetStringValues(span, span.SplitQuoted(separator))).IsEquivalentTo(
        [
            values[0],
            string.Join(stringSeparator, values[1], values[2], values[3]),
            values[4]
        ]);
    }

    [Test]
    [Arguments(" ")]
    [Arguments(",")]
    [Arguments("||")]
    public async Task SplitQuotedOnString(string separator)
    {
        var values = new[] { "This", "\"is", "a\"", "split", "string" };
        var value = string.Join(separator, values);
        var span = value.AsSpan();
        await Assert.That(GetStringValues(span, span.SplitQuoted(separator.AsSpan()))).IsEquivalentTo(
        [
            values[0],
            string.Join(separator, values[1], values[2]),
            values[3],
            values[4]
        ]);
    }

    [Test]
    [Arguments(" ")]
    [Arguments(",")]
    [Arguments("||")]
    public async Task SplitQuotedOnStringWithMultiple(string separator)
    {
        var values = new[] { "This", "\"is", "a", "split\"", "string" };
        var value = string.Join(separator, values);
        var span = value.AsSpan();
        await Assert.That(GetStringValues(span, span.SplitQuoted(separator.AsSpan()))).IsEquivalentTo(
        [
            values[0],
            string.Join(separator, values[1], values[2], values[3]),
            values[4]
        ]);
    }

    [Test]
    public async Task GetEnumValues()
    {
        var value = $"{nameof(EnumValue.First)}|{nameof(EnumValue.Second)}|{nameof(EnumValue.Third)}";
        var span = value.AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        var first = span.GetNextEnum<EnumValue>(ref enumerator, true);
        var second = span.GetNextEnum<EnumValue>(ref enumerator);
        var third = span.GetNextEnum<EnumValue>(ref enumerator, true);
        await Assert.That(first).IsEqualTo(EnumValue.First);
        await Assert.That(second).IsEqualTo(EnumValue.Second);
        await Assert.That(third).IsEqualTo(EnumValue.Third);

    }

    [Test]
    public async Task TryGetEnumValues()
    {
        var value = $"{nameof(EnumValue.First)}|{nameof(EnumValue.Second)}|{nameof(EnumValue.Third)}";
        var span = value.AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        var firstResult = span.TryGetNextEnum<EnumValue>(ref enumerator, true, out var firstEnum);
        var secondResult = span.TryGetNextEnum<EnumValue>(ref enumerator, out var secondEnum);
        var thirdResult = span.TryGetNextEnum<EnumValue>(ref enumerator, true, out var thirdEnum);
        var forthResult = span.TryGetNextEnum<EnumValue>(ref enumerator, out _);

        await Assert.That(firstResult).IsTrue();
        await Assert.That(firstEnum).IsEqualTo(EnumValue.First);
        await Assert.That(secondResult).IsTrue();
        await Assert.That(secondEnum).IsEqualTo(EnumValue.Second);
        await Assert.That(thirdResult).IsTrue();
        await Assert.That(thirdEnum).IsEqualTo(EnumValue.Third);
        await Assert.That(forthResult).IsFalse();
    }

    private static List<string> GetStringValues(ReadOnlySpan<char> span, SpanSplitEnumerator<char> enumerator)
    {
        var results = new List<string>();
        while (span.TryGetNextString(ref enumerator, out var result))
        {
            results.Add(result);
        }

        return results;
    }

    private static List<string> GetStringValues(ReadOnlySpan<char> span, JoinedSpanSplitEnumerator<char> enumerator)
    {
        var results = new List<string>();
        while (enumerator.MoveNext())
        {
            results.Add(span[enumerator.Current].ToString());
        }

        return results;
    }

#if NET9_0_OR_GREATER
    private static List<string> GetStringValues(ReadOnlySpan<char> span, System.MemoryExtensions.SpanSplitEnumerator<char> enumerator)
    {
        var results = new List<string>();
        while (span.TryGetNextString(ref enumerator, out var result))
        {
            results.Add(result);
        }

        return results;
    }
#endif

#if NETCOREAPP2_1_OR_GREATER
    public class GetValues
    {
        [Test]
        [MethodDataSource(nameof(GetCharValuesData))]
        public async Task CharSeparated(Func<Random, int, double> creator, GetValuesCharDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(i => creator(random, i)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            var parsedValues = getValues(span, '|', 10, System.Globalization.CultureInfo.CurrentCulture);
            await Assert.That(parsedValues).IsEquivalentTo(randomValues);
        }

        public static Func<(Func<Random, int, double>, GetValuesCharDelegate<double>)> GetCharValuesData() => () => (static (random, _) => random.NextDouble(), MemoryExtensions.GetDoubleValues);

        [Test]
        [MethodDataSource(nameof(GetStringValuesData))]
        public async Task StringSeparated(Func<Random, int, double> creator, GetValuesSpanDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(i => creator(random, i)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            var parsedValues = getValues(span, "|", 10, System.Globalization.CultureInfo.CurrentCulture);
            await Assert.That(parsedValues).IsEquivalentTo(randomValues);
        }

        public static Func<(Func<Random, int, double>, GetValuesSpanDelegate<double>)> GetStringValuesData()
        {
            return () => (static (random, _) => random.NextDouble(), MemoryExtensions.GetDoubleValues);
        }
    }

    [Test]
    [MethodDataSource(nameof(GetData))]
    public async Task Get(Func<Random, int, object> creator, GetDelegateObject parser, System.Globalization.NumberStyles style)
    {
        var random = new Random();

        var randomValues = System.Linq.Enumerable.Range(0, 10).Select(i => creator(random, i)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        IList<object?> values = [];
        IList<Exception?> exceptions = [];
        for (var i = 0; i < 10; i++)
        {
            try
            {
                values.Add(parser(span, ref enumerator, style, System.Globalization.CultureInfo.CurrentCulture));
            }
            catch (InvalidOperationException e)
            {
                exceptions.Add(e);
            }
        }

        await Assert.That(enumerator.MoveNext()).IsFalse();
        await Assert.That(exceptions).IsEmpty();
        await Assert.That(values).HasCount().EqualTo(10).And.IsEquivalentTo(randomValues);
    }

    public static IEnumerable<Func<(Func<Random, int, object>, GetDelegateObject, System.Globalization.NumberStyles)>> GetData()
    {
        yield return () => (static (random, _) => random.NextDouble(), CreateDelegate(MemoryExtensions.GetNextDouble), System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float);
        yield return () => (static (random, _) => (float)random.NextDouble(), CreateDelegate(MemoryExtensions.GetNextSingle), System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float);
        yield return () => (static (random, _) => (byte)random.Next(byte.MaxValue), CreateDelegate(MemoryExtensions.GetNextByte), System.Globalization.NumberStyles.Integer);
        yield return () => (static (random, _) => (short)random.Next(short.MaxValue), CreateDelegate(MemoryExtensions.GetNextInt16), System.Globalization.NumberStyles.Integer);
        yield return () => (static (random, _) => (ushort)random.Next(ushort.MaxValue), CreateDelegate(MemoryExtensions.GetNextUInt16), System.Globalization.NumberStyles.Integer);
        yield return () => (static (random, _) => random.Next(), CreateDelegate(MemoryExtensions.GetNextInt32), System.Globalization.NumberStyles.Integer);
        yield return () => (static (random, _) => (uint)random.Next(), CreateDelegate(MemoryExtensions.GetNextUInt32), System.Globalization.NumberStyles.Integer);
        yield return () => (static (random, _) => (long)random.Next() + int.MaxValue, CreateDelegate(MemoryExtensions.GetNextInt64), System.Globalization.NumberStyles.Integer);
        yield return () => (static (random, _) => (ulong)random.Next() + uint.MaxValue, CreateDelegate(MemoryExtensions.GetNextUInt64), System.Globalization.NumberStyles.Integer);

        static GetDelegateObject CreateDelegate<T>(GetDelegate<T> @delegate)
        {
            return GetDelegateFunc;

            object? GetDelegateFunc(
                ReadOnlySpan<char> span,
#if NET9_0_OR_GREATER
                ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator,
#else
                ref SpanSplitEnumerator<char> enumerator,
#endif
                System.Globalization.NumberStyles style,
                IFormatProvider? provider)
            {
                return @delegate(span, ref enumerator, style, provider);
            }
        }
    }

    public class TryGetValues
    {
        [Test]
        [MethodDataSource(nameof(TryGetValuesCharData))]
        public async Task CharSeparated(Func<Random, int, double> creator, TryGetValuesCharDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(i => creator(random, i)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            await Assert.That(getValues(span, '|', 10, System.Globalization.CultureInfo.CurrentCulture, out var parsedValues)).IsTrue();
            await Assert.That(parsedValues).IsEquivalentTo(randomValues);
        }

        public static Func<(Func<Random, int, double>, TryGetValuesCharDelegate<double>)> TryGetValuesCharData() => () => (static (random, _) => random.NextDouble(), MemoryExtensions.TryGetDoubleValues);

        [Test]
        [MethodDataSource(nameof(TryGetValuesSpanData))]
        public async Task StringSeparated(Func<Random, int, double> creator, TryGetValuesSpanDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(i => creator(random, i)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            await Assert.That(getValues(span, "|".AsSpan(), 10, System.Globalization.CultureInfo.CurrentCulture, out var parsedValues)).IsTrue();
            await Assert.That(parsedValues).IsEquivalentTo(randomValues);
        }

        public static Func<(Func<Random, int, double>, TryGetValuesSpanDelegate<double>)> TryGetValuesSpanData() => () => (static (random, _) => random.NextDouble(), MemoryExtensions.TryGetDoubleValues);
    }

    [Test]
    [MethodDataSource(nameof(TryGetData))]
    public async Task TryGet(Func<Random, int, object> creator, TryGetDelegateObject parser)
    {
        var random = new Random();
        var randomValues = System.Linq.Enumerable.Range(0, 10).Select(i => creator(random, i)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        IList<object> values = [];
        while (parser(span, ref enumerator, System.Globalization.CultureInfo.CurrentCulture, out var value))
        {
            values.Add(value);
        }

        await Assert.That(enumerator.MoveNext()).IsFalse();
        await Assert.That(values).HasCount().EqualTo(10).And.IsEquivalentTo(randomValues);
    }

    public static IEnumerable<Func<(Func<Random, int, object>, TryGetDelegateObject)>> TryGetData()
    {
        yield return () => (static (random, _) => random.NextDouble(), CreateDelegate<double>(MemoryExtensions.TryGetNextDouble));
        yield return () => (static (random, _) => (float)random.NextDouble(), CreateDelegate<float>(MemoryExtensions.TryGetNextSingle));
        yield return () => (static (random, _) => (byte)random.Next(byte.MaxValue), CreateDelegate<byte>(MemoryExtensions.TryGetNextByte));
        yield return () => (static (random, _) => (short)random.Next(short.MaxValue), CreateDelegate<short>(MemoryExtensions.TryGetNextInt16));
        yield return () => (static (random, _) => (ushort)random.Next(ushort.MaxValue), CreateDelegate<ushort>(MemoryExtensions.TryGetNextUInt16));
        yield return () => (static (random, _) => random.Next(), CreateDelegate<int>(MemoryExtensions.TryGetNextInt32));
        yield return () => (static (random, _) => (uint)random.Next(), CreateDelegate<uint>(MemoryExtensions.TryGetNextUInt32));
        yield return () => (static (random, _) => (long)random.Next() + int.MaxValue, CreateDelegate<long>(MemoryExtensions.TryGetNextInt64));
        yield return () => (static (random, _) => (ulong)random.Next() + uint.MaxValue, CreateDelegate<ulong>(MemoryExtensions.TryGetNextUInt64));

        static TryGetDelegateObject CreateDelegate<T>(TryGetDelegate<T> @delegate)
        {
            return TryGetDelegateFunc;

            bool TryGetDelegateFunc(
                ReadOnlySpan<char> span,
#if NET9_0_OR_GREATER
                ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator,
#else
                ref SpanSplitEnumerator<char> enumerator,
#endif
                IFormatProvider? provider,
                [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out object? value)
            {
                var result = @delegate(span, ref enumerator, provider, out var item);
                value = item;
                return result;
            }
        }
    }

    public delegate bool TryGetValuesSpanDelegate<TResult>(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, int count, IFormatProvider? provider, out TResult[]? output);

    public delegate bool TryGetValuesCharDelegate<TResult>(ReadOnlySpan<char> input, char separator, int count, IFormatProvider? provider, out TResult[]? output);

    public delegate bool TryGetDelegateObject(
        ReadOnlySpan<char> span,
#if NET9_0_OR_GREATER
        ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator,
#else
        ref SpanSplitEnumerator<char> enumerator,
#endif
        IFormatProvider? provider,
        [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out object? value);

    private delegate bool TryGetDelegate<T>(
        ReadOnlySpan<char> span,
#if NET9_0_OR_GREATER
        ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator,
#else
        ref SpanSplitEnumerator<char> enumerator,
#endif
        IFormatProvider? provider,
        out T value);

    public delegate TResult[] GetValuesSpanDelegate<out TResult>(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, int count, IFormatProvider? provider);

    public delegate TResult[] GetValuesCharDelegate<out TResult>(ReadOnlySpan<char> input, char separator, int count, IFormatProvider? provider);

    public delegate object? GetDelegateObject(
        ReadOnlySpan<char> span,
#if NET9_0_OR_GREATER
        ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator,
#else
        ref SpanSplitEnumerator<char> enumerator,
#endif
        System.Globalization.NumberStyles style,
        IFormatProvider? provider);

    private delegate T GetDelegate<out T>(
        ReadOnlySpan<char> span,
#if NET9_0_OR_GREATER
        ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator,
#else
        ref SpanSplitEnumerator<char> enumerator,
#endif
        System.Globalization.NumberStyles style,
        IFormatProvider? provider);
#endif

    private enum EnumValue
    {
        First,
        Second,
        Third,
    }
}