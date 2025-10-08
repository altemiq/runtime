namespace Altemiq.Data;

public class SnakeCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => ("SomeTitle", "some_title");
        yield return () => ("someTitle", "some_title");
        yield return () => ("some title", "some_title");
        yield return () => ("some title that will be underscored", "some_title_that_will_be_underscored");
        yield return () => ("SomeTitleThatWillBeUnderscored", "some_title_that_will_be_underscored");
        yield return () => ("SomeForeignWordsLikeÄgyptenÑu", "some_foreign_words_like_ägypten_ñu");
        yield return () => ("Some wordsTo be Underscored", "some_words_to_be_underscored");
    }
}