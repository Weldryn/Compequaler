using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Compequaler.Hash
{
    internal static class FNVAlgorithm
    {
        public const int _factor32 = 16777619;

        public const int _seed32 = -2128831035;

        public const int _xorFolding32to31Mask = 0x7FFF_FFFF;

        public static int FNV1a32Fast(int hashes, int hashCode)
        {
            unchecked
            {
                hashes ^= hashCode;
                hashes *= _factor32;
            }
            return hashes;
        }

        public static int FNV1a32(int hashes, int hashCode)
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
        public static int XORFolding32to31(int hashes)
            => unchecked((int)((uint)hashes >> 31)) ^ (hashes & _xorFolding32to31Mask);
    }
}
