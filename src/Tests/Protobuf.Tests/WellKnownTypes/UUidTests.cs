namespace Altemiq.Protobuf.WellKnownTypes;

public class UUidTests
{
    [Fact]
    public void FromGuid()
    {
        var guid = Guid.NewGuid();
        Uuid.ForGuid(guid).ToGuid().Should().Be(guid);
    }

    [Fact]
    public void Descriptor() => Uuid.Descriptor.Should().NotBeNull();

    [Fact]
    public void Parser() => Uuid.Parser.Should().NotBeNull();

    [Fact]
    public void NewUuid() => Uuid.NewUuid().Should().NotBeNull();

#if NET9_0_OR_GREATER
    [Fact]
    public void FromGuidVersion7()
    {
        var guid = Guid.CreateVersion7();
        Uuid.ForGuid(guid).ToGuid().Should().Be(guid);
    }

    [Fact]
    public void CreateVersion7() => Uuid.CreateVersion7().Should().NotBeNull();

    [Fact]
    public void CreateVersion7WithTimestamp() => Uuid.CreateVersion7(DateTimeOffset.UtcNow).Should().NotBeNull();
#endif
}