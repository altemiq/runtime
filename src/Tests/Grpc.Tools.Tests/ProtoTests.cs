namespace Altemiq.Grpc.Tools;

public class ProtoTests
{
    [Fact]
    public void TestUuid()
    {
        var guid = Guid.NewGuid();
        var testMessage = new Protobuf.WellKnownTypes.Tests.Test { Uuid = Protobuf.WellKnownTypes.Uuid.ForGuid(guid) };
        testMessage.Uuid.ToGuid().Should().Be(guid);
    }
}