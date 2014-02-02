using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;

namespace great_data_expectations
{
    class Program
    {
        static void Main(string[] args)
        {
            var expectations = ExpectationGenerator.GeneratExpectations(DateTime.Now.AddHours(-24D), DateTime.Now);

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

    public interface IAmAnExpectation
    {
        string GetRelativeAddress();
    }

    public class HourlyExpectation : IAmAnExpectation
    {
        private readonly DateTime _epoch;
        public HourlyExpectation(DateTime epoch)
        {
            _epoch = epoch;
        }

        public string GetRelativeAddress()
        {
            return string.Format("iislogs/dataset=cdsagenttest/webserver=web01/year={0}/month={1}/day={2}/hour={3}", _epoch.Year.ToString("##").Substring(2), _epoch.Month.ToString("00"), _epoch.Day.ToString("00"), _epoch.Hour.ToString("00"));
        }
    }

    public class ExpectationGenerator
    {
        public static IEnumerable<IAmAnExpectation> GeneratExpectations(DateTime startDateTime, DateTime endDateTime)
        {
            List<IAmAnExpectation> expectations = new List<IAmAnExpectation>();

            DateTime current = startDateTime;
            while (current <= endDateTime)
            {
                expectations.Add(new HourlyExpectation(current));
                current = current.AddHours(1D);
            }

            return expectations;
        }
    }
}
