namespace Altemiq.Extensions.Caching.Hybrid;

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

public class HybridCacheTests
{
    [Fact]
    public void AddSerializerFactory()
    {
        var services = new ServiceCollection();
        _ = services.AddHybridCache().AddProtobufSerializerFactory();
        var factory = Assert.IsType<IHybridCacheSerializerFactory>(services.BuildServiceProvider().GetService<IHybridCacheSerializerFactory>(), false);
        Assert.True(factory.TryCreateSerializer<Tests.SomeProtobufMessage>(out var serializer));
        Assert.NotNull(serializer);
    }

    [Fact]
    public void AddSerializer()
    {
        var services = new ServiceCollection();
        _ = services.AddHybridCache().AddProtobufSerializer<Tests.SomeProtobufMessage>();
        Assert.NotNull(services.BuildServiceProvider().GetService<IHybridCacheSerializer<Tests.SomeProtobufMessage>>());
    }
}