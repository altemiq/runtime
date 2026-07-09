namespace Altemiq.Grpc.Tools;

public class ProtoTests
{
    [Test]
    [MethodDataSource(nameof(GuidData))]
    public async Task Uuid(Guid input)
    {
        var testMessage = new Protobuf.WellKnownTypes.Tests.Test { Uuid = Protobuf.WellKnownTypes.Uuid.ForGuid(input) };
        _ = await Assert.That(testMessage.Uuid.ToGuid()).IsEqualTo(input);
    }

    public static IEnumerable<Func<Guid>> GuidData()
    {
        yield return Guid.NewGuid;
#if NET9_0_OR_GREATER
        yield return Guid.CreateVersion7;
#endif
    }
}