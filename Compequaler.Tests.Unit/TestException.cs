using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

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
    public class TestInconclusiveException : TestException
    {
        public TestInconclusiveException() { }
        public TestInconclusiveException(Expression<Func<bool>> expr) : base(expr.Body.ToString()) { }
        public TestInconclusiveException(string message) : base(message) { }
        public TestInconclusiveException(string message, Exception inner) : base(message, inner) { }
        protected TestInconclusiveException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
