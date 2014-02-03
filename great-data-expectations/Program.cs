using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations.Core;
using GreatExpectations.Generation;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations
{
    class Program
    {
        static void Main(string[] args)
        {
            var expectationGenerator = new ExpectationGenerator();
            var expectations = expectationGenerator.GeneratExpectations(DateTime.Now.AddHours(-24D), DateTime.Now);

            AssertExpectations(expectations);

            Console.ReadLine();
        }

        private static void AssertExpectations(IEnumerable<IAmAnExpectation> expectations)
        {
            var storageAccount =
                CloudStorageAccount.Parse(
                    "DefaultEndpointsProtocol=https;AccountName=elastacloudne;AccountKey=x8hhFGPKXs7Dht8Aq8tBTE0dCVpZ9tMC7UGTJ2Bws9pnmtyNi5DatRlRhYW6n12PN8mVkDuckvzitVkxW0sVZw==");
            var container = storageAccount.CreateCloudBlobClient().GetContainerReference("cdsagenttest");

            foreach (var anExpectation in expectations)
            {
                var expectedVirtualPath = anExpectation.GetRelativeAddress();
                var target = container.GetDirectoryReference(expectedVirtualPath).ListBlobs().ToArray();

                if (target.Count() > anExpectation.Description.MaxFileExpectation ||
                    target.Count() < anExpectation.Description.MinFileExpectation)
                {
                    if (anExpectation.IsInDataIngressWindow())
                    {
                        Console.WriteLine("The expectation passed with a warning; the target virtual path {0} does not yet exist but is within its data ingress window. There is {1} remaining. ", expectedVirtualPath, (DateTime.Now + anExpectation.Description.DataIngressTimespan) - DateTime.Now);
                    }
                    else
                    {
                        Console.WriteLine(
                            "The expectation failed to be met: Expected {0} min and {1} max, Actual: {2}. Target virtual path: {3}",
                            anExpectation.Description.MinFileExpectation, anExpectation.Description.MaxFileExpectation,
                            target.Count(), expectedVirtualPath);
                    }
                }

                foreach (var listBlobItem in target)
                {
                    Console.WriteLine(listBlobItem.Uri);
                }
            }
        }
    }
}
