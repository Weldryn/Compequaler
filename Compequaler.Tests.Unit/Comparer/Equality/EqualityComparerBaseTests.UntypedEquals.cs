using AutoFixture.Xunit2;
using Compequaler.Comparer.Equality;
using Compequaler.Comparer.Equality.Implementations;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparer.Equality
{
	public partial class EqualityComparerBaseTests
	{
		public class UntypedEquals
		{
			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForSameValueTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 1));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForDifferentValueTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 2));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForDifferentRefTypes(
				bool handleNulls,
				Version model1,
				Version model2)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(model1, model2));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldNotDelegateToImplAndEquateForSameRefTypes(bool handleNulls, Version model)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.True(sut.Equals(model, model));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldEquateOnNullsForValueTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.True(sut.Equals(null, null));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldEquateOnNullsForRefTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.True(sut.Equals(null, null));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldNotEquateOnSingleNullForValueTypes(bool handlingNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handlingNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.False(sut.Equals(null, 0));
				Assert.False(sut.Equals(0, null));
			}

			[Theory]
			[AutoData]
			public void ShouldNotEquateOnSingleNullWhenHandlingNullsForRefTypes(Version reference)
			{
				var sut = new TestableEqualityComparerBase<Version>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.False(sut.Equals(null, reference));
				Assert.False(sut.Equals(reference, null));
			}

			[Theory]
			[AutoData]
			public void ShouldDelegateToImplOnSingleNullWhenNotHandlingNullsForRefTypes(Version reference)
			{
				var sut = new TestableEqualityComparerBase<Version>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(null, reference));
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(reference, null));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldThrowOnAnyUnsupportedTypeForRefTypes(
				bool handlingNulls,
				Version model,
				object model1,
				Uri model2)
			{
				var sut = new TestableEqualityComparerBase<Version>(handlingNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model1, model));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model, model1));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model2, model1));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model1, model2));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldThrowOnAnyUnsupportedTypeForValTypes(
				bool handlingNulls,
				int value,
				double value1,
				object model)
			{
				var sut = new TestableEqualityComparerBase<int>(handlingNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(value1, value));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(value, value1));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model, value1));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(value1, model));
			}

			private class TestableEqualityComparerBase<T> : EqualityComparerBase<T>
			{
				public TestableEqualityComparerBase(
					bool shouldHandleNulls)
					: base(shouldHandleNulls)
				{
				}

				public Func<T, T, bool> EqualsCoreImpl { get; set; }

				protected override bool EqualsCore(T x, T y)
					=> EqualsCoreImpl?.Invoke(x, y) ?? base.EqualsCore(x, y);

				public IEqualityComparer AsExplicitlyUntyped() => this;

				public TestableEqualityComparerBase<T> ThrowsOnImplCall(Func<Exception> createException)
				{
					EqualsCoreImpl = (x, y) => throw createException();
					return this;
				}
			}
		}
	}
}
