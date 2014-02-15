using System;

namespace GreatExpectations.Tenancy
{
    public class ExpectationDescription
    {
        public ExpectationFrequency Frequency { get; set; }
        public string Prefix { get; set; }
        public int MinFileExpectation { get; set; }
        public int MaxFileExpectation { get; set; }
        public TimeSpan DataIngressTimespan { get; set; }
        public string CustomVariableFormat { get; set; }
    }

    public enum ExpectationFrequency
    {
        Hourly
    }
}