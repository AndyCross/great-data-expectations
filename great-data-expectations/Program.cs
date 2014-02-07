using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations.Generation;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var storageAccount=CloudStorageAccount.Parse(
                "DefaultEndpointsProtocol=https;AccountName=elastacloudne;AccountKey=x8hhFGPKXs7Dht8Aq8tBTE0dCVpZ9tMC7UGTJ2Bws9pnmtyNi5DatRlRhYW6n12PN8mVkDuckvzitVkxW0sVZw==");
            var containerName = "cdsagenttest";

            // The use of EpochPersistence allows repeated and incremental expectation checking. Be sure to call EpochPersistence.SetLastSatisfied
            // note that this is optional in the workflow and instead arbitrary datetimes can be passed to expectationGenerator.GenerateExpectations
            var lastExecutionEpoch = EpochPeristence.GetLastSatisfied(storageAccount, containerName);

            // Create an instance of the ExpectationGenerator; note that this is instance to allow for variant implementations
            // Not yet implemented, but planned for predicatable but not date-oriented generators and different temporal durations
            var expectationGenerator = new ExpectationGenerator();
            var expectations = expectationGenerator.GenerateExpectations(lastExecutionEpoch, DateTime.Now);

            // This is a blocking call that iterates over the IAmAnExpection[] and returns Assertions (Results alongside Expectations)
            var assertions = Assert.Expectations(expectations, storageAccount, containerName).ToArray();
            foreach (var assertion in assertions)
            {
                Console.WriteLine("{0}: {1}", assertion.Result, assertion.Message);
            }
            
            // If incremental expectations are required, set a satisfied bit on the storage container, allowing the next iterations to 
            // start from where this timepoint ended
            EpochPeristence.SetLastSatisfied(storageAccount, containerName,
                assertions.OrderByDescending(a => a.Raw.Epoch).First(a => a.Result == AssertionResult.Success));

            Console.ReadLine();
        }

    }
}
