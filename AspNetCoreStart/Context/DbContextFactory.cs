using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreStart.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetCoreStart.Context
{
    public class DbContextFactory : IDbContextFactory
    {
        public ApplicationDbContext CreateDbContext(Tenant tenant, IConfiguration configuration)
        {
            var options = new DbContextOptions<ApplicationDbContext>();
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>(options);
            return new ApplicationDbContext(dbContextOptionsBuilder.Options, configuration, tenant);
        }
    }
}
