using Compequaler.Comparers.Equality;
using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public class DefaultEqualityComparerTests
    {
		[Fact]
		public void ShouldCreateRuntimeHashEqualityComparer()
			=> Assert.IsType<RuntimeHashEqualityComparer>(DefaultEqualityComparer<RuntimeHash>.CreateDefault().Inner);

		[Fact]
		public void ShouldCreateHashableEqualityComparer()
			=> Assert.IsType<HashableEqualityComparer<IRuntimeHashable>>(DefaultEqualityComparer<IRuntimeHashable>.CreateDefault().Inner);

		[Fact]
		public void ShouldCreateClassicEqualityComparer()
			=> Assert.IsType<ClassicEqualityComparer<object>>(DefaultEqualityComparer<object>.CreateDefault().Inner);
	}
}
