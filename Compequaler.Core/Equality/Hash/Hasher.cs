using Compequaler.Equality.Hash.Implementations;

namespace Compequaler.Equality.Hash
{
    public static class Hasher
    {
        public static RuntimeHash Seed { get; }
            = new RuntimeHash(FNV1aAlgorithm._seed32bits);

        internal static RuntimeHash NullHashedSeed { get; }
            = Seed.HashNull();

        internal static int NullHashedSeedHashCode { get; }
            = NullHashedSeed.GetHashCode();
    }
}
