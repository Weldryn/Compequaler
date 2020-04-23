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
		public class GetHashCode
		{
			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForRefTypes(bool handleNulls, object model)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(model));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForValTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(1));
			}

			[Fact]
			public void ShouldDelegateToImplOnNullWhenNotHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<object>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(null));
			}

			[Fact]
			public void ShouldReturnOnNullWhenHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<object>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				var expected = Hasher.Seed.Hash<object>(null).GetHashCode();
				Assert.Equal(expected, sut.GetHashCode(null));
			}

			private class TestableEqualityComparerBase<T> : EqualityComparerBase<T>
			{
				public TestableEqualityComparerBase(
					bool shouldHandleNulls)
					: base(shouldHandleNulls)
				{
				}

				public Func<T, int> GetHashCodeCoreImpl { get; set; }

				protected override int GetHashCodeCore(T obj)
					=> GetHashCodeCoreImpl?.Invoke(obj) ?? base.GetHashCodeCore(obj);

				public IEqualityComparer<T> AsExplicitlyTyped() => this;

				public TestableEqualityComparerBase<T> ThrowsOnImplCall(Func<Exception> createException)
				{
					GetHashCodeCoreImpl = x => throw createException();
					return this;
				}
			}
		}
	}
}
