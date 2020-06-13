using System;
using System.Linq.Expressions;

namespace Compequaler.Tests.Unit
{
	[Serializable]
    public class TestException : Exception
    {
        public TestException() { }
        public TestException(string message) : base(message) { }
        public TestException(string message, Exception inner) : base(message, inner) { }
        protected TestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    
    [Serializable]
    public class FailedTestException : TestException
    {
        public FailedTestException() { }
        public FailedTestException(Expression<Func<bool>> expr) : base(expr.Body.ToString()) { }
        public FailedTestException(string message) : base(message) { }
        public FailedTestException(string message, Exception inner) : base(message, inner) { }
        protected FailedTestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class SuccessfulTestException : TestException
    {
        public SuccessfulTestException() { }
        public SuccessfulTestException(string message) : base(message) { }
        public SuccessfulTestException(string message, Exception inner) : base(message, inner) { }
        protected SuccessfulTestException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }

	[Serializable]
	public class SetupTestException : TestException
	{
		public SetupTestException() { }
		public SetupTestException(string message) : base(message) { }
		public SetupTestException(string message, Exception inner) : base(message, inner) { }
		protected SetupTestException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
	}
}
