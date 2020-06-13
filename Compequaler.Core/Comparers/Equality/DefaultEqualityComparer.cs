using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash.Implementations;
using Compequaler.Utilities;
using System;
using System.Threading;

namespace Compequaler.Comparers.Equality
{
	public sealed class DefaultEqualityComparer<T>
		: IEqualityComparer<T>,
		IRuntimeHasher<T>,
		System.Collections.Generic.IEqualityComparer<T>,
		IEqualityComparer,
		IRuntimeHasher,
		System.Collections.IEqualityComparer
	{
		private static DefaultEqualityComparer<T> _instance;

		public static DefaultEqualityComparer<T> Instance
		{
			get
			{
				Interlocked.MemoryBarrier();
				var instance = _instance;

				if (instance != null) return _instance;

				instance = CreateDefault();
				Interlocked.CompareExchange(ref _instance, instance, null);
				return _instance;
			}
		}

		internal DefaultEqualityComparer(IEqualityComparer<T> equalityComparer)
		{
			Inner = equalityComparer ?? throw new ArgumentNullException(nameof(equalityComparer));
		}

		internal IEqualityComparer<T> Inner { get; }
		 
		public override bool Equals(object obj)
			=> Helper.EqualsByType(typeof(DefaultEqualityComparer<T>), obj);

		public override int GetHashCode()
			=> Helper.GetHashCodeByType(typeof(DefaultEqualityComparer<T>));

		internal static DefaultEqualityComparer<T> CreateDefault()
		{
			IEqualityComparer<T> comparer;

			if (Helper<T>.IsHashable)
			{
				var type = typeof(HashableEqualityComparer<>).MakeGenericType(typeof(T));
				comparer = (IEqualityComparer<T>)Activator.CreateInstance(type);
			}
			else if (Helper<T>.IsRuntimeHash)
			{
				comparer = (IEqualityComparer<T>)(object)new RuntimeHashEqualityComparer();
			}
			else comparer = new ClassicEqualityComparer<T>();

			return new DefaultEqualityComparer<T>(comparer);
		}

		public RuntimeHash GetRuntimeHash(RuntimeHash seed, T obj)
			=> Inner.GetRuntimeHash(seed, obj);

		public bool Equals(T x, T y)
			=> Inner.Equals(x, y);

		public int GetHashCode(T obj)
			=> Inner.GetHashCode(obj);

		bool System.Collections.IEqualityComparer.Equals(object x, object y)
			=> ((IEqualityComparer)Inner).Equals(x, y);

		int System.Collections.IEqualityComparer.GetHashCode(object obj)
			=> ((IEqualityComparer)Inner).GetHashCode(obj);

		RuntimeHash IRuntimeHasher.GetRuntimeHash(RuntimeHash seed, object obj)
			=> ((IRuntimeHasher)Inner).GetRuntimeHash(seed, obj);
	}
}
