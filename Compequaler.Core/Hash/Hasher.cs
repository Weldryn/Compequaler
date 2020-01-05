using System;
using System.Collections.Generic;
using System.Text;

namespace Compequaler.Hash
{
    public static class Hasher
    {
        public static RuntimeHash Seed { get; } = new RuntimeHash(FNVAlgorithm._seed32);
    }
}
