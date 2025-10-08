namespace Altemiq.Data;

public class CamelCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => ("customer", "customer");
        yield return () => ("CUSTOMER", "cUSTOMER");
        yield return () => ("CUStomer", "cUStomer");
        yield return () => ("customer_name", "customerName");
        yield return () => ("customer_first_name", "customerFirstName");
        yield return () => ("customer_first_name goes here", "customerFirstNameGoesHere");
        yield return () => ("customer name", "customerName");
        yield return () => ("customer   name", "customerName");
        yield return () => ("", "");
    }
}