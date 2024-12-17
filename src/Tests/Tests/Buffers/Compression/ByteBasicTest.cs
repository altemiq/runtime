namespace Altemiq.Buffers.Compression;

using Xunit.Abstractions;

public class ByteBasicTest
{
    private static readonly ISByteCodec[] codecs = [
        new VariableByte(),
        new Differential.DifferentialVariableByte(),
    ];

    [Theory]
    [MemberData(nameof(Data))]
    public void SaulTest(ByteIntegerCodec codec)
    {
        var C = codec.Codec!;
        for (var x = 0; x < 50 * 4; ++x)
        {
            int[] a = [2, 3, 4, 5];
            var b = new sbyte[90 * 4];
            var c = new int[a.Length];

            var aOffset = 0;
            var bOffset = x;
            C.Compress(a, ref aOffset, b, ref bOffset, a.Length);
            var len = bOffset - x;

            bOffset = x;
            var cOffset = 0;
            C.Decompress(b, ref bOffset, c, ref cOffset, len);
            _ = a.Should().HaveSameElementsAs(c);
        }
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void VaryingLengthTest(ByteIntegerCodec codec)
    {
        const int N = 4096;
        var data = System.Linq.Enumerable.Range(0, N).ToArray();

        var c = codec.Codec!;
        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void VaryingLengthTest2(ByteIntegerCodec codec)
    {
        const int N = 128;
        var data = new int[N];
        data[127] = -1;
        var c = codec.Codec!;

        for (var l = 1; l <= 128; l++)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }

        for (var l = 128; l <= N; l *= 2)
        {
            var comp = TestUtils.Compress(c, TestUtils.CopyArray(data, l));
            var answer = TestUtils.Uncompress(c, comp, l);
            _ = answer.Should().HaveSameElementsAs(data.Take(l));
        }
    }

    public static TheoryData<ByteIntegerCodec> Data()
    {
        return new(codecs.Select(c => new ByteIntegerCodec(c)));
    }

    public class ByteIntegerCodec : IXunitSerializable
    {
        public ByteIntegerCodec()
            : this(default)
        {
        }

        internal ByteIntegerCodec(ISByteCodec? codec)
        {
            Codec = codec;
        }

        internal ISByteCodec? Codec { get; private set; }

        public void Deserialize(IXunitSerializationInfo info)
        {
            if (info.GetValue<Type>(nameof(Codec)) is { } codecType)
            {
                Codec = Activator.CreateInstance(codecType) as ISByteCodec;
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