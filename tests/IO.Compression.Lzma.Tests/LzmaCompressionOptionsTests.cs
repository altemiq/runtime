// -----------------------------------------------------------------------
// <copyright file="LzmaCompressionOptionsTests.cs" company="KingR">
// Copyright (c) KingR. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression.Tests;

public class LzmaCompressionOptionsTests
{
    [Test]
    public async Task FromDefaultOptions()
    {
        await Assert.That(LzmaEncoderTests.GetDefaultProperties()).IsEquivalentTo(new LzmaCompressionOptions().ToDictionary());
    }
}