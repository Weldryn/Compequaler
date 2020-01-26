using System;
using System.Collections.Generic;
using System.Text;

namespace Compequaler.Hash
{
	public interface IRuntimeHashable
	{
		RuntimeHash Hash(RuntimeHash seed);
	}
}
