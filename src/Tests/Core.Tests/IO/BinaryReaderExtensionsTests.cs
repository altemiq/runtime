// -----------------------------------------------------------------------
// <copyright file="BinaryReaderExtensionsTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.IO;

public class BinaryReaderExtensionsTests
{
    [Fact]
    public void ReadString()
    {
        const string value = "This is a string";
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(value));
        stream.Position = 0;

        using var reader = new BinaryReader(stream);
        _ = reader.ReadString(value.Length).Should().Be(value);
    }
}