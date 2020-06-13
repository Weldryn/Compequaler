using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;

namespace Compequaler.Comparers.Equality.Implementations
{
	internal sealed class HashableEqualityComparer<T>
		: EqualityComparerBase<T>,
		IEqualityComparer<T>,
		IRuntimeHasher<T>,
		System.Collections.Generic.IEqualityComparer<T>,
		IEqualityComparer,
		IRuntimeHasher,
		System.Collections.IEqualityComparer
		where T : IRuntimeHashable
	{
		public HashableEqualityComparer() : base(true) { }

		protected internal override RuntimeHash GetRuntimeHashCore(RuntimeHash seed, T obj)
			=> obj.GetRuntimeHash(seed);
	}
}
