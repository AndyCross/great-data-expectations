using GreatExpectations.Tenancy;

namespace GreatExpectations.Core
{
    public interface IAmAnExpectation
    {
        string GetRelativeAddress();
        bool IsInDataIngressWindow();
        ExpectationDescription Description { get; }
    }
}