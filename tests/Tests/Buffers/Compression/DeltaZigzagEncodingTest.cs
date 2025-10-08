namespace Altemiq.Buffers.Compression;

public class DeltaZigzagEncodingTest
{
    [Test]
    [Arguments(0, 0)]
    [Arguments(1, 2)]
    [Arguments(2, 4)]
    [Arguments(3, 6)]
    [Arguments(-1, 1)]
    [Arguments(-2, 3)]
    [Arguments(-3, 5)]
    public async Task CheckZigzagEncode(int value, int expected)
    {
        var e = new DeltaZigzagEncoding.Encoder(0);
        await Assert.That(e.Encode(value)).IsEqualTo(expected);
    }

    [Test]
    [Arguments(0, 0)]
    [Arguments(1, -1)]
    [Arguments(2, 1)]
    [Arguments(3, -2)]
    [Arguments(4, 2)]
    [Arguments(5, -3)]
    public async Task CheckZigzagDecoder(int value, int expected)
    {
        var d = new DeltaZigzagEncoding.Decoder(0);
        await Assert.That(d.Decode(value)).IsEqualTo(expected);
    }

    [Test]
    public async Task CheckEncodeSimple()
    {
        var e = new DeltaZigzagEncoding.Encoder(0);
        int[] data = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9];
        var output = new int[data.Length];
        e.Encode(data, output);
        await Assert.That(output).IsEquivalentTo([0, 2, 2, 2, 2, 2, 2, 2, 2, 2]);
        await Assert.That(data[^1]).IsEqualTo(e.ContextValue);
    }

    [Test]
    public async Task CheckDecodeSimple()
    {
        var d = new DeltaZigzagEncoding.Decoder(0);
        int[] data = [0, 2, 2, 2, 2, 2, 2, 2, 2, 2];
        var r = new int[data.Length];
        d.Decode(data, r);
        await Assert.That(r).IsEquivalentTo([0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);
        await Assert.That(r[^1]).IsEqualTo(d.ContextValue);
    }

    [Test]
    [Arguments(0)]
    [Arguments(1)]
    [Arguments(1375228800)]
    [Arguments(1 << 30)]
    [Arguments(1 << 31)]
    public async Task CheckSpots(int value)
    {
        var c = new SpotChecker();
        await c.Check(value);
    }

    private class SpotChecker
    {
        private readonly DeltaZigzagEncoding.Encoder encoder = new(0);
        private readonly DeltaZigzagEncoding.Decoder decoder = new(0);

        public async Task Check(int value)
        {
            encoder.ContextValue = 0;
            decoder.ContextValue = 0;
            await Assert.That(decoder.Decode(encoder.Encode(value))).IsEqualTo(value);
        }
    }
}