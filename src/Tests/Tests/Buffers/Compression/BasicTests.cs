namespace Altemiq.Buffers.Compression;

public partial class BasicTests
{
    private static readonly IEnumerable<Func<IInt32Codec>> codecs =
    [
        () => new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>(),
        () => new Copy(),
        () => new VariableByte(),
        () => new Differential.DifferentialVariableByte(),
        () => new Composition<BinaryPacking, VariableByte>(),
        () => new Composition<NewPfdS9, VariableByte>(),
        () => new Composition<NewPfdS16, VariableByte>(),
        () => new Composition<OptPfdS9, VariableByte>(),
        () => new Composition<OptPfdS16, VariableByte>(),
        () => new Composition<FastPatchingFrameOfReference128, VariableByte>(),
        () => new Composition<FastPatchingFrameOfReference256, VariableByte>(),
        () => new Simple9(),
        () => new Simple16(),
        () => new Composition<Differential.XorBinaryPacking, VariableByte>(),
        () => new Composition<DeltaZigzagBinaryPacking, DeltaZigzagVariableByte>()
    ];

    [Test]
    [MethodDataSource(nameof(IntegerCodecs))]
    public async Task SaulTest(IntegerCodec codec)
    {
        var ic = codec.Codec!;

        for (var x = 0; x < 50; ++x)
        {
            int[] a = [2, 3, 4, 5];
            var b = new int[90];
            var c = new int[a.Length];

            var aOffset = 0;
            var bOffset = x;
            ic.Compress(a, ref aOffset, b, ref bOffset, a.Length);
            var len = bOffset - x;

            bOffset = x;
            var cOffset = 0;
            ic.Decompress(b, ref bOffset, c, ref cOffset, len);
            await Assert.That(c).IsEquivalentTo(a);
        }
    }

    [Test]
    [MethodDataSource(nameof(IntegerCodecs))]
    public async Task VaryingLengthTest(IntegerCodec codec)
    {
        const int N = 4096;
        var data = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = k;
        }

        var c = codec.Codec!;

        for (var L = 1; L <= 128; L++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, L));
            var answer = TestUtils.Uncompress(c, comp, L);
            for (var k = 0; k < L; ++k)
            {
                await Assert.That(answer[k]).IsEqualTo(data[k]);
            }
        }
        for (var L = 128; L <= N; L *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, L));
            var answer = TestUtils.Uncompress(c, comp, L);
            for (var k = 0; k < L; ++k)
            {
                await Assert.That(answer[k]).IsEqualTo(data[k]);
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(ComplexCodecs))]
    public async Task VaryingLengthTest2(IntegerCodec codec)
    {
        const int N = 128;
        var data = new int[N];
        data[127] = -1;
        var c = codec.Codec!;

        for (var L = 1; L <= 128; L++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, L));
            var answer = TestUtils.Uncompress(c, comp, L);
            for (var k = 0; k < L; ++k)
            {
                await Assert.That(answer[k]).IsEqualTo(data[k]);
            }
        }
        for (var L = 128; L <= N; L *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, L));
            var answer = TestUtils.Uncompress(c, comp, L);
            for (var k = 0; k < L; ++k)
            {
                await Assert.That(answer[k]).IsEqualTo(data[k]);
            }
        }
    }

    public static IEnumerable<Func<IntegerCodec>> IntegerCodecs()
    {
        return codecs.Select(c => new Func<IntegerCodec>(() => new IntegerCodec(c())));
    }

    public static IEnumerable<Func<IntegerCodec>> ComplexCodecs()
    {
        return codecs.Select(c => c()).Where(c => c is not Simple9 and not Simple16).Select(c => new Func<IntegerCodec>(() => new IntegerCodec(c)));
    }

    [Test]
    public Task CheckDeltaZigzagZero()
    {
        return TestZeroInZeroOut(new DeltaZigzagVariableByte());
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public Task CheckDeltaZigzagVariableByte(int[][] data)
    {
        return TestCodec(new DeltaZigzagVariableByte(), new DeltaZigzagVariableByte(), data);
    }

    [Test]
    public async Task CheckDeltaZigzagPacking()
    {
        var codec = new DeltaZigzagBinaryPacking();
        await TestZeroInZeroOut(codec);
        await TestSpurious(codec);
    }

    [Test]
    public async Task CheckDeltaZigzagZeroInZeroOut()
    {
        IInt32Codec compo = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());
        await TestZeroInZeroOut(compo);
    }

    [Test]
    public async Task CheckDeltaZigzagUnsorted()
    {
        IInt32Codec compo = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());
        await TestUnsorted(compo);
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task CheckDeltaZigzagPackingComposition(int[][] data)
    {
        IInt32Codec compo = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());
        IInt32Codec compo2 = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());

        await TestCodec(compo, compo2, data);
    }

    [Test]
    public async Task CheckXorBinaryPacking()
    {
        await TestZeroInZeroOut(new Differential.XorBinaryPacking());
        await TestSpurious(new Differential.XorBinaryPacking());
    }

    [Test]
    public Task CheckXorBinaryPacking1()
    {
        return TestZeroInZeroOut(new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte()));
    }

    [Test]
    public Task CheckXorBinaryPacking2()
    {
        return TestUnsorted(new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte()));
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task CheckXorBinaryPacking3(int[][] data)
    {
        IInt32Codec c = new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte());
        IInt32Codec co = new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte());
        await TestCodec(c, co, data);
    }

    public static IEnumerable<Func<int[][]>> Data()
    {
        return GetAllData().Select(x => new Func<int[][]>(() => x));

        static IEnumerable<int[][]> GetAllData()
        {
            return GetData(5, 10).Concat(GetData(5, 14)).Concat(GetData(2, 18));

            static IEnumerable<int[][]> GetData(int N, int nbr)
            {
                var cdg = new ClusteredDataGenerator();
                for (var sparsity = 1; sparsity < 31 - nbr; sparsity += 4)
                {
                    var data = new int[N][];
                    var max = 1 << (nbr + 9);
                    for (var k = 0; k < N; ++k)
                    {
                        data[k] = cdg.GenerateClustered(1 << nbr, max);
                    }

                    yield return data;
                }
            }
        }
    }

    [Test]
    [Arguments(0)]
    [Arguments(8)]
    [Arguments(16)]
    [Arguments(24)]
    [Arguments(30)]
    public async Task VerifyBitPacking(int bit)
    {
        const int N = 32;
        var r = new Random();
        var data = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = r.Next(1 << bit);
        }

        var compressed = new int[N];
        var uncompressed = new int[N];

        BitPacking.Pack(data.AsSpan(0), compressed.AsSpan(0), bit);
        BitPacking.Unpack(compressed.AsSpan(0), uncompressed.AsSpan(0), bit);

        await Assert.That(data).IsEquivalentTo(data);
    }

    [Test]
    [Arguments(0)]
    [Arguments(8)]
    [Arguments(16)]
    [Arguments(24)]
    [Arguments(30)]
    public async Task VerifyWithoutMask(int bit)
    {
        const int N = 32;
        var r = new Random();
        var data = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = r.Next(1 << bit);
        }

        var compressed = new int[N];
        var uncompressed = new int[N];
        BitPacking.PackWithoutMask(data.AsSpan(0), compressed.AsSpan(0), bit);
        BitPacking.Unpack(compressed.AsSpan(0), uncompressed.AsSpan(0), bit);

        await Assert.That(data).IsEquivalentTo(data);
    }

    [Test]
    [Arguments(0)]
    [Arguments(8)]
    [Arguments(16)]
    [Arguments(24)]
    [Arguments(30)]
    public async Task VerifyWithExceptions(int bit)
    {
        const int N = 32;
        const int TIMES = 1000;
        var r = new Random();
        var data = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = r.Next();
        }

        var compressed = new int[N];
        var uncompressed = new int[N];
        for (var t = 0; t < TIMES; ++t)
        {
            BitPacking.Pack(data.AsSpan(0), compressed.AsSpan(0), bit);
            BitPacking.Unpack(compressed.AsSpan(0), uncompressed.AsSpan(0), bit);

            // Check assertions.
            MaskArray(data, (1 << bit) - 1);
            await Assert.That(uncompressed).IsEquivalentTo(data);
        }

        static void MaskArray(int[] array, int mask)
        {
            for (int i = 0, end = array.Length; i < end; ++i)
            {
                array[i] &= mask;
            }
        }
    }

    [Test]
    [MethodDataSource(nameof(BasicTestData))]
    public Task BasicTest(IntegerCodec c, IntegerCodec co, int[][] data)
    {
        return TestCodec(c.Codec, co.Codec, data);
    }

    public static IEnumerable<Func<(IntegerCodec, IntegerCodec, int[][])>> BasicTestData()
    {
        return AddData(5, 10).Concat(AddData(5, 14)).Concat(AddData(2, 18));

        static IEnumerable<Func<(IntegerCodec, IntegerCodec, int[][])>> AddData(int N, int nbr)
        {
            for (var sparsity = 1; sparsity < 31 - nbr; sparsity += 4)
            {
                var generatedData = GenerateData(N, nbr, sparsity);
                yield return () => (new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()), new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Copy()), new IntegerCodec(new Copy()), generatedData);
                yield return () => (new IntegerCodec(new VariableByte()), new IntegerCodec(new VariableByte()), generatedData);
                yield return () => (new IntegerCodec(new Differential.DifferentialVariableByte()), new IntegerCodec(new Differential.DifferentialVariableByte()), generatedData);
                yield return () => (new IntegerCodec(new Composition<BinaryPacking, VariableByte>()), new IntegerCodec(new Composition<BinaryPacking, VariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Composition<NewPfdS9, VariableByte>()), new IntegerCodec(new Composition<NewPfdS9, VariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Composition<NewPfdS16, VariableByte>()), new IntegerCodec(new Composition<NewPfdS16, VariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Composition<OptPfdS9, VariableByte>()), new IntegerCodec(new Composition<OptPfdS9, VariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Composition<OptPfdS16, VariableByte>()), new IntegerCodec(new Composition<OptPfdS16, VariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>()), new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>()), new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>()), generatedData);
                yield return () => (new IntegerCodec(new Simple9()), new IntegerCodec(new Simple9()), generatedData);
            }

            static int[][] GenerateData(int N, int nbr, int sparsity)
            {
                var cdg = new ClusteredDataGenerator();

                var data = new int[N][];
                var max = 1 << (nbr + sparsity);
                for (var k = 0; k < N; ++k)
                {
                    data[k] = cdg.GenerateClustered(1 << nbr, max);
                }

                return data;
            }
        }
    }

    [Test]
    [Arguments(typeof(Differential.DifferentialBinaryPacking))]
    [Arguments(typeof(BinaryPacking))]
    [Arguments(typeof(NewPfdS9))]
    [Arguments(typeof(NewPfdS16))]
    [Arguments(typeof(OptPfdS9))]
    [Arguments(typeof(OptPfdS16))]
    [Arguments(typeof(FastPatchingFrameOfReference128))]
    [Arguments(typeof(FastPatchingFrameOfReference256))]
    public async Task SpuriousOut(Type type)
    {
        var codec = Activator.CreateInstance(type) as IInt32Codec;
        await Assert.That(codec).IsNotNull();
        await TestSpurious(codec!);
    }

    [Test]
    [MethodDataSource(nameof(ZeroInZeroOutData))]
    public Task ZeroInZeroOut(IntegerCodec codec)
    {
        return TestZeroInZeroOut(codec.Codec!);
    }

    public static IEnumerable<Func<IntegerCodec>> ZeroInZeroOutData()
    {
        yield return () => new IntegerCodec(new Differential.DifferentialBinaryPacking());
        yield return () => new IntegerCodec(new Differential.DifferentialVariableByte());
        yield return () => new IntegerCodec(new BinaryPacking());
        yield return () => new IntegerCodec(new NewPfdS9());
        yield return () => new IntegerCodec(new NewPfdS16());
        yield return () => new IntegerCodec(new OptPfdS9());
        yield return () => new IntegerCodec(new OptPfdS16());
        yield return () => new IntegerCodec(new FastPatchingFrameOfReference128());
        yield return () => new IntegerCodec(new FastPatchingFrameOfReference256());
        yield return () => new IntegerCodec(new VariableByte());
        yield return () => new IntegerCodec(new Composition<Differential.DifferentialBinaryPacking, VariableByte>());
        yield return () => new IntegerCodec(new Composition<BinaryPacking, VariableByte>());
        yield return () => new IntegerCodec(new Composition<NewPfdS9, VariableByte>());
        yield return () => new IntegerCodec(new Composition<NewPfdS16, VariableByte>());
        yield return () => new IntegerCodec(new Composition<OptPfdS9, VariableByte>());
        yield return () => new IntegerCodec(new Composition<OptPfdS16, VariableByte>());
        yield return () => new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>());
        yield return () => new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>());
        yield return () => new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>());
    }

    private static async Task TestSpurious(IInt32Codec c)
    {
        var x = new int[1024];
        int[] y = [];
        var i0 = 0;
        var i1 = 0;
        for (var inlength = 0; inlength < 32; ++inlength)
        {
            c.Compress(x, ref i0, y, ref i1, inlength);
            await Assert.That(i1).IsEqualTo(0);
        }
    }

    private static async Task TestZeroInZeroOut(IInt32Codec c)
    {
        int[] x = [];
        int[] y = [];
        var i0 = 0;
        var i1 = 0;
        c.Compress(x, ref i0, y, ref i1, 0);
        await Assert.That(i1).IsEqualTo(0);

        int[] @out = [];
        var outpos = 0;
        c.Decompress(y, ref i1, @out, ref outpos, 0);
        await Assert.That(outpos).IsEqualTo(0);
    }

    //private static void Test(IInt32Codec c, IInt32Codec co, int N, int nbr)
    //{
    //    var cdg = new ClusteredDataGenerator();
    //    for (var sparsity = 1; sparsity < 31 - nbr; sparsity += 4)
    //    {
    //        var data = new int[N][];
    //        var max = 1 << (nbr + 9);
    //        for (var k = 0; k < N; ++k)
    //        {
    //            data[k] = cdg.GenerateClustered(1 << nbr, max);
    //        }

    //        TestCodec(c, co, data);
    //    }
    //}

    private static async Task TestCodec(IInt32Codec? c, IInt32Codec? co, int[][] data)
    {
        await Assert.That(c).IsNotNull();
        await Assert.That(co).IsNotNull();

        var N = data.Length;
        var maxlength = 0;
        for (var k = 0; k < N; ++k)
        {
            if (data[k].Length > maxlength)
            {
                maxlength = data[k].Length;
            }
        }

        var buffer = new int[maxlength + 1024];

        // 4x + 1024 to account for the possibility of some negative compression.
        var dataout = new int[(4 * maxlength) + 1024];
        var inpos = 0;
        var outpos = 0;
        for (var k = 0; k < N; ++k)
        {
            var backupdata = TestUtils.CopyArray(data[k], data[k].Length);

            inpos = 1;
            outpos = 0;

            if (c is not Differential.IDifferentialInt32Codec)
            {
                Differential.Delta.Forward(backupdata);
            }

            c!.Compress(backupdata, ref inpos, dataout, ref outpos, backupdata.Length - inpos);

            var thiscompsize = outpos + 1;
            inpos = 0;
            outpos = 1;
            buffer[0] = backupdata[0];
            co!.Decompress(dataout, ref inpos, buffer, ref outpos, thiscompsize - 1);

            if (c is not Differential.IDifferentialInt32Codec)
            {
                Differential.Delta.Inverse(buffer.AsSpan(0, outpos));
            }

            // Check assertions.
            await Assert.That(data[k]).HasCount().EqualTo(outpos);
            var bufferCutout = TestUtils.CopyArray(buffer, outpos);
            await Assert.That(data[k]).IsEquivalentTo(bufferCutout);
        }
    }

    [Test]
    [MethodDataSource(nameof(ExampleCodecs))]
    public async Task TestUnsortedExample(IntegerCodec codec)
    {
        var c = codec.Codec!;
        await TestUnsorted(c);
        await TestUnsorted2(c);
        await TestUnsorted3(c);
    }

    public static IEnumerable<Func<IntegerCodec>> ExampleCodecs()
    {
        yield return () => new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>());
        yield return () => new IntegerCodec(new Composition<Differential.DifferentialBinaryPacking, VariableByte>());
        yield return () => new IntegerCodec(new VariableByte());
        yield return () => new IntegerCodec(new Differential.DifferentialVariableByte());
        yield return () => new IntegerCodec(new Composition<BinaryPacking, VariableByte>());
        yield return () => new IntegerCodec(new Composition<NewPfdS9, VariableByte>());
        yield return () => new IntegerCodec(new Composition<NewPfdS16, VariableByte>());
        yield return () => new IntegerCodec(new Composition<OptPfdS9, VariableByte>());
        yield return () => new IntegerCodec(new Composition<OptPfdS16, VariableByte>());
        yield return () => new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>());
        yield return () => new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>());
    }

    private static async Task TestUnsorted(IInt32Codec codec)
    {
        int[] lengths = [133, 1026, 1333333];
        foreach (var N in lengths)
        {
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

            data[5] = -311;
            // could need more compressing
            var compressed = new int[(int)Math.Ceiling(N * 1.01) + 1024];
            var inputoffset = 0;
            var outputoffset = 0;
            codec.Compress(data, ref inputoffset, compressed, ref outputoffset, data.Length);
            // we can repack the data: (optional)
            compressed = TestUtils.CopyArray(compressed, outputoffset);

            var recovered = new int[N];
            var recoffset = 0;
            var inpos = 0;
            codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length);
            await Assert.That(data).IsEquivalentTo(data);
        }
    }

    private static async Task TestUnsorted2(IInt32Codec codec)
    {
        var data = new int[128];
        data[5] = -1;
        var compressed = new int[1024];
        var inputoffset = 0;
        var outputoffset = 0;
        codec.Compress(data, ref inputoffset, compressed, ref outputoffset, data.Length);
        // we can repack the data: (optional)
        compressed = TestUtils.CopyArray(compressed, outputoffset);

        var recovered = new int[128];
        var recoffset = 0;
        var inpos = 0;
        codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length);
        await Assert.That(data).IsEquivalentTo(data);
    }

    private static async Task TestUnsorted3(IInt32Codec codec)
    {
        var data = new int[128];
        data[127] = -1;
        var compressed = new int[1024];
        var inputoffset = 0;
        var outputoffset = 0;
        codec.Compress(data, ref inputoffset, compressed, ref outputoffset, data.Length);
        // we can repack the data: (optional)
        compressed = TestUtils.CopyArray(compressed, outputoffset);

        var recovered = new int[128];
        var recoffset = 0;
        var inpos = 0;
        codec.Decompress(compressed, ref inpos, recovered, ref recoffset, compressed.Length);
        await Assert.That(data).IsEquivalentTo(data);
    }

    [Test]
    public async Task FastPFor()
    {
        // proposed by Stefan Ackermann (https://github.com/Stivo)
        var codec1 = new FastPatchingFrameOfReference256();
        var codec2 = new FastPatchingFrameOfReference256();
        const int N = FastPatchingFrameOfReference256.BlockSize;
        var data = new int[N];
        for (var i = 0; i < N; i++)
        {
            data[i] = 0;
        }

        data[126] = -1;
        var comp = TestUtils.Compress(codec1, TestUtils.CopyArray(data, N));
        await Assert.That(TestUtils.Uncompress(codec2, comp, N)).IsEquivalentTo(data);
    }

    [Test]
    public async Task FastPFor128()
    {
        // proposed by Stefan Ackermann (https://github.com/Stivo)
        var codec1 = new FastPatchingFrameOfReference128();
        var codec2 = new FastPatchingFrameOfReference128();
        const int N = FastPatchingFrameOfReference128.BlockSize;
        var data = new int[N];
        for (var i = 0; i < N; i++)
        {
            data[i] = 0;
        }

        data[126] = -1;
        var comp = TestUtils.Compress(codec1, TestUtils.CopyArray(data, N));
        await Assert.That(TestUtils.Uncompress(codec2, comp, N)).IsEquivalentTo(data);
    }

    public class IntegerCodec
    {
        internal IntegerCodec(IInt32Codec codec)
        {
            Codec = codec;
        }

        internal virtual IInt32Codec Codec { get; private set; }

        public override string? ToString()
        {
            return Codec.ToString();
        }
    }
}