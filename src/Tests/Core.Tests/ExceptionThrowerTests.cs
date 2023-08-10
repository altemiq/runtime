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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'default' expression", Justification = "This is required for .NET 7.0")]
        public void ThrowOnNull()
        {
            var act = () => ArgumentNullExceptionThrower.ThrowIfNull(default(object));
            act.Should().Throw<System.ArgumentNullException>();
        }

        [Fact]
        public void NotThrowOnNotNull()
        {
            var act = () => ArgumentNullExceptionThrower.ThrowIfNull(string.Empty);
            act.Should().NotThrow<System.ArgumentNullException>();
        }
    }

    public class ArgumentException
    {
        [Fact]
        public void ThrowOnNull()
        {
            var act = () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(default);
            act.Should().Throw<System.ArgumentException>();
        }

        [Fact]
        public void ThrowIfEmpty()
        {
            var act = () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(string.Empty);
            act.Should().Throw<System.ArgumentException>();
        }

        [Fact]
        public void NotThrow()
        {
            var act = () => ArgumentExceptionThrower.ThrowIfNullOrEmpty(nameof(string.Empty));
            act.Should().NotThrow<System.ArgumentException>();
        }
    }

    public class ArgumentOutOfRangeException
    {
        [Fact]
        public void ThrowOnLessThanZero()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfLessThanZero(-1);
            act.Should().Throw<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ThrowOnTooLow()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfNotBetween(-1, 0, 1);
            act.Should().Throw<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void ThrowOnTooHigh()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfNotBetween(2, 0, 1);
            act.Should().Throw<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void NotThrowIfPositive()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfLessThanZero(1);
            act.Should().NotThrow<System.ArgumentOutOfRangeException>();
        }

        [Fact]
        public void NotThrowIfValid()
        {
            var act = () => ArgumentOutOfRangeExceptionThrower.ThrowIfNotBetween(1, 0, 2);
            act.Should().NotThrow<System.ArgumentOutOfRangeException>();
        }
    }
}