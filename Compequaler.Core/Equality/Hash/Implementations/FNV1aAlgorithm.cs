using System.Runtime.CompilerServices;

namespace Compequaler.Equality.Hash.Implementations
{
    internal static class FNV1aAlgorithm
    {
        public const uint _factor32bits = 16777619;

        public const uint _seed32bits = 2166136261;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FNV(uint hashes, int hashCode)
            => FNV1a32(hashes, hashCode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FNV(uint hashes, uint hashCode)
            => FNV1a32(hashes, hashCode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FNV1a32Fast(uint hashes, int hashCode)
            => FNV1a32Fast(hashes, unchecked((uint)hashCode));

        public static uint FNV1a32Fast(uint hashes, uint hashCode)
            => unchecked((hashes ^ hashCode) * _factor32bits);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FNV1a32(uint hashes, int hashCode)
            => FNV1a32(hashes, unchecked((uint)hashCode));

        public static uint FNV1a32(uint hashes, uint hashCode)
        {
            unchecked
            {
                hashes = (hashes ^ hashCode & 0xFF) * _factor32bits;
                hashes = (hashes ^ (hashCode >> 8) & 0xFF) * _factor32bits;
                hashes = (hashes ^ (hashCode >> 16) & 0xFF) * _factor32bits;
                hashes = (hashes ^ (hashCode >> 24) & 0xFF) * _factor32bits;
            }
            return hashes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int XORFold31(uint hashes)
            => unchecked((int)((hashes >> 31) ^ (hashes & 0x7FFF_FFFFu)));
    }
}
