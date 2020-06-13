using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System;
using System.Collections.Generic;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public partial class HashableEqualityComparerTests
	{
		public class GetRuntimeHashCore
		{
			[Fact]
			public void ShouldCallGetRuntimeHash()
			{
				var sut = new HashableEqualityComparer<Hashable>();
				Assert.Throws<SuccessfulTestException>(() =>
					sut.GetRuntimeHashCore(Hasher.Seed, Hashable.Create(seed =>
						throw new SuccessfulTestException())));
			}

			[Fact]
			public void ShouldGetRuntimeHash()
			{
				var sut = new HashableEqualityComparer<Hashable>();
				var hashCode = EqualityComparer<object>.Default.GetHashCode(new object());
				var expected = new RuntimeHash(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashCode));
				var model = Hashable.Create(seed => expected);
				Assert.Equal(expected, sut.GetRuntimeHashCore(Hasher.Seed, model));
			}
		}

		private class Hashable : IRuntimeHashable
		{
			public Hashable(Func<RuntimeHash, RuntimeHash> delegte)
			{
				Delegate = delegte ?? throw new SetupTestException();
			}

			private Func<RuntimeHash, RuntimeHash> Delegate { get; }

			public RuntimeHash GetRuntimeHash(RuntimeHash seed) => Delegate(seed);

			public static Hashable Create(Func<RuntimeHash, RuntimeHash> delegte)
				=> new Hashable(delegte);
		}
	}
}
