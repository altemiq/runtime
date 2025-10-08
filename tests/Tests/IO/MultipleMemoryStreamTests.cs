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
        await Assert.That(dictionary.Values).All().Satisfy(x => x.IsTypeOf<MemoryStream>());
    }
}