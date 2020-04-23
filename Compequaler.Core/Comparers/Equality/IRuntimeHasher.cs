using Compequaler.Equality.Hash.Implementations;

namespace Compequaler.Comparers.Equality
{
	public interface IRuntimeHasher
	{
		RuntimeHash GetRuntimeHash(RuntimeHash seed, object obj);
	}

	public interface IRuntimeHasher<in T>
	{
		RuntimeHash GetRuntimeHash(RuntimeHash seed, T obj);
	}
}
