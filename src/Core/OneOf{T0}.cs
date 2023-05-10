// -----------------------------------------------------------------------
// <copyright file="OneOf{T0}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using static OneOf;

/// <summary>
/// Represents an option type with a single type.
/// </summary>
/// <typeparam name="T0">The type of option.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct OneOf<T0> : IOneOf, IEquatable<OneOf<T0>>
{
    private readonly T0? value0;

    private OneOf(int index, T0? value0 = default)
    {
        this.Index = index;
        this.value0 = value0;
    }

    /// <inheritdoc />
    public object? Value =>
        this.Index switch
        {
            0 => this.value0,
            _ => throw new InvalidOperationException(),
        };

    /// <inheritdoc />
    public int Index { get; }

    /// <summary>
    /// Gets a value indicating whether this instance contains a <typeparamref name="T0"/> value.
    /// </summary>
    public bool IsT0 => this.Index == 0;

    /// <summary>
    /// Gets the value as a <typeparamref name="T0"/> instance.
    /// </summary>
    public T0? AsT0 => this.IsT0 ? this.value0 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T0), this.Index));

    /// <summary>
    /// Converts an instance <typeparamref name="T0"/> to an instance of <see cref="OneOf{T0}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0>(T0? t) => new(0, value0: t);

    /// <summary>
    /// Implements the equality operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(OneOf<T0> left, OneOf<T0> right) => left.Equals(right);

    /// <summary>
    /// Implements the inequality operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(OneOf<T0> left, OneOf<T0> right) => !left.Equals(right);

    /// <summary>
    /// Performs an action on this instance.
    /// </summary>
    /// <param name="f0">The <see cref="Action{T}"/> action for <typeparamref name="T0"/>.</param>
    public void Switch(Action<T0?> f0)
    {
        if (this.IsT0 && f0 is not null)
        {
            f0(this.value0);
            return;
        }

        throw new InvalidOperationException();
    }

    /// <summary>
    /// Matches this instance.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="f0">The <see cref="Func{T, TResult}"/> for <typeparamref name="T0"/>.</param>
    /// <returns>The matched result.</returns>
    public TResult Match<TResult>(Func<T0?, TResult> f0) => (this.Index, f0) switch
    {
        (0, not null) => f0(this.value0),
        _ => throw new InvalidOperationException(),
    };

    /// <summary>
    /// Maps the <typeparamref name="T0"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{TResult}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T0"/> instance.</exception>
    public OneOf<TResult> MapT0<TResult>(Func<T0?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => mapFunc(this.AsT0),
            _ => throw new InvalidOperationException(),
        };
    }

    /// <inheritdoc />
    public bool Equals(OneOf<T0> other) =>
        this.Index == other.Index &&
        this.Index switch
        {
            0 => Equals(this.value0, other.value0),
            _ => false,
        };

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is OneOf<T0> o && this.Equals(o);

    /// <inheritdoc />
    public override string? ToString() =>
        this.Index switch
        {
            0 => FormatValue(this.value0),
            _ => default,
        };

    /// <inheritdoc />
    public override int GetHashCode()
    {
        unchecked
        {
            return (GetHashCodeCore(this.Index, this.value0) * 397) ^ this.Index;

            static int GetHashCodeCore(int index, T0? value0)
            {
                var hashCode = index switch
                {
                    0 => value0?.GetHashCode(),
                    _ => default,
                };

                return hashCode ?? 0;
            }
        }
    }
}