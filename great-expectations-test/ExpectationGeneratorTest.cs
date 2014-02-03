using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations;
using GreatExpectations.Generation;
using Xunit;

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
                var expectations = generator.GeneratExpectations(DateTime.Now.AddDays(-1D).AddSeconds(1D), DateTime.Now);

                Assert.Equal(24, expectations.Count());
            }
        }
    }
}
