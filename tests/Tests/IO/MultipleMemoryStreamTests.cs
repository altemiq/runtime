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
        await Assert.That(stream.SwitchTo("first")).IsTrue();
        await Assert.That(stream.SwitchTo("second")).IsTrue();
        IEnumerable<Stream> streams = dictionary.Values;
        await Assert.That(streams).All().Satisfy(x => x.IsTypeOf<MemoryStream, Stream>().And.IsTypeOf<Stream, MemoryStream>());
    }
}