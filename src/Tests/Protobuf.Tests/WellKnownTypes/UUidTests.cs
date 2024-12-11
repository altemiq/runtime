namespace Altemiq.Protobuf.WellKnownTypes;

public class UUidTests
{
    [Fact]
    public void FromGuid()
    {
        Guid guid = Guid.NewGuid();
        _ = Uuid.ForGuid(guid).ToGuid().Should().Be(guid);
    }

    [Fact]
    public void Descriptor()
    {
        _ = Uuid.Descriptor.Should().NotBeNull();
    }

    [Fact]
    public void Parser()
    {
        _ = Uuid.Parser.Should().NotBeNull();
    }

    [Fact]
    public void NewUuid()
    {
        _ = Uuid.NewUuid().Should().NotBeNull();
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void FromGuidVersion7()
    {
        Guid guid = Guid.CreateVersion7();
        _ = Uuid.ForGuid(guid).ToGuid().Should().Be(guid);
    }

    [Fact]
    public void CreateVersion7()
    {
        _ = Uuid.CreateVersion7().Should().NotBeNull();
    }

    [Fact]
    public void CreateVersion7WithTimestamp()
    {
        _ = Uuid.CreateVersion7(DateTimeOffset.UtcNow).Should().NotBeNull();
    }
#endif
}