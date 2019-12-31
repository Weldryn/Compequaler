using Compequaler.Hash;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Compequaler.Tests.Unit.Hash
{
    public class FNV32bAlgorithmTests
    {
        public FNV32bAlgorithmTests(ITestOutputHelper output)
        {
            Output = output;
        }

        private ITestOutputHelper Output { get; }

        [Theory]
        [InlineData(FNVAlgorithm._factor32, 1, 0)]
        [InlineData(FNVAlgorithm._factor32, 0, 1)]
        [InlineData(unchecked(0xFF01 * FNVAlgorithm._factor32), 0xFF00, 1)]
        [InlineData(unchecked(0xFF01 * FNVAlgorithm._factor32), 1, 0xFF00)]
        [InlineData(unchecked(0xFF_FF01 * FNVAlgorithm._factor32), 0xFF_FF00, 1)]
        [InlineData(unchecked(0xFF_FF01 * FNVAlgorithm._factor32), 1, 0xFF_FF00)]
        [InlineData(unchecked(0xFF_00FF * FNVAlgorithm._factor32), 0xFF_0000, 0xFF)]
        [InlineData(unchecked(0xFF_FFFF * FNVAlgorithm._factor32), 0xFF, 0xFF_FF00)]
        public void ShouldApplyFNVFast(int expectedResult, int hashes, int hashCode)
            => Assert.Equal(expectedResult, FNVAlgorithm.FNV1a32Fast(hashes, hashCode));

        [Theory]
        [InlineData(unchecked(FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 1, 0)]
        [InlineData(unchecked(FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 0, 1)]
        [InlineData(unchecked(0xFF01 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 0xFF00, 1)]
        [InlineData(unchecked((FNVAlgorithm._factor32 ^ 0xFF) * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 1, 0xFF00)]
        [InlineData(unchecked(0xFF_FF01 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 0xFF_FF00, 1)]
        [InlineData(unchecked(((FNVAlgorithm._factor32 ^ 0xFF) * FNVAlgorithm._factor32 ^ 0xFF) * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 1, 0xFF_FF00)]
        [InlineData(unchecked(0xFF_0000 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 0xFF_00FF, 0xFF)]
        [InlineData(unchecked(0xFF * FNVAlgorithm._factor32 * FNVAlgorithm._factor32), 0xFF, 0xFF_00FF)]
        public void ShouldApplyFNV(int expectedResult, int hashes, int hashCode)
            => Assert.Equal(expectedResult, FNVAlgorithm.FNV1a32(hashes, hashCode));

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1, 1)]
        [InlineData(0x0FFF_FFFF, 0x0FFFF_FFF)]
        [InlineData(0, unchecked((int)0x8000_0001))]
        [InlineData(2, unchecked((int)0x8000_0003))]
        [InlineData(0x7FFF_FFFE, unchecked((int)0xFFFF_FFFF))]
        public void ShouldXORFold(int expectedResult, int hashes)
            => Assert.Equal(expectedResult, FNVAlgorithm.XORFolding32to31(hashes));
    }
}
