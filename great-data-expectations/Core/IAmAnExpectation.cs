using System;
using GreatExpectations.Tenancy;

namespace GreatExpectations.Core
{
    public interface IAmAnExpectation
    {
        string GetRelativeAddress();
        bool IsInDataIngressWindow();
        DateTime Epoch { get; }
        ExpectationDescription Description { get; }
    }
}