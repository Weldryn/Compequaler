using AutoFixture.Xunit2;
using Compequaler.Equality.Hash;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Compequaler.Tests.Unit.Equality.Hash
{
    public class RuntimeHashEqualityComparerTests
    {
        [Theory]
        [InlineAutoData]
        public void EqualsShouldEquateOnSameHashes(uint hashes)
        {
            var comparer = new RuntimeHashEqualityComparer();
            var hash = new RuntimeHash(hashes);
            Assert.True(comparer.Equals(hash, hash));
        }

        [Theory]
        [InlineAutoData]
        public void EqualsShouldNotEquateOnDifferentHashes(uint hashes1, uint hashes2)
        {
            if (hashes1 == hashes2) throw new TestInconclusiveException(() => hashes1 == hashes2);

            var hash1 = new RuntimeHash(hashes1);
            var hash2 = new RuntimeHash(hashes2);
            var comparer = new RuntimeHashEqualityComparer();
            Assert.False(comparer.Equals(hash1, hash2));
        }

        [Theory]
        [InlineAutoData]
        public void NonGenericEqualsShouldEquateOnSameHashes(uint hashes)
        {
            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            var hash = new RuntimeHash(hashes);
            Assert.True(comparer.Equals(hash, hash));
        }

        [Theory]
        [InlineAutoData]
        public void NonGenericEqualsShouldNotEquateOnDifferentHashes(uint hashes1, uint hashes2)
        {
            if (hashes1 == hashes2) throw new TestInconclusiveException(() => hashes1 == hashes2);

            var hash1 = new RuntimeHash(hashes1);
            var hash2 = new RuntimeHash(hashes2);
            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            Assert.False(comparer.Equals(hash1, hash2));
        }

        [Fact]
        public void NonGenericEqualsShouldEquateOnBothNulls()
        {
            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            Assert.True(comparer.Equals(null, null));
        }

        [Fact]
        public void NonGenericEqualsShouldNotEquateOnOneNullObject()
        {
            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            Assert.False(comparer.Equals(new object(), null));
            Assert.False(comparer.Equals(null, new Version()));
        }

        [Theory]
        [InlineAutoData]
        public void NonGenericEqualsShouldThrowOnOtherObjects(uint hashes1, uint hashes2)
        {
            var hash1 = new RuntimeHash(hashes1);
            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            Assert.Throws<ArgumentException>(() => comparer.Equals(hash1, hashes2));
            Assert.Throws<ArgumentException>(() => comparer.Equals(hash1, new object()));
            Assert.Throws<ArgumentException>(() => comparer.Equals(hashes1, new object()));
        }

        [Theory]
        [InlineAutoData]
        public void GetHashCodeShouldEquateOnSameHashes(uint hashes)
        {
            var comparer = new RuntimeHashEqualityComparer();
            Assert.Equal(new RuntimeHash(hashes).GetHashCode(),
                comparer.GetHashCode(new RuntimeHash(hashes)));
        }

        [Theory]
        [InlineAutoData]
        public void GetHashCodeShouldNotEquateOnDifferentHashes(uint hashes1, uint hashes2)
        {
            if (hashes1 == hashes2) throw new TestInconclusiveException(() => hashes1 == hashes2);

            var comparer = new RuntimeHashEqualityComparer();
            Assert.NotEqual(comparer.GetHashCode(new RuntimeHash(hashes1)),
                comparer.GetHashCode(new RuntimeHash(hashes2)));
        }

        [Theory]
        [InlineAutoData]
        public void NonGenericGetHashCodeShouldEquateOnSameHashes(uint hashes)
        {
            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            Assert.Equal(new RuntimeHash(hashes).GetHashCode(),
                comparer.GetHashCode(new RuntimeHash(hashes)));
        }

        [Theory]
        [InlineAutoData]
        public void NonGenericGetHashCodeShouldNotEquateOnDifferentHashes(uint hashes1, uint hashes2)
        {
            if (hashes1 == hashes2) throw new TestInconclusiveException(() => hashes1 == hashes2);

            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            Assert.NotEqual(comparer.GetHashCode(new RuntimeHash(hashes1)),
                comparer.GetHashCode(new RuntimeHash(hashes2)));
        }

        [Fact]
        public void NonGenericGetHashCodeShouldReturnZero()
        {
            IEqualityComparer comparer = new RuntimeHashEqualityComparer();
            Assert.Equal(0, comparer.GetHashCode(new object()));
            Assert.Equal(0, comparer.GetHashCode(null));
        }
    }
}
