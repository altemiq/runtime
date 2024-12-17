namespace Altemiq.Buffers.Compression;

public class AdhocTests
{
    [Fact]
    public void BiggerCompressedArray0()
    {
        // No problem: for comparison.
        TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference128(), new VariableByte()), 0, 16384);
        TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference256(), new VariableByte()), 0, 16384);
    }

    [Fact]
    public void BiggerCompressedArray1()
    {
        // Compressed array is bigger than original, because of VariableByte.
        TestUtils.AssertSymmetry(new VariableByte(), -1);
    }

    [Fact]
    public void BiggerCompressedArray2()
    {
        // Compressed array is bigger than original, because of Composition.
        TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference128(), new VariableByte()), 65535, 65535);
        TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference256(), new VariableByte()), 65535, 65535);
    }
}