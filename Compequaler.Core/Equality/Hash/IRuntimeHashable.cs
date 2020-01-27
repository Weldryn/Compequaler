using System;
using System.Collections.Generic;
using System.Text;

namespace Compequaler.Equality.Hash
{
	public interface IRuntimeHashable
	{
		RuntimeHash Hash(RuntimeHash seed);
	}
}
