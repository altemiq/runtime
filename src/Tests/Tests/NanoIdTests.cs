// -----------------------------------------------------------------------
// <copyright file="NanoIds.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class NanoIds
{
    private const int DefaultSize = 21;
    private const string DefaultAlphabet = "_-0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    [Fact]
    public void Default() => Assert.Equal(DefaultSize, NanoId.Generate().Length);

    [Fact]
    public async Task DefaultAsync() => Assert.Equal(DefaultSize, (await NanoId.GenerateAsync()).Length);

    [Fact]
    public void CustomSize()
    {
        const int Size = 10;
        Assert.Equal(Size, NanoId.Generate(size: Size).Length);
    }

    [Fact]
    public async Task CustomSizeAsync()
    {
        const int Size = 10;
        Assert.Equal(Size, (await NanoId.GenerateAsync(size: Size)).Length);
    }

    [Fact]
    public void CustomAlphabet() => Assert.Equal(DefaultSize, NanoId.Generate("1234abcd").Length);

    [Fact]
    public async Task CustomAlphabetAsync() => Assert.Equal(DefaultSize, (await NanoId.GenerateAsync("1234abcd")).Length);

    [Fact]
    public void CustomAlphabetAndSize()
    {
        const int Size = 7;
        Assert.Equal(Size, NanoId.Generate("1234abcd", Size).Length);
    }

    [Fact]
    public async Task CustomAlphabetAndSizeAsync()
    {
        const int Size = 7;
        Assert.Equal(Size, (await NanoId.GenerateAsync("1234abcd", Size)).Length);
    }

    [Fact]
    public void CustomRandom() => Assert.Equal(DefaultSize, NanoId.Generate(new Random(10)).Length);

    [Fact]
    public async Task CustomRandomAsync() => Assert.Equal(DefaultSize, (await NanoId.GenerateAsync(new Random(10))).Length);

    [Fact]
    public void SingleLetterAlphabet() => Assert.Equal("aaaaa", NanoId.Generate("a", 5));

    [Fact]
    public async Task SingleLetterAlphabetAsync() => Assert.Equal("aaaaa", await NanoId.GenerateAsync("a", 5));

    [Theory]
    [InlineData(4, "adca")]
    [InlineData(18, "cbadcbadcbadcbadcc")]
    public void PredefinedRandomSequence(int size, string expected)
    {
        var random = new PredefinedRandom([2, 255, 3, 7, 7, 7, 7, 7, 0, 1]);
        Assert.Equal(expected, NanoId.Generate(random, "abcde", size));
    }

    [Theory]
    [InlineData(4, "adca")]
    [InlineData(18, "cbadcbadcbadcbadcc")]
    public async Task PredefinedRandomSequenceAsync(int size, string expected)
    {
        var random = new PredefinedRandom([2, 255, 3, 7, 7, 7, 7, 7, 0, 1]);
        Assert.Equal(expected, (await NanoId.GenerateAsync(random, "abcde", size)));
    }

    [Fact]
    public void GeneratesUrlFriendlyIDs()
    {
        const int Total = 10;
        var count = Total;
        while (count > 0)
        {
            var result = NanoId.Generate();
            Assert.Equal(DefaultSize, result.Length);

            foreach (var c in result)
            {
                Assert.Contains($"{c}", result);
            }

            count--;
        }
    }

    [Fact]
    public async Task GeneratesUrlFriendlyIDsAsync()
    {
        const int Total = 10;
        var count = Total;
        while (count > 0)
        {
            var result = await NanoId.GenerateAsync();
            Assert.Equal(DefaultSize, result.Length);

            foreach (var c in result)
            {
                Assert.Contains($"{c}", result);
            }

            count--;
        }
    }

    [Fact]
    public void HasNoCollisions()
    {
        const int Total = 100 * 1000;
        var dictUsed = new Dictionary<string, bool>();

        var count = Total;
        while (count > 0)
        {
            var result = NanoId.Generate();
            Assert.False(dictUsed.TryGetValue(result, out _));
            dictUsed.Add(result, true);

            count--;
        }
    }

    [Fact]
    public async Task HasNoCollisionsAsync()
    {
        const int Total = 100 * 1000;
        var dictUsed = new Dictionary<string, bool>();

        var count = Total;
        while (count > 0)
        {
            var result = await NanoId.GenerateAsync();
            Assert.False(dictUsed.TryGetValue(result, out _));
            dictUsed.Add(result, true);

            count--;
        }
    }

    [Fact]
    public void FlatDistribution()
    {
        const int Total = 100 * 1000;
        var chars = new Dictionary<char, int>();

        var count = Total;
        while (count > 0)
        {
            var id = NanoId.Generate();
            for (var i = 0; i < DefaultSize; i++)
            {
                var c = id[i];
#if NETCOREAPP2_0_OR_GREATER
                chars.TryAdd(c, 0);
#else
                if (!chars.ContainsKey(c))
                {
                    chars.Add(c, 0);
                }
#endif
                chars[c] += 1;
            }

            count--;
        }

        foreach (var c in chars)
        {
            var distribution = c.Value * DefaultAlphabet.Length / (double)(Total * DefaultSize);
            Assert.Equal(1, distribution, 0.05);
        }
    }

    [Fact]
    public async Task FlatDistributionAsync()
    {
        const int Total = 100 * 1000;
        var chars = new Dictionary<char, int>();

        var count = Total;
        while (count > 0)
        {
            var id = await NanoId.GenerateAsync();
            for (var i = 0; i < DefaultSize; i++)
            {
                var c = id[i];
#if NETCOREAPP2_0_OR_GREATER
                chars.TryAdd(c, 0);
#else
                if (!chars.ContainsKey(c))
                {
                    chars.Add(c, 0);
                }
#endif
                chars[c] += 1;
            }

            count--;
        }

        foreach (var c in chars)
        {
            var distribution = c.Value * DefaultAlphabet.Length / (double)(Total * DefaultSize);
            Assert.Equal(1, distribution, 0.05);
        }
    }

    [Fact]
    public void Mask()
    {
        for (var length = 1; length < 256; length++)
        {
            var mask1 = (2 << (int)Math.Floor(Math.Log(length - 1) / Math.Log(2))) - 1;
#if NET7_0_OR_GREATER
            var mask2 = (2 << (31 - int.LeadingZeroCount((length - 1) | 1))) - 1;
#else
            var mask2 = (2 << 31 - NanoId.LeadingZeroCount((length - 1) | 1)) - 1;
#endif
            Assert.Equal(mask2, mask1);
        }
    }

    private class PredefinedRandom(byte[] sequence) : Random
    {
        public override void NextBytes(byte[] buffer)
        {
            var seq = this.GetSequence(buffer.Length).GetEnumerator();
            var i = 0;
            while (seq.MoveNext())
            {
                buffer[i++] = seq.Current;
            }
        }

#if NETCOREAPP2_1_OR_GREATER
        public override void NextBytes(Span<byte> buffer)
        {
            var bytes = new byte[buffer.Length];
            this.NextBytes(bytes);
            bytes.CopyTo(buffer);
        }
#endif

        private IEnumerable<byte> GetSequence(int size)
        {
            var result = sequence.AsEnumerable();

            // Update the sequence to match nanoid.js tests and implementation
            // which takes random bytes in reverse order (as of 3489e1e3b0dd7678b72c30f5fb00b806c8ce4fef).
            for (var i = 0; i < (size / sequence.Length); i++)
            {
                result = result.Concat(result);
            }

            return result.Take(size).Reverse();
        }
    }
}