namespace Altemiq.Grpc.Tools;

using Microsoft.Extensions.DependencyModel;

public class RuntimeTests
{
    [Test]
    public async Task RuntimeShouldContainTools()
    {
        await Assert.That(DependencyContext.Default)
            .IsNotNull()
            .And.Member(
                dependencyContext => dependencyContext.RuntimeLibraries,
                runtimeLibraries => runtimeLibraries.Contains(rl => rl.Name == "Altemiq.Grpc.Tools"));
    }
}