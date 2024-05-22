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
        [InlineData((sbyte)-1, true)]
        [InlineData(default(sbyte), false)]
        [InlineData((short)-1, true)]
        [InlineData(default(short), false)]
        [InlineData(-1, true)]
        [InlineData(default(int), false)]
        [InlineData(-1L, true)]
        [InlineData(default(long), false)]
        [InlineData(-1F, true)]
        [InlineData(default(float), false)]
        [InlineData(-1D, true)]
        [InlineData(default(double), false)]
        public void OnNegative<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfNegative), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        [Theory]
        [InlineData((sbyte)0, true)]
        [InlineData((sbyte)-1, true)]
        [InlineData((sbyte)1, false)]
        [InlineData((short)0, true)]
        [InlineData((short)-1, true)]
        [InlineData((short)1, false)]
        [InlineData(0, true)]
        [InlineData(-1, true)]
        [InlineData(1, false)]
        [InlineData((long)0L, true)]
        [InlineData((long)-1L, true)]
        [InlineData((long)1L, false)]
        [InlineData((float)0F, true)]
        [InlineData((float)-1F, true)]
        [InlineData((float)1F, false)]
        [InlineData((double)0D, true)]
        [InlineData((double)-1D, true)]
        [InlineData((double)1D, false)]
        public void OnNegativeOrZero<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfNegativeOrZero), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        [Theory]
        [InlineData((sbyte)0, true)]
        [InlineData((sbyte)-1, false)]
        [InlineData((sbyte)1, false)]
        [InlineData((short)0, true)]
        [InlineData((short)-1, false)]
        [InlineData((short)1, false)]
        [InlineData(0, true)]
        [InlineData(-1, false)]
        [InlineData(1, false)]
        [InlineData((long)0L, true)]
        [InlineData((long)-1L, false)]
        [InlineData((long)1L, false)]
        [InlineData((float)0F, true)]
        [InlineData((float)-1F, false)]
        [InlineData((float)1F, false)]
        [InlineData((double)0D, true)]
        [InlineData((double)-1D, false)]
        [InlineData((double)1D, false)]
        public void OnZero<T>(T value, bool @throw) => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfZero), typeof(ArgumentOutOfRangeExceptionThrower), value), @throw);

        [Theory]
        [InlineData((sbyte)-1, true)]
        [InlineData((sbyte)1, false)]
        [InlineData((short)-1, true)]
        [InlineData((short)1, false)]
        [InlineData(-1, true)]
        [InlineData(1, false)]
        [InlineData(-1L, true)]
        [InlineData(1L, false)]
        [InlineData(-1F, true)]
        [InlineData(1F, false)]
        [InlineData(-1D, true)]
        [InlineData(1D, false)]
        public void OnLessThan<T>(T value, bool @throw)
            where T : struct => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfLessThan), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        [Theory]
        [InlineData((sbyte)-1, true)]
        [InlineData((sbyte)0, true)]
        [InlineData((sbyte)1, false)]
        [InlineData((short)-1, true)]
        [InlineData((short)0, true)]
        [InlineData((short)1, false)]
        [InlineData(-1, true)]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(-1L, true)]
        [InlineData(0L, true)]
        [InlineData(1L, false)]
        [InlineData(-1F, true)]
        [InlineData(0F, true)]
        [InlineData(1F, false)]
        [InlineData(-1D, true)]
        [InlineData(0D, true)]
        [InlineData(1D, false)]
        public void OnLessThanOrEqual<T>(T value, bool @throw)
            where T : struct => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfLessThanOrEqual), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        [Theory]
        [InlineData((sbyte)-1, false)]
        [InlineData((sbyte)1, true)]
        [InlineData((short)-1, false)]
        [InlineData((short)1, true)]
        [InlineData(-1, false)]
        [InlineData(1, true)]
        [InlineData(-1L, false)]
        [InlineData(1L, true)]
        [InlineData(-1F, false)]
        [InlineData(1F, true)]
        [InlineData(-1D, false)]
        [InlineData(1D, true)]
        public void OnGreaterThan<T>(T value, bool @throw)
            where T : struct => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

        [Theory]
        [InlineData((sbyte)-1, false)]
        [InlineData((sbyte)0, true)]
        [InlineData((sbyte)1, true)]
        [InlineData((short)-1, false)]
        [InlineData((short)0, true)]
        [InlineData((short)1, true)]
        [InlineData(-1, false)]
        [InlineData(0, true)]
        [InlineData(1, true)]
        [InlineData(-1L, false)]
        [InlineData(0L, true)]
        [InlineData(1L, true)]
        [InlineData(-1F, false)]
        [InlineData(0F, true)]
        [InlineData(1F, true)]
        [InlineData(-1D, false)]
        [InlineData(0D, true)]
        [InlineData(1D, true)]
        public void OnGreaterThanOrEqual<T>(T value, bool @throw)
            where T : struct => Do<System.ArgumentOutOfRangeException>(() => Run(nameof(ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThanOrEqual), typeof(ArgumentOutOfRangeExceptionThrower), value, default), @throw);

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
        public void NotThrowNotNull() => Do<System.ObjectDisposedException>(
            () =>
            {
                var notNull = new object();
                ObjectDisposedExceptionThrower.ThrowIf(notNull is null, notNull);
            },
            false);
    }

    private static void Do<T>(Action act, bool @throw = true)
        where T : System.Exception
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