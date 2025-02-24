namespace Altemiq.Buffers.Compression;

public class DeltaZigzagEncodingTest
{
    [Test]
    public async Task CheckZigzagEncode()
    {
        var e = new DeltaZigzagEncoding.Encoder(0);
        await Assert.That(ZigzagEncode(e, 0)).IsEqualTo(0);
        await Assert.That(ZigzagEncode(e, 1)).IsEqualTo(2);
        await Assert.That(ZigzagEncode(e, 2)).IsEqualTo(4);
        await Assert.That(ZigzagEncode(e, 3)).IsEqualTo(6);
        await Assert.That(ZigzagEncode(e, -1)).IsEqualTo(1);
        await Assert.That(ZigzagEncode(e, -2)).IsEqualTo(3);
        await Assert.That(ZigzagEncode(e, -3)).IsEqualTo(5);

        static int ZigzagEncode(DeltaZigzagEncoding.Encoder e, int value)
        {
            e.ContextValue = 0;
            return e.Encode(value);
        }
    }

    [Test]
    public async Task CheckZigzagDecoder()
    {
        var d = new DeltaZigzagEncoding.Decoder(0);
        await Assert.That(ZigzagDecode(d, 0)).IsEqualTo(0);
        await Assert.That(ZigzagDecode(d, 1)).IsEqualTo(-1);
        await Assert.That(ZigzagDecode(d, 2)).IsEqualTo(1);
        await Assert.That(ZigzagDecode(d, 3)).IsEqualTo(-2);
        await Assert.That(ZigzagDecode(d, 4)).IsEqualTo(2);
        await Assert.That(ZigzagDecode(d, 5)).IsEqualTo(-3);

        static int ZigzagDecode(DeltaZigzagEncoding.Decoder d, int value)
        {
            d.ContextValue = 0;
            return d.Decode(value);
        }
    }

    [Test]
    public Task CheckEncodeSimple()
    {
        return CheckEncode(new DeltaZigzagEncoding.Encoder(0), [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], [0, 2, 2, 2, 2, 2, 2, 2, 2, 2]);

        static async Task CheckEncode(DeltaZigzagEncoding.Encoder e, int[] data, int[] expected)
        {
            var output = new int[data.Length];
            e.Encode(data, output);
            await Assert.That(output).IsEquivalentTo(expected);
            await Assert.That(data[^1]).IsEqualTo(e.ContextValue);
        }
    }

    [Test]
    public Task CheckDecodeSimple()
    {
        return CheckDecode(new DeltaZigzagEncoding.Decoder(0), [0, 2, 2, 2, 2, 2, 2, 2, 2, 2], [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

        static async Task CheckDecode(DeltaZigzagEncoding.Decoder d, int[] data, int[] expected)
        {
            var r = new int[data.Length];
            d.Decode(data, r);
            await Assert.That(r).IsEquivalentTo(expected);
            await Assert.That(r[^1]).IsEqualTo(d.ContextValue);
        }
    }

    [Test]
    public async Task CheckSpots()
    {
        var c = new SpotChecker();
        await c.Check(0);
        await c.Check(1);
        await c.Check(1375228800);
        await c.Check(1 << 30);
        await c.Check(1 << 31);
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