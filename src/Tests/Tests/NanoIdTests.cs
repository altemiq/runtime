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

    [Test]
    public async Task Default() => await Assert.That(NanoId.Generate()).HasCount().EqualTo(DefaultSize);

    [Test]
    public async Task DefaultAsync() => await Assert.That(await NanoId.GenerateAsync()).HasCount().EqualTo(DefaultSize);

    [Test]
    public async Task CustomSize()
    {
        const int Size = 10;
        await Assert.That(NanoId.Generate(size: Size)).HasCount().EqualTo(Size);
    }

    [Test]
    public async Task CustomSizeAsync()
    {
        const int Size = 10;
        await Assert.That(await NanoId.GenerateAsync(size: Size)).HasCount().EqualTo(Size);
    }

    [Test]
    public async Task CustomAlphabet() => await Assert.That(NanoId.Generate("1234abcd")).HasCount().EqualTo(DefaultSize);

    [Test]
    public async Task CustomAlphabetAsync() => await Assert.That(await NanoId.GenerateAsync("1234abcd")).HasCount().EqualTo(DefaultSize);

    [Test]
    public async Task CustomAlphabetAndSize()
    {
        const int Size = 7;
        await Assert.That(NanoId.Generate("1234abcd", Size)).HasCount().EqualTo(Size);
    }

    [Test]
    public async Task CustomAlphabetAndSizeAsync()
    {
        const int Size = 7;
        await Assert.That(await NanoId.GenerateAsync("1234abcd", Size)).HasCount().EqualTo(Size);
    }

    [Test]
    public async Task CustomRandom() => await Assert.That(NanoId.Generate(new Random(10))).HasCount().EqualTo(DefaultSize);

    [Test]
    public async Task CustomRandomAsync() => await Assert.That(await NanoId.GenerateAsync(new Random(10))).HasCount().EqualTo(DefaultSize);

    [Test]
    public async Task SingleLetterAlphabet() => await Assert.That(NanoId.Generate("a", 5)).IsEqualTo("aaaaa");

    [Test]
    public async Task SingleLetterAlphabetAsync() => await Assert.That(await NanoId.GenerateAsync("a", 5)).IsEqualTo("aaaaa");

    [Test]
    [Arguments(4, "adca")]
    [Arguments(18, "cbadcbadcbadcbadcc")]
    public async Task PredefinedRandomSequence(int size, string expected)
    {
        var random = new PredefinedRandom([2, 255, 3, 7, 7, 7, 7, 7, 0, 1]);
        await Assert.That(NanoId.Generate(random, "abcde", size)).IsEqualTo(expected);
    }

    [Test]
    [Arguments(4, "adca")]
    [Arguments(18, "cbadcbadcbadcbadcc")]
    public async Task PredefinedRandomSequenceAsync(int size, string expected)
    {
        var random = new PredefinedRandom([2, 255, 3, 7, 7, 7, 7, 7, 0, 1]);
        await Assert.That(await NanoId.GenerateAsync(random, "abcde", size)).IsEqualTo(expected);
    }

    [Test]
    public async Task GeneratesUrlFriendlyIDs()
    {
        const int Total = 10;
        var count = Total;
        while (count > 0)
        {
            var result = NanoId.Generate();
            await Assert.That(result).HasCount().EqualTo(DefaultSize);

            foreach (var c in result)
            {
                await Assert.That(result).Contains($"{c}");
            }

            count--;
        }
    }

    [Test]
    public async Task GeneratesUrlFriendlyIDsAsync()
    {
        const int Total = 10;
        var count = Total;
        while (count > 0)
        {
            var result = await NanoId.GenerateAsync();
            await Assert.That(result).HasCount().EqualTo(DefaultSize);

            foreach (var c in result)
            {
                await Assert.That(result).Contains($"{c}");
            }

            count--;
        }
    }

    [Test]
    public async Task HasNoCollisions()
    {
        const int Total = 100 * 1000;
        var dictUsed = new Dictionary<string, bool>();

        var count = Total;
        while (count > 0)
        {
            var result = NanoId.Generate();
            await Assert.That(dictUsed.TryGetValue(result, out _)).IsFalse();
            dictUsed.Add(result, true);

            count--;
        }
    }

    [Test]
    public async Task HasNoCollisionsAsync()
    {
        const int Total = 100 * 1000;
        var dictUsed = new Dictionary<string, bool>();

        var count = Total;
        while (count > 0)
        {
            var result = await NanoId.GenerateAsync();
            await Assert.That(dictUsed.TryGetValue(result, out _)).IsFalse();
            dictUsed.Add(result, true);

            count--;
        }
    }

    [Test]
    public async Task FlatDistribution()
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

        foreach (var distribution in chars.Select(c => c.Value * DefaultAlphabet.Length / (double)(Total * DefaultSize)))
        {
            await Assert.That(distribution).IsBetween(0.95, 1.05);
        }
    }

    [Test]
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
            await Assert.That(distribution).IsBetween(0.95, 1.05);
        }
    }

    [Test]
    public async Task Mask()
    {
        for (var length = 1; length < 256; length++)
        {
            var mask1 = (2 << (int)Math.Floor(Math.Log(length - 1) / Math.Log(2))) - 1;
#if NET7_0_OR_GREATER
            var mask2 = (2 << (31 - int.LeadingZeroCount((length - 1) | 1))) - 1;
#else
            var mask2 = (2 << 31 - NanoId.LeadingZeroCount((length - 1) | 1)) - 1;
#endif
            await Assert.That(mask1).IsEqualTo(mask2);
        }
    }

    private class PredefinedRandom(byte[] sequence) : Random
    {
        public override void NextBytes(byte[] buffer)
        {
            using var seq = this.GetSequence(buffer.Length).GetEnumerator();
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
            for (var i = 0; i < size / sequence.Length; i++)
            {
                result = result.Concat(result);
            }

            return result.Take(size).Reverse();
        }
    }
}