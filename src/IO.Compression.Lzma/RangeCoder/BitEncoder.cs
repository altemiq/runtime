// -----------------------------------------------------------------------
// <copyright file="BitEncoder.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace System.IO.Compression.RangeCoder;

/// <summary>
/// The bit encoder.
/// </summary>
internal struct BitEncoder
{
    /// <summary>
    /// The number of bit price shift bits.
    /// </summary>
    public const int BitPriceShiftBits = 6;

    private const int BitModelTotalBits = 11;
    private const uint BitModelTotal = 1 << BitModelTotalBits;

    private const int MoveBits = 5;
    private const int MoveReducingBits = 2;

    private static readonly uint[] ProbPrices = GetPropPrices();

    private uint probability;

    /// <summary>
    /// Initializes this instance.
    /// </summary>
    public void Init() => this.probability = BitModelTotal >> 1;

    /// <summary>
    /// Updates the model.
    /// </summary>
    /// <param name="symbol">The symbol.</param>
    public void UpdateModel(uint symbol)
    {
        if (symbol is 0U)
        {
            this.probability += (BitModelTotal - this.probability) >> MoveBits;
        }
        else
        {
            this.probability -= this.probability >> MoveBits;
        }
    }

    /// <summary>
    /// Encodes the symbol.
    /// </summary>
    /// <param name="encoder">The encoder.</param>
    /// <param name="symbol">The symbol.</param>
    public void Encode(RangeEncoder encoder, uint symbol)
    {
        var newBound = (encoder.Range >> BitModelTotalBits) * this.probability;
        if (symbol is 0)
        {
            encoder.Range = newBound;
            this.probability += (BitModelTotal - this.probability) >> MoveBits;
        }
        else
        {
            encoder.Low += newBound;
            encoder.Range -= newBound;
            this.probability -= this.probability >> MoveBits;
        }

        if (encoder.Range < RangeEncoder.TopValue)
        {
            encoder.Range <<= 8;
            encoder.ShiftLow();
        }
    }

    /// <summary>
    /// Gets the price.
    /// </summary>
    /// <param name="symbol">The symbol.</param>
    /// <returns>The price.</returns>
    public readonly uint GetPrice(uint symbol) => ProbPrices[(((this.probability - symbol) ^ (-(int)symbol)) & (BitModelTotal - 1)) >> MoveReducingBits];

    /// <summary>
    /// GEts the zero price.
    /// </summary>
    /// <returns>The zero price.</returns>
    public readonly uint GetPrice0() => ProbPrices[this.probability >> MoveReducingBits];

    /// <summary>
    /// Gets the one price.
    /// </summary>
    /// <returns>The one price.</returns>
    public readonly uint GetPrice1() => ProbPrices[(BitModelTotal - this.probability) >> MoveReducingBits];

    private static uint[] GetPropPrices()
    {
        const int Bits = BitModelTotalBits - MoveReducingBits;
        var propPrices = new uint[BitModelTotal >> MoveReducingBits];
        for (var i = Bits - 1; i >= 0; i--)
        {
            var start = 1U << (Bits - i - 1);
            var end = 1U << (Bits - i);
            for (var j = start; j < end; j++)
            {
                propPrices[j] = ((uint)i << BitPriceShiftBits) + (((end - j) << BitPriceShiftBits) >> (Bits - i - 1));
            }
        }

        return propPrices;
    }
}