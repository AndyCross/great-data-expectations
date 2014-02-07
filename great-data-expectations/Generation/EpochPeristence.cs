using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations.Generation
{
    internal class EpochPeristence
    {

        public static DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName, DateTime defaultDateTime)
        {
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            containerReference.FetchAttributes();
            DateTime epoch;

            if (DateTime.TryParse(containerReference.Metadata["misshaversham"], out epoch))
            {
                return epoch;
            }

            return defaultDateTime;
        }

        public static DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName)
        {
            return GetLastSatisfied(storageAccount, containerName,
                DateTime.Now.AddHours(-24D).AddMinutes(DateTime.Now.Minute*-1D));
        }

        public static void SetLastSatisfied(CloudStorageAccount storageAccount, string containerName, Assertion assertion)
        {
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            containerReference.FetchAttributes();
            DateTime epoch = assertion.Raw.Epoch;

            containerReference.Metadata["misshaversham"] = epoch.ToString();
            containerReference.SetMetadata();
        }
    }
}
