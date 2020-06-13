using Compequaler.Comparers.Equality;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System;
#if NETSTANDARD1_0
using System.Reflection;
#endif
using System.Runtime.CompilerServices;

namespace Compequaler.Utilities
{
	internal static class Helper<T>
    {
        static Helper()
        {
            var type = typeof(T);

#if NETSTANDARD1_0
            var typeInfo = type.GetTypeInfo();
            IsHashable = typeof(IRuntimeHashable).GetTypeInfo().IsAssignableFrom(typeInfo);
            IsReference = typeInfo.IsClass || typeInfo.IsInterface;
            IsRuntimeHash = typeInfo.Equals(typeof(RuntimeHash));
#else
            IsHashable = typeof(IRuntimeHashable).IsAssignableFrom(type);
            IsReference = type.IsClass || type.IsInterface;
            IsRuntimeHash = type.Equals(typeof(RuntimeHash));
#endif
        }

        public static bool CanHandleNulls { get; } = default(T) == null;

        public static bool IsHashable { get; }

        public static bool IsReference { get; }

        public static bool IsRuntimeHash { get; }

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

        /// <summary>
        /// Let <typeparamref name="T"/> be anything, checks for null without boxing
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull(T obj)
            => !CanHandleNulls ? false : obj == null;

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
        static Helper() { }

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
