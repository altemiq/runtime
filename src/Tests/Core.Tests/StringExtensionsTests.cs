// -----------------------------------------------------------------------
// <copyright file="StringExtensions.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using System.Runtime.Serialization.Json;

public class StringExtensionsTests
{
    public const string SingleLineSimple = "1,2,3,4,5";

    public const string SingleLineWithEmptyValues = "1,2,\"\",4,5";

    public const string SingleLineWithNullValues = "1,2,,4,5";

    public const string SingleLineUnquoted = "1,2,This is an unquoted string,4,5";

    public const string SingleLineQuoted = "1,2,\"This is a quoted string\",4,5";

    public const string SingleLineQuotedComma = "1,2,\"This is a quoted string, with a comma\",4,5";

    public const string SingleLineQuotedEmbeddedQuotes = "1,2,\"This is a quoted string, with \"\"embedded quotes\"\"\",4,5";

    public const string SingleLineQuotedCommaEmbeddedQuotes = "1,2,\"This is a quoted string, with a comma and \"\"embedded quotes\"\"\",4,5";

    public const string SingleLineNewLine = """
        1,2,"This is a quoted string
        With a newline in it",4,5
        """;


    public const string SingleLineNewLineEmbeddedQuotes = """"
        1,2,"This is a quoted string
        With a newline in it and ""embedded quotes""",4,5
        """";

    public const string SingleLineNewLineComma = """
        1,2,"This is a quoted string
        ,
        With a newline in it
        ,",4,5
        """;

    public const string SingleLineNewLineCommaEmbeddedQuotes = """
        1,2,"This is a quoted string
        ,
        With a newline in it and ""embedded quotes""
        ,",4,5
        """;

    [Fact]
    public void NullString() => new Func<string[]>(() => default(string)!.SplitQuoted()).Should().ThrowExactly<ArgumentNullException>();

    [Fact]
    public void EmptyString() => string.Empty.SplitQuoted().Should().HaveCount(1).And.HaveElementAt(0, string.Empty);

    [Fact]
    public void EmptyStringWithOptions() => string.Empty.SplitQuoted(',', StringSplitOptions.RemoveEmptyEntries).Should().BeEmpty();

    [Theory]
    [InlineData(SingleLineSimple, "3", 5, StringSplitOptions.None)]
    [InlineData(SingleLineWithEmptyValues, "", 5, StringSplitOptions.None)]
    [InlineData(SingleLineWithNullValues, null, 5, StringSplitOptions.None)]
    [InlineData(SingleLineWithNullValues, "4", 4, StringSplitOptions.RemoveEmptyEntries)]
    public void ReadSimple(string input, string value, int length, StringSplitOptions options) =>
        input.SplitQuoted(',', options)
            .Should().NotBeNull()
            .And.Subject.Should().HaveCount(length)
            .And.HaveElementAt(2, value);

    [Theory]
    [InlineData(SingleLineUnquoted, 5)]
    [InlineData(SingleLineQuoted, 5)]
    [InlineData(SingleLineQuotedComma, 5)]
    [InlineData(SingleLineQuotedEmbeddedQuotes, 5)]
    [InlineData(SingleLineQuotedCommaEmbeddedQuotes, 5)]
    [InlineData(SingleLineNewLine, 5)]
    [InlineData(SingleLineNewLineComma, 5)]
    [InlineData(SingleLineNewLineEmbeddedQuotes, 5)]
    [InlineData(SingleLineNewLineCommaEmbeddedQuotes, 5)]
    public void ReadLength(string input, int length) =>
        input.SplitQuoted(',')
            .Should().NotBeNull()
            .And.Subject.Should().HaveCount(length);

    [Theory]
    [InlineData("", null, StringQuoteOptions.None, true)]
    [InlineData("value", null, StringQuoteOptions.None, true)]
    [InlineData("", null, StringQuoteOptions.QuoteAll, true)]
    [InlineData("value", null, StringQuoteOptions.QuoteAll, false)]
    [InlineData("value,second", ",", StringQuoteOptions.QuoteAll, true)]
    [InlineData("value,second", ",", StringQuoteOptions.None, true)]
    [InlineData("value\rsecond", null, StringQuoteOptions.QuoteAll, true)]
    [InlineData("value\rsecond", null, StringQuoteOptions.QuoteAll & ~StringQuoteOptions.QuoteNewLine, false)]
    [InlineData("value€second", null, StringQuoteOptions.QuoteNonAscii, true)]
    [InlineData("value€second", null, StringQuoteOptions.QuoteAll & ~StringQuoteOptions.QuoteNonAscii, false)]
    public void Quote(string input, string? delimeter, StringQuoteOptions options, bool quoted)
    {
        var d = delimeter?.ToCharArray() ?? Array.Empty<char>();
        input.Quote(d, options).Should().Be(quoted ? "\"" + input + "\"" : input);
    }

    [Fact]
    public void QuoteNull() => default(string).Quote().Should().Be(string.Empty);

#if NET5_0_OR_GREATER
    [Fact]
    public void TrimEntries() => "This , is, a".Split(',', StringSplitOptions.TrimEntries).Should().BeEquivalentTo(new string[] { "This", "is", "a" });
#endif
}