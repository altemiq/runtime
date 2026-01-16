using TUnit.Assertions.Core;
using TUnit.Assertions.Sources;

namespace Altemiq.Buffers.Compression;

public static class BufferExtensions
{
    public static CollectionAssertionBase<TCollection, TItem> HasSameSequenceAs<TCollection, TItem>(
        this CollectionAssertionBase<TCollection, TItem> collection,
        TCollection expected,
        [System.Runtime.CompilerServices.CallerArgumentExpression(nameof(expected))] string? expression = null)
        where TCollection : IEnumerable<TItem>
    {
        if (collection is not IAssertionSource<TCollection> { Context: { } context })
        {
            return collection;
        }

        context.ExpressionBuilder.Append($".HasSameSequenceAs({expression})");
        return new SequenceEqualsAssertion<TCollection, TItem>(context, expected);

    }

    private sealed class SequenceEqualsAssertion<TCollection, TItem>(AssertionContext<TCollection> context, TCollection expected) : CollectionAssertionBase<TCollection, TItem>(context)
        where TCollection : IEnumerable<TItem>
    {
        protected override Task<AssertionResult> CheckAsync(EvaluationMetadata<TCollection> metadata)
        {
            var result = metadata.Value switch
            {
                null => AssertionResult.Failed($"{metadata.Value} cannot be null"),
                { } value when value.SequenceEqual(expected) => AssertionResult.Passed,
                { } value => AssertionResult.Failed($"{Format(expected)} is not equivalent to {Format(value)}"),
            };

            return Task.FromResult(result);

            static string Format(IEnumerable<TItem> items)
            {
                return $"[{string.Join(", ", items)}]";
            }
        }
    }
}