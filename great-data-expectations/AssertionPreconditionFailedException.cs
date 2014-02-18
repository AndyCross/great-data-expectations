using System;

namespace GreatExpectations
{
    public class AssertionPreconditionFailedException : Exception
    {
        public AssertionPreconditionFailedException(string message) : base(message)
        {
        }
    }
}