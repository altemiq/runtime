namespace Altemiq.Extensions.Caching.Hybrid;

using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.DependencyInjection;

public class HybridCacheTests
{
    [Test]
    public async Task AddSerializerFactory()
    {
        var services = new ServiceCollection();
        _ = services.AddHybridCache().AddProtobufSerializerFactory();
        IHybridCacheSerializer<Tests.SomeProtobufMessage>? serializer = default;
        _ = await Assert.That(services.BuildServiceProvider().GetService<IHybridCacheSerializerFactory>())
            .IsAssignableTo<IHybridCacheSerializerFactory>()
            .And.Satisfies(f => f!.TryCreateSerializer(out serializer), v => v.IsTrue());
        await Assert.That(serializer).IsNotNull();
    }

    [Test]
    public async Task AddSerializer()
    {
        var services = new ServiceCollection();
        _ = services.AddHybridCache().AddProtobufSerializer<Tests.SomeProtobufMessage>();
        await Assert.That(services.BuildServiceProvider().GetService<IHybridCacheSerializer<Tests.SomeProtobufMessage>>()).IsNotNull();
    }
}