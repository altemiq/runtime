// -----------------------------------------------------------------------
// <copyright file="StringExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

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

#if NET5_0_OR_GREATER
    private static readonly string[] expectation = ["This", "is", "a"];
#endif

    [Fact]
    public void NullString() => Assert.Throws<ArgumentNullException>(static () => default(string)!.SplitQuoted());

    [Fact]
    public void EmptyString() => Assert.Single(string.Empty.SplitQuoted(), string.Empty);

    [Fact]
    public void EmptyStringWithOptions() => Assert.Empty(string.Empty.SplitQuoted(',', StringSplitOptions.RemoveEmptyEntries)!);

    [Theory]
    [InlineData(SingleLineSimple, "3", 5, StringSplitOptions.None)]
    [InlineData(SingleLineWithEmptyValues, "", 5, StringSplitOptions.None)]
    [InlineData(SingleLineWithNullValues, null, 5, StringSplitOptions.None)]
    [InlineData(SingleLineWithNullValues, "4", 4, StringSplitOptions.RemoveEmptyEntries)]
    public void ReadSimple(string input, string? value, int length, StringSplitOptions options)
    {
        var result = input.SplitQuoted(',', options);
        Assert.NotNull(result);
        Assert.Equal(length, result.Length);
        Assert.Equal(value, result.Skip(2).Take(1).Single());
    }

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
    public void ReadLength(string input, int length)
    {
        var result = input.SplitQuoted(',');
        Assert.NotNull(result);
        Assert.Equal(length, result.Length);
    }

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
    public void Quote(string input, string? delimiter, StringQuoteOptions options, bool quoted)
    {
        var d = delimiter?.ToCharArray() ?? [];
        Assert.Equal(quoted ? "\"" + input + "\"" : input, input.Quote(d, options));
    }

    [Theory]
    [InlineData("value,second", ',', StringQuoteOptions.QuoteAll, true)]
    [InlineData("value,second", ',', StringQuoteOptions.None, true)]
    public void QuoteChar(string input, char delimiter, StringQuoteOptions options, bool quoted) => Assert.Equal(quoted ? "\"" + input + "\"" : input, input.Quote(delimiter, options));

    [Fact]
    public void QuoteNull() => Assert.Equal(string.Empty, default(string).Quote());

    [Theory]
    [MemberData(nameof(GetCharArrays))]
    public void SplitQuotedWithEmptyDelimeter(char[]? chars)
    {
        const string Value = "This is a string";
        Assert.Equal(4, Value.SplitQuoted(chars, options: StringSplitOptions.None).Length);
    }

    public static TheoryData<char[]?> GetCharArrays() => new()
    {
        { default(char[]) },
        { [] },
    };

    [Fact]
    public void Format()
    {
        const int Formattable = 123;
        Assert.Equal(Formattable.ToString(default(IFormatProvider)), Format(Formattable));

        static string? Format<T>(T value)
            where T : IFormattable
        {
            return value.ToString(default);
        }
    }

#if NET5_0_OR_GREATER
    [Fact]
    public void TrimEntries() => Assert.Equal(expectation, "This , is, a".SplitQuoted(',', StringSplitOptions.TrimEntries));
#endif
}