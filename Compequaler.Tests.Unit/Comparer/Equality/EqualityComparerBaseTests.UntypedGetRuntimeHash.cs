using AutoFixture.Xunit2;
using Compequaler.Comparer.Equality;
using Compequaler.Comparer.Equality.Implementations;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparer.Equality
{
	public partial class EqualityComparerBaseTests
	{
		public class UntypedGetRuntimeHash
		{
			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldDelegateToImplForRefTypes(bool handleNulls, Version model)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, model));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForValTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, 1));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldReturnOnNullsForValTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				var expected = Hasher.Seed.Hash<object>(null);
				Assert.Equal(expected, sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldDelegateToImplOnNullWhenNotHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<Version>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldReturnOnNullWhenHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<Version>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				var expected = Hasher.Seed.Hash<object>(null);
				Assert.Equal(expected, sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldThrowOnWrongTypeForRefTypes(bool handleNulls, Uri model)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<ArgumentException>(() => sut.GetRuntimeHash(Hasher.Seed, model));
			}

			[Theory]
			[InlineAutoData(true)]
			[InlineAutoData(false)]
			public void ShouldThrowOnWrongTypeForValTypes(bool handleNulls, double model)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<ArgumentException>(() => sut.GetRuntimeHash(Hasher.Seed, model));
			}

			private class TestableEqualityComparerBase<T> : EqualityComparerBase<T>
			{
				public TestableEqualityComparerBase(
					bool shouldHandleNulls)
					: base(shouldHandleNulls)
				{
				}

				public Func<RuntimeHash, T, RuntimeHash> GetRuntimeHashCoreImpl { get; set; }

				protected override RuntimeHash GetRuntimeHashCore(RuntimeHash seed, T obj)
					=> GetRuntimeHashCoreImpl?.Invoke(seed, obj) ?? base.GetRuntimeHashCore(seed, obj);

				public IEqualityComparer AsExplicitlyUntyped() => this;

				public TestableEqualityComparerBase<T> ThrowsOnImplCall(Func<Exception> createException)
				{
					GetRuntimeHashCoreImpl = (seed, x) => throw createException();
					return this;
				}
			}
		}
	}
}
