namespace Altemiq.Globalization;

public class TextInfoExtensionTests
{
    [Test]
    [Arguments("customer", "customer")]
    [Arguments("CUSTOMER", "cUSTOMER")]
    [Arguments("CUStomer", "cUStomer")]
    [Arguments("customer_name", "customerName")]
    [Arguments("customer_first_name", "customerFirstName")]
    [Arguments("customer_first_name goes here", "customerFirstNameGoesHere")]
    [Arguments("customer name", "customerName")]
    [Arguments("customer   name", "customerName")]
    [Arguments("", "")]
    public async Task ToCamelCase(string input, string expected) => await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToCamelCase(input)).IsEqualTo(expected);
    
    [Test]
    [Arguments("SomeTitle", "some_title")]
    [Arguments("someTitle", "some_title")]
    [Arguments("some title", "some_title")]
    [Arguments("some title that will be underscored", "some_title_that_will_be_underscored")]
    [Arguments("SomeTitleThatWillBeUnderscored", "some_title_that_will_be_underscored")]
    [Arguments("SomeForeignWordsLikeÄgyptenÑu", "some_foreign_words_like_ägypten_ñu")]
    [Arguments("Some wordsTo be Underscored", "some_words_to_be_underscored")]
    public async Task ToSnakeCase(string input, string expected) => await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToSnakeCase(input)).IsEqualTo(expected);

    [Test]
    [Arguments("SomeWords", "some-words")]
    [Arguments("SOME words TOGETHER", "some-words-together")]
    [Arguments("A spanish word EL niño", "a-spanish-word-el-niño")]
    [Arguments("SomeForeignWords ÆgÑuÄgypten", "some-foreign-words-æg-ñu-ägypten")]
    [Arguments("A VeryShortSENTENCE", "a-very-short-sentence")]
    public async Task ToKebabCase(string input, string expected) => await Assert.That(System.Globalization.CultureInfo.InvariantCulture.TextInfo.ToKebabCase(input)).IsEqualTo(expected);
}