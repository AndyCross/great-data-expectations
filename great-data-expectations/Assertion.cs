using System.Diagnostics;
using System.Globalization;
using GreatExpectations.Core;

namespace GreatExpectations
{
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Assertion
    {
        private IAmAnExpectation _raw;
        private readonly AssertionResult _result;
        private readonly string _message;

        public Assertion(IAmAnExpectation raw, AssertionResult result, string message)
        {
            _raw = raw;
            _result = result;
            _message = message;
        }

        public IAmAnExpectation Raw
        {
            get { return _raw; }
        }

        public AssertionResult Result
        {
            get { return _result; }
        }

        public string Message
        {
            get { return _message; }
        }

        public string DebuggerDisplay
        {
            get
            {
                return string.Format(CultureInfo.InvariantCulture,
                    "{0} for date {1}", Result, _raw.Epoch);
            }
        }
    }
}