using AutoFixture.Xunit2;
using Compequaler.Comparers.Equality;
using Compequaler.Comparers.Equality.Implementations;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
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
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForSameNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, 1));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForDifferentNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
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
			public void ShouldEquateOnNullsForNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
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

			[Fact]
			public void ShouldNotEquateOnSingleNullWhenHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.False(sut.Equals(null, 1));
				Assert.False(sut.Equals(1, null));
			}

			[Fact]
			public void ShouldDelegateToImplOnSingleNullWhenNotHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(null, 1));
				Assert.Throws<SuccessfulTestException>(() => sut.Equals(1, null));
			}

			[Theory]
			[InlineAutoData(true, "sthqrg")]
			[InlineAutoData(true, 1)]
			[InlineAutoData(false, "sthqrg")]
			[InlineAutoData(false, 1)]
			public void ShouldThrowOnAnyUnsupportedTypeForRefTypes(
				bool handlingNulls,
				object other,
				Version model)
			{
				var sut = new TestableEqualityComparerBase<Version>(handlingNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(other, model));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model, other));
			}

			[Theory]
			[InlineData(true, "qdrgqerg")]
			[InlineData(true, 1.0)]
			[InlineData(false, "qdrgqerg")]
			[InlineData(false, 1.0)]
			public void ShouldThrowOnAnyUnsupportedTypeForValTypes(
				bool handlingNulls,
				object model)
			{
				var sut = new TestableEqualityComparerBase<int>(handlingNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model, 1));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(1, model));
			}

			[Theory]
			[InlineData(true, "drdru")]
			[InlineData(true, 1.0)]
			[InlineData(false, "drdru")]
			[InlineData(false, 1.0)]
			public void ShouldThrowOnAnyUnsupportedTypeForNullableTypes(
				bool handlingNulls,
				object model)
			{
				var sut = new TestableEqualityComparerBase<int?>(handlingNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(model, 1));
				Assert.ThrowsAny<ArgumentException>(() => sut.Equals(1, model));
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
