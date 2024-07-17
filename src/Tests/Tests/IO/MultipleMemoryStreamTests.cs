// -----------------------------------------------------------------------
// <copyright file="MultipleMemoryStreamTests.cs" company="Altavec">
// Copyright (c) Altavec. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altavec.IO;

public class MultipleMemoryStreamTests
{
    [Fact]
    public void CreateMultiple()
    {
        var dictionary = new Dictionary<string, Stream>();
        var stream = new MultipleMemoryStream(dictionary);
        stream.Invoking(s => s.SwitchTo("first")).Should().NotThrow();
        stream.Invoking(s => s.SwitchTo("second")).Should().NotThrow();

        dictionary.Values.Should()
            .HaveCount(2)
            .And.AllBeOfType<MemoryStream>();
    }
}
