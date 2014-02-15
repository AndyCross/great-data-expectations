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
            public void Returns24ExpecationsADay()
            {
                var generator = new ExpectationGenerator();
                var expectations = generator.GenerateExpectations(DateTime.Now.AddDays(-1D).AddSeconds(1D), DateTime.Now, "iislogs/dataset=cdsagenttest/webserver=web01", 1, 1, TODO);

                Assert.Equal(24, expectations.Count());
            }
        }
    }
}
