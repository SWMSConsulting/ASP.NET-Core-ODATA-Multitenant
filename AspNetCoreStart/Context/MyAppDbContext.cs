using AspNetCoreStart.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model;

namespace AspNetCoreStart.Context
{
    public class MyAppDbContext : ApplicationDbContext
    {
        public MyAppDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, Tenant tenant) : base(options, configuration, tenant)
        {

        }

        public MyAppDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, ITenantProvider tenantProvider) : base(options, configuration, tenantProvider)
        {

        }

        public DbSet<Todo> Todos { get; set; }
    }
}
