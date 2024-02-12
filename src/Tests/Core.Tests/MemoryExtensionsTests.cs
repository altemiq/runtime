// -----------------------------------------------------------------------
// <copyright file="MemoryExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class MemoryExtensionsTests
{
    [Fact]
    public void MoveNextOnEmptyString() => string.Empty.AsSpan().Split().MoveNext().Should().BeFalse();

    [Fact]
    public void MoveNextOrThrowOnEmptyString() => string.Empty.Invoking(s =>
    {
        var enumerator = s.AsSpan().Split();
        enumerator.MoveNextOrThrow();
    }).Should().ThrowExactly<InvalidOperationException>();

    [Fact]
    public void SplitOnChar()
    {
        const string Value = "This is a split string";
        var span = Value.AsSpan();
        var enumerator = span.Split();
        _ = span.GetNextString(ref enumerator).Should().Be("This");
        _ = span.GetNextString(ref enumerator).Should().Be("is");
        _ = span.GetNextString(ref enumerator).Should().Be("a");
        _ = span.GetNextString(ref enumerator).Should().Be("split");
        _ = span.GetNextString(ref enumerator).Should().Be("string");
        _ = enumerator.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void SplitEmptyValuesOnChar()
    {
        const string EmptyValues = ",,,,";
        var span = EmptyValues.AsSpan();
        var enumerator = span.Split(',', StringSplitOptions.RemoveEmptyEntries);
        _ = enumerator.MoveNext().Should().BeFalse();
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
        var enumerator = span.Split(separator);
        _ = span.GetNextString(ref enumerator).Should().Be(values[0]);
        _ = span.GetNextString(ref enumerator).Should().Be(values[1]);
        _ = span.GetNextString(ref enumerator).Should().Be(values[2]);
        _ = span.GetNextString(ref enumerator).Should().Be(values[3]);
        _ = span.GetNextString(ref enumerator).Should().Be(values[4]);
        _ = enumerator.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void SplitEmptyValuesOnString()
    {
        const string EmptyValues = ",,,,";
        var span = EmptyValues.AsSpan();
        var enumerator = span.Split(",", StringSplitOptions.RemoveEmptyEntries);
        _ = enumerator.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void GetEnumValues()
    {
        var value = $"{nameof(EnumValue.First)}|{nameof(EnumValue.Second)}|{nameof(EnumValue.Third)}";
        var span = value.AsSpan();
        var enumerator = span.Split('|');
        _ = span.GetNextEnum<EnumValue>(ref enumerator, true).Should().Be(EnumValue.First);
        _ = span.GetNextEnum<EnumValue>(ref enumerator).Should().Be(EnumValue.Second);
        _ = span.GetNextEnum<EnumValue>(ref enumerator, true).Should().Be(EnumValue.Third);
    }

    [Fact]
    public void TryGetEnumValues()
    {
        var value = $"{nameof(EnumValue.First)}|{nameof(EnumValue.Second)}|{nameof(EnumValue.Third)}";
        var span = value.AsSpan();
        var enumerator = span.Split('|');
        _ = span.TryGetNextEnum<EnumValue>(ref enumerator, true, out var enumValue).Should().BeTrue();
        _ = enumValue.Should().Be(EnumValue.First);
        _ = span.TryGetNextEnum(ref enumerator, out enumValue).Should().BeTrue();
        _ = enumValue.Should().Be(EnumValue.Second);
        _ = span.TryGetNextEnum(ref enumerator, true, out enumValue).Should().BeTrue();
        _ = enumValue.Should().Be(EnumValue.Third);
        _ = span.TryGetNextEnum<EnumValue>(ref enumerator, out _).Should().BeFalse();
    }

#if NETCOREAPP2_1_OR_GREATER
    [Theory]
    [MemberData(nameof(GetValuesData))]
    public void GetValues(Func<Random, int, double> creator, GetValuesDelegate<double> getValues)
    {
        var random = new Random();
        var randomValues = Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        var parsedValues = getValues(span, '|', 10, System.Globalization.CultureInfo.CurrentCulture);
        _ = parsedValues.Should().BeEquivalentTo(randomValues);
    }

    public static TheoryData<Func<Random, int, double>, GetValuesDelegate<double>> GetValuesData() => new() { { (Random random, int _) => random.NextDouble(), new GetValuesDelegate<double>(MemoryExtensions.GetDoubleValues) } };

    [Theory]
    [MemberData(nameof(GetData))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1042:The member referenced by the MemberData attribute returns untyped data rows", Justification = "This cannot be expressed by TheoryData")]
    public void Get<T>(Func<Random, int, T> creator, GetDelegate<T> parser, System.Globalization.NumberStyles style)
    {
        var random = new Random();
        var randomValues = Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        var enumerator = span.Split("|");
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

            _ = ex.Should().BeNull();
            _ = value.Should().Be(randomValues[i]);
            count++;
        }

        _ = count.Should().Be(10);
        _ = enumerator.MoveNext().Should().BeFalse();
    }

    public static IEnumerable<object[]> GetData
    {
        get
        {
            yield return new object[] { (Random random, int _) => random.NextDouble(), new GetDelegate<double>(MemoryExtensions.GetNextDouble), System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float };
            yield return new object[] { (Random random, int _) => (float)random.NextDouble(), new GetDelegate<float>(MemoryExtensions.GetNextSingle), System.Globalization.NumberStyles.AllowThousands | System.Globalization.NumberStyles.Float };
            yield return new object[] { (Random random, int _) => (byte)random.Next(byte.MaxValue), new GetDelegate<byte>(MemoryExtensions.GetNextByte), System.Globalization.NumberStyles.Integer };
            yield return new object[] { (Random random, int _) => (short)random.Next(short.MaxValue), new GetDelegate<short>(MemoryExtensions.GetNextInt16), System.Globalization.NumberStyles.Integer };
            yield return new object[] { (Random random, int _) => (ushort)random.Next(ushort.MaxValue), new GetDelegate<ushort>(MemoryExtensions.GetNextUInt16), System.Globalization.NumberStyles.Integer };
            yield return new object[] { (Random random, int _) => random.Next(), new GetDelegate<int>(MemoryExtensions.GetNextInt32), System.Globalization.NumberStyles.Integer };
            yield return new object[] { (Random random, int _) => (uint)random.Next(), new GetDelegate<uint>(MemoryExtensions.GetNextUInt32), System.Globalization.NumberStyles.Integer };
            yield return new object[] { (Random random, int _) => (long)random.Next() + int.MaxValue, new GetDelegate<long>(MemoryExtensions.GetNextInt64), System.Globalization.NumberStyles.Integer };
            yield return new object[] { (Random random, int _) => (ulong)random.Next() + uint.MaxValue, new GetDelegate<ulong>(MemoryExtensions.GetNextUInt64), System.Globalization.NumberStyles.Integer };
        }
    }

    [Theory]
    [MemberData(nameof(TryGetValuesData))]
    public void TryGetValues(Func<Random, int, double> creator, TryGetValuesDelegate<double> getValues)
    {
        var random = new Random();
        var randomValues = Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        _ = getValues(span, '|', 10, System.Globalization.CultureInfo.CurrentCulture, out var parsedValues).Should().BeTrue();
        _ = parsedValues.Should().BeEquivalentTo(randomValues);
    }

    public static TheoryData<Func<Random, int, double>, TryGetValuesDelegate<double>> TryGetValuesData() => new() { { (Random random, int _) => random.NextDouble(), new TryGetValuesDelegate<double>(MemoryExtensions.TryGetDoubleValues) } };

    [Theory]
    [MemberData(nameof(TryGetData))]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "xUnit1042:The member referenced by the MemberData attribute returns untyped data rows", Justification = "This cannot be expressed by TheoryData")]
    public static void TryGet<T>(Func<Random, int, T> creator, TryGetDelegate<T> parser)
    {
        var random = new Random();
        var randomValues = Enumerable.Range(0, 10).Select(_ => creator(random, _)).ToArray();
        var span = string.Join("|", randomValues).AsSpan();
        var enumerator = span.Split("|");
        var count = 0;
        while (parser(span, ref enumerator, System.Globalization.CultureInfo.CurrentCulture, out var value))
        {
            _ = value.Should().Be(randomValues[count]);
            count++;
        }

        _ = count.Should().Be(10);
        _ = enumerator.MoveNext().Should().BeFalse();
    }

    public static IEnumerable<object[]> TryGetData
    {
        get
        {
            yield return new object[] { (Random random, int _) => random.NextDouble(), new TryGetDelegate<double>(MemoryExtensions.TryGetNextDouble) };
            yield return new object[] { (Random random, int _) => (float)random.NextDouble(), new TryGetDelegate<float>(MemoryExtensions.TryGetNextSingle) };
            yield return new object[] { (Random random, int _) => (byte)random.Next(byte.MaxValue), new TryGetDelegate<byte>(MemoryExtensions.TryGetNextByte) };
            yield return new object[] { (Random random, int _) => (short)random.Next(short.MaxValue), new TryGetDelegate<short>(MemoryExtensions.TryGetNextInt16) };
            yield return new object[] { (Random random, int _) => (ushort)random.Next(ushort.MaxValue), new TryGetDelegate<ushort>(MemoryExtensions.TryGetNextUInt16) };
            yield return new object[] { (Random random, int _) => random.Next(), new TryGetDelegate<int>(MemoryExtensions.TryGetNextInt32) };
            yield return new object[] { (Random random, int _) => (uint)random.Next(), new TryGetDelegate<uint>(MemoryExtensions.TryGetNextUInt32) };
            yield return new object[] { (Random random, int _) => (long)random.Next() + int.MaxValue, new TryGetDelegate<long>(MemoryExtensions.TryGetNextInt64) };
            yield return new object[] { (Random random, int _) => (ulong)random.Next() + uint.MaxValue, new TryGetDelegate<ulong>(MemoryExtensions.TryGetNextUInt64) };
        }
    }

    public delegate bool TryGetValuesDelegate<T>(ReadOnlySpan<char> input, char separator, int count, IFormatProvider? provider, out T[]? output);

    public delegate bool TryGetDelegate<T>(ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, IFormatProvider? provider, out T value);

    public delegate T[] GetValuesDelegate<T>(ReadOnlySpan<char> input, char separator, int count, IFormatProvider? provider);

    public delegate T GetDelegate<T>(ReadOnlySpan<char> span, ref SpanSplitEnumerator<char> enumerator, System.Globalization.NumberStyles style, IFormatProvider? provider);
#endif

    private enum EnumValue
    {
        First,
        Second,
        Third,
    }
}