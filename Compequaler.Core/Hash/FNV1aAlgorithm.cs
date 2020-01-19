using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Compequaler.Hash
{
    internal static class FNV1aAlgorithm
    {
        public const uint _factor32bits = 16777619;

        public const uint _seed32bits = 2166136261;

        public static uint FNV1a32Fast(uint hashes, int hashCode)
        {
            unchecked
            {
                hashes ^= (uint)hashCode;
                hashes *= _factor32bits;
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
                hashes *= _factor32bits;
                hashes ^= (hashCode >> 8) & 0xFF;
                hashes *= _factor32bits;
                hashes ^= (hashCode >> 16) & 0xFF;
                hashes *= _factor32bits;
                hashes ^= (hashCode >> 24) & 0xFF;
                hashes *= _factor32bits;
            }
            return hashes;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int XORFold31(uint hashes)
            => unchecked((int)((hashes >> 31) ^ (hashes & 0x7FFF_FFFFu)));
    }
}
