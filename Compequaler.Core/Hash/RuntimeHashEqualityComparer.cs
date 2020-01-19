using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Compequaler.Hash
{
    public class RuntimeHashEqualityComparer : IEqualityComparer<RuntimeHash>, IEqualityComparer
    {
        public bool Equals(RuntimeHash x, RuntimeHash y) => x.Equals(y);

        public int GetHashCode(RuntimeHash obj) => obj.GetHashCode();

        bool IEqualityComparer.Equals(object x, object y)
        {
            if (x is RuntimeHash xRH && y is RuntimeHash yRH) return Equals(xRH, yRH);
            if (x is null && y is null) return true;

            throw new ArgumentException("Can't compare '" +
                (x?.GetType().AssemblyQualifiedName ?? "<null>") + "' with '" +
                (y?.GetType().AssemblyQualifiedName ?? "<null>") + "'");
        }

        int IEqualityComparer.GetHashCode(object obj)
        {
            if (obj is null || !(obj is RuntimeHash hashes))
                return 0;

            return GetHashCode(hashes);
        }

        private static RuntimeHashEqualityComparer _default;

        public static RuntimeHashEqualityComparer Default
            => _default ?? (_default = new RuntimeHashEqualityComparer());

    }
}
