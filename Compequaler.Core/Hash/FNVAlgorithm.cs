using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Compequaler.Hash
{
    internal static class FNVAlgorithm
    {
        public const uint _factor32 = 16777619;

        public const uint _seed32 = 2166136261;

        public const int _xorFolding32to31Mask = 0x7FFF_FFFF;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FNV1a32Fast(uint hashes, int hashCode)
            => FNV1a32Fast(hashes, unchecked((uint)hashCode));

        public static uint FNV1a32Fast(uint hashes, uint hashCode)
        {
            unchecked
            {
                hashes ^= hashCode;
                hashes *= _factor32;
            }
            return hashes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint FNV1a32(uint hashes, int hashCode)
            => FNV1a32(hashes, unchecked((uint)hashCode));

        public static uint FNV1a32(uint hashes, uint hashCode)
        {
            unchecked
            {
                hashes ^= hashCode & 0xFF;
                hashes *= _factor32;
                hashes ^= (hashCode >> 8) & 0xFF;
                hashes *= _factor32;
                hashes ^= (hashCode >> 16) & 0xFF;
                hashes *= _factor32;
                hashes ^= (hashCode >> 24) & 0xFF;
                hashes *= _factor32;
            }
            return hashes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int XORFolding32to31(uint hashes)
            => unchecked((int)((hashes >> 31) ^ (hashes & _xorFolding32to31Mask)));
    }
}
