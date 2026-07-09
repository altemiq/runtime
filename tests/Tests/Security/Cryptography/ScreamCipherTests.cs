namespace Altemiq.Security.Cryptography;

public class ScreamCipherTests
{
    private const string Original = "Hello world!";
    private const string Encoded = "A̰áăăå ȁåȃăa̲!";

    [Test]
    public async Task Encode()
    {
        _ = await Assert.That(ScreamCipher.Encode(Original)).IsEqualTo(Encoded);
    }

    [Test]
    public async Task Decode()
    {
        _ = await Assert.That(ScreamCipher.Decode(Encoded)).IsEqualTo(Original);
    }
}