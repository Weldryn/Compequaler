using Compequaler.Equality.Hash.Implementations;
using System.Collections;

namespace Compequaler.Comparers.Equality.Implementations
{
	internal sealed class RuntimeHashEqualityComparer
		: EqualityComparerBase<RuntimeHash>,
		IEqualityComparer<RuntimeHash>,
		IRuntimeHasher<RuntimeHash>,
		System.Collections.Generic.IEqualityComparer<RuntimeHash>,
		IEqualityComparer,
		IRuntimeHasher,
		System.Collections.IEqualityComparer
	{
		public RuntimeHashEqualityComparer() : base(false) { }

		protected internal override RuntimeHash GetRuntimeHashCore(RuntimeHash seed, RuntimeHash obj)
			=> seed.HashRaw(obj.Hashes, System.Collections.Generic.EqualityComparer<uint>.Default);

		protected internal override bool EqualsCore(RuntimeHash x, RuntimeHash y)
			=> x.Equals(y);

		protected internal override int GetHashCodeCore(RuntimeHash obj)
			=> obj.GetHashCode();
	}
}
