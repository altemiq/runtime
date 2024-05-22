// -----------------------------------------------------------------------
// <copyright file="ExceptionThrowerTests.cs" company="Altemiq">
// Copyright (c) Altemiq. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Altemiq;

public class ExceptionThrowerTests
{
    public class ArgumentNullException
    {
        [Fact]
#if !NET7_0_OR_GREATER
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'default' expression", Justification = "This is required for .NET 7.0")]
#endif
        public void ThrowOnNull() => Do<System.ArgumentNullException>(() => ArgumentNullExceptionThrower.ThrowIfNull(default(object)));

        [Fact]
        public void NotThrowOnNotNull() => Do<System.ArgumentNullException>(() => ArgumentNullExceptionThrower.ThrowIfNull(string.Empty), false);
    }

    public class ArgumentException
    {
        [Fact]
        public void ThrowOnNull() => Do<System.ArgumentException>(() => ArgumentExceptionThrower.ThrowIfNullOrEmpty(default));

        [Fact]
        public void ThrowIfEmpty() => Do<System.ArgumentException>(() => ArgumentExceptionThrower.ThrowIfNullOrEmpty(string.Empty));

        [Fact]
        public void ThrowIfEmptyOrWhiteSpace() => Do<System.ArgumentException>(() => ArgumentExceptionThrower.ThrowIfNullOrWhiteSpace("    "));

        [Fact]
        public void NotThrow() => Do<System.ArgumentException>(() => ArgumentExceptionThrower.ThrowIfNullOrEmpty(nameof(string.Empty)), false);
    }

    public class ArgumentOutOfRangeException
    {
#if !NET8_0_OR_GREATER
        [Theory]
        [InlineData(0, 1, 2, true)]
        [InlineData(1, 0, 2, false)]
        [Obsolete("This will be removed when the method is removed")]
        public void OnNotBetween(int value, int min, int max, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => ArgumentOutOfRangeExceptionThrower.ThrowIfNotBetween(value, min, max), @throw);
#endif

        [Theory]
        [MemberData(nameof(NegativeData))]
        public void OnNegative<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfNegative), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        public static TheoryData<object, bool> NegativeData() => new()
        {
            { (sbyte)-1, true },
            { default(sbyte), false },
            { (short)-1, true },
            { default(short), false },
            { -1, true },
            { default(int), false },
            { -1L, true },
            { default(long), false },
            { -1F, true },
            { default(float), false },
            { -1D, true },
            { default(double), false },
            { -1M, true },
            { default(decimal), false },
        };

        [Theory]
        [MemberData(nameof(NegativeOrZeroData))]
        public void OnNegativeOrZero<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfNegativeOrZero), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        public static TheoryData<object, bool> NegativeOrZeroData() => new()
        {
            { (sbyte)0, true },
            { (sbyte)-1, true },
            { (sbyte)1, false },
            { (short)0, true },
            { (short)-1, true },
            { (short)1, false },
            { 0, true },
            { -1, true },
            { 1, false },
            { 0L, true },
            { -1L, true },
            { 1L, false },
            { 0F, true },
            { -1F, true },
            { 1F, false },
            { 0D, true },
            { -1D, true },
            { 1D, false },
            { 0M, true },
            { -1M, true },
            { 1M, false },
        };

        [Theory]
        [MemberData(nameof(ZeroData))]
        public void OnZero<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfZero), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        public static TheoryData<object, bool> ZeroData() => new()
        {
            { (sbyte)0, true },
            { (sbyte)-1, false },
            { (sbyte)1, false },
            { (short)0, true },
            { (short)-1, false },
            { (short)1, false },
            { 0, true },
            { -1, false },
            { 1, false },
            { 0L, true },
            { -1L, false },
            { 1L, false },
            { 0F, true },
            { -1F, false },
            { 1F, false },
            { 0D, true },
            { -1D, false },
            { 1D, false },
            { 0M, true },
            { -1M, false },
            { 1M, false },
        };

        [Theory]
        [MemberData(nameof(LessThanData))]
        public void OnLessThan<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfLessThan), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static TheoryData<object, bool> LessThanData() => new()
{
            { (sbyte)-1, true },
            { (sbyte)1, false },
            { (short)-1, true },
            { (short)1, false },
            { -1, true },
            { 1, false },
            { -1L, true },
            { 1L, false },
            { -1F, true },
            { 1F, false },
            { -1D, true },
            { 1D, false },
            { -1M, true },
            { 1M, false },
        };

        [Theory]
        [MemberData(nameof(LessThanOrEqualData))]
        public void OnLessThanOrEqual<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfLessThanOrEqual), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static TheoryData<object, bool> LessThanOrEqualData() => new()
        {
            { (sbyte)-1, true },
            { (sbyte)0, true },
            { (sbyte)1, false },
            { (short)-1, true },
            { (short)0, true },
            { (short)1, false },
            { -1, true },
            { 0, true },
            { 1, false },
            { -1L, true },
            { 0L, true },
            { 1L, false },
            { -1F, true },
            { 0F, true },
            { 1F, false },
            { -1D, true },
            { 0D, true },
            { 1D, false },
            { -1M, true },
            { 0M, true },
            { 1M, false },
        };

        [Theory]
        [MemberData(nameof(GreaterThanData))]

        public void OnGreaterThan<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static TheoryData<object, bool> GreaterThanData() => new()
        {
            { (sbyte)-1, false },
            { (sbyte)1, true },
            { (short)-1, false },
            { (short)1, true },
            { -1, false },
            { 1, true },
            { -1L, false },
            { 1L, true },
            { -1F, false },
            { 1F, true },
            { -1D, false },
            { 1D, true },
            { -1M, false },
            { 1M, true },
        };

        [Theory]
        [MemberData(nameof(GreaterThanOrEqualData))]
        public void OnGreaterThanOrEqual<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThanOrEqual), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        public static TheoryData<object, bool> GreaterThanOrEqualData() => new()
        {
            { (sbyte)-1, false },
            { (sbyte)0, true },
            { (sbyte)1, true },
            { (short)-1, false },
            { (short)0, true },
            { (short)1, true },
            { -1, false },
            { 0, true },
            { 1, true },
            { -1L, false },
            { 0L, true },
            { 1L, true },
            { -1F, false },
            { 0F, true },
            { 1F, true },
            { -1D, false },
            { 0D, true },
            { 1D, true },
            { -1M, false },
            { 0M, true },
            { 1M, true },
        };

        [Fact]
        public void OnValid() => Do<System.ArgumentOutOfRangeException>(
            () =>
            {
                ArgumentOutOfRangeExceptionThrower.ThrowIfLessThan(1, 0);
                ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan(1, 2);
            },
            false);

        [Theory]
        [InlineData(1, true)]
        [InlineData(-1, false)]
        public void OnEqual(int value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => ArgumentOutOfRangeExceptionThrower.ThrowIfEqual(value, 1), @throw);

        [Theory]
        [InlineData(1, false)]
        [InlineData(-1, true)]
        public void OnNotEqual(int value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => ArgumentOutOfRangeExceptionThrower.ThrowIfNotEqual(value, 1), @throw);

        private static System.Reflection.MethodInfo GetMethod<T>(string name, Type type)
        {
            var method = type.GetMethod(name, [typeof(T), typeof(string)])
                ?? type.GetMethod(name)
                ?? throw new InvalidOperationException();
            if (method.IsGenericMethod)
            {
                method = method.MakeGenericMethod(typeof(T));
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

        private static void Run<T>(string name, Type type, params T[] values) => Run(GetMethod<T>(name, type), values);
    }

    public class ObjectDisposedException
    {
        [Fact]
        public void ThrowOnNull() => Do<System.ObjectDisposedException>(
            () =>
            {
                object? @null = default;
                ObjectDisposedExceptionThrower.ThrowIf(@null is null, @null);
            });

        [Fact]
        public void ThrowOnNullWithType() => Do<System.ObjectDisposedException>(
            () =>
            {
                string? @null = default;
                ObjectDisposedExceptionThrower.ThrowIf(@null is null, typeof(string));
            });

        [Fact]
        public void ThrowOnNotNull() => Do<System.ObjectDisposedException>(
            () =>
            {
                var @null = new object();
                ObjectDisposedExceptionThrower.ThrowIf(@null is not null, @null!);
            });

        [Fact]
        public void ThrowOnNotNullWithType() => Do<System.ObjectDisposedException>(
            () =>
            {
                var @null = string.Empty;
                ObjectDisposedExceptionThrower.ThrowIf(@null is not null, default!);
            });

        [Fact]
        public void NotThrowNotNull() => Do<System.ObjectDisposedException>(
            () =>
            {
                var notNull = new object();
                ObjectDisposedExceptionThrower.ThrowIf(notNull is null, notNull);
            },
            false);

        [Fact]
        public void NotThrowNotNullWithType() => Do<System.ObjectDisposedException>(
            () =>
            {
                var notNull = string.Empty;
                ObjectDisposedExceptionThrower.ThrowIf(notNull is null, typeof(string));
            },
            false);
    }

    private static void Do<T>(Action act, bool @throw = true)
        where T : Exception
    {
        var should = act.Should();
        if (@throw)
        {
            _ = should.Throw<T>();
        }
        else
        {
            _ = should.NotThrow<T>();
        }
    }
}