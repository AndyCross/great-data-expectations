using System.Collections.Generic;
using GreatExpectations.Core;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations
{
    internal interface IAssert
    {
        IEnumerable<Assertion> Expectations(IEnumerable<IAmAnExpectation> expectations,
            CloudStorageAccount storageAccount, string containerName);
    }
}