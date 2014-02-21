great-data-expectations
=======================

Great Data Expectations is a pro-active data discovery library built for Windows Azure Blob Storage. It allows the decription of Expectations which encapsulates a path or endpoint where a set of files should exist. When "Asserted" these Expectations are checked and a report returned to the caller. If these files do not exist an error or warning will be returned.

![Build Status with MyGet](https://www.myget.org/BuildSource/Badge/great-expectations?identifier=24e54932-fb0d-40f7-9ffc-a11aaea548f1)

=====
#Usage

```
PM> Install-Package GreatExpectations -Pre
```

Once installed, use the ExpectationGenerator and the Assert class to build and run your data expectations:

```csharp
// Create an instance of the ExpectationGenerator; note that this is instance to allow for variant implementations
var expectationGenerator = new ExpectationGenerator();
var expectations = expectationGenerator.GenerateExpectations(lastExecutionEpoch, endDateTime, dataSetPrefix, minFileExpectation, maxFileExpectation, customVariableFormat);

// This is a blocking call that iterates over the IAmAnExpection[] and returns Assertions (Results alongside Expectations)
var assertions = GreatExpectations.Assert.Expectations(expectations, storageAccount, containerName).ToArray();
foreach (var assertion in assertions)
{
    Console.WriteLine("{0}: {1}", assertion.Result, assertion.Message);
}
```

Alternatively, use the [MissHaversham](http://en.wikipedia.org/wiki/Miss_Havisham) class (*she's static and vengeful*).  
