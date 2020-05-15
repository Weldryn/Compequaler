using Compequaler.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Compequaler.Tests.Unit.Utilities
{
	public partial class HelperTests
	{
		public class IsNull
		{
			[Fact]
			public void ShouldReturnFalse()
			{
				Assert.False(Helper<int?>.IsNull(0));
				Assert.False(Helper<string>.IsNull(string.Empty));
				Assert.False(Helper<Version>.IsNull(new Version()));
				Assert.False(Helper<int>.IsNull(0));
			}

			[Fact]
			public void ShouldReturnTrue()
			{
				Assert.True(Helper<int?>.IsNull(null));
				Assert.True(Helper<string>.IsNull(null));
				Assert.True(Helper<Version>.IsNull(null));
			}
		}
	}
}
