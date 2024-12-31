// -----------------------------------------------------------------------
// <copyright file="SerializableHalf.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

#if NET5_0_OR_GREATER
using Xunit.Sdk;

public class SerializableHalf : IXunitSerializable
{
    private Half value;

    public SerializableHalf()
    {
    }

    public SerializableHalf(Half value) => this.value = value;

    public void Deserialize(IXunitSerializationInfo info) => this.value = BitConverter.Int16BitsToHalf(info.GetValue<short>($"{nameof(Half)}.{nameof(this.value)}"));

    public void Serialize(IXunitSerializationInfo info) => info.AddValue($"{nameof(Half)}.{nameof(this.value)}", BitConverter.HalfToInt16Bits(this.value), typeof(short));

    public static implicit operator Half(SerializableHalf value) => value.value;

    public static implicit operator SerializableHalf(Half value) => new(value);

    public static implicit operator SerializableHalf(double value)
    {
        if (value <= (double)Half.MinValue)
        {
            return new(Half.MinValue);
        }
        else if (value >= (double)Half.MaxValue)
        {
            return new(Half.MaxValue);
        }

        return new((Half)value);
    }

    public override string ToString() => this.value.ToString();

    public override bool Equals(object? obj) => obj switch
    {
        SerializableHalf half => HalfEqualityComparer.Instance.Equals(this.value, half.value),
        Half half => HalfEqualityComparer.Instance.Equals(this.value, half),
        _ => base.Equals(obj),
    };

    public override int GetHashCode() => HalfEqualityComparer.Instance.GetHashCode(this);
}

public class HalfEqualityComparer : IEqualityComparer<Half>
{
    public static readonly HalfEqualityComparer Instance = new();

    public bool Equals(Half x, Half y) => x.Equals(y);

    public int GetHashCode([System.Diagnostics.CodeAnalysis.DisallowNull] Half obj) => obj.GetHashCode();
}
#endif