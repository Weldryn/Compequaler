using System;
using System.Collections.Generic;
using System.Text;

namespace Compequaler.Equality.Hash
{
    public static class Hasher
    {
        public static RuntimeHash Seed { get; } = new RuntimeHash(FNV1aAlgorithm._seed32bits);
    }
}
