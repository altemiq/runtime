namespace Altemiq.Data;

public class KebabCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return static () => ("SomeWords", "some-words");
        yield return static () => ("SOME words TOGETHER", "some-words-together");
        yield return static () => ("A spanish word EL niño", "a-spanish-word-el-niño");
        yield return static () => ("SomeForeignWords ÆgÑuÄgypten", "some-foreign-words-æg-ñu-ägypten");
        yield return static () => ("A VeryShortSENTENCE", "a-very-short-sentence");
    }
}