// -----------------------------------------------------------------------
// <copyright file="MultipleMemoryStreamTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class MultipleMemoryStreamTests
{
    [Test]
    public async Task CreateMultiple()
    {
        var dictionary = new Dictionary<string, Stream>();
        var stream = new MultipleMemoryStream(dictionary);
        _ = await Assert.That(stream.SwitchTo("first")).IsTrue();
        _ = await Assert.That(stream.SwitchTo("second")).IsTrue();
        _ = await Assert.That(dictionary.Values)
            .All()
            .Satisfy(static x => x.IsTypeOf<MemoryStream>().And.IsTypeOf<Stream>());
    }
}