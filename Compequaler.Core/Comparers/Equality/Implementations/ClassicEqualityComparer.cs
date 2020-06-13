using Compequaler.Equality.Hash.Implementations;

namespace Compequaler.Comparers.Equality.Implementations
{
	internal sealed class ClassicEqualityComparer<T>
		: EqualityComparerBase<T>,
		IEqualityComparer<T>,
		IRuntimeHasher<T>,
		System.Collections.Generic.IEqualityComparer<T>,
		IEqualityComparer,
		IRuntimeHasher,
		System.Collections.IEqualityComparer
	{
		public ClassicEqualityComparer() : base(true) { }

		protected internal override RuntimeHash GetRuntimeHashCore(RuntimeHash seed, T obj)
			=> seed.HashRaw(obj, System.Collections.Generic.EqualityComparer<T>.Default);
	}
}
