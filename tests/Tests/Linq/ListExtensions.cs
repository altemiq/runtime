using TUnit.Assertions.Sources;

namespace Altemiq.Linq;

using TUnit.Assertions.Core;

public static partial class ListExtensions
{
    public static CollectionAssertionBase<IEnumerable<T>, T> IsEqualTo<T>(
        this CollectionAssertionBase<IEnumerable<T>, T> first,
        int firstIndex,
        IEnumerable<T> second,
        int secondIndex = default,
        int count = -1,
        bool firstReadOnly = false,
        bool secondReadOnly = false,
        [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(second))] string? expression = null)
        where T : IEquatable<T>
    {
        if (first is IAssertionSource<IEnumerable<T>> { Context: { } context })
        {
            context.ExpressionBuilder.Append($".IsLinqEqualTo({expression})");
            return new LinqEqualsAssertion<T>(
                context,
                second,
                firstIndex,
                secondIndex,
                count,
                firstReadOnly,
                secondReadOnly);
        }

        return first;
    }

    public static CollectionAssertionBase<IEnumerable<T>, T> IsNotEqualTo<T>(
        this CollectionAssertionBase<IEnumerable<T>, T> first,
        int firstIndex,
        IEnumerable<T> second,
        int secondIndex = default,
        int count = -1,
        bool firstReadOnly = false,
        bool secondReadOnly = false,
        [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(second))] string? expression = null)
        where T : IEquatable<T>
    {
        if (first is IAssertionSource<IEnumerable<T>> { Context: { } context })
        {
            context.ExpressionBuilder.Append($".IsLinqEqualTo({expression})");
            return new LinqEqualsAssertion<T>(
                context,
                second,
                firstIndex,
                secondIndex,
                count,
                firstReadOnly,
                secondReadOnly,
                false);
        }

        return first;
    }

    public static LinqIndexOfWrapper<T> HasIndexOf<T>(this CollectionAssertionBase<IEnumerable<T>, T> source, IEnumerable<T> value)
        where T : IEquatable<T>
    {
        if (source is IAssertionSource<IEnumerable<T>> { Context: { } context })
        {
            context.ExpressionBuilder.Append(".HasIndexOf()");
            return new(context, value);
        }

        throw new InvalidOperationException();
    }

    public static LinqIndexOfAnyWrapper<T> HasIndexOfAny<T>(this CollectionAssertionBase<IEnumerable<T>, T> source, IEnumerable<T> value)
        where T : IEquatable<T>
    {
        if (source is IAssertionSource<IEnumerable<T>> { Context: { } context })
        {
            context.ExpressionBuilder.Append(".HasIndexOf()");
            return new(context, value);
        }

        throw new InvalidOperationException();
    }

}