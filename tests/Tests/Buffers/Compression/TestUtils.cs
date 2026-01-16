namespace Altemiq.Buffers.Compression;

internal static class TestUtils
{
    public static async Task AssertSymmetry(IInt32Codec codec, params int[] orig)
    {
        // There are some cases that compressed array is bigger than original
        // array.  So output array for compress must be larger.
        //
        // Example:
        //  - VariableByte compresses an array like [ -1 ].
        //  - Composition compresses a short array.

        const int Extend = 2;

        var compressed = new int[orig.Length + Extend];
        var (_, compressedOutputPosition) = codec.Compress(orig, compressed);

        await Assert.That(compressedOutputPosition).IsBetween(0, orig.Length + Extend);

        // Decompress an array.
        var decompressed = new int[orig.Length];
        var (_, decompressedOutputPosition) = codec.Decompress(compressed.AsSpan(0, compressedOutputPosition), decompressed);

        // Compare between uncompressed and orig arrays.
        Array.Resize(ref decompressed, decompressedOutputPosition);
        await Assert.That(orig).HasSameSequenceAs(decompressed);
    }

    public static int[] Compress(IInt32Codec codec, ReadOnlySpan<int> data)
    {
        var output = new int[data.Length * 4];
        var (_, outputPosition) = codec.Compress(data, output);
        Array.Resize(ref output, outputPosition);
        return output;
    }

    public static int[] Decompress(IInt32Codec codec, ReadOnlySpan<int> data, int length)
    {
        var output = new int[length + 1024];
        var (_, outputPosition) = codec.Decompress(data, output);
        Array.Resize(ref output, outputPosition);
        return output;
    }

    public static sbyte[] Compress(ISByteCodec codec, ReadOnlySpan<int> data)
    {
        var output = new sbyte[data.Length * 4 * 4];
        var (_, outputPosition) = codec.Compress(data, output);
        Array.Resize(ref output, outputPosition);
        return output;
    }

    public static int[] Decompress(ISByteCodec codec, ReadOnlySpan<sbyte> data, int length)
    {
        var output = new int[length + 1024];
        var (_, outputPosition) = codec.Decompress(data, output);
        Array.Resize(ref output, outputPosition);
        return output;
    }

    public static int[] CompressHeadless(IHeadlessInt32Codec codec, ReadOnlySpan<int> data)
    {
        var output = new int[data.Length * 4];
        var (_, outputPosition) = codec.Compress(data, output);
        Array.Resize(ref output, outputPosition);
        return output;
    }

    public static async Task<int[]> DecompressHeadless(IHeadlessInt32Codec codec, int[] data, int length)
    {
        var output = new int[length + 1024];
        var (_, outputPosition) = codec.Decompress(data, output.AsSpan(0, length));
        await Assert.That(outputPosition).IsGreaterThanOrEqualTo(length);
        Array.Resize(ref output, outputPosition);
        return output;
    }

    public static T[] CopyArray<T>(T[] array, int size)
    {
        var copy = new T[size];
        Array.Copy(array, 0, copy, 0, Math.Min(array.Length, size));
        return copy;
    }

    public static string ArrayToString<T>(T[] array)
    {
        return string.Join(", ", array);
    }
}