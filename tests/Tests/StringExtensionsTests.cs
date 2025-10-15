// -----------------------------------------------------------------------
// <copyright file="StringExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class StringExtensionsTests
{
    private const string SingleLineSimple = "1,2,3,4,5";

    private const string SingleLineWithEmptyValues = "1,2,\"\",4,5";

    private const string SingleLineWithNullValues = "1,2,,4,5";

    private const string SingleLineUnquoted = "1,2,This is an unquoted string,4,5";

    private const string SingleLineQuoted = "1,2,\"This is a quoted string\",4,5";

    private const string SingleLineQuotedComma = "1,2,\"This is a quoted string, with a comma\",4,5";

    private const string SingleLineQuotedEmbeddedQuotes = "1,2,\"This is a quoted string, with \"\"embedded quotes\"\"\",4,5";

    private const string SingleLineQuotedCommaEmbeddedQuotes = "1,2,\"This is a quoted string, with a comma and \"\"embedded quotes\"\"\",4,5";

    private const string SingleLineNewLine = """
        1,2,"This is a quoted string
        With a newline in it",4,5
        """;


    private const string SingleLineNewLineEmbeddedQuotes = """"
        1,2,"This is a quoted string
        With a newline in it and ""embedded quotes""",4,5
        """";

    private const string SingleLineNewLineComma = """
        1,2,"This is a quoted string
        ,
        With a newline in it
        ,",4,5
        """;

    private const string SingleLineNewLineCommaEmbeddedQuotes = """
        1,2,"This is a quoted string
        ,
        With a newline in it and ""embedded quotes""
        ,",4,5
        """;

#if NET5_0_OR_GREATER
    private static readonly string[] Expectation = ["This", "is", "a"];
#endif

    [Test]
    public async Task NullString() => await Assert.That(static () => default(string)!.SplitQuoted()).Throws<ArgumentNullException>();

    [Test]
    public async Task EmptyString() => await Assert.That(string.Empty.SplitQuoted()).HasSingleItem();

    [Test]
    public async Task EmptyStringWithOptions() => await Assert.That(string.Empty.SplitQuoted(',', StringSplitOptions.RemoveEmptyEntries)).IsEmpty();

    [Test]
    [Arguments(SingleLineSimple, "3", 5, StringSplitOptions.None)]
    [Arguments(SingleLineWithEmptyValues, "", 5, StringSplitOptions.None)]
    [Arguments(SingleLineWithNullValues, null, 5, StringSplitOptions.None)]
    [Arguments(SingleLineWithNullValues, "4", 4, StringSplitOptions.RemoveEmptyEntries)]
    public async Task ReadSimple(string input, string? value, int length, StringSplitOptions options)
    {
        var result = input.SplitQuoted(',', options);
        await Assert.That(result).IsNotNull();
        await Assert.That(result).HasCount().EqualTo(length);
        await Assert.That(result.Skip(2).Take(1).Single()).IsEqualTo(value);
    }

    [Test]
    [Arguments(SingleLineUnquoted, 5)]
    [Arguments(SingleLineQuoted, 5)]
    [Arguments(SingleLineQuotedComma, 5)]
    [Arguments(SingleLineQuotedEmbeddedQuotes, 5)]
    [Arguments(SingleLineQuotedCommaEmbeddedQuotes, 5)]
    [Arguments(SingleLineNewLine, 5)]
    [Arguments(SingleLineNewLineComma, 5)]
    [Arguments(SingleLineNewLineEmbeddedQuotes, 5)]
    [Arguments(SingleLineNewLineCommaEmbeddedQuotes, 5)]
    public async Task ReadLength(string input, int length)
    {
        var result = input.SplitQuoted(',');
        await Assert.That(result).IsNotNull();
        await Assert.That(result).HasCount().EqualTo(length);
    }

    [Test]
    [Arguments("", null, StringQuoteOptions.None, true)]
    [Arguments("value", null, StringQuoteOptions.None, true)]
    [Arguments("", null, StringQuoteOptions.QuoteAll, true)]
    [Arguments("value", null, StringQuoteOptions.QuoteAll, false)]
    [Arguments("value,second", ",", StringQuoteOptions.QuoteAll, true)]
    [Arguments("value,second", ",", StringQuoteOptions.None, true)]
    [Arguments("value\rsecond", null, StringQuoteOptions.QuoteAll, true)]
    [Arguments("value\rsecond", null, StringQuoteOptions.QuoteAll & ~StringQuoteOptions.QuoteNewLine, false)]
    [Arguments("value€second", null, StringQuoteOptions.QuoteNonAscii, true)]
    [Arguments("value€second", null, StringQuoteOptions.QuoteAll & ~StringQuoteOptions.QuoteNonAscii, false)]
    public async Task Quote(string input, string? delimiter, StringQuoteOptions options, bool quoted)
    {
        var d = delimiter?.ToCharArray() ?? [];
        await Assert.That(input.Quote(d, options)).IsEqualTo(quoted ? "\"" + input + "\"" : input);
    }

    [Test]
    [Arguments("value,second", ',', StringQuoteOptions.QuoteAll, true)]
    [Arguments("value,second", ',', StringQuoteOptions.None, true)]
    public async Task QuoteChar(string input, char delimiter, StringQuoteOptions options, bool quoted) => await Assert.That(input.Quote(delimiter, options)).IsEqualTo(quoted ? "\"" + input + "\"" : input);

    [Test]
    public async Task QuoteNull() => await Assert.That(default(string).Quote()).IsEqualTo(string.Empty);

    [Test]
    [MethodDataSource(nameof(GetCharArrays))]
    public async Task SplitQuotedWithEmptyDelimiter(char[]? chars)
    {
        const string Value = "This is a string";
        await Assert.That(Value.SplitQuoted(chars, options: StringSplitOptions.None)).HasCount().EqualTo(4);
    }

    public static IEnumerable<Func<char[]?>> GetCharArrays()
    {
        yield return () => default;
        yield return () => [];
    }

    [Test]
    public async Task Format()
    {
        const int Formattable = 123;
        await Assert.That(FormatValue(Formattable)).IsEqualTo(Formattable.ToString(default(IFormatProvider)));

        static string FormatValue<T>(T value)
            where T : IFormattable
        {
            return value.ToString(default);
        }
    }

#if NET5_0_OR_GREATER
    [Test]
    public async Task TrimEntries() => await Assert.That("This , is, a".SplitQuoted(',', StringSplitOptions.TrimEntries)).IsEquivalentTo(Expectation);
#endif
}