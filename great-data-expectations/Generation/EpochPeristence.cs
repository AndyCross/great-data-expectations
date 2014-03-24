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
        private const string metadataKeyNameFormat = "misshaversham_{0}";
        public DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName, DateTime defaultDateTime, string satisfiedName)
        {
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            containerReference.FetchAttributes();

            if (containerReference.Metadata.ContainsKey(GetMetadataKeyName(satisfiedName)))
            {
                DateTime epoch;

                if (DateTime.TryParseExact(containerReference.Metadata[GetMetadataKeyName(satisfiedName)], "g",
                    DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None, out epoch))
                {
                    return epoch;
                }
            }

            return defaultDateTime;
        }

        public DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName, string satisfiedName)
        {
            return GetLastSatisfied(storageAccount, containerName,
                DateTime.Now.AddHours(-24D).AddMinutes(DateTime.Now.Minute * -1D), satisfiedName);
        }

        public void SetLastSatisfied(CloudStorageAccount storageAccount, string containerName, Assertion assertion, string satisfiedName)
        {
            var containerReference = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            containerReference.FetchAttributes();
            DateTime epoch = assertion.Raw.Epoch;

            containerReference.Metadata[GetMetadataKeyName(satisfiedName)] = epoch.ToString("g", DateTimeFormatInfo.InvariantInfo);
            containerReference.SetMetadata();
        }

        private string GetMetadataKeyName(string jobName)
        {
            return string.Format(metadataKeyNameFormat, jobName);
        }
    }
}
