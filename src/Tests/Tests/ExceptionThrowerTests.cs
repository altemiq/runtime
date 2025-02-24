// -----------------------------------------------------------------------
// <copyright file="ExceptionThrowerTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

using TUnit.Assertions.AssertConditions.Throws;

public class ExceptionThrowerTests
{
    public class ArgumentNullException
    {
        [Test]
#if !NET7_0_OR_GREATER
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'default' expression", Justification = "This is required for .NET 7.0")]
#endif
        public Task ThrowOnNull() => Do<System.ArgumentNullException>(static () => ArgumentNullExceptionThrower.ThrowIfNull(default(object)));

        [Test]
        public Task NotThrowOnNotNull() => Do<System.ArgumentNullException>(static () => ArgumentNullExceptionThrower.ThrowIfNull(string.Empty), false);
    }

    public class ArgumentException
    {
        [Test]
        public Task ThrowOnNull() => Do<System.ArgumentNullException>(static () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(default));

        [Test]
        public Task ThrowIfEmpty() => Do<System.ArgumentException>(static () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(string.Empty));

        [Test]
        public Task ThrowIfEmptyOrWhiteSpace() => Do<System.ArgumentException>(static () => ArgumentExceptionThrower.ThrowIfNullOrWhiteSpace("    "));

        [Test]
        public Task NotThrow() => Do<System.ArgumentException>(static () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(nameof(string.Empty)), false);
    }

    public class ArgumentOutOfRangeException
    {
#if !NET8_0_OR_GREATER
        [Test]
        [Arguments(0, 1, 2, true)]
        [Arguments(1, 0, 2, false)]
        [Obsolete("This will be removed when the method is removed")]
        public Task OnNotBetween(int value, int min, int max, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => ArgumentOutOfRangeExceptionThrower.ThrowIfNotBetween(value, min, max), @throw);
#endif

        [Test]
        [MethodDataSource(nameof(NegativeData))]
        public Task OnNegative(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfNegative), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        public static IEnumerable<Func<(object, bool)>> NegativeData()
        {
            yield return () => ((sbyte)-1, true);
            yield return () => (default(sbyte), false);
            yield return () => ((short)-1, true);
            yield return () => (default(short), false);
            yield return () => (-1, true);
            yield return () => (default(int), false);
            yield return () => (-1L, true);
            yield return () => (default(long), false);
            yield return () => (-1F, true);
            yield return () => (default(float), false);
            yield return () => (-1D, true);
            yield return () => (default(double), false);
            yield return () => (-1M, true);
            yield return () => (default(decimal), false);
        }

        [Test]
        [MethodDataSource(nameof(NegativeOrZeroData))]
        public Task OnNegativeOrZero(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfNegativeOrZero), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        public static IEnumerable<Func<(object, bool)>> NegativeOrZeroData()
        {
            yield return () => ((sbyte)0, true);
            yield return () => ((sbyte)-1, true);
            yield return () => ((sbyte)1, false);
            yield return () => ((short)0, true);
            yield return () => ((short)-1, true);
            yield return () => ((short)1, false);
            yield return () => (0, true);
            yield return () => (-1, true);
            yield return () => (1, false);
            yield return () => (0L, true);
            yield return () => (-1L, true);
            yield return () => (1L, false);
            yield return () => (0F, true);
            yield return () => (-1F, true);
            yield return () => (1F, false);
            yield return () => (0D, true);
            yield return () => (-1D, true);
            yield return () => (1D, false);
            yield return () => (0M, true);
            yield return () => (-1M, true);
            yield return () => (1M, false);
        }

        [Test]
        [MethodDataSource(nameof(ZeroData))]
        public Task OnZero(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfZero), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        public static IEnumerable<Func<(object, bool)>> ZeroData()
        {
            yield return () => ((sbyte)0, true);
            yield return () => ((sbyte)-1, false);
            yield return () => ((sbyte)1, false);
            yield return () => ((short)0, true);
            yield return () => ((short)-1, false);
            yield return () => ((short)1, false);
            yield return () => (0, true);
            yield return () => (-1, false);
            yield return () => (1, false);
            yield return () => (0L, true);
            yield return () => (-1L, false);
            yield return () => (1L, false);
            yield return () => (0F, true);
            yield return () => (-1F, false);
            yield return () => (1F, false);
            yield return () => (0D, true);
            yield return () => (-1D, false);
            yield return () => (1D, false);
            yield return () => (0M, true);
            yield return () => (-1M, false);
            yield return () => (1M, false);
        }

        [Test]
        [MethodDataSource(nameof(LessThanData))]
        public Task OnLessThan(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfLessThan), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static IEnumerable<Func<(object, bool)>> LessThanData()
        {
            yield return () => ((sbyte)-1, true);
            yield return () => ((sbyte)1, false);
            yield return () => ((short)-1, true);
            yield return () => ((short)1, false);
            yield return () => (-1, true);
            yield return () => (1, false);
            yield return () => (-1L, true);
            yield return () => (1L, false);
            yield return () => (-1F, true);
            yield return () => (1F, false);
            yield return () => (-1D, true);
            yield return () => (1D, false);
            yield return () => (-1M, true);
            yield return () => (1M, false);
        }

        [Test]
        [MethodDataSource(nameof(LessThanOrEqualData))]
        public Task OnLessThanOrEqual(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfLessThanOrEqual), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static IEnumerable<Func<(object, bool)>> LessThanOrEqualData()
        {
            yield return () => ((sbyte)-1, true);
            yield return () => ((sbyte)0, true);
            yield return () => ((sbyte)1, false);
            yield return () => ((short)-1, true);
            yield return () => ((short)0, true);
            yield return () => ((short)1, false);
            yield return () => (-1, true);
            yield return () => (0, true);
            yield return () => (1, false);
            yield return () => (-1L, true);
            yield return () => (0L, true);
            yield return () => (1L, false);
            yield return () => (-1F, true);
            yield return () => (0F, true);
            yield return () => (1F, false);
            yield return () => (-1D, true);
            yield return () => (0D, true);
            yield return () => (1D, false);
            yield return () => (-1M, true);
            yield return () => (0M, true);
            yield return () => (1M, false);
        }

        [Test]
        [MethodDataSource(nameof(GreaterThanData))]
        public Task OnGreaterThan(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static IEnumerable<Func<(object, bool)>> GreaterThanData()
        {
            yield return () => ((sbyte)-1, false);
            yield return () => ((sbyte)1, true);
            yield return () => ((short)-1, false);
            yield return () => ((short)1, true);
            yield return () => (-1, false);
            yield return () => (1, true);
            yield return () => (-1L, false);
            yield return () => (1L, true);
            yield return () => (-1F, false);
            yield return () => (1F, true);
            yield return () => (-1D, false);
            yield return () => (1D, true);
            yield return () => (-1M, false);
            yield return () => (1M, true);
        }

        [Test]
        [MethodDataSource(nameof(GreaterThanOrEqualData))]
        public Task OnGreaterThanOrEqual(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThanOrEqual), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static IEnumerable<Func<(object, bool)>> GreaterThanOrEqualData()
        {
            yield return () => ((sbyte)-1, false);
            yield return () => ((sbyte)0, true);
            yield return () => ((sbyte)1, true);
            yield return () => ((short)-1, false);
            yield return () => ((short)0, true);
            yield return () => ((short)1, true);
            yield return () => (-1, false);
            yield return () => (0, true);
            yield return () => (1, true);
            yield return () => (-1L, false);
            yield return () => (0L, true);
            yield return () => (1L, true);
            yield return () => (-1F, false);
            yield return () => (0F, true);
            yield return () => (1F, true);
            yield return () => (-1D, false);
            yield return () => (0D, true);
            yield return () => (1D, true);
            yield return () => (-1M, false);
            yield return () => (0M, true);
            yield return () => (1M, true);
        }

        [Test]
        public Task OnValid() => Do<System.ArgumentOutOfRangeException>(
            static () =>
            {
                ArgumentOutOfRangeExceptionThrower.ThrowIfLessThan(1, 0);
                ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan(1, 2);
            },
            false);

        [Test]
        [Arguments(1, true)]
        [Arguments(-1, false)]
        public Task OnEqual(int value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => ArgumentOutOfRangeExceptionThrower.ThrowIfEqual(value, 1), @throw);

        [Test]
        [Arguments(1, false)]
        [Arguments(-1, true)]
        public Task OnNotEqual(int value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => ArgumentOutOfRangeExceptionThrower.ThrowIfNotEqual(value, 1), @throw);

        private static System.Reflection.MethodInfo GetMethod(string name, Type type, Type parameterType)
        {
            var method = type.GetMethod(name, [parameterType, typeof(string)])
                ?? type.GetMethod(name)
                ?? throw new InvalidOperationException();
            if (method.IsGenericMethod)
            {
                method = method.MakeGenericMethod(parameterType);
            }

            return method;
        }

        private static void Run<T>(System.Reflection.MethodInfo methodInfo, params T[] values)
        {
            try
            {
                methodInfo.Invoke(null, [.. values, nameof(values)]);
            }
            catch (System.Reflection.TargetInvocationException ex) when (ex.InnerException is not null)
            {
                throw ex.InnerException;
            }
        }

        private static void Run<T>(string name, Type type, params T[] values)
        {
            Run(GetMethod(name, type, GetValuesType(values)), values);
            
            static Type GetValuesType(T[] values)
            {
                if (values is not null && values.FirstOrDefault(x => x is not null) is { } first)
                {
                    return first.GetType();
                }

                return typeof(T);
            }
        }
    }

    public class ObjectDisposedException
    {
        [Test]
        public Task ThrowOnNull() => Do<System.ObjectDisposedException>(
            static () =>
            {
                object? @null = default;
                ObjectDisposedExceptionThrower.ThrowIf(@null is null, @null);
            });

        [Test]
        public Task ThrowOnNullWithType() => Do<System.ObjectDisposedException>(
            static () =>
            {
                string? @null = default;
                ObjectDisposedExceptionThrower.ThrowIf(@null is null, typeof(string));
            });

        [Test]
        public Task ThrowOnNotNull() => Do<System.ObjectDisposedException>(
            static () =>
            {
                var @null = new object();
                ObjectDisposedExceptionThrower.ThrowIf(@null is not null, @null!);
            });

        [Test]
        public Task ThrowOnNotNullWithType() => Do<System.ObjectDisposedException>(
            static () =>
            {
                var @null = string.Empty;
                ObjectDisposedExceptionThrower.ThrowIf(@null is not null, default!);
            });

        [Test]
        public Task NotThrowNotNull() => Do<System.ObjectDisposedException>(
            static () =>
            {
                var notNull = new object();
                ObjectDisposedExceptionThrower.ThrowIf(notNull is null, notNull);
            },
            false);

        [Test]
        public Task NotThrowNotNullWithType() => Do<System.ObjectDisposedException>(
            static () =>
            {
                var notNull = string.Empty;
                ObjectDisposedExceptionThrower.ThrowIf(notNull is null, typeof(string));
            },
            false);
    }

    private static async Task Do<T>(Action act, bool @throw = true)
        where T : Exception
    {
        if (@throw)
        {
            await Assert.That(act).Throws<T>();
        }
        else
        {
            act();
        }
    }
}