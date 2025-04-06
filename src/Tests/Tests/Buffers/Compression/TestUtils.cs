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
        var compressedInputPosition = 0;
        var compressedOutputPosition = 0;
        codec.Compress(orig, ref compressedInputPosition, compressed, ref compressedOutputPosition, orig.Length);

        await Assert.That(compressedOutputPosition).IsBetween(0, orig.Length + Extend);

        // Decompress an array.
        var decompressed = new int[orig.Length];
        var decompressedInputPosition = 0;
        var decompressedOutputPosition = 0;
        codec.Decompress(compressed, ref decompressedInputPosition, decompressed, ref decompressedOutputPosition, compressedOutputPosition);

        // Compare between uncompressed and orig arrays.
        System.Array.Resize(ref decompressed, decompressedOutputPosition);
        await Assert.That(orig).IsEquivalentTo(decompressed);
    }

    public static int[] Compress(IInt32Codec codec, int[] data)
    {
        var output = new int[data.Length * 4];
        var inputPosition = 0;
        var outputPosition = 0;
        codec.Compress(data, ref inputPosition, output, ref outputPosition, data.Length);
        System.Array.Resize(ref output, outputPosition);
        return output;
    }

    public static int[] Decompress(IInt32Codec codec, int[] data, int len)
    {
        var output = new int[len + 1024];
        var inputPosition = 0;
        var outputPosition = 0;
        codec.Decompress(data, ref inputPosition, output, ref outputPosition, data.Length);
        System.Array.Resize(ref output, outputPosition);
        return output;
    }

    public static sbyte[] Compress(ISByteCodec codec, int[] data)
    {
        var output = new sbyte[data.Length * 4 * 4];
        var inputPosition = 0;
        var outputPosition = 0;
        codec.Compress(data, ref inputPosition, output, ref outputPosition, data.Length);
        System.Array.Resize(ref output, outputPosition);
        return output;
    }

    public static int[] Decompress(ISByteCodec codec, sbyte[] data, int len)
    {
        var output = new int[len + 1024];
        var inputPosition = 0;
        var outputPosition = 0;
        codec.Decompress(data, ref inputPosition, output, ref outputPosition, data.Length);
        System.Array.Resize(ref output, outputPosition);
        return output;
    }

    public static int[] CompressHeadless(IHeadlessInt32Codec codec, int[] data)
    {
        var output = new int[data.Length * 4];
        var inputPosition = 0;
        var outputPosition = 0;
        codec.Compress(data, ref inputPosition, output, ref outputPosition, data.Length);
        System.Array.Resize(ref output, outputPosition);
        return output;
    }

    public static async Task<int[]> DecompressHeadless(IHeadlessInt32Codec codec, int[] data, int length)
    {
        var output = new int[length + 1024];
        var inputPosition = 0;
        var outputPosition = 0;
        codec.Decompress(data, ref inputPosition, output, ref outputPosition, data.Length, length);
        await Assert.That(outputPosition).IsGreaterThanOrEqualTo(length);
        System.Array.Resize(ref output, outputPosition);
        return output;
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
}