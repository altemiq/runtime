namespace Altemiq.Protobuf.WellKnownTypes;

public class VersionReflectionTests
{
    [Test]
    public async Task Name()
    {
        _ = await Assert.That(VersionReflection.Descriptor.Name).IsEqualTo("altemiq/protobuf/version.proto");
    }
}