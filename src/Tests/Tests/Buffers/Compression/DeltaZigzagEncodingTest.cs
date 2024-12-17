namespace Altemiq.Buffers.Compression;

public class DeltaZigzagEncodingTest
{
    [Fact]
    public void CheckZigzagEncode()
    {
        var e = new DeltaZigzagEncoding.Encoder(0);
        _ = ZigzagEncode(e, 0).Should().Be(0);
        _ = ZigzagEncode(e, 1).Should().Be(2);
        _ = ZigzagEncode(e, 2).Should().Be(4);
        _ = ZigzagEncode(e, 3).Should().Be(6);
        _ = ZigzagEncode(e, -1).Should().Be(1);
        _ = ZigzagEncode(e, -2).Should().Be(3);
        _ = ZigzagEncode(e, -3).Should().Be(5);

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
        _ = ZigzagDecode(d, 0).Should().Be(0);
        _ = ZigzagDecode(d, 1).Should().Be(-1);
        _ = ZigzagDecode(d, 2).Should().Be(1);
        _ = ZigzagDecode(d, 3).Should().Be(-2);
        _ = ZigzagDecode(d, 4).Should().Be(2);
        _ = ZigzagDecode(d, 5).Should().Be(-3);

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
            _ = output.Should().HaveSameElementsAs(expected);
            _ = data.Should().HaveElementAt(data.Length - 1, e.ContextValue);
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
            _ = r.Should().HaveSameElementsAs(expected).And.HaveElementAt(r.Length - 1, d.ContextValue);
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
            var value2 = decoder.Decode(encoder.Encode(value));
            _ = value.Should().Be(value2);
        }
    }
}