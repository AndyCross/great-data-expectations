﻿using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatExpectations.Core;
using GreatExpectations.Generation;
using Microsoft.WindowsAzure.Storage;

namespace GreatExpectations
{
    public class MissHaversham
    {
        private readonly EpochPeristence _epochPeristence;

        public MissHaversham() : this (new EpochPeristence())
        {
            
        }

        private MissHaversham(EpochPeristence epochPeristence)
        {
            _epochPeristence = epochPeristence;
        }

        internal Assertion[] AssertImpl(CloudStorageAccount storageAccount, string containerName, string dataSetPrefix, int minFileExpectation = 1, int maxFileExpectation = 1, string customVariableFormat = "", DateTime? forceStartDateTime = null, DateTime? forceEndDateTime = null)
        {
            // The use of EpochPersistence allows repeated and incremental expectation checking. Be sure to call EpochPersistence.SetLastSatisfied
            // note that this is optional in the workflow and instead arbitrary datetimes can be passed to expectationGenerator.GenerateExpectations
            DateTime lastExecutionEpoch, endDateTime;
            lastExecutionEpoch = forceStartDateTime.HasValue ? forceStartDateTime.Value : _epochPeristence.GetLastSatisfied(storageAccount, containerName);
            var nextExecutionEpoch = lastExecutionEpoch.AddHours(1D);
            endDateTime = forceEndDateTime.HasValue ? forceEndDateTime.Value : DateTime.Now;

            // Create an instance of the ExpectationGenerator; note that this is instance to allow for variant implementations
            // Not yet implemented, but planned for predicatable but not date-oriented generators and different temporal durations
            var expectationGenerator = new ExpectationGenerator();
            var expectations = expectationGenerator.GenerateExpectations(nextExecutionEpoch, endDateTime, dataSetPrefix, minFileExpectation, maxFileExpectation, customVariableFormat);

            // This is a blocking call that iterates over the IAmAnExpection[] and returns Assertions (Results alongside Expectations)
            var assertions = GreatExpectations.Assert.Expectations(expectations, storageAccount, containerName).ToArray();
            foreach (var assertion in assertions)
            {
                Console.WriteLine("{0}: {1}", assertion.Result, assertion.Message);
            }

            // If incremental expectations are required, set a satisfied bit on the storage container, allowing the next iterations to 
            // start from where this timepoint ended
            _epochPeristence.SetLastSatisfied(storageAccount, containerName,
                assertions.OrderByDescending(a => a.Raw.Epoch).First(a => a.Result == AssertionResult.Success));
            return assertions;
        }

        /// <summary>
        /// Generates a set of Assertions based on the storageAccount and container supplied; using default rulesets.
        /// </summary>
        /// <param name="storageAccount">The account that contains the data to expect</param>
        /// <param name="containerName">The container that contains the data to expect. Note that metadata will be added to this container UNLESS the forcestart/end datetimes are supplied.</param>
        /// <param name="dataSetPrefix">The prefix to the path of execution, i.e. path/to/files/[variable format]</param>
        /// <param name="minFileExpectation">Fail if less than this number of files are found</param>
        /// <param name="maxFileExpectation">Fail if more than this number of files are found</param>
        /// <param name="customVariableFormat">A format that allows for year, month, day, hour placeholders {0},{1},{2},{3} repectively to be placed into a string, appended to the datasetprefix in order to locate files. The default is &quot;year={0}/month={1}/day={2}/hour={3}&quot;</param>
        /// <param name="forceStartDateTime">A datetime to start asserting</param>
        /// <param name="forceEndDateTime">A datetime to cease asserting</param>
        /// <returns></returns>
        public static Assertion[] Assert(CloudStorageAccount storageAccount, string containerName, string dataSetPrefix,
            int minFileExpectation = 1, int maxFileExpectation = 1, string customVariableFormat = "",
            DateTime? forceStartDateTime = null, DateTime? forceEndDateTime = null)
        {
            var spinster = new MissHaversham();
            return spinster.AssertImpl(storageAccount, containerName, dataSetPrefix, minFileExpectation, maxFileExpectation,
                customVariableFormat, forceStartDateTime, forceEndDateTime);
        }
    }
}
