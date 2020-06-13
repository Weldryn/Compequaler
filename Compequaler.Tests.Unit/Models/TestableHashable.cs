using Compequaler.Equality.Hash;
using Compequaler.Equality.Hash.Implementations;
using System;
using System.Collections.Generic;

namespace Compequaler.Tests.Unit.Models
{
	public class TestableHashable : IRuntimeHashable, IEquatable<TestableHashable>
	{
		public TestableHashable(Func<RuntimeHash, RuntimeHash> hashDelegate)
		{
			HashDelegate = hashDelegate;
		}

		public Func<RuntimeHash, RuntimeHash> HashDelegate { get; }

		public RuntimeHash GetRuntimeHash(RuntimeHash seed)
			=> HashDelegate(seed);

		public static TestableHashable Create<T>(T value, IEqualityComparer<T> equalityComparer)
			=> new TestableHashable(seed => seed.Hash(value, equalityComparer));

		public static TestableHashable Create<T>(T value)
			=> new TestableHashable(seed => seed.Hash(value));

		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			if (ReferenceEquals(this, obj)) return true;

			return Equals(obj as TestableHashable);
		}

		public bool Equals(TestableHashable other)
		{
			if (other == null) return false;
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(HashDelegate, other.HashDelegate)) return true;

			return false;
		}
	}
}
