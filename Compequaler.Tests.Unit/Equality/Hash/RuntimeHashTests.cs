using AutoFixture;
using AutoFixture.Xunit2;
using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Compequaler.Tests.Unit.Equality.Hash
{
    public partial class RuntimeHashTests
    {
        public class Instance
        {
            public Instance()
            {
                Fixture = new Fixture();
                Fixture.Register(() => new RuntimeHash(Fixture.Create<uint>()));
            }

            private Fixture Fixture { get; }

            [Fact]
            public void CastToIntShouldBeSameAsGetHashCode()
            {
                var sut = Fixture.Create<RuntimeHash>();
                Assert.Equal(sut, sut.GetHashCode());
            }

            [Theory]
            [InlineAutoData]
            public void SameHashesShouldEquate(uint hashes)
            {
                var sut1 = new RuntimeHash(hashes);
                var sut2 = new RuntimeHash(hashes);
                Assert.Equal(sut1.Hashes, sut2.Hashes);
                Assert.True(sut1.Equals(sut2));
            }

            [Fact]
            public void DifferentHashesShouldNotEquate()
            {
                var sut1 = Fixture.Create<RuntimeHash>();
                var sut2 = Fixture.Create<RuntimeHash>();
                Assert.NotEqual(sut1.Hashes, sut2.Hashes);
                Assert.False(sut1.Equals(sut2));
            }

            [Theory]
            [InlineAutoData]
            public void SameGenericHashesShouldEquate(uint hashes)
            {
                var sut1 = new RuntimeHash(hashes);
                var sut2 = new RuntimeHash(hashes);
                Assert.Equal(sut1.Hashes, sut2.Hashes);
                Assert.True(sut1.Equals((object)sut2));
            }

            [Fact]
            public void DifferentGenericHashesShouldNotEquate()
            {
                var sut1 = Fixture.Create<RuntimeHash>();
                var sut2 = Fixture.Create<RuntimeHash>();
                Assert.NotEqual(sut1.Hashes, sut2.Hashes);
                Assert.False(sut1.Equals((object)sut2));
            }

            [Fact]
            public void DifferentTypeShouldThrowOnEquals()
            {
                var sut = Fixture.Create<RuntimeHash>();
                Assert.ThrowsAny<ArgumentException>(() => sut.Equals(new object()));
            }

            [Fact]
            public void GenericNullShouldNotThrowOnEquals()
            {
                var sut = Fixture.Create<RuntimeHash>();
                Assert.False(sut.Equals(null));
            }
        }
    }
}
