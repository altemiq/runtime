namespace Altemiq.Extensions.Configuration.Yaml;

public static class ConfigurationProviderExtensions
{
    public static string? Get(this Microsoft.Extensions.Configuration.IConfigurationProvider provider, string key)
    {
        return provider.TryGet(key, out var value) ? value : throw new InvalidOperationException("Key not found");
    }
}