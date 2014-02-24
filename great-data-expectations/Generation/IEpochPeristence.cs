using System;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations.Generation
{
    internal interface IEpochPeristence
    {
        DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName, DateTime defaultDateTime);
        DateTime GetLastSatisfied(CloudStorageAccount storageAccount, string containerName);
        void SetLastSatisfied(CloudStorageAccount storageAccount, string containerName, Assertion assertion);
    }
}