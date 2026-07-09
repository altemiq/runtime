namespace Altemiq.Data;

public class SnakeCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return static () => ("SomeTitle", "some_title");
        yield return static () => ("someTitle", "some_title");
        yield return static () => ("some title", "some_title");
        yield return static () => ("some title that will be underscored", "some_title_that_will_be_underscored");
        yield return static () => ("SomeTitleThatWillBeUnderscored", "some_title_that_will_be_underscored");
        yield return static () => ("SomeForeignWordsLikeÄgyptenÑu", "some_foreign_words_like_ägypten_ñu");
        yield return static () => ("Some wordsTo be Underscored", "some_words_to_be_underscored");
    }
}