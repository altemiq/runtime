namespace Altemiq.Protobuf.WellKnownTypes;

public class SemverReflectionTests
{
    [Test]
    public async Task Name()
    {
        _ = await Assert.That(SemverReflection.Descriptor.Name).IsEqualTo("altemiq/protobuf/semver.proto");
    }
}