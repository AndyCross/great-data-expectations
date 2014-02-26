using System;
using GreatExpectations.Core;
using GreatExpectations.Temporal;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Generation
{
    public class ExpectationFactory
    {
        public static IAmAnExpectation Build(ExpectationFrequency pathPrefix, string dataPathPrefix, int minFileExpectation, int maxFileExpectation, string customVariableFormat, DateTime current)
        {
            IAmAnExpectation built;
            switch (pathPrefix)
            {
                case ExpectationFrequency.Hourly:
                    built = new HourlyExpectation(new ExpectationDescription()
                    {
                        Frequency = ExpectationFrequency.Hourly,
                        MinFileExpectation = minFileExpectation,
                        MaxFileExpectation = maxFileExpectation,
                        Prefix = dataPathPrefix,
                        DataIngressTimespan = TimeSpan.FromMinutes(60.1D),
                        CustomVariableFormat = customVariableFormat
                    }, current);
                    break;
                case ExpectationFrequency.Daily:
                    built = new DailyExpectation(new ExpectationDescription()
                    {
                        Frequency = ExpectationFrequency.Hourly,
                        MinFileExpectation = minFileExpectation,
                        MaxFileExpectation = maxFileExpectation,
                        Prefix = dataPathPrefix,
                        DataIngressTimespan = TimeSpan.FromMinutes(60.1D),
                        CustomVariableFormat = customVariableFormat
                    }, current);
                    break;
                case ExpectationFrequency.Weekly:
                case ExpectationFrequency.Monthly:
                    throw new NotImplementedException();
                default:
                    throw new ArgumentOutOfRangeException("pathPrefix");
            }

            return built;
        }
    }
}