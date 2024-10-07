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
        stream.Invoking(static s => s.SwitchTo("first")).Should().NotThrow();
        stream.Invoking(static s => s.SwitchTo("second")).Should().NotThrow();

        dictionary.Values.Should()
            .HaveCount(2)
            .And.AllBeOfType<MemoryStream>();
    }
}