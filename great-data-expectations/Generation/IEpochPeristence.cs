using System;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations.Generation
{
    internal interface IEpochPeristence
    {
        DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName, DateTime defaultDateTime, string satisfiedName);
        DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName, string satisfiedName);
        void SetLastSatisfied(CloudStorageAccount storageAccount, string containerName, Assertion assertion, string satisfiedName);
    }
}