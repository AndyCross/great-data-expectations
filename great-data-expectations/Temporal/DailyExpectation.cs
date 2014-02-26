using System;
using GreatExpectations.Core;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Temporal
{
    public class DailyExpectation : IAmAnExpectation
    {
        private const string defaultFormat = "year={0}/month={1}/day={2}";
        private readonly ExpectationDescription _expectationDescription;
        private readonly DateTime _epoch;
        public DailyExpectation(ExpectationDescription expectationDescription, DateTime epoch)
        {
            _expectationDescription = expectationDescription;
            _epoch = epoch;
        }

        public string GetRelativeAddress()
        {
            var prefix = _expectationDescription.Prefix;
            var format = defaultFormat;
            if (!string.IsNullOrEmpty(_expectationDescription.CustomVariableFormat))
            {
                format = _expectationDescription.CustomVariableFormat;
            }

            var variablePortion = string.Format(format, _epoch.Year.ToString("##").Substring(2), _epoch.Month.ToString("00"), _epoch.Day.ToString("00"));

            return string.Join("/", prefix, variablePortion);
        }

        public bool IsInDataIngressWindow()
        {
            return _epoch > (DateTime.Now - _expectationDescription.DataIngressTimespan);
        }

        public ExpectationDescription Description {get { return _expectationDescription; }}
        public DateTime IncrementEpoch(DateTime current)
        {
            return current.AddDays(1D);
        }

        public DateTime Epoch {get { return _epoch; }}
    }
}