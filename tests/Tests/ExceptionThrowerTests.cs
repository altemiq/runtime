// -----------------------------------------------------------------------
// <copyright file="ExceptionThrowerTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public static class ExceptionThrowerTests
{
    public class ArgumentNullException
    {
        [Test]
#if !NET7_0_OR_GREATER
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'default' expression", Justification = "This is required for .NET 7.0")]
#endif
        public Task ThrowOnNull() => Do<System.ArgumentNullException>(static () => System.ArgumentNullException.ThrowIfNull(default(object)));

        [Test]
        public Task NotThrowOnNotNull() => Do<System.ArgumentNullException>(static () => System.ArgumentNullException.ThrowIfNull(string.Empty), false);
    }

    public class ArgumentException
    {
        [Test]
        public Task ThrowOnNull() => Do<System.ArgumentNullException>(static () => System.ArgumentException.ThrowIfNullOrEmpty(default));

        [Test]
        public Task ThrowIfEmpty() => Do<System.ArgumentException>(static () => System.ArgumentException.ThrowIfNullOrEmpty(string.Empty));

        [Test]
        public Task ThrowIfEmptyOrWhiteSpace() => Do<System.ArgumentException>(static () => System.ArgumentException.ThrowIfNullOrWhiteSpace("    "));

        [Test]
        public Task NotThrow() => Do<System.ArgumentException>(static () => System.ArgumentException.ThrowIfNullOrEmpty(nameof(string.Empty)), false);
    }

    public class ArgumentOutOfRangeException
    {

        [Test]
        [MethodDataSource(nameof(NegativeData))]
        public Task OnNegative(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() =>
            Run(
                "ThrowIfNegative",
#if NET8_0_OR_GREATER
                typeof(System.ArgumentOutOfRangeException),
#else
                typeof(ArgumentExceptionExtensions),
#endif
                value),
            @throw);

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
        public Task OnNegativeOrZero(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() =>
            Run(
                "ThrowIfNegativeOrZero",
#if NET8_0_OR_GREATER
                typeof(System.ArgumentOutOfRangeException),
#else
                typeof(ArgumentExceptionExtensions),
#endif
                value),
            @throw);

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
        public Task OnZero(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() =>
            Run(
                "ThrowIfZero",
#if NET8_0_OR_GREATER
                typeof(System.ArgumentOutOfRangeException),
#else
                typeof(ArgumentExceptionExtensions),
#endif
                value),
            @throw);

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
        public Task OnLessThan(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() =>
            Run(
                "ThrowIfLessThan",
#if NET8_0_OR_GREATER
                typeof(System.ArgumentOutOfRangeException),
#else
                typeof(ArgumentExceptionExtensions),
#endif
                value,
                default),
            @throw);

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
        public Task OnLessThanOrEqual(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() =>
            Run(
                "ThrowIfLessThanOrEqual",
#if NET8_0_OR_GREATER
                typeof(System.ArgumentOutOfRangeException),
#else
                typeof(ArgumentExceptionExtensions),
#endif
                value,
                default),
            @throw);

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
        public Task OnGreaterThan(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() =>
            Run(
                "ThrowIfGreaterThan",
#if NET8_0_OR_GREATER
                typeof(System.ArgumentOutOfRangeException),
#else
                typeof(ArgumentExceptionExtensions),
#endif
                value,
                default),
            @throw);

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
        public Task OnGreaterThanOrEqual(object value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() =>
            Run(
                "ThrowIfGreaterThanOrEqual",
#if NET8_0_OR_GREATER
                typeof(System.ArgumentOutOfRangeException),
#else
                typeof(ArgumentExceptionExtensions),
#endif
                value,
                default),
            @throw);

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
                System.ArgumentOutOfRangeException.ThrowIfLessThan(1, 0);
                System.ArgumentOutOfRangeException.ThrowIfGreaterThan(1, 2);
            },
            false);

        [Test]
        [Arguments(1, true)]
        [Arguments(-1, false)]
        public Task OnEqual(int value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => System.ArgumentOutOfRangeException.ThrowIfEqual(value, 1), @throw);

        [Test]
        [Arguments(1, false)]
        [Arguments(-1, true)]
        public Task OnNotEqual(int value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => System.ArgumentOutOfRangeException.ThrowIfNotEqual(value, 1), @throw);

        private static System.Reflection.MethodInfo GetMethod(string name, Type type, Type parameterType)
        {
            var method = type.GetMethod(name, [parameterType, typeof(string)])
                ?? type.GetMethod(name)
                ?? throw new InvalidOperationException();
            return method.IsGenericMethod
                ? method.MakeGenericMethod(parameterType)
                : method;
        }

        private static void Run<T>(System.Reflection.MethodInfo methodInfo, params IEnumerable<T>? values)
        {
            try
            {
                if (values is null)
                {
                    methodInfo.Invoke(null, [values, nameof(values)]);
                }
                else
                {
                    methodInfo.Invoke(null, [.. values, nameof(values)]);
                }
            }
            catch (System.Reflection.TargetInvocationException ex) when (ex.InnerException is not null)
            {
                throw ex.InnerException;
            }
        }

        private static void Run<T>(string name, Type type, params ICollection<T?>? values)
        {
            Run(GetMethod(name, type, GetValuesType(values)), values);

            static Type GetValuesType(IEnumerable<T?>? values)
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
                System.ObjectDisposedException.ThrowIf(@null is null, @null);
            });

        [Test]
        public Task ThrowOnNullWithType() => Do<System.ObjectDisposedException>(
            static () =>
            {
                string? @null = default;
                System.ObjectDisposedException.ThrowIf(@null is null, typeof(string));
            });

        [Test]
        public Task ThrowOnNotNull() => Do<System.ObjectDisposedException>(static () => System.ObjectDisposedException.ThrowIf(true, new object()));

        [Test]
        public Task ThrowOnNotNullWithType() => Do<System.ObjectDisposedException>(static () => System.ObjectDisposedException.ThrowIf(true, typeof(string)));

        [Test]
        public Task NotThrowNotNull() => Do<System.ObjectDisposedException>(
            static () =>
            {
                var notNull = new object();
                System.ObjectDisposedException.ThrowIf(notNull is null, notNull);
            },
            false);

        [Test]
        public Task NotThrowNotNullWithType() => Do<System.ObjectDisposedException>(
            static () =>
            {
                var notNull = string.Empty;
                System.ObjectDisposedException.ThrowIf(notNull is null, typeof(string));
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