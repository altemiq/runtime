// -----------------------------------------------------------------------
// <copyright file="OneOf{T0,T1}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using static OneOf;

/// <summary>
/// Represents an option type with two types.
/// </summary>
/// <typeparam name="T0">The first option type.</typeparam>
/// <typeparam name="T1">The second option type.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct OneOf<T0, T1> : IOneOf, IEquatable<OneOf<T0, T1>>
{
    private readonly T0? value0;
    private readonly T1? value1;

    private OneOf(int index, T0? value0 = default, T1? value1 = default)
    {
        this.Index = index;
        this.value0 = value0;
        this.value1 = value1;
    }

    /// <inheritdoc />
    public object? Value =>
        this.Index switch
        {
            0 => this.value0,
            1 => this.value1,
            _ => throw new InvalidOperationException(),
        };

    /// <inheritdoc />
    public int Index { get; }

    /// <summary>
    /// Gets a value indicating whether this instance contains a <typeparamref name="T0"/> value.
    /// </summary>
    public bool IsT0 => this.Index == 0;

    /// <summary>
    /// Gets a value indicating whether this instance contains a <typeparamref name="T1"/> value.
    /// </summary>
    public bool IsT1 => this.Index == 1;

    /// <summary>
    /// Gets the value as a <typeparamref name="T0"/> instance.
    /// </summary>
    public T0? AsT0 => this.IsT0 ? this.value0 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T0), this.Index));

    /// <summary>
    /// Gets the value as a <typeparamref name="T1"/> instance.
    /// </summary>
    public T1? AsT1 => this.IsT1 ? this.value1 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T1), this.Index));

    /// <summary>
    /// Converts an instance <typeparamref name="T0"/> to an instance of <see cref="OneOf{T0,T1}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0, T1>(T0? t) => new(0, value0: t);

    /// <summary>
    /// Converts an instance <typeparamref name="T1"/> to an instance of <see cref="OneOf{T0,T1}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0, T1>(T1? t) => new(1, value1: t);

    /// <summary>
    /// Implements the equality operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(OneOf<T0, T1> left, OneOf<T0, T1> right) => left.Equals(right);

    /// <summary>
    /// Implements the inequality operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(OneOf<T0, T1> left, OneOf<T0, T1> right) => !left.Equals(right);

    /// <summary>
    /// Performs an action on this instance.
    /// </summary>
    /// <param name="f0">The <see cref="Action{T}"/> action for <typeparamref name="T0"/>.</param>
    /// <param name="f1">The <see cref="Action{T}"/> action for <typeparamref name="T1"/>.</param>
    public void Switch(Action<T0?> f0, Action<T1?> f1)
    {
        if (this.IsT0 && f0 is not null)
        {
            f0(this.value0);
            return;
        }

        if (this.IsT1 && f1 is not null)
        {
            f1(this.value1);
            return;
        }

        throw new InvalidOperationException();
    }

    /// <summary>
    /// Matches this instance.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="f0">The <see cref="Func{T, TResult}"/> for <typeparamref name="T0"/>.</param>
    /// <param name="f1">The <see cref="Func{T, TResult}"/> for <typeparamref name="T1"/>.</param>
    /// <returns>The matched result.</returns>
    public TResult Match<TResult>(Func<T0?, TResult> f0, Func<T1?, TResult> f1) => (this.Index, f0, f1) switch
    {
        (0, not null, _) => f0(this.value0),
        (1, _, not null) => f1(this.value1),
        _ => throw new InvalidOperationException(),
    };

    /// <summary>
    /// Maps the <typeparamref name="T0"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{TResult, T1}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T0"/> instance.</exception>
    public OneOf<TResult, T1> MapT0<TResult>(Func<T0?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => mapFunc(this.AsT0),
            1 => this.AsT1,
            _ => throw new InvalidOperationException(),
        };
    }

    /// <summary>
    /// Maps the <typeparamref name="T1"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{T0, TResult}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T1"/> instance.</exception>
    public OneOf<T0, TResult> MapT1<TResult>(Func<T1?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => this.AsT0,
            1 => mapFunc(this.AsT1),
            _ => throw new InvalidOperationException(),
        };
    }

    /// <summary>
    /// Tries to pick the value as a <typeparamref name="T0"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="remainder">The remainder.</param>
    /// <returns><see langword="true"/> upon success; otherwise <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Index"/> is out of range.</exception>
    public bool TryPickT0(out T0? value, out T1? remainder)
    {
        value = this.IsT0 ? this.AsT0 : default;
        remainder = this.Index switch
        {
            0 => default,
            1 => this.AsT1,
            _ => throw new InvalidOperationException(),
        };

        return this.IsT0;
    }

    /// <summary>
    /// Tries to pick the value as a <typeparamref name="T1"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="remainder">The remainder.</param>
    /// <returns><see langword="true"/> upon success; otherwise <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Index"/> is out of range.</exception>
    public bool TryPickT1(out T1? value, out T0? remainder)
    {
        value = this.IsT1 ? this.AsT1 : default;
        remainder = this.Index switch
        {
            0 => this.AsT0,
            1 => default,
            _ => throw new InvalidOperationException(),
        };

        return this.IsT1;
    }

    /// <inheritdoc/>
    public bool Equals(OneOf<T0, T1> other) =>
        this.Index == other.Index &&
        this.Index switch
        {
            0 => Equals(this.value0, other.value0),
            1 => Equals(this.value1, other.value1),
            _ => false,
        };

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OneOf<T0, T1> o && this.Equals(o);

    /// <inheritdoc/>
    public override string? ToString() =>
        this.Index switch
        {
            0 => FormatValue(this.value0),
            1 => FormatValue(this.value1),
            _ => default,
        };

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            return (GetHashCodeCore(this.Index, this.value0, this.value1) * 397) ^ this.Index;

            static int GetHashCodeCore(int index, T0? value0, T1? value1)
            {
                var hashCode = index switch
                {
                    0 => value0?.GetHashCode(),
                    1 => value1?.GetHashCode(),
                    _ => default,
                };

                return hashCode ?? 0;
            }
        }
    }
}