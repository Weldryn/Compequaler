using Compequaler.Equality.Hash.Implementations;

namespace Compequaler.Equality.Hash
{
	public interface IRuntimeHashable
	{
		RuntimeHash GetRuntimeHash(RuntimeHash seed);
	}
}
