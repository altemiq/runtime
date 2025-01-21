namespace Altemiq.Buffers.Compression;

public class DeltaZigzagEncodingTest
{
    [Fact]
    public void CheckZigzagEncode()
    {
        var e = new DeltaZigzagEncoding.Encoder(0);
        Assert.Equal(0, ZigzagEncode(e, 0));
        Assert.Equal(2, ZigzagEncode(e, 1));
        Assert.Equal(4, ZigzagEncode(e, 2));
        Assert.Equal(6, ZigzagEncode(e, 3));
        Assert.Equal(1, ZigzagEncode(e, -1));
        Assert.Equal(3, ZigzagEncode(e, -2));
        Assert.Equal(5, ZigzagEncode(e, -3));

        static int ZigzagEncode(DeltaZigzagEncoding.Encoder e, int value)
        {
            e.ContextValue = 0;
            return e.Encode(value);
        }
    }

    [Fact]
    public void CheckZigzagDecoder()
    {
        var d = new DeltaZigzagEncoding.Decoder(0);
        Assert.Equal(0, ZigzagDecode(d, 0));
        Assert.Equal(-1, ZigzagDecode(d, 1));
        Assert.Equal(1, ZigzagDecode(d, 2));
        Assert.Equal(-2, ZigzagDecode(d, 3));
        Assert.Equal(2, ZigzagDecode(d, 4));
        Assert.Equal(-3, ZigzagDecode(d, 5));

        static int ZigzagDecode(DeltaZigzagEncoding.Decoder d, int value)
        {
            d.ContextValue = 0;
            return d.Decode(value);
        }
    }

    [Fact]
    public void CheckEncodeSimple()
    {
        CheckEncode(new DeltaZigzagEncoding.Encoder(0), [0, 1, 2, 3, 4, 5, 6, 7, 8, 9], [0, 2, 2, 2, 2, 2, 2, 2, 2, 2]);

        static void CheckEncode(DeltaZigzagEncoding.Encoder e, int[] data, int[] expected)
        {
            var output = new int[data.Length];
            e.Encode(data, output);
            Assert.Equal(expected, output);
            Assert.Equal(e.ContextValue, data[^1]);
        }
    }

    [Fact]
    public void CheckDecodeSimple()
    {
        CheckDecode(new DeltaZigzagEncoding.Decoder(0), [0, 2, 2, 2, 2, 2, 2, 2, 2, 2], [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]);

        static void CheckDecode(DeltaZigzagEncoding.Decoder d, int[] data, int[] expected)
        {
            var r = new int[data.Length];
            d.Decode(data, r);
            Assert.Equal(expected, r);
            Assert.Equal(d.ContextValue, r[^1]);
        }
    }

    [Fact]
    public void CheckSpots()
    {
        var c = new SpotChecker();
        c.Check(0);
        c.Check(1);
        c.Check(1375228800);
        c.Check(1 << 30);
        c.Check(1 << 31);
    }

    private class SpotChecker
    {
        private readonly DeltaZigzagEncoding.Encoder encoder = new(0);
        private readonly DeltaZigzagEncoding.Decoder decoder = new(0);

        public void Check(int value)
        {
            encoder.ContextValue = 0;
            decoder.ContextValue = 0;
            Assert.Equal(value, decoder.Decode(encoder.Encode(value)));
        }
    }
}