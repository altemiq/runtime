namespace Altemiq.Data;

public class KebabCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => ("SomeWords", "some-words");
        yield return () => ("SOME words TOGETHER", "some-words-together");
        yield return () => ("A spanish word EL niño", "a-spanish-word-el-niño");
        yield return () => ("SomeForeignWords ÆgÑuÄgypten", "some-foreign-words-æg-ñu-ägypten");
        yield return () => ("A VeryShortSENTENCE", "a-very-short-sentence");
    }
}