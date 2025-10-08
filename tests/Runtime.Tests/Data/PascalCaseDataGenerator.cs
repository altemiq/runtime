namespace Altemiq.Data;

public class PascalCaseDataGenerator : DataSourceGeneratorAttribute<string, string>
{
    protected override IEnumerable<Func<(string, string)>> GenerateDataSources(DataGeneratorMetadata dataGeneratorMetadata)
    {
        yield return () => ("customer", "Customer");
        yield return () => ("CUSTOMER", "CUSTOMER");
        yield return () => ("CUStomer", "CUStomer");
        yield return () => ("customer_name", "CustomerName");
        yield return () => ("customer_first_name", "CustomerFirstName");
        yield return () => ("customer_first_name goes here", "CustomerFirstNameGoesHere");
        yield return () => ("customer name", "CustomerName");
        yield return () => ("customer   name", "CustomerName");
        yield return () => ("customer-first-name", "CustomerFirstName");
        yield return () => ("_customer-first-name", "CustomerFirstName");
        yield return () => (" customer__first--name", "CustomerFirstName");
    }
}