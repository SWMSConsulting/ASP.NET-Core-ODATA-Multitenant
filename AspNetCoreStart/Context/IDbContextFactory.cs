using AspNetCoreStart.MultiTenancy;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreStart.Context
{
    public interface IDbContextFactory
    {
        ApplicationDbContext CreateDbContext(Tenant tenant, IConfiguration configuration);
    }
}