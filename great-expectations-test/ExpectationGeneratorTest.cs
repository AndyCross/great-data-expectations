using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations;
using GreatExpectations.Generation;
using Xunit;

using Assert = Xunit.Assert;

namespace great_expectations_test
{
    public class ExpectationGeneratorTest
    {
        public class TheGenerateMethod
        {
            [Fact]
            public void Returns24ExpecationsADayForHourly()
            {
                var generator = new ExpectationGenerator();
                var expectations = generator.GenerateExpectations(ExpectationFrequency.Hourly, DateTime.Now.AddDays(-1D).AddSeconds(1D), DateTime.Now, "iislogs/dataset=cdsagenttest/webserver=web01", 1, 1, customVariableFormat: "");

                Assert.Equal(24, expectations.Count());
            }
            [Fact]
            public void Returns1ExpecationsADayForDaily()
            {
                var generator = new ExpectationGenerator();
                var expectations = generator.GenerateExpectations(ExpectationFrequency.Daily, DateTime.Now.AddDays(-1D).AddSeconds(1D), DateTime.Now, "iislogs/dataset=cdsagenttest/webserver=web01", 1, 1, customVariableFormat: "");

                Assert.Equal(1, expectations.Count());
            }
        }
    }
}
