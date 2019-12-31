using System;
using System.Collections.Generic;
using System.Text;

namespace Compequaler.Hash
{
    public readonly struct RuntimeHash : IEquatable<RuntimeHash>
    {
        internal RuntimeHash(int hashes)
        {
            Hashes = hashes;
        }

        internal int Hashes { get; }

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
            => FNVAlgorithm.XORFolding32to31(Hashes);

        public override string ToString()
            => GetHashCode().ToString();

        public static implicit operator int(RuntimeHash hashes)
            => hashes.GetHashCode();
    }
}
