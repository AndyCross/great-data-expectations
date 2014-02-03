using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=elastacloudne;AccountKey=x8hhFGPKXs7Dht8Aq8tBTE0dCVpZ9tMC7UGTJ2Bws9pnmtyNi5DatRlRhYW6n12PN8mVkDuckvzitVkxW0sVZw==");
            var container = storageAccount.CreateCloudBlobClient().GetContainerReference("cdsagenttest");

            foreach (var anExpectation in expectations)
            {
                var expectedVirtualPath = anExpectation.GetRelativeAddress();
                var subDirFiles = container.GetDirectoryReference(expectedVirtualPath).ListBlobs();
                foreach (var listBlobItem in subDirFiles)
                {
                    Console.WriteLine(listBlobItem.Uri);
                }
            }

            Console.ReadLine();
        }

    }
}
