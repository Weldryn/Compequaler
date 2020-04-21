namespace Compequaler.Comparer.Equality
{
	public interface IEqualityComparer<in T> : System.Collections.Generic.IEqualityComparer<T>, IRuntimeHasher<T>
	{
	}

	public interface IEqualityComparer : System.Collections.IEqualityComparer, IRuntimeHasher
	{
	}
}
