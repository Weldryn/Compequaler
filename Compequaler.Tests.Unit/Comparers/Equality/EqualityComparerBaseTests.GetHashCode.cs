using Compequaler.Comparers.Equality;
using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash.Implementations;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class EqualityComparerBaseTests
	{
		public class GetHashCode
		{
			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForRefTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(new object()));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(1));
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
				var expected = System.Collections.Generic.EqualityComparer<object>.Default.GetHashCode(null);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
				Assert.Equal(expected, sut.GetHashCode(null));
			}

			[Fact]
			public void ShouldDelegateToImplOnNullWhenNotHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetHashCode(null));
			}

			[Fact]
			public void ShouldReturnOnNullWhenHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				var expected = System.Collections.Generic.EqualityComparer<int?>.Default.GetHashCode(null);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
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

				protected internal override int GetHashCodeCore(T obj)
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
