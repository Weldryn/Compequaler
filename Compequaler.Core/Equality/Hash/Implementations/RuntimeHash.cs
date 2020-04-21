using Compequaler.Comparer.Equality;
using Compequaler.Utilities;
using System;
using System.Runtime.CompilerServices;

namespace Compequaler.Equality.Hash.Implementations
{
    public readonly struct RuntimeHash : IEquatable<RuntimeHash>
    {
        internal RuntimeHash(uint hashes)
        {
            Hashes = hashes;
        }

        internal uint Hashes { get; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public RuntimeHash Hash<T>(T value)
            => Hash(value, (IEqualityComparer<T>)null);

        public RuntimeHash Hash<T>(T value, IEqualityComparer<T> equalityComparer)
            => Helper<T>.Normalise(equalityComparer).GetRuntimeHash(this, value);

        public RuntimeHash Hash<T>(
            T value,
            System.Collections.Generic.IEqualityComparer<T> equalityComparer)
        {
            if (Helper<T>.TryGetHasherFromNormalisedComparer(ref equalityComparer, out var hasher))
                return hasher.GetRuntimeHash(this, value);

            return HashRaw(value, equalityComparer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal RuntimeHash HashNull()
            => new RuntimeHash(FNV1aAlgorithm.FNV(Hashes, 0));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal RuntimeHash HashRaw<T>(
            T value,
            System.Collections.Generic.IEqualityComparer<T> equalityComparer)
        {
            if (value == null &&
                System.Collections.Generic.EqualityComparer<T>.Default.Equals(equalityComparer))
                return HashNull();

            return HashRaw(equalityComparer.GetHashCode(value));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal RuntimeHash HashRaw(int hashCode)
            => new RuntimeHash(FNV1aAlgorithm.FNV(Hashes, hashCode));

        public override bool Equals(object obj)
        {
            if (obj is RuntimeHash hash) return Equals(hash);
            if (obj == null) return false;

            throw new ArgumentException("Expected an instance of type '" +
                typeof(RuntimeHash).AssemblyQualifiedName +
                "'. Got '" + obj.GetType().AssemblyQualifiedName + "'");
        }

        public bool Equals(RuntimeHash other)
            => Hashes.Equals(other.Hashes);

        public override int GetHashCode()
            => FNV1aAlgorithm.XORFold31(Hashes);

        public override string ToString()
            => ToString("X8");

        public string ToString(string hashesFormat)
            => GetHashCode().ToString(hashesFormat);

        public static implicit operator int(RuntimeHash hashes)
            => hashes.GetHashCode();

        public static bool operator ==(RuntimeHash first, RuntimeHash second)
            => first.Equals(second);

        public static bool operator !=(RuntimeHash first, RuntimeHash second)
            => !first.Equals(second);

        public static RuntimeHash operator ^(RuntimeHash seed, IRuntimeHashable hashable)
        {
            if (hashable == null) throw new ArgumentNullException(nameof(hashable));

            return hashable.GetRuntimeHash(seed);
        }
    }
}
