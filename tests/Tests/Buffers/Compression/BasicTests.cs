using System.Buffers;

namespace Altemiq.Buffers.Compression;

public class BasicTests
{
    private static readonly IEnumerable<Func<IInt32Codec>> Codecs =
    [
        Differential.DifferentialComposition.Create<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>,
        () => new Copy(),
        () => new VariableByte(),
        () => new Differential.DifferentialVariableByte(),
        Composition.Create<BinaryPacking, VariableByte>,
        Composition.Create<NewPfdS9, VariableByte>,
        Composition.Create<NewPfdS16, VariableByte>,
        Composition.Create<OptPfdS9, VariableByte>,
        Composition.Create<OptPfdS16, VariableByte>,
        Composition.Create<FastPatchingFrameOfReference128, VariableByte>,
        Composition.Create<FastPatchingFrameOfReference256, VariableByte>,
        () => new Simple9(),
        () => new Simple16(),
        Composition.Create<Differential.XorBinaryPacking, VariableByte>,
        Composition.Create<DeltaZigzagBinaryPacking, DeltaZigzagVariableByte>
    ];

    [Test]
    [MethodDataSource(nameof(IntegerCodecs))]
    public async Task SaulTest(IntegerCodec codec)
    {
        var ic = codec.Codec;

        for (var x = 0; x < 50; x++)
        {
            int[] data = [2, 3, 4, 5];
            var compressed = new int[90];
            var decompressed = new int[data.Length];

            var dataOffset = 0;
            var compressedOffset = x;
            ic.Compress(data, ref dataOffset, compressed, ref compressedOffset, data.Length);
            var length = compressedOffset - x;

            compressedOffset = x;
            var decompressedOffset = 0;
            ic.Decompress(compressed, ref compressedOffset, decompressed, ref decompressedOffset, length);
            await Assert.That(decompressed).HasSameSequenceAs(data);
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

        var c = codec.Codec;

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            for (var k = 0; k < l; ++k)
            {
                await Assert.That(answer[k]).IsEqualTo(data[k]);
            }
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            for (var k = 0; k < l; ++k)
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
        var c = codec.Codec;

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            for (var k = 0; k < l; ++k)
            {
                await Assert.That(answer[k]).IsEqualTo(data[k]);
            }
        }
        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Decompress(c, comp, l);
            for (var k = 0; k < l; ++k)
            {
                await Assert.That(answer[k]).IsEqualTo(data[k]);
            }
        }
    }

    public static IEnumerable<Func<IntegerCodec>> IntegerCodecs()
    {
        return Codecs.Select(c => new Func<IntegerCodec>(() => new(c())));
    }

    public static IEnumerable<Func<IntegerCodec>> ComplexCodecs()
    {
        return Codecs.Select(c => c()).Where(c => c is not Simple9 and not Simple16).Select(c => new Func<IntegerCodec>(() => new(c)));
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
    [MatrixDataSource]
    public async Task CheckDeltaZigzagUnsorted([Matrix(133, 1026, 1333333)] int size)
    {
        IInt32Codec compo = new Composition(new DeltaZigzagBinaryPacking(), new VariableByte());
        await TestUnsorted(compo, size);
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
    [MatrixDataSource]
    public Task CheckXorBinaryPacking2([Matrix(133, 1026, 1333333)] int size)
    {
        return TestUnsorted(new Differential.DifferentialComposition(new Differential.XorBinaryPacking(), new Differential.DifferentialVariableByte()), size);
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

            static IEnumerable<int[][]> GetData(int n, int nbr)
            {
                var cdg = new ClusteredDataGenerator();
                for (var sparsity = 1; sparsity < 31 - nbr; sparsity += 4)
                {
                    var data = new int[n][];
                    var max = 1 << (nbr + 9);
                    for (var k = 0; k < n; ++k)
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

        await Assert.That(data).HasSameSequenceAs(data);
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

        await Assert.That(data).HasSameSequenceAs(data);
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
        const int Times = 1000;
        var r = new Random();
        var data = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = r.Next();
        }

        var compressed = new int[N];
        var uncompressed = new int[N];
        for (var t = 0; t < Times; ++t)
        {
            BitPacking.Pack(data.AsSpan(0), compressed.AsSpan(0), bit);
            BitPacking.Unpack(compressed.AsSpan(0), uncompressed.AsSpan(0), bit);

            // Check assertions.
            MaskArray(data, (1 << bit) - 1);
            await Assert.That(uncompressed).HasSameSequenceAs(data);
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

        static IEnumerable<Func<(IntegerCodec, IntegerCodec, int[][])>> AddData(int n, int nbr)
        {
            for (var sparsity = 1; sparsity < 31 - nbr; sparsity += 4)
            {
                var generatedData = GenerateData(n, nbr, sparsity);
                yield return () => (new(Differential.DifferentialComposition.Create<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()), new(Differential.DifferentialComposition.Create<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>()), generatedData);
                yield return () => (new(new Copy()), new(new Copy()), generatedData);
                yield return () => (new(new VariableByte()), new(new VariableByte()), generatedData);
                yield return () => (new(new Differential.DifferentialVariableByte()), new(new Differential.DifferentialVariableByte()), generatedData);
                yield return () => (new(Composition.Create<BinaryPacking, VariableByte>()), new(Composition.Create<BinaryPacking, VariableByte>()), generatedData);
                yield return () => (new(Composition.Create<NewPfdS9, VariableByte>()), new(Composition.Create<NewPfdS9, VariableByte>()), generatedData);
                yield return () => (new(Composition.Create<NewPfdS16, VariableByte>()), new(Composition.Create<NewPfdS16, VariableByte>()), generatedData);
                yield return () => (new(Composition.Create<OptPfdS9, VariableByte>()), new(Composition.Create<OptPfdS9, VariableByte>()), generatedData);
                yield return () => (new(Composition.Create<OptPfdS16, VariableByte>()), new(Composition.Create<OptPfdS16, VariableByte>()), generatedData);
                yield return () => (new(Composition.Create<FastPatchingFrameOfReference128, VariableByte>()), new(Composition.Create<FastPatchingFrameOfReference128, VariableByte>()), generatedData);
                yield return () => (new(Composition.Create<FastPatchingFrameOfReference256, VariableByte>()), new(Composition.Create<FastPatchingFrameOfReference256, VariableByte>()), generatedData);
                yield return () => (new(new Simple9()), new(new Simple9()), generatedData);
            }

            static int[][] GenerateData(int n, int nbr, int sparsity)
            {
                var cdg = new ClusteredDataGenerator();

                var data = new int[n][];
                var max = 1 << (nbr + sparsity);
                for (var k = 0; k < n; ++k)
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
        return TestZeroInZeroOut(codec.Codec);
    }

    public static IEnumerable<Func<IntegerCodec>> ZeroInZeroOutData()
    {
        yield return () => new(new Differential.DifferentialBinaryPacking());
        yield return () => new(new Differential.DifferentialVariableByte());
        yield return () => new(new BinaryPacking());
        yield return () => new(new NewPfdS9());
        yield return () => new(new NewPfdS16());
        yield return () => new(new OptPfdS9());
        yield return () => new(new OptPfdS16());
        yield return () => new(new FastPatchingFrameOfReference128());
        yield return () => new(new FastPatchingFrameOfReference256());
        yield return () => new(new VariableByte());
        yield return () => new(Composition.Create<Differential.DifferentialBinaryPacking, VariableByte>());
        yield return () => new(Composition.Create<BinaryPacking, VariableByte>());
        yield return () => new(Composition.Create<NewPfdS9, VariableByte>());
        yield return () => new(Composition.Create<NewPfdS16, VariableByte>());
        yield return () => new(Composition.Create<OptPfdS9, VariableByte>());
        yield return () => new(Composition.Create<OptPfdS16, VariableByte>());
        yield return () => new(Composition.Create<FastPatchingFrameOfReference128, VariableByte>());
        yield return () => new(Composition.Create<FastPatchingFrameOfReference256, VariableByte>());
        yield return () => new(Differential.DifferentialComposition.Create<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>());
    }

    private static async Task TestSpurious(IInt32Codec c)
    {
        var x = new int[1024];
        int[] y = [];
        var i0 = 0;
        var i1 = 0;
        for (var length = 0; length < 32; ++length)
        {
            c.Compress(x, ref i0, y, ref i1, length);
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

        int[] output = [];
        var outputPosition = 0;
        c.Decompress(y, ref i1, output, ref outputPosition, 0);
        await Assert.That(outputPosition).IsEqualTo(0);
    }

    private static async Task TestCodec(IInt32Codec c, IInt32Codec co, int[][] data)
    {
        var n = data.Length;
        var maxlength = 0;
        for (var k = 0; k < n; ++k)
        {
            if (data[k].Length > maxlength)
            {
                maxlength = data[k].Length;
            }
        }

        var buffer = new int[maxlength + 1024];

        // 4x + 1024 to account for the possibility of some negative compression.
        var dataOutput = new int[4 * maxlength + 1024];
        for (var k = 0; k < n; ++k)
        {
            var backupData = TestUtils.CopyArray(data[k], data[k].Length);

            var inputPosition = 1;
            var outputPosition = 0;

            if (c is not Differential.IDifferentialInt32Codec)
            {
                Differential.Delta.Forward(backupData);
            }

            c.Compress(backupData, ref inputPosition, dataOutput, ref outputPosition, backupData.Length - inputPosition);

            var size = outputPosition + 1;
            inputPosition = 0;
            outputPosition = 1;
            buffer[0] = backupData[0];
            co.Decompress(dataOutput, ref inputPosition, buffer, ref outputPosition, size - 1);

            if (c is not Differential.IDifferentialInt32Codec)
            {
                Differential.Delta.Inverse(buffer.AsSpan(0, outputPosition));
            }

            // Check assertions.
            await Assert.That(data[k]).HasCount().EqualTo(outputPosition);
            await Assert.That(data[k]).HasSameSequenceAs(TestUtils.CopyArray(buffer, outputPosition));
        }
    }

    [Test]
    [MethodDataSource(nameof(ExampleCodecsWithSizes))]
    public async Task TestUnsortedExample(IntegerCodec codec, int size)
    {
        await TestUnsorted(codec.Codec, size);
    }

    [Test]
    [MethodDataSource(nameof(ExampleCodecs))]
    public async Task TestUnsorted2Example(IntegerCodec codec)
    {
        await TestUnsorted2(codec.Codec);
    }

    [Test]
    [MethodDataSource(nameof(ExampleCodecs))]
    public async Task TestUnsorted3Example(IntegerCodec codec)
    {
        await TestUnsorted3(codec.Codec);
    }

    public static IEnumerable<Func<IntegerCodec>> ExampleCodecs()
    {
        yield return () => new(Differential.DifferentialComposition.Create<Differential.DifferentialBinaryPacking, Differential.DifferentialVariableByte>());
        yield return () => new(Composition.Create<Differential.DifferentialBinaryPacking, VariableByte>());
        yield return () => new(new VariableByte());
        yield return () => new(new Differential.DifferentialVariableByte());
        yield return () => new(Composition.Create<BinaryPacking, VariableByte>());
        yield return () => new(Composition.Create<NewPfdS9, VariableByte>());
        yield return () => new(Composition.Create<NewPfdS16, VariableByte>());
        yield return () => new(Composition.Create<OptPfdS9, VariableByte>());
        yield return () => new(Composition.Create<OptPfdS16, VariableByte>());
        yield return () => new(Composition.Create<FastPatchingFrameOfReference128, VariableByte>());
        yield return () => new(Composition.Create<FastPatchingFrameOfReference256, VariableByte>());
    }

    public static IEnumerable<Func<(IntegerCodec, int)>> ExampleCodecsWithSizes()
    {
        return ExampleCodecs().SelectMany(Create);

        static IEnumerable<Func<(IntegerCodec, int)>> Create(Func<IntegerCodec> codec)
        {
            yield return () => (codec(), 133);
            yield return () => (codec(), 1026);
            yield return () => (codec(), 1333333);
        }

    }

    private static async Task TestUnsorted(IInt32Codec codec, int n)
    {
        var data = new int[n];
        // initialize the data (most will be small)
        for (var k = 0; k < n; k += 1)
        {
            data[k] = 3;
        }

        // throw some larger values
        for (var k = 0; k < n; k += 5)
        {
            data[k] = 100;
        }

        for (var k = 0; k < n; k += 533)
        {
            data[k] = 10000;
        }

        data[5] = -311;
        // could need more compressing
        var compressed = new int[(int)Math.Ceiling(n * 1.01) + 1024];
        var inputOffset = 0;
        var outputOffset = 0;
        codec.Compress(data, ref inputOffset, compressed, ref outputOffset, data.Length);
        // we can repack the data: (optional)
        compressed = TestUtils.CopyArray(compressed, outputOffset);

        var decompressed = new int[n];
        var decompressedOffset = 0;
        var compressedPosition = 0;
        codec.Decompress(compressed, ref compressedPosition, decompressed, ref decompressedOffset, compressed.Length);
        await Assert.That(decompressed).HasSameSequenceAs(data);
    }

    private static async Task TestUnsorted2(IInt32Codec codec)
    {
        var data = new int[128];
        data[5] = -1;
        var compressed = new int[1024];
        var inputOffset = 0;
        var outputOffset = 0;
        codec.Compress(data, ref inputOffset, compressed, ref outputOffset, data.Length);
        // we can repack the data: (optional)
        compressed = TestUtils.CopyArray(compressed, outputOffset);

        var decompressed = new int[128];
        var decompressedPosition = 0;
        var compressedPosition = 0;
        codec.Decompress(compressed, ref compressedPosition, decompressed, ref decompressedPosition, compressed.Length);
        await Assert.That(data).HasSameSequenceAs(data);
    }

    private static async Task TestUnsorted3(IInt32Codec codec)
    {
        var data = new int[128];
        data[127] = -1;
        var compressed = new int[1024];
        var inputOffset = 0;
        var outputOffset = 0;
        codec.Compress(data, ref inputOffset, compressed, ref outputOffset, data.Length);
        // we can repack the data: (optional)
        compressed = TestUtils.CopyArray(compressed, outputOffset);

        var recovered = new int[128];
        var decompressedPosition = 0;
        var compressedPosition = 0;
        codec.Decompress(compressed, ref compressedPosition, recovered, ref decompressedPosition, compressed.Length);
        await Assert.That(data).HasSameSequenceAs(data);
    }

    [Test]
    public async Task FastPFor()
    {
        // proposed by Stefan Ackermann (https://github.com/Stivo)
        var codec1 = new FastPatchingFrameOfReference256();
        var codec2 = new FastPatchingFrameOfReference256();
        const int N = FastPatchingFrameOfReference256.BlockSize;
        var data = new int[N];
        data[126] = -1;
        var comp = TestUtils.Compress(codec1, TestUtils.CopyArray(data, N));
        await Assert.That(TestUtils.Decompress(codec2, comp, N)).HasSameSequenceAs(data);
    }

    [Test]
    public async Task FastPFor128()
    {
        // proposed by Stefan Ackermann (https://github.com/Stivo)
        var codec1 = new FastPatchingFrameOfReference128();
        var codec2 = new FastPatchingFrameOfReference128();
        const int N = FastPatchingFrameOfReference128.BlockSize;
        var data = new int[N];
        data[126] = -1;
        var comp = TestUtils.Compress(codec1, TestUtils.CopyArray(data, N));
        await Assert.That(TestUtils.Decompress(codec2, comp, N)).HasSameSequenceAs(data);
    }

    public sealed class IntegerCodec
    {
        internal IntegerCodec(IInt32Codec codec)
        {
            Codec = codec;
        }

        internal IInt32Codec Codec { get; private set; }

        public override string? ToString()
        {
            return Codec.ToString();
        }
    }
}