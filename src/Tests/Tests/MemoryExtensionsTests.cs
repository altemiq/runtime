// -----------------------------------------------------------------------
// <copyright file="MemoryExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class MemoryExtensionsTests
{
    [Theory]
    [InlineData("This is an old string", "an old", "a new", "This is a new string")]
    [InlineData("This is an old string", "a new", "blah", "This is an old string")]
    public void Replace(string input, string old, string @new, string expected) => Assert.Equal(expected, input.AsSpan().Replace(old.AsSpan(), @new.AsSpan()).ToString());

    [Fact]
    public void IndexOfAny()
    {
        Assert.Equal(2, Create("This is an old string").IndexOfAny(Create("is"), Create("an"), Create("old"), Create("string")));
        Assert.Equal(-1, default(ReadOnlySpan<char>).IndexOfAny(default, default));

        static ReadOnlySpan<char> Create(string input)
        {
            return input.AsSpan();
        }
    }

#if !NET9_0_OR_GREATER
    [Fact]
    public void MoveNextOnEmptyString()
    {
        var enumerator = string.Empty.AsSpan().Split();
        Assert.True(enumerator.MoveNext());
        Assert.False(enumerator.MoveNext());
    }
#endif

    [Fact]
    public void MoveNextOrThrowOnEmptyString() => Assert.Throws<InvalidOperationException>(() =>
    {
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(string.Empty.AsSpan(), ' ');
#else
            string.Empty.AsSpan().Split();
#endif
        enumerator.MoveNextOrThrow();
        enumerator.MoveNextOrThrow();
    });

    [Fact]
    public void SplitOnChar()
    {
        const string Value = "This is a split string";
        var span = Value.AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, ' ');
#else
            span.Split();
#endif
        Assert.Equal("This", span.GetNextString(ref enumerator));
        Assert.Equal("is", span.GetNextString(ref enumerator));
        Assert.Equal("a", span.GetNextString(ref enumerator));
        Assert.Equal("split", span.GetNextString(ref enumerator));
        Assert.Equal("string", span.GetNextString(ref enumerator));
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void SplitEmptyValuesOnChar()
    {
        const string EmptyValues = ",,,,";
        var span = EmptyValues.AsSpan();
        var enumerator = span.Split(',', StringSplitOptions.RemoveEmptyEntries);
        Assert.False(enumerator.MoveNext());
    }

    [Theory]
    [InlineData(" ")]
    [InlineData(",")]
    [InlineData("||")]
    public void SplitOnString(string separator)
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
        Assert.Equal(values[0], span.GetNextString(ref enumerator));
        Assert.Equal(values[1], span.GetNextString(ref enumerator));
        Assert.Equal(values[2], span.GetNextString(ref enumerator));
        Assert.Equal(values[3], span.GetNextString(ref enumerator));
        Assert.Equal(values[4], span.GetNextString(ref enumerator));
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void SplitOnMultipleString()
    {
        const string First = "This,is";
        const string Second = "a";
        const string Third = "split";
        const string Forth = "string";
        const string Space = " ";
        const string Pipe = "|";
        const string Value = $"{First}{Pipe}{Second}{Space}{Third}{Pipe}{Forth}";
        var span = Value.AsSpan();
        var enumerator = span.Split(Space, Pipe);
        Assert.Equal(First, span.GetNextString(ref enumerator));
        Assert.Equal(Second, span.GetNextString(ref enumerator));
        Assert.Equal(Third, span.GetNextString(ref enumerator));
        Assert.Equal(Forth, span.GetNextString(ref enumerator));
    }

    [Fact]
    public void SplitEmptyValuesOnString()
    {
        const string EmptyValues = ",,,,";
        var span = EmptyValues.AsSpan();
        var enumerator = span.Split(",".AsSpan(), StringSplitOptions.RemoveEmptyEntries);
        Assert.False(enumerator.MoveNext());
    }

    [Theory]
    [InlineData(' ')]
    [InlineData(',')]
    [InlineData('|')]
    public void SplitQuotedOnChar(char separator)
    {
        var stringSeparator = new string(separator, 1);
        var values = new[] { "This", "\"is", "a\"", "split", "string" };
        var value = string.Join(stringSeparator, values);
        var span = value.AsSpan();
        var enumerator = span.SplitQuoted(separator);
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[0], span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(string.Join(stringSeparator, values[1], values[2]), span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[3], span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[4], span[enumerator.Current].ToString());
        Assert.False(enumerator.MoveNext());
    }

    [Theory]
    [InlineData(' ')]
    [InlineData(',')]
    [InlineData('|')]
    public void SplitQuotedOnCharWithMultiple(char separator)
    {
        var stringSeparator = new string(separator, 1);
        var values = new[] { "This", "\"is", "a", "split\"", "string" };
        var value = string.Join(stringSeparator, values);
        var span = value.AsSpan();
        var enumerator = span.SplitQuoted(separator);
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[0], span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(string.Join(stringSeparator, values[1], values[2], values[3]), span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[4], span[enumerator.Current].ToString());
        Assert.False(enumerator.MoveNext());
    }

    [Theory]
    [InlineData(" ")]
    [InlineData(",")]
    [InlineData("||")]
    public void SplitQuotedOnString(string separator)
    {
        var values = new[] { "This", "\"is", "a\"", "split", "string" };
        var value = string.Join(separator, values);
        var span = value.AsSpan();
        var enumerator = span.SplitQuoted(separator.AsSpan());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[0], span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(string.Join(separator, values[1], values[2]), span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[3], span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[4], span[enumerator.Current].ToString());
        Assert.False(enumerator.MoveNext());
    }

    [Theory]
    [InlineData(" ")]
    [InlineData(",")]
    [InlineData("||")]
    public void SplitQuotedOnStringWithMultiple(string separator)
    {
        var values = new[] { "This", "\"is", "a", "split\"", "string" };
        var value = string.Join(separator, values);
        var span = value.AsSpan();
        var enumerator = span.SplitQuoted(separator.AsSpan());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[0], span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(string.Join(separator, values[1], values[2], values[3]), span[enumerator.Current].ToString());
        Assert.True(enumerator.MoveNext());
        Assert.Equal(values[4], span[enumerator.Current].ToString());
        Assert.False(enumerator.MoveNext());
    }

    [Fact]
    public void GetEnumValues()
    {
        var value = $"{nameof(EnumValue.First)}|{nameof(EnumValue.Second)}|{nameof(EnumValue.Third)}";
        var span = value.AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        Assert.Equal(EnumValue.First, span.GetNextEnum<EnumValue>(ref enumerator, true));
        Assert.Equal(EnumValue.Second, span.GetNextEnum<EnumValue>(ref enumerator));
        Assert.Equal(EnumValue.Third, span.GetNextEnum<EnumValue>(ref enumerator, true));
    }

    [Fact]
    public void TryGetEnumValues()
    {
        var value = $"{nameof(EnumValue.First)}|{nameof(EnumValue.Second)}|{nameof(EnumValue.Third)}";
        var span = value.AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        Assert.True(span.TryGetNextEnum<EnumValue>(ref enumerator, true, out var enumValue));
        Assert.Equal(EnumValue.First, enumValue);
        Assert.True(span.TryGetNextEnum(ref enumerator, out enumValue));
        Assert.Equal(EnumValue.Second, enumValue);
        Assert.True(span.TryGetNextEnum(ref enumerator, true, out enumValue));
        Assert.Equal(EnumValue.Third, enumValue);
        Assert.False(span.TryGetNextEnum<EnumValue>(ref enumerator, out _));
    }

#if NETCOREAPP2_1_OR_GREATER
    public class GetValues
    {
        [Theory]
        [MemberData(nameof(GetCharValuesData))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1044:Avoid using TheoryData type arguments that are not serializable", Justification = "Checked")]
        public void CharSeparated(Func<Random, int, double> creator, GetValuesCharDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            var parsedValues = getValues(span, '|', 10, System.Globalization.CultureInfo.CurrentCulture);
            Assert.Equal(randomValues, parsedValues);
        }

        public static TheoryData<Func<Random, int, double>, GetValuesCharDelegate<double>> GetCharValuesData() => new() { { static (Random random, int _) => random.NextDouble(), new GetValuesCharDelegate<double>(MemoryExtensions.GetDoubleValues) } };

        [Theory]
        [MemberData(nameof(GetStringValuesData))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1044:Avoid using TheoryData type arguments that are not serializable", Justification = "Checked")]
        public void StringSeparated(Func<Random, int, double> creator, GetValuesSpanDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            var parsedValues = getValues(span, "|", 10, System.Globalization.CultureInfo.CurrentCulture);
            Assert.Equal(randomValues, parsedValues);
        }

        public static TheoryData<Func<Random, int, double>, GetValuesSpanDelegate<double>> GetStringValuesData() => new()
        {
            { static (Random random, int _) => random.NextDouble(), new GetValuesSpanDelegate<double>(MemoryExtensions.GetDoubleValues) }
        };
    }

    [Theory]
    [MemberData(nameof(GetData))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1042:The member referenced by the MemberData attribute returns untyped data rows", Justification = "This cannot be expressed by TheoryData")]
    public void Get<T>(Func<Random, int, T> creator, GetDelegate<T> parser, System.Globalization.NumberStyles style)
    {
        var random = new Random();
        var randomValues = System.Linq.Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        var count = 0;
        for (var i = 0; i < 10; i++)
        {
            Exception? ex = default;
            var value = default(T);
            try
            {
                value = parser(span, ref enumerator, style, System.Globalization.CultureInfo.CurrentCulture);
            }
            catch (InvalidOperationException e)
            {
                ex = e;
            }

            Assert.Null(ex);
            Assert.Equal(randomValues[i], value);
            count++;
        }

        Assert.Equal(10, count);
        Assert.False(enumerator.MoveNext());
    }

    public static IEnumerable<object[]> GetData
    {
        get
        {
            yield return new object[] { static (Random random, int _) => random.NextDouble(), new GetDelegate<double>(MemoryExtensions.GetNextDouble), System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float };
            yield return new object[] { static (Random random, int _) => (float)random.NextDouble(), new GetDelegate<float>(MemoryExtensions.GetNextSingle), System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float };
            yield return new object[] { static (Random random, int _) => (byte)random.Next(byte.MaxValue), new GetDelegate<byte>(MemoryExtensions.GetNextByte), System.Globalization.NumberStyles.Integer };
            yield return new object[] { static (Random random, int _) => (short)random.Next(short.MaxValue), new GetDelegate<short>(MemoryExtensions.GetNextInt16), System.Globalization.NumberStyles.Integer };
            yield return new object[] { static (Random random, int _) => (ushort)random.Next(ushort.MaxValue), new GetDelegate<ushort>(MemoryExtensions.GetNextUInt16), System.Globalization.NumberStyles.Integer };
            yield return new object[] { static (Random random, int _) => random.Next(), new GetDelegate<int>(MemoryExtensions.GetNextInt32), System.Globalization.NumberStyles.Integer };
            yield return new object[] { static (Random random, int _) => (uint)random.Next(), new GetDelegate<uint>(MemoryExtensions.GetNextUInt32), System.Globalization.NumberStyles.Integer };
            yield return new object[] { static (Random random, int _) => (long)random.Next() + int.MaxValue, new GetDelegate<long>(MemoryExtensions.GetNextInt64), System.Globalization.NumberStyles.Integer };
            yield return new object[] { static (Random random, int _) => (ulong)random.Next() + uint.MaxValue, new GetDelegate<ulong>(MemoryExtensions.GetNextUInt64), System.Globalization.NumberStyles.Integer };
        }
    }

    public class TryGetValues
    {
        [Theory]
        [MemberData(nameof(TryGetValuesCharData))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1044:Avoid using TheoryData type arguments that are not serializable", Justification = "Checked")]
        public void CharSeparated(Func<Random, int, double> creator, TryGetValuesCharDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            Assert.True(getValues(span, '|', 10, System.Globalization.CultureInfo.CurrentCulture, out var parsedValues));
            Assert.Equal(randomValues, parsedValues);
        }

        public static TheoryData<Func<Random, int, double>, TryGetValuesCharDelegate<double>> TryGetValuesCharData() => new() { { static (Random random, int _) => random.NextDouble(), new TryGetValuesCharDelegate<double>(MemoryExtensions.TryGetDoubleValues) } };

        [Theory]
        [MemberData(nameof(TryGetValuesSpanData))]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1044:Avoid using TheoryData type arguments that are not serializable", Justification = "Checked")]
        public void StringSeparated(Func<Random, int, double> creator, TryGetValuesSpanDelegate<double> getValues)
        {
            var random = new Random();
            var randomValues = System.Linq.Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
            var span = string.Join("|", randomValues).AsSpan();
            Assert.True(getValues(span, "|".AsSpan(), 10, System.Globalization.CultureInfo.CurrentCulture, out var parsedValues));
            Assert.Equal(randomValues, parsedValues);
        }

        public static TheoryData<Func<Random, int, double>, TryGetValuesSpanDelegate<double>> TryGetValuesSpanData() => new() { { static (Random random, int _) => random.NextDouble(), new TryGetValuesSpanDelegate<double>(MemoryExtensions.TryGetDoubleValues) } };
    }

    [Theory]
    [MemberData(nameof(TryGetData))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1042:The member referenced by the MemberData attribute returns untyped data rows", Justification = "This cannot be expressed by TheoryData")]
    public static void TryGet<T>(Func<Random, int, T> creator, TryGetDelegate<T> parser)
    {
        var random = new Random();
        var randomValues = System.Linq.Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        var enumerator =
#if NET9_0_OR_GREATER
            System.MemoryExtensions.Split(span, '|');
#else
            span.Split('|');
#endif
        var count = 0;
        while (parser(span, ref enumerator, System.Globalization.CultureInfo.CurrentCulture, out var value))
        {
            Assert.Equal(randomValues[count], value);
            count++;
        }

        Assert.Equal(10, count);
        Assert.False(enumerator.MoveNext());
    }

    public static IEnumerable<object[]> TryGetData
    {
        get
        {
            yield return new object[] { static (Random random, int _) => random.NextDouble(), new TryGetDelegate<double>(MemoryExtensions.TryGetNextDouble) };
            yield return new object[] { static (Random random, int _) => (float)random.NextDouble(), new TryGetDelegate<float>(MemoryExtensions.TryGetNextSingle) };
            yield return new object[] { static (Random random, int _) => (byte)random.Next(byte.MaxValue), new TryGetDelegate<byte>(MemoryExtensions.TryGetNextByte) };
            yield return new object[] { static (Random random, int _) => (short)random.Next(short.MaxValue), new TryGetDelegate<short>(MemoryExtensions.TryGetNextInt16) };
            yield return new object[] { static (Random random, int _) => (ushort)random.Next(ushort.MaxValue), new TryGetDelegate<ushort>(MemoryExtensions.TryGetNextUInt16) };
            yield return new object[] { static (Random random, int _) => random.Next(), new TryGetDelegate<int>(MemoryExtensions.TryGetNextInt32) };
            yield return new object[] { static (Random random, int _) => (uint)random.Next(), new TryGetDelegate<uint>(MemoryExtensions.TryGetNextUInt32) };
            yield return new object[] { static (Random random, int _) => (long)random.Next() + int.MaxValue, new TryGetDelegate<long>(MemoryExtensions.TryGetNextInt64) };
            yield return new object[] { static (Random random, int _) => (ulong)random.Next() + uint.MaxValue, new TryGetDelegate<ulong>(MemoryExtensions.TryGetNextUInt64) };
        }
    }

    public delegate bool TryGetValuesSpanDelegate<TResult>(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, int count, IFormatProvider? provider, out TResult[]? output);

    public delegate bool TryGetValuesCharDelegate<TResult>(ReadOnlySpan<char> input, char separator, int count, IFormatProvider? provider, out TResult[]? output);

    public delegate bool TryGetDelegate<T>(
        ReadOnlySpan<char> span,
#if NET9_0_OR_GREATER
        ref System.MemoryExtensions.SpanSplitEnumerator<char> enumerator,
#else
        ref SpanSplitEnumerator<char> enumerator,
#endif
        IFormatProvider? provider,
        out T value);

    public delegate TResult[] GetValuesSpanDelegate<TResult>(ReadOnlySpan<char> input, ReadOnlySpan<char> separator, int count, IFormatProvider? provider);

    public delegate TResult[] GetValuesCharDelegate<TResult>(ReadOnlySpan<char> input, char separator, int count, IFormatProvider? provider);

    public delegate T GetDelegate<T>(
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