namespace Altemiq.Buffers.Compression;

public class AdhocTests
{
    [Test]
    public async Task BiggerCompressedArray0()
    {
        // No problem: for comparison.
        await TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference128(), new VariableByte()), 0, 16384);
        await TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference256(), new VariableByte()), 0, 16384);
    }

    [Test]
    public async Task BiggerCompressedArray1()
    {
        // Compressed array is bigger than original, because of VariableByte.
        await TestUtils.AssertSymmetry(new VariableByte(), -1);
    }

    [Test]
    public async Task BiggerCompressedArray2()
    {
        // Compressed array is bigger than original, because of Composition.
        await TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference128(), new VariableByte()), 65535, 65535);
        await TestUtils.AssertSymmetry(new Composition(new FastPatchingFrameOfReference256(), new VariableByte()), 65535, 65535);
    }
}