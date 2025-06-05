namespace Altemiq.Protobuf.WellKnownTypes;

public class UuidReflectionTests
{

    [Test]
    public async Task Name()
    {
        await Assert.That(UuidReflection.Descriptor.Name).IsEqualTo("altemiq/protobuf/uuid.proto");
    }
}