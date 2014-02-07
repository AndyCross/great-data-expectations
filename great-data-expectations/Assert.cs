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
                            "The expectation failed to be met: Expected {0} min and {1} max, Actual: {2}. Target virtual path: {3}",
                            anExpectation.Description.MinFileExpectation, anExpectation.Description.MaxFileExpectation,
                            target.Count(), expectedVirtualPath);

                        assertions.Add(new Assertion(anExpectation, AssertionResult.Failure, message));
                    }
                }
                else
                {
                    assertions.Add(new Assertion(anExpectation, AssertionResult.Success, string.Empty));
                }
            }

            return assertions;
        }
    }

    public class Assertion
    {
        private IAmAnExpectation _raw;
        private readonly AssertionResult _result;
        private readonly string _message;

        public Assertion(IAmAnExpectation raw, AssertionResult result, string message)
        {
            _raw = raw;
            _result = result;
            _message = message;
        }

        public IAmAnExpectation Raw
        {
            get { return _raw; }
        }

        public AssertionResult Result
        {
            get { return _result; }
        }

        public string Message
        {
            get { return _message; }
        }
    }

    public enum AssertionResult
    {
        Success,
        Failure,
        Inconclusive,
        Warning
    }
}