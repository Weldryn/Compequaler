using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System.Collections.Generic;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class RuntimeHashEqualityComparerTests
	{
		[Fact]
		public void ShouldGetRuntimeHash()
		{
			var sut = new RuntimeHashEqualityComparer();
			var hashCode = EqualityComparer<uint>.Default.GetHashCode(1);
			var expected = new RuntimeHash(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashCode));
			Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, new RuntimeHash(1)));
		}

		[Fact]
		public void ShouldEquateOnSameRuntimeHash()
		{
			var sut = new RuntimeHashEqualityComparer();
			Assert.True(sut.EqualsCore(Hasher.Seed, Hasher.Seed));
		}

		[Fact]
		public void ShouldNotEquateOnDifferentRuntimeHash()
		{
			var sut = new RuntimeHashEqualityComparer();
			Assert.False(sut.EqualsCore(new RuntimeHash(0), new RuntimeHash(1)));
		}

		[Fact]
		public void ShouldGetHashCode()
		{
			var sut = new RuntimeHashEqualityComparer();
			var expected = FNV1aAlgorithm.XORFold31(Hasher.Seed.Hashes);
			Assert.Equal(expected, sut.GetHashCodeCore(Hasher.Seed));
		}
	}
}
