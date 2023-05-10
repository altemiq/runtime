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
        span.GetNextString(ref enumerator).Should().Be("This");
        span.GetNextString(ref enumerator).Should().Be("is");
        span.GetNextString(ref enumerator).Should().Be("a");
        span.GetNextString(ref enumerator).Should().Be("split");
        span.GetNextString(ref enumerator).Should().Be("string");
        enumerator.MoveNext().Should().BeFalse();
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
        span.GetNextString(ref enumerator).Should().Be(values[0]);
        span.GetNextString(ref enumerator).Should().Be(values[1]);
        span.GetNextString(ref enumerator).Should().Be(values[2]);
        span.GetNextString(ref enumerator).Should().Be(values[3]);
        span.GetNextString(ref enumerator).Should().Be(values[4]);
        enumerator.MoveNext().Should().BeFalse();
    }

    [Fact]
    public void ReadDoubleValues()
    {
        var random = new Random();
        var randomValues = Enumerable.Range(0, 10).Select(_ => random.NextDouble()).ToArray();
        var value = string.Join("|", randomValues);
        var span = value.AsSpan();
        var enumerator = span.Split("|");
        var count = 0;
        while (span.TryGetNextDouble(ref enumerator, System.Globalization.CultureInfo.CurrentCulture, out var doubleValue))
        {
            doubleValue.Should().Be(randomValues[count]);
            count++;
        }

        count.Should().Be(10);
        enumerator.MoveNext().Should().BeFalse();
    }
}
