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
    public void Default() => NanoId.Generate().Should().HaveLength(DefaultSize);

    [Fact]
    public async Task DefaultAsync() => (await NanoId.GenerateAsync()).Should().HaveLength(DefaultSize);

    [Fact]
    public void CustomSize()
    {
        const int Size = 10;
        _ = NanoId.Generate(size: Size).Should().HaveLength(Size);
    }

    [Fact]
    public async Task CustomSizeAsync()
    {
        const int Size = 10;
        _ = (await NanoId.GenerateAsync(size: Size)).Should().HaveLength(Size);
    }

    [Fact]
    public void CustomAlphabet() => NanoId.Generate("1234abcd").Should().HaveLength(DefaultSize);

    [Fact]
    public async Task CustomAlphabetAsync() => (await NanoId.GenerateAsync("1234abcd")).Should().HaveLength(DefaultSize);

    [Fact]
    public void CustomAlphabetAndSize()
    {
        const int Size = 7;
        _ = NanoId.Generate("1234abcd", Size).Should().HaveLength(Size);
    }

    [Fact]
    public async Task CustomAlphabetAndSizeAsync()
    {
        const int Size = 7;
        _ = (await NanoId.GenerateAsync("1234abcd", Size)).Should().HaveLength(Size);
    }

    [Fact]
    public void CustomRandom() => NanoId.Generate(new Random(10)).Should().HaveLength(DefaultSize);

    [Fact]
    public async Task CustomRandomAsync() => (await NanoId.GenerateAsync(new Random(10))).Should().HaveLength(DefaultSize);

    [Fact]
    public void SingleLetterAlphabet() => NanoId.Generate("a", 5).Should().Be("aaaaa");

    [Fact]
    public async Task SingleLetterAlphabetAsync() => (await NanoId.GenerateAsync("a", 5)).Should().Be("aaaaa");

    [Theory]
    [InlineData(4, "adca")]
    [InlineData(18, "cbadcbadcbadcbadcc")]
    public void PredefinedRandomSequence(int size, string expected)
    {
        var random = new PredefinedRandom([2, 255, 3, 7, 7, 7, 7, 7, 0, 1]);
        _ = NanoId.Generate(random, "abcde", size).Should().Be(expected);
    }

    [Theory]
    [InlineData(4, "adca")]
    [InlineData(18, "cbadcbadcbadcbadcc")]
    public async Task PredefinedRandomSequenceAsync(int size, string expected)
    {
        var random = new PredefinedRandom([2, 255, 3, 7, 7, 7, 7, 7, 0, 1]);
        _ = (await NanoId.GenerateAsync(random, "abcde", size)).Should().Be(expected);
    }

    [Fact]
    public void GeneratesUrlFriendlyIDs()
    {
        const int Total = 10;
        var count = Total;
        while (count > 0)
        {
            var result = NanoId.Generate();
            _ = result.Should().HaveLength(DefaultSize);

            foreach (var c in result)
            {
                _ = result.Should().Contain($"{c}");
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
            _ = result.Should().HaveLength(DefaultSize);

            foreach (var c in result)
            {
                _ = result.Should().Contain($"{c}");
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
            _ = dictUsed.TryGetValue(result, out _).Should().BeFalse();
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
            _ = dictUsed.TryGetValue(result, out _).Should().BeFalse();
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
            distribution.Should().BeApproximately(1, 0.05);
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
            distribution.Should().BeApproximately(1, 0.05);
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
            mask1.Should().Be(mask2);
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
