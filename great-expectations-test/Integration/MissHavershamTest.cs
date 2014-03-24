using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations;
using GreatExpectations.Core;
using GreatExpectations.Generation;
using Microsoft.WindowsAzure.Storage;
using Xunit;

namespace great_expectations_test.Integration
{
    public class MissHavershamTest
    {
        public class TheAssertMethod
        {
            [Fact]
            public void SensibleForDateRange()
            {
                var storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("greatexpectationsconnectionstring"));

                var storage = storageAccount.CreateCloudBlobClient().ListContainers().ToArray();

                var assertions =
                    MissHaversham.Assert(ExpectationFrequency.Hourly,
                        storageAccount, "cdsagenttest", "iislogs/dataset=cdsagenttest/webserver=web01", "exampleJobName", 1, 1,
                        string.Empty, DateTime.Now.AddDays(-1D), DateTime.Now);

                Xunit.Assert.NotEmpty(assertions);
            }

            [Fact]
            public void SensibleForEpochPersistence()
            {
                var storageAccount = CloudStorageAccount.Parse(Environment.GetEnvironmentVariable("greatexpectationsconnectionstring"));

                var storage = storageAccount.CreateCloudBlobClient().ListContainers().ToArray();

                var assertions =
                    MissHaversham.Assert(ExpectationFrequency.Hourly,
                        storageAccount, "cdsagenttest", "iislogs/dataset=cdsagenttest/webserver=web01", "exampleJobName", 1, 1,
                        string.Empty);

                Xunit.Assert.NotEmpty(assertions);
            }

            [Fact]
            public void LiveishEpochPersistence()
            {
                var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=asoshdinsight;AccountKey=wW4yKx4JMo7wMTbStV09RVmvlY6bwFpbqqtgS9ch2IuPJARbodocJ1lOcISUQYkkmmx1OLfVxEZhxdDVQ08Rhg==");
                
                var assertions =
                    MissHaversham.Assert(ExpectationFrequency.Daily,
                        storageAccount, "webtrends", "output/SessionSummary/Processed/Complete/ASOS Main Site", "exampleJobName", 1, 5,
                        "20{0}/{1}/{2}");


                Xunit.Assert.NotEmpty(assertions);

            }
        }
    }
}
