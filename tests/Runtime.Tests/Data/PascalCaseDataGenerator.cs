namespace Altemiq.Data;

public class PascalCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return static () => ("customer", "Customer");
        yield return static () => ("CUSTOMER", "CUSTOMER");
        yield return static () => ("CUStomer", "CUStomer");
        yield return static () => ("customer_name", "CustomerName");
        yield return static () => ("customer_first_name", "CustomerFirstName");
        yield return static () => ("customer_first_name goes here", "CustomerFirstNameGoesHere");
        yield return static () => ("customer name", "CustomerName");
        yield return static () => ("customer   name", "CustomerName");
        yield return static () => ("customer-first-name", "CustomerFirstName");
        yield return static () => ("_customer-first-name", "CustomerFirstName");
        yield return static () => (" customer__first--name", "CustomerFirstName");
    }
}