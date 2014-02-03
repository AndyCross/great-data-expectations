using System.Collections.Generic;

namespace GreatExpectations.Tenancy
{
    public interface ITenantConfig
    {
        string TenantId { get; set; }
        IEnumerable<ExpectationDescription> ExpectationList();
    }
}