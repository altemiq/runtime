namespace Altemiq.Protobuf.WellKnownTypes;

public class VersionReflectionTests
{
    [Test]
    public async Task Name()
    {
        await Assert.That(VersionReflection.Descriptor.Name).IsEqualTo("altemiq/protobuf/version.proto");
    }
}
