namespace Altemiq.Data;

public class CamelCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return static () => ("customer", "customer");
        yield return static () => ("CUSTOMER", "cUSTOMER");
        yield return static () => ("CUStomer", "cUStomer");
        yield return static () => ("customer_name", "customerName");
        yield return static () => ("customer_first_name", "customerFirstName");
        yield return static () => ("customer_first_name goes here", "customerFirstNameGoesHere");
        yield return static () => ("customer name", "customerName");
        yield return static () => ("customer   name", "customerName");
        yield return static () => ("", "");
    }
}