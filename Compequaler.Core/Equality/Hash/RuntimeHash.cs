using System;
using System.Collections.Generic;
using System.Text;

namespace Compequaler.Equality.Hash
{
    public readonly struct RuntimeHash : IEquatable<RuntimeHash>
    {
        internal RuntimeHash(uint hashes)
        {
            Hashes = hashes;
        }

        internal uint Hashes { get; }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is RuntimeHash hash) return Equals(hash);

            throw new ArgumentException("Expected an instance of type '" + typeof(RuntimeHash).AssemblyQualifiedName
                + "'. Got '" + obj.GetType().AssemblyQualifiedName + "'");
        }

        public bool Equals(RuntimeHash other)
            => Hashes == other.Hashes;

        public override int GetHashCode()
            => FNV1aAlgorithm.XORFold31(Hashes);

        public override string ToString()
            => GetHashCode().ToString();

        public static implicit operator int(RuntimeHash hashes)
            => hashes.GetHashCode();

        public static bool operator ==(RuntimeHash first, RuntimeHash second)
            => first.Equals(second);

        public static bool operator !=(RuntimeHash first, RuntimeHash second)
            => !first.Equals(second);
    }
}
