namespace Altemiq.Buffers.Compression;

public class HeadlessBasicTest
{
    private static readonly IEnumerable<Func<IHeadlessInt32Codec>> Codecs =
        [
            () => new Copy(),
            () => new VariableByte(),
            HeadlessComposition.Create<BinaryPacking, VariableByte>,
            HeadlessComposition.Create<NewPfdS9, VariableByte>,
            HeadlessComposition.Create<NewPfdS16, VariableByte>,
            HeadlessComposition.Create<OptPfdS9, VariableByte>,
            HeadlessComposition.Create<OptPfdS16, VariableByte>,
            HeadlessComposition.Create<FastPatchingFrameOfReference128, VariableByte>,
            HeadlessComposition.Create<FastPatchingFrameOfReference256, VariableByte>,
            () => new Simple9(),
            () => new Simple16()
        ];

    public static IEnumerable<Func<HeadlessIntegerCodec>> Data()
    {
        return Codecs.Select(c => new Func<HeadlessIntegerCodec>(() => new() { Codec = c() }));
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task ConsistentTest(HeadlessIntegerCodec codec)
    {
        const int Size = 4096;
        var data = new int[Size];
        var rev = new int[Size];
        for (var k = 0; k < Size; ++k)
        {
            data[k] = k % 128;
        }

        var c = codec.Codec;
        var outBuf = new int[Size + 1024];
        for (var n = 0; n <= Size; ++n)
        {
            var inputPosition = 0;
            var outputPosition = 0;
            c.Compress(data, ref inputPosition, outBuf, ref outputPosition, n);

            var decompressedInputPosition = 0;
            var decompressedOutputPosition = 0;
            c.Decompress(outBuf, ref decompressedInputPosition, rev, ref decompressedOutputPosition, outputPosition, n);
            await Assert.That(decompressedOutputPosition).IsEqualTo(n);
            await Assert.That(decompressedInputPosition).IsEqualTo(outputPosition);
            await Assert.That(rev.Take(n)).IsEquivalentTo(data.Take(n));
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest(HeadlessIntegerCodec codec)
    {
        const int Size = 4096;
        var data = new int[Size];
        for (var k = 0; k < Size; ++k)
        {
            data[k] = k;
        }

        var c = codec.Codec;

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.DecompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
        for (var l = 128; l <= Size; l *= 2)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.DecompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest2(HeadlessIntegerCodec codec)
    {
        var c = codec.Codec;
        if (c is Simple9 or Simple16)
        {
            return;
        }

        const int Size = 128;
        var data = new int[Size];
        data[127] = -1;

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.DecompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }

        for (var l = 128; l <= Size; l *= 2)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.DecompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
    }

    public class HeadlessIntegerCodec
    {
        internal IHeadlessInt32Codec Codec { get; init; } = null!;

        public override string? ToString()
        {
            return Codec.ToString();
        }
    }
}