using Compequaler.Equality.Hash;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Compequaler.Tests.Unit.Equality.Hash
{
    public class FNV1a32bAlgorithmTests
    {
        [Theory]
        [InlineData(FNV1aAlgorithm._factor32bits, 1, 0)]
        [InlineData(FNV1aAlgorithm._factor32bits, 0, 1)]
        [InlineData(unchecked(0xFF01 * FNV1aAlgorithm._factor32bits), 0xFF00, 1)]
        [InlineData(unchecked(0xFF01 * FNV1aAlgorithm._factor32bits), 1, 0xFF00)]
        [InlineData(unchecked(0xFF_FF01 * FNV1aAlgorithm._factor32bits), 0xFF_FF00, 1)]
        [InlineData(unchecked(0xFF_FF01 * FNV1aAlgorithm._factor32bits), 1, 0xFF_FF00)]
        [InlineData(unchecked(0xFF_00FF * FNV1aAlgorithm._factor32bits), 0xFF_0000, 0xFF)]
        [InlineData(unchecked(0xFF_FFFF * FNV1aAlgorithm._factor32bits), 0xFF, 0xFF_FF00)]
        public void ShouldApplyFNV1aFast(uint expectedResult, uint hashes, int hashCode)
            => Assert.Equal(expectedResult, FNV1aAlgorithm.FNV1a32Fast(hashes, hashCode));

        [Theory]
        [InlineData(unchecked(FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 1, 0)]
        [InlineData(unchecked(FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 0, 1)]
        [InlineData(unchecked(0xFF01 * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 0xFF00, 1)]
        [InlineData(unchecked((FNV1aAlgorithm._factor32bits ^ 0xFF) * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 1, 0xFF00)]
        [InlineData(unchecked(0xFF_FF01 * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 0xFF_FF00, 1)]
        [InlineData(unchecked(((FNV1aAlgorithm._factor32bits ^ 0xFF) * FNV1aAlgorithm._factor32bits ^ 0xFF) * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 1, 0xFF_FF00)]
        [InlineData(unchecked(0xFF_0000 * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 0xFF_00FF, 0xFF)]
        [InlineData(unchecked(0xFF * FNV1aAlgorithm._factor32bits * FNV1aAlgorithm._factor32bits), 0xFF, 0xFF_00FF)]
        public void ShouldApplyFNV1a(uint expectedResult, uint hashes, int hashCode)
            => Assert.Equal(expectedResult, FNV1aAlgorithm.FNV1a32(hashes, hashCode));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(0x0FFF_FFFF, 0x0FFFF_FFF)]
        [InlineData(0, 0x8000_0001)]
        [InlineData(2, 0x8000_0003)]
        [InlineData(0x7FFF_FFFE, 0xFFFF_FFFF)]
        public void ShouldXORFold31b(int expectedResult, uint hashes)
            => Assert.Equal(expectedResult, FNV1aAlgorithm.XORFold31(hashes));
    }
}
