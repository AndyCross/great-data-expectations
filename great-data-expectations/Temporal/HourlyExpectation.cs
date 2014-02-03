using System;
using GreatExpectations.Core;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Temporal
{
    public class HourlyExpectation : IAmAnExpectation
    {
        private readonly ExpectationDescription _expectationDescription;
        private readonly DateTime _epoch;
        public HourlyExpectation(ExpectationDescription expectationDescription, DateTime epoch)
        {
            _expectationDescription = expectationDescription;
            _epoch = epoch;
        }

        public string GetRelativeAddress()
        {
            var prefix = _expectationDescription.Prefix;
            var variablePortion = string.Format("year={0}/month={1}/day={2}/hour={3}", _epoch.Year.ToString("##").Substring(2), _epoch.Month.ToString("00"), _epoch.Day.ToString("00"), _epoch.Hour.ToString("00"));

            return string.Join("/", prefix, variablePortion);
        }

        public bool IsInDataIngressWindow()
        {
            return _epoch > (DateTime.Now - _expectationDescription.DataIngressTimespan);
        }

        public ExpectationDescription Description {get { return _expectationDescription; }}
    }
}