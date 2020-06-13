using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System.Collections.Generic;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class ClassicEqualityComparerTests
	{
		public class GetRuntimeHashCore
		{
			[Fact]
			public void ShouldGetRuntimeHashForRefTypes()
			{
				var sut = new ClassicEqualityComparer<object>();
				var model = new object();
				var hashCode = EqualityComparer<object>.Default.GetHashCode(model);
				var expected = new RuntimeHash(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashCode));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, model));
			}

			[Fact]
			public void ShouldGetRuntimeHashOnNullForRefTypes()
			{
				var sut = new ClassicEqualityComparer<object>();
				var hashCode = EqualityComparer<object>.Default.GetHashCode(null);
				var expected = new RuntimeHash(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashCode));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, null));
			}

			[Fact]
			public void ShouldGetRuntimeHashForValTypes()
			{
				var sut = new ClassicEqualityComparer<int>();
				var hashCode = EqualityComparer<int>.Default.GetHashCode(1);
				var expected = new RuntimeHash(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashCode));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, 1));
			}

			[Fact]
			public void ShouldGetRuntimeHashForNullableTypes()
			{
				var sut = new ClassicEqualityComparer<int?>();
				var hashCode = EqualityComparer<int?>.Default.GetHashCode(1);
				var expected = new RuntimeHash(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashCode));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, 1));
			}

			[Fact]
			public void ShouldGetRuntimeHashOnNullForNullableTypes()
			{
				var sut = new ClassicEqualityComparer<int?>();
				var hashCode = EqualityComparer<int?>.Default.GetHashCode(null);
				var expected = new RuntimeHash(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashCode));
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, null));
			}
		}
	}
}
