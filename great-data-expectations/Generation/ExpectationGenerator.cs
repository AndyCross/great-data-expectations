using System;
using System.Collections.Generic;
using GreatExpectations.Core;
using GreatExpectations.Temporal;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Generation
{
    public class ExpectationGenerator
    {
        public IEnumerable<IAmAnExpectation> GeneratExpectations(DateTime startDateTime, DateTime endDateTime)
        {
            List<IAmAnExpectation> expectations = new List<IAmAnExpectation>();

            DateTime current = startDateTime;
            while (current <= endDateTime)
            {
                expectations.Add(new HourlyExpectation(new ExpectationDescription()
                        {
                            Frequency = ExpectationFrequency.Hourly, MinFileExpectation = 1, MaxFileExpectation = 1,
                            Prefix = "iislogs/dataset=cdsagenttest/webserver=web01"
                        }, 
                        current));
                current = current.AddHours(1D);
            }

            return expectations;
        }
    }
}