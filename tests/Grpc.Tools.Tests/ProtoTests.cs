namespace Altemiq.Grpc.Tools;

public class ProtoTests
{
    [Test]
    [MethodDataSource(nameof(GuidData))]
    public async Task Uuid(Guid guid)
    {
        var testMessage = new Protobuf.WellKnownTypes.Tests.Test { Uuid = Protobuf.WellKnownTypes.Uuid.ForGuid(guid) };
        await Assert.That(testMessage.Uuid.ToGuid()).IsEqualTo(guid);
    }

    public static IEnumerable<Func<Guid>> GuidData()
    {
        yield return Guid.NewGuid;
#if NET9_0_OR_GREATER
        yield return Guid.CreateVersion7;
#endif
    }
}