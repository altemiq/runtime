namespace Altemiq.Protobuf;

public class UUidTests
{
    [Fact]
    public void FromGuid()
    {
        var guid = Guid.NewGuid();
        var uuid = WellKnownTypes.Uuid.ForGuid(guid);
        uuid.ToGuid().Should().Be(guid);
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void FromGuidVersion7()
    {
        var guid = Guid.CreateVersion7();
        var uuid = WellKnownTypes.Uuid.ForGuid(guid);
        uuid.ToGuid().Should().Be(guid);
    }
#endif
}