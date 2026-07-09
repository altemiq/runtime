namespace Altemiq.Protobuf.WellKnownTypes;

public class UuidTests
{
    [Test]
    public async Task FromGuid()
    {
        var guid = Guid.NewGuid();
        _ = await Assert.That(Uuid.ForGuid(guid).ToGuid()).IsEqualTo(guid);
    }

    [Test]
    public async Task Descriptor()
    {
        _ = await Assert.That(Uuid.Descriptor).IsNotNull();
    }

    [Test]
    public async Task Parser()
    {
        _ = await Assert.That(Uuid.Parser).IsNotNull();
    }

    [Test]
    public async Task NewUuid()
    {
        _ = await Assert.That(Uuid.NewUuid()).IsNotNull();
    }

#if NET9_0_OR_GREATER
    [Test]
    public async Task FromGuidVersion7()
    {
        var guid = Guid.CreateVersion7();
        _ = await Assert.That(Uuid.ForGuid(guid).ToGuid()).IsEqualTo(guid);
    }

    [Test]
    public async Task CreateVersion7()
    {
        _ = await Assert.That(Uuid.CreateVersion7()).IsNotNull();
    }

    [Test]
    public async Task CreateVersion7WithTimestamp()
    {
        _ = await Assert.That(Uuid.CreateVersion7(DateTimeOffset.UtcNow)).IsNotNull();
    }
#endif
}