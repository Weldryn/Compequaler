using Compequaler.Equality.Hash.Implementations;
using System;

namespace Compequaler.Equality.Hash
{
	public static class RuntimeHashableExtensions
	{
		public static RuntimeHash GetRuntimeHash<T>(this T hashable) where T : IRuntimeHashable
			=> hashable?.GetRuntimeHash(Hasher.Seed) ?? throw new ArgumentNullException(nameof(hashable));
	}
}
