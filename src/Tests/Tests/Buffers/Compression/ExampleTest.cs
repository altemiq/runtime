namespace Altemiq.Buffers.Compression;

using System.Threading.Tasks;

public class ExampleTest
{
    [Test]
    public async Task SuperSimpleExample()
    {
        var iic = new Differential.DifferentialInt32Compressor();
        var data = new int[2342351];
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }

        var compressed = iic.Compress(data);
        var recov = iic.Uncompress(compressed);
        await Assert.That(recov).IsEquivalentTo(data);
    }

    [Test]
    public async Task BasicExample()
    {
        var data = new int[2342351];

        // data should be sorted for best results
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }

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
        // if you get java.lang.ArrayIndexOutOfBoundsException, try
        // allocating more memory
        var inputoffset = 0;
        var outputoffset = 0;
        codec.Compress(data, ref inputoffset, compressed, ref outputoffset, data.Length);

        // got it!
        // inputoffset should be at data.Length but outputoffset tells us where we are...
        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, outputoffset);

        /**
		     *
		     * now uncompressing
		     *
		     * This assumes that we otherwise know how many integers have been
		     * compressed. See basicExampleHeadless for a more general case.
		     */
        var recovered = new int[data.Length];
        var recoffset = 0;
        var inpos = 0;
        codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length);
        await Assert.That(recovered).IsEquivalentTo(data);
    }

    /**
	     * Like the basicExample, but we store the input array size manually.
	     */
    [Test]
    public async Task BasicExampleHeadless()
    {
        var data = new int[2342351];
        // data should be sorted for best results
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }

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
        // if you get java.lang.ArrayIndexOutOfBoundsException, try
        // allocating more memory
        var inputoffset = 0;
        var outputoffset = 1;
        var initValue = 0;
        compressed[0] = data.Length; // we manually store how many integers we
        codec.Compress(data, ref inputoffset, compressed, ref outputoffset, data.Length, ref initValue);

        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, outputoffset);

        var howmany = compressed[0]; // we manually stored the number of compressed integers
        var recovered = new int[howmany];
        var recoffset = 0;
        var inpos = 1;
        initValue = 0;
        codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length, howmany, ref initValue);
        await Assert.That(recovered).IsEquivalentTo(data);
    }

    [Test]
    public async Task UnsortedExample()
    {
        const int N = 1333333;
        var data = new int[N];

        // initialize the data (most will be small)
        for (var k = 0; k < N; k += 1)
        {
            data[k] = 3;
        }
        // throw some larger values
        for (var k = 0; k < N; k += 5)
        {
            data[k] = 100;
        }

        for (var k = 0; k < N; k += 533)
        {
            data[k] = 10000;
        }

        var compressed = new int[N + 1024]; // could need more
        var codec = new Composition(new FastPatchingFrameOfReference256(), new VariableByte());
        // compressing
        var inputoffset = 0;
        var outputoffset = 0;
        codec.Compress(data, ref inputoffset, compressed, ref outputoffset, data.Length);
        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, outputoffset);

        var recovered = new int[N];
        var recoffset = 0;
        var inpos = 0;
        codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length);
        await Assert.That(recovered).IsEquivalentTo(data);
    }

    [Test]
    public async Task AdvancedExample()
    {
        const int TotalSize = 2342351; // some arbitrary number
        const int ChunkSize = 16384; // size of each chunk, choose a multiple of 128
        var data = new int[TotalSize];
        // data should be sorted for best results
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }
        // next we compose a CODEC. Most of the processing
        // will be done with binary packing, and leftovers will
        // be processed using variable byte, using variable byte
        // only for the last chunk!
        var regularcodec = new Differential.DifferentialBinaryPacking();
        var ivb = new Differential.DifferentialVariableByte();
        var lastcodec = new Differential.DifferentialComposition(regularcodec, ivb);

        // output vector should be large enough...
        var compressed = new int[TotalSize + 1024];

        var inputoffset = 0;
        var outputoffset = 0;
        for (var k = 0; k < TotalSize / ChunkSize; ++k)
        {
            regularcodec.Compress(data, ref inputoffset, compressed, ref outputoffset, ChunkSize);
        }

        lastcodec.Compress(data, ref inputoffset, compressed, ref outputoffset, TotalSize % ChunkSize);

        // we can repack the data:
        System.Array.Resize(ref compressed, outputoffset);

        var recovered = new int[ChunkSize];
        var compoff = 0;
        var currentpos = 0;

        while (compoff < compressed.Length)
        {
            var recoffset = 0;
            regularcodec.Decompress(compressed, ref compoff, recovered, ref recoffset, compressed.Length - compoff);

            if (recoffset < ChunkSize)
            {
                // last chunk detected
                ivb.Decompress(compressed, ref compoff, recovered, ref recoffset, compressed.Length - compoff);
            }

            for (var i = 0; i < recoffset; ++i)
            {
                await Assert.That(recovered[i]).IsEqualTo(data[currentpos + i]);
            }

            currentpos += recoffset;
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
        var previous = 0;

        codec.Compress(uncompressed1, ref inPos, compressed, ref outPos, uncompressed1.Length);
        var length1 = outPos - previous;
        previous = outPos;
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