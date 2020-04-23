using Compequaler.Comparers.Equality;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Compequaler.Utilities
{
    internal static class Helper<T>
    {
        public static bool CanHandleNulls { get; } = default(T) == null;

#if NETSTANDARD1_6
        public static bool IsHashableType 
            => typeof(IRuntimeHashable).GetTypeInfo().IsAssignableFrom(typeof(T));
#else
        public static bool IsHashableType 
            => typeof(IRuntimeHashable).IsAssignableFrom(typeof(T));
#endif

        public static bool IsRuntimeHashType => typeof(T).Equals(typeof(RuntimeHash));

        public static bool TryGetHasherFromNormalisedComparer(
            ref System.Collections.Generic.IEqualityComparer<T> comparer,
            out IRuntimeHasher<T> hasher)
        {
            if (comparer == null)
            {
                var defaultEqualityComparer = DefaultEqualityComparer<T>.Instance;
                comparer = defaultEqualityComparer;
                hasher = defaultEqualityComparer;
            }
            else hasher = comparer as IRuntimeHasher<T>;

            return hasher != null;
        }

        public static IEqualityComparer<T> Normalise(IEqualityComparer<T> comparer)
            => comparer ?? DefaultEqualityComparer<T>.Instance;

        public static Exception CreateNotEquatableException(object x, object y)
            => new ArgumentException("Equality comparer of " + typeof(T).FullName + ": can't compare '" +
                (x?.GetType().AssemblyQualifiedName ?? "<null>") + "' with '" +
                (y?.GetType().AssemblyQualifiedName ?? "<null>") + "'");

        public static Exception CreateNotHashableException(object x)
            => new ArgumentException("Equality comparer of " + typeof(T).FullName + ": can't hash '" +
                (x?.GetType().AssemblyQualifiedName ?? "<null>") + "'");
    }

    internal static class Helper
    {
        internal static bool EqualsByRef(object first, object second)
            => ReferenceEquals(first, second);

        internal static bool EqualsByType(Type equalsToType, object obj)
            => obj != null && equalsToType.Equals(obj.GetType());

        internal static int GetHashCodeByRef(object obj)
            => Hasher.Seed
                .HashRaw(RuntimeHelpers.GetHashCode(obj))
                .GetHashCode();

        internal static int GetHashCodeByType(Type type)
            => Hasher.Seed
                .HashRaw(type.GetHashCode())
                .GetHashCode();
    }
}
