using System;
using System.Collections.Generic;
using System.Linq;
using GreatExpectations.Core;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations
{
    public static class Assert{
        internal static IEnumerable<Assertion> Expectations(IEnumerable<IAmAnExpectation> expectations, CloudStorageAccount storageAccount, string containerName)
        {
            var container = storageAccount.CreateCloudBlobClient().GetContainerReference(containerName);
            if (!container.Exists())
            {
                throw new AssertionPreconditionFailedException(string.Format("The container {0} was not found", containerName));
            }
            var assertions = new List<Assertion>();

            foreach (var anExpectation in expectations)
            {
                var expectedVirtualPath = anExpectation.GetRelativeAddress();
                var target = container.GetDirectoryReference(expectedVirtualPath).ListBlobs().ToArray();

                if (target.Count() > anExpectation.Description.MaxFileExpectation ||
                    target.Count() < anExpectation.Description.MinFileExpectation)
                {
                    if (anExpectation.IsInDataIngressWindow())
                    {
                        var message = string.Format("The expectation passed with a warning; the target virtual path {0} does not yet exist but is within its data ingress window. There is {1} remaining. ", 
                            expectedVirtualPath, (anExpectation.Epoch + anExpectation.Description.DataIngressTimespan) - DateTime.Now);

                        assertions.Add(new Assertion(anExpectation, AssertionResult.Warning, message));
                    }
                    else
                    {
                        var message = string.Format(
                            "The expectation failed to be met: Expected between {0} and {1}, Actual: {2}. Target virtual path: {3}",
                            anExpectation.Description.MinFileExpectation, anExpectation.Description.MaxFileExpectation,
                            target.Count(), expectedVirtualPath);

                        assertions.Add(new Assertion(anExpectation, AssertionResult.Failure, message));
                    }
                }
                else
                {
                    var message = string.Format(
                        "The expectation was met: Expected between {0} and {1}, Actual: {2}. Target virtual path: {3}",
                        anExpectation.Description.MinFileExpectation, anExpectation.Description.MaxFileExpectation,
                        target.Count(), expectedVirtualPath);

                    assertions.Add(new Assertion(anExpectation, AssertionResult.Success, message));
                }
            }

            return assertions;
        }
    }
}