using System;
using System.Collections.Generic;
using GreatExpectations.Core;
using GreatExpectations.Temporal;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Generation
{
    public class ExpectationGenerator
    {
        public IEnumerable<IAmAnExpectation> GenerateExpectations(DateTime startDateTime, DateTime endDateTime, string dataPathPrefix, int minFileExpectation, int maxFileExpectation, string customVariableFormat = "")
        {
            List<IAmAnExpectation> expectations = new List<IAmAnExpectation>();

            DateTime current = startDateTime;
            while (current <= endDateTime)
            {
                expectations.Add(new HourlyExpectation(new ExpectationDescription()
                        {
                            Frequency = ExpectationFrequency.Hourly, MinFileExpectation = minFileExpectation, MaxFileExpectation = maxFileExpectation,
                            Prefix = dataPathPrefix, DataIngressTimespan = TimeSpan.FromMinutes(60.1D), CustomVariableFormat = customVariableFormat
                        }, 
                        current));
                current = current.AddHours(1D);
            }

            return expectations;
        }
    }
}