using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using Compequaler.Utilities;
using System.Runtime.CompilerServices;

namespace Compequaler.Comparers.Equality.Implementations
{
	internal abstract class EqualityComparerBase<T>
		: IEqualityComparer<T>,
		IRuntimeHasher<T>,
		System.Collections.Generic.IEqualityComparer<T>,
		IEqualityComparer,
		IRuntimeHasher,
		System.Collections.IEqualityComparer
	{
		public EqualityComparerBase(bool shouldHandleNullsEverywhere)
			: this(shouldHandleNullsEverywhere, shouldHandleNullsEverywhere)
		{ }

		public EqualityComparerBase(
			bool shouldHandleNullsInEquals,
			bool shouldHandleNullsInGetHashes)
		{
			var canHandleNulls = Helper<T>.CanHandleNulls;
			HandleNullsInEquals = canHandleNulls && shouldHandleNullsInEquals;
			HandleNullsInGetHashes = canHandleNulls && shouldHandleNullsInGetHashes;
		}

		protected bool HandleNullsInEquals { get; }

		protected bool HandleNullsInGetHashes { get; }

		bool System.Collections.IEqualityComparer.Equals(object x, object y)
		{
			if (ReferenceEquals(x, y)) return true;

			if (x is T xT)
			{
				if (y is T yT) return EqualsCore(xT, yT);
				else if (y == null)
				{
					if (!Helper<T>.CanHandleNulls || HandleNullsInEquals) return false;

					return EqualsCore(xT, (T)y);
				}
			}
			else if (x == null)
			{
				if (!Helper<T>.CanHandleNulls || HandleNullsInEquals) return false;

				if (y is T yT) return EqualsCore((T)x, yT);

				//	else y == null already covered by ReferenceEquals check
			}

			throw Helper<T>.CreateNotEquatableException(x, y);
		}

		public bool Equals(T x, T y)
		{
			if (Helper<T>.CanHandleNulls && ReferenceEquals(x, y)) return true;

			if (HandleNullsInEquals)
			{
				if (x == null || y == null) return false;
			}

			return EqualsCore(x, y);
		}

		protected virtual bool EqualsCore(T x, T y)
			=> System.Collections.Generic.EqualityComparer<T>.Default.Equals(x, y);

		int System.Collections.IEqualityComparer.GetHashCode(object obj)
		{
			if (obj is T t) return GetHashCodeCore(t);

			if (obj == null)
			{
				if (!Helper<T>.CanHandleNulls || HandleNullsInGetHashes)
					return Hasher.NullHashedSeedHashCode;

				return GetHashCodeCore((T)obj);
			}

			throw Helper<T>.CreateNotHashableException(obj);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int GetHashCode(T obj)
		{
			if (HandleNullsInGetHashes && obj == null)
				return Hasher.NullHashedSeedHashCode;

			return GetHashCodeCore(obj);
		}

		protected virtual int GetHashCodeCore(T obj)
			=> GetRuntimeHashCore(Hasher.Seed, obj).GetHashCode();

		RuntimeHash IRuntimeHasher.GetRuntimeHash(RuntimeHash seed, object obj)
		{
			if (!Helper<T>.CanHandleNulls || HandleNullsInGetHashes)
			{
				if (obj is T objT) return GetRuntimeHashCore(seed, objT);
				if (obj == null) return seed.HashNull();
			}
			else if (obj is T objT)
				return GetRuntimeHashCore(seed, objT);
			else if(obj == null)
				return GetRuntimeHashCore(seed, (T)obj);

			throw Helper<T>.CreateNotHashableException(obj);
		}

		public RuntimeHash GetRuntimeHash(RuntimeHash seed, T obj)
		{
			if (HandleNullsInGetHashes && obj == null)
				return seed.HashNull();

			return GetRuntimeHashCore(seed, obj);
		}

		protected virtual RuntimeHash GetRuntimeHashCore(RuntimeHash seed, T obj)
			=> seed.Hash(obj, DefaultEqualityComparer<T>.Instance);

		public override bool Equals(object obj)
			=> Helper.EqualsByRef(this, obj);

		public override int GetHashCode()
			=> Helper.GetHashCodeByRef(this);
	}
}
