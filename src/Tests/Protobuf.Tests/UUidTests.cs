namespace Altemiq.Protobuf;

public class UUidTests
{
    [Fact]
    public void FromGuid()
    {
        var guid = Guid.NewGuid();
        WellKnownTypes.Uuid.ForGuid(guid).ToGuid().Should().Be(guid);
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void FromGuidVersion7()
    {
        var guid = Guid.CreateVersion7();
        WellKnownTypes.Uuid.ForGuid(guid).ToGuid().Should().Be(guid);
    }

    [Fact]
    public void CreateVersion7() => WellKnownTypes.Uuid.CreateVersion7().Should().NotBeNull();
#endif
}