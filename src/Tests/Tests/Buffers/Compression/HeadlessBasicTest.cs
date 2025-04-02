namespace Altemiq.Buffers.Compression;

public class HeadlessBasicTest
{
    private static readonly IEnumerable<Func<IHeadlessInt32Codec>> codecs =
        [
            () => new Copy(),
            () => new VariableByte(),
            () => new HeadlessComposition<BinaryPacking, VariableByte>(),
            () => new HeadlessComposition<NewPfdS9, VariableByte>(),
            () => new HeadlessComposition<NewPfdS16, VariableByte>(),
            () => new HeadlessComposition<OptPfdS9, VariableByte>(),
            () => new HeadlessComposition<OptPfdS16, VariableByte>(),
            () => new HeadlessComposition<FastPatchingFrameOfReference128, VariableByte>(),
            () => new HeadlessComposition<FastPatchingFrameOfReference256, VariableByte>(),
            () => new Simple9(),
            () => new Simple16()
        ];

    public static IEnumerable<Func<SkippableIntegerCodec>> Data()
    {
        return codecs.Select(c => new Func<SkippableIntegerCodec>(() => new SkippableIntegerCodec { Codec = c() }));
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task ConsistentTest(SkippableIntegerCodec codec)
    {
        const int N = 4096;
        var data = new int[N];
        var rev = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = k % 128;
        }

        var c = codec.Codec;
        var outBuf = new int[N + 1024];
        for (var n = 0; n <= N; ++n)
        {
            var inPos = 0;
            var outPos = 0;
            c.Compress(data, ref inPos, outBuf, ref outPos, n);

            var inPoso = 0;
            var outPoso = 0;
            c.Decompress(outBuf, ref inPoso, rev, ref outPoso, outPos, n);
            await Assert.That(outPoso).IsEqualTo(n);
            await Assert.That(inPoso).IsEqualTo(outPos);
            await Assert.That(rev.Take(n)).IsEquivalentTo(data.Take(n));
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest(SkippableIntegerCodec codec)
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
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.UncompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.UncompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
    }

    [Test]
    [MethodDataSource(nameof(Data))]
    public async Task VaryingLengthTest2(SkippableIntegerCodec codec)
    {
        const int N = 128;
        var data = new int[N];
        data[127] = -1;
        var c = codec.Codec;

        if (c is Simple9 or Simple16)
        {
            return;
        }

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.UncompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = await TestUtils.UncompressHeadless(c, comp, l);
            await Assert.That(answer).IsEquivalentTo(data.Take(l));
        }
    }

    public class SkippableIntegerCodec
    {
        internal IHeadlessInt32Codec Codec { get; init; } = null!;

        public override string? ToString()
        {
            return Codec.ToString();
        }
    }
}