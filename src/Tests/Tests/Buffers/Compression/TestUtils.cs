namespace Altemiq.Buffers.Compression;

using FluentAssertions.Execution;

internal static class TestUtils
{
    public static void AssertSymmetry(IInt32Codec codec, params int[] orig)
    {
        // There are some cases that compressed array is bigger than original
        // array.  So output array for compress must be larger.
        //
        // Example:
        //  - VariableByte compresses an array like [ -1 ].
        //  - Composition compresses a short array.

        const int EXTEND = 1;

        var compressed = new int[orig.Length + EXTEND];
        var c_inpos = 0;
        var c_outpos = 0;
        codec.Compress(orig, ref c_inpos, compressed, ref c_outpos, orig.Length);

        _ = c_outpos.Should().BeLessThanOrEqualTo(orig.Length + EXTEND);

        // Uncompress an array.
        var uncompressed = new int[orig.Length];
        var u_inpos = 0;
        var u_outpos = 0;
        codec.Decompress(compressed, ref u_inpos, uncompressed, ref u_outpos, c_outpos);

        // Compare between uncompressed and orig arrays.
        System.Array.Resize(ref uncompressed, u_outpos);
        _ = uncompressed.Should().HaveSameElementsAs(orig);
    }

    public static int[] Compress(IInt32Codec codec, int[] data)
    {
        var outBuf = new int[data.Length * 4];
        var inPos = 0;
        var outPos = 0;
        codec.Compress(data, ref inPos, outBuf, ref outPos, data.Length);
        System.Array.Resize(ref outBuf, outPos);
        return outBuf;
    }

    public static int[] Uncompress(IInt32Codec codec, int[] data, int len)
    {
        var outBuf = new int[len + 1024];
        var inPos = 0;
        var outPos = 0;
        codec.Decompress(data, ref inPos, outBuf, ref outPos, data.Length);
        System.Array.Resize(ref outBuf, outPos);
        return outBuf;
    }

    public static sbyte[] Compress(ISByteCodec codec, int[] data)
    {
        var outBuf = new sbyte[data.Length * 4 * 4];
        var inPos = 0;
        var outPos = 0;
        codec.Compress(data, ref inPos, outBuf, ref outPos, data.Length);
        System.Array.Resize(ref outBuf, outPos);
        return outBuf;
    }

    public static int[] Uncompress(ISByteCodec codec, sbyte[] data, int len)
    {
        var outBuf = new int[len + 1024];
        var inPos = 0;
        var outPos = 0;
        codec.Decompress(data, ref inPos, outBuf, ref outPos, data.Length);
        System.Array.Resize(ref outBuf, outPos);
        return outBuf;
    }

    public static int[] CompressHeadless(IHeadlessInt32Codec codec, int[] data)
    {
        var outBuf = new int[data.Length * 4];
        var inPos = 0;
        var outPos = 0;
        codec.Compress(data, ref inPos, outBuf, ref outPos, data.Length);
        System.Array.Resize(ref outBuf, outPos);
        return outBuf;
    }

    public static int[] UncompressHeadless(IHeadlessInt32Codec codec, int[] data, int len)
    {
        var outBuf = new int[len + 1024];
        var inPos = 0;
        var outPos = 0;
        codec.Decompress(data, ref inPos, outBuf, ref outPos, data.Length, len);
        _ = outPos.Should().BeGreaterThanOrEqualTo(len);
        System.Array.Resize(ref outBuf, outPos);
        return outBuf;
    }

    public static T[] CopyArray<T>(T[] array, int size)
    {
        var copy = new T[size];
        System.Array.Copy(array, 0, copy, 0, Math.Min(array.Length, size));
        return copy;
    }

    public static string ArrayToString<T>(T[] array)
    {
        return string.Join(", ", array);
    }

    public static AndConstraint<FluentAssertions.Collections.GenericCollectionAssertions<T>> HaveSameElementsAs<T>(this FluentAssertions.Collections.GenericCollectionAssertions<T> source, IEnumerable<T> expected)
        where T : notnull
    {
        var sourceEnumerator = source.Subject.GetEnumerator();
        var expectedEnumerator = expected.GetEnumerator();

        var index = 0;
        while (sourceEnumerator.MoveNext() && expectedEnumerator.MoveNext())
        {
            sourceEnumerator.Current
                .Should()
                .Be(expectedEnumerator.Current, "Expected {context:collection} to contain {0} at index {1}{reason}, but found {2}.", expectedEnumerator.Current, index, source.Subject);
            index++;
        }

        sourceEnumerator.MoveNext().Should().BeFalse("Expected {context:collection} to have the same number of items as {0}{reason}.", expectedEnumerator);
        expectedEnumerator.MoveNext().Should().BeFalse("Expected {context:collection} to have the same number of items as {0}{reason}.", sourceEnumerator);

        return new(source);
    }
}