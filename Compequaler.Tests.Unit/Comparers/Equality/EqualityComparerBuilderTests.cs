using Compequaler.Comparers.Equality;
using Compequaler.Tests.Unit.Models;
using System;
using System.Collections.Generic;
using Xunit;

namespace Compequaler.Tests.Unit.Comparers.Equality
{
	public class EqualityComparerBuilderTests
	{
		[Fact]
		public void Test()
		{
			IEqualityComparerBuilder<ModelRoot> builder = null;

		}
	}
}

namespace Compequaler.Comparers.Equality
{
	public interface IEqualityComparerBuilder<in T>
	{

	}
}

namespace Compequaler.Tests.Unit.Models
{
	public class Model3
	{
		public Version Version { get; set; }

		public double Double { get; set; }

		public List<Model2> Model2s { get; set; }
	}

	public class Model2
	{
		public string String { get; set; }

		public int Integer { get; set; }
	}

	public class ModelRoot
	{
		public string String { get; set; }

		public Model3 Model3 { get; set; }

		public Dictionary<string, Model2> Model2PerStrings { get; set; }
	}
}
