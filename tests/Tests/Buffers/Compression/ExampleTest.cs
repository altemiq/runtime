namespace Altemiq.Buffers.Compression;

using System.Threading.Tasks;

public class ExampleTest
{
    private readonly int[] data;

    public ExampleTest()
    {
        data = new int[2342351];

        // data should be sorted for best results
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }
    }

    [Test]
    public async Task SuperSimpleExample()
    {
        var iic = new Differential.DifferentialInt32Compressor();
        var compressed = iic.Compress(data);
        var decompressed = iic.Decompress(compressed);
        _ = await Assert.That(decompressed).HasSameSequenceAs(data);
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
        var (_, outputOffset) = codec.Compress(data, compressed);

        // got it!
        // inputOffset should be at data.Length but outputOffset tells us where we are...
        // we can repack the data: (optional)
        Array.Resize(ref compressed, outputOffset);

        // now decompressing
        // This assumes that we otherwise know how many integers have been
        // compressed. See basicExampleHeadless for a more general case.
        var decompressed = new int[data.Length];
        _ = codec.Decompress(compressed, decompressed);
        _ = await Assert.That(decompressed).HasSameSequenceAs(data);
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
        var initValue = 0;
        compressed[0] = data.Length; // we manually store how many integers we
        var (_, written) = codec.Compress(data, compressed.AsSpan(1), ref initValue);
        var outputOffset = written + 1;

        // we can repack the data: (optional)
        Array.Resize(ref compressed, outputOffset);

        var howMany = compressed[0]; // we manually stored the number of compressed integers
        var decompressed = new int[howMany];
        initValue = 0;
        _ = codec.Decompress(compressed.AsSpan(1), decompressed.AsSpan(0, howMany), ref initValue);
        _ = await Assert.That(decompressed).HasSameSequenceAs(data);
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
        var (_, compressedOffset) = codec.Compress(unsortedData, compressed);
        // we can repack the data: (optional)
        Array.Resize(ref compressed, compressedOffset);

        var decompressed = new int[N];
        _ = codec.Decompress(compressed, decompressed);
        _ = await Assert.That(decompressed).HasSameSequenceAs(unsortedData);
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
        int written;
        for (var k = 0; k < data.Length / ChunkSize; ++k)
        {
            (var read, written) = regularCodec.Compress(data.AsSpan(inputOffset, ChunkSize), compressed.AsSpan(outputOffset));
            inputOffset += read;
            outputOffset += written;
        }

        (_, written) = lastCodec.Compress(data.AsSpan(inputOffset, data.Length % ChunkSize), compressed.AsSpan(outputOffset));
        outputOffset += written;

        // we can repack the data:
        Array.Resize(ref compressed, outputOffset);

        var decompressed = new int[ChunkSize];
        var compressedOffset = 0;
        var currentPosition = 0;

        while (compressedOffset < compressed.Length)
        {
            var (read, decompressedOffset) = regularCodec.Decompress(compressed.AsSpan(compressedOffset), decompressed);
            compressedOffset += read;

            if (decompressedOffset < ChunkSize)
            {
                // last chunk detected
                (read, written) = variableByte.Decompress(compressed.AsSpan(compressedOffset), decompressed.AsSpan(decompressedOffset));
                compressedOffset += read;
                decompressedOffset += written;
            }

            for (var i = 0; i < decompressedOffset; ++i)
            {
                _ = await Assert.That(decompressed[i]).IsEqualTo(data[currentPosition + i]);
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
        var (_, outPos) = codec.Compress(uncompressed1, compressed);
        var length1 = outPos;
        var previous = outPos;
        var (_, outPos2) = codec.Compress(uncompressed2, compressed.AsSpan(outPos));
        outPos += outPos2;
        var length2 = outPos - previous;

        Array.Resize(ref compressed, length1 + length2);

        var recovered1 = new int[uncompressed1.Length];
        var recovered2 = new int[uncompressed1.Length];

        var (inPos, _) = codec.Decompress(compressed, recovered1.AsSpan(0, uncompressed1.Length));

        _ = codec.Decompress(compressed.AsSpan(inPos), recovered2.AsSpan(0, uncompressed2.Length));
        _ = await Assert.That(recovered1).HasSameSequenceAs(uncompressed1);
        _ = await Assert.That(recovered2).HasSameSequenceAs(uncompressed2);
    }
}