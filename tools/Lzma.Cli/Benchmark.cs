// -----------------------------------------------------------------------
// <copyright file="Benchmark.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq.Lzma.Cli;

using System.IO.Compression;

/// <summary>
/// The benchmarks.
/// </summary>
internal static class Benchmark
{
    private const uint AdditionalSize = 6 << 20;
    private const uint CompressedAdditionalSize = 1 << 10;

    private const int SubBits = 8;

    /// <summary>
    /// Runs the benchmark.
    /// </summary>
    /// <param name="iterations">The number of iterations.</param>
    /// <param name="dictionarySize">The dictionary size.</param>
    /// <returns>The return value.</returns>
    public static int Run(int iterations, uint dictionarySize)
    {
        if (iterations <= 0)
        {
            return 0;
        }

        if (dictionarySize < 1 << 18)
        {
            Console.WriteLine("\nError: dictionary size for benchmark must be >= 19 (512 KB)");
            return 1;
        }

        Console.Write("\n       Compressing                Decompressing\n\n");

        // undo the bit shifting
        var dictionary = 1;
        while ((1U << dictionary) < dictionarySize)
        {
            dictionary++;
        }

        LzmaCompressionOptions options = new() { Dictionary = dictionary };

        var bufferSize = dictionarySize + AdditionalSize;
        var compressedBufferSize = (bufferSize / 2) + CompressedAdditionalSize;

        byte[]? propArray = null;

        var rg = new BenchRandomGenerator(bufferSize);

        rg.Generate();
        var crc = new System.IO.Compression.Common.Crc();
        crc.Init();
        crc.Update(rg.Buffer, 0, (uint)rg.Buffer.Length);

        var totalBenchSize = 0L;
        var totalEncodeTime = 0L;
        var totalDecodeTime = 0L;
        var totalCompressedSize = 0L;

        var rawStream = new MemoryStream(rg.Buffer);
        var compressedStream = new MemoryStream((int)compressedBufferSize);
        var crcOutStream = new CrcOutStream();
        for (var i = 0; i < iterations; i++)
        {
            long start = default;
            _ = rawStream.Seek(0, SeekOrigin.Begin);
            _ = compressedStream.Seek(0, SeekOrigin.Begin);
            var inSize = 0L;
            var encoder = options.CreateEncoder();
            if (propArray is null)
            {
                var propStream = new MemoryStream();
                encoder.WriteCoderProperties(propStream);
                propArray = propStream.ToArray();
            }

            encoder.Compress(rawStream, compressedStream, (ins, _) =>
            {
                if (inSize is not 0L || ins < dictionarySize)
                {
                    return;
                }

                start = System.Diagnostics.Stopwatch.GetTimestamp();
                inSize = ins;
            });
            var timeSpan = System.Diagnostics.Stopwatch.GetElapsedTime(start);
            var encodeTime = timeSpan.Ticks;

            var compressedSize = compressedStream.Position;
            if (inSize is 0)
            {
                throw new InvalidOperationException("Internal ERROR 1282");
            }

            var benchSize = bufferSize - inSize;
            PrintResults(dictionarySize, encodeTime, benchSize, decompressMode: false, 0);

            var decodeTime = 0L;
            for (var j = 0; j < 2; j++)
            {
                _ = compressedStream.Seek(0, SeekOrigin.Begin);
                crcOutStream.Init();

                var decoder = new LzmaDecoder(propArray);
                ulong outSize = bufferSize;
                start = System.Diagnostics.Stopwatch.GetTimestamp();
                decoder.Decode(compressedStream, crcOutStream, (long)outSize);
                timeSpan = System.Diagnostics.Stopwatch.GetElapsedTime(start);
                decodeTime = timeSpan.Ticks;
                if (crcOutStream.Digest != crc.Digest)
                {
                    throw new InvalidOperationException("CRC Error");
                }
            }

            Console.Write("     ");
            PrintResults(dictionarySize, decodeTime, bufferSize, decompressMode: true, compressedSize);
            Console.WriteLine();

            totalBenchSize += benchSize;
            totalEncodeTime += encodeTime;
            totalDecodeTime += decodeTime;
            totalCompressedSize += compressedSize;
        }

        Console.WriteLine("---------------------------------------------------");
        PrintResults(dictionarySize, totalEncodeTime, totalBenchSize, decompressMode: false, 0);
        Console.Write("     ");
        PrintResults(dictionarySize, totalDecodeTime, bufferSize * iterations, decompressMode: true, totalCompressedSize);
        Console.WriteLine("    Average");
        return 0;

        static void PrintResults(
            uint dictionarySize,
            long elapsedTime,
            long size,
            bool decompressMode,
            long secondSize)
        {
            var speed = MyMultDiv64(size, elapsedTime);
            PrintValue(speed / 1024);
            Console.Write(" KB/s  ");
            var rating = decompressMode ? GetDecompressRating(elapsedTime, size, secondSize) : GetCompressRating(dictionarySize, elapsedTime, size);
            PrintRating(rating);

            static long MyMultDiv64(long value, long elapsedTime)
            {
                long frequency = TimeSpan.TicksPerSecond;
                while (frequency > 1000000)
                {
                    frequency >>= 1;
                    elapsedTime >>= 1;
                }

                if (elapsedTime is 0)
                {
                    elapsedTime = 1;
                }

                return value * frequency / elapsedTime;
            }

            static void PrintValue(long v)
            {
                var s = v.ToString(System.Globalization.CultureInfo.CurrentCulture).PadLeft(6, ' ');
                Console.Write(s);
            }

            static void PrintRating(long rating)
            {
                PrintValue(rating / 1000000);
                Console.Write(" MIPS");
            }

            static long GetDecompressRating(long elapsedTime, long outSize, long inSize)
            {
                var commands = (inSize * 220) + (outSize * 20);
                return MyMultDiv64(commands, elapsedTime);
            }

            static long GetCompressRating(uint dictionarySize, long elapsedTime, long size)
            {
                var t = GetLogSize(dictionarySize) - (18 << SubBits);
                var commandsForOne = 1060 + ((t * t * 10) >> (2 * SubBits));
                return MyMultDiv64(size * commandsForOne, elapsedTime);
            }
        }
    }

    private static int GetLogSize(uint size)
    {
        for (var i = SubBits; i < 32; i++)
        {
            for (var j = 0; j < 1 << SubBits; j++)
            {
                if (size <= (1U << i) + (j << (i - SubBits)))
                {
                    return (i << SubBits) + j;
                }
            }
        }

        return 32 << SubBits;
    }

    private sealed class BenchRandomGenerator(uint bufferSize)
    {
        private readonly BitRandomGenerator rG = new();
        private uint pos;
        private uint rep0;

        public byte[] Buffer { get; } = new byte[bufferSize];

        public void Generate()
        {
            this.rG.Init();
            this.rep0 = 1;
            while (this.pos < this.Buffer.Length)
            {
                if (this.GetRndBit() == 0 || this.pos < 1)
                {
                    this.Buffer[this.pos++] = (byte)this.rG.GetRnd(8);
                }
                else
                {
                    uint len;
                    if (this.rG.GetRnd(3) == 0)
                    {
                        len = 1 + this.GetLen1();
                    }
                    else
                    {
                        do
                        {
                            this.rep0 = this.GetOffset();
                        }
                        while (this.rep0 >= this.pos);
                        this.rep0++;
                        len = 2 + this.GetLen2();
                    }

                    for (uint i = 0; i < len && this.pos < this.Buffer.Length; i++, this.pos++)
                    {
                        this.Buffer[this.pos] = this.Buffer[this.pos - this.rep0];
                    }
                }
            }
        }

        private uint GetRndBit() => this.rG.GetRnd(1);

        private uint GetLogRandBits(int numBits)
        {
            var len = this.rG.GetRnd(numBits);
            return this.rG.GetRnd((int)len);
        }

        private uint GetOffset() => this.GetRndBit() == 0 ? this.GetLogRandBits(4) : (this.GetLogRandBits(4) << 10) | this.rG.GetRnd(10);

        private uint GetLen1() => this.rG.GetRnd(1 + (int)this.rG.GetRnd(2));

        private uint GetLen2() => this.rG.GetRnd(2 + (int)this.rG.GetRnd(2));

        private sealed class BitRandomGenerator
        {
            private readonly RandomGenerator rG = new();
            private uint value;
            private int currentBits;

            public void Init()
            {
                this.value = 0;
                this.currentBits = 0;
            }

            public uint GetRnd(int bits)
            {
                uint result;
                if (this.currentBits > bits)
                {
                    result = this.value & ((1U << bits) - 1);
                    this.value >>= bits;
                    this.currentBits -= bits;
                    return result;
                }

                bits -= this.currentBits;
                result = this.value << bits;
                this.value = this.rG.GetRnd();
                result |= this.value & ((1U << bits) - 1);
                this.value >>= bits;
                this.currentBits = 32 - bits;
                return result;
            }

            private sealed class RandomGenerator
            {
                private uint a1 = 362436069;
                private uint a2 = 521288629;

                public uint GetRnd()
                {
                    var v = this.a1 = (36969 * (this.a1 & 0xffff)) + (this.a1 >> 16);
                    var v1 = this.a2 = (18000 * (this.a2 & 0xffff)) + (this.a2 >> 16);
                    return (v << 16) ^ v1;
                }
            }
        }
    }

    private sealed class CrcOutStream : Stream
    {
        private readonly System.IO.Compression.Common.Crc crc = new();

        public override bool CanRead => false;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => 0;

        public override long Position
        {
            get => 0;
            set => System.Diagnostics.Debug.Write(string.Create(System.Globalization.CultureInfo.InvariantCulture, $"Ignoring setting the position to {value}"));
        }

        public uint Digest => this.crc.Digest;

        public void Init() => this.crc.Init();

        public override void Flush()
        {
        }

        public override long Seek(long offset, SeekOrigin origin) => 0L;

        public override void SetLength(long value)
        {
        }

        public override int Read(byte[] buffer, int offset, int count) => 0;

        public override void WriteByte(byte value) => this.crc.Update(value);

        public override void Write(byte[] buffer, int offset, int count) => this.crc.Update(buffer, (uint)offset, (uint)count);
    }
}