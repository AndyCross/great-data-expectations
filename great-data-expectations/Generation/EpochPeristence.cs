using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations.Core;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations.Generation
{
    internal class EpochPeristence : IEpochPeristence
    {
        private const string metadataKeyName = "misshaversham";
        public DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName, DateTime defaultDateTime)
        {
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            containerReference.FetchAttributes();

            if (containerReference.Metadata.ContainsKey(metadataKeyName))
            {
                DateTime epoch;

                if (DateTime.TryParseExact(containerReference.Metadata[metadataKeyName], "g",
                    DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out epoch))
                {
                    return epoch;
                }
            }

            return defaultDateTime;
        }

        public DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName)
        {
            return GetLastSatisfied(storageAccount, containerName,
                DateTime.Now.AddHours(-24D).AddMinutes(DateTime.Now.Minute * -1D));
        }

        public void SetLastSatisfied(CloudStorageAccount storageAccount, string containerName, Assertion assertion)
        {
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            containerReference.FetchAttributes();
            DateTime epoch = assertion.Raw.Epoch;

            containerReference.Metadata[metadataKeyName] = epoch.ToString("g", DateTimeFormatInfo.InvariantInfo);
            containerReference.SetMetadata();
        }
    }
}
