namespace Altemiq.Buffers.Compression;

public class BoundaryTest
{
    [Theory]
    [InlineData(32)]
    [InlineData(128)]
    [InlineData(256)]
    public void TestIntegratedComposition(int value)
    {
        Around(new Differential.DifferentialComposition(new Differential.DifferentialBinaryPacking(), new Differential.DifferentialVariableByte()), value);
    }

    [Theory]
    [InlineData(32)]
    [InlineData(128)]
    [InlineData(256)]
    public void TestComposition(int value)
    {
        Around(new Composition(new BinaryPacking(), new VariableByte()), value);
    }

    private static void Around(IInt32Codec c, int value)
    {
        CompressAndUncompress(value - 1, c);
        CompressAndUncompress(value, c);
        CompressAndUncompress(value + 1, c);

        static void CompressAndUncompress(int length, IInt32Codec c)
        {
            // Initialize array.
            var source = new int[length];
            for (var i = 0; i < source.Length; ++i)
            {
                source[i] = i;
            }

            // Compress an array.
            var compressed = new int[length];
            var c_inpos = 0;
            var c_outpos = 0;
            c.Compress(source, ref c_inpos, compressed, ref c_outpos, source.Length);
            Assert.True(c_outpos <= length);

            // Uncompress an array.
            var uncompressed = new int[length];
            var u_inpos = 0;
            var u_outpos = 0;
            c.Decompress(compressed, ref u_inpos, uncompressed, ref u_outpos, c_outpos);

            // Compare between uncompressed and original arrays.
            var target = TestUtils.CopyArray(uncompressed, u_outpos);
            Assert.Equal(source, target);
        }
    }
}