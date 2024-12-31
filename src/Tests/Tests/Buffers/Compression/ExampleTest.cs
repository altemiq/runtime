namespace Altemiq.Buffers.Compression;

public class ExampleTest(ITestOutputHelper testOutputHelper)
{
    [Fact]
    public void SuperSimpleExample()
    {
        var iic = new Differential.DifferentialInt32Compressor();
        var data = new int[2342351];
        for (var k = 0; k < data.Length; ++k)
        {
            data[k] = k;
        }

        testOutputHelper.WriteLine("Compressing " + data.Length + " integers using friendly interface");
        var compressed = iic.Compress(data);
        var recov = iic.Uncompress(compressed);
        testOutputHelper.WriteLine("compressed from " + (data.Length * 4 / 1024) + "KB to " + (compressed.Length * 4 / 1024) + "KB");
        _ = recov.Should().HaveSameElementsAs(data);
    }

    [Fact]
    public void BasicExample()
    {
        var data = new int[2342351];
        testOutputHelper.WriteLine("Compressing " + data.Length + " integers in one go");

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
        // inputoffset should be at data.Length but outputoffset tells
        // us where we are...
        testOutputHelper.WriteLine(
            "compressed from " + (data.Length * 4 / 1024) + "KB to " + (outputoffset * 4 / 1024) + "KB");
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
        _ = recovered.Should().HaveSameElementsAs(data);
    }

    /**
	     * Like the basicExample, but we store the input array size manually.
	     */
    [Fact]
    public void BasicExampleHeadless()
    {
        var data = new int[2342351];
        testOutputHelper.WriteLine("Compressing " + data.Length + " integers in one go using the headless approach");
        // data should be sorted for best
        // results
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

        // got it! inputoffset should be at data.Length but outputoffset tells us where we are...
        testOutputHelper.WriteLine("compressed from " + (data.Length * 4 / 1024) + "KB to " + (outputoffset * 4 / 1024) + "KB");

        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, outputoffset);

        var howmany = compressed[0]; // we manually stored the number of compressed integers
        var recovered = new int[howmany];
        var recoffset = 0;
        var inpos = 1;
        initValue = 0;
        codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length, howmany, ref initValue);
        _ = recovered.Should().HaveSameElementsAs(data);
    }

    [Fact]
    public void UnsortedExample()
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
        testOutputHelper.WriteLine("compressed unsorted integers from " + (data.Length * 4 / 1024) + "KB to " + (outputoffset * 4 / 1024) + "KB");
        // we can repack the data: (optional)
        System.Array.Resize(ref compressed, outputoffset);

        var recovered = new int[N];
        var recoffset = 0;
        var inpos = 0;
        codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length);
        _ = recovered.Should().HaveSameElementsAs(data);
    }

    [Fact]
    public void AdvancedExample()
    {
        const int TotalSize = 2342351; // some arbitrary number
        const int ChunkSize = 16384; // size of each chunk, choose a multiple of 128
        testOutputHelper.WriteLine("Compressing " + TotalSize + " integers using chunks of " + ChunkSize + " integers (" + (ChunkSize * 4 / 1024) + "KB)");
        testOutputHelper.WriteLine("(It is often better for applications to work in chunks fitting in CPU cache.)");
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

        // got it! inputoffset should be at data.Length but outputoffset tells us where we are...
        testOutputHelper.WriteLine("compressed from " + (data.Length * 4 / 1024) + "KB to " + (outputoffset * 4 / 1024) + "KB");

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
                if (data[currentpos + i] != recovered[i])
                {
                    throw new Exception("bug"); // could use assert
                }
            }
            currentpos += recoffset;
        }
        testOutputHelper.WriteLine("data is recovered without loss");
    }

    [Fact]
    public void HeadlessDemo()
    {
        testOutputHelper.WriteLine("Compressing arrays with minimal header...");
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
        testOutputHelper.WriteLine("compressed unsorted integers from " + (uncompressed1.Length * 4) + "B to " + (length1 * 4) + "B");
        testOutputHelper.WriteLine("compressed unsorted integers from " + (uncompressed2.Length * 4) + "B to " + (length2 * 4) + "B");
        testOutputHelper.WriteLine("Total compressed output " + compressed.Length);

        var recovered1 = new int[uncompressed1.Length];
        var recovered2 = new int[uncompressed1.Length];

        inPos = 0;
        outPos = 0;
        testOutputHelper.WriteLine("Decoding first array starting at pos = " + inPos);
        codec.Decompress(compressed, ref inPos, recovered1, ref outPos, compressed.Length, uncompressed1.Length);

        outPos = 0;
        testOutputHelper.WriteLine("Decoding second array starting at pos = " + inPos);
        codec.Decompress(compressed, ref inPos, recovered2, ref outPos, compressed.Length, uncompressed2.Length);
        _ = recovered1.Should().HaveSameElementsAs(uncompressed1);
        _ = recovered2.Should().HaveSameElementsAs(uncompressed2);

        testOutputHelper.WriteLine("The arrays match, your code is probably ok.");
    }
}