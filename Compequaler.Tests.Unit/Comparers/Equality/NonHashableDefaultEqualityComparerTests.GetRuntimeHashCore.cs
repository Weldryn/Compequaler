using AutoFixture.Xunit2;
using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using Compequaler.Utilities;
using System.Collections.Generic;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class NonHashableDefaultEqualityComparerTests
	{
		public class GetRuntimeHashCore
		{
			[Fact]
			public void ShouldGetRuntimeHashForRefTypes()
			{
				var sut = new NonHashableDefaultEqualityComparer<object>();
				var model = new object();
				var expected = EqualityComparer<object>.Default.GetHashCode(model);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, model));
			}

			[Fact]
			public void ShouldGetRuntimeHashOnNullForRefTypes()
			{
				var sut = new NonHashableDefaultEqualityComparer<object>();
				var expected = EqualityComparer<object>.Default.GetHashCode(null);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldGetRuntimeHashForValTypes()
			{
				var sut = new NonHashableDefaultEqualityComparer<int>();
				var expected = EqualityComparer<int>.Default.GetHashCode(1);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, 1));
			}

			[Fact]
			public void ShouldGetRuntimeHashForNullableTypes()
			{
				var sut = new NonHashableDefaultEqualityComparer<int?>();
				var expected = EqualityComparer<int?>.Default.GetHashCode(1);
				expected = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, expected));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, 1));
			}
		}
	}
}
