using System;
using System.Collections.Generic;
using GreatExpectations.Core;
using GreatExpectations.Temporal;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Generation
{
    public class ExpectationGenerator
    {
        public IEnumerable<IAmAnExpectation> GenerateExpectations(DateTime startDateTime, DateTime endDateTime)
        {
            List<IAmAnExpectation> expectations = new List<IAmAnExpectation>();

            DateTime current = startDateTime;
            while (current <= endDateTime)
            {
                expectations.Add(new HourlyExpectation(new ExpectationDescription()
                        {
                            Frequency = ExpectationFrequency.Hourly, MinFileExpectation = 1, MaxFileExpectation = 1,
                            Prefix = "iislogs/dataset=cdsagenttest/webserver=web01", DataIngressTimespan = TimeSpan.FromMinutes(60.1D)
                        }, 
                        current));
                current = current.AddHours(1D);
            }

            return expectations;
        }
    }
}