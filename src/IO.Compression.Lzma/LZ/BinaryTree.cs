// -----------------------------------------------------------------------
// <copyright file="BinaryTree.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression.LZ;

/// <summary>
/// The BIN tree.
/// </summary>
internal sealed class BinaryTree : InWindow, IMatchFinder
{
    private const uint Hash2Size = 1U << 10;
    private const uint Hash3Size = 1U << 16;
    private const uint BinaryTree2HashSize = 1U << 16;
    private const uint StartMaxLen = 1U;
    private const uint Hash3Offset = Hash2Size;
    private const uint EmptyHashValue = default;
    private const uint MaxValForNormalize = (1U << 31) - 1U;

    private readonly bool hashArray;

    private readonly uint numHashDirectBytes;
    private readonly uint minMatchCheck;
    private readonly uint fixHashSize;

    private uint cyclicBufferPos;
    private uint cyclicBufferSize;
    private uint matchMaximumLength;

    private uint[] son = [];
    private uint[] hash = [];

    private uint cutValue = byte.MaxValue;
    private uint hashMask;
    private uint hashSizeSum;

    /// <summary>
    /// Initializes a new instance of the <see cref="BinaryTree"/> class.
    /// </summary>
    /// <param name="numHashBytes">The number of hash bytes.</param>
    public BinaryTree(int numHashBytes)
    {
        this.hashArray = numHashBytes > 2;
        if (this.hashArray)
        {
            this.numHashDirectBytes = 0U;
            this.minMatchCheck = 4U;
            this.fixHashSize = Hash2Size + Hash3Size;
        }
        else
        {
            this.numHashDirectBytes = 2U;
            this.minMatchCheck = 2U + 1U;
            this.fixHashSize = 0U;
        }
    }

    /// <inheritdoc/>
    public override void Init(Stream stream)
    {
        base.Init(stream);
        for (var i = 0U; i < this.hashSizeSum; i++)
        {
            this.hash[i] = EmptyHashValue;
        }

        this.cyclicBufferPos = 0;
        this.ReduceOffsets(-1);
    }

    /// <inheritdoc />
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="historySize"/> is too large.</exception>
    public void Create(uint historySize, uint addBufferBefore, uint matchLength, uint addBufferAfter)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(historySize, MaxValForNormalize - 256);

        this.cutValue = 16 + (matchLength >> 1);

        var windowReserveSize = ((historySize + addBufferBefore + matchLength + addBufferAfter) / 2) + 256;

        this.Create(historySize + addBufferBefore, matchLength + addBufferAfter, windowReserveSize);

        this.matchMaximumLength = matchLength;

        var newCyclicBufferSize = historySize + 1;
        if (this.cyclicBufferSize != newCyclicBufferSize)
        {
            this.cyclicBufferSize = newCyclicBufferSize;
            this.son = new uint[this.cyclicBufferSize * 2];
        }

        var hashSize = BinaryTree2HashSize;

        if (this.hashArray)
        {
            hashSize = historySize - 1;
            hashSize |= hashSize >> 1;
            hashSize |= hashSize >> 2;
            hashSize |= hashSize >> 4;
            hashSize |= hashSize >> 8;
            hashSize >>= 1;
            hashSize |= ushort.MaxValue;
            if (hashSize > (1 << 24))
            {
                hashSize >>= 1;
            }

            this.hashMask = hashSize;
            hashSize++;
            hashSize += this.fixHashSize;
        }

        if (hashSize == this.hashSizeSum)
        {
            return;
        }

        this.hashSizeSum = hashSize;
        this.hash = new uint[hashSize];
    }

    /// <inheritdoc/>
    public uint GetMatches(uint[] distances)
    {
        uint lenLimit;
        if (this.Position + this.matchMaximumLength <= this.StreamPosition)
        {
            lenLimit = this.matchMaximumLength;
        }
        else
        {
            lenLimit = this.StreamPosition - this.Position;
            if (lenLimit < this.minMatchCheck)
            {
                this.MovePosition();
                return 0;
            }
        }

        var offset = 0U;
        var matchMinPos = (this.Position > this.cyclicBufferSize) ? (this.Position - this.cyclicBufferSize) : 0;
        var cur = this.BufferOffset + this.Position;

        // to avoid items for len < hashSize
        var maxLen = StartMaxLen;
        uint hashValue;
        var hash2Value = 0U;
        var hash3Value = 0U;

        if (this.Buffer is null)
        {
            throw new InvalidOperationException();
        }

        if (this.hashArray)
        {
            var temp = Common.Crc.Table[this.Buffer[cur]] ^ this.Buffer[cur + 1];
            hash2Value = temp & (Hash2Size - 1);
            temp ^= (uint)this.Buffer[cur + 2] << 8;
            hash3Value = temp & (Hash3Size - 1);
            hashValue = (temp ^ (Common.Crc.Table[this.Buffer[cur + 3]] << 5)) & this.hashMask;
        }
        else
        {
            hashValue = this.Buffer[cur] ^ ((uint)this.Buffer[cur + 1] << 8);
        }

        var curMatch = this.hash[this.fixHashSize + hashValue];
        if (this.hashArray)
        {
            var curMatch2 = this.hash[hash2Value];
            var curMatch3 = this.hash[Hash3Offset + hash3Value];
            this.hash[hash2Value] = this.Position;
            this.hash[Hash3Offset + hash3Value] = this.Position;
            if (curMatch2 > matchMinPos
                && this.Buffer[this.BufferOffset + curMatch2] == this.Buffer[cur])
            {
                distances[offset++] = maxLen = 2;
                distances[offset++] = this.Position - curMatch2 - 1;
            }

            if (curMatch3 > matchMinPos
                && this.Buffer[this.BufferOffset + curMatch3] == this.Buffer[cur])
            {
                if (curMatch3 == curMatch2
                    && offset > 1)
                {
                    offset -= 2;
                }

                distances[offset++] = maxLen = 3;
                distances[offset++] = this.Position - curMatch3 - 1;
                curMatch2 = curMatch3;
            }

            if (offset is > 1 && curMatch2 == curMatch)
            {
                offset -= 2;
                maxLen = StartMaxLen;
            }
        }

        this.hash[this.fixHashSize + hashValue] = this.Position;

        var ptr0 = (this.cyclicBufferPos << 1) + 1;
        var ptr1 = this.cyclicBufferPos << 1;

        var len0 = this.numHashDirectBytes;
        var len1 = this.numHashDirectBytes;

        if (this.numHashDirectBytes is not 0
            && curMatch > matchMinPos
            && this.Buffer[this.BufferOffset + curMatch + this.numHashDirectBytes] != this.Buffer[cur + this.numHashDirectBytes])
        {
            distances[offset++] = maxLen = this.numHashDirectBytes;
            distances[offset++] = this.Position - curMatch - 1;
        }

        var count = this.cutValue;

        while (true)
        {
            if (curMatch <= matchMinPos || count-- is 0)
            {
                this.son[ptr0] = this.son[ptr1] = EmptyHashValue;
                break;
            }

            var delta = this.Position - curMatch;
            var cyclicPos = ((delta <= this.cyclicBufferPos)
                ? (this.cyclicBufferPos - delta)
                : (this.cyclicBufferPos - delta + this.cyclicBufferSize)) << 1;

            var pby1 = this.BufferOffset + curMatch;
            var len = Math.Min(len0, len1);
            if (this.Buffer[pby1 + len] == this.Buffer[cur + len])
            {
                while (++len != lenLimit)
                {
                    if (this.Buffer[pby1 + len] != this.Buffer[cur + len])
                    {
                        break;
                    }
                }

                if (maxLen < len)
                {
                    distances[offset++] = maxLen = len;
                    distances[offset++] = delta - 1;
                    if (len == lenLimit)
                    {
                        this.son[ptr1] = this.son[cyclicPos];
                        this.son[ptr0] = this.son[cyclicPos + 1];
                        break;
                    }
                }
            }

            if (this.Buffer[pby1 + len] < this.Buffer[cur + len])
            {
                this.son[ptr1] = curMatch;
                ptr1 = cyclicPos + 1;
                curMatch = this.son[ptr1];
                len1 = len;
            }
            else
            {
                this.son[ptr0] = curMatch;
                ptr0 = cyclicPos;
                curMatch = this.son[ptr0];
                len0 = len;
            }
        }

        this.MovePosition();
        return offset;
    }

    /// <inheritdoc />
    public void Skip(uint num)
    {
        do
        {
            uint lenLimit;
            if (this.Position + this.matchMaximumLength <= this.StreamPosition)
            {
                lenLimit = this.matchMaximumLength;
            }
            else
            {
                lenLimit = this.StreamPosition - this.Position;
                if (lenLimit < this.minMatchCheck)
                {
                    this.MovePosition();
                    continue;
                }
            }

            var matchMinPos = (this.Position > this.cyclicBufferSize) ? (this.Position - this.cyclicBufferSize) : 0;
            var cur = this.BufferOffset + this.Position;

            uint hashValue;
            if (this.Buffer is null)
            {
                throw new InvalidOperationException();
            }

            if (this.hashArray)
            {
                var temp = Common.Crc.Table[this.Buffer[cur]] ^ this.Buffer[cur + 1];
                var hash2Value = temp & (Hash2Size - 1);
                this.hash[hash2Value] = this.Position;
                temp ^= (uint)this.Buffer[cur + 2] << 8;
                var hash3Value = temp & (Hash3Size - 1);
                this.hash[Hash3Offset + hash3Value] = this.Position;
                hashValue = (temp ^ (Common.Crc.Table[this.Buffer[cur + 3]] << 5)) & this.hashMask;
            }
            else
            {
                hashValue = this.Buffer[cur] ^ ((uint)this.Buffer[cur + 1] << 8);
            }

            var curMatch = this.hash[this.fixHashSize + hashValue];
            this.hash[this.fixHashSize + hashValue] = this.Position;

            var ptr0 = (this.cyclicBufferPos << 1) + 1;
            var ptr1 = this.cyclicBufferPos << 1;

            var len0 = this.numHashDirectBytes;
            var len1 = this.numHashDirectBytes;

            var count = this.cutValue;
            while (true)
            {
                if (curMatch <= matchMinPos || count-- is 0)
                {
                    this.son[ptr0] = this.son[ptr1] = EmptyHashValue;
                    break;
                }

                var delta = this.Position - curMatch;
                var cyclicPos = ((delta <= this.cyclicBufferPos)
                    ? (this.cyclicBufferPos - delta)
                    : (this.cyclicBufferPos - delta + this.cyclicBufferSize)) << 1;

                var pby1 = this.BufferOffset + curMatch;
                var len = Math.Min(len0, len1);
                if (this.Buffer[pby1 + len] == this.Buffer[cur + len])
                {
                    while (++len != lenLimit)
                    {
                        if (this.Buffer[pby1 + len] != this.Buffer[cur + len])
                        {
                            break;
                        }
                    }

                    if (len == lenLimit)
                    {
                        this.son[ptr1] = this.son[cyclicPos];
                        this.son[ptr0] = this.son[cyclicPos + 1];
                        break;
                    }
                }

                if (this.Buffer[pby1 + len] < this.Buffer[cur + len])
                {
                    this.son[ptr1] = curMatch;
                    ptr1 = cyclicPos + 1;
                    curMatch = this.son[ptr1];
                    len1 = len;
                }
                else
                {
                    this.son[ptr0] = curMatch;
                    ptr0 = cyclicPos;
                    curMatch = this.son[ptr0];
                    len0 = len;
                }
            }

            this.MovePosition();
        }
        while (--num is not 0U);
    }

    /// <inheritdoc/>
    protected override void MovePosition()
    {
        if (++this.cyclicBufferPos >= this.cyclicBufferSize)
        {
            this.cyclicBufferPos = 0;
        }

        base.MovePosition();
        if (this.Position is MaxValForNormalize)
        {
            this.Normalize();
        }
    }

    private static void NormalizeLinks(uint[] items, uint numItems, uint subValue)
    {
        for (var i = 0U; i < numItems; i++)
        {
            var value = items[i];
            if (value <= subValue)
            {
                value = EmptyHashValue;
            }
            else
            {
                value -= subValue;
            }

            items[i] = value;
        }
    }

    private void Normalize()
    {
        var subValue = this.Position - this.cyclicBufferSize;
        NormalizeLinks(this.son, this.cyclicBufferSize * 2, subValue);
        NormalizeLinks(this.hash, this.hashSizeSum, subValue);
        this.ReduceOffsets((int)subValue);
    }
}