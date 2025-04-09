namespace Altemiq.Buffers.Compression;

using System.Threading.Tasks;

public class ExampleTest
{
    private readonly int[] data;

    public ExampleTest()
    {
        this.data = new int[2342351];

        // data should be sorted for best results
        for (var k = 0; k < this.data.Length; ++k)
        {
            this.data[k] = k;
        }
    }

    [Test]
    public async Task SuperSimpleExample()
    {
        var iic = new Differential.DifferentialInt32Compressor();
        var compressed = iic.Compress(data);
        var decompressed = iic.Decompress(compressed);
        await Assert.That(decompressed).IsEquivalentTo(data);
    }

    [Test]
    public async Task BasicExample()
    {
        // Very important: the data is in sorted order!!! If not, you
        // will get very poor compression with IntegratedBinaryPacking,
        // you should use another CODEC.

        // next we compose a CODEC. Most of the processing
        // will be done with binary packing, and leftovers will
        // be processed using variable byte
        var codec = new Differential.DifferentialComposition(new Differential.DifferentialBinaryPacking(), new Differential.DifferentialVariableByte());

        // output vector should be large enough...
        var compressed = new int[data.Length + 1024];

        // compressed might not be large enough in some cases
        // if you get IndexOutOfBoundsException, try
        // allocating more memory
        var inputOffset = 0;
        var outputOffset = 0;
        codec.Compress(data, ref inputOffset, compressed, ref outputOffset, data.Length);

        // got it!
        // inputOffset should be at data.Length but outputOffset tells us where we are...
        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, outputOffset);

        // now decompressing
        // This assumes that we otherwise know how many integers have been
        // compressed. See basicExampleHeadless for a more general case.
        var decompressed = new int[data.Length];
        var decompressedOffset = 0;
        var decompressedPosition = 0;
        codec.Decompress(compressed, ref decompressedPosition, decompressed, ref decompressedOffset, compressed.Length);
        await Assert.That(decompressed).IsEquivalentTo(data);
    }

    // Like the BasicExample, but we store the input array size manually.
    [Test]
    public async Task BasicExampleHeadless()
    {
        // Very important: the data is in sorted order!!! If not, you
        // will get very poor compression with IntegratedBinaryPacking,
        // you should use another CODEC.

        // next we compose a CODEC. Most of the processing
        // will be done with binary packing, and leftovers will
        // be processed using variable byte
        var codec = new Differential.HeadlessDifferentialComposition(new Differential.DifferentialBinaryPacking(), new Differential.DifferentialVariableByte());

        // output vector should be large enough...
        var compressed = new int[data.Length + 1024];

        // compressed might not be large enough in some cases
        // if you get IndexOutOfBoundsException, try allocating more memory
        var inputOffset = 0;
        var outputOffset = 1;
        var initValue = 0;
        compressed[0] = data.Length; // we manually store how many integers we
        codec.Compress(data, ref inputOffset, compressed, ref outputOffset, data.Length, ref initValue);

        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, outputOffset);

        var howMany = compressed[0]; // we manually stored the number of compressed integers
        var decompressed = new int[howMany];
        var decompressedOffset = 0;
        var compressedOffset = 1;
        initValue = 0;
        codec.Decompress(compressed, ref compressedOffset, decompressed, ref decompressedOffset, compressed.Length, howMany, ref initValue);
        await Assert.That(decompressed).IsEquivalentTo(data);
    }

    [Test]
    public async Task UnsortedExample()
    {
        const int N = 1333333;
        var unsortedData = new int[N];

        // initialize the data (most will be small)
        for (var k = 0; k < N; k += 1)
        {
            unsortedData[k] = 3;
        }

        // throw some larger values
        for (var k = 0; k < N; k += 5)
        {
            unsortedData[k] = 100;
        }

        for (var k = 0; k < N; k += 533)
        {
            unsortedData[k] = 10000;
        }

        var compressed = new int[N + 1024]; // could need more
        var codec = new Composition(new FastPatchingFrameOfReference256(), new VariableByte());
        // compressing
        var inputOffset = 0;
        var compressedOffset = 0;
        codec.Compress(unsortedData, ref inputOffset, compressed, ref compressedOffset, unsortedData.Length);
        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, compressedOffset);

        var decompressed = new int[N];
        var decompressedOffset = 0;
        compressedOffset = 0;
        codec.Decompress(compressed, ref compressedOffset, decompressed, ref decompressedOffset, compressed.Length);
        await Assert.That(decompressed).IsEquivalentTo(unsortedData);
    }

    [Test]
    public async Task AdvancedExample()
    {
        const int ChunkSize = 16384; // size of each chunk, choose a multiple of 128

        // next we compose a CODEC. Most of the processing
        // will be done with binary packing, and leftovers will
        // be processed using variable byte, using variable byte
        // only for the last chunk!
        var regularCodec = new Differential.DifferentialBinaryPacking();
        var variableByte = new Differential.DifferentialVariableByte();
        var lastCodec = new Differential.DifferentialComposition(regularCodec, variableByte);

        // output vector should be large enough...
        var compressed = new int[data.Length + 1024];

        var inputOffset = 0;
        var outputOffset = 0;
        for (var k = 0; k < data.Length / ChunkSize; ++k)
        {
            regularCodec.Compress(data, ref inputOffset, compressed, ref outputOffset, ChunkSize);
        }

        lastCodec.Compress(data, ref inputOffset, compressed, ref outputOffset, data.Length % ChunkSize);

        // we can repack the data:
        System.Array.Resize(ref compressed, outputOffset);

        var decompressed = new int[ChunkSize];
        var compressedOffset = 0;
        var currentPosition = 0;

        while (compressedOffset < compressed.Length)
        {
            var decompressedOffset = 0;
            regularCodec.Decompress(compressed, ref compressedOffset, decompressed, ref decompressedOffset, compressed.Length - compressedOffset);

            if (decompressedOffset < ChunkSize)
            {
                // last chunk detected
                variableByte.Decompress(compressed, ref compressedOffset, decompressed, ref decompressedOffset, compressed.Length - compressedOffset);
            }

            for (var i = 0; i < decompressedOffset; ++i)
            {
                await Assert.That(decompressed[i]).IsEqualTo(data[currentPosition + i]);
            }

            currentPosition += decompressedOffset;
        }
    }

    [Test]
    public async Task HeadlessDemo()
    {
        int[] uncompressed1 = [1, 2, 1, 3, 1];
        int[] uncompressed2 = [3, 2, 4, 6, 1];

        var compressed = new int[uncompressed1.Length + uncompressed2.Length + 1024];

        var codec = new HeadlessComposition(new BinaryPacking(), new VariableByte());

        // compressing
        var inPos = 0;
        var outPos = 0;

        codec.Compress(uncompressed1, ref inPos, compressed, ref outPos, uncompressed1.Length);
        var length1 = outPos;
        var previous = outPos;
        inPos = 0;
        codec.Compress(uncompressed2, ref inPos, compressed, ref outPos, uncompressed2.Length);
        var length2 = outPos - previous;

        System.Array.Resize(ref compressed, length1 + length2);

        var recovered1 = new int[uncompressed1.Length];
        var recovered2 = new int[uncompressed1.Length];

        inPos = 0;
        outPos = 0;
        codec.Decompress(compressed, ref inPos, recovered1, ref outPos, compressed.Length, uncompressed1.Length);

        outPos = 0;
        codec.Decompress(compressed, ref inPos, recovered2, ref outPos, compressed.Length, uncompressed2.Length);
        await Assert.That(recovered1).IsEquivalentTo(uncompressed1);
        await Assert.That(recovered2).IsEquivalentTo(uncompressed2);
    }
}