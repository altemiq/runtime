namespace Altemiq.Buffers.Compression;

using Xunit.Sdk;

public class HeadlessBasicTest(ITestOutputHelper testOutputHelper)
{
    private static readonly IHeadlessInt32Codec[] codecs =
        [
            new Copy(),
            new VariableByte(),
            new HeadlessComposition<BinaryPacking, VariableByte>(),
            new HeadlessComposition<NewPfdS9, VariableByte>(),
            new HeadlessComposition<NewPfdS16, VariableByte>(),
            new HeadlessComposition<OptPfdS9, VariableByte>(),
            new HeadlessComposition<OptPfdS16, VariableByte>(),
            new HeadlessComposition<FastPatchingFrameOfReference128, VariableByte>(),
            new HeadlessComposition<FastPatchingFrameOfReference256, VariableByte>(),
            new Simple9(),
            new Simple16()
        ];

    public static TheoryData<SkippableIntegerCodec> Data()
    {
        return new(codecs.Select(c => new SkippableIntegerCodec(c)));
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void ConsistentTest(SkippableIntegerCodec codec)
    {
        const int N = 4096;
        var data = new int[N];
        var rev = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = k % 128;
        }

        var c = codec.Codec!;
        testOutputHelper.WriteLine("[SkippeableBasicTest.consistentTest] codec = " + c);
        var outBuf = new int[N + 1024];
        for (var n = 0; n <= N; ++n)
        {
            var inPos = 0;
            var outPos = 0;
            c.Compress(data, ref inPos, outBuf, ref outPos, n);

            var inPoso = 0;
            var outPoso = 0;
            c.Decompress(outBuf, ref inPoso, rev, ref outPoso, outPos, n);
            _ = outPoso.Should().Be(n);
            _ = inPoso.Should().Be(outPos);
            _ = rev.Take(n).Should().HaveSameElementsAs(data.Take(n));
        }
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void VaryingLengthTest(SkippableIntegerCodec codec)
    {
        const int N = 4096;
        var data = new int[N];
        for (var k = 0; k < N; ++k)
        {
            data[k] = k;
        }

        var c = codec.Codec!;

        testOutputHelper.WriteLine("[SkippeableBasicTest.varyingLengthTest] codec = " + c);
        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.UncompressHeadless(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }
        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.UncompressHeadless(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void VaryingLengthTest2(SkippableIntegerCodec codec)
    {
        const int N = 128;
        var data = new int[N];
        data[127] = -1;
        var c = codec.Codec!;

        testOutputHelper.WriteLine("[SkippeableBasicTest.varyingLengthTest2] codec = " + c);

        if (c is Simple9)
        {
            return;
        }

        if (c is Simple16)
        {
            return;
        }

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.UncompressHeadless(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }
        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.CompressHeadless(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.UncompressHeadless(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }
    }

    public class SkippableIntegerCodec : IXunitSerializable
    {
        public SkippableIntegerCodec()
            : this(default)
        {
        }

        internal SkippableIntegerCodec(IHeadlessInt32Codec? codec)
        {
            Codec = codec;
        }

        internal IHeadlessInt32Codec? Codec { get; private set; }

        public void Deserialize(IXunitSerializationInfo info)
        {
            if (info.GetValue<Type>(nameof(Codec)) is { } codecType)
            {
                Codec = Activator.CreateInstance(codecType) as IHeadlessInt32Codec;
            }
        }

        public void Serialize(IXunitSerializationInfo info)
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