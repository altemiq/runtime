// -----------------------------------------------------------------------
// <copyright file="MultipleMemoryStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class MultipleMemoryStreamTests
{
    [Fact]
    public void CreateMultiple()
    {
        var dictionary = new Dictionary<string, Stream>();
        var stream = new MultipleMemoryStream(dictionary);
        Assert.True(stream.SwitchTo("first"));
        Assert.True(stream.SwitchTo("second"));

        Assert.Collection(
            dictionary.Values, 
            first => Assert.IsType<MemoryStream>(first),
            second => Assert.IsType<MemoryStream>(second));
    }
}