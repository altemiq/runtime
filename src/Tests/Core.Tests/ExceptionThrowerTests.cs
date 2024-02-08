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
        public void ThrowOnNull()
        {
            var act = () => ArgumentNullExceptionThrower.ThrowIfNull(default(object));
            _ = act.Should().Throw<System.ArgumentNullException>();
        }

        [Fact]
        public void NotThrowOnNotNull()
        {
            var act = () => ArgumentNullExceptionThrower.ThrowIfNull(string.Empty);
            _ = act.Should().NotThrow<System.ArgumentNullException>();
        }
    }

    public class ArgumentException
    {
        [Fact]
        public void ThrowOnNull()
        {
            var act = () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(default);
            _ = act.Should().Throw<System.ArgumentException>();
        }

        [Fact]
        public void ThrowIfEmpty()
        {
            var act = () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(string.Empty);
            _ = act.Should().Throw<System.ArgumentException>();
        }

        [Fact]
        public void NotThrow()
        {
            var act = () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(nameof(string.Empty));
            _ = act.Should().NotThrow<System.ArgumentException>();
        }
    }

    public class ArgumentOutOfRangeException
    {
        [Fact]
        public void ThrowIfNegative()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfNegative(-1);
            _ = act.Should().Throw<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ThrowOnTooLow()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfLessThan(-1, 0);
            _ = act.Should().Throw<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ThrowOnTooHigh()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan(2, 0);
            _ = act.Should().Throw<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void NotThrowIfPositive()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfNegative(1);
            _ = act.Should().NotThrow<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void NotThrowIfValid()
        {
            var act = () =>
            {
                ArgumentOutOfRangeExceptionThrower.ThrowIfLessThan(1, 0);
                ArgumentOutOfRangeExceptionThrower.ThrowIfGreaterThan(1, 2);
            };
            _ = act.Should().NotThrow<System.ArgumentOutOfRangeException>();
        }
    }
}