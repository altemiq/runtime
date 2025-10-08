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
            var compressedInputPosition = 0;
            var compressedOutputPosition = 0;
            c.Compress(source, ref compressedInputPosition, compressed, ref compressedOutputPosition, source.Length);
            await Assert.That(compressedOutputPosition).IsLessThanOrEqualTo(length);

            // Decompress an array.
            var decompressed = new int[length];
            var decompressedInputPosition = 0;
            var decompressedOutputPosition = 0;
            c.Decompress(compressed, ref decompressedInputPosition, decompressed, ref decompressedOutputPosition, compressedOutputPosition);

            // Compare between decompressed and original arrays.
            var target = TestUtils.CopyArray(decompressed, decompressedOutputPosition);
            await Assert.That(target).IsEquivalentTo(source);
        }
    }
}