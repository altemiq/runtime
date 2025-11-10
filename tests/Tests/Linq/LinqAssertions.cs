namespace Altemiq.Linq;

using TUnit.Assertions.Core;
using TUnit.Assertions.Sources;

public class LinqIndexOfWrapper<T>(AssertionContext<IEnumerable<T>> context, IEnumerable<T> other) : CollectionAssertionBase<IEnumerable<T>, T>(context)
    where T : IEquatable<T>
{
    public LinqIndexOfAssertion<T> EqualTo(int expectedIndex,
        bool readOnly = false,
        [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expectedIndex))]
        string? expression = null)
    {
        this.Context.ExpressionBuilder.Append($".EqualTo({expression})");
        return new(this.Context, other, expectedIndex, readOnly);
    }
}

public class LinqIndexOfAssertion<T>(AssertionContext<IEnumerable<T>> context, IEnumerable<T> other, int expectedIndex, bool readOnly) : CollectionAssertionBase<IEnumerable<T>, T>(context)
    where T : IEquatable<T>
{
    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<IEnumerable<T>> metadata)
    {
        if (metadata.Value is not { } value)
        {
            return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was null"));
        }

        if (readOnly)
        {
            if (value is IReadOnlyList<T> s)
            {
                if (other is IReadOnlyList<T> o)
                {
                    if (s.IndexOf(o) == expectedIndex)
                    {
                        return Task.FromResult(AssertionResult.Passed);
                    }
                }
                else
                {
                    return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IReadOnlyList<>)}"));
                }
            }
            else
            {
                return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was not of type {nameof(IReadOnlyList<>)}"));
            }
        }
        else if (value is IList<T> s)
        {
            if (other is IList<T> o)
            {
                if (s.IndexOf(o) == expectedIndex)
                {
                    return Task.FromResult(AssertionResult.Passed);
                }
            }
            else
            {
                return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IList<>)}"));
            }
        }
        else
        {
            return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was not of type {nameof(IList<>)}"));
        }

        return Task.FromResult(AssertionResult.Failed($"index of {other} withing {value} was not equal to {expectedIndex}"));
    }
}

public class LinqIndexOfAnyWrapper<T>(AssertionContext<IEnumerable<T>> context, IEnumerable<T> other) : CollectionAssertionBase<IEnumerable<T>, T>(context)
    where T : IEquatable<T>
{
    public LinqIndexOfAnyAssertion<T> EqualTo(int expectedIndex,
        bool readOnly = false,
        [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expectedIndex))]
        string? expression = null)
    {
        this.Context.ExpressionBuilder.Append($".EqualTo({expression})");
        return new(this.Context, other, expectedIndex, readOnly);
    }
}

public class LinqIndexOfAnyAssertion<T>(AssertionContext<IEnumerable<T>> context, IEnumerable<T> other, int expectedIndex, bool readOnly) : CollectionAssertionBase<IEnumerable<T>, T>(context)
    where T : IEquatable<T>
{
    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<IEnumerable<T>> metadata)
    {
        if (metadata.Value is not { } value)
        {
            return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was null"));
        }

        if (readOnly)
        {
            if (value is IReadOnlyList<T> s)
            {
                if (other is IReadOnlyList<T> o)
                {
                    if (s.IndexOf(o) == expectedIndex)
                    {
                        return Task.FromResult(AssertionResult.Passed);
                    }
                }
                else
                {
                    return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IReadOnlyList<>)}"));
                }
            }
            else
            {
                return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was not of type {nameof(IReadOnlyList<>)}"));
            }
        }
        else if (value is IList<T> s)
        {
            if (other is IList<T> o)
            {
                if (s.IndexOf(o) == expectedIndex)
                {
                    return Task.FromResult(AssertionResult.Passed);
                }
            }
            else
            {
                return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IList<>)}"));
            }
        }
        else
        {
            return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was not of type {nameof(IList<>)}"));
        }

        return Task.FromResult(AssertionResult.Failed($"index of {other} withing {value} was not equal to {expectedIndex}"));
    }
}

public class LinqEqualsAssertion<T>(AssertionContext<IEnumerable<T>> context, IEnumerable<T> other, int sourceIndex, int otherIndex, int count, bool sourceReadOnly, bool otherReadOnly, bool result = true) : CollectionAssertionBase<IEnumerable<T>, T>(context)
    where T : IEquatable<T>
{
    protected override string GetExpectation() => result ? $"to be equal to {other}" : $"to not be equal to {other}";

    protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<IEnumerable<T>> metadata)
    {
        if (metadata.Value is not { } value)
        {
            return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was null"));
        }

        if (sourceReadOnly)
        {
            if (value is IReadOnlyList<T> s)
            {
                if (otherReadOnly)
                {
                    if (other is IReadOnlyList<T> o)
                    {
                        if (s.Equals(sourceIndex, o, otherIndex, GetCountReadOnlyListCount(count, o)) == result)
                        {
                            return Task.FromResult(AssertionResult.Passed);
                        }
                    }
                    else
                    {
                        return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IReadOnlyList<>)}"));
                    }
                }
                else if (other is IList<T> o)
                {
                    if (s.Equals(sourceIndex, o, otherIndex, GetCountListCount(count, o)) == result)
                    {
                        return Task.FromResult(AssertionResult.Passed);
                    }
                }
                else
                {
                    return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IList<>)}"));
                }
            }
            else
            {
                return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was not of type {nameof(IReadOnlyList<>)}"));
            }
        }
        else if (value is IList<T> s)
        {
            if (otherReadOnly)
            {
                if (other is IReadOnlyList<T> o)
                {
                    if (s.Equals(sourceIndex, o, otherIndex, GetCountReadOnlyListCount(count, o)) == result)
                    {
                        return Task.FromResult(AssertionResult.Passed);
                    }
                }
                else
                {
                    return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IReadOnlyList<>)}"));
                }
            }
            else if (other is IList<T> o)
            {
                if (s.Equals(sourceIndex, o, otherIndex, GetCountListCount(count, o)) == result)
                {
                    return Task.FromResult(AssertionResult.Passed);
                }
            }
            else
            {
                return Task.FromResult(AssertionResult.Failed($"{nameof(other)} was not of type {nameof(IList<>)}"));
            }
        }
        else
        {
            return Task.FromResult(AssertionResult.Failed($"{nameof(value)} was not of type {nameof(IList<>)}"));
        }

        return Task.FromResult(AssertionResult.Failed(result ? $"{value} was not equal to {other}" : $"{value} was equal to {other}"));

        static int GetCountListCount(int count, ICollection<T> source)
        {
            return count switch
            {
                < 0 => source.Count,
                _ => count,
            };
        }
        static int GetCountReadOnlyListCount(int count, IReadOnlyCollection<T> source)
        {
            return count switch
            {
                < 0 => source.Count,
                _ => count,
            };
        }
    }
}