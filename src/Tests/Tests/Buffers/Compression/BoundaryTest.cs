namespace Altemiq.Buffers.Compression;

public class BoundaryTest
{
    [Test]
    [Arguments(32)]
    [Arguments(128)]
    [Arguments(256)]
    public async Task TestIntegratedComposition(int value)
    {
        await Around(new Differential.DifferentialComposition(new Differential.DifferentialBinaryPacking(), new Differential.DifferentialVariableByte()), value);
    }

    [Test]
    [Arguments(32)]
    [Arguments(128)]
    [Arguments(256)]
    public async Task TestComposition(int value)
    {
        await Around(new Composition(new BinaryPacking(), new VariableByte()), value);
    }

    private static async Task Around(IInt32Codec c, int value)
    {
        await CompressAndUncompress(value - 1, c);
        await CompressAndUncompress(value, c);
        await CompressAndUncompress(value + 1, c);

        static async Task CompressAndUncompress(int length, IInt32Codec c)
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
            await Assert.That(c_outpos).IsLessThanOrEqualTo(length);

            // Uncompress an array.
            var uncompressed = new int[length];
            var u_inpos = 0;
            var u_outpos = 0;
            c.Decompress(compressed, ref u_inpos, uncompressed, ref u_outpos, c_outpos);

            // Compare between uncompressed and original arrays.
            var target = TestUtils.CopyArray(uncompressed, u_outpos);
            await Assert.That(target).IsEquivalentTo(source);
        }
    }
}