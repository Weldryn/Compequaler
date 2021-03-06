﻿using AutoFixture.Xunit2;
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
			public void ShouldDelegateToImplForNullableTypes(bool handleNulls)
			{
				var sut = new TestableEqualityComparerBase<int?>(handleNulls)
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
				var expected = System.Collections.Generic.EqualityComparer<object>.Default.GetHashCode(null);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
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
				var expected = System.Collections.Generic.EqualityComparer<object>.Default.GetHashCode(null);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
				Assert.Equal(expected, sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldDelegateToImplOnNullWhenNotHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(false)
					.ThrowsOnImplCall(() => new SuccessfulTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<SuccessfulTestException>(() => sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldReturnOnNullWhenHandlingNullsForNullableTypes()
			{
				var sut = new TestableEqualityComparerBase<int?>(true)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				var expected = System.Collections.Generic.EqualityComparer<object>.Default.GetHashCode(null);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
				Assert.Equal(expected, sut.GetRuntimeHash(Hasher.Seed, null));
			}

			[Theory]
			[InlineData(true, "sehrsth")]
			[InlineData(true, 1)]
			[InlineData(false, "sehrsth")]
			[InlineData(false, 1)]
			public void ShouldThrowOnWrongTypeForRefTypes(bool handleNulls, object model)
			{
				var sut = new TestableEqualityComparerBase<Version>(handleNulls)
					.ThrowsOnImplCall(() => new FailedTestException())
					.AsExplicitlyUntyped();
				Assert.Throws<ArgumentException>(() => sut.GetRuntimeHash(Hasher.Seed, model));
			}

			[Theory]
			[InlineData(true, "sehrsth")]
			[InlineData(true, 1.0)]
			[InlineData(false, "sehrsth")]
			[InlineData(false, 1.0)]
			public void ShouldThrowOnWrongTypeForValTypes(bool handleNulls, object model)
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

				protected internal override RuntimeHash GetRuntimeHashCore(RuntimeHash seed, T obj)
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
