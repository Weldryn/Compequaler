using Compequaler.Equality.Hash.Implementations;
using System.Collections;

namespace Compequaler.Comparers.Equality.Implementations
{
	internal sealed class NonHashableDefaultEqualityComparer<T>
		: EqualityComparerBase<T>,
		IEqualityComparer<T>,
		IRuntimeHasher<T>,
		System.Collections.Generic.IEqualityComparer<T>,
		IEqualityComparer,
		IRuntimeHasher,
		System.Collections.IEqualityComparer
	{
		public NonHashableDefaultEqualityComparer() : base(true) { }

		protected internal override RuntimeHash GetRuntimeHashCore(RuntimeHash seed, T obj)
			=> seed.HashRaw(obj, System.Collections.Generic.EqualityComparer<T>.Default);

		public override string ToString()
		{
			var type = typeof(DefaultEqualityComparer<>);

			return type.Namespace + "." + type.Name + "'1<" + typeof(T).FullName + ">";
		}
	}
}
