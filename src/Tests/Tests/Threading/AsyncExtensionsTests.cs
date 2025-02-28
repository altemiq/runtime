namespace Altemiq.Threading;

using TUnit.Assertions.AssertConditions.Throws;

public class AsyncExtensionsTests
{
    [Test]
    public async Task ShouldAwaitWithException()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(100);
        await Assert.That(async () => await cancellationTokenSource.Token).Throws<OperationCanceledException>();
    }

    [Test]
    public async Task ShouldAwaitWithoutExceptionAsBool()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(100);
        await Assert.That(async () => await cancellationTokenSource.Token.ConfigureAwait(false)).ThrowsNothing();
    }

    [Test]
    public async Task ShouldAwaitWithoutExceptionAsOptions()
    {
        var cancellationTokenSource = new CancellationTokenSource();
        cancellationTokenSource.CancelAfter(100);
        await Assert.That(async () => await cancellationTokenSource.Token.ConfigureAwait(CancellationTokenConfigureAwaitOptions.SuppressThrowing)).ThrowsNothing();
    }
}
