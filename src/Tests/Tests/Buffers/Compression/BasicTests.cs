namespace Altemiq.Buffers.Compression;

using Xunit.Sdk;

public partial class BasicTests(ITestOutputHelper testOutputHelper)
{
    private static readonly IInt32Codec[] codecs =
    [
        new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>(),
        new Copy(),
        new VariableByte(),
        new Differential.DifferentialVariableByte(),
        new Composition<BinaryPacking, VariableByte>(),
        new Composition<NewPfdS9, VariableByte>(),
        new Composition<NewPfdS16, VariableByte>(),
        new Composition<OptPfdS9, VariableByte>(),
        new Composition<OptPfdS16, VariableByte>(),
        new Composition<FastPatchingFrameOfReference128, VariableByte>(),
        new Composition<FastPatchingFrameOfReference256, VariableByte>(),
        new Simple9(),
        new Simple16(),
        new Composition<Differential.XorBinaryPacking, VariableByte>(),
        new Composition<DeltaZigzagBinaryPacking, DeltaZigzagVariableByte>()
    ];

    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    [Theory]
    [MemberData(nameof(IntegerCodecs))]
    public void SaulTest(IntegerCodec codec)
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
            _ = a.Should().HaveSameElementsAs(c);
        }
    }

    [Theory]
    [MemberData(nameof(IntegerCodecs))]
    public void VaryingLengthTest(IntegerCodec codec)
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
                if (answer[k] != data[k])
                {
                    throw new InvalidOperationException("bug");
                }
            }
        }
        for (var L = 128; L <= N; L *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, L));
            var answer = TestUtils.Uncompress(c, comp, L);
            for (var k = 0; k < L; ++k)
            {
                if (answer[k] != data[k])
                {
                    _testOutputHelper.WriteLine(TestUtils.ArrayToString(TestUtils.CopyArray(answer, L)));
                    _testOutputHelper.WriteLine(TestUtils.ArrayToString(TestUtils.CopyArray(data, L)));
                    throw new InvalidOperationException("bug");
                }
            }
        }
    }

    [Theory]
    [MemberData(nameof(ComplexCodecs))]
    public void VaryingLengthTest2(IntegerCodec codec)
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
                if (answer[k] != data[k])
                {
                    throw new Exception("bug");
                }
            }
        }
        for (var L = 128; L <= N; L *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, L));
            var answer = TestUtils.Uncompress(c, comp, L);
            for (var k = 0; k < L; ++k)
            {
                if (answer[k] != data[k])
                {
                    throw new Exception("bug");
                }
            }
        }
    }

    public static TheoryData<IntegerCodec> IntegerCodecs()
    {
        return new(codecs.Select(c => new IntegerCodec(c)));
    }

    public static TheoryData<IntegerCodec> ComplexCodecs()
    {
        return new(codecs.Where(c => c is not Simple9 and not Simple16).Select(c => new IntegerCodec(c)));
    }

    [Fact]
    public void CheckDeltaZigzagZero()
    {
        TestZeroInZeroOut(new DeltaZigzagVariableByte());
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void CheckDeltaZigzagVariableByte(int[][] data)
    {
        TestCodec(new DeltaZigzagVariableByte(), new DeltaZigzagVariableByte(), data);
    }

    [Fact]
    public void CheckDeltaZigzagPacking()
    {
        var codec = new DeltaZigzagBinaryPacking();
        TestZeroInZeroOut(codec);
        TestSpurious(codec);
    }

    [Fact]
    public void CheckDeltaZigzagZeroInZeroOut()
    {
        IInt32Codec compo = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());
        TestZeroInZeroOut(compo);
    }

    [Fact]
    public void CheckDeltaZigzagUnsorted()
    {
        IInt32Codec compo = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());
        TestUnsorted(compo);
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void CheckDeltaZigzagPackingComposition(int[][] data)
    {
        IInt32Codec compo = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());
        IInt32Codec compo2 = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());

        TestCodec(compo, compo2, data);
    }

    [Fact]
    public void CheckXorBinaryPacking()
    {
        TestZeroInZeroOut(new Differential.XorBinaryPacking());
        TestSpurious(new Differential.XorBinaryPacking());
    }

    [Fact]
    public void CheckXorBinaryPacking1()
    {
        TestZeroInZeroOut(new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte()));
    }

    [Fact]
    public void CheckXorBinaryPacking2()
    {
        TestUnsorted(new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte()));
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void CheckXorBinaryPacking3(int[][] data)
    {
        IInt32Codec c = new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte());
        IInt32Codec co = new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte());
        TestCodec(c, co, data);
    }

    public static TheoryData<int[][]> Data()
    {
        return new TheoryData<int[][]>(GetAllData());

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

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(30)]
    public void VerifyBitPacking(int bit)
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

        _ = uncompressed.Should().HaveSameElementsAs(data);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(30)]
    public void VerifyWithoutMask(int bit)
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

        _ = uncompressed.Should().HaveSameElementsAs(data);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(8)]
    [InlineData(16)]
    [InlineData(24)]
    [InlineData(30)]
    public void VerifyWithExceptions(int bit)
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
            _ = uncompressed.Should().HaveSameElementsAs(data);
        }

        static void MaskArray(int[] array, int mask)
        {
            for (int i = 0, end = array.Length; i < end; ++i)
            {
                array[i] &= mask;
            }
        }
    }

    [Theory]
    [MemberData(nameof(BasicTestData))]
    public void BasicTest(IntegerCodec c, IntegerCodec co, int[][] data)
    {
        TestCodec(c.Codec, co.Codec, data);
    }

    public static TheoryData<IntegerCodec, IntegerCodec, int[][]> BasicTestData()
    {
        var data = new TheoryData<IntegerCodec, IntegerCodec, int[][]>();

        _ = AddData(data, 5, 10);
        _ = AddData(data, 5, 14);
        _ = AddData(data, 2, 18);

        return data;

        static TheoryData<IntegerCodec, IntegerCodec, int[][]> AddData(TheoryData<IntegerCodec, IntegerCodec, int[][]> data, int N, int nbr)
        {
            for (var sparsity = 1; sparsity < 31 - nbr; sparsity += 4)
            {
                var generatedData = GenerateData(N, nbr, sparsity);
                data.Add(new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()), new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Copy()), new IntegerCodec(new Copy()), generatedData);
                data.Add(new IntegerCodec(new VariableByte()), new IntegerCodec(new VariableByte()), generatedData);
                data.Add(new IntegerCodec(new Differential.DifferentialVariableByte()), new IntegerCodec(new Differential.DifferentialVariableByte()), generatedData);
                data.Add(new IntegerCodec(new Composition<BinaryPacking, VariableByte>()), new IntegerCodec(new Composition<BinaryPacking, VariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Composition<NewPfdS9, VariableByte>()), new IntegerCodec(new Composition<NewPfdS9, VariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Composition<NewPfdS16, VariableByte>()), new IntegerCodec(new Composition<NewPfdS16, VariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Composition<OptPfdS9, VariableByte>()), new IntegerCodec(new Composition<OptPfdS9, VariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Composition<OptPfdS16, VariableByte>()), new IntegerCodec(new Composition<OptPfdS16, VariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>()), new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>()), new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>()), generatedData);
                data.Add(new IntegerCodec(new Simple9()), new IntegerCodec(new Simple9()), generatedData);
            }

            return data;

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

    [Theory]
    [InlineData(typeof(Differential.DifferentialBinaryPacking))]
    [InlineData(typeof(BinaryPacking))]
    [InlineData(typeof(NewPfdS9))]
    [InlineData(typeof(NewPfdS16))]
    [InlineData(typeof(OptPfdS9))]
    [InlineData(typeof(OptPfdS16))]
    [InlineData(typeof(FastPatchingFrameOfReference128))]
    [InlineData(typeof(FastPatchingFrameOfReference256))]
    public void SpuriousOut(Type type)
    {
        var codec = Activator.CreateInstance(type) as IInt32Codec;
        _ = codec.Should().NotBeNull();
        TestSpurious(codec!);
    }

    [Theory]
    [MemberData(nameof(ZeroInZeroOutData))]
    public void ZeroInZeroOut(IntegerCodec codec)
    {
        TestZeroInZeroOut(codec.Codec!);
    }

    public static TheoryData<IntegerCodec> ZeroInZeroOutData()
    {
        return [
            new IntegerCodec(new Differential.DifferentialBinaryPacking()),
            new IntegerCodec(new Differential.DifferentialVariableByte()),
            new IntegerCodec(new BinaryPacking()),
            new IntegerCodec(new NewPfdS9()),
            new IntegerCodec(new NewPfdS16()),
            new IntegerCodec(new OptPfdS9()),
            new IntegerCodec(new OptPfdS16()),
            new IntegerCodec(new FastPatchingFrameOfReference128()),
            new IntegerCodec(new FastPatchingFrameOfReference256()),
            new IntegerCodec(new VariableByte()),
            new IntegerCodec(new Composition<Differential.DifferentialBinaryPacking, VariableByte>()),
            new IntegerCodec(new Composition<BinaryPacking, VariableByte>()),
            new IntegerCodec(new Composition<NewPfdS9, VariableByte>()),
            new IntegerCodec(new Composition<NewPfdS16, VariableByte>()),
            new IntegerCodec(new Composition<OptPfdS9, VariableByte>()),
            new IntegerCodec(new Composition<OptPfdS16, VariableByte>()),
            new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>()),
            new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>()),
            new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()),
        ];
    }

    private static void TestSpurious(IInt32Codec c)
    {
        var x = new int[1024];
        int[] y = [];
        var i0 = 0;
        var i1 = 0;
        for (var inlength = 0; inlength < 32; ++inlength)
        {
            c.Compress(x, ref i0, y, ref i1, inlength);
            _ = i1.Should().Be(0);
        }
    }

    private static void TestZeroInZeroOut(IInt32Codec c)
    {
        int[] x = [];
        int[] y = [];
        var i0 = 0;
        var i1 = 0;
        c.Compress(x, ref i0, y, ref i1, 0);
        _ = i1.Should().Be(0);

        int[] @out = [];
        var outpos = 0;
        c.Decompress(y, ref i1, @out, ref outpos, 0);
        _ = outpos.Should().Be(0);
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

    private static void TestCodec(IInt32Codec? c, IInt32Codec? co, int[][] data)
    {
        _ = c.Should().NotBeNull();
        _ = co.Should().NotBeNull();

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
            _ = outpos.Should().Be(data[k].Length);
            var bufferCutout = TestUtils.CopyArray(buffer, outpos);
            _ = bufferCutout.Should().HaveSameElementsAs(data[k]);
        }
    }

    [Theory]
    [MemberData(nameof(ExampleCodecs))]
    public void TestUnsortedExample(IntegerCodec codec)
    {
        var c = codec.Codec!;
        TestUnsorted(c);
        TestUnsorted2(c);
        TestUnsorted3(c);
    }

    public static TheoryData<IntegerCodec> ExampleCodecs()
    {
        return [
            new IntegerCodec(new Differential.DifferentialComposition<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()),
            new IntegerCodec(new Composition<Differential.DifferentialBinaryPacking, VariableByte>()),
            new IntegerCodec(new VariableByte()),
            new IntegerCodec(new Differential.DifferentialVariableByte()),
            new IntegerCodec(new Composition<BinaryPacking, VariableByte>()),
            new IntegerCodec(new Composition<NewPfdS9, VariableByte>()),
            new IntegerCodec(new Composition<NewPfdS16, VariableByte>()),
            new IntegerCodec(new Composition<OptPfdS9, VariableByte>()),
            new IntegerCodec(new Composition<OptPfdS16, VariableByte>()),
            new IntegerCodec(new Composition<FastPatchingFrameOfReference128, VariableByte>()),
            new IntegerCodec(new Composition<FastPatchingFrameOfReference256, VariableByte>()),
        ];
    }

    private static void TestUnsorted(IInt32Codec codec)
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
            _ = recovered.Should().HaveSameElementsAs(data);
        }
    }

    private static void TestUnsorted2(IInt32Codec codec)
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
        _ = recovered.Should().HaveSameElementsAs(data);
    }

    private static void TestUnsorted3(IInt32Codec codec)
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
        _ = recovered.Should().HaveSameElementsAs(data);
    }

    [Fact]
    public void FastPFor()
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
        _ = TestUtils.Uncompress(codec2, comp, N).Should().HaveSameElementsAs(data);
    }

    [Fact]
    public void FastPFor128()
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
        _ = TestUtils.Uncompress(codec2, comp, N).Should().HaveSameElementsAs(data);
    }

    public class IntegerCodec : IXunitSerializable
    {
        public IntegerCodec()
            : this(default)
        {
        }

        internal IntegerCodec(IInt32Codec? codec)
        {
            Codec = codec;
        }

        internal virtual IInt32Codec? Codec { get; private set; }

        public virtual void Deserialize(IXunitSerializationInfo info)
        {
            if (info.GetValue<Type>(nameof(Codec)) is { } codecType)
            {
                Codec = Activator.CreateInstance(codecType) as IInt32Codec;
            }
        }

        public virtual void Serialize(IXunitSerializationInfo info)
        {
            if (Codec is not null)
            {
                info.AddValue(nameof(Codec), Codec.GetType());
            }
        }

        public override string? ToString()
        {
            return Codec?.ToString() ?? base.ToString();
        }
    }
}