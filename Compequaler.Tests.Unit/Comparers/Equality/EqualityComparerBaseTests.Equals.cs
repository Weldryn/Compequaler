using AutoFixture.Xunit2;
using Compequaler.Comparers.Equality;
using Compequaler.Comparers.Equality.Implementations;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class EqualityComparerBaseTests
	{
		public class Equals
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
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForDifferentValueTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 2));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForDifferentRefTypes(
				bool handleNulls,
				object model1,
				object model2)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(model1, model2));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldNotDelegateToImplAndEquateForSameRefTypes(bool handleNulls, object model)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				Assert.True(sut.Equals(model, model));
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
			[AutoData]
			public void ShouldNotEquateOnSingleNullWhenHandlingNullsForRefTypes(object reference)
			{
				var sut = new TestableEqualityComparerBase<object>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				Assert.False(sut.Equals(null, reference));
				Assert.False(sut.Equals(reference, null));
			}

			[Theory]
			[AutoData]
			public void ShouldDelegateToImplOnSingleNullWhenNotHandlingNullsForRefTypes(object reference)
			{
				var sut = new TestableEqualityComparerBase<object>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
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
