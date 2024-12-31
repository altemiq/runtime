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
        _ = services.BuildServiceProvider().GetService<IHybridCacheSerializerFactory>()
            .Should().BeAssignableTo<IHybridCacheSerializerFactory>()
            .Which.TryCreateSerializer<Tests.SomeProtobufMessage>(out var serializer).Should().BeTrue();
        serializer.Should().NotBeNull();
    }

    [Fact]
    public void AddSerializer()
    {
        var services = new ServiceCollection();
        _ = services.AddHybridCache().AddProtobufSerializer<Tests.SomeProtobufMessage>();
        services.BuildServiceProvider().GetService<IHybridCacheSerializer<Tests.SomeProtobufMessage>>().Should().NotBeNull();
    }
}