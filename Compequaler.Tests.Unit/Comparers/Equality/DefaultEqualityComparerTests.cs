using AutoFixture;
using Compequaler.Comparers.Equality.Implementations;
using Compequaler.Equality.Hash.Implementations;
using System;
using System.Collections;
using System.Collections.Generic;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
    public class DefaultEqualityComparerTests
    {
        public static IEnumerable<object[]> GetParams(int hashesCount)
        {
            var fixture = new Fixture();
            var testParam = new List<object>(hashesCount + 1) { new RuntimeHashDefaultEqualityComparer() };

            for (int i = 0; i < hashesCount; i++) testParam.Add(fixture.Create<uint>());

            yield return testParam.ToArray();
        }

        [Theory]
        [MemberData(nameof(GetParams), 1)]
        public void EqualsShouldEquateOnSameHashes(IEqualityComparer<RuntimeHash> comparer, uint hashes)
        {
            var hash = new RuntimeHash(hashes);
            Assert.True(comparer.Equals(hash, hash));
        }

        [Theory]
        [MemberData(nameof(GetParams), 2)]
        public void EqualsShouldNotEquateOnDifferentHashes(IEqualityComparer<RuntimeHash> comparer, uint hashes1, uint hashes2)
        {
            if (hashes1 == hashes2) throw new TestInconclusiveException(() => hashes1 == hashes2);

            var hash1 = new RuntimeHash(hashes1);
            var hash2 = new RuntimeHash(hashes2);
            Assert.False(comparer.Equals(hash1, hash2));
        }

        [Theory]
        [MemberData(nameof(GetParams), 0)]
        public void NonGenericEqualsShouldEquateOnBothNulls(IEqualityComparer comparer)
        {
            Assert.True(comparer.Equals(null, null));
        }

        [Theory]
        [MemberData(nameof(GetParams), 0)]
        public void NonGenericEqualsShouldNotEquateOnOneNullObject(IEqualityComparer comparer)
        {
            Assert.False(comparer.Equals(new object(), null));
            Assert.False(comparer.Equals(null, 0));
        }

        [Theory]
        [MemberData(nameof(GetParams), 2)]
        public void NonGenericEqualsShouldThrowOnOtherObjects(IEqualityComparer comparer, uint hashes1, uint hashes2)
        {
            if (hashes1 == hashes2) throw new TestInconclusiveException(() => hashes1 == hashes2);

            var hash1 = new RuntimeHash(hashes1);
            Assert.Throws<ArgumentException>(() => comparer.Equals(hash1, hashes2));
            Assert.Throws<ArgumentException>(() => comparer.Equals(hash1, new object()));
            Assert.Throws<ArgumentException>(() => comparer.Equals(hashes1, new object()));
        }

        [Theory]
        [MemberData(nameof(GetParams), 1)]
        public void NonGenericGetHashCodeShouldEquateOnSameHashes(IEqualityComparer comparer, uint hashes)
        {
            var expectedHashCode = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, hashes));
            var hashCode = comparer.GetHashCode(new RuntimeHash(hashes));
            Assert.Equal(expectedHashCode, hashCode);
        }

        [Theory]
        [MemberData(nameof(GetParams), 2)]
        public void NonGenericGetHashCodeShouldNotEquateOnDifferentHashes(IEqualityComparer comparer, uint hashes1, uint hashes2)
        {
            if (hashes1 == hashes2) throw new TestInconclusiveException(() => hashes1 == hashes2);

            var first = comparer.GetHashCode(new RuntimeHash(hashes1));
            var second = comparer.GetHashCode(new RuntimeHash(hashes2));
            Assert.NotEqual(first, second);
        }

        [Theory]
        [MemberData(nameof(GetParams), 0)]
        public void NonGenericGetHashCodeShouldReturnZero(IEqualityComparer comparer)
        {
            var expectedHashCode = FNV1aAlgorithm.XORFold31(FNV1aAlgorithm.FNV(FNV1aAlgorithm._seed32bits, 0));
            var hashCode = comparer.GetHashCode(null);
            Assert.Equal(expectedHashCode, hashCode);
        }

        [Theory]
        [MemberData(nameof(GetParams), 0)]
        public void NonGenericGetHashCodeShouldThrow(IEqualityComparer comparer)
        {
            Assert.Throws<ArgumentException>(() => comparer.GetHashCode(new object()));
            Assert.Throws<ArgumentException>(() => comparer.GetHashCode(1));
        }
    }
}
