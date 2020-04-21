using Compequaler.Comparer.Equality;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using Compequaler.Tests.Unit.Models;
using System.Collections.Generic;
using Xunit;

namespace Compequaler.Tests.Unit.Equality.Hash
{
	public partial class RuntimeHashTests
	{
		public class Hash
		{
			public static IEnumerable<object[]> GetExpectedHash(int value)
			{
				yield return new object[]
					{
					FNV1aAlgorithm.XORFold31(
						FNV1aAlgorithm.FNV(
							FNV1aAlgorithm._seed32bits,
							EqualityComparer<int>.Default.GetHashCode(value))),
					value
					};
			}

			[Theory]
			[MemberData(nameof(GetExpectedHash), 123456)]
			public void HashShouldEquate(int expectedHash, int value)
			{
				var hashable = new TestableHashable(seed => seed.Hash(value));

				Assert.Equal(expectedHash, Hasher.Seed.Hash(value).GetHashCode());
				Assert.Equal(expectedHash, Hasher.Seed.Hash(value, EqualityComparer<int>.Default).GetHashCode());
				Assert.Equal(expectedHash, Hasher.Seed.Hash(value, DefaultEqualityComparer<int>.Instance).GetHashCode());
				Assert.Equal(expectedHash, DefaultEqualityComparer<int>.Instance.GetRuntimeHash(Hasher.Seed, value).GetHashCode());
				Assert.Equal(expectedHash, DefaultEqualityComparer<int>.Instance.GetHashCode(value));
				Assert.Equal(expectedHash, hashable.GetRuntimeHash().GetHashCode());
				Assert.Equal(expectedHash, hashable.GetRuntimeHash(Hasher.Seed).GetHashCode());
			}

			[Theory]
			[MemberData(nameof(GetExpectedHash), 123456)]
			public void HashShouldNotEquate(int expectedHash, int value)
			{
				var hashable = new TestableHashable(seed => seed.Hash(value));
				var unseeded = new RuntimeHash();

				Assert.NotEqual(expectedHash, unseeded.Hash(value).GetHashCode());
				Assert.NotEqual(expectedHash, unseeded.Hash(value, EqualityComparer<int>.Default).GetHashCode());
				Assert.NotEqual(expectedHash, unseeded.Hash(value, DefaultEqualityComparer<int>.Instance).GetHashCode());
				Assert.NotEqual(expectedHash, DefaultEqualityComparer<int>.Instance.GetRuntimeHash(unseeded, value).GetHashCode());
				Assert.NotEqual(expectedHash, hashable.GetRuntimeHash(unseeded).GetHashCode());
			}
		}
	}
}
