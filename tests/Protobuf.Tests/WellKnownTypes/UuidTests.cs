namespace Altemiq.Protobuf.WellKnownTypes;

public class UuidTests
{
    [Test]
    public async Task FromGuid()
    {
        Guid guid = Guid.NewGuid();
        await Assert.That(Uuid.ForGuid(guid).ToGuid()).IsEqualTo(guid);
    }

    [Test]
    public async Task Descriptor()
    {
        await Assert.That(Uuid.Descriptor).IsNotNull();
    }

    [Test]
    public async Task Parser()
    {
        await Assert.That(Uuid.Parser).IsNotNull();
    }

    [Test]
    public async Task NewUuid()
    {
        await Assert.That(Uuid.NewUuid()).IsNotNull();
    }

#if NET9_0_OR_GREATER
    [Test]
    public async Task FromGuidVersion7()
    {
        Guid guid = Guid.CreateVersion7();
        await Assert.That(Uuid.ForGuid(guid).ToGuid()).IsEqualTo(guid);
    }

    [Test]
    public async Task CreateVersion7()
    {
        await Assert.That(Uuid.CreateVersion7()).IsNotNull();
    }

    [Test]
    public async Task CreateVersion7WithTimestamp()
    {
        await Assert.That(Uuid.CreateVersion7(DateTimeOffset.UtcNow)).IsNotNull();
    }
#endif
}