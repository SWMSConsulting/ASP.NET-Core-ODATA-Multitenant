using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace AspNetCoreStart.MultiTenancy
{
    public interface ITenantProvider
    {
        IEnumerable<Tenant> GetAllTenants();
        Tenant GetCurrentTenant();
    }
}
