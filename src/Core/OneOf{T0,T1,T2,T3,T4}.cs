// -----------------------------------------------------------------------
// <copyright file="OneOf{T0,T1,T2,T3,T4}.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using static OneOf;

/// <summary>
/// Represents an option type with three types.
/// </summary>
/// <typeparam name="T0">The first option type.</typeparam>
/// <typeparam name="T1">The second option type.</typeparam>
/// <typeparam name="T2">The third option type.</typeparam>
/// <typeparam name="T3">The forth option type.</typeparam>
/// <typeparam name="T4">The fifth option type.</typeparam>
[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
public readonly struct OneOf<T0, T1, T2, T3, T4> : IOneOf, IEquatable<OneOf<T0, T1, T2, T3, T4>>
{
    private readonly T0? value0;
    private readonly T1? value1;
    private readonly T2? value2;
    private readonly T3? value3;
    private readonly T4? value4;

    private OneOf(int index, T0? value0 = default, T1? value1 = default, T2? value2 = default, T3? value3 = default, T4? value4 = default)
    {
        this.Index = index;
        this.value0 = value0;
        this.value1 = value1;
        this.value2 = value2;
        this.value3 = value3;
        this.value4 = value4;
    }

    /// <inheritdoc/>
    public object? Value =>
        this.Index switch
        {
            0 => this.value0,
            1 => this.value1,
            2 => this.value2,
            3 => this.value3,
            4 => this.value4,
            _ => throw new InvalidOperationException(),
        };

    /// <inheritdoc/>
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
    /// Gets a value indicating whether this instance contains a <typeparamref name="T2"/> value.
    /// </summary>
    public bool IsT2 => this.Index == 2;

    /// <summary>
    /// Gets a value indicating whether this instance contains a <typeparamref name="T3"/> value.
    /// </summary>
    public bool IsT3 => this.Index == 3;

    /// <summary>
    /// Gets a value indicating whether this instance contains a <typeparamref name="T4"/> value.
    /// </summary>
    public bool IsT4 => this.Index == 4;

    /// <summary>
    /// Gets the value as a <typeparamref name="T0"/> instance.
    /// </summary>
    public T0? AsT0 => this.IsT0 ? this.value0 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T0), this.Index));

    /// <summary>
    /// Gets the value as a <typeparamref name="T1"/> instance.
    /// </summary>
    public T1? AsT1 => this.IsT1 ? this.value1 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T1), this.Index));

    /// <summary>
    /// Gets the value as a <typeparamref name="T2"/> instance.
    /// </summary>
    public T2? AsT2 => this.IsT2 ? this.value2 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T2), this.Index));

    /// <summary>
    /// Gets the value as a <typeparamref name="T3"/> instance.
    /// </summary>
    public T3? AsT3 => this.IsT3 ? this.value3 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T3), this.Index));

    /// <summary>
    /// Gets the value as a <typeparamref name="T4"/> instance.
    /// </summary>
    public T4? AsT4 => this.IsT4 ? this.value4 : throw new InvalidOperationException(string.Format(Properties.Resources.Culture, Properties.Resources.CannotReturnAsType, nameof(T4), this.Index));

    /// <summary>
    /// Converts an instance <typeparamref name="T0"/> to an instance of <see cref="OneOf{T0,T1,T2}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0, T1, T2, T3, T4>(T0? t) => new(0, value0: t);

    /// <summary>
    /// Converts an instance <typeparamref name="T1"/> to an instance of <see cref="OneOf{T0,T1,T2}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0, T1, T2, T3, T4>(T1? t) => new(1, value1: t);

    /// <summary>
    /// Converts an instance <typeparamref name="T2"/> to an instance of <see cref="OneOf{T0,T1,T2}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0, T1, T2, T3, T4>(T2? t) => new(2, value2: t);

    /// <summary>
    /// Converts an instance <typeparamref name="T3"/> to an instance of <see cref="OneOf{T0,T1,T2,T3}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0, T1, T2, T3, T4>(T3? t) => new(3, value3: t);

    /// <summary>
    /// Converts an instance <typeparamref name="T4"/> to an instance of <see cref="OneOf{T0,T1,T2,T3}"/>.
    /// </summary>
    /// <param name="t">The value.</param>
    public static implicit operator OneOf<T0, T1, T2, T3, T4>(T4? t) => new(4, value4: t);

    /// <summary>
    /// Implements the equality operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator ==(OneOf<T0, T1, T2, T3, T4> left, OneOf<T0, T1, T2, T3, T4> right) => left.Equals(right);

    /// <summary>
    /// Implements the inequality operator.
    /// </summary>
    /// <param name="left">The left operand.</param>
    /// <param name="right">The right operand.</param>
    /// <returns>The result of the operator.</returns>
    public static bool operator !=(OneOf<T0, T1, T2, T3, T4> left, OneOf<T0, T1, T2, T3, T4> right) => !left.Equals(right);

    /// <summary>
    /// Performs an action on this instance.
    /// </summary>
    /// <param name="f0">The <see cref="Action{T}"/> action for <typeparamref name="T0"/>.</param>
    /// <param name="f1">The <see cref="Action{T}"/> action for <typeparamref name="T1"/>.</param>
    /// <param name="f2">The <see cref="Action{T}"/> action for <typeparamref name="T2"/>.</param>
    /// <param name="f3">The <see cref="Action{T}"/> action for <typeparamref name="T3"/>.</param>
    /// <param name="f4">The <see cref="Action{T}"/> action for <typeparamref name="T4"/>.</param>
    public void Switch(Action<T0?> f0, Action<T1?> f1, Action<T2?> f2, Action<T3?> f3, Action<T4?> f4)
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

        if (this.IsT2 && f2 is not null)
        {
            f2(this.value2);
            return;
        }

        if (this.IsT3 && f3 is not null)
        {
            f3(this.value3);
            return;
        }

        if (this.IsT4 && f4 is not null)
        {
            f4(this.value4);
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
    /// <param name="f2">The <see cref="Func{T, TResult}"/> for <typeparamref name="T2"/>.</param>
    /// <param name="f3">The <see cref="Func{T, TResult}"/> for <typeparamref name="T3"/>.</param>
    /// <param name="f4">The <see cref="Func{T, TResult}"/> for <typeparamref name="T4"/>.</param>
    /// <returns>The matched result.</returns>
    public TResult Match<TResult>(Func<T0?, TResult> f0, Func<T1?, TResult> f1, Func<T2?, TResult> f2, Func<T3?, TResult> f3, Func<T4?, TResult> f4) => (this.Index, f0, f1, f2, f3, f4) switch
    {
        (0, not null, _, _, _, _) => f0(this.value0),
        (1, _, not null, _, _, _) => f1(this.value1),
        (2, _, _, not null, _, _) => f2(this.value2),
        (3, _, _, _, not null, _) => f3(this.value3),
        (4, _, _, _, _, not null) => f4(this.value4),
        _ => throw new InvalidOperationException(),
    };

    /// <summary>
    /// Maps the <typeparamref name="T0"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{TResult,T1,T2,T3,T4}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T0"/> instance.</exception>
    public OneOf<TResult, T1, T2, T3, T4> MapT0<TResult>(Func<T0?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => mapFunc(this.AsT0),
            1 => this.AsT1,
            2 => this.AsT2,
            3 => this.AsT3,
            4 => this.AsT4,
            _ => throw new InvalidOperationException(),
        };
    }

    /// <summary>
    /// Maps the <typeparamref name="T1"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{T0,TResult,T1,T3,T4}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T1"/> instance.</exception>
    public OneOf<T0, TResult, T2, T3, T4> MapT1<TResult>(Func<T1?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => this.AsT0,
            1 => mapFunc(this.AsT1),
            2 => this.AsT2,
            3 => this.AsT3,
            4 => this.AsT4,
            _ => throw new InvalidOperationException(),
        };
    }

    /// <summary>
    /// Maps the <typeparamref name="T2"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{T0,T1,TResult,T3,T4}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T0"/> instance.</exception>
    public OneOf<T0, T1, TResult, T3, T4> MapT2<TResult>(Func<T2?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => this.AsT0,
            1 => this.AsT1,
            2 => mapFunc(this.AsT2),
            3 => this.AsT3,
            4 => this.AsT4,
            _ => throw new InvalidOperationException(),
        };
    }

    /// <summary>
    /// Maps the <typeparamref name="T3"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{T0,T1,T2,TResult,T4}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T0"/> instance.</exception>
    public OneOf<T0, T1, T2, TResult, T4> MapT3<TResult>(Func<T3?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => this.AsT0,
            1 => this.AsT1,
            2 => this.AsT2,
            3 => mapFunc(this.AsT3),
            4 => this.AsT4,
            _ => throw new InvalidOperationException(),
        };
    }

    /// <summary>
    /// Maps the <typeparamref name="T3"/> instance through the function.
    /// </summary>
    /// <typeparam name="TResult">The type of result.</typeparam>
    /// <param name="mapFunc">The map function.</param>
    /// <returns>A new instance of <see cref="OneOf{T0,T1,T2,TResult,T4}"/>.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapFunc"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">This instance does not represent a <typeparamref name="T0"/> instance.</exception>
    public OneOf<T0, T1, T2, T3, TResult> MapT4<TResult>(Func<T4?, TResult> mapFunc)
    {
        ArgumentNullExceptionThrower.ThrowIfNull(mapFunc);
        return this.Index switch
        {
            0 => this.AsT0,
            1 => this.AsT1,
            2 => this.AsT2,
            3 => this.AsT3,
            4 => mapFunc(this.AsT4),
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
    public bool TryPickT0(out T0? value, out OneOf<T1, T2, T3, T4> remainder)
    {
        value = this.IsT0 ? this.AsT0 : default;
        remainder = this.Index switch
        {
            0 => default,
            1 => this.AsT1,
            2 => this.AsT2,
            3 => this.AsT3,
            4 => this.AsT4,
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
    public bool TryPickT1(out T1? value, out OneOf<T0, T2, T3, T4> remainder)
    {
        value = this.IsT1 ? this.AsT1 : default;
        remainder = this.Index switch
        {
            0 => this.AsT0,
            1 => default,
            2 => this.AsT2,
            3 => this.AsT3,
            4 => this.AsT4,
            _ => throw new InvalidOperationException(),
        };

        return this.IsT1;
    }

    /// <summary>
    /// Tries to pick the value as a <typeparamref name="T2"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="remainder">The remainder.</param>
    /// <returns><see langword="true"/> upon success; otherwise <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Index"/> is out of range.</exception>
    public bool TryPickT2(out T2? value, out OneOf<T0, T1, T3, T4> remainder)
    {
        value = this.IsT2 ? this.AsT2 : default;
        remainder = this.Index switch
        {
            0 => this.AsT0,
            1 => this.AsT1,
            2 => default,
            3 => this.AsT3,
            4 => this.AsT4,
            _ => throw new InvalidOperationException(),
        };

        return this.IsT2;
    }

    /// <summary>
    /// Tries to pick the value as a <typeparamref name="T2"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="remainder">The remainder.</param>
    /// <returns><see langword="true"/> upon success; otherwise <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Index"/> is out of range.</exception>
    public bool TryPickT3(out T3? value, out OneOf<T0, T1, T2, T4> remainder)
    {
        value = this.IsT3 ? this.AsT3 : default;
        remainder = this.Index switch
        {
            0 => this.AsT0,
            1 => this.AsT1,
            2 => this.AsT2,
            3 => default,
            4 => this.AsT4,
            _ => throw new InvalidOperationException(),
        };

        return this.IsT3;
    }

    /// <summary>
    /// Tries to pick the value as a <typeparamref name="T2"/>.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <param name="remainder">The remainder.</param>
    /// <returns><see langword="true"/> upon success; otherwise <see langword="false"/>.</returns>
    /// <exception cref="InvalidOperationException"><see cref="Index"/> is out of range.</exception>
    public bool TryPickT4(out T4? value, out OneOf<T0, T1, T2, T3> remainder)
    {
        value = this.IsT4 ? this.AsT4 : default;
        remainder = this.Index switch
        {
            0 => this.AsT0,
            1 => this.AsT1,
            2 => this.AsT2,
            3 => this.AsT3,
            4 => default,
            _ => throw new InvalidOperationException(),
        };

        return this.IsT4;
    }

    /// <inheritdoc/>
    public bool Equals(OneOf<T0, T1, T2, T3, T4> other) =>
        this.Index == other.Index &&
        this.Index switch
        {
            0 => Equals(this.value0, other.value0),
            1 => Equals(this.value1, other.value1),
            2 => Equals(this.value2, other.value2),
            3 => Equals(this.value3, other.value3),
            4 => Equals(this.value4, other.value4),
            _ => false,
        };

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is OneOf<T0, T1, T2, T3, T4> o && this.Equals(o);

    /// <inheritdoc/>
    public override string? ToString() =>
        this.Index switch
        {
            0 => FormatValue(this.value0),
            1 => FormatValue(this.value1),
            2 => FormatValue(this.value2),
            3 => FormatValue(this.value3),
            4 => FormatValue(this.value4),
            _ => default,
        };

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        unchecked
        {
            return (GetHashCode(this.Index, this.value0, this.value1, this.value2, this.value3, this.value4) * 397) ^ this.Index;

            static int GetHashCode(int index, T0? value0, T1? value1, T2? value2, T3? value3, T4? value4)
            {
                var hashCode = index switch
                {
                    0 => value0?.GetHashCode(),
                    1 => value1?.GetHashCode(),
                    2 => value2?.GetHashCode(),
                    3 => value3?.GetHashCode(),
                    4 => value4?.GetHashCode(),
                    _ => default,
                };

                return hashCode ?? 0;
            }
        }
    }
}