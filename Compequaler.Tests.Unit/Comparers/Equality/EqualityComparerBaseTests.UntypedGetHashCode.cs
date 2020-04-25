using AutoFixture.Xunit2;
using Compequaler.Comparers.Equality;
using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class EqualityComparerBaseTests
	{
		public class UntypedGetHashCode
		{
			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForRefTypes(bool handleNulls, Version model)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(model));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForValTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(1));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldReturnOnNullsForValTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				var expected = Hasher.Seed.Hash<object>(null).GetHashCode();
				Assert.Equal(expected, sut.GetHashCode(null));
			}

			[Fact]
			public void ShouldDelegateToImplOnNullWhenNotHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<Version>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(null));
			}

			[Fact]
			public void ShouldReturnOnNullWhenHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<Version>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				var expected = Hasher.Seed.Hash<Version>(null).GetHashCode();
				Assert.Equal(expected, sut.GetHashCode(null));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldThrowOnWrongTypeForRefTypes(bool handleNulls, Uri model)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<ArgumentException>(() => sut.GetHashCode(model));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldThrowOnWrongTypeForValTypes(bool handleNulls, double model)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<ArgumentException>(() => sut.GetHashCode(model));
			}

			private class TestableEqualityComparerBase<T> : EqualityComparerBase<T>
			{
				public TestableEqualityComparerBase(
					bool shouldHandleNulls)
					: base(shouldHandleNulls)
				{
				}

				public Func<T, int> GetHashCodeCoreImpl { get; set; }

				protected internal override int GetHashCodeCore(T obj)
					=> GetHashCodeCoreImpl?.Invoke(obj) ?? base.GetHashCodeCore(obj);

				public IEqualityComparer AsExplicitlyUntyped() => this;

				public TestableEqualityComparerBase<T> ThrowsOnImplCall(Func<Exception> createException)
				{
					GetHashCodeCoreImpl = x => throw createException();
					return this;
				}
			}
		}
	}
}
