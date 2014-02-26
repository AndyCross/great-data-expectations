using System;
using System.Collections.Generic;
using GreatExpectations.Core;

namespace GreatExpectations.Generation
{
    public interface IExpectationGenerator
    {
        IEnumerable<IAmAnExpectation> GenerateExpectations(ExpectationFrequency dateTime, DateTime startDateTime, DateTime endDateTime, string dataPathPrefix, int minFileExpectation, int maxFileExpectation, string customVariableFormat = "");
    }
}