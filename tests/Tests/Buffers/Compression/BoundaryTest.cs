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
        await CompressAndDecompress(value - 1, c);
        await CompressAndDecompress(value, c);
        await CompressAndDecompress(value + 1, c);

        static async Task CompressAndDecompress(int length, IInt32Codec c)
        {
            // Initialize array.
            var source = new int[length];
            for (var i = 0; i < source.Length; ++i)
            {
                source[i] = i;
            }

            // Compress an array.
            var compressed = new int[length];
            var (_, compressedOutputPosition) = c.Compress(source, compressed);
            await Assert.That(compressedOutputPosition).IsLessThanOrEqualTo(length);

            // Decompress an array.
            var decompressed = new int[length];
            var (_, decompressedOutputPosition) = c.Decompress(compressed.AsSpan(0, compressedOutputPosition), decompressed);

            // Compare between decompressed and original arrays.
            var target = TestUtils.CopyArray(decompressed, decompressedOutputPosition);
            await Assert.That(target).HasSameSequenceAs(source);
        }
    }
}