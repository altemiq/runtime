namespace Altemiq.Grpc.Tools;

public class ProtoTests
{
    [Test]
    public async Task TestUuid()
    {
        var guid = Guid.NewGuid();
        var testMessage = new Protobuf.WellKnownTypes.Tests.Test { Uuid = Protobuf.WellKnownTypes.Uuid.ForGuid(guid) };
        await Assert.That(testMessage.Uuid.ToGuid()).IsEqualTo(guid);
    }
}