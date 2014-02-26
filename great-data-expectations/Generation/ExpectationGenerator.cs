using System;
using System.Collections.Generic;
using GreatExpectations.Core;
using GreatExpectations.Temporal;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Generation
{
    public class ExpectationGenerator : IExpectationGenerator
    {
        public IEnumerable<IAmAnExpectation> GenerateExpectations(ExpectationFrequency frequency, DateTime startDateTime, DateTime endDateTime, string dataPathPrefix, int minFileExpectation, int maxFileExpectation, string customVariableFormat = "")
        {
            List<IAmAnExpectation> expectations = new List<IAmAnExpectation>();

            DateTime current = startDateTime;
            while (current <= endDateTime)
            {
                var expectation = ExpectationFactory.Build(frequency, dataPathPrefix, minFileExpectation, maxFileExpectation, customVariableFormat, current);
                expectations.Add(expectation);
                current = expectation.IncrementEpoch(current);
            }

            return expectations;
        }
    }
}