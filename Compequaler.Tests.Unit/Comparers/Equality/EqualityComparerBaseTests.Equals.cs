using Compequaler.Comparers.Equality;
using Compequaler.Comparers.Equality.Implementations;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class EqualityComparerBaseTests
	{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
		public class Equals
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
		{
			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForSameValueTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 1));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForSameNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 1));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForDifferentValueTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 2));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForDifferentNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 2));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForDifferentRefTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(new object(), new object()));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldNotDelegateToImplAndEquateForSameRefTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				var reference = new object();
				Assert.True(sut.Equals(reference, reference));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldEquateOnNullsForRefTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				Assert.True(sut.Equals(null, null));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldEquateOnNullsForNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				Assert.True(sut.Equals(null, null));
			}

			[Fact]
			public void ShouldNotEquateOnSingleNullWhenHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<object>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				var reference = new object();
				Assert.False(sut.Equals(null, reference));
				Assert.False(sut.Equals(reference, null));
			}

			[Fact]
			public void ShouldNotEquateOnSingleNullWhenHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<object>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				Assert.False(sut.Equals(null, 1));
				Assert.False(sut.Equals(1, null));
			}

			[Fact]
			public void ShouldDelegateToImplOnSingleNullWhenNotHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<object>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				var reference = new object();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(null, reference));
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(reference, null));
			}

			private class TestableEqualityComparerBase<T> : EqualityComparerBase<T>
			{
				public TestableEqualityComparerBase(
					bool shouldHandleNulls)
					: base(shouldHandleNulls)
				{
				}

				public Func<T, T, bool> EqualsCoreImpl { get; set; }

				protected internal override bool EqualsCore(T x, T y)
					=> EqualsCoreImpl?.Invoke(x, y) ?? base.EqualsCore(x, y);

				public IEqualityComparer<T> AsExplicitlyTyped() => this;

				public TestableEqualityComparerBase<T> ThrowsOnImplCall(Func<Exception> createException)
				{
					EqualsCoreImpl = (x, y) => throw createException();
					return this;
				}
			}
		}
	}
}
