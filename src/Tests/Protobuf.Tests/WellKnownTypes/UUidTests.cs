namespace Altemiq.Protobuf.WellKnownTypes;

public class UUidTests
{
    [Fact]
    public void FromGuid()
    {
        Guid guid = Guid.NewGuid();
        Assert.Equal(guid, Uuid.ForGuid(guid).ToGuid());
    }

    [Fact]
    public void Descriptor()
    {
        Assert.NotNull(Uuid.Descriptor);
    }

    [Fact]
    public void Parser()
    {
        Assert.NotNull(Uuid.Parser);
    }

    [Fact]
    public void NewUuid()
    {
        Assert.NotNull(Uuid.NewUuid());
    }

#if NET9_0_OR_GREATER
    [Fact]
    public void FromGuidVersion7()
    {
        Guid guid = Guid.CreateVersion7();
        Assert.Equal(guid, Uuid.ForGuid(guid).ToGuid());
    }

    [Fact]
    public void CreateVersion7()
    {
        Assert.NotNull(Uuid.CreateVersion7());
    }

    [Fact]
    public void CreateVersion7WithTimestamp()
    {
        Assert.NotNull(Uuid.CreateVersion7(DateTimeOffset.UtcNow));
    }
#endif
}