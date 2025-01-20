namespace Altemiq.Buffers.Compression;

using Xunit.Sdk;

public class Int32CompressorTests
{
    private static readonly IEnumerable<Int32Compressor> ic =
        [
            new Int32Compressor<VariableByte>(),
            new Int32Compressor<HeadlessComposition<BinaryPacking, VariableByte>>()
        ];

    public static TheoryData<IntegerCompressor> Data()
    {
        return new(ic.Select(i => new IntegerCompressor(i)));
    }

    [Theory]
    [MemberData(nameof(Data))]
    public void BasicTest(IntegerCompressor compressor)
    {
        var i = compressor.Compressor!;

        for (var n = 1; n <= 10000; n *= 10)
        {
            var orig = new int[n];
            for (var k = 0; k < n; k++)
            {
                orig[k] = (3 * k) + 5;
            }

            var comp = i.Compress(orig);
            var back = i.Uncompress(comp);

            Assert.Equal(orig, back);
        }
    }

    public class IntegerCompressor : IXunitSerializable
    {
        public IntegerCompressor()
            : this(default)
        {
        }

        internal IntegerCompressor(Int32Compressor? compressor)
        {
            Compressor = compressor;
        }

        internal virtual Int32Compressor? Compressor { get; private set; }

        public virtual void Deserialize(IXunitSerializationInfo info)
        {
            if (info.GetValue<Type>(nameof(Compressor)) is { } compressorType)
            {
                Compressor = Activator.CreateInstance(compressorType) as Int32Compressor;
            }
        }

        public virtual void Serialize(IXunitSerializationInfo info)
        {
            if (Compressor is not null)
            {
                info.AddValue(nameof(Compressor), Compressor.GetType());
            }
        }

        public override string? ToString()
        {
            return Compressor?.ToString() ?? base.ToString();
        }
    }
}