using Compequaler.Comparer.Equality.Implementations;
using Xunit;

namespace Compequaler.Tests.Unit.Comparer.Equality
{
	public partial class EqualityComparerBaseTests
	{
		public class Instance
		{
			[Theory]
			[InlineData(true, true)]
			[InlineData(true, false)]
			[InlineData(false, true)]
			[InlineData(false, false)]
			public void ShouldNotHandleNullsForValueTypes(
				bool shouldHandleNullsInEquals,
				bool shouldHandleNullsInGetHashes)
			{
				var sut = new TestableEqualityComparerBase<int>(
					shouldHandleNullsInEquals,
					shouldHandleNullsInGetHashes);

				Assert.False(sut.HandleNullsInEquals);
				Assert.False(sut.HandleNullsInGetHashes);
			}

			[Theory]
			[InlineData(true, true)]
			[InlineData(true, false)]
			[InlineData(false, true)]
			[InlineData(false, false)]
			public void ShouldHandleNullsForRefTypes(
				bool shouldHandleNullsInEquals,
				bool shouldHandleNullsInGetHashes)
			{
				var sut = new TestableEqualityComparerBase<object>(
					shouldHandleNullsInEquals,
					shouldHandleNullsInGetHashes);

				Assert.Equal(shouldHandleNullsInEquals,
					sut.HandleNullsInEquals);
				Assert.Equal(shouldHandleNullsInGetHashes,
					sut.HandleNullsInGetHashes);
			}

			private class TestableEqualityComparerBase<T> : EqualityComparerBase<T>
			{
				public TestableEqualityComparerBase(
					bool shouldHandleNullsInEquals,
					bool shouldHandleNullsInGetHashes)
					: base(shouldHandleNullsInEquals,
						  shouldHandleNullsInGetHashes)
				{
				}

				public bool HandleNullsInEquals => base.HandleNullsInEquals;

				public bool HandleNullsInGetHashes => base.HandleNullsInGetHashes;
			}
		}
	}
}
