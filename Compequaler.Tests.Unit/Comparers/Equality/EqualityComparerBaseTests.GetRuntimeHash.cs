using Compequaler.Comparers.Equality;
using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class EqualityComparerBaseTests
	{
		public class GetRuntimeHash
		{
			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForRefTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<object>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, new object()));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, 1));
			}

			[Theory]
			[InlineData(true)]
			[InlineData(false)]
			public void ShouldDelegateToImplForValTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int>(handleNulls)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, 1));
			}

			[Fact]
			public void ShouldDelegateToImplOnNullWhenNotHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<object>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldReturnOnNullWhenHandlingNullsForRefTypes()
			{
				var sut = new TestableEqualityComparerBase<object>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				var expected = Hasher.Seed.Hash<Version>(null);
				Assert.Equal(expected, sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldDelegateToImplOnNullWhenNotHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyTyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldReturnOnNullWhenHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyTyped();
				var expected = Hasher.Seed.Hash<Version>(null);
				Assert.Equal(expected, sut.GetRuntimeHash(Hasher.Seed, null));
			}

			private class TestableEqualityComparerBase<T> : EqualityComparerBase<T>
			{
				public TestableEqualityComparerBase(
					bool shouldHandleNulls)
					: base(shouldHandleNulls)
				{
				}

				public Func<RuntimeHash, T, RuntimeHash> GetRuntimeHashCoreImpl { get; set; }

				protected internal override RuntimeHash GetRuntimeHashCore(RuntimeHash seed, T obj)
					=> GetRuntimeHashCoreImpl?.Invoke(seed, obj) ?? base.GetRuntimeHashCore(seed, obj);

				public IEqualityComparer<T> AsExplicitlyTyped() => this;

				public TestableEqualityComparerBase<T> ThrowsOnImplCall(Func<Exception> createException)
				{
					GetRuntimeHashCoreImpl = (seed, x) => throw createException();
					return this;
				}
			}
		}
	}
}
